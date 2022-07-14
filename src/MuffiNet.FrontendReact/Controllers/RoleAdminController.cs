using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MuffiNet.FrontendReact.Controllers
{
    [Authorize("Administrator")]
    public class RoleAdminController : Controller
    {
        // TODO build out API controller for Role Administration

        public IActionResult Index()
        {
            return View();
        }
    }
}
