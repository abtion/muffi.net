using Api.Shared.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Presentation.UserAdministration.Commands;

namespace Api.WithReact.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OidcController : ControllerBase
{
    private static bool AdministratorAppRoleRun = false;

    /// <summary>
    /// This constructor is called once per application start.
    /// It is used to ensure that the administrator app role is created.
    /// </summary>
    /// <remarks>
    /// Should be moved to another location, but the current location 
    /// is the only one that is called once per application start.
    /// </remarks>
    public OidcController([FromServices] AdministratorAppRoleAssignmentCommandHandler handler)
    {
        if (!AdministratorAppRoleRun)
        {
            try
            {
                handler.Handle(new(), CancellationToken.None).Wait();
                AdministratorAppRoleRun = true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    [HttpGet("frontend-configuration")]
    public ActionResult<OpenIdConnectFrontend> FrontendConfiguration([FromServices] IConfiguration configuration, CancellationToken cancellationToken)
    {
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