using Microsoft.AspNetCore.Mvc;

namespace MuffiNet.FrontendReact.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OidcController : ControllerBase
{
    [HttpGet("frontend-configuration")]
    public ActionResult<OpenIdConnectFrontend> FrontendConfiguration([FromServices] IConfiguration configuration, CancellationToken cancellationToken)
    {
        return new OpenIdConnectFrontend()
        {
            Authority = configuration.GetValue<string>("Authentication:Authority"),
            ClientId = configuration.GetValue<string>("Authentication:ClientId"),
            Scopes = configuration.GetValue<string>("Authentication:FrontendScopes"),
        };
    }

    public record OpenIdConnectFrontend
    {
        public string Authority { get; set; } = default!;

        public string ClientId { get; set; } = default!;

        public string Scopes { get; set; } = default!;
    }
}