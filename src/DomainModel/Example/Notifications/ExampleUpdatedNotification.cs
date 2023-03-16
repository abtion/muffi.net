using MediatR;

namespace DomainModel.Example.Notifications;

public class ExampleUpdatedNotification : INotification
{
    public ExampleUpdatedNotification(IExampleModel model)
    {
        Model = model;
    }

    public IExampleModel Model { get; }
}