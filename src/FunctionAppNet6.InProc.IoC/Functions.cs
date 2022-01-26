using FunctionAppNet6.InProc.IoC.Models;
using FunctionAppNet6.InProc.IoC.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Net;
using System.Threading.Tasks;

namespace FunctionAppNet6.InProc.IoC
{
    public class Functions
    {
        private readonly IGreetingService service;

        public Functions(IGreetingService service)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [OpenApiOperation(operationId: "greeting", tags: new[] { "greeting" }, Summary = "Greetings", Description = "This shows a welcome message.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter("name", Type = typeof(string), In = ParameterLocation.Query, Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Greeting), Summary = "The response", Description = "This returns the response")]

        [FunctionName(nameof(Net60InProcIoCHttpTrigger))]
        public async Task<IActionResult> Net60InProcIoCHttpTrigger(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "greetings")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];
            var instance = await this.service.GreetAsync(name).ConfigureAwait(false);

            return new OkObjectResult(instance);
        }
    }
}

