using System;

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
            this.AuthenticationElement = new AuthenticationElement()
                                             {
                                                 ClientId = "[CLIENT_ID]",
                                                 ClientSecret = "[CLIENT_SECRET]",
                                                 UseServicePrinciple = true,
                                                 TenantName = "[TENANT_NAME]",
                                                 AadInstanceUrl = "https://login.microsoftonline.com/",
                                                 ManagementInstanceUrl = "https://management.core.windows.net/",
                                             };

            this.ApplicationInsightsElement = new ApplicationInsightsElement();

            this.WebTestElementCollection = new WebTestElementCollection
                                                {
                                                    new WebTestElement() { TestType = TestType.UrlPingTest },
                                                    new WebTestElement() { TestType = TestType.MultiStepTest },
                                                };

            this.WebTestSettingsElement = new Mock<IWebTestSettingsElement>();
            this.WebTestSettingsElement.SetupGet(p => p.Authentication).Returns(this.AuthenticationElement);
            this.WebTestSettingsElement.SetupGet(p => p.ApplicationInsight).Returns(this.ApplicationInsightsElement);
            this.WebTestSettingsElement.SetupGet(p => p.WebTests).Returns(this.WebTestElementCollection);

            this.WebTestService = new WebTestService(this.WebTestSettingsElement.Object);
        }

        /// <summary>
        /// Gets the <see cref="AuthenticationElement"/> instance.
        /// </summary>
        public AuthenticationElement AuthenticationElement { get; }

        /// <summary>
        /// Gets the <see cref="ApplicationInsightsElement"/> instance.
        /// </summary>
        public ApplicationInsightsElement ApplicationInsightsElement { get; }

        /// <summary>
        /// Gets the <see cref="WebTestElementCollection"/> instance.
        /// </summary>
        public WebTestElementCollection WebTestElementCollection { get; }

        /// <summary>
        /// Gets the <see cref="Mock{IWebTestSettingsElement}"/> instance.
        /// </summary>
        public Mock<IWebTestSettingsElement> WebTestSettingsElement { get; }

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