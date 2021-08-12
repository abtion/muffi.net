﻿using Microsoft.Extensions.DependencyInjection;
using MuffiNet.Backend.DomainModel.Commands.ExampleCommand;
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
            services.AddScoped<ExampleCommandHandler>();

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