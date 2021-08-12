using Microsoft.Extensions.DependencyInjection;
using MuffiNet.Backend.DomainModel.Commands.CreateSupportTicket;
using MuffiNet.Backend.DomainModel.Commands.DeleteSupportTicket;
using MuffiNet.Backend.DomainModel.Commands.RequestOssIdFromOss;
using MuffiNet.Backend.DomainModel.Queries.EstimatedWaitingTime;
using MuffiNet.Backend.DomainModel.Queries.ReadSupportTicket;
using MuffiNet.Backend.DomainModel.Queries.ReadSupportTicketById;
using MuffiNet.Backend.DomainModel.Queries.ReadVideoGrantForCustomerToken;
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
            services.AddScoped<CreateSupportTicketHandler>();
            services.AddScoped<DeleteSupportTicketHandler>();
            services.AddScoped<RequestOssIdFromOssHandler>();

            // Query Handlers
            services.AddScoped<ReadSupportTicketHandler>();
            services.AddScoped<ReadSupportTicketByIdHandler>();
            services.AddScoped<ReadVideoGrantForCustomerTokenHandler>();
            services.AddScoped<EstimatedWaitingTimeHandler>();

            // Services
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddTransient<ICurrentDateTimeService, CurrentDateTimeService>();
            services.AddTransient<IExampleService, ExampleService>();

            return new DomainModelBuilder(services);
        }
    }
}