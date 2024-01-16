using Api.Shared.Authentication.OpenIdConnect;
using Api.WithReact;
using Api.WithReact.Hubs;
using Domain;
using Infrastructure;
using Presentation;
using Presentation.UserManagement;

var builder = WebApplication.CreateBuilder(args);

//
var configuration = builder.Configuration;

// OIDC Authentication
builder.Services.AddOidcAuthentication(configuration);

// services
builder.Services.AddInfrastructure(configuration);
builder.Services.AddUserManagementInfrastructure();
builder.Services.AddDomain();
builder.Services.AddPresentation();
builder.Services.AddUserManagementPresentation();
builder.Services.AddApi();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.AddSignalRHubs(configuration);

builder.Services.AddApplicationInsightsTelemetry(
    (options) =>
    {
        options.ConnectionString = configuration["APPINSIGHTS_CONNECTIONSTRING"];
    }
);

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Test"))
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.EnableTryItOutByDefault();
    });

    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSignalRCors();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseSignalRHubs();

app.MapFallbackToFile("index.html");

app.Run();
