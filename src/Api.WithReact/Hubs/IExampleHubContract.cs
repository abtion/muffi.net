using DomainModel.Models;

namespace Api.WithReact.Hubs;

public interface IExampleHubContract {
    Task SomeEntityUpdated(SomeEntityUpdatedMessage message);
    Task SomeEntityCreated(SomeEntityCreatedMessage message);
    Task SomeEntityDeleted(SomeEntityDeletedMessage message);
}

public class SomeEntityCreatedMessage {
    public SomeEntityCreatedMessage(ExampleEntityRecord entity) {
        Entity = entity;
    }

    public ExampleEntityRecord Entity { get; private set; }
}

public class SomeEntityUpdatedMessage {
    public SomeEntityUpdatedMessage(ExampleEntityRecord entity) {
        Entity = entity;
    }

    public ExampleEntityRecord Entity { get; private set; }
}

public class SomeEntityDeletedMessage {
    public SomeEntityDeletedMessage(int entityId) {
        EntityId = entityId;
    }

    public int EntityId { get; private set; }
}