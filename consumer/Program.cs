using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using common;
using Marten;
using Marten.Events;
using Marten.Events.Daemon.Resiliency;
using Marten.Events.Projections;
using Weasel.Postgresql;

namespace consumer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            const string connectionString =
                "PORT = 5432; HOST = 127.0.0.1; TIMEOUT = 15; POOLING = True; MINPOOLSIZE = 1; MAXPOOLSIZE = 100; COMMANDTIMEOUT = 20; DATABASE = 'postgres'; PASSWORD = 'Password12!'; USER ID = 'postgres'";
            var store = DocumentStore.For(options =>
            {
                options.Connection(connectionString);
                options.AutoCreateSchemaObjects = args.Any() ? AutoCreate.CreateOnly : AutoCreate.None;
                options.DatabaseSchemaName = "EventStore";
                options.Events.DatabaseSchemaName = "EventStore";
                options.Events.AddEventType(typeof(AnEvent));
                options.Projections.AsyncMode = DaemonMode.HotCold;
                options.Projections.Add(new MartenSubscription(new MartenEventsConsumer()), ProjectionLifecycle.Async, "customConsumer");

                //options.Events.AsyncProjections.AggregateStreamsWith<Counter>();
            });
            
            var documentStore = (DocumentStore)store;
            using var daemon = documentStore.BuildProjectionDaemon();
            await daemon.StartAllShards();
            Console.ReadLine();
        }
    }
    
    
    public class MartenSubscription: IProjection
    {
        private readonly IMartenEventsConsumer _consumer;

        public MartenSubscription(IMartenEventsConsumer consumer)
        {
            this._consumer = consumer;
        }

        public void Apply(
            IDocumentOperations operations,
            IReadOnlyList<StreamAction> streams
        )
        {
            throw new NotSupportedException("Subscription should be only run asynchronously");
        }

        public Task ApplyAsync(
            IDocumentOperations operations,
            IReadOnlyList<StreamAction> streams,
            CancellationToken ct
        )
        {
            return _consumer.ConsumeAsync(streams);
        }
    }
    
    public interface IMartenEventsConsumer
    {
        Task ConsumeAsync(IReadOnlyList<StreamAction> streamActions);
    }
    
    public class MartenEventsConsumer: IMartenEventsConsumer
    {
        public static List<object> Events { get; } = new();

        public Task ConsumeAsync(IReadOnlyList<StreamAction> streamActions)
        {
            foreach (var @event in streamActions.SelectMany(streamAction => streamAction.Events))
            {
                Events.Add(@event);
                Console.WriteLine($"{@event.Sequence} - {@event.EventTypeName}");
            }

            return Task.CompletedTask;
        }
    }

}