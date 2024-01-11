using Domain.Example.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.Example;

public static class RegisterServices
{
    public static IServiceCollection AddExample(this IServiceCollection services)
    {
        services.AddScoped<CreateExampleUseCase>();
        services.AddScoped<UpdateExampleUseCase>();
        services.AddScoped<DeleteExampleUseCase>();

        return services;
    }
}
