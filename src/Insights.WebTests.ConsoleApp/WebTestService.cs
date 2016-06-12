using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

using Aliencube.Azure.Insights.WebTests.ConsoleApp.Settings;
using Aliencube.Azure.Insights.WebTests.Models;
using Aliencube.Azure.Insights.WebTests.Models.Options;

using Microsoft.Azure;
using Microsoft.Azure.Management.Insights;
using Microsoft.Azure.Management.Insights.Models;
using Microsoft.Azure.Management.Resources;
using Microsoft.Azure.Management.Resources.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

using Sposs.WebTests.ConsoleApp;

namespace Aliencube.Azure.Insights.WebTests.ConsoleApp
{
    /// <summary>
    /// This represents the service entity for the web test.
    /// </summary>
    public class WebTestService : IWebTestService
    {
        private readonly AuthenticationElement _auth;
        private readonly ApplicationInsightsElement _appInsights;
        private readonly WebTestElementCollection _webTests;

        private IResourceManagementClient _resourceManagementClient;
        private IInsightsManagementClient _insightsManagementClient;
        private bool _disposed;

        /// <summary>
        /// Initialises a new instance of the <see cref="WebTestService"/> class.
        /// </summary>
        /// <param name="settings"><see cref="IWebTestSettingsElement"/> instance.</param>
        public WebTestService(IWebTestSettingsElement settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            this._auth = settings.Authentication;
            this._appInsights = settings.ApplicationInsight;
            this._webTests = settings.WebTests;
        }

        /// <summary>
        /// Processes creating web test resources.
        /// </summary>
        /// <returns>Returns <c>True</c>; if processed successfully; otherwise returns <c>False</c>.</returns>
        public async Task<bool> ProcessAsync()
        {
            Console.WriteLine("Acquiring credentials...");
            var credentials = await this.GetCredentialsAsync().ConfigureAwait(false);
            Console.WriteLine("Credentials acquired");

            this._resourceManagementClient = new ResourceManagementClient(credentials);
            this._insightsManagementClient = new InsightsManagementClient(credentials);

            Console.WriteLine("Requesting resource for Applicaion Insights...");
            var insightsResource = await this.GetInsightsResourceAsync(this._resourceManagementClient).ConfigureAwait(false);
            Console.WriteLine("Resource for Application Insights requested");

            foreach (var webTest in this._webTests.OfType<WebTestElement>())
            {
                Console.WriteLine($"Creating/updating web test - {webTest.Name}...");
                var webTestResource = await this.CreateOrUpdateWebTestAsync(webTest, this._resourceManagementClient, insightsResource).ConfigureAwait(false);
                Console.WriteLine($"Web test - {webTest.Name} created/updated");

                Console.WriteLine($"Creating/updating alerts on web test - {webTest.Name}");
                await this.CreateOrUpdateAlertsAsync(webTest, this._insightsManagementClient, webTestResource, insightsResource).ConfigureAwait(false);
                Console.WriteLine($"Alerts on web test - {webTest.Name} created/updated");
            }

            return true;
        }

        /// <summary>
        /// Gets the <see cref="SubscriptionCloudCredentials"/> instance as Azure subscription credentials.
        /// </summary>
        /// <returns>Returns the <see cref="SubscriptionCloudCredentials"/> instance as Azure subscription credentials.</returns>
        public async Task<SubscriptionCloudCredentials> GetCredentialsAsync()
        {
            var authenticationContext = new AuthenticationContext($"{this._auth.AadInstanceUrl.TrimEnd('/')}/{this._appInsights.TenantFqdn}", false);
            var credential = new ClientCredential(this._auth.ClientId, this._auth.ClientSecret);
            var result = await authenticationContext.AcquireTokenAsync($"{this._auth.ManagementInstanceUrl.TrimEnd('/')}/", credential).ConfigureAwait(false);
            return new TokenCloudCredentials(this._appInsights.SubscriptionId, result.AccessToken);
        }

        /// <summary>
        /// Gets the <see cref="GenericResourceExtended"/> instance as an Application Insights resource.
        /// </summary>
        /// <param name="client"><see cref="IResourceManagementClient"/> instance.</param>
        /// <returns>Returns the <see cref="GenericResourceExtended"/> instance as an Application Insights resource.</returns>
        public async Task<GenericResourceExtended> GetInsightsResourceAsync(IResourceManagementClient client)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            var identity = new ResourceIdentity(this._appInsights.Name, "microsoft.insights/components", "2015-05-01");
            var result = await client.Resources.GetAsync(this._appInsights.ResourceGroup, identity).ConfigureAwait(false);
            if (result.StatusCode == HttpStatusCode.OK)
            {
                return result.Resource;
            }

            throw new HttpResponseException(result.StatusCode);
        }

        /// <summary>
        /// Creates or updates web test resource.
        /// </summary>
        /// <param name="webTest"><see cref="WebTestElement"/> instance from configuration.</param>
        /// <param name="client"><see cref="IResourceManagementClient"/> instance.</param>
        /// <param name="insightsResource"><see cref="ResourceBaseExtended"/> instance as an Application Insights resource.</param>
        /// <returns>Returns <c>True</c>, if the web test resource creted/updated successfully; otherwise returns <c>False</c>.</returns>
        public async Task<GenericResourceExtended> CreateOrUpdateWebTestAsync(WebTestElement webTest, IResourceManagementClient client, ResourceBaseExtended insightsResource)
        {
            if (webTest == null)
            {
                throw new ArgumentNullException(nameof(webTest));
            }

            if (webTest.TestType != TestType.UrlPingTest)
            {
                throw new InvalidOperationException("Invalid web test type");
            }

            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            if (insightsResource == null)
            {
                throw new ArgumentNullException(nameof(insightsResource));
            }

            var monitorId = $"{webTest.Name}-{this._appInsights.Name}";
            var identity = new ResourceIdentity(monitorId, "microsoft.insights/webtests", "2015-05-01");

            var resource = new GenericResource(this._appInsights.Location)
                               {
                                   Tags =
                                       {
                                           { $"hidden-link:{insightsResource.Id}", "Resource" }
                                       },
                                   Properties = new PingWebTestProperties()
                                                    {
                                                        Name = webTest.Name,
                                                        SyntheticMonitorId = monitorId,
                                                        Enabled = true,
                                                        Configuration = new PingWebTestConfiguration(webTest.Url),
                                                        Locations = { WebTestLocations.AUSydney },
                                                        RetryEnabled = webTest.EnableRetriesForFailure,
                                                    }
                               };

            var result = await client.Resources.CreateOrUpdateAsync(this._appInsights.ResourceGroup, identity, resource).ConfigureAwait(false);
            if (result.StatusCode == HttpStatusCode.Created || result.StatusCode == HttpStatusCode.OK)
            {
                return result.Resource;
            }

            throw new HttpResponseException(result.StatusCode);
        }

        /// <summary>
        /// Creates or updates alert resource.
        /// </summary>
        /// <param name="webTest"><see cref="WebTestElement"/> instance from configuration.</param>
        /// <param name="client"><see cref="IInsightsManagementClient"/> instance.</param>
        /// <param name="webTestResource"><see cref="ResourceBaseExtended"/> instance as a Web Test resource.</param>
        /// <param name="insightsResource"><see cref="ResourceBaseExtended"/> instance as an Application Insights resource.</param>
        /// <returns>Returns <c>True</c>, if the web test resource creted/updated successfully; otherwise returns <c>False</c>.</returns>
        public async Task<bool> CreateOrUpdateAlertsAsync(WebTestElement webTest, IInsightsManagementClient client, ResourceBaseExtended webTestResource, ResourceBaseExtended insightsResource)
        {
            if (webTest == null)
            {
                throw new ArgumentNullException(nameof(webTest));
            }

            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            if (webTestResource == null)
            {
                throw new ArgumentNullException(nameof(webTestResource));
            }

            if (insightsResource == null)
            {
                throw new ArgumentNullException(nameof(insightsResource));
            }

            var alertName = $"{webTest.Name}-alert";

            var ruleEmailAction = new RuleEmailAction() { SendToServiceOwners = webTest.Alerts.SendAlertToAdmin };
            if (webTest.Alerts.Recipients != null && webTest.Alerts.Recipients.Any())
            {
                ruleEmailAction.CustomEmails = webTest.Alerts.Recipients;
            }

            var parameters = new RuleCreateOrUpdateParameters
                                 {
                                     Tags =
                                         {
                                             { $"hidden-link:{insightsResource.Id}", "Resource" },
                                             { $"hidden-link:{webTestResource.Id}", "Resource" }
                                         },
                                     Location = "East US",
                                     Properties = new Rule()
                                                      {
                                                          Name = alertName,
                                                          Description = string.Empty,
                                                          IsEnabled = webTest.Alerts.StatusEnabled,
                                                          LastUpdatedTime = DateTime.UtcNow,
                                                          Actions = { ruleEmailAction },
                                                          Condition = new LocationThresholdRuleCondition
                                                                          {
                                                                              DataSource = new RuleMetricDataSource
                                                                                               {
                                                                                                   MetricName = "GSMT_AvRaW",
                                                                                                   ResourceUri = webTestResource.Id
                                                                                               },
                                                                              FailedLocationCount = webTest.Alerts.AlertLocationThreshold,
                                                                              WindowSize = TimeSpan.FromMinutes(webTest.Alerts.AlertFailureTimeWindow)
                                                                          },
                                                      },
                                 };

            var result = await client.AlertOperations.CreateOrUpdateRuleAsync(this._appInsights.ResourceGroup, parameters).ConfigureAwait(false);
            if (result.StatusCode == HttpStatusCode.Created || result.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }

            throw new HttpResponseException(result.StatusCode);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (this._disposed)
            {
                return;
            }

            this._resourceManagementClient?.Dispose();
            this._insightsManagementClient?.Dispose();

            this._disposed = true;
        }
    }
}
