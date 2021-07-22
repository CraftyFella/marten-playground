using System;
using common;

namespace consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var documentStore = DocumentStoreFactory.Create();
            using var daemon = documentStore.BuildProjectionDaemon();
            daemon.StartAll();
            Console.ReadLine();
        }
    }
}