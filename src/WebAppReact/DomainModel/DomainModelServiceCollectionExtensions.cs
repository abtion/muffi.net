using Microsoft.Extensions.DependencyInjection;
using WebAppReact.DomainModel.Commands.CompleteRoom;
using WebAppReact.DomainModel.Commands.CreateRoom;
using WebAppReact.DomainModel.Commands.CreateSupportTicket;
using WebAppReact.DomainModel.Commands.DeleteSupportTicket;
using WebAppReact.DomainModel.Commands.RequestOssIdFromOss;
using WebAppReact.DomainModel.Queries.EstimatedWaitingTime;
using WebAppReact.DomainModel.Queries.ReadSupportTicket;
using WebAppReact.DomainModel.Queries.ReadSupportTicketById;
using WebAppReact.DomainModel.Queries.ReadVideoGrantForCustomerToken;
using WebAppReact.Hubs;
using WebAppReact.Services;

namespace WebAppReact.DomainModel
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
            services.AddScoped<CreateRoomHandler>();
            services.AddScoped<CompleteRoomHandler>();
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
            services.AddScoped<ITwilioService, TwilioService>();
            services.AddTransient<ICurrentDateTimeService, CurrentDateTimeService>();
            services.AddTransient<ICare1Service, Care1Service>();
            //services.AddScoped<UserManager<ApplicationUser>>();

            return new DomainModelBuilder(services);
        }
    }
}