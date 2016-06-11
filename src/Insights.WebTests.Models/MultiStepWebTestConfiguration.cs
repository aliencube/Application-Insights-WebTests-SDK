namespace Aliencube.Azure.Insights.WebTests.Models
{
    /// <summary>
    /// This represents the configuration entity for multi-step web test.
    /// </summary>
    public class MultiStepWebTestConfiguration : WebTestConfiguration
    {
        /// <summary>
        /// Gets the web test XML serialised value.
        /// </summary>
        public override string WebTest { get; }
    }
}