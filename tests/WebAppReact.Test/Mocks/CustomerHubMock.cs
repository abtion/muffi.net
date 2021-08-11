using System.Threading.Tasks;
using WebAppReact.Hubs;

namespace WebAppReact.Test.Mocks
{
    public class CustomerHubMock : CustomerHub
    {
        public CustomerHubMock() : base(null)
        {
            // skip
        }

        public int TechnicianHasStartedCallCounter;
        public TechnicianHasStartedCallMessage LatestStartedCallMessage;

        public int TechnicianHasEndedCallCounter;
        public TechnicianHasEndedCallMessage LatestEndedCallMessage;

        public int TechnicianHasCreatedOssCaseCounter;
        public TechnicianHasCreatedOssCaseMessage LatestTechnicianHasCreatedOssCaseMessage;

        public override Task TechnicianHasEndedCall(TechnicianHasEndedCallMessage message)
        {
            TechnicianHasEndedCallCounter++;
            LatestEndedCallMessage = message;
            return Task.CompletedTask;
        }

        public override Task TechnicianHasStartedCall(TechnicianHasStartedCallMessage message)
        {
            TechnicianHasStartedCallCounter++;
            LatestStartedCallMessage = message;
            return Task.CompletedTask;
        }

        public override Task TechnicianHasCreatedOssCase(TechnicianHasCreatedOssCaseMessage message)
        {
            TechnicianHasCreatedOssCaseCounter++;
            LatestTechnicianHasCreatedOssCaseMessage = message;
            return Task.CompletedTask;
        }

        public override Task JoinGroup(string supportTicketId)
        {
            return Task.CompletedTask;
        }
    }
}