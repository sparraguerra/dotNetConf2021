using FunctioFunctionAppNet6.InProc.Static.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;
using System.Threading.Tasks;

namespace FunctionAppNet6.InProc.Static
{
    public static class Functions
    {
        [OpenApiOperation(operationId: "greeting", tags: new[] { "greeting" }, Summary = "Greetings", Description = "This shows a welcome message.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter("name", Type = typeof(string), In = ParameterLocation.Query, Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Greeting), Summary = "The response", Description = "This returns the response")]

        [FunctionName(nameof(Net60InProcStaticHttpTrigger))]
        public static async Task<IActionResult> Net60InProcStaticHttpTrigger(
             [HttpTrigger(AuthorizationLevel.Function, "get", Route = "greetings")] HttpRequest req,
             ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];
            var message = $"Hello, {name}!";

            var instance = new Greeting() { Message = message };
            var result = new OkObjectResult(instance);

            return await Task.FromResult(result).ConfigureAwait(false);
        }
    }
}

