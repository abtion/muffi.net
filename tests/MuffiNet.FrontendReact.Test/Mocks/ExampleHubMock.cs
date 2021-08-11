using System.Threading.Tasks;
using MuffiNet.FrontendReact.Hubs;
namespace MuffiNet.FrontendReact.Test.Mocks
{
    public class TechnicianHubMock : ExampleHub
    {
        public TechnicianHubMock() : base(null)
        {
            // skip
        }

        public int SupportTicketDeletedMessageCounter;
        public SomeEntityDeletedMessage LatestSupportTicketDeletedMessage;

        public int SupportTicketCreatedMessageCounter;
        public SomeEntityCreatedMessage LatestSupportTicketCreatedMessage;

        public int SupportTicketUpdatedMessageCounter;
        public SomeEntityUpdatedMessage LatestSupportTicketUpdatedMessage;

        public override Task SomeEntityCreated(SomeEntityCreatedMessage message)
        {
            SupportTicketCreatedMessageCounter++;
            LatestSupportTicketCreatedMessage = message;
            return Task.CompletedTask;
        }

        public override Task SomeEntityDeleted(SomeEntityDeletedMessage message)
        {
            SupportTicketDeletedMessageCounter++;
            LatestSupportTicketDeletedMessage = message;
            return Task.CompletedTask;
        }
        public override Task SomeEntityUpdated(SomeEntityUpdatedMessage message)
        {
            SupportTicketUpdatedMessageCounter++;
            LatestSupportTicketUpdatedMessage = message;
            return Task.CompletedTask;
        }
    }
}
