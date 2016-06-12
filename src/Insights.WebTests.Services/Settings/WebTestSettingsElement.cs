using System.Configuration;

namespace Aliencube.Azure.Insights.WebTests.Services.Settings
{
    /// <summary>
    /// This represents the configuration entity for azure web tests.
    /// </summary>
    public sealed class WebTestSettingsElement : ConfigurationSection, IWebTestSettingsElement
    {
        private bool _disposed;

        /// <summary>
        /// Gets or sets the <see cref="AuthenticationElement"/> instance.
        /// </summary>
        [ConfigurationProperty("authentication", IsRequired = true)]
        public AuthenticationElement Authentication
        {
            get { return (AuthenticationElement)this["authentication"]; }
            set { this["authentication"] = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="ApplicationInsightsElement"/> instance.
        /// </summary>
        [ConfigurationProperty("applicationInsights", IsRequired = true)]
        public ApplicationInsightsElement ApplicationInsight
        {
            get { return (ApplicationInsightsElement)this["applicationInsights"]; }
            set { this["applicationInsights"] = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="WebTestElementCollection"/> instance.
        /// </summary>
        [ConfigurationProperty("webTests", IsRequired = true)]
        public WebTestElementCollection WebTests
        {
            get { return (WebTestElementCollection)this["webTests"]; }
            set { this["webTests"] = value; }
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="WebTestSettingsElement"/> class.
        /// </summary>
        /// <returns>Returns the <see cref="IWebTestSettingsElement"/> instance.</returns>
        public static IWebTestSettingsElement CreateInstance()
        {
            var instance = ConfigurationManager.GetSection("webTestSettings") as WebTestSettingsElement;
            return instance;
        }

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