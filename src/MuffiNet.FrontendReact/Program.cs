using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MuffiNet.Backend.Data;
using MuffiNet.Backend.DomainModel;
using MuffiNet.Backend.HubContracts;
using MuffiNet.FrontendReact.Hubs;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Configuration.AddJsonFile("appsettings.Local.json", true, true);

// Add services to the container.
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = configuration.GetValue<string>("Authentication:Authority");
        options.ClaimsIssuer = configuration.GetValue<string>("Authentication:ClaimsIssuer"); 
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateTokenReplay = true,
            ValidIssuer = configuration.GetValue<string>("Authentication:ValidIssuer"),
            ValidAudience = configuration.GetValue<string>("Authentication:ValidAudience"),
        };
    });

builder.Services.AddControllers();
builder.Services.AddRazorPages();

builder.Services.AddRazorPages();

builder.Services.AddDomainModel();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()
    )
);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Add SignalR Hubs here
builder.Services.AddTransient<IExampleHubContract, ExampleHub>();

// SignalR setup + enable CORS for SignalR
builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddPolicy("ClientPermission", policy =>
    {
        policy.AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin();
    });
});

// In production, the React files will be served from this directory
builder.Services.AddSpaStaticFiles(configuration =>
{
    configuration.RootPath = "ClientApp/build";
});

builder.Services.AddApplicationInsightsTelemetry(builder.Configuration["APPINSIGHTS_CONNECTIONSTRING"]);
builder.Services.AddHttpContextAccessor();





var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Test"))
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSpaStaticFiles();

// SignalR CORS
app.UseCors("ClientPermission");

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller}/{action=Index}/{id?}");
    endpoints.MapRazorPages();

    // Setup SignalR Hubs
    endpoints.MapHub<ExampleHub>("/hubs/example");
});

app.UseSpa(spa =>
{
    spa.Options.SourcePath = "ClientApp";
    spa.Options.PackageManagerCommand = "yarn";

    if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Test"))
    {
        spa.UseReactDevelopmentServer(npmScript: "start");
    }
});

app.Run();