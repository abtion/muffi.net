using Microsoft.Extensions.DependencyInjection;
using MuffiNet.FrontendReact.DomainModel.Commands.CreateSupportTicket;
using MuffiNet.FrontendReact.DomainModel.Commands.DeleteSupportTicket;
using MuffiNet.FrontendReact.DomainModel.Commands.RequestOssIdFromOss;
using MuffiNet.FrontendReact.DomainModel.Queries.EstimatedWaitingTime;
using MuffiNet.FrontendReact.DomainModel.Queries.ReadSupportTicket;
using MuffiNet.FrontendReact.DomainModel.Queries.ReadSupportTicketById;
using MuffiNet.FrontendReact.DomainModel.Queries.ReadVideoGrantForCustomerToken;
using MuffiNet.FrontendReact.Hubs;
using MuffiNet.FrontendReact.Services;

namespace MuffiNet.FrontendReact.DomainModel
{
    public static class DomainModelServiceCollectionExtensions
    {
        public static IDomainModelBuilder AddDomainModel(this IServiceCollection services)
        {
            // Setup
            services.AddScoped<DomainModelTransaction>();

            // Command Handlers
            services.AddScoped<CreateSupportTicketHandler>();
            services.AddScoped<DeleteSupportTicketHandler>();
            services.AddScoped<RequestOssIdFromOssHandler>();

            // Query Handlers
            services.AddScoped<ReadSupportTicketHandler>();
            services.AddScoped<ReadSupportTicketByIdHandler>();
            services.AddScoped<ReadVideoGrantForCustomerTokenHandler>();
            services.AddScoped<EstimatedWaitingTimeHandler>();

            // SignalR
            services.AddTransient<CustomerHub>();
            services.AddTransient<TechnicianHub>();

            // Services
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddTransient<ICurrentDateTimeService, CurrentDateTimeService>();
            services.AddTransient<IExampleService, ExampleService>();

            return new DomainModelBuilder(services);
        }
    }
}