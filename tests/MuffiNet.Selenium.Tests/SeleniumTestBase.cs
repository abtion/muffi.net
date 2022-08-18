using Microsoft.Playwright;
using System;

namespace MuffiNet.FrontendReact.Selenium.Tests;

public class SeleniumTestBase : IDisposable
{
    protected IBrowserContext context { get; private init; }
    private bool isDisposed;
    private readonly IntegrationFixture integrationFixture;

    protected string siteUrl { get; private set; } = "https://localhost:4001/";


    // https://jeremydmiller.com/2018/08/27/a-way-to-use-docker-for-integration-tests/
    // https://www.roundthecode.com/dotnet/asp-net-core-web-api/asp-net-core-testserver-xunit-test-web-api-endpoints

    public SeleniumTestBase(IntegrationFixture integrationFixture)
    {
        this.integrationFixture = integrationFixture;

        var contextOptions = new BrowserNewContextOptions()
        {
            IgnoreHTTPSErrors = true,
        };

        context = integrationFixture.browser.NewContextAsync(contextOptions).Result;
    }

    public void Dispose()
    {
        // Dispose of unmanaged resources.
        Dispose(true);
        // Suppress finalization.
        GC.SuppressFinalize(this);

    }
    protected virtual void Dispose(bool disposing)
    {
        if (isDisposed) return;

        // free managed resources
        if (disposing)
        {
            // Truncate tables (except for users)
            integrationFixture.CleanUpBetweenTests();
        }

        // free unmanaged resources
        // https://docs.microsoft.com/en-us/dotnet/standard/garbage-collection/unmanaged

        isDisposed = true;
    }
}
