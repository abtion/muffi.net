namespace MuffiNet.FrontendReact;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            })
            .ConfigureAppConfiguration((context, builder) =>
            {
                builder.AddJsonFile("appsettings.Local.json", true, true);
                builder.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.Local.json", true, true);
            });
}
