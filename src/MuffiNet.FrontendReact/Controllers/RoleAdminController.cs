using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MuffiNet.FrontendReact.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrators")]
    public class RoleAdminController : ControllerBase
    {
        // TODO build out API controller for Role Administration

        [HttpGet]
        public ActionResult<string> Index()
        {
            return "LOL";
        }
    }
}
