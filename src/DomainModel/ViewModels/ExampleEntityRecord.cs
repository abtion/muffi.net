using DomainModel.Example;

namespace DomainModel.ViewModels;

public record ExampleEntityRecord : IExampleModel
{
    public ExampleEntityRecord(
        int id,
        string name,
        string description,
        string email,
        string phone)
    {
        Id = id;
        Name = name;
        Description = description;
        Email = email;
        Phone = phone;
    }

    public int Id { get; }
    public string Name { get; }
    public string Description { get; }
    public string Email { get; }
    public string Phone { get; }
}