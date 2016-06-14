using System;

using Aliencube.AdalWrapper;
using Aliencube.Azure.Insights.WebTests.Services.Settings;
using Aliencube.Azure.Insights.WebTests.Services.Tests.Fixtures;

using FluentAssertions;

using Microsoft.Azure;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

using Moq;

using Xunit;

namespace Aliencube.Azure.Insights.WebTests.Services.Tests
{
    /// <summary>
    /// This represents the test entity for the <see cref="WebTestService"/> class.
    /// </summary>
    public class WebTestServiceTest : IClassFixture<WebTestServiceFixture>
    {
        private readonly Mock<AuthenticationElement> _auth;
        private readonly Mock<ApplicationInsightsElement> _appInsights;
        private readonly WebTestElementCollection _webtests;
        private readonly Mock<IWebTestSettingsElement> _settings;
        private readonly Mock<IAuthenticationResultWrapper> _authResult;
        private readonly Mock<IAuthenticationContextWrapper> _authContext;

        private IWebTestService _service;

        /// <summary>
        /// Initialises a new instance of the <see cref="WebTestServiceTest"/> class.
        /// </summary>
        /// <param name="fixture"><see cref="WebTestServiceFixture"/> instance.</param>
        public WebTestServiceTest(WebTestServiceFixture fixture)
        {
            this._auth = fixture.AuthenticationElement;
            this._appInsights = fixture.ApplicationInsightsElement;
            this._webtests = fixture.WebTestElementCollection;
            this._settings = fixture.WebTestSettingsElement;
            this._authResult = fixture.AuthenticationResult;
            this._authContext = fixture.AuthenticationContext;
            this._service = fixture.WebTestService;
        }

        /// <summary>
        /// Tests whether the constructor should throw an exception or not.
        /// </summary>
        [Fact]
        public void Given_NullParameter_Constructor_ShouldThrow_Exception()
        {
            Action action = () => { var service = new WebTestService(null, this._authContext.Object); };
            action.ShouldThrow<ArgumentNullException>();

            action = () => { var service = new WebTestService(this._settings.Object, null); };
            action.ShouldThrow<ArgumentNullException>();
        }

        /// <summary>
        /// Tests whether the constructor should NOT throw an exception or not.
        /// </summary>
        [Fact]
        public void Given_Parameter_Constructor_ShouldThrow_NoException()
        {
            Action action = () => { var service = new WebTestService(this._settings.Object, this._authContext.Object); };
            action.ShouldNotThrow<Exception>();
        }

        /// <summary>
        /// Tests whether the method should return result or not.
        /// </summary>
        [Theory]
        [InlineData("ACCESS_TOKEN")]
        public async void Given_ClientCredential_GetCredentialsAsync_ShouldReturn_Result(string accessToken)
        {
            this._auth.SetupGet(p => p.ClientId).Returns("CLIENT_ID");
            this._auth.SetupGet(p => p.ClientSecret).Returns("CLIENT_SECRET");
            this._auth.SetupGet(p => p.UseServicePrinciple).Returns(true);
            this._auth.SetupGet(p => p.AadInstanceUrl).Returns("https://login.microsoftonline.com/");
            this._auth.SetupGet(p => p.ManagementInstanceUrl).Returns("https://management.core.windows.net/");

            this._appInsights.SetupGet(p => p.SubscriptionId).Returns("SUBSCRIPTION_ID");

            this._settings.SetupGet(p => p.Authentication).Returns(this._auth.Object);
            this._settings.SetupGet(p => p.ApplicationInsight).Returns(this._appInsights.Object);

            this._authResult.SetupGet(p => p.AccessToken).Returns(accessToken);
            this._authContext.Setup(p => p.AcquireTokenAsync(It.IsAny<string>(), It.IsAny<ClientCredential>())).ReturnsAsync(this._authResult.Object);

            this._service = new WebTestService(this._settings.Object, this._authContext.Object);

            var result = (await this._service.GetCredentialsAsync().ConfigureAwait(false)) as TokenCloudCredentials;
            result.Should().NotBeNull();
            result.SubscriptionId.Should().BeEquivalentTo(this._appInsights.Object.SubscriptionId);
            result.Token.Should().BeEquivalentTo(accessToken);
        }

        /// <summary>
        /// Tests whether the method should return result or not.
        /// </summary>
        [Theory]
        [InlineData("ACCESS_TOKEN")]
        public async void Given_UserCredential_GetCredentialsAsync_ShouldReturn_Result(string accessToken)
        {
            this._auth.SetupGet(p => p.ClientId).Returns("CLIENT_ID");
            this._auth.SetupGet(p => p.UseServicePrinciple).Returns(false);
            this._auth.SetupGet(p => p.Username).Returns("USERNAME");
            this._auth.SetupGet(p => p.Password).Returns("PASSWORD");
            this._auth.SetupGet(p => p.AadInstanceUrl).Returns("https://login.microsoftonline.com/");
            this._auth.SetupGet(p => p.ManagementInstanceUrl).Returns("https://management.core.windows.net/");

            this._appInsights.SetupGet(p => p.SubscriptionId).Returns("SUBSCRIPTION_ID");

            this._settings.SetupGet(p => p.Authentication).Returns(this._auth.Object);
            this._settings.SetupGet(p => p.ApplicationInsight).Returns(this._appInsights.Object);

            this._authResult.SetupGet(p => p.AccessToken).Returns(accessToken);
            this._authContext.Setup(p => p.AcquireTokenAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<UserCredential>())).ReturnsAsync(this._authResult.Object);

            this._service = new WebTestService(this._settings.Object, this._authContext.Object);

            var result = (await this._service.GetCredentialsAsync().ConfigureAwait(false)) as TokenCloudCredentials;
            result.Should().NotBeNull();
            result.SubscriptionId.Should().BeEquivalentTo(this._appInsights.Object.SubscriptionId);
            result.Token.Should().BeEquivalentTo(accessToken);
        }
    }
}