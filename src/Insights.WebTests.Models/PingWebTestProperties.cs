using Aliencube.Azure.Insights.WebTests.Models.Options;

namespace Aliencube.Azure.Insights.WebTests.Models
{
    /// <summary>
    /// This represents the properties entity for ping web test.
    /// </summary>
    public class PingWebTestProperties : WebTestProperties
    {
        /// <summary>
        /// Gets the web test kind.
        /// </summary>
        public override string Kind => TestKind.Ping;
    }
}