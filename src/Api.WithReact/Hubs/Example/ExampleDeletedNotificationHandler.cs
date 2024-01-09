using Domain.Example.Notifications;
using MediatR;

namespace Api.WithReact.Hubs.Example;

public class ExampleDeletedNotificationHandler : INotificationHandler<ExampleDeletedNotification>
{
    private readonly IExampleHubContract exampleHub;

    public ExampleDeletedNotificationHandler(IExampleHubContract exampleHub)
    {
        this.exampleHub = exampleHub;
    }

    public async Task Handle(ExampleDeletedNotification notification, CancellationToken cancellationToken)
    {
        await exampleHub.SomeEntityDeleted(new SomeEntityDeletedMessage(notification.EntityId));
    }
}
