using System;
using System.Net;

using Microsoft.VisualStudio.TestTools.WebTesting;

namespace Aliencube.Azure.Insights.WebTests.Models
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

    /// <summary>
    /// This represents the configuration entity for ping web test.
    /// </summary>
    public class PingWebTestConfiguration : WebTestConfiguration
    {
        private DeclarativeWebTest _test;

        public PingWebTestConfiguration(string url, HttpStatusCode expectedHttpStatusCode = HttpStatusCode.OK)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            this.Initialise(url, expectedHttpStatusCode);

            this._test = new DeclarativeWebTest() { Proxy = "default" };
        }

        /// <summary>
        /// Gets the web test XML serialised value.
        /// </summary>
        public override string WebTest => this._test.ToXml();

        private void Initialise(string url, HttpStatusCode expectedHttpStatusCode)
        {
            var request = new WebTestRequest(url) { ExpectedHttpStatusCode = (int)expectedHttpStatusCode };
            this._test.Items.Add(request);
        }
    }

    /// <summary>
    /// This represents the configuration entity for multi-step web test.
    /// </summary>
    public class MultiStepWebTestConfiguration : WebTestConfiguration
    {
        public override string WebTest { get; }
    }
}