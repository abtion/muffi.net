using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MuffiNet.Backend.Data;
using OpenQA.Selenium.Chrome;
using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace MuffiNet.FrontendReact.Selenium.Tests;

/// <remarks>
/// https://xunit.net/docs/shared-context
/// </remarks>
public sealed class IntegrationFixture : IDisposable
{
    private Process process;
    private IConfigurationRoot Configuration;
    internal ChromeDriver webDriver { get; private set; }

    public IntegrationFixture()
    {
        var task = Task.Run(() =>
        {
            DropDatabase();
            CreateDatabase();
            SeedDatabase();
        });

        if (task.Wait(TimeSpan.FromSeconds(80)))
        {
            webDriver = StartWebDriver();
            process = StartServer();
        }
        else
        {
            throw new TimeoutException("Database setup timed out");
        }
    }

    private void DropDatabase()
    {
        var config = GetConfiguration();

        try
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseSqlServer(config.GetConnectionString("DefaultConnection"));

            using var context = new ApplicationDbContext(builder.Options);
            context.Database.EnsureDeletedAsync().Wait();

            Console.WriteLine("Dropping test database: success");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Dropping test database: failure");
            Console.WriteLine(ex.ToString());

            throw new InvalidOperationException("Nonzero exit code when dropping test database", ex);
        }
    }

    private void SeedDatabase()
    {
        Console.WriteLine("Seeding database");

        //var queryString = File.ReadAllText(Path.Join(GetWorkingDirectory(), "..", "..", "sql", "create-user.sql"));
        //SqlConnection connection = DbConnection();
        //SqlCommand command = new SqlCommand(queryString, connection);
        //command.Connection.Open();
        //command.ExecuteNonQuery();
        //command.Dispose();
        //connection.Dispose();

        Console.WriteLine("Seeding database: success");
    }

    private void CreateDatabase()
    {
        var config = GetConfiguration();

        try
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseSqlServer(config.GetConnectionString("DefaultConnection"));

            using var context = new ApplicationDbContext(builder.Options);
            context.Database.EnsureCreatedAsync().Wait();

            Console.WriteLine("Creating/updating test database: success");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Creating/updating test database: failure");
            Console.WriteLine(ex.ToString());

            throw new InvalidOperationException("Nonzero exit code when creating/updating test database", ex);
        }
    }

    private Process StartServer()
    {
        Console.WriteLine("Starting server");
        var process = Process.Start(CreateStartInfo("run --launch-profile \"MuffiNet.FrontendReact Test\""));

        string output;

        do
        {
            output = process.StandardOutput.ReadLine();
        }
        while (!output.Contains("Now listening on", StringComparison.InvariantCultureIgnoreCase));
        Console.WriteLine("Starting server: Success");

        return process;
    }

    private ChromeDriver StartWebDriver()
    {
        var options = new ChromeOptions();
        options.AddArguments("--headless");
        options.AddArguments("ignore-certificate-errors");

        var webDriver = new ChromeDriver(options);
        webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        webDriver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(120);

        return webDriver;
    }

    private ProcessStartInfo CreateStartInfo(string arguments)
    {
        var startInfo = new ProcessStartInfo()
        {
            FileName = "dotnet",
            Arguments = arguments,
            UseShellExecute = false,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            WorkingDirectory = GetWorkingDirectory(),
        };
        startInfo.Environment["ASPNETCORE_ENVIRONMENT"] = "Test";

        return startInfo;
    }

    private string GetWorkingDirectory()
    {
        var workingDirectory = Path.Join(Directory.GetCurrentDirectory());

        // getting the beginning of the path right
        // could have been done with a simple replace - but this seems more bullet proof
        for (int i = 0; i < 5; i++)
        {
            workingDirectory = Path.Join(workingDirectory, "..");
        }

        return Path.Join(workingDirectory, "src", "MuffiNet.FrontendReact");
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        webDriver.Quit();
        webDriver.Dispose();

        if (process != null && !process.HasExited)
        {
            process.Kill(entireProcessTree: true);
        }
    }

    public void CleanUpBetweenTests()
    {
        // This is largely inspired by capybara
        // https://github.com/teamcapybara/capybara/blob/090bebf3a0ed3758220c435566c2716495ba11ae/lib/capybara/selenium/driver.rb#L506
        webDriver.Manage().Cookies.DeleteAllCookies();
        webDriver.ExecuteScript("window.sessionStorage.clear()");
        webDriver.ExecuteScript("window.localStorage.clear()");
        webDriver.Navigate().GoToUrl("about:blank");

        // Truncate data in database
        //SqlCommand command = new SqlCommand("TRUNCATE TABLE [dbo].[SupportTickets]", DbConnection());
        //command.Connection.Open();
        //command.ExecuteNonQuery();
    }

    public SqlConnection DbConnection()
    {
        var config = GetConfiguration();
        return new SqlConnection(config["ConnectionStrings:DefaultConnection"]);
    }

    // This is a combination of code from dotnet:
    // https://github.com/dotnet/aspnetcore/blob/8b30d862de6c9146f466061d51aa3f1414ee2337/src/DefaultBuilder/src/WebHost.cs#L171
    //
    // And code from our own host builder
    // https://github.com/abtion/care1-vci/blob/a34d975fb9178517370206bfaf4ce626a8302558/src/InfarePortal.FrontendReact/Program.cs#L22
    public IConfigurationRoot GetConfiguration()
    {
        if (Configuration != null) return Configuration;

        // From dotnet
        var config = new ConfigurationBuilder();

        config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.Test.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables("DOTNET_")
            .AddUserSecrets(System.Reflection.Assembly.GetExecutingAssembly(), optional: true, reloadOnChange: true);

        var args = Environment.GetCommandLineArgs();
        if (args != null)
        {
            config.AddCommandLine(args);
        }

        //// From our own hostbuilder
        //config.AddJsonFile("appsettings.Local.json", true, true)
        //      .AddJsonFile($"appsettings.Test.Local.json", true, true);

        Configuration = config.Build();

        return Configuration;
    }
}

[CollectionDefinition("Selenium")]
public class IntegrationCollectionFixture : ICollectionFixture<IntegrationFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}