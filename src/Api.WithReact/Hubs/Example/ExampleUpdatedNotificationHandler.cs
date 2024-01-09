using Domain.Example.Notifications;
using MediatR;

namespace Api.WithReact.Hubs.Example;

public class ExampleUpdatedNotificationHandler : INotificationHandler<ExampleUpdatedNotification>
{
    private readonly IExampleHubContract exampleHub;

    public ExampleUpdatedNotificationHandler(IExampleHubContract exampleHub)
    {
        this.exampleHub = exampleHub;
    }

    public async Task Handle(ExampleUpdatedNotification notification, CancellationToken cancellationToken)
    {
        await exampleHub.SomeEntityUpdated(new SomeEntityUpdatedMessage(notification.Model));
    }
}
