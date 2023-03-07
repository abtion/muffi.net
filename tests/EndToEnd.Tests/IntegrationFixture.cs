using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using DomainModel.Data;

namespace EndToEnd.Tests;

/// <remarks>
/// https://xunit.net/docs/shared-context
/// </remarks>
public sealed class IntegrationFixture : IDisposable {
    private Process process;
    private IConfigurationRoot Configuration;
    internal IBrowser browser { get; private init; }

    public IntegrationFixture() {
        // The database in currently not in use - no need to drop, create and reseed
        //var task = Task.Run(() =>
        //{
        //    DropDatabase();
        //    CreateDatabase();
        //    SeedDatabase();
        //});

        //if (task.Wait(TimeSpan.FromSeconds(80)))
        //{
        browser = CreateBrowser().Result;
        process = StartServer();
        //}
        //else
        //{
        //    throw new TimeoutException("Database setup timed out");
        //}
    }

    private void DropDatabase() {
        var config = GetConfiguration();

        try {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseSqlServer(config.GetConnectionString("DefaultConnection"));

            using var context = new ApplicationDbContext(builder.Options);
            context.Database.EnsureDeletedAsync().Wait();

            Console.WriteLine("Dropping test database: success");
        }
        catch (Exception ex) {
            Console.WriteLine("Dropping test database: failure");
            Console.WriteLine(ex.ToString());

            throw new InvalidOperationException("Nonzero exit code when dropping test database", ex);
        }
    }

    private void SeedDatabase() {
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

    private void CreateDatabase() {
        var config = GetConfiguration();

        try {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseSqlServer(config.GetConnectionString("DefaultConnection"));

            using var context = new ApplicationDbContext(builder.Options);
            context.Database.EnsureCreatedAsync().Wait();

            Console.WriteLine("Creating/updating test database: success");
        }
        catch (Exception ex) {
            Console.WriteLine("Creating/updating test database: failure");
            Console.WriteLine(ex.ToString());

            throw new InvalidOperationException("Nonzero exit code when creating/updating test database", ex);
        }
    }

    private Process StartServer() {
        Console.WriteLine("Starting server");
        var process = Process.Start(CreateStartInfo("run --launch-profile \"MuffiNet.FrontendReact Test\""));

        string output;

        do {
            output = process.StandardOutput.ReadLine();

            if (output is not null && output.Contains("Now listening on", StringComparison.InvariantCultureIgnoreCase)) {
                return process; // server ready
            }
        }
        while (!process.StandardOutput.EndOfStream);

        throw new InvalidOperationException($"Error starting server: {process.StandardError.ReadToEnd()}");
    }

    private async Task<IBrowser> CreateBrowser() {
        var playwright = await Playwright.CreateAsync();

        return await playwright.Chromium.LaunchAsync();
    }

    private ProcessStartInfo CreateStartInfo(string arguments) {
        var startInfo = new ProcessStartInfo() {
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

    private string GetWorkingDirectory() {
        return Path.GetFullPath(
            "../../../../../src/MuffiNet.FrontendReact",
            Directory.GetCurrentDirectory()
        );
    }

    public void Dispose() {
        GC.SuppressFinalize(this);

        browser.DisposeAsync().AsTask().Wait();

        if (process != null && !process.HasExited) {
            process.Kill(entireProcessTree: true);
        }
    }

    public void CleanUpBetweenTests() {
        // Truncate data in database
        //SqlCommand command = new SqlCommand("TRUNCATE TABLE [dbo].[SupportTickets]", DbConnection());
        //command.Connection.Open();
        //command.ExecuteNonQuery();
    }

    public SqlConnection DbConnection() {
        var config = GetConfiguration();
        return new SqlConnection(config["ConnectionStrings:DefaultConnection"]);
    }

    // This is a combination of code from dotnet:
    // https://github.com/dotnet/aspnetcore/blob/8b30d862de6c9146f466061d51aa3f1414ee2337/src/DefaultBuilder/src/WebHost.cs#L171
    //
    // And code from our own host builder
    // https://github.com/abtion/care1-vci/blob/a34d975fb9178517370206bfaf4ce626a8302558/src/InfarePortal.FrontendReact/Program.cs#L22
    public IConfigurationRoot GetConfiguration() {
        if (Configuration != null) return Configuration;

        // From dotnet
        var config = new ConfigurationBuilder();

        config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile("appsettings.Test.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables("DOTNET_")
            .AddUserSecrets(System.Reflection.Assembly.GetExecutingAssembly(), optional: true, reloadOnChange: true);

        var args = Environment.GetCommandLineArgs();
        if (args != null) {
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
public class IntegrationCollectionFixture : ICollectionFixture<IntegrationFixture> {
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}