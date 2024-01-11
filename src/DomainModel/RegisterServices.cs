using Domain.Example;
using Domain.UserAdministration;
using Domain.UserAdministration.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Domain;

public static class RegisterServices
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddTransient<ICurrentDateTimeService, CurrentDateTimeService>();

        services.AddUserAdministration();
        services.AddExample();

        // TODO Should be moved to infrastructure
        services.AddTransient<IConfiguredGraphServiceClient, ConfiguredGraphServiceClient>();
        services.AddScoped<IGetAppRolesFromAzureIdentity, GetAppRolesFromAzureIdentity>();
        services.AddScoped<IGetAppRoleAssignmentsFromAzureIdentity, GetAppRoleAssignmentsFromAzureIdentity>();
        services.AddScoped<IGetUserAppRoleAssignmentsFromAzureIdentity, GetUserAppRoleAssignmentsFromAzureIdentity>();
        services.AddScoped<IGetUserFromAzureIdentity, GetUserFromAzureIdentity>();
        services.AddScoped<IAddUserAppRoleAssignmentToAzureIdentity, AddUserAppRoleAssignmentToAzureIdentity>();
        services.AddScoped<IDeleteUserAppRoleAssignmentFromAzureIdentity, DeleteUserAppRoleAssignmentFromAzureIdentity>();
        services.AddScoped<IUpdateUserInAzureIdentity, UpdateUserInAzureIdentity>();

        services
            .AddOptions<AzureIdentityAdministrationOptions>()
            .Configure<IConfiguration>(
                (options, configuration) =>
                {
                    configuration
                        .GetRequiredSection(AzureIdentityAdministrationOptions.OptionsName)
                        .Bind(options);
                }
            );

        return services;
    }
}
