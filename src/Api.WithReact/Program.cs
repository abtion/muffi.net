using Api.Shared.Authentication.OpenIdConnect;
using Api.WithReact;
using Api.WithReact.Hubs;
using DomainModel;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddOidcAuthentication(configuration);

// services
builder.Services.AddDatabase(configuration);
builder.Services.AddDomainModel(configuration);
builder.Services.AddApi();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.AddSignalRHubs(configuration);

builder.Services.AddApplicationInsightsTelemetry(
    (options) => options.ConnectionString = configuration["APPINSIGHTS_CONNECTIONSTRING"]
);

// builder.Services.AddHttpContextAccessor(); // necessary?

var app = builder.Build();

// app.UseDefaultFiles(); // necessary?
// app.UseStaticFiles(); // necessary?

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Test"))
{
    app.UseSwagger();
    app.UseSwaggerUI(options => options.EnableTryItOutByDefault());
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}

app.UseHttpsRedirection();

app.UseSignalRCors();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseSignalRHubs();

app.MapFallbackToFile("index.html");

app.Run();
