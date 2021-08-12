using System.Threading.Tasks;

namespace MuffiNet.Backend.Services
{
    public interface IExampleService
    {
        Task<string> PerformService(string string1, string string2, string string3);

    }
}