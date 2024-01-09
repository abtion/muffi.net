using Api.WithReact.Hubs;
using Api.WithReact.Hubs.Example;
using Domain.Example.Notifications;
using MediatR;

namespace Api.WithReact;

public static class RegisterServices
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        // controller classes are not added to the IoC container by default
        services.AddControllers();

        services.AddScoped<INotificationHandler<ExampleCreatedNotification>, ExampleCreatedNotificationHandler>();
        services.AddScoped<INotificationHandler<ExampleUpdatedNotification>, ExampleUpdatedNotificationHandler>();
        services.AddScoped<INotificationHandler<ExampleDeletedNotification>, ExampleDeletedNotificationHandler>();

        services.AddScoped<IExampleHubContract, ExampleHub>();

        return services;
    }
}