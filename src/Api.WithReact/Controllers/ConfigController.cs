using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.WithReact.Controllers;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class ConfigController : ControllerBase {
    [HttpGet("application-insights-connection-string")]
    public IActionResult ApplicationInsightsConnectionString([FromServices] IConfiguration configuration) {
        var connectionString = configuration.GetValue<string>("ApplicationInsights:ConnectionString");

        return Ok(connectionString);
    }
}
