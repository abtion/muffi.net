using MediatR;

namespace DomainModel.Queries;

public record ExampleLoadAllQuery : IRequest<ExampleLoadAllResponse> {
}