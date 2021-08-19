using MuffiNet.Backend.Services;
using System.Threading.Tasks;

namespace MuffiNet.Test.Shared.Mocks
{
    public class ExampleServiceMock : IExampleService
    {
        public async Task<string> PerformService(string string1, string string2, string string3)
        {
            return await Task.FromResult("123456");
        }
    }
}