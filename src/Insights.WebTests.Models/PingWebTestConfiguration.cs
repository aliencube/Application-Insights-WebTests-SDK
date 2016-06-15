using System;
using System.Net;

using Aliencube.Azure.Insights.WebTests.Models.Exceptions;
using Aliencube.Azure.Insights.WebTests.Models.Extensions;
using Aliencube.Azure.Insights.WebTests.Models.Serialisation;

namespace Aliencube.Azure.Insights.WebTests.Models
{
    /// <summary>
    /// This represents the configuration entity for ping web test.
    /// </summary>
    public class PingWebTestConfiguration : WebTestConfiguration
    {
        private readonly WebTest _test;

        /// <summary>
        /// Initialises a new instance of the <see cref="PingWebTestConfiguration"/> class.
        /// </summary>
        /// <param name="name">Web test name.</param>
        /// <param name="url">Test URL.</param>
        /// <param name="timeout">Timeout value.</param>
        /// <param name="parseDependentRequests">Value indicating whether to parse dependent requests or not.</param>
        /// <param name="expectedHttpStatusCode">Expected HTTP status code. This SHOULD be <c>0</c>, if it's not required.</param>
        /// <param name="text">Text to find in the validation rule.</param>
        public PingWebTestConfiguration(string name, string url, int timeout, bool parseDependentRequests, int expectedHttpStatusCode, string text = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            if (timeout <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(timeout));
            }

            if (expectedHttpStatusCode < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(expectedHttpStatusCode));
            }

            if (expectedHttpStatusCode != 0 && !Enum.IsDefined(typeof(HttpStatusCode), expectedHttpStatusCode))
            {
                throw new InvalidHttpStatusCodeException();
            }

            this._test = new WebTest(name, url, timeout, parseDependentRequests, expectedHttpStatusCode, text);
        }

        /// <summary>
        /// Gets the web test XML serialised value.
        /// </summary>
        public override string WebTest => this._test.ToXml();
    }
}