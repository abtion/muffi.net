using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Linq;
using Xunit;
using FluentAssertions;

namespace MuffiNet.FrontendReact.Selenium.Tests
{
    [Collection("Selenium")]
    public class LoginLogoutTests : SeleniumTestBase
    {
        public LoginLogoutTests(IntegrationFixture integrationFixture) : base(integrationFixture)
        {
            // skip
        }

        [Fact]
        public void Given_UserExists_When_LoggingIn_Then_UserIsLoggedInAndRedirected()
        {
            webDriver.Navigate().GoToUrl($"{siteUrl}technician");

            wait().Until(webDriver => webDriver.FindElement(By.TagName("main")));

            webDriver.Url.Should().StartWith($"{siteUrl}Identity/Account/Login?");

            var emailInput = webDriver.FindElement(By.Id("Input_Email"));
            emailInput.Clear();
            emailInput.SendKeys("care1@abtion.com");

            var passwordInput = webDriver.FindElement(By.Id("Input_Password"));
            passwordInput.Clear();
            passwordInput.SendKeys("Test1234!");

            var submitButton = webDriver.FindElement(By.TagName("form")).FindElement(By.TagName("button"));
            submitButton.Click();

            wait().Until(webDriver => webDriver.FindElement(By.TagName("h1")));

            webDriver.Url.Should().StartWith($"{siteUrl}technician");
        }

        [Fact]
        public void Given_UserDoesNotExist_When_LoggingIn_Then_UserIsNotLoggedIn()
        {
            webDriver.Navigate().GoToUrl($"{siteUrl}technician");

            wait().Until(webDriver => webDriver.FindElement(By.TagName("main")));

            webDriver.Url.Should().StartWith($"{siteUrl}Identity/Account/Login?");

            var emailInput = webDriver.FindElement(By.Id("Input_Email"));
            emailInput.Clear();
            emailInput.SendKeys("care2@abtion.com");

            var passwordInput = webDriver.FindElement(By.Id("Input_Password"));
            passwordInput.Clear();
            passwordInput.SendKeys("Test1234!");

            var submitButton = webDriver.FindElement(By.TagName("form")).FindElement(By.TagName("button"));
            submitButton.Click();

            wait().Until(webDriver => webDriver.FindElement(By.TagName("h1")));

            webDriver.Url.Should().StartWith($"{siteUrl}Identity/Account/Login?");
        }

        [Fact]
        public void Given_UserExistsAndIsLoggedIn_When_LoggingOut_Then_UserIsLoggedOutAndRedirected()
        {
            // log in
            webDriver.Navigate().GoToUrl($"{siteUrl}technician");

            wait().Until(webDriver => webDriver.FindElement(By.TagName("main")));

            webDriver.Url.Should().StartWith($"{siteUrl}Identity/Account/Login?");

            var emailInput = webDriver.FindElement(By.Id("Input_Email"));
            emailInput.Clear();
            emailInput.SendKeys("care1@abtion.com");

            var passwordInput = webDriver.FindElement(By.Id("Input_Password"));
            passwordInput.Clear();
            passwordInput.SendKeys("Test1234!");

            var submitButton = webDriver.FindElement(By.TagName("form")).FindElement(By.TagName("button"));
            submitButton.Click();

            wait().Until(webDriver => webDriver.FindElement(By.TagName("h1")));

            // find logout button
            var logoutButton = webDriver.FindElement(By.TagName("header")).FindElements(By.TagName("a")).Where(p => p.Text == "Logout").Single();

            // Act
            logoutButton.Click();

            // check
            wait().Until(webDriver => webDriver.FindElement(By.TagName("h1")));

            webDriver.Url.Should().StartWith($"{siteUrl}Identity/Account/Login?");
        }
    }
}