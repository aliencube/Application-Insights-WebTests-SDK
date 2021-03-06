﻿namespace Aliencube.Azure.Insights.WebTests.Models
{
    /// <summary>
    /// This represents the configuration entity for web test. This MUST be inherited.
    /// </summary>
    public abstract class WebTestConfiguration
    {
        /// <summary>
        /// Gets the web test XML serialised value.
        /// </summary>
        public abstract string WebTest { get; }
    }
}