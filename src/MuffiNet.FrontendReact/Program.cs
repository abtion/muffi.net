using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MuffiNet.Authentication.OpenIdConnect;
using MuffiNet.Backend.Data;
using MuffiNet.Backend.DomainModel;
using MuffiNet.Backend.HubContracts;
using MuffiNet.FrontendReact.Hubs;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// OIDC Authentication
builder.Services.AddOidcAuthentication(builder.Configuration);

builder.Services.AddControllers();
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
//builder.Services.AddSpaStaticFiles(configuration =>
//{
//    configuration.RootPath = "ClientApp/build";
//});

builder.Services.AddApplicationInsightsTelemetry(builder.Configuration["APPINSIGHTS_CONNECTIONSTRING"]);
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
//app.UseSpaStaticFiles();

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

//app.UseSpa(spa =>
//{
//    spa.Options.SourcePath = "ClientApp";
//    spa.Options.PackageManagerCommand = "yarn";

//    if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Test"))
//    {
//        spa.UseReactDevelopmentServer(npmScript: "start");
//    }
//});

app.Run();