using System.Threading.Tasks;
using common;

namespace consumer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var documentStore = DocumentStoreFactory.Create();
            using var daemon = documentStore.BuildProjectionDaemon();
            daemon.StartAll();
            await daemon.WaitForNonStaleResults().ConfigureAwait(false);
            await daemon.StopAll().ConfigureAwait(false);
        }
    }
}