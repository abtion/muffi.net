namespace Presentation.Example.Dtos;

public record ExampleDto 
{
    public ExampleDto(int id, string name, string description, string email, string phone)
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
