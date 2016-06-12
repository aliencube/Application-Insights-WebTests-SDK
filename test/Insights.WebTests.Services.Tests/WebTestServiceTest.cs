using System;

using Aliencube.Azure.Insights.WebTests.Services.Settings;
using Aliencube.Azure.Insights.WebTests.Services.Tests.Fixtures;

using FluentAssertions;

using Moq;

using Xunit;

namespace Aliencube.Azure.Insights.WebTests.Services.Tests
{
    /// <summary>
    /// This represents the test entity for the <see cref="WebTestService"/> class.
    /// </summary>
    public class WebTestServiceTest : IClassFixture<WebTestServiceFixture>
    {
        private readonly AuthenticationElement _auth;
        private readonly ApplicationInsightsElement _appInsights;
        private readonly WebTestElementCollection _webtests;
        private readonly Mock<IWebTestSettingsElement> _settings;
        private readonly IWebTestService _service;

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
            this._service = fixture.WebTestService;
        }

        [Fact]
        public void Given_NullParameter_Constructor_ShouldThrow_Exception()
        {
            Action action = () => { var service = new WebTestService(null); };
            action.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void Given_Parameter_Constructor_ShouldThrow_NoException()
        {
            Action action = () => { var service = new WebTestService(this._settings.Object); };
            action.ShouldNotThrow<Exception>();
        }
    }
}