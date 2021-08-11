using Moq;
using System;
using System.Threading.Tasks;
using MuffiNet.FrontendReact.Services;

namespace MuffiNet.FrontendReact.Test.Mocks
{
    public class ExampleServiceMock : IExampleService
    {
        public async Task<string> PerformService(string string1, string string2, string string3)
        {
            return await Task.FromResult("12, 34, 56");
        }
    }
}