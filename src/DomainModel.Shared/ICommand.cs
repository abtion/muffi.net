using MediatR;

namespace DomainModel.Shared;

public interface ICommand<out TResponse> : IRequest<TResponse>
{
    // skip
}