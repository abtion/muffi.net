//using Api.WithReact.Hubs;
//using DomainModel.ViewModels;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.SignalR.Client;
//using Microsoft.AspNetCore.TestHost;
//using Microsoft.Extensions.DependencyInjection;
//using System.Threading.Tasks;

//namespace Api.WithReact.Tests.Hubs;

// TODO evaluate tests - are we just testing that SignalR works?

//[Collection("Hubs")]
//public class ExampleHubTests {
//    private static HubConnection SetupHubConnection(TestServer server) {
//        var connection = new HubConnectionBuilder()
//            .WithUrl(
//                "http://localhost/hubs/example",
//                o => o.HttpMessageHandlerFactory = _ => server.CreateHandler())
//            .Build();

//        return connection;
//    }

//    private static TestServer CreateServer() {
//        var webHostBuilder = new WebHostBuilder()
//            .ConfigureServices(services => {
//                services.AddSignalR();
//            })
//            .Configure(app => {
//                app.UseRouting();
//                app.UseEndpoints(endpoints => {
//                    // Setup SignalR Hubs
//                    endpoints.MapHub<ExampleHub>("/hubs/example");
//                });
//            });

//        var server = new TestServer(webHostBuilder);
//        return server;
//    }

//    [Fact]
//    public async Task ShouldSendEntityCreatedMessage() {
//        using (var server = CreateServer()) {
//            await using (var connection = SetupHubConnection(server)) {

//                connection.On<SomeEntityCreatedMessage>("SomeEntityCreated", msg => {
//                    msg.Should().NotBeNull();
//                });

//                var message = new SomeEntityCreatedMessage(new ExampleEntityRecord(123, "My Name", "MyDescription", "My Email", "My Phone"));

//                await connection.StartAsync();
//                await connection.InvokeAsync("SomeEntityCreated", message);
//            }
//        }
//    }

//    [Fact]
//    public async Task ShouldSendEntityDeletedMessage() {
//        using (var server = CreateServer()) {
//            await using (var connection = SetupHubConnection(server)) {
//                connection.On<SomeEntityDeletedMessage>("SomeEntityDeleted", msg => {
//                    msg.Should().Be(123);
//                });

//                var message = new SomeEntityDeletedMessage(123);

//                await connection.StartAsync();
//                await connection.InvokeAsync("SomeEntityDeleted", message);
//            }
//        }
//    }

//    [Fact]
//    public async Task ShouldSendEntityUpdatedMessage() {
//        using (var server = CreateServer()) {
//            await using (var connection = SetupHubConnection(server)) {

//                connection.On<SomeEntityUpdatedMessage>("SomeEntityUpdated", msg => {
//                    msg.Should().NotBeNull();
//                });

//                var message = new SomeEntityUpdatedMessage(new ExampleEntityRecord(123, "My Name", "MyDescription", "My Email", "My Phone"));

//                await connection.StartAsync();
//                await connection.InvokeAsync("SomeEntityUpdated", message);
//            }
//        }
//    }
//}
