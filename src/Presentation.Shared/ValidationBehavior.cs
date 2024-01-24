using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Presentation.Shared;
public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> Validators) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!Validators.Any())
            return await next();

        var validationFailures = new List<ValidationFailure>();

        foreach (var validator in Validators) 
        {
            var result = validator.Validate(request);

            if (result is not null) 
            {
                validationFailures.AddRange(result.Errors);
            }
        }

        if (validationFailures.Any())
            throw new ValidationException("Unable to validate", validationFailures);

        return await next();
    }
}