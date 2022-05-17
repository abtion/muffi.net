using FluentAssertions;
using OpenQA.Selenium;
using Xunit;

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

            Wait().Until(webDriver => webDriver.FindElement(By.TagName("h1")));

            webDriver.Url.Should().Be($"{siteUrl}");
        }
    }
}