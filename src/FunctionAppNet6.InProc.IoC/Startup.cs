using FunctionAppNet6.InProc.IoC.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(FunctionAppNet6.InProc.IoC.Startup))]
namespace FunctionAppNet6.InProc.IoC
{
    public class Startup : FunctionsStartup
    { 
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddTransient<IGreetingService, GreetingService>();
        }
    }
}
