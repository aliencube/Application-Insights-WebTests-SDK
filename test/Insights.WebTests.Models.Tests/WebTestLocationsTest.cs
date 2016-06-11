using System;

using Aliencube.Azure.Insights.WebTests.Models.Options;

using FluentAssertions;

using Xunit;

namespace Aliencube.Azure.Insights.WebTests.Models.Tests
{
    public class WebTestLocationsTest
    {
        [Fact]
        public void Given_InvalidLocation_GetWebTestLocations_ShouldThrow_Exception()
        {
            Action action = () => { var locations = WebTestLocations.GetWebTestLocations(TestLocations.None); };
            action.ShouldThrow<ArgumentException>();
        }

        [Theory]
        [InlineData(TestLocations.UsIlChicago)]
        public void Given_Location_GetWebTestLocations_ShouldReturn_Results(TestLocations locations)
        {
            var results = WebTestLocations.GetWebTestLocations(locations);
            results.Count.Should().Be(1);
        }

        [Theory]
        [InlineData(TestLocations.UsIlChicago | TestLocations.AuSydney)]
        public void Given_Locations_GetWebTestLocations_ShouldReturn_Results(TestLocations locations)
        {
            var results = WebTestLocations.GetWebTestLocations(locations);
            results.Count.Should().Be(2);
        }
    }
}