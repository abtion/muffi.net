using DomainModel.Example.Commands;
using DomainModel.Example.Queries;
using DomainModel.Example.Services;
using DomainModel.Shared;
using DomainModel.UserAdministration;
using DomainModel.UserAdministration.Commands;
using DomainModel.UserAdministration.Queries;
using DomainModel.UserAdministration.Services;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace DomainModel;

public static class EntryPoint
{
    public static IServiceCollection AddDomainModel(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<DomainModelTransaction>();
        services.AddTransient<ICurrentDateTimeService, CurrentDateTimeService>();
        services.AddTransient<IExampleReverseStringService, ExampleReverseStringService>();

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(EntryPoint).Assembly);
            //cfg.RegisterServicesFromAssemblyContaining(typeof(ICommandHandler<,>));
            // cfg.AddBehavior < IPipelineBehavior <, >, ValidationBehavior <, >> ();
        });

        // keep these lines until mediator.Send([ICommand]) is used in all controllers and tests
        services.AddScoped<ExampleLoadAllQueryHandler>();
        services.AddScoped<ExampleLoadSingleQueryHandler>();
        services.AddScoped<ExampleCreateCommandHandler>();
        services.AddScoped<ExampleUpdateCommandHandler>();
        services.AddScoped<ExampleDeleteCommandHandler>();


        services.AddScoped<RevokeAllAccessCommandHandler>();
        services.AddScoped<UpdateUserCommandHandler>();
        services.AddScoped<LoadUserQueryHandler>();
        services.AddScoped<LoadUsersAndRolesQueryHandler>();
        services.AddScoped<AdministratorAppRoleAssignmentCommandHandler>();
        
        services.AddTransient<IConfiguredGraphServiceClient, ConfiguredGraphServiceClient>();
        services.AddScoped<IGetAppRolesFromAzureIdentity, GetAppRolesFromAzureIdentity>();
        services.AddScoped<IGetAppRoleAssignmentsFromAzureIdentity, GetAppRoleAssignmentsFromAzureIdentity>();
        services.AddScoped<IGetUserAppRoleAssignmentsFromAzureIdentity, GetUserAppRoleAssignmentsFromAzureIdentity>();
        services.AddScoped<IGetUserFromAzureIdentity, GetUserFromAzureIdentity>();
        services.AddScoped<IAddUserAppRoleAssignmentToAzureIdentity, AddUserAppRoleAssignmentToAzureIdentity>();
        services.AddScoped<IDeleteUserAppRoleAssignmentFromAzureIdentity, DeleteUserAppRoleAssignmentFromAzureIdentity>();
        services.AddScoped<IUpdateUserInAzureIdentity, UpdateUserInAzureIdentity>();




        services.AddScoped<AbstractValidator<ExampleCreateCommand>, ExampleCreateCommandValidator>();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.AddValidatorsFromAssembly(typeof(EntryPoint).Assembly);

        // https://code-maze.com/cqrs-mediatr-fluentvalidation/
        // services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.AddOptions<AzureIdentityAdministrationOptions>()
            .Configure<IConfiguration>((options, configuration) =>
            {
                configuration.GetRequiredSection(AzureIdentityAdministrationOptions.OptionsName).Bind(options);
            });

        return services;
    }
}