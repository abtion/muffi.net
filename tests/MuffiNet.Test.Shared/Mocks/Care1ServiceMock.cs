using Moq;
using System;
using System.Threading.Tasks;
using MuffiNet.FrontendReact.Services;

namespace MuffiNet.Test.Shared.Mocks
{
    public class Care1ServiceMock : ICare1Service
    {
        public async Task<string> CreateLinkToOss(string supportTicketId, string customerName, string technicianInitials)
        {
            return await Task.FromResult("123456");
        }
    }
}