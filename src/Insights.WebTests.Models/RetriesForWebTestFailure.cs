namespace Aliencube.Azure.Insights.WebTests.Models
{
    /// <summary>
    /// This specifies web test failure retry.
    /// </summary>
    public enum RetriesForWebTestFailure
    {
        /// <summary>
        /// Identifies retry disabled.
        /// </summary>
        Disable = 0,

        /// <summary>
        /// Identifies retry enabled.
        /// </summary>
        Enable = 1,
    }
}