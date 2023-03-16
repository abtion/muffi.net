using Api.WithReact.Hubs;
using DomainModel;
using DomainModel.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using Test.Shared.Mocks;

namespace Api.WithReact.Tests;

public class ServiceConfigurationTests
{
    [Fact]
    public void CanResolveServiceDependencies()
    {
        var builder = WebApplication.CreateBuilder();

        // count the build-in services before adding application services
        var msServiceCount = builder.Services.Count;

        // add application services
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase(GetType().Name));

        builder.Services.AddDomainModel();
        builder.Services.AddApi();

        var serviceDescriptor = builder.Services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(IExampleHubContract));
        builder.Services.Remove(serviceDescriptor);
        builder.Services.AddScoped<IExampleHubContract, ExampleHubMock>();

        var app = builder.Build();

        var test = () =>
        {
            var customServices = builder.Services.Skip(msServiceCount).Where(p => !SkipList.Contains(p.ServiceType.FullName));

            foreach (var descriptor in customServices)
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine($"{descriptor.ServiceType}");
                    app.Services.GetService(descriptor.ServiceType);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(descriptor.ServiceType.FullName);
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
        };

        test.Should().NotThrow();
    }

    private IEnumerable<string> SkipList = new List<string>
    {
        "Microsoft.Extensions.Http.DefaultTypedHttpClientFactory`1+Cache[TClient]",
        "Microsoft.Extensions.Http.ITypedHttpClientFactory`1[TClient]",
        "MediatR.IPipelineBehavior`2"
    };
}
