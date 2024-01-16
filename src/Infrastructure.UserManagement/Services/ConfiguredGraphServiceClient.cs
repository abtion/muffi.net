using Azure.Identity;
using Microsoft.Extensions.Options;
using Microsoft.Graph;

namespace Domain.UserAdministration.Services;

public interface IConfiguredGraphServiceClient
{
    public GraphServiceClient Client { get; }

    public AzureIdentityAdministrationOptions Options { get; }
}

public class ConfiguredGraphServiceClient : IConfiguredGraphServiceClient
{
    public ConfiguredGraphServiceClient(IOptions<AzureIdentityAdministrationOptions> options)
    {
        Options = options.Value;

        string[] scopes = ["https://graph.microsoft.com/.default"];

        var clientSecretCredential = new ClientSecretCredential(
            Options.DirectoryTenantId,
            Options.ClientId,
            Options.ClientSecret
        );

        Client = new GraphServiceClient(clientSecretCredential, scopes);
    }

    public GraphServiceClient Client { get; }

    public AzureIdentityAdministrationOptions Options { get; }
}