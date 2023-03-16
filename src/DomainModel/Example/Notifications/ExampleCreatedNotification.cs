using MediatR;

namespace DomainModel.Example.Notifications;

public class ExampleCreatedNotification : INotification
{
    public ExampleCreatedNotification(IExampleModel model)
    {
        Model = model;
    }

    public IExampleModel Model { get; }
}