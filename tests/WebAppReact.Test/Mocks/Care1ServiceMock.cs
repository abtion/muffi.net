using Moq;
using System;
using System.Threading.Tasks;
using WebAppReact.Services;

namespace WebAppReact.Test.Mocks
{
    public class Care1ServiceMock : ICare1Service
    {
        public async Task<string> CreateLinkToOss(string supportTicketId, string customerName, string technicianInitials)
        {
            return await Task.FromResult("123456");
        }
    }
}