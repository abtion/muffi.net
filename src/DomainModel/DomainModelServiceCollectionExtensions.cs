using DomainModel.Services.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DomainModel;

public static class DomainModelServiceCollectionExtensions {
    public static IServiceCollection AddUserRoleService(this IServiceCollection services, IConfiguration config) {
        services.AddSingleton<UserRoleService>();

        services.AddOptions<ActiveDirectoryConfig>()
            .Configure<IConfiguration>((options, configuration) => {
                configuration.GetRequiredSection(nameof(ActiveDirectoryConfig)).Bind(options);
            });

        return services;
    }
}