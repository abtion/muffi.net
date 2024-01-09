using MediatR;

namespace Domain.Shared;

public interface ICommand<out TResponse> : IRequest<TResponse>
{
    // skip
}