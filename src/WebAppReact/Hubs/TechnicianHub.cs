using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using static WebAppReact.DomainModel.Queries.ReadSupportTicket.ReadSupportTicketResponse;

namespace WebAppReact.Hubs
{
    public interface ITechnicianHubContract
    {
        Task SupportTicketUpdated(SupportTicketUpdatedMessage message);
        Task SupportTicketCreated(SupportTicketCreatedMessage message);
        Task SupportTicketDeleted(SupportTicketDeletedMessage message);
    }

    [Authorize]
    public class TechnicianHub : Hub
    {
        private readonly IHubContext<TechnicianHub> context;

        public TechnicianHub(IHubContext<TechnicianHub> context)
        {
            this.context = context;
        }

        public async virtual Task SupportTicketUpdated(SupportTicketUpdatedMessage message)
        {
            await context.Clients.All.SendAsync(nameof(SupportTicketUpdated), message);
        }

        public async virtual Task SupportTicketCreated(SupportTicketCreatedMessage message)
        {
            await context.Clients.All.SendAsync(nameof(SupportTicketCreated), message);
        }

        public async virtual Task SupportTicketDeleted(SupportTicketDeletedMessage message)
        {
            await context.Clients.All.SendAsync(nameof(SupportTicketDeleted), message);
        }
    }

    public class SupportTicketCreatedMessage
    {
        public SupportTicketCreatedMessage(SupportTicketRecord supportTicket)
        {
            SupportTicket = supportTicket;
        }

        public SupportTicketRecord SupportTicket { get; private set; }
    }

    public class SupportTicketUpdatedMessage
    {
        public SupportTicketUpdatedMessage(SupportTicketRecord supportTicket)
        {
            SupportTicket = supportTicket;
        }

        public SupportTicketRecord SupportTicket { get; private set; }
    }

    public class SupportTicketDeletedMessage
    {
        public SupportTicketDeletedMessage(string supportTicketId)
        {
            SupportTicketId = supportTicketId;
        }

        public string SupportTicketId { get; private set; }
    }
}