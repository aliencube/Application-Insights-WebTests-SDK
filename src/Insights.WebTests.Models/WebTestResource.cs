using System;

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
        private readonly ResourceBaseExtended _insights;
        private readonly WebTestType _testType;

        private string _syntheticMonitorId;
        private ResourceIdentity _resourceIdentity;

        /// <summary>
        /// Initialises a new instance of the <see cref="WebTestResource"/> class.
        /// </summary>
        /// <param name="name">Web test name.</param>
        /// <param name="insights"><see cref="ResourceBaseExtended"/> instance representing Application Insights component.</param>
        /// <param name="testType">Type of the web test. Default value is <c>WebTestType.UriPingTest</c>. </param>
        public WebTestResource(string name, ResourceBaseExtended insights, WebTestType testType = WebTestType.UrlPingTest)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            this._name = name;

            if (insights == null)
            {
                throw new ArgumentNullException(nameof(insights));
            }

            this._insights = insights;

            this.Initialise();
        }

        private void Initialise()
        {
            this._syntheticMonitorId = $"{this._name}-{this._insights.Name}";
            this._resourceIdentity = new ResourceIdentity(this._syntheticMonitorId, ResourceType, ApiVersion);

            this.Location = this._insights.Location;
            this.Tags.Add($"hidden-link:{this._insights.Id}", "Resource");

            this.Properties = this.CreateWebTestProperties();
        }

        private WebTestProperties CreateWebTestProperties()
        {
            switch (this._testType)
            {
                case WebTestType.UrlPingTest:
                    return this.CreatePingWebTestProperties();

                case WebTestType.MultiStepTest:
                    return new MultiStepWebTestProperties();

                default:
                    return null;
            }
        }

        private PingWebTestProperties CreatePingWebTestProperties()
        {
            var properties = new PingWebTestProperties()
                                 {
                                     Name = this._name,
                                     TestStatus = WebTestStatus.Enabled,
                                     TestFrequency = TestFrequency._5Minutes,
                                     TestTimeout = TestTimeout._120Seconds,
                                     Kind = WebTestKind.Ping,
                                     EnableRetriesForWebTestFailure = RetriesForWebTestFailure.Enable,
                                     Locations = { },
                                     Configuration = null,
                                     SyntheticMonitorId = this._syntheticMonitorId,
                                 };
            return properties;
        }
    }


    public enum RetriesForWebTestFailure
    {
        Disable = 0,
        Enable = 1,
    }
}