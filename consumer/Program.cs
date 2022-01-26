using System;
using System.Linq;
using System.Threading.Tasks;
using common;

namespace consumer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var documentStore = DocumentStoreFactory.Create(args.Any());
            using var daemon = documentStore.BuildProjectionDaemon();
            await daemon.StartAllShards();
            Console.ReadLine();
        }
    }
}