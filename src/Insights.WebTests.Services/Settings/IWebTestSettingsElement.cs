using System;

namespace Aliencube.Azure.Insights.WebTests.Services.Settings
{
    /// <summary>
    /// This provides the interfaces to the <see cref="WebTestSettingsElement"/> class.
    /// </summary>
    public interface IWebTestSettingsElement : IDisposable
    {
        /// <summary>
        /// Gets or sets the <see cref="AuthenticationElement"/> instance.
        /// </summary>
        AuthenticationElement Authentication { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ApplicationInsightsElement"/> instance.
        /// </summary>
        ApplicationInsightsElement ApplicationInsight { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="WebTestElementCollection"/> instance.
        /// </summary>
        WebTestElementCollection WebTests { get; set; }
    }
}