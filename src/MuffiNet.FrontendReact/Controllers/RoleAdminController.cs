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
        public async Task<ActionResult<object>> GetData()
        {
            var roles = await userRoleService.ListAppRoles();
            var users = await userRoleService.ListUsers();

            return new
            {
                roles = roles.Select(role => new
                {
                    id = role.Id,
                    name = role.DisplayName,
                }),
                users = users
            };
        }

        [HttpPost("update-user")]
        public async Task<ActionResult<object>> UpdateUser([FromBody] User user)
        {
            await userRoleService.UpdateUser(user);

            return new OkResult();
        }

        [HttpGet("user-details")]
        public async Task<ActionResult<UserDetails>> GetUserDetails([FromQuery] string userID)
        {
            return await userRoleService.GetUserDetails(userID);
        }
    }
}
