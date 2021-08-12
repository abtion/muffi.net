using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MuffiNet.Backend.HubContracts;
using MuffiNet.Backend.Data;
using MuffiNet.Backend.DomainModel;
using MuffiNet.FrontendReact.Hubs;
using MuffiNet.Backend.Models;

namespace MuffiNet.FrontendReact
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<ApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddIdentityServer()
                .AddApiAuthorization<ApplicationUser, ApplicationDbContext>(options =>
                { // Debugging identityserver purposes
                    // foreach (var c in options.Clients) {
                    //     c.AccessTokenLifetime = 30;
                    // }
                });

            services.AddAuthentication()
                .AddIdentityServerJwt();

            services.TryAddEnumerable(
                ServiceDescriptor.Singleton<IPostConfigureOptions<JwtBearerOptions>,
                    ConfigureJwtBearerOptions>());

            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddDomainModel();

            // Add SignalR Hubs here
            services.AddTransient<IExampleHubContract, ExampleHub>();

            // SignalR setup + enable CORS for SignalR
            services.AddSignalR();
            services.AddCors(options =>
            {
                options.AddPolicy("ClientPermission", policy =>
                {
                    policy.AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin();
                    //.WithOrigins("http://localhost:3000", "http://localhost:5000", "https://localhost:5001")
                    //.AllowCredentials();
                });
            });

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

            services.AddApplicationInsightsTelemetry(Configuration["APPINSIGHTS_CONNECTIONSTRING"]);
            services.AddHttpContextAccessor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment() || env.IsEnvironment("Test"))
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            // SignalR CORS
            app.UseCors("ClientPermission");

            app.UseRouting();

            app.UseAuthentication();
            app.UseIdentityServer();
            app.UseAuthorization();
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

                if (env.IsDevelopment() || env.IsEnvironment("Test"))
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}