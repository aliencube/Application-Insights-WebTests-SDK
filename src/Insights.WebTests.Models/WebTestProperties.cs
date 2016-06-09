using System.Collections.Generic;

using Hyak.Common;

using Newtonsoft.Json;

namespace Aliencube.Azure.Insights.WebTests.Models
{
    /// <summary>
    /// This represents the properties entity for web test.
    /// </summary>
    public abstract class WebTestProperties
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="WebTestProperties"/> class.
        /// </summary>
        protected WebTestProperties()
        {
            this.Description = string.Empty;
            this.Locations = new LazyList<WebTestLocation>();
        }

        /// <summary>
        /// Gets or sets the name of the web test.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the web test.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets the web test status whether it's enabled or disabled. This is a caculated value from <see cref="WebTestStatus"/>.
        /// </summary>
        public bool Enabled => this.TestStatus == WebTestStatus.Enabled;

        /// <summary>
        /// Gets or sets the web test status whether it's enabled or disabled.
        /// </summary>
        [JsonIgnore]
        public WebTestStatus TestStatus { get; set; }

        /// <summary>
        /// Gets the test frequency value in seconds. This is a calculated value from <see cref="TestFrequency"/>.
        /// </summary>
        public int Frequency => (int)this.TestFrequency * 60;

        /// <summary>
        /// Gets or sets the test frequency value in minutes.
        /// </summary>
        [JsonIgnore]
        public TestFrequency TestFrequency { get; set; }

        /// <summary>
        /// Gets the test timeout value in seconds. This is a read-only value from <see cref="TestTimeout"/>.
        /// </summary>
        public int Timeout => (int)this.TestTimeout;

        /// <summary>
        /// Gets or sets the test timeout value in seconds.
        /// </summary>
        [JsonIgnore]
        public TestTimeout TestTimeout { get; set; }

        /// <summary>
        /// Gets or sets the web test kind.
        /// </summary>
        public abstract string Kind { get; set; }

        /// <summary>
        /// Gets the value indicating whether to enable retries when the web test fails. This is a calculated value from <see cref="EnableRetriesForWebTestFailure"/>.
        /// </summary>
        public bool RetryEnabled => this.EnableRetriesForWebTestFailure == RetriesForWebTestFailure.Enable;

        /// <summary>
        /// Gets or sets the value indicating whether to enable retries when the web test fails.
        /// </summary>
        [JsonIgnore]
        public RetriesForWebTestFailure EnableRetriesForWebTestFailure { get; set; }

        /// <summary>
        /// Gets or sets the list of <see cref="WebTestLocation"/> objects.
        /// </summary>
        public IList<WebTestLocation> Locations { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="WebTestConfiguration"/> object.
        /// </summary>
        public abstract WebTestConfiguration Configuration { get; set; }

        /// <summary>
        /// Gets or sets the monitor Id.
        /// </summary>
        public string SyntheticMonitorId { get; set; }

        /// <summary>
        /// Returns a string which represents the object instance.
        /// </summary>
        /// <param name="properties"><see cref="WebTestProperties"/> instance.</param>
        public static implicit operator string(WebTestProperties properties)
        {
            return properties.ToString();
        }

        /// <summary>
        /// Converts the current object instance to a string.
        /// </summary>
        /// <returns>Returns a string which represents the object instance.</returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}