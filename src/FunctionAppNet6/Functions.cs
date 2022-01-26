using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;

namespace FunctionAppNet6
{
    public static class Functions
    {
        [FunctionName(nameof(MyBlobFunction))]
        public static async Task MyBlobFunction(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [Blob("netconf/{query.id}", FileAccess.Write, Connection = "MyStorageConnection")] Stream blob,
            ILogger log)
        {
            await req.Body.CopyToAsync(blob);

            log.LogInformation($"Persisted blob {req.Query["id"]}.");
        }
    }
}
