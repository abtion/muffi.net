using Microsoft.Extensions.DependencyInjection;
using MuffiNet.Backend.DomainModel.Commands.ExampleCreateCommand;
using MuffiNet.Backend.DomainModel.Commands.ExampleDeleteCommand;
using MuffiNet.Backend.DomainModel.Queries.ExampleQuery;

using MuffiNet.Backend.Services;

namespace MuffiNet.Backend.DomainModel
{
    public static class DomainModelServiceCollectionExtensions
    {
        public static IDomainModelBuilder AddDomainModel(this IServiceCollection services)
        {
            // Setup
            services.AddScoped<DomainModelTransaction>();

            // Command Handlers
            services.AddScoped<ExampleCreateCommandHandler>();
            services.AddScoped<ExampleDeleteCommandHandler>();

            // Query Handlers
            services.AddScoped<ExampleQueryHandler>();

            // Services
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddTransient<ICurrentDateTimeService, CurrentDateTimeService>();
            services.AddTransient<IExampleService, ExampleService>();

            return new DomainModelBuilder(services);
        }
    }
}