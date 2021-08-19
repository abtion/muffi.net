using System.Threading.Tasks;

namespace MuffiNet.Backend.Services
{
    public class ExampleService : IExampleService
    {
        public async Task<string> PerformService(string string1, string string2, string string3)
        {
            var result = $"{string1}, {string2}, {string3}";

            return await Task.FromResult(result);
        }
    }
}