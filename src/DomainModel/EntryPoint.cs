using DomainModel.Example.Commands;
using DomainModel.Example.Queries;
using DomainModel.Services;
using DomainModel.Shared;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace DomainModel;

public static class EntryPoint
{
    public static IServiceCollection AddDomainModel(this IServiceCollection services)
    {

        services.AddScoped<DomainModelTransaction>();
        services.AddTransient<ICurrentDateTimeService, CurrentDateTimeService>();
        services.AddTransient<IExampleReverseStringService, ExampleReverseStringService>();

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(EntryPoint).Assembly);
            //cfg.RegisterServicesFromAssemblyContaining(typeof(ICommandHandler<,>));
            // cfg.AddBehavior < IPipelineBehavior <, >, ValidationBehavior <, >> ();
        });

        // keep these lines until mediator.Send([ICommand]) is used in all controllers and tests
        services.AddScoped<ExampleLoadAllQueryHandler>();
        services.AddScoped<ExampleLoadSingleQueryHandler>();
        services.AddScoped<ExampleCreateCommandHandler>();
        services.AddScoped<ExampleUpdateCommandHandler>();
        services.AddScoped<ExampleDeleteCommandHandler>();

        services.AddScoped<AbstractValidator<ExampleCreateCommand>, ExampleCreateCommandValidator>();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.AddValidatorsFromAssembly(typeof(EntryPoint).Assembly);

        // https://code-maze.com/cqrs-mediatr-fluentvalidation/
        // services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}