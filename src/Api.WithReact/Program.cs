using Api.Shared.Authentication.OpenIdConnect;
using Api.WithReact;
using Api.WithReact.Hubs;
using DomainModel;
using DomainModel.UserAdministration.Commands;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

// OIDC Authentication
builder.Services.AddOidcAuthentication(configuration);

builder.Services.AddDatabase(configuration);
builder.Services.AddDomainModel(configuration);
builder.Services.AddApi();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.AddSignalRHubs(configuration);

builder.Services.AddApplicationInsightsTelemetry((options) =>
{
    options.ConnectionString = configuration["APPINSIGHTS_CONNECTIONSTRING"];
});

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

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
app.Run();