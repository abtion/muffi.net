using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MuffiNet.Backend.DomainModel.Commands.ExampleCreateCommand;
using MuffiNet.Backend.DomainModel.Commands.ExampleDeleteCommand;
using MuffiNet.Backend.DomainModel.Commands.ExampleUpdateCommand;
using MuffiNet.Backend.DomainModel.Queries.ExampleQuery;
using MuffiNet.Backend.DomainModel.Queries.ExampleQueryAll;
using MuffiNet.Backend.Services;
using MuffiNet.Backend.Services.Authorization;

namespace MuffiNet.Backend.DomainModel;

public static class DomainModelServiceCollectionExtensions {
    public static IDomainModelBuilder AddDomainModel(this IServiceCollection services) {
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

    public static IServiceCollection AddUserRoleService(this IServiceCollection services, IConfiguration config) {
        services.AddSingleton<UserRoleService>();

        services.Configure<ActiveDirectoryConfig>(config.GetRequiredSection(nameof(ActiveDirectoryConfig)));

        return services;
    }
}
