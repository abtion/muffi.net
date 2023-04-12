namespace DomainModel.UserAdministration;

public class AzureIdentityAdministrationOptions
{
    public const string OptionsName = "AzureIdentityAdministration";
    // See the README.md file in the project root for details about this configuration.

#nullable disable

    public string EnterpriseApplicationObjectId { get; set; }

    public string AppRegistrationObjectId { get; set; }

    public string ClientId { get; set; }

    public string ClientSecret { get; set; }

    public string DirectoryTenantId { get; set; }

    public string AllUsersAppRoleId { get; set; }

    public string AdministratorsAppRoleId { get; set; }

#nullable enable

    public string? AdministratorUserId { get; set; }
}
