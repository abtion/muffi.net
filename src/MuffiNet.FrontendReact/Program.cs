using Microsoft.EntityFrameworkCore;
using MuffiNet.Authentication.OpenIdConnect;
using MuffiNet.Backend.Data;
using MuffiNet.Backend.DomainModel;
using MuffiNet.FrontendReact.Hubs;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

// OIDC Authentication
builder.Services.AddOidcAuthentication(configuration);

builder.Services.AddControllers();
builder.Services.AddRazorPages();

builder.Services.AddDomainModel();
builder.Services.AddUserRoleService(configuration);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        configuration.GetConnectionString("DefaultConnection"),
        sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()
    )
);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.AddSignalRHubs(configuration);

builder.Services.AddApplicationInsightsTelemetry((options) => {
    options.ConnectionString = configuration["APPINSIGHTS_CONNECTIONSTRING"];
});

builder.Services.AddHttpContextAccessor();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Test")) {
    app.UseSwagger();
    app.UseSwaggerUI(options => {
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