using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(WebAppReact.Areas.Identity.IdentityHostingStartup))]
namespace WebAppReact.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}