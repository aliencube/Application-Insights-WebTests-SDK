using System;

using Aliencube.Azure.Insights.WebTests.Models.Exceptions;

using FluentAssertions;

using Xunit;

namespace Aliencube.Azure.Insights.WebTests.Models.Tests
{
    public class PingWebTestConfigurationTest
    {
        [Theory]
        [InlineData("name", "http://localhost", 120, true, 200, "text")]
        public void Given_NullParameter_Constructor_ShouldThrow_Exception(string name, string url, int timeout, bool parseDependentRequests, int expectedHttpStatusCode, string text)
        {
            Action action = () => { var instance = new PingWebTestConfiguration(null, url, timeout, parseDependentRequests, expectedHttpStatusCode, text); };
            action.ShouldThrow<ArgumentNullException>();

            action = () => { var instance = new PingWebTestConfiguration(name, null, timeout, parseDependentRequests, expectedHttpStatusCode, text); };
            action.ShouldThrow<ArgumentNullException>();

            action = () => { var instance = new PingWebTestConfiguration(name, url, 0, parseDependentRequests, expectedHttpStatusCode, text); };
            action.ShouldThrow<ArgumentOutOfRangeException>();

            action = () => { var instance = new PingWebTestConfiguration(name, url, timeout, parseDependentRequests, -1, text); };
            action.ShouldThrow<ArgumentOutOfRangeException>();

            action = () => { var instance = new PingWebTestConfiguration(name, url, timeout, parseDependentRequests, 1, text); };
            action.ShouldThrow<InvalidHttpStatusCodeException>();
        }

        [Theory]
        [InlineData("name", "http://localhost", 120, true, 200, "text")]
        public void Given_Parameters_Constructor_ShouldThrow_NoException(string name, string url, int timeout, bool parseDependentRequests, int expectedHttpStatusCode, string text)
        {
            Action action = () => { var instance = new PingWebTestConfiguration(name, url, timeout, parseDependentRequests, expectedHttpStatusCode, text); };
            action.ShouldNotThrow<Exception>();
        }

        [Theory]
        [InlineData("name", "http://localhost", 120, true, 200, "text")]
        public void Given_Parameters_WebTest_ShouldReturn_SerialisedXml(string name, string url, int timeout, bool parseDependentRequests, int expectedHttpStatusCode, string text)
        {
            var instance = new PingWebTestConfiguration(name, url, timeout, parseDependentRequests, expectedHttpStatusCode, text);
            instance.WebTest.Should().StartWithEquivalent("<?xml");
            instance.WebTest.Should().ContainEquivalentOf("<WebTest");
            instance.WebTest.Should().ContainEquivalentOf("<Items>");
            instance.WebTest.Should().ContainEquivalentOf("<Request");
            instance.WebTest.Should().ContainEquivalentOf("<ValidationRules>");
            instance.WebTest.Should().ContainEquivalentOf("<RuleParameters");
        }
    }
}