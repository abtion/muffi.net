using Domain.Example.Notifications;
using MediatR;

namespace Api.WithReact.Hubs.Example;

public class ExampleCreatedNotificationHandler : INotificationHandler<ExampleCreatedNotification>
{
    private readonly IExampleHubContract exampleHub;

    public ExampleCreatedNotificationHandler(IExampleHubContract exampleHub)
    {
        this.exampleHub = exampleHub;
    }

    public async Task Handle(ExampleCreatedNotification notification, CancellationToken cancellationToken)
    {
        await exampleHub.SomeEntityCreated(new SomeEntityCreatedMessage(notification.Model));
    }
}
