using System.Threading.Tasks;
using MuffiNet.FrontendReact.Hubs;
namespace MuffiNet.FrontendReact.Test.Mocks
{
    public class TechnicianHubMock : TechnicianHub
    {
        public TechnicianHubMock() : base(null)
        {
            // skip
        }

        public int SupportTicketDeletedMessageCounter;
        public SupportTicketDeletedMessage LatestSupportTicketDeletedMessage;

        public int SupportTicketCreatedMessageCounter;
        public SupportTicketCreatedMessage LatestSupportTicketCreatedMessage;

        public int SupportTicketUpdatedMessageCounter;
        public SupportTicketUpdatedMessage LatestSupportTicketUpdatedMessage;

        public override Task SupportTicketCreated(SupportTicketCreatedMessage message)
        {
            SupportTicketCreatedMessageCounter++;
            LatestSupportTicketCreatedMessage = message;
            return Task.CompletedTask;
        }

        public override Task SupportTicketDeleted(SupportTicketDeletedMessage message)
        {
            SupportTicketDeletedMessageCounter++;
            LatestSupportTicketDeletedMessage = message;
            return Task.CompletedTask;
        }
        public override Task SupportTicketUpdated(SupportTicketUpdatedMessage message)
        {
            SupportTicketUpdatedMessageCounter++;
            LatestSupportTicketUpdatedMessage = message;
            return Task.CompletedTask;
        }
    }
}
