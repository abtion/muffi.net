using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using Xunit;
using FluentAssertions;

namespace WebAppReact.Selenium.Tests
{
    [Collection("Selenium")]
    public class CustomerLandingPageTests : SeleniumTestBase
    {
        public CustomerLandingPageTests(IntegrationFixture integrationFixture) : base(integrationFixture)
        {
            // skip
        }

        [Fact]
        public void Given_SystemIsRunning_When_TheLandingPageIsCalled_Then_ThePageIsLoaded()
        {
            webDriver.Navigate().GoToUrl(siteUrl);
            Assert.True(true);
        }

        [Fact]
        public void Given_SystemIsRunning_When_TheLandingPageIsCall_Then_ThePageIsShown()
        {
            webDriver.Navigate().GoToUrl(siteUrl);

            wait().Until(webDriver => webDriver.FindElement(By.TagName("h1")));

            webDriver.Url.Should().Be($"{siteUrl}care1");
            webDriver.Title.Should().Be("VCI");

            var leadEl = webDriver.FindElement(By.TagName("h1"));
            leadEl.Text.Should().StartWith("Altid lige ved hånden");

            var continueInput = webDriver.FindElement(By.TagName("button"));
            continueInput.Displayed.Should().Be(true);
            continueInput.Text.Should().Be("Start Live Service");
        }

        [Theory]
        [InlineData("care1", "care1")]
        [InlineData("codan", "codan")]
        [InlineData("apple", "apple")]
        [InlineData("samsung", "samsung")]
        [InlineData("telia", "telia")]
        [InlineData("telenor", "telenor")]
        [InlineData("gjensidige", "gjensidige")]
        [InlineData("abtion", "care1")]
        [InlineData("coop", "care1")]
        public void Given_ACustomerSpecificUrlIsUsed_When_TheLandingPageIsCall_Then_TheUrlIsRedirectedToTheCorrectClient(string clientPostfix, string expectedRedirect)
        {
            webDriver.Navigate().GoToUrl($"{siteUrl}{clientPostfix}");

            wait().Until(webDriver => webDriver.FindElement(By.TagName("h1")));

            webDriver.Url.Should().Be($"{siteUrl}{expectedRedirect}");
        }

        [Theory]
        [InlineData("care1", "Care1Logo")]
        [InlineData("codan", "CodanLogo")]
        [InlineData("apple", "AppleLogo")]
        [InlineData("samsung", "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAABVwAAADx")] // Samsung Logo is small and included inline "SamsungLogo"
        [InlineData("telia", "TeliaLogo")]
        [InlineData("telenor", "TelenorLogo")]
        [InlineData("gjensidige", "GjensidigeLogo")]
        [InlineData("abtion", "Care1Logo")]
        [InlineData("coop", "Care1Logo")]
        public void Given_ACustomerSpecificUrlIsUsed_When_TheLandingPageIsCall_Then_TheImageWhichIsShownIsTheRightOne(string clientPostfix, string logoFilename)
        {
            webDriver.Navigate().GoToUrl($"{siteUrl}{clientPostfix}");

            wait().Until(webDriver => webDriver.FindElement(By.TagName("h1")));

            var logo = webDriver.FindElement(By.TagName("img"));
            var src = logo.GetAttribute("src");

            if (src.StartsWith("data:image/png;base64"))
            {
                // added to take care of Samsung exception
                src.Should().Contain($"{logoFilename}");
                src.Should().Contain($"png");
            }
            else
            {
                src.Should().Contain($"assets/{logoFilename}.");
                src.Should().Contain($".png");
            }
        }
    }
}