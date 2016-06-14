using System;
using System.Linq;

using Aliencube.AdalWrapper;
using Aliencube.Azure.Insights.WebTests.Models.Options;
using Aliencube.Azure.Insights.WebTests.Services.Settings;

using Moq;

namespace Aliencube.Azure.Insights.WebTests.Services.Tests.Fixtures
{
    /// <summary>
    /// This represents the fixture entity for the <see cref="WebTestServiceTest"/> class.s
    /// </summary>
    public class WebTestServiceFixture : IDisposable
    {
        private bool _disposed;

        /// <summary>
        /// Initialises a new instance of the <see cref="WebTestServiceFixture"/> class.
        /// </summary>
        public WebTestServiceFixture()
        {
            this.AuthenticationElement = new Mock<AuthenticationElement>();

            this.ApplicationInsightsElement = new Mock<ApplicationInsightsElement>();

            this.WebTestElementCollection = new WebTestElementCollection();

            this.WebTestSettingsElement = new Mock<IWebTestSettingsElement>();
            this.WebTestSettingsElement.SetupGet(p => p.Authentication).Returns(this.AuthenticationElement.Object);
            this.WebTestSettingsElement.SetupGet(p => p.ApplicationInsight).Returns(this.ApplicationInsightsElement.Object);
            this.WebTestSettingsElement.SetupGet(p => p.WebTests).Returns(this.WebTestElementCollection);

            this.AuthenticationResult = new Mock<IAuthenticationResultWrapper>();

            this.AuthenticationContext = new Mock<IAuthenticationContextWrapper>();

            this.WebTestService = new WebTestService(this.WebTestSettingsElement.Object, this.AuthenticationContext.Object);
        }

        /// <summary>
        /// Gets the <see cref="Mock{AuthenticationElement}"/> instance.
        /// </summary>
        public Mock<AuthenticationElement> AuthenticationElement { get; }

        /// <summary>
        /// Gets the <see cref="Mock{ApplicationInsightsElement}"/> instance.
        /// </summary>
        public Mock<ApplicationInsightsElement> ApplicationInsightsElement { get; }

        /// <summary>
        /// Gets the <see cref="Mock{WebTestElementCollection}"/> instance.
        /// </summary>
        public WebTestElementCollection WebTestElementCollection { get; }

        /// <summary>
        /// Gets the <see cref="Mock{IWebTestSettingsElement}"/> instance.
        /// </summary>
        public Mock<IWebTestSettingsElement> WebTestSettingsElement { get; }

        /// <summary>
        /// Gets the <see cref="Mock{IAuthenticationResultWrapper}"/> instance.
        /// </summary>
        public Mock<IAuthenticationResultWrapper> AuthenticationResult { get; }

        /// <summary>
        /// Gets the <see cref="Mock{IAuthenticationContextWrapper}"/> instance.
        /// </summary>
        public Mock<IAuthenticationContextWrapper> AuthenticationContext { get; }

        /// <summary>
        /// Gets the <see cref="IWebTestService"/> instance.
        /// </summary>
        public IWebTestService WebTestService { get; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (this._disposed)
            {
                return;
            }

            this._disposed = true;
        }
    }
}