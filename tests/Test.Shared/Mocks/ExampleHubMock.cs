using MuffiNet.Backend.HubContracts;
using System.Threading.Tasks;

namespace MuffiNet.Test.Shared.Mocks;

public class ExampleHubMock : IExampleHubContract {

    public int EntityDeletedMessageCounter { get; private set; }
    public SomeEntityDeletedMessage? LatestEntityDeletedMessage { get; private set; }

    public int EntityCreatedMessageCounter { get; private set; }
    public SomeEntityCreatedMessage? LatestEntityCreatedMessage { get; private set; }

    public int EntityUpdatedMessageCounter { get; private set; }
    public SomeEntityUpdatedMessage? LatestEntityUpdatedMessage { get; private set; }

    public Task SomeEntityCreated(SomeEntityCreatedMessage message) {
        EntityCreatedMessageCounter++;
        LatestEntityCreatedMessage = message;
        return Task.CompletedTask;
    }

    public Task SomeEntityDeleted(SomeEntityDeletedMessage message) {
        EntityDeletedMessageCounter++;
        LatestEntityDeletedMessage = message;
        return Task.CompletedTask;
    }
    public Task SomeEntityUpdated(SomeEntityUpdatedMessage message) {
        EntityUpdatedMessageCounter++;
        LatestEntityUpdatedMessage = message;
        return Task.CompletedTask;
    }
}
