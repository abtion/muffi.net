using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using Xunit;
using FluentAssertions;

namespace MuffiNet.FrontendReact.Selenium.Tests
{
    [Collection("Selenium")]
    public class CreateTicketTests : SeleniumTestBase
    {
        public CreateTicketTests(IntegrationFixture integrationFixture) : base(integrationFixture)
        {
            // skip
        }

        [Fact]
        public void Given_SystemIsRunning_When_TheCreatingSupportTicket_Then_TheWaitingPageIsLoaded()
        {
            // Arrange
            webDriver.Navigate().GoToUrl(siteUrl);

            wait().Until(webDriver => webDriver.FindElement(By.TagName("h1")));

            var continueInput = webDriver.FindElement(By.TagName("button"));
            continueInput.Click();

            // Act
            var nameInput = webDriver.FindElement(By.Id("nameInput"));
            nameInput.Clear();
            nameInput.SendKeys("End-to-end test");

            var emailInput = webDriver.FindElement(By.Id("emailInput"));
            emailInput.Clear();
            emailInput.SendKeys("care1@abtion.com");

            var phoneInput = webDriver.FindElement(By.Id("phoneInput"));
            phoneInput.Clear();
            phoneInput.SendKeys("12345678");

            var brandSelect = webDriver.FindElement(By.Id("brandSelect"));
            var selectElement = new SelectElement(brandSelect);
            selectElement.SelectByValue("apple");

            var recordConsent = webDriver.FindElement(By.Id("recordConsent"));
            recordConsent.Click();

            var privacyPolicyConsent = webDriver.FindElement(By.Id("privacyPolicyConsent"));
            privacyPolicyConsent.Click();
            var submitButton = webDriver.FindElement(By.TagName("form")).FindElement(By.TagName("button"));
            submitButton.Click();

            // Check
            wait().Until(webDriver => webDriver.FindElement(By.TagName("h1")));

            var welcomeHeader = webDriver.FindElement(By.TagName("h1"));
            welcomeHeader.Text.Should().Be("Velkommen til Care1 Live");
        }

        [Fact]
        public void Given_ConsentIsNotChecked_When_TheCreatingSupportTicket_Then_ErrorMessageShouldBeShown()
        {
            // Arrange
            webDriver.Navigate().GoToUrl(siteUrl);

            wait().Until(webDriver => webDriver.FindElement(By.TagName("h1")));

            var continueInput = webDriver.FindElement(By.TagName("button"));
            continueInput.Click();

            // Act
            var nameInput = webDriver.FindElement(By.Id("nameInput"));
            nameInput.Clear();
            nameInput.SendKeys("End-to-end test");

            var emailInput = webDriver.FindElement(By.Id("emailInput"));
            emailInput.Clear();
            emailInput.SendKeys("care1@abtion.com");

            var phoneInput = webDriver.FindElement(By.Id("phoneInput"));
            phoneInput.Clear();
            phoneInput.SendKeys("12345678");

            var brandSelect = webDriver.FindElement(By.Id("brandSelect"));
            var selectElement = new SelectElement(brandSelect);
            selectElement.SelectByValue("apple");

            var privacyPolicyConsent = webDriver.FindElement(By.Id("privacyPolicyConsent"));
            privacyPolicyConsent.Click();
            var submitButton = webDriver.FindElement(By.TagName("form")).FindElement(By.TagName("button"));
            submitButton.Click();

            // Check
            wait().Until(webDriver => webDriver.FindElement(By.TagName("form")));
            webDriver.Url.Should().Be($"{siteUrl}care1/sign-up");
        }
    }
}