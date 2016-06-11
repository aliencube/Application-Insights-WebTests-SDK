using System;
using System.Linq;
using System.Net;
using System.Xml.Serialization;

using Aliencube.Azure.Insights.WebTests.Models.Exceptions;

namespace Aliencube.Azure.Insights.WebTests.Models.Serialisation
{
    /// <summary>
    /// This represents the web test item request entity to be serialised in the <see cref="WebTestConfiguration"/> class.
    /// </summary>
    public class WebTestItemRequest
    {
        private const string Get = "GET";
        private const string Utf8 = "utf-8";

        /// <summary>
        /// Initialises a new instance of the <see cref="WebTestItemRequest"/> class.
        /// </summary>
        public WebTestItemRequest()
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="WebTestItemRequest"/> class.
        /// </summary>
        /// <param name="url">Test URL.</param>
        /// <param name="timeout">Timeout value.</param>
        /// <param name="parseDependentRequests">Value indicating whether to parse dependent requests or not.</param>
        /// <param name="expectedHttpStatusCode">Expected HTTP status code. This SHOULD be <c>0</c>, if it's not required.</param>
        public WebTestItemRequest(string url, int timeout, bool parseDependentRequests, int expectedHttpStatusCode)
        {
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

            if (expectedHttpStatusCode != 0
                && Enum.GetValues(typeof(HttpStatusCode)).Cast<int>().All(p => p != expectedHttpStatusCode))
            {
                throw new InvalidHttpStatusCodeException();
            }

            this.Method = Get;
            this.Guid = Guid.NewGuid();
            this.Version = 1.1M;
            this.Url = url;
            this.ThinkTime = 0;
            this.Timeout = timeout;
            this.ParseDependentRequests = parseDependentRequests;
            this.FollowRedirects = true;
            this.RecordResult = true;
            this.Cache = false;
            this.ResponseTimeGoal = 0;
            this.Encoding = Utf8;
            this.ExpectedHttpStatusCode = expectedHttpStatusCode;
            this.ExpectedResponseUrl = string.Empty;
            this.ReportingName = string.Empty;
            this.IgnoreHttpStatusCode = false;
        }

        /// <summary>
        /// Gets the method. This is always <c>String.Empty</c>.
        /// </summary>
        [XmlAttribute()]
        public string Method { get; }

        /// <summary>
        /// Gets the GUID.
        /// </summary>
        [XmlAttribute()]
        public Guid Guid { get; }

        /// <summary>
        /// Gets the version. This is always <c>1.1</c>.
        /// </summary>
        [XmlAttribute()]
        public decimal Version { get; }

        /// <summary>
        /// Gets the test URL.
        /// </summary>
        [XmlAttribute()]
        public string Url { get; }

        /// <summary>
        /// Gets the think time. This is always <c>0</c>.
        /// </summary>
        [XmlAttribute()]
        public int ThinkTime { get; }

        /// <summary>
        /// Gets the timeout.
        /// </summary>
        [XmlAttribute()]
        public int Timeout { get; }

        /// <summary>
        /// Gets the value indicating whether to parse dependent requests or not.
        /// </summary>
        [XmlAttribute()]
        public bool ParseDependentRequests { get; }

        /// <summary>
        /// Gets the value indicating whether to follow redirects or not. This is always <c>True</c>.
        /// </summary>
        [XmlAttribute()]
        public bool FollowRedirects { get; }

        /// <summary>
        /// Gets the value indicating whether to record results or not. This is always <c>True</c>.
        /// </summary>
        [XmlAttribute()]
        public bool RecordResult { get; }

        /// <summary>
        /// Gets the value indicating whether to cache or not. This is always <c>False</c>.
        /// </summary>
        [XmlAttribute()]
        public bool Cache { get; }

        /// <summary>
        /// Gets the response time goal. This is always <c>0</c>.
        /// </summary>
        [XmlAttribute()]
        public int ResponseTimeGoal { get; }

        /// <summary>
        /// Gets the encoding. This is always <c>utf-8</c>.
        /// </summary>
        [XmlAttribute()]
        public string Encoding { get; }

        /// <summary>
        /// Gets the expected HTTP status code.
        /// </summary>
        [XmlAttribute()]
        public int ExpectedHttpStatusCode { get; }

        /// <summary>
        /// Gets the expected response URL. This is always <c>String.Empty</c>.
        /// </summary>
        [XmlAttribute()]
        public string ExpectedResponseUrl { get; }

        /// <summary>
        /// Gets the reporting name. This is always <c>String.Empty</c>.
        /// </summary>
        [XmlAttribute()]
        public string ReportingName { get; }

        /// <summary>
        /// Gets the value indicating whether to ignore HTTP status code or not. This is always <c>False</c>.
        /// </summary>
        [XmlAttribute()]
        public bool IgnoreHttpStatusCode { get; }
    }
}