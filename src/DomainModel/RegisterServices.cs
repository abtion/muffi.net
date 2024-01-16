using Domain.Example;
using Domain.UserAdministration;
using Microsoft.Extensions.DependencyInjection;

namespace Domain;

public static class RegisterServices
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddTransient<ICurrentDateTimeService, CurrentDateTimeService>();

        services.AddUserAdministration();
        services.AddExample();

        return services;
    }
}
