using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DomainModel.Services;
using DomainModel.Services.Authorization;
using DomainModel.Queries.ExampleQuery;
using DomainModel.Queries.ExampleQueryAll;
using DomainModel.Commands.ExampleCreateCommand;
using DomainModel.Commands.ExampleDeleteCommand;
using DomainModel.Commands.ExampleUpdateCommand;

namespace DomainModel;

public static class DomainModelServiceCollectionExtensions
{
    public static IDomainModelBuilder AddDomainModel(this IServiceCollection services)
    {
        // Setup
        services.AddScoped<DomainModelTransaction>();

        // Command Handlers
        services.AddScoped<ExampleCreateCommandHandler>();
        services.AddScoped<ExampleUpdateCommandHandler>();
        services.AddScoped<ExampleDeleteCommandHandler>();

        // Query Handlers
        services.AddScoped<ExampleQueryHandler>();
        services.AddScoped<ExampleQueryAllHandler>();

        // Services
        //services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddTransient<ICurrentDateTimeService, CurrentDateTimeService>();
        services.AddTransient<IExampleReverseStringService, ExampleReverseStringService>();

        return new DomainModelBuilder(services);
    }

    public static IServiceCollection AddUserRoleService(this IServiceCollection services, IConfiguration config)
    {
        services.AddSingleton<UserRoleService>();

        services.Configure<ActiveDirectoryConfig>(config.GetRequiredSection(nameof(ActiveDirectoryConfig)));

        return services;
    }
}
