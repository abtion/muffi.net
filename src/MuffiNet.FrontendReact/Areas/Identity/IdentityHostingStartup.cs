[assembly: HostingStartup(typeof(MuffiNet.FrontendReact.Areas.Identity.IdentityHostingStartup))]
namespace MuffiNet.FrontendReact.Areas.Identity;

public class IdentityHostingStartup : IHostingStartup
{
    public void Configure(IWebHostBuilder builder)
    {
        builder.ConfigureServices((context, services) =>
        {
        });
    }
}