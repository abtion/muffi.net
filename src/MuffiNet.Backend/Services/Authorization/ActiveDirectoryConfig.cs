namespace MuffiNet.Backend.Services.Authorization;

public class ActiveDirectoryConfig
{
    // See the README.md file in the project root for details about this configuration.

    // TODO will this work with IConfig<> ?
    //private ActiveDirectoryConfig()
    //{}

    #nullable disable

    public string AppID { get; set; }

    public string AppRegistrationID { get; set; }

    public string AppClientID { get; set; }

    public string AppClientSecret { get; set; }

    public string DirectoryTenantID { get; set; }

    public string AdminRoleID { get; set; }

    #nullable enable

    public string? AdminUserID { get; set; }
}
