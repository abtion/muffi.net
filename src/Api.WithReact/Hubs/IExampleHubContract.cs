using Domain.Example.Entities;

namespace Api.WithReact.Hubs;

public interface IExampleHubContract
{
    Task SomeEntityUpdated(SomeEntityUpdatedMessage message);
    Task SomeEntityCreated(SomeEntityCreatedMessage message);
    Task SomeEntityDeleted(SomeEntityDeletedMessage message);
}

public class SomeEntityCreatedMessage(ExampleEntity Entity)
{
    public ExampleEntity Entity { get; } = Entity;
}

public class SomeEntityUpdatedMessage(ExampleEntity Entity)
{
    public ExampleEntity Entity { get; } = Entity;
}

public class SomeEntityDeletedMessage(int EntityId)
{
    public int EntityId { get; } = EntityId;
}