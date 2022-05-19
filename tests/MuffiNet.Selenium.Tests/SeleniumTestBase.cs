using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace MuffiNet.FrontendReact.Selenium.Tests;

public class SeleniumTestBase : IDisposable
{
    protected IWebDriver webDriver { get; private set; }
    private bool isDisposed;
    private readonly IntegrationFixture integrationFixture;

    protected string siteUrl { get; private set; } = "https://localhost:4001/";


    // https://jeremydmiller.com/2018/08/27/a-way-to-use-docker-for-integration-tests/
    // https://www.roundthecode.com/dotnet/asp-net-core-web-api/asp-net-core-testserver-xunit-test-web-api-endpoints

    public SeleniumTestBase(IntegrationFixture integrationFixture)
    {
        this.integrationFixture = integrationFixture;
        webDriver = integrationFixture.webDriver;
    }

    protected WebDriverWait Wait()
    {
        return new WebDriverWait(webDriver, TimeSpan.FromSeconds(10));
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