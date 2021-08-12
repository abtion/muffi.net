using System.Threading.Tasks;
using MuffiNet.Backend.HubContracts;
using MuffiNet.FrontendReact.Hubs;

namespace MuffiNet.Test.Shared.Mocks
{
    public class ExampleHubMock : ExampleHub
    {
        public ExampleHubMock() : base(null)
        {
            // skip
        }

        public int EntityDeletedMessageCounter;
        public SomeEntityDeletedMessage? LatestEntityDeletedMessage;

        public int EntityCreatedMessageCounter;
        public SomeEntityCreatedMessage? LatestEntityCreatedMessage;

        public int EntityUpdatedMessageCounter;
        public SomeEntityUpdatedMessage? LatestEntityUpdatedMessage;

        public override Task SomeEntityCreated(SomeEntityCreatedMessage message)
        {
            EntityCreatedMessageCounter++;
            LatestEntityCreatedMessage = message;
            return Task.CompletedTask;
        }

        public override Task SomeEntityDeleted(SomeEntityDeletedMessage message)
        {
            EntityDeletedMessageCounter++;
            LatestEntityDeletedMessage = message;
            return Task.CompletedTask;
        }
        public override Task SomeEntityUpdated(SomeEntityUpdatedMessage message)
        {
            EntityUpdatedMessageCounter++;
            LatestEntityUpdatedMessage = message;
            return Task.CompletedTask;
        }
    }
}
