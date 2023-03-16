using Api.WithReact.Hubs;
using Api.WithReact.Hubs.Example;
using DomainModel.Data;
using DomainModel.Example.Notifications;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.WithReact
{
    public static class EntryPoint
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

        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()
                )
            );

            return services;
        }
    }
}