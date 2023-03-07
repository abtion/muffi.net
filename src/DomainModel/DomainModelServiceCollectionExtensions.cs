using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DomainModel.Services;
using DomainModel.Services.Authorization;
using DomainModel.Commands;
using DomainModel.Queries;

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
        services.AddScoped<ExampleLoadSingleQueryHandler>();
        services.AddScoped<ExampleLoadAllQueryHandler>();

        // Services
        //services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddTransient<ICurrentDateTimeService, CurrentDateTimeService>();
        services.AddTransient<IExampleReverseStringService, ExampleReverseStringService>();

        return new DomainModelBuilder(services);
    }

    public static IServiceCollection AddUserRoleService(this IServiceCollection services, IConfiguration config)
    {
        services.AddSingleton<UserRoleService>();

        services.AddOptions<ActiveDirectoryConfig>()
            .Configure<IConfiguration>((options, configuration) => {
                configuration.GetRequiredSection(nameof(ActiveDirectoryConfig)).Bind(options);
            });

        return services;
    }
}
