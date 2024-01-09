using MediatR;

namespace Domain.Example.Notifications;

public class ExampleDeletedNotification : INotification
{
    public ExampleDeletedNotification(int entityId)
    {
        EntityId = entityId;
    }

    public int EntityId { get; }
}