using DomainModel.Example;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Data.Models;

public class ExampleEntity : IExampleModel
{
    [Required]
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(30)]
    public string Phone { get; set; } = string.Empty;
}