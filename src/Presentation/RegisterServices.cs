using Domain.Example.Queries;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Presentation.Example.Commands;
using Presentation.Example.Dtos;
using Presentation.Example.Mappers;

namespace Presentation;

public static class RegisterServices
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddScoped<ExampleLoadAllQueryHandler>();
        services.AddScoped<ExampleLoadSingleQueryHandler>();

        services.AddSingleton<ExampleMapper>();

        // keep these lines until mediator.Send([ICommand]) is used in all controllers and tests
        services.AddScoped<ExampleCreateCommandHandler>();
        services.AddScoped<ExampleUpdateCommandHandler>();
        services.AddScoped<ExampleDeleteCommandHandler>();

        // MediatR setup with validation pipeline behavior
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(RegisterServices).Assembly);
        });

        // https://code-maze.com/cqrs-mediatr-fluentvalidation/
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddValidatorsFromAssembly(typeof(RegisterServices).Assembly);

        services.AddScoped<AbstractValidator<ExampleCreateCommand>, ExampleCreateCommandValidator>();

        return services;
    }
}
