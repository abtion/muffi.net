using System;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.Net;

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