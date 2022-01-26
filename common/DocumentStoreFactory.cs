using Marten;
using Marten.Events.Daemon.Resiliency;
using Marten.Events.Projections;
using Weasel.Postgresql;

namespace common
{
    
    public static class DocumentStoreFactory
    {
        public static DocumentStore Create(bool createSchema)
        {
            const string connectionString =
                "PORT = 5432; HOST = 127.0.0.1; TIMEOUT = 15; POOLING = True; MINPOOLSIZE = 1; MAXPOOLSIZE = 100; COMMANDTIMEOUT = 20; DATABASE = 'postgres'; PASSWORD = 'Password12!'; USER ID = 'postgres'";
            var store = DocumentStore.For(options =>
            {
                options.Connection(connectionString);
                options.AutoCreateSchemaObjects = createSchema ? AutoCreate.CreateOnly : AutoCreate.None;
                options.DatabaseSchemaName = "EventStore";
                options.Events.DatabaseSchemaName = "EventStore";
                options.Events.AddEventType(typeof(AnEvent));
                options.Projections.AsyncMode = DaemonMode.Solo;
                options.Projections.Add<CounterProjection>(ProjectionLifecycle.Async);
                //options.Events.AsyncProjections.AggregateStreamsWith<Counter>();
            });
            return store;
        }
    }
}