using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using MuffiNet.Backend.Services;

namespace MuffiNet.Test.Shared.Mocks
{
    public class CurrentUserServiceMock : ICurrentUserService
    {
        public async Task<IdentityUser> CurrentUser()
        {
            return await Task.FromResult(new IdentityUser()
            {
                Id = "e0169f6f-c521-4d75-9144-a46c692af355",
                UserName = "donald@duck.disney",
                Email = "donald@duck.disney"
            });
        }
    }
}