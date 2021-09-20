using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.TestHost;
using MuffiNet.Backend.HubContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using MuffiNet.FrontendReact.Hubs;

namespace MuffiNet.FrontendReact.Tests.Hubs
{
    [Collection("Hubs")]
    public class ExampleHubTests
    {
        private static HubConnection SetupHubConnection()
        {
            var webHostBuilder = new WebHostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddSignalR();
                })
                .Configure(app =>
                {
                    app.UseRouting();
                    app.UseEndpoints(endpoints =>
                    {
                        // Setup SignalR Hubs
                        endpoints.MapHub<ExampleHub>("/hubs/example");
                    });
                });

            var server = new TestServer(webHostBuilder);

            var connection = new HubConnectionBuilder()
                .WithUrl(
                    "http://localhost/hubs/example",
                    o => o.HttpMessageHandlerFactory = _ => server.CreateHandler())
                .Build();
            return connection;
        }

        [Fact]
        public async Task ShouldSendEntityCreatedMessage()
        {
            HubConnection connection = SetupHubConnection();

            connection.On<SomeEntityUpdatedMessage>("SomeEntityCreated", msg =>
            {
                msg.Should().NotBeNull();
            });

            var message = new SomeEntityCreatedMessage(new Backend.Models.ExampleEntityRecord(123, "My Name", "MyDescription", "My Email", "My Phone"));

            await connection.StartAsync();
            await connection.InvokeAsync("SomeEntityCreated", message);
        }

        [Fact]
        public async Task ShouldSendEntityDeletedMessage()
        {
            HubConnection connection = SetupHubConnection();

            connection.On<SomeEntityUpdatedMessage>("SomeEntityDeleted", msg =>
            {
                msg.Should().NotBeNull();
            });

            var message = new SomeEntityDeletedMessage(123);

            await connection.StartAsync();
            await connection.InvokeAsync("SomeEntityDeleted", message);
        }

        [Fact]
        public async Task ShouldSendEntityUpdatedMessage()
        {
            HubConnection connection = SetupHubConnection();

            connection.On<SomeEntityUpdatedMessage>("SomeEntityUpdated", msg =>
            {
                msg.Should().NotBeNull();
            });

            var message = new SomeEntityUpdatedMessage(new Backend.Models.ExampleEntityRecord(123, "My Name", "MyDescription", "My Email", "My Phone"));

            await connection.StartAsync();
            await connection.InvokeAsync("SomeEntityUpdated", message);
        }
    }
}