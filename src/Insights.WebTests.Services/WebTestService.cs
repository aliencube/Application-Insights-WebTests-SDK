﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

using Aliencube.AdalWrapper;
using Aliencube.Azure.Insights.WebTests.Models;
using Aliencube.Azure.Insights.WebTests.Models.Options;
using Aliencube.Azure.Insights.WebTests.Services.Extensions;
using Aliencube.Azure.Insights.WebTests.Services.Settings;

using Microsoft.Azure;
using Microsoft.Azure.Management.Insights;
using Microsoft.Azure.Management.Insights.Models;
using Microsoft.Azure.Management.Resources;
using Microsoft.Azure.Management.Resources.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace Aliencube.Azure.Insights.WebTests.Services
{
    /// <summary>
    /// This represents the service entity for the web test.
    /// </summary>
    public class WebTestService : IWebTestService
    {
        private const string InsightsResourceType = "microsoft.insights/components";
        private const string InsightsApiVersion = "2015-05-01";
        private const string InsightsAlertResourceLocation = "East US";

        private readonly AuthenticationElement _auth;
        private readonly ApplicationInsightsElement _appInsights;
        private readonly List<WebTestElement> _webTests;
        private readonly IAuthenticationContextWrapper _authenticationContext;

        private IResourceManagementClient _resourceManagementClient;
        private IInsightsManagementClient _insightsManagementClient;
        private bool _disposed;

        /// <summary>
        /// Initialises a new instance of the <see cref="WebTestService"/> class.
        /// </summary>
        /// <param name="settings"><see cref="IWebTestSettingsElement"/> instance.</param>
        /// <param name="authenticationContext"><see cref="IAuthenticationContextWrapper"/> instance.</param>
        public WebTestService(IWebTestSettingsElement settings, IAuthenticationContextWrapper authenticationContext)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            this._auth = settings.Authentication.Clone();
            this._appInsights = settings.ApplicationInsight.Clone();
            this._webTests = settings.WebTests.Clone().OfType<WebTestElement>().ToList();

            if (authenticationContext == null)
            {
                throw new ArgumentNullException(nameof(authenticationContext));
            }

            this._authenticationContext = authenticationContext;
        }

        /// <summary>
        /// Processes creating web test resources.
        /// </summary>
        /// <param name="options"><see cref="CommandBuildOptions"/> instance.</param>
        /// <returns>Returns <c>True</c>; if processed successfully; otherwise returns <c>False</c>.</returns>
        public async Task<bool> ProcessAsync(CommandBuildOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return await this.ProcessAsync(options.Name, options.Url, options.AuthType, options.AccessToken, options.TestType).ConfigureAwait(false);
        }

        /// <summary>
        /// Processes creating web test resources.
        /// </summary>
        /// <param name="name">Web test name.</param>
        /// <param name="url">Web test URL.</param>
        /// <param name="authType"><see cref="AuthType"/> value.</param>
        /// <param name="accessToken">Access token value.</param>
        /// <param name="testType"><see cref="TestType"/> value. Default is <c>TestType.UriPingTest</c>.</param>
        /// <returns>Returns <c>True</c>; if processed successfully; otherwise returns <c>False</c>.</returns>
        public async Task<bool> ProcessAsync(string name, string url, AuthType authType = AuthType.None, string accessToken = null, TestType testType = TestType.UrlPingTest)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            var credentials = await this.GetCredentialsAsync().ConfigureAwait(false);

            this._resourceManagementClient = new ResourceManagementClient(credentials);
            this._insightsManagementClient = new InsightsManagementClient(credentials);

            var insightsResource = await this.GetInsightsResourceAsync(this._resourceManagementClient).ConfigureAwait(false);

            var webTest = this._webTests.SingleOrDefault(p => p.TestType == testType);
            if (webTest == null)
            {
                return false;
            }

            var webTestResource = await this.CreateOrUpdateWebTestAsync(name, url, authType, accessToken, webTest, this._resourceManagementClient, insightsResource).ConfigureAwait(false);

            var result = await this.CreateOrUpdateAlertsAsync(name, webTest, this._insightsManagementClient, webTestResource, insightsResource).ConfigureAwait(false);

            return result;
        }

        /// <summary>
        /// Gets the <see cref="SubscriptionCloudCredentials"/> instance as Azure subscription credentials.
        /// </summary>
        /// <returns>Returns the <see cref="SubscriptionCloudCredentials"/> instance as Azure subscription credentials.</returns>
        public async Task<SubscriptionCloudCredentials> GetCredentialsAsync()
        {
#if !TEST
            Console.WriteLine("Acquiring credentials ...");
#endif

            IAuthenticationResultWrapper result;
            if (this._auth.UseServicePrinciple)
            {
                var cc = new ClientCredential(this._auth.ClientId, this._auth.ClientSecret);
                result = await this._authenticationContext.AcquireTokenAsync($"{this._auth.ManagementInstanceUrl.TrimEnd('/')}/", cc).ConfigureAwait(false);

#if !TEST
                Console.WriteLine("Credentials acquired");
#endif
                return new TokenCloudCredentials(this._appInsights.SubscriptionId, result.AccessToken);
            }

            var uc = new UserPasswordCredential(this._auth.Username, this._auth.Password);
            result = await this._authenticationContext.AcquireTokenAsync($"{this._auth.ManagementInstanceUrl.TrimEnd('/')}/", this._auth.ClientId, uc).ConfigureAwait(false);

#if !TEST
            Console.WriteLine("Credentials acquired");
#endif
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

#if !TEST
            Console.WriteLine("Retrieving Application Insights details ...");
#endif
            var identity = new ResourceIdentity(this._appInsights.Name, InsightsResourceType, InsightsApiVersion);
            var result = await client.Resources.GetAsync(this._appInsights.ResourceGroup, identity).ConfigureAwait(false);
            if (!IsAcceptableHttpStatusCode(result.StatusCode, new[] { HttpStatusCode.OK }))
            {
                throw new HttpResponseException(result.StatusCode);
            }

#if !TEST
            Console.WriteLine("Application Insights details retrieved");
#endif
            return result.Resource;
        }

        /// <summary>
        /// Creates or updates web test resource.
        /// </summary>
        /// <param name="name">Name of the web test.</param>
        /// <param name="url">URL of the web test.</param>
        /// <param name="authType"><see cref="AuthType"/> value.</param>
        /// <param name="accessToken">Access token value.</param>
        /// <param name="webTest"><see cref="WebTestElement"/> instance from configuration.</param>
        /// <param name="client"><see cref="IResourceManagementClient"/> instance.</param>
        /// <param name="insightsResource"><see cref="ResourceBaseExtended"/> instance as an Application Insights resource.</param>
        /// <returns>Returns <c>True</c>, if the web test resource creted/updated successfully; otherwise returns <c>False</c>.</returns>
        public async Task<GenericResourceExtended> CreateOrUpdateWebTestAsync(string name, string url, AuthType authType, string accessToken, WebTestElement webTest, IResourceManagementClient client, ResourceBaseExtended insightsResource)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            if (webTest == null)
            {
                throw new ArgumentNullException(nameof(webTest));
            }

            // TODO: for now it only supports PING test.
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

#if !TEST
            Console.WriteLine($"Processing web test for {name} started ...");
#endif
            var resource = new WebTestResource(name, url, insightsResource, webTest.TestType);
            resource.CreateWebTestProperties(webTest.TestLocations, webTest.Status, webTest.Frequency, webTest.SuccessCriteria.Timeout, webTest.ParseDependentRequests, webTest.RetriesForWebTestFailure, authType: authType, accessToken: accessToken);

            var result = await client.Resources.CreateOrUpdateAsync(this._appInsights.ResourceGroup, resource.ResourceIdentity, resource).ConfigureAwait(false);
            if (!IsAcceptableHttpStatusCode(result.StatusCode, new[] { HttpStatusCode.OK, HttpStatusCode.Created }))
            {
                throw new HttpResponseException(result.StatusCode);
            }

#if !TEST
            Console.WriteLine($"Processing web test for {name} completed");
#endif
            return result.Resource;
        }

        /// <summary>
        /// Creates or updates alert resource.
        /// </summary>
        /// <param name="name">Name of the web test.</param>
        /// <param name="webTest"><see cref="WebTestElement"/> instance from configuration.</param>
        /// <param name="client"><see cref="IInsightsManagementClient"/> instance.</param>
        /// <param name="webTestResource"><see cref="ResourceBaseExtended"/> instance as a Web Test resource.</param>
        /// <param name="insightsResource"><see cref="ResourceBaseExtended"/> instance as an Application Insights resource.</param>
        /// <returns>Returns <c>True</c>, if the web test resource creted/updated successfully; otherwise returns <c>False</c>.</returns>
        public async Task<bool> CreateOrUpdateAlertsAsync(string name, WebTestElement webTest, IInsightsManagementClient client, ResourceBaseExtended webTestResource, ResourceBaseExtended insightsResource)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

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

#if !TEST
            Console.WriteLine($"Processing alert for {name} started ...");
#endif
            var parameters = (new RuleCreateOrUpdateParameters() { Location = InsightsAlertResourceLocation })
                                 .AddTags(webTestResource, insightsResource)
                                 .AddProperties(name, webTest, webTestResource, insightsResource);

            var result = await client.AlertOperations.CreateOrUpdateRuleAsync(this._appInsights.ResourceGroup, parameters).ConfigureAwait(false);
            if (!IsAcceptableHttpStatusCode(result.StatusCode, new[] { HttpStatusCode.OK, HttpStatusCode.Created }))
            {
                throw new HttpResponseException(result.StatusCode);
            }

#if !TEST
            Console.WriteLine($"Processing alert for {name} completed");
#endif
            return true;
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

        private static bool IsAcceptableHttpStatusCode(HttpStatusCode statusCode, IEnumerable<HttpStatusCode> acceptableStatusCodes)
        {
            var result = acceptableStatusCodes.Any(p => p == statusCode);
            return result;
        }
    }
}
