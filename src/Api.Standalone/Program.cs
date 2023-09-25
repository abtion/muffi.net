using Api.Shared.Authentication.OpenIdConnect;
using Api.Standalone;
using DomainModel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// configuration
var configuration = builder.Configuration;

// OIDC Authentication
builder.Services.AddOidcAuthentication(builder.Configuration);

// services
builder.Services.AddDatabase(configuration);
builder.Services.AddDomainModel(configuration);
builder.Services.AddApi();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();

// Some useful additions
//builder.Services.AddDistributedMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (
    app.Environment.IsEnvironment("Local")
    || app.Environment.IsStaging()
    || app.Environment.IsDevelopment()
)
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.EnableTryItOutByDefault();
    });
}

app.UseHttpsRedirection();

app.MapEndpoints();

app.Run();
