using DomainModel.Services.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DomainModel;

public static class DomainModelServiceCollectionExtensions
{
    //public static IServiceCollection AddDomainModel(this IServiceCollection services)
    //{
    //    // Setup
    //    services.AddScoped<DomainModelTransaction>();

    //    // Command Handlers
    //    services.AddScoped<ExampleCreateCommandHandler>();
    //    services.AddScoped<ExampleUpdateCommandHandler>();
    //    services.AddScoped<ExampleDeleteCommandHandler>();

    //    // Query Handlers
    //    services.AddScoped<ExampleLoadSingleQueryHandler>();
    //    services.AddScoped<ExampleLoadAllQueryHandler>();

    //    // Services
    //    //services.AddScoped<ICurrentUserService, CurrentUserService>();
    //    services.AddTransient<ICurrentDateTimeService, CurrentDateTimeService>();
    //    services.AddTransient<IExampleReverseStringService, ExampleReverseStringService>();

    //    return services;
    //}

    public static IServiceCollection AddUserRoleService(this IServiceCollection services, IConfiguration config)
    {
        services.AddSingleton<UserRoleService>();

        services.AddOptions<ActiveDirectoryConfig>()
            .Configure<IConfiguration>((options, configuration) =>
            {
                configuration.GetRequiredSection(nameof(ActiveDirectoryConfig)).Bind(options);
            });

        return services;
    }
}
