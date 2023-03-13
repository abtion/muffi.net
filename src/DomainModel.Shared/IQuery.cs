using MediatR;

namespace DomainModel.Shared;

public interface IQuery<out TResponse> : IRequest<TResponse> {
    // skip
}