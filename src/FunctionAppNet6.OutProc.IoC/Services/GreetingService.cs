using FunctionAppNet6.OutProc.IoC.Models;
using System.Threading.Tasks;

namespace FunctionAppNet6.OutProc.IoC.Services
{
    public interface IGreetingService
    {
        Task<Greeting> GreetAsync(string name);
    }

    public class GreetingService : IGreetingService
    {
        public async Task<Greeting> GreetAsync(string name)
        {
            var greeting = new Greeting { Message = $"Hello, {name}!" };

            return await Task.FromResult(greeting).ConfigureAwait(false);
        }
    }
}
