using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using Xunit;
using FluentAssertions;

namespace MuffiNet.FrontendReact.Selenium.Tests
{
    [Collection("Selenium")]
    public class LandingPageTests : SeleniumTestBase
    {
        public LandingPageTests(IntegrationFixture integrationFixture) : base(integrationFixture)
        {
            // skip
        }

        [Fact]
        public void Given_SystemIsRunning_When_TheLandingPageIsCall_Then_ThePageIsShown()
        {
            webDriver.Navigate().GoToUrl(siteUrl);

            wait().Until(webDriver => webDriver.FindElement(By.TagName("h1")));

            webDriver.Url.Should().Be($"{siteUrl}");
        }
    }
}