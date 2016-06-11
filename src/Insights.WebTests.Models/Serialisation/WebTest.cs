using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml.Serialization;

using Aliencube.Azure.Insights.WebTests.Models.Exceptions;

namespace Aliencube.Azure.Insights.WebTests.Models.Serialisation
{
    /// <summary>
    /// This represents the web test entity to be serialised in the <see cref="WebTestConfiguration"/> class.
    /// </summary>
    [XmlRoot(Namespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010", IsNullable = false)]
    public class WebTest
    {
        private const string Default = "default";

        /// <summary>
        /// Initialises a new instance of the <see cref="WebTest"/> class.
        /// </summary>
        public WebTest()
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="WebTest"/> class.
        /// </summary>
        /// <param name="name">Web test name.</param>
        /// <param name="url">Test URL.</param>
        /// <param name="timeout">Timeout value.</param>
        /// <param name="parseDependentRequests">Value indicating whether to parse dependent requests or not.</param>
        /// <param name="expectedHttpStatusCode">Expected HTTP status code. This SHOULD be <c>0</c>, if it's not required.</param>
        /// <param name="text">Text to find in the validation rule.</param>
        public WebTest(string name, string url, int timeout, bool parseDependentRequests, int expectedHttpStatusCode, string text = null)
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

            if (expectedHttpStatusCode != 0
                && Enum.GetValues(typeof(HttpStatusCode)).Cast<int>().All(p => p != expectedHttpStatusCode))
            {
                throw new InvalidHttpStatusCodeException();
            }

            this.Name = name;
            this.Id = Guid.NewGuid();
            this.Enabled = true;
            this.CssProjectStructure = string.Empty;
            this.CssIteration = string.Empty;
            this.Timeout = timeout;
            this.WorkItemIds = string.Empty;
            this.Description = string.Empty;
            this.CredentialUserName = string.Empty;
            this.CredentialPassword = string.Empty;
            this.PreAuthenticate = true;
            this.Proxy = Default;
            this.StopOnError = false;
            this.RecordedResultFile = string.Empty;
            this.ResultsLocale = string.Empty;

            this.Items = new List<WebTestItemRequest>()
                             {
                                 new WebTestItemRequest(url, timeout, parseDependentRequests, expectedHttpStatusCode),
                             };

            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }

            this.ValidationRules = new List<WebTestValidationRule>()
                                       {
                                           new WebTestValidationRule(text),
                                       };
        }

        /// <summary>
        /// Gets the web test name.
        /// </summary>
        [XmlAttribute()]
        public string Name { get; }

        /// <summary>
        /// Gets the Id as GUID format.
        /// </summary>
        [XmlAttribute()]
        public Guid Id { get; }

        /// <summary>
        /// Gets the value indicating whether the test is enabled or not. This is always <c>True</c>.
        /// </summary>
        [XmlAttribute()]
        public bool Enabled { get; }

        /// <summary>
        /// Gets the CSS project structure. This is always <c>String.Empty</c>.
        /// </summary>
        [XmlAttribute()]
        public string CssProjectStructure { get; }

        /// <summary>
        /// Gets the CSS iteration. This is always <c>String.Empty</c>.
        /// </summary>
        [XmlAttribute()]
        public string CssIteration { get; }

        /// <summary>
        /// Gets the timeout value in seconds.
        /// </summary>
        [XmlAttribute()]
        public int Timeout { get; }

        /// <summary>
        /// Gets the list of work item Ids delimited by comma. This is always <c>String.Empty</c>.
        /// </summary>
        [XmlAttribute()]
        public string WorkItemIds { get; }

        /// <summary>
        /// Gets the description. This is always <c>String.Empty</c>.
        /// </summary>
        [XmlAttribute()]
        public string Description { get; }

        /// <summary>
        /// Gets the credential username. This is always <c>String.Empty</c>.
        /// </summary>
        [XmlAttribute()]
        public string CredentialUserName { get; }

        /// <summary>
        /// Gets the credential password. This is always <c>String.Empty</c>.
        /// </summary>
        [XmlAttribute()]
        public string CredentialPassword { get; }

        /// <summary>
        /// Gets the value indicating whether the test is pre-authenticated or not.  This is always <c>True</c>.
        /// </summary>
        [XmlAttribute()]
        public bool PreAuthenticate { get; }

        /// <summary>
        /// Gets the proxy value. This is always <c>default</c>.
        /// </summary>
        [XmlAttribute()]
        public string Proxy { get; }

        /// <summary>
        /// Gets the value indicating whether to stop on error or not. This is always <c>False</c>.
        /// </summary>
        [XmlAttribute()]
        public bool StopOnError { get; }

        /// <summary>
        /// Gets the result filename to be recorded. This is always <c>String.Empty</c>.
        /// </summary>
        [XmlAttribute()]
        public string RecordedResultFile { get; }

        /// <summary>
        /// Gets the result locale. This is always <c>String.Empty</c>.
        /// </summary>
        [XmlAttribute()]
        public string ResultsLocale { get; }

        /// <summary>
        /// Gets the list of the <see cref="WebTestItemRequest"/> objects.
        /// </summary>
        [XmlArray("Items", IsNullable = false)]
        [XmlArrayItem("Request", IsNullable = false)]
        public List<WebTestItemRequest> Items { get; }

        /// <summary>
        /// Gets the list of the <see cref="WebTestValidationRule"/> objects.
        /// </summary>
        [XmlArray("ValidationRules", IsNullable = true)]
        [XmlArrayItem("ValidationRule", IsNullable = false)]
        public List<WebTestValidationRule> ValidationRules { get; }
    }
}