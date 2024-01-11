using Domain.UserAdministration.Commands;
using Domain.UserAdministration.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.UserAdministration;

public static class RegisterServices
{
    public static IServiceCollection AddUserAdministration(this IServiceCollection services)
    {
        services.AddScoped<RevokeAllAccessCommandHandler>();
        services.AddScoped<UpdateUserCommandHandler>();
        services.AddScoped<LoadUserQueryHandler>();
        services.AddScoped<LoadUsersAndRolesQueryHandler>();
        services.AddScoped<AdministratorAppRoleAssignmentCommandHandler>();

        return services;
    }
}
