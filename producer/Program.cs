using System;
using System.Linq;
using System.Threading.Tasks;
using common;

namespace producer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var stream = Guid.Parse("cb90cb3f-867f-4649-a991-5b84ab526bc1");
            var documentStore = DocumentStoreFactory.Create(args.Any());
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