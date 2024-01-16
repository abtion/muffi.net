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
        services.AddScoped<IGetAppRoleAssignmentForUser, AppRoleAssignmentRepository>();
        services.AddScoped<IGetAppRoleAssigment, AppRoleAssignmentRepository>();

        services.AddScoped<IGetAppRoles, AppRoleRepository>();

        services.AddScoped<IUpdateUserDetails, UserRepository>();
        services.AddScoped<IGetUserAppRoleAssignments, UserRepository>();
        services.AddScoped<IGetUserDetails, UserRepository>();
        
        // Configuration of Azure Identity API
        services.AddTransient<IConfiguredGraphServiceClient, ConfiguredGraphServiceClient>();

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
