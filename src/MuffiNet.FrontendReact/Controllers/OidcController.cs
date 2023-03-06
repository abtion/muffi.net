using Microsoft.AspNetCore.Mvc;
using MuffiNet.Authentication.OpenIdConnect;

namespace MuffiNet.FrontendReact.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OidcController : ControllerBase {
    [HttpGet("frontend-configuration")]
    public ActionResult<OpenIdConnectFrontend> FrontendConfiguration([FromServices] IConfiguration configuration, CancellationToken cancellationToken) {
        return new OpenIdConnectFrontend(
            configuration.GetValue<string>("Authentication:Authority")
                ?? throw new OidcConfigurationInvalidException($"Missing configuration for Authentication:Authority"),
            configuration.GetValue<string>("Authentication:ClientId")
                ?? throw new OidcConfigurationInvalidException($"Missing configuration for Authentication:ClientId"),
            configuration.GetValue<string>("Authentication:FrontendScopes")
                ?? throw new OidcConfigurationInvalidException($"Missing configuration for Authentication:FrontendScopes")
            );
    }

    public record OpenIdConnectFrontend(string Authority, string ClientId, string Scopes);
}