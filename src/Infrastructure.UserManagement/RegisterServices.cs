using Domain.UserAdministration;
using Domain.UserAdministration.Repositories;
using Domain.UserAdministration.Services;
using Infrastructure.UserManagement.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Presentation.UserManagement.Repositories;

namespace Presentation.UserManagement;

public static class RegisterServices
{
    public static IServiceCollection AddUserManagementInfrastructure(this IServiceCollection services)
    {
        // Repositories
        services.AddScoped<IAssignAppRoleToUser, AppRoleAssignmentRepository>();
        services.AddScoped<IRemoveAppRoleFromUser, AppRoleAssignmentRepository>();

        services.AddScoped<IUpdateUserDetails, UserRepository>();
        services.AddScoped<IGetUserAppRoleAssignments, UserRepository>();
        services.AddScoped<IGetUserDetails, UserRepository>();
        
        // Services
        services.AddTransient<IConfiguredGraphServiceClient, ConfiguredGraphServiceClient>();
        services.AddScoped<IGetAppRolesFromAzureIdentity, GetAppRolesFromAzureIdentity>();
        services.AddScoped<IGetAppRoleAssignmentsFromAzureIdentity, GetAppRoleAssignmentsFromAzureIdentity>();
        services.AddScoped<IGetUserFromAzureIdentity, GetUserFromAzureIdentity>();

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
