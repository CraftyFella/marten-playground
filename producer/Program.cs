using System;
using System.Linq;
using System.Threading.Tasks;
using common;
using Marten;
using Marten.Events.Daemon.Resiliency;
using Marten.Events.Projections;
using Weasel.Postgresql;

namespace producer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var stream = Guid.Parse("cb90cb3f-867f-4649-a991-5b84ab526bc1");
            const string connectionString =
                "PORT = 5432; HOST = 127.0.0.1; TIMEOUT = 15; POOLING = True; MINPOOLSIZE = 1; MAXPOOLSIZE = 100; COMMANDTIMEOUT = 20; DATABASE = 'postgres'; PASSWORD = 'Password12!'; USER ID = 'postgres'";
            
            var store = DocumentStore.For(options =>
            {
                options.Connection(connectionString);
                options.AutoCreateSchemaObjects = args.Any() ? AutoCreate.CreateOnly : AutoCreate.None;
                options.DatabaseSchemaName = "EventStore";
                options.Events.DatabaseSchemaName = "EventStore";
                options.Events.AddEventType(typeof(AnEvent));
            });
            var documentStore = (DocumentStore)store;
            var session = documentStore.OpenSession();
            var eventStore = session.Events;

            while (true)
            {
                var state = await eventStore.FetchStreamStateAsync(stream);
                var @event = new AnEvent("Description");
                eventStore.Append(stream, (state?.Version ?? 0) + 2, @event, @event);
                await session.SaveChangesAsync();
                Console.WriteLine("Produced 2 events. Press enter to send another 2");
                Console.ReadLine();
            }
        }
    }
}