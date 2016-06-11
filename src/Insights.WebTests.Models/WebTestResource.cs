using System;

using Aliencube.Azure.Insights.WebTests.Models.Options;

using Microsoft.Azure;
using Microsoft.Azure.Management.Resources.Models;

namespace Aliencube.Azure.Insights.WebTests.Models
{
    /// <summary>
    /// This represents the resource entity for web test.
    /// </summary>
    public class WebTestResource : GenericResource
    {
        private const string ResourceType = "Microsoft.Insights/webtests";
        private const string ApiVersion = "2015-05-01";

        private readonly string _name;
        private readonly string _url;
        private readonly ResourceBaseExtended _insights;
        private readonly TestType _testType;

        private string _syntheticMonitorId;

        /// <summary>
        /// Initialises a new instance of the <see cref="WebTestResource"/> class.
        /// </summary>
        /// <param name="name">Web test name.</param>
        /// <param name="url">Test URL.</param>
        /// <param name="insights"><see cref="ResourceBaseExtended"/> instance representing Application Insights component.</param>
        /// <param name="testType">Type of the web test. Default value is <c>WebTestType.UriPingTest</c>. </param>
        public WebTestResource(string name, string url, ResourceBaseExtended insights, TestType testType = TestType.UrlPingTest)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            this._name = name;

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            this._url = url;

            if (insights == null)
            {
                throw new ArgumentNullException(nameof(insights));
            }

            this._insights = insights;

            this._testType = testType;

            this.Initialise();
        }

        /// <summary>
        /// Gets the <see cref="Microsoft.Azure.ResourceIdentity"/> instance.
        /// </summary>
        public ResourceIdentity ResourceIdentity { get; private set; }

        /// <summary>
        /// Creates a new instance of the web test properties inheriting the <see cref="WebTestProperties"/> class.
        /// </summary>
        /// <param name="testLocations"><see cref="TestLocations"/> value.</param>
        /// <param name="testStatus"><see cref="TestStatus"/> value.</param>
        /// <param name="testFrequency"><see cref="TestFrequency"/> value.</param>
        /// <param name="testTimeout"><see cref="TestTimeout"/> value.</param>
        /// <param name="parseDependentRequests">Value indicating whether to parse dependent requests or not.</param>
        /// <param name="retriesForWebTestFailure"><see cref="RetriesForWebTestFailure"/> value.</param>
        /// <param name="expectedHttpStatusCode">HTTP status code expected. Default value is <c>200</c></param>
        /// <param name="text">Text to find in the validation rule.</param>
        public void CreateWebTestProperties(TestLocations testLocations, TestStatus testStatus, TestFrequency testFrequency, TestTimeout testTimeout, bool parseDependentRequests, RetriesForWebTestFailure retriesForWebTestFailure, int expectedHttpStatusCode = 200, string text = null)
        {
            WebTestProperties properties;
            switch (this._testType)
            {
                case TestType.UrlPingTest:
                    properties = this.CreatePingWebTestProperties(testLocations, testStatus, testFrequency, testTimeout, parseDependentRequests, retriesForWebTestFailure, expectedHttpStatusCode, text);
                    break;

                case TestType.MultiStepTest:
                    properties = new MultiStepWebTestProperties();
                    break;

                default:
                    properties = null;
                    break;
            }

            this.Properties = properties;
        }

        private void Initialise()
        {
            this._syntheticMonitorId = $"{this._name}-{this._insights.Name}";
            this.ResourceIdentity = new ResourceIdentity(this._syntheticMonitorId, ResourceType, ApiVersion);

            this.Location = this._insights.Location;
            this.Tags.Add($"hidden-link:{this._insights.Id}", "Resource");
        }

        private PingWebTestProperties CreatePingWebTestProperties(TestLocations testLocations, TestStatus testStatus, TestFrequency testFrequency, TestTimeout testTimeout, bool parseDependentRequests, RetriesForWebTestFailure retriesForWebTestFailure, int expectedHttpStatusCode = 200, string text = null)
        {
            var properties = new PingWebTestProperties()
                                 {
                                     Name = this._name,
                                     TestStatus = testStatus,
                                     TestFrequency = testFrequency,
                                     TestTimeout = testTimeout,
                                     Kind = TestKind.Ping,
                                     EnableRetriesForWebTestFailure = retriesForWebTestFailure,
                                     Locations = WebTestLocations.GetWebTestLocations(testLocations),
                                     Configuration = new PingWebTestConfiguration(this._name, this._url, (int)testTimeout, parseDependentRequests, expectedHttpStatusCode, text),
                                     SyntheticMonitorId = this._syntheticMonitorId,
                                 };
            return properties;
        }
    }
}