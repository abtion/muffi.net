using DomainModel.Models;

namespace DomainModel.Commands;

public record ExampleUpdateResponse {
    public ExampleEntity? ExampleEntity { get; set; }
}