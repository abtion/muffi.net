using System.Threading.Tasks;

namespace MuffiNet.FrontendReact.Services
{
    public interface IExampleService
    {
        Task<string> PerformService(string string1, string string2, string string3);

    }
}