using Microsoft.Extensions.DependencyInjection;
using Presentation.UserAdministration.Commands;
using Presentation.UserAdministration.Queries;

namespace Presentation.UserManagement;

public static class RegisterServices
{
    public static IServiceCollection AddUserManagementPresentation(this IServiceCollection services)
    {
        services.AddScoped<RevokeAllAccessCommandHandler>();
        services.AddScoped<UpdateUserCommandHandler>();
        services.AddScoped<LoadUserQueryHandler>();
        services.AddScoped<LoadUsersAndRolesQueryHandler>();
        services.AddScoped<AdministratorAppRoleAssignmentCommandHandler>();

        return services;
    }
}
