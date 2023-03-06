using DomainModel.HubContracts;

namespace MuffiNet.FrontendReact.Hubs;

public static class HubExtensions {
    private const string SignalRHubPolicyName = "SignalRHubsPolicy";

    internal static IServiceCollection AddSignalRHubs(this WebApplicationBuilder builder, IConfiguration configuration) {

        // 
        builder.Services.AddTransient<IExampleHubContract, ExampleHub>();

        builder.Services.AddSignalR();

        builder.Services.AddCors(options => {
            options.AddPolicy(SignalRHubPolicyName, policy => {
                policy.AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithOrigins("https://localhost:5001", "https://localhost:44337");
            });
        });

        return builder.Services;
    }

    internal static IApplicationBuilder UseSignalRCors(this IApplicationBuilder app) {

        app.UseCors(SignalRHubPolicyName);

        return app;
    }

    internal static IEndpointRouteBuilder UseSignalRHubs(this IEndpointRouteBuilder app) {
        app.MapHub<ExampleHub>("/hubs/example");

        return app;
    }
}