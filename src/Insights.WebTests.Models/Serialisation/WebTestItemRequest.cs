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

            this.Initialise();

            this.Url = url;
            this.Timeout = timeout;
            this.ParseDependentRequests = parseDependentRequests;
            this.ExpectedHttpStatusCode = expectedHttpStatusCode;
        }

        /// <summary>
        /// Gets or sets the method. This is always <c>String.Empty</c>.
        /// </summary>
        [XmlAttribute()]
        public string Method { get; set; }

        /// <summary>
        /// Gets or sets the GUID.
        /// </summary>
        [XmlAttribute()]
        public Guid Guid { get; set; }

        /// <summary>
        /// Gets or sets the version. This is always <c>1.1</c>.
        /// </summary>
        [XmlAttribute()]
        public decimal Version { get; set; }

        /// <summary>
        /// Gets or sets the test URL.
        /// </summary>
        [XmlAttribute()]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the think time. This is always <c>0</c>.
        /// </summary>
        [XmlAttribute()]
        public int ThinkTime { get; set; }

        /// <summary>
        /// Gets or sets the timeout.
        /// </summary>
        [XmlAttribute()]
        public int Timeout { get; set; }

        /// <summary>
        /// Gets or sets the value indicating whether to parse dependent requests or not.
        /// </summary>
        [XmlAttribute()]
        public bool ParseDependentRequests { get; set; }

        /// <summary>
        /// Gets or sets the value indicating whether to follow redirects or not. This is always <c>True</c>.
        /// </summary>
        [XmlAttribute()]
        public bool FollowRedirects { get; set; }

        /// <summary>
        /// Gets or sets the value indicating whether to record results or not. This is always <c>True</c>.
        /// </summary>
        [XmlAttribute()]
        public bool RecordResult { get; set; }

        /// <summary>
        /// Gets or sets the value indicating whether to cache or not. This is always <c>False</c>.
        /// </summary>
        [XmlAttribute()]
        public bool Cache { get; set; }

        /// <summary>
        /// Gets or sets the response time goal. This is always <c>0</c>.
        /// </summary>
        [XmlAttribute()]
        public int ResponseTimeGoal { get; set; }

        /// <summary>
        /// Gets or sets the encoding. This is always <c>utf-8</c>.
        /// </summary>
        [XmlAttribute()]
        public string Encoding { get; set; }

        /// <summary>
        /// Gets or sets the expected HTTP status code.
        /// </summary>
        [XmlAttribute()]
        public int ExpectedHttpStatusCode { get; set; }

        /// <summary>
        /// Gets or sets the expected response URL. This is always <c>String.Empty</c>.
        /// </summary>
        [XmlAttribute()]
        public string ExpectedResponseUrl { get; set; }

        /// <summary>
        /// Gets or sets the reporting name. This is always <c>String.Empty</c>.
        /// </summary>
        [XmlAttribute()]
        public string ReportingName { get; set; }

        /// <summary>
        /// Gets or sets the value indicating whether to ignore HTTP status code or not. This is always <c>False</c>.
        /// </summary>
        [XmlAttribute()]
        public bool IgnoreHttpStatusCode { get; set; }

        private void Initialise()
        {
            this.Method = Get;
            this.Guid = Guid.NewGuid();
            this.Version = 1.1M;
            this.ThinkTime = 0;
            this.FollowRedirects = true;
            this.RecordResult = true;
            this.Cache = false;
            this.ResponseTimeGoal = 0;
            this.Encoding = Utf8;
            this.ExpectedResponseUrl = string.Empty;
            this.ReportingName = string.Empty;
            this.IgnoreHttpStatusCode = false;
        }
    }
}