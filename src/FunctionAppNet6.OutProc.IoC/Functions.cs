using FunctionAppNet6.OutProc.IoC.Models;
using FunctionAppNet6.OutProc.IoC.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Net;
using System.Threading.Tasks;

namespace FunctionAppNet6.OutProc.IoC
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

        [Function(nameof(Net60OutProcIoCHttpTrigger))]
        public async Task<HttpResponseData> Net60OutProcIoCHttpTrigger([HttpTrigger(AuthorizationLevel.Function, "get", Route = "greetings")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("Net60HttpTrigger");
            logger.LogInformation("C# HTTP trigger function processed a request.");

            var queries = QueryHelpers.ParseNullableQuery(req.Url.Query);
            var name = queries["name"];

            var response = req.CreateResponse(HttpStatusCode.OK);

            var instance = await this.service.GreetAsync(name).ConfigureAwait(false);
            await response.WriteAsJsonAsync<Greeting>(instance).ConfigureAwait(false);

            return response;
        }
    }
}
