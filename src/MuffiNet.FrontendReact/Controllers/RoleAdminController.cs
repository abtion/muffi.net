using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MuffiNet.Backend.Services.Authorization;

namespace MuffiNet.FrontendReact.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrators")]
    public class RoleAdminController : ControllerBase
    {
        // TODO build out API controller for Role Administration

        private UserRoleService userRoleService;

        public RoleAdminController(UserRoleService userRoleService)
        {
            this.userRoleService = userRoleService;
        }

        [HttpGet("get-data")]
        public async Task<ActionResult<object>> Index()
        {
            var roles = await userRoleService.ListAppRoles();

            return new
            {
                roles = roles.Select(role => new
                {
                    id = role.Id,
                    name = role.DisplayName,
                })
            };
        }
    }
}
