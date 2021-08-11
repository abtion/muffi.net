using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static System.Net.Mime.MediaTypeNames;

namespace MuffiNet.FrontendReact.Selenium.Tests
{
    public class SeleniumTestBase : IDisposable
    {
        protected readonly IWebDriver webDriver;

        private readonly IntegrationFixture integrationFixture;

        protected string siteUrl = "https://localhost:4001/";


        // https://jeremydmiller.com/2018/08/27/a-way-to-use-docker-for-integration-tests/
        // https://www.roundthecode.com/dotnet/asp-net-core-web-api/asp-net-core-testserver-xunit-test-web-api-endpoints

        public SeleniumTestBase(IntegrationFixture integrationFixture)
        {
            this.integrationFixture = integrationFixture;
            webDriver = integrationFixture.webDriver;
        }

        protected WebDriverWait wait()
        {
            return new WebDriverWait(webDriver, TimeSpan.FromSeconds(10));
        }

        public void Dispose()
        {
            // Truncate tables (except for users)
            integrationFixture.CleanUpBetweenTests();
        }
    }
}