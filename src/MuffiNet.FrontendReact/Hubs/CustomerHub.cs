using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace MuffiNet.FrontendReact.Hubs
{
    public interface ICustomerHubContract
    {
        Task JoinGroup(string supportTicketId);
        Task TechnicianHasEndedCall(TechnicianHasEndedCallMessage message);
        Task TechnicianHasStartedCall(TechnicianHasStartedCallMessage message);
    }

    public class CustomerHub : Hub
    {
        private readonly IHubContext<CustomerHub> context;

        public CustomerHub(IHubContext<CustomerHub> context)
        {
            this.context = context;
        }

        public async virtual Task JoinGroup(string supportTicketId)
        {
            await context.Groups.AddToGroupAsync(Context.ConnectionId, supportTicketId);
        }

        public async virtual Task TechnicianHasEndedCall(TechnicianHasEndedCallMessage message)
        {
            await context.Clients.Group(message.SupportTicketId).SendAsync(nameof(TechnicianHasEndedCall), message);
        }

        public async virtual Task TechnicianHasStartedCall(TechnicianHasStartedCallMessage message)
        {
            await context.Clients.Group(message.SupportTicketId).SendAsync(nameof(TechnicianHasStartedCall).ToLower(), message);
        }

        public async virtual Task TechnicianHasCreatedOssCase(TechnicianHasCreatedOssCaseMessage message)
        {
            await context.Clients.Group(message.SupportTicketId).SendAsync(nameof(TechnicianHasCreatedOssCase).ToLower(), message);
        }
    }

    public class TechnicianHasStartedCallMessage
    {
        public TechnicianHasStartedCallMessage(string supportTicketId)
        {
            SupportTicketId = supportTicketId;
        }

        public string SupportTicketId { get; private set; }
    }

    public class TechnicianHasEndedCallMessage
    {
        public TechnicianHasEndedCallMessage(string supportTicketId)
        {
            SupportTicketId = supportTicketId;
        }

        public string SupportTicketId { get; private set; }
    }

    public class TechnicianHasCreatedOssCaseMessage
    {
        public TechnicianHasCreatedOssCaseMessage(string supportTicketId, string ossId)
        {
            OssId = ossId;
            SupportTicketId = supportTicketId;
        }

        public string OssId { get; private set; }
        public string SupportTicketId { get; private set; }

    }
}