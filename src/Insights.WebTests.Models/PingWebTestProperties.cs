﻿namespace Aliencube.Azure.Insights.WebTests.Models
{
    /// <summary>
    /// This represents the properties entity for ping web test.
    /// </summary>
    public class PingWebTestProperties : WebTestProperties
    {
        /// <summary>
        /// Gets or sets the <see cref="PingWebTestConfiguration"/> object.
        /// </summary>
        public override WebTestConfiguration Configuration { get; set; }
    }
}