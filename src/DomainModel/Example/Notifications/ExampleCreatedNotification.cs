using Domain.Example.Entities;
using MediatR;

namespace Domain.Example.Notifications;

public class ExampleCreatedNotification : INotification
{
    public ExampleCreatedNotification(ExampleEntity model)
    {
        Model = model;
    }

    public ExampleEntity Model { get; }
}