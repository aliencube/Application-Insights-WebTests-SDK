namespace Aliencube.Azure.Insights.WebTests.Models
{
    /// <summary>
    /// This represents the properties entity for multi-step web test.
    /// </summary>
    public class MultiStepWebTestProperties : WebTestProperties
    {
        /// <summary>
        /// Gets or sets the <see cref="MultiStepWebTestConfiguration"/> object.
        /// </summary>
        public override WebTestConfiguration Configuration { get; set; }
    }
}