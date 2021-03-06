﻿using System;

using Aliencube.AdalWrapper;
using Aliencube.Azure.Insights.WebTests.Services.Settings;

using Microsoft.Azure.Management.Insights;
using Microsoft.Azure.Management.Resources;

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

            this.WebTestElement = new Mock<WebTestElement>();

            this.WebTestElementCollection = new WebTestElementCollection();

            this.WebTestSettingsElement = new Mock<IWebTestSettingsElement>();
            this.WebTestSettingsElement.SetupGet(p => p.Authentication).Returns(this.AuthenticationElement.Object);
            this.WebTestSettingsElement.SetupGet(p => p.ApplicationInsight).Returns(this.ApplicationInsightsElement.Object);
            this.WebTestSettingsElement.SetupGet(p => p.WebTests).Returns(this.WebTestElementCollection);

            this.AuthenticationResult = new Mock<IAuthenticationResultWrapper>();

            this.AuthenticationContext = new Mock<IAuthenticationContextWrapper>();

            this.ResourceOperations = new Mock<IResourceOperations>();

            this.AlertOperations = new Mock<IAlertOperations>();

            this.ResourceManagementClient = new Mock<IResourceManagementClient>();

            this.InsightsManagementClient = new Mock<IInsightsManagementClient>();

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
        /// Gets the <see cref="Mock{WebTestElement}"/> instance.
        /// </summary>
        public Mock<WebTestElement> WebTestElement { get; }

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
        /// Gets the <see cref="Mock{IResourceOperations}"/> instance.
        /// </summary>
        public Mock<IResourceOperations> ResourceOperations { get; }

        /// <summary>
        /// Gets the <see cref="Mock{IAlertOperations}"/> instance.
        /// </summary>
        public Mock<IAlertOperations> AlertOperations { get; }

        /// <summary>
        /// Gets the <see cref="Mock{IResourceManagementClient}"/> instance.
        /// </summary>
        public Mock<IResourceManagementClient> ResourceManagementClient { get; }

        /// <summary>
        /// Gets the <see cref="Mock{IInsightsManagementClient}"/> instance.
        /// </summary>
        public Mock<IInsightsManagementClient> InsightsManagementClient { get; }

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