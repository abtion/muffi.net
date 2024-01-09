using Domain.Example.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace Presentation;

public static class RegisterServices
{
    public static IServiceCollection AddPresentation(this IServiceCollection services) 
    {
        services.AddScoped<ExampleLoadAllQueryHandler>();
        services.AddScoped<ExampleLoadSingleQueryHandler>();

        return services;
    }
}
