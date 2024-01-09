using Domain.Example.Entities;
using MediatR;

namespace Domain.Example.Notifications;

public class ExampleUpdatedNotification : INotification
{
    public ExampleUpdatedNotification(ExampleEntity model)
    {
        Model = model;
    }

    public ExampleEntity Model { get; }
}