using System;
using System.Net;

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
        private readonly WebTestType _testType;

        private string _syntheticMonitorId;
        private ResourceIdentity _resourceIdentity;

        /// <summary>
        /// Initialises a new instance of the <see cref="WebTestResource"/> class.
        /// </summary>
        /// <param name="name">Web test name.</param>
        /// <param name="url">Test URL.</param>
        /// <param name="insights"><see cref="ResourceBaseExtended"/> instance representing Application Insights component.</param>
        /// <param name="testType">Type of the web test. Default value is <c>WebTestType.UriPingTest</c>. </param>
        public WebTestResource(string name, string url, ResourceBaseExtended insights, WebTestType testType = WebTestType.UrlPingTest)
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
        /// Creates a new instance of the web test properties inheriting the <see cref="WebTestProperties"/> class.
        /// </summary>
        /// <param name="testLocations"><see cref="TestLocations"/> value.</param>
        /// <param name="testStatus"><see cref="TestStatus"/> value.</param>
        /// <param name="testFrequency"><see cref="TestFrequency"/> value.</param>
        /// <param name="testTimeout"><see cref="TestTimeout"/> value.</param>
        /// <param name="retriesForWebTestFailure"><see cref="RetriesForWebTestFailure"/> value.</param>
        /// <param name="expectedHttpStatusCode"><see cref="HttpStatusCode"/> value expected. Default value is <c>HttpStatusCode.OK</c></param>
        public void CreateWebTestProperties(TestLocations testLocations, TestStatus testStatus, TestFrequency testFrequency, TestTimeout testTimeout, RetriesForWebTestFailure retriesForWebTestFailure, HttpStatusCode expectedHttpStatusCode = HttpStatusCode.OK)
        {
            WebTestProperties properties;
            switch (this._testType)
            {
                case WebTestType.UrlPingTest:
                    properties = this.CreatePingWebTestProperties(testLocations, testStatus, testFrequency, testTimeout, retriesForWebTestFailure, expectedHttpStatusCode);
                    break;

                case WebTestType.MultiStepTest:
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
            this._resourceIdentity = new ResourceIdentity(this._syntheticMonitorId, ResourceType, ApiVersion);

            this.Location = this._insights.Location;
            this.Tags.Add($"hidden-link:{this._insights.Id}", "Resource");
        }

        private PingWebTestProperties CreatePingWebTestProperties(TestLocations testLocations, TestStatus testStatus, TestFrequency testFrequency, TestTimeout testTimeout, RetriesForWebTestFailure retriesForWebTestFailure, HttpStatusCode expectedHttpStatusCode = HttpStatusCode.OK)
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
                                     Configuration = new PingWebTestConfiguration(this._url, expectedHttpStatusCode),
                                     SyntheticMonitorId = this._syntheticMonitorId,
                                 };
            return properties;
        }
    }
}