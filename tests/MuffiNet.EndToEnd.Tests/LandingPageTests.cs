using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace MuffiNet.FrontendReact.Selenium.Tests;

[Collection("Selenium")]
public class LandingPageTests : SeleniumTestBase
{
    public LandingPageTests(IntegrationFixture integrationFixture) : base(integrationFixture)
    {
        // skip
    }

    [Fact]
    public async Task Given_SystemIsRunning_When_TheLandingPageIsCall_Then_ThePageIsShown()
    {
        var page = await context.NewPageAsync();

        await page.GotoAsync(siteUrl);

        await page.WaitForSelectorAsync("h1");

        page.Url.Should().Be(siteUrl);
    }
}
