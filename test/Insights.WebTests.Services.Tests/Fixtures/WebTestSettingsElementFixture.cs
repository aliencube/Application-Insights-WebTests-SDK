using System;

using Aliencube.Azure.Insights.WebTests.Services.Settings;

namespace Aliencube.Azure.Insights.WebTests.Services.Tests.Fixtures
{
    /// <summary>
    /// This represents the fixture entity for the <see cref="WebTestSettingsElementTest"/> class.
    /// </summary>
    public class WebTestSettingsElementFixture : IDisposable
    {
        private bool _disposed;

        /// <summary>
        /// Initialises a new instance of the <see cref="WebTestSettingsElementFixture"/> class.
        /// </summary>
        public WebTestSettingsElementFixture()
        {
            this.WebTestSettingsElement = Settings.WebTestSettingsElement.CreateInstance();
        }

        /// <summary>
        /// Gets the <see cref="IWebTestSettingsElement"/> instance.
        /// </summary>
        public IWebTestSettingsElement WebTestSettingsElement { get; }

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