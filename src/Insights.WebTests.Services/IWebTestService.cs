using System;
using System.Threading.Tasks;

using Aliencube.Azure.Insights.WebTests.Models.Options;
using Aliencube.Azure.Insights.WebTests.Services.Settings;

using Microsoft.Azure;
using Microsoft.Azure.Management.Insights;
using Microsoft.Azure.Management.Resources;
using Microsoft.Azure.Management.Resources.Models;

namespace Aliencube.Azure.Insights.WebTests.Services
{
    /// <summary>
    /// This provides interfaces to the <see cref="WebTestService"/> class.
    /// </summary>
    public interface IWebTestService : IDisposable
    {
        /// <summary>
        /// Processes creating web test resources.
        /// </summary>
        /// <param name="options"><see cref="CommandBuildOptions"/> instance.</param>
        /// <returns>Returns <c>True</c>; if processed successfully; otherwise returns <c>False</c>.</returns>
        Task<bool> ProcessAsync(CommandBuildOptions options);

        /// <summary>
        /// Processes creating web test resources.
        /// </summary>
        /// <param name="name">Web test name.</param>
        /// <param name="url">Web test URL.</param>
        /// <param name="authType"><see cref="AuthType"/> value.</param>
        /// <param name="accessToken">Access token value.</param>
        /// <param name="testType"><see cref="TestType"/> value. Default is <c>TestType.UriPingTest</c>.</param>
        /// <returns>Returns <c>True</c>; if processed successfully; otherwise returns <c>False</c>.</returns>
        Task<bool> ProcessAsync(string name, string url, AuthType authType = AuthType.None, string accessToken = null, TestType testType = TestType.UrlPingTest);

        /// <summary>
        /// Gets the <see cref="SubscriptionCloudCredentials"/> instance as Azure subscription credentials.
        /// </summary>
        /// <returns>Returns the <see cref="SubscriptionCloudCredentials"/> instance as Azure subscription credentials.</returns>
        Task<SubscriptionCloudCredentials> GetCredentialsAsync();

        /// <summary>
        /// Gets the <see cref="GenericResourceExtended"/> instance as an Application Insights resource.
        /// </summary>
        /// <param name="client"><see cref="IResourceManagementClient"/> instance.</param>
        /// <returns>Returns the <see cref="GenericResourceExtended"/> instance as an Application Insights resource.</returns>
        Task<GenericResourceExtended> GetInsightsResourceAsync(IResourceManagementClient client);

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
        /// <returns>Returns the <see cref="GenericResourceExtended"/> instance as a web test resource.</returns>
        Task<GenericResourceExtended> CreateOrUpdateWebTestAsync(string name, string url, AuthType authType, string accessToken, WebTestElement webTest, IResourceManagementClient client, ResourceBaseExtended insightsResource);

        /// <summary>
        /// Creates or updates alert resource.
        /// </summary>
        /// <param name="name">Name of the web test.</param>
        /// <param name="webTest"><see cref="WebTestElement"/> instance from configuration.</param>
        /// <param name="client"><see cref="IInsightsManagementClient"/> instance.</param>
        /// <param name="webTestResource"><see cref="ResourceBaseExtended"/> instance as a Web Test resource.</param>
        /// <param name="insightsResource"><see cref="ResourceBaseExtended"/> instance as an Application Insights resource.</param>
        /// <returns>Returns <c>True</c>, if the web test resource creted/updated successfully; otherwise returns <c>False</c>.</returns>
        Task<bool> CreateOrUpdateAlertsAsync(string name, WebTestElement webTest, IInsightsManagementClient client, ResourceBaseExtended webTestResource, ResourceBaseExtended insightsResource);
    }
}