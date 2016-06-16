using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

using Aliencube.AdalWrapper;
using Aliencube.Azure.Insights.WebTests.Models.Options;
using Aliencube.Azure.Insights.WebTests.Services.Settings;
using Aliencube.Azure.Insights.WebTests.Services.Tests.Fixtures;

using FluentAssertions;

using Microsoft.Azure;
using Microsoft.Azure.Management.Insights;
using Microsoft.Azure.Management.Resources;
using Microsoft.Azure.Management.Resources.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

using Moq;

using Xunit;
using Xunit.Sdk;

namespace Aliencube.Azure.Insights.WebTests.Services.Tests
{
    /// <summary>
    /// This represents the test entity for the <see cref="WebTestService"/> class.
    /// </summary>
    public class WebTestServiceTest : IClassFixture<WebTestServiceFixture>
    {
        private readonly Mock<AuthenticationElement> _auth;
        private readonly Mock<ApplicationInsightsElement> _appInsights;
        private readonly Mock<WebTestElement> _webTest;
        private readonly WebTestElementCollection _webTests;
        private readonly Mock<IWebTestSettingsElement> _settings;
        private readonly Mock<IAuthenticationResultWrapper> _authResult;
        private readonly Mock<IAuthenticationContextWrapper> _authContext;
        private readonly Mock<IResourceOperations> _resourceOperations;
        private readonly Mock<IResourceManagementClient> _resourceClient;
        private readonly Mock<IInsightsManagementClient> _insightsClient;

        private IWebTestService _service;

        /// <summary>
        /// Initialises a new instance of the <see cref="WebTestServiceTest"/> class.
        /// </summary>
        /// <param name="fixture"><see cref="WebTestServiceFixture"/> instance.</param>
        public WebTestServiceTest(WebTestServiceFixture fixture)
        {
            this._auth = fixture.AuthenticationElement;
            this._appInsights = fixture.ApplicationInsightsElement;
            this._webTest = fixture.WebTestElement;
            this._webTests = fixture.WebTestElementCollection;
            this._settings = fixture.WebTestSettingsElement;
            this._authResult = fixture.AuthenticationResult;
            this._authContext = fixture.AuthenticationContext;
            this._resourceOperations = fixture.ResourceOperations;
            this._resourceClient = fixture.ResourceManagementClient;
            this._insightsClient = fixture.InsightsManagementClient;
            this._service = fixture.WebTestService;
        }

        /// <summary>
        /// Tests whether the constructor should throw an exception or not.
        /// </summary>
        [Fact]
        public void Given_NullParameter_Constructor_ShouldThrow_Exception()
        {
            Action action = () => { var service = new WebTestService(null, this._authContext.Object); };
            action.ShouldThrow<ArgumentNullException>();

            action = () => { var service = new WebTestService(this._settings.Object, null); };
            action.ShouldThrow<ArgumentNullException>();
        }

        /// <summary>
        /// Tests whether the constructor should NOT throw an exception or not.
        /// </summary>
        [Fact]
        public void Given_Parameter_Constructor_ShouldThrow_NoException()
        {
            Action action = () => { var service = new WebTestService(this._settings.Object, this._authContext.Object); };
            action.ShouldNotThrow<Exception>();
        }

        /// <summary>
        /// Tests whether the method should return result or not.
        /// </summary>
        /// <param name="accessToken">Access token value.</param>
        [Theory]
        [InlineData("ACCESS_TOKEN")]
        public async void Given_ClientCredential_GetCredentialsAsync_ShouldReturn_Result(string accessToken)
        {
            this._auth.SetupGet(p => p.ClientId).Returns("CLIENT_ID");
            this._auth.SetupGet(p => p.ClientSecret).Returns("CLIENT_SECRET");
            this._auth.SetupGet(p => p.UseServicePrinciple).Returns(true);
            this._auth.SetupGet(p => p.AadInstanceUrl).Returns("https://login.microsoftonline.com/");
            this._auth.SetupGet(p => p.ManagementInstanceUrl).Returns("https://management.core.windows.net/");

            this._appInsights.SetupGet(p => p.SubscriptionId).Returns("SUBSCRIPTION_ID");

            this._settings.SetupGet(p => p.Authentication).Returns(this._auth.Object);
            this._settings.SetupGet(p => p.ApplicationInsight).Returns(this._appInsights.Object);

            this._authResult.SetupGet(p => p.AccessToken).Returns(accessToken);
            this._authContext.Setup(p => p.AcquireTokenAsync(It.IsAny<string>(), It.IsAny<ClientCredential>())).ReturnsAsync(this._authResult.Object);

            this._service = new WebTestService(this._settings.Object, this._authContext.Object);

            var result = (await this._service.GetCredentialsAsync().ConfigureAwait(false)) as TokenCloudCredentials;
            result.Should().NotBeNull();
            result.SubscriptionId.Should().BeEquivalentTo(this._appInsights.Object.SubscriptionId);
            result.Token.Should().BeEquivalentTo(accessToken);
        }

        /// <summary>
        /// Tests whether the method should return result or not.
        /// </summary>
        /// <param name="accessToken">Access token value.</param>
        [Theory]
        [InlineData("ACCESS_TOKEN")]
        public async void Given_UserCredential_GetCredentialsAsync_ShouldReturn_Result(string accessToken)
        {
            this._auth.SetupGet(p => p.ClientId).Returns("CLIENT_ID");
            this._auth.SetupGet(p => p.UseServicePrinciple).Returns(false);
            this._auth.SetupGet(p => p.Username).Returns("USERNAME");
            this._auth.SetupGet(p => p.Password).Returns("PASSWORD");
            this._auth.SetupGet(p => p.AadInstanceUrl).Returns("https://login.microsoftonline.com/");
            this._auth.SetupGet(p => p.ManagementInstanceUrl).Returns("https://management.core.windows.net/");

            this._appInsights.SetupGet(p => p.SubscriptionId).Returns("SUBSCRIPTION_ID");

            this._settings.SetupGet(p => p.Authentication).Returns(this._auth.Object);
            this._settings.SetupGet(p => p.ApplicationInsight).Returns(this._appInsights.Object);

            this._authResult.SetupGet(p => p.AccessToken).Returns(accessToken);
            this._authContext.Setup(p => p.AcquireTokenAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<UserCredential>())).ReturnsAsync(this._authResult.Object);

            this._service = new WebTestService(this._settings.Object, this._authContext.Object);

            var result = (await this._service.GetCredentialsAsync().ConfigureAwait(false)) as TokenCloudCredentials;
            result.Should().NotBeNull();
            result.SubscriptionId.Should().BeEquivalentTo(this._appInsights.Object.SubscriptionId);
            result.Token.Should().BeEquivalentTo(accessToken);
        }

        /// <summary>
        /// Tests whether the method should throw an exception or not.
        /// </summary>
        [Fact]
        public void Given_NullParameter_GetInsightsResourceAsync_ShouldThrow_Exception()
        {
            Func<Task> func = async () => { var result = await this._service.GetInsightsResourceAsync(null).ConfigureAwait(false); };
            func.ShouldThrow<ArgumentNullException>();
        }

        /// <summary>
        /// Tests whether the method should throw an exception or not.
        /// </summary>
        /// <param name="appInsightsName">Application Insights resource name.</param>
        /// <param name="resourceGroup">Resource group name.</param>
        /// <param name="statusCode"><see cref="HttpStatusCode"/> value.</param>
        [Theory]
        [InlineData("APPLICATION_INSIGHTS_NAME", "RESOURCE_GROUP_NAME", HttpStatusCode.BadRequest)]
        public void Given_InvalidHttpStatusCode_GetInsightsResourceAsync_ShouldThrow_Exception(string appInsightsName, string resourceGroup, HttpStatusCode statusCode)
        {
            this._appInsights.SetupGet(p => p.Name).Returns(appInsightsName);
            this._appInsights.SetupGet(p => p.ResourceGroup).Returns(resourceGroup);

            this._settings.SetupGet(p => p.ApplicationInsight).Returns(this._appInsights.Object);

            var resourceResult = new ResourceGetResult() { StatusCode = statusCode };
            this._resourceOperations.Setup(p => p.GetAsync(It.IsAny<string>(), It.IsAny<ResourceIdentity>(), It.IsAny<CancellationToken>())).ReturnsAsync(resourceResult);

            this._resourceClient.SetupGet(p => p.Resources).Returns(this._resourceOperations.Object);

            Func<Task> func = async () => { var result = await this._service.GetInsightsResourceAsync(this._resourceClient.Object).ConfigureAwait(false); };
            func.ShouldThrow<HttpResponseException>().And.Response.StatusCode.Should().Be(statusCode);
        }

        /// <summary>
        /// Tests whether the method should return result or not.
        /// </summary>
        /// <param name="appInsightsName">Application Insights resource name.</param>
        /// <param name="resourceGroup">Resource group name.</param>
        /// <param name="location">Resource location.</param>
        [Theory]
        [InlineData("APPLICATION_INSIGHTS_NAME", "RESOURCE_GROUP_NAME", "Central US")]
        public async void Given_Parameter_GetInsightsResourceAsync_ShouldReturn_Result(string appInsightsName, string resourceGroup, string location)
        {
            this._appInsights.SetupGet(p => p.Name).Returns(appInsightsName);
            this._appInsights.SetupGet(p => p.ResourceGroup).Returns(resourceGroup);

            this._settings.SetupGet(p => p.ApplicationInsight).Returns(this._appInsights.Object);

            var resource = new GenericResourceExtended(location);
            var resourceResult = new ResourceGetResult() { StatusCode = HttpStatusCode.OK, Resource = resource };
            this._resourceOperations.Setup(p => p.GetAsync(It.IsAny<string>(), It.IsAny<ResourceIdentity>(), It.IsAny<CancellationToken>())).ReturnsAsync(resourceResult);

            this._resourceClient.SetupGet(p => p.Resources).Returns(this._resourceOperations.Object);

            var result = await this._service.GetInsightsResourceAsync(this._resourceClient.Object).ConfigureAwait(false);
            result.Location.Should().BeEquivalentTo(location);
        }

        /// <summary>
        /// Tests whether the method should throw an exception or not.
        /// </summary>
        /// <param name="name">Web test name.</param>
        /// <param name="url">Web test URL.</param>
        /// <param name="testType">Web test type.</param>
        /// <param name="location">Web test resource location.</param>
        [Theory]
        [InlineData("WEBTEST_NAME", "http://localhost", TestType.UrlPingTest, "Central US")]
        public void Given_NullParameters_CreateOrUpdateWebTestAsync_ShouldThrow_Exception(string name, string url, TestType testType, string location)
        {
            this._webTest.SetupGet(p => p.TestType).Returns(testType);

            var insightsResource = new ResourceBaseExtended(location);

            Func<Task> func = async () => { var result = await this._service.CreateOrUpdateWebTestAsync(null, url, this._webTest.Object, this._resourceClient.Object, insightsResource).ConfigureAwait(false); };
            func.ShouldThrow<ArgumentNullException>();

            func = async () => { var result = await this._service.CreateOrUpdateWebTestAsync(name, null, this._webTest.Object, this._resourceClient.Object, insightsResource).ConfigureAwait(false); };
            func.ShouldThrow<ArgumentNullException>();

            func = async () => { var result = await this._service.CreateOrUpdateWebTestAsync(name, url, null, this._resourceClient.Object, insightsResource).ConfigureAwait(false); };
            func.ShouldThrow<ArgumentNullException>();

            func = async () => { var result = await this._service.CreateOrUpdateWebTestAsync(name, url, this._webTest.Object, null, insightsResource).ConfigureAwait(false); };
            func.ShouldThrow<ArgumentNullException>();

            func = async () => { var result = await this._service.CreateOrUpdateWebTestAsync(name, url, this._webTest.Object, this._resourceClient.Object, null).ConfigureAwait(false); };
            func.ShouldThrow<ArgumentNullException>();
        }

        /// <summary>
        /// Tests whether the method should throw an exception or not.
        /// </summary>
        /// <param name="name">Web test name.</param>
        /// <param name="url">Web test URL.</param>
        /// <param name="testType">Web test type.</param>
        [Theory]
        [InlineData("WEBTEST_NAME", "http://localhost", TestType.MultiStepTest)]
        public void Given_InvalidTestType_CreateOrUpdateWebTestAsync_ShouldThrow_Exception(string name, string url, TestType testType)
        {
            this._webTest.SetupGet(p => p.TestType).Returns(testType);

            var insightsResource = new ResourceBaseExtended();

            Func<Task> func = async () => { var result = await this._service.CreateOrUpdateWebTestAsync(name, url, this._webTest.Object, this._resourceClient.Object, insightsResource).ConfigureAwait(false); };
            func.ShouldThrow<InvalidOperationException>();
        }

        /// <summary>
        /// Tests whether the method should throw an exception or not.
        /// </summary>
        /// <param name="name">Web test name.</param>
        /// <param name="url">Web test URL.</param>
        /// <param name="testStatus"><see cref="TestStatus"/> value.</param>
        /// <param name="testFrequency"><see cref="TestFrequency"/> value.</param>
        /// <param name="testTimeout"><see cref="TestTimeout"/> value.</param>
        /// <param name="parseDependentRequests">Value indicating whether to parse dependent requests or not.</param>
        /// <param name="retriesForWebTestFailure">Value indicating whether to retry for web test failure or not.</param>
        /// <param name="testLocations"><see cref="TestLocations"/> value.</param>
        /// <param name="resourceGroup">Resource group name.</param>
        /// <param name="statusCode"><see cref="HttpStatusCode"/> value.</param>
        [Theory]
        [InlineData("WEBTEST_NAME", "http://localhost", TestStatus.Enabled, TestFrequency._5Minutes, TestTimeout._120Seconds, false, RetriesForWebTestFailure.Enable, TestLocations.AuSydney | TestLocations.BrSaoPaulo, "RESOURCE_GROUP", HttpStatusCode.BadRequest)]
        public void Given_InvalidHttpStatusCode_CreateOrUpdateWebTestAsync_ShouldThrow_Exception(string name, string url, TestStatus testStatus, TestFrequency testFrequency, TestTimeout testTimeout, bool parseDependentRequests, RetriesForWebTestFailure retriesForWebTestFailure, TestLocations testLocations, string resourceGroup, HttpStatusCode statusCode)
        {
            var successCriteria = new Mock<SucessCriteriaElement>();
            successCriteria.SetupGet(p => p.Timeout).Returns(testTimeout);

            this._webTest.SetupGet(p => p.TestType).Returns(TestType.UrlPingTest);
            this._webTest.SetupGet(p => p.Status).Returns(testStatus);
            this._webTest.SetupGet(p => p.Frequency).Returns(testFrequency);
            this._webTest.SetupGet(p => p.ParseDependentRequests).Returns(parseDependentRequests);
            this._webTest.SetupGet(p => p.SuccessCriteria).Returns(successCriteria.Object);
            this._webTest.SetupGet(p => p.RetriesForWebTestFailure).Returns(retriesForWebTestFailure);
            this._webTest.SetupGet(p => p.TestLocations).Returns(testLocations);

            this._appInsights.SetupGet(p => p.ResourceGroup).Returns(resourceGroup);

            this._settings.SetupGet(p => p.ApplicationInsight).Returns(this._appInsights.Object);

            var resourceResult = new ResourceCreateOrUpdateResult() { StatusCode = statusCode };
            this._resourceOperations.Setup(p => p.CreateOrUpdateAsync(It.IsAny<string>(), It.IsAny<ResourceIdentity>(), It.IsAny<GenericResource>(), It.IsAny<CancellationToken>())).ReturnsAsync(resourceResult);

            this._resourceClient.Setup(p => p.Resources).Returns(this._resourceOperations.Object);

            var id = Guid.NewGuid();
            var insightsResource = new ResourceBaseExtended() { Id = id.ToString(), Name = name };

            Func<Task> func = async () => { var result = await this._service.CreateOrUpdateWebTestAsync(name, url, this._webTest.Object, this._resourceClient.Object, insightsResource).ConfigureAwait(false); };
            func.ShouldThrow<HttpResponseException>().And.Response.StatusCode.Should().Be(statusCode);
        }

        /// <summary>
        /// Tests whether the method should return result or not.
        /// </summary>
        /// <param name="name">Web test name.</param>
        /// <param name="url">Web test URL.</param>
        /// <param name="testStatus"><see cref="TestStatus"/> value.</param>
        /// <param name="testFrequency"><see cref="TestFrequency"/> value.</param>
        /// <param name="testTimeout"><see cref="TestTimeout"/> value.</param>
        /// <param name="parseDependentRequests">Value indicating whether to parse dependent requests or not.</param>
        /// <param name="retriesForWebTestFailure">Value indicating whether to retry for web test failure or not.</param>
        /// <param name="testLocations"><see cref="TestLocations"/> value.</param>
        /// <param name="resourceGroup">Resource group name.</param>
        /// <param name="location">Resouce location.</param>
        [Theory]
        [InlineData("WEBTEST_NAME", "http://localhost", TestStatus.Enabled, TestFrequency._5Minutes, TestTimeout._120Seconds, false, RetriesForWebTestFailure.Enable, TestLocations.AuSydney | TestLocations.BrSaoPaulo, "RESOURCE_GROUP", "Central US")]
        public async void Given_Parameters_CreateOrUpdateWebTestAsync_ShouldReturn_Result(string name, string url, TestStatus testStatus, TestFrequency testFrequency, TestTimeout testTimeout, bool parseDependentRequests, RetriesForWebTestFailure retriesForWebTestFailure, TestLocations testLocations, string resourceGroup, string location)
        {
            var successCriteria = new Mock<SucessCriteriaElement>();
            successCriteria.SetupGet(p => p.Timeout).Returns(testTimeout);

            this._webTest.SetupGet(p => p.TestType).Returns(TestType.UrlPingTest);
            this._webTest.SetupGet(p => p.Status).Returns(testStatus);
            this._webTest.SetupGet(p => p.Frequency).Returns(testFrequency);
            this._webTest.SetupGet(p => p.ParseDependentRequests).Returns(parseDependentRequests);
            this._webTest.SetupGet(p => p.SuccessCriteria).Returns(successCriteria.Object);
            this._webTest.SetupGet(p => p.RetriesForWebTestFailure).Returns(retriesForWebTestFailure);
            this._webTest.SetupGet(p => p.TestLocations).Returns(testLocations);

            this._appInsights.SetupGet(p => p.ResourceGroup).Returns(resourceGroup);

            this._settings.SetupGet(p => p.ApplicationInsight).Returns(this._appInsights.Object);

            var resource = new GenericResourceExtended(location);
            var resourceResult = new ResourceCreateOrUpdateResult() { StatusCode = HttpStatusCode.OK, Resource = resource };
            this._resourceOperations.Setup(p => p.CreateOrUpdateAsync(It.IsAny<string>(), It.IsAny<ResourceIdentity>(), It.IsAny<GenericResource>(), It.IsAny<CancellationToken>())).ReturnsAsync(resourceResult);

            this._resourceClient.Setup(p => p.Resources).Returns(this._resourceOperations.Object);

            var id = Guid.NewGuid();
            var insightsResource = new ResourceBaseExtended(location) { Id = id.ToString(), Name = name };

            var result = await this._service.CreateOrUpdateWebTestAsync(name, url, this._webTest.Object, this._resourceClient.Object, insightsResource).ConfigureAwait(false);
            result.Location.Should().BeEquivalentTo(location);
        }

        /// <summary>
        /// Tests whether the method should throw an exception or not.
        /// </summary>
        /// <param name="name">Web test name.</param>
        /// <param name="location">Resource location.</param>
        [Theory]
        [InlineData("WEBTEST_NAME", "Central US")]
        public void Given_NullParameters_CreateOrUpdateAlertsAsync_ShouldThrow_Exception(string name, string location)
        {
            var webTestResource = new ResourceBaseExtended(location);
            var insightsResource = new ResourceBaseExtended(location);

            Func<Task> func = async () => { var result = await this._service.CreateOrUpdateAlertsAsync(null, this._webTest.Object, this._insightsClient.Object, webTestResource, insightsResource).ConfigureAwait(false); };
            func.ShouldThrow<ArgumentNullException>();

            func = async () => { var result = await this._service.CreateOrUpdateAlertsAsync(name, null, this._insightsClient.Object, webTestResource, insightsResource).ConfigureAwait(false); };
            func.ShouldThrow<ArgumentNullException>();

            func = async () => { var result = await this._service.CreateOrUpdateAlertsAsync(name, this._webTest.Object, null, webTestResource, insightsResource).ConfigureAwait(false); };
            func.ShouldThrow<ArgumentNullException>();

            func = async () => { var result = await this._service.CreateOrUpdateAlertsAsync(name, this._webTest.Object, this._insightsClient.Object, null, insightsResource).ConfigureAwait(false); };
            func.ShouldThrow<ArgumentNullException>();

            func = async () => { var result = await this._service.CreateOrUpdateAlertsAsync(name, this._webTest.Object, this._insightsClient.Object, webTestResource, null).ConfigureAwait(false); };
            func.ShouldThrow<ArgumentNullException>();
        }
    }
}