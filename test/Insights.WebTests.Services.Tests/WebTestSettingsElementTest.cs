using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Aliencube.Azure.Insights.WebTests.Models.Options;
using Aliencube.Azure.Insights.WebTests.Services.Settings;
using Aliencube.Azure.Insights.WebTests.Services.Tests.Fixtures;

using FluentAssertions;

using Xunit;

namespace Aliencube.Azure.Insights.WebTests.Services.Tests
{
    /// <summary>
    /// This represents the test entity for the <see cref="WebTestSettingsElement"/> class.
    /// </summary>
    public class WebTestSettingsElementTest : IClassFixture<WebTestSettingsElementFixture>
    {
        private readonly AuthenticationElement _auth;
        private readonly ApplicationInsightsElement _insights;
        private readonly List<WebTestElement> _webTests;

        /// <summary>
        /// Initialises a new instance of the <see cref="WebTestSettingsElementTest"/> class.
        /// </summary>
        /// <param name="fixture"><see cref="WebTestSettingsElementFixture"/> instance.</param>
        public WebTestSettingsElementTest(WebTestSettingsElementFixture fixture)
        {
            this._auth = fixture.WebTestSettingsElement.Authentication;
            this._insights = fixture.WebTestSettingsElement.ApplicationInsight;
            this._webTests = fixture.WebTestSettingsElement.WebTests.OfType<WebTestElement>().ToList();
        }

        /// <summary>
        /// Tests whether the <see cref="AuthenticationElement"/> instance should return results or not.
        /// </summary>
        [Fact]
        public void Given_AppConfig_Authentication_ShouldReturn_Results()
        {
            this._auth.ClientId.Should().BeEquivalentTo("[CLIENT_ID]");
            this._auth.ClientSecret.Should().BeEquivalentTo("[CLIENT_SECRET]");
            this._auth.UseServicePrinciple.Should().BeFalse();
            this._auth.TenantName.Should().BeEquivalentTo("[TENANT_NAME]");
        }

        /// <summary>
        /// Tests whether the <see cref="ApplicationInsightsElement"/> instance should return results or not.
        /// </summary>
        [Fact]
        public void Given_AppConfig_ApplicationInsights_ShouldReturn_Results()
        {
            this._insights.Name.Should().BeEquivalentTo("[APPLICATION_INSIGHTS_NAME]");
            this._insights.ResourceGroup.Should().BeEquivalentTo("[RESOURCE_GROUP_NAME]");
            this._insights.SubscriptionId.Should().BeEquivalentTo("[SUBSCRIPTION_ID]");
        }

        [Fact]
        public void Given_AppConfig_WebTests_Should_Return_Results()
        {
            this._webTests.Count.Should().Be(2);
            this._webTests.Should().Contain(p => p.TestType == TestType.UrlPingTest);
            this._webTests.Should().Contain(p => p.TestType == TestType.MultiStepTest);
        }
    }
}
