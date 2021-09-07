using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using MuffiNet.Backend.Models;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MuffiNet.Backend.Services
{
    public interface ICurrentUserService
    {
        Task<IdentityUser> CurrentUser();
    }

    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor httpContext;
        private readonly UserManager<ApplicationUser> userManager;

        public CurrentUserService(IHttpContextAccessor httpContext, UserManager<ApplicationUser> userManager)
        {
            this.httpContext = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<IdentityUser> CurrentUser()
        {
            var userId = httpContext.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
            var user = await userManager.FindByIdAsync(userId?.Value);

            return user;
        }
    }
}