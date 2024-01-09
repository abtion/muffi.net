using MediatR;

namespace Domain.Shared;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
    // skip
}