using FluentValidation;
using static Domain.Example.Commands.ExampleCreateCommandHandler;

namespace Domain.Example.Commands;

public class ExampleCreateCommandValidator : AbstractValidator<ExampleCreateCommand>
{
    public ExampleCreateCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name cannot be empty");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email address cannot be empty");

        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("Please specify a valid email address");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description cannot be empty");

        RuleFor(x => x.Phone)
            .NotEmpty()
            .WithMessage("Phone cannot be empty");
    }
}