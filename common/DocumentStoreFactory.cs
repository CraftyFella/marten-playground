using Marten;

namespace common
{
    
    public static class DocumentStoreFactory
    {
        public static DocumentStore Create()
        {
            const string connectionString =
                "PORT = 5432; HOST = 127.0.0.1; TIMEOUT = 15; POOLING = True; MINPOOLSIZE = 1; MAXPOOLSIZE = 100; COMMANDTIMEOUT = 20; DATABASE = 'postgres'; PASSWORD = 'Password12!'; USER ID = 'postgres'";
            var store = DocumentStore.For(options =>
            {
                options.Connection(connectionString);
                options.AutoCreateSchemaObjects = AutoCreate.CreateOnly;
                options.DatabaseSchemaName = "EventStore";
                options.Events.DatabaseSchemaName = "EventStore";
                options.Events.AddEventType(typeof(AnEvent));
                options.Events.AsyncProjections.AggregateStreamsWith<Counter>();
            });
            return store;
        }
    }
}