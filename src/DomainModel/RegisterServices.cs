using Domain.UserAdministration;
using Domain.UserAdministration.Commands;
using Domain.UserAdministration.Queries;
using Domain.UserAdministration.Services;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Domain;

public static class RegisterServices
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddTransient<ICurrentDateTimeService, CurrentDateTimeService>();

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(RegisterServices).Assembly);
            //cfg.RegisterServicesFromAssemblyContaining(typeof(ICommandHandler<,>));
            // cfg.AddBehavior < IPipelineBehavior <, >, ValidationBehavior <, >> ();
        });

        services.AddScoped<RevokeAllAccessCommandHandler>();
        services.AddScoped<UpdateUserCommandHandler>();
        services.AddScoped<LoadUserQueryHandler>();
        services.AddScoped<LoadUsersAndRolesQueryHandler>();
        services.AddScoped<AdministratorAppRoleAssignmentCommandHandler>();

        services.AddTransient<IConfiguredGraphServiceClient, ConfiguredGraphServiceClient>();
        services.AddScoped<IGetAppRolesFromAzureIdentity, GetAppRolesFromAzureIdentity>();
        services.AddScoped<
            IGetAppRoleAssignmentsFromAzureIdentity,
            GetAppRoleAssignmentsFromAzureIdentity
        >();
        services.AddScoped<
            IGetUserAppRoleAssignmentsFromAzureIdentity,
            GetUserAppRoleAssignmentsFromAzureIdentity
        >();
        services.AddScoped<IGetUserFromAzureIdentity, GetUserFromAzureIdentity>();
        services.AddScoped<
            IAddUserAppRoleAssignmentToAzureIdentity,
            AddUserAppRoleAssignmentToAzureIdentity
        >();
        services.AddScoped<
            IDeleteUserAppRoleAssignmentFromAzureIdentity,
            DeleteUserAppRoleAssignmentFromAzureIdentity
        >();
        services.AddScoped<IUpdateUserInAzureIdentity, UpdateUserInAzureIdentity>();

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.AddValidatorsFromAssembly(typeof(RegisterServices).Assembly);

        // https://code-maze.com/cqrs-mediatr-fluentvalidation/
        // services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

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
