namespace Aliencube.Azure.Insights.WebTests.Models.Options
{
    /// <summary>
    /// This specifies the test alert failure time window in minutes.
    /// </summary>
    public enum TestAlertFailureTimeWindow
    {
        /// <summary>
        /// Indicates 5 minutes.
        /// </summary>
        _5Minutes = 5,

        /// <summary>
        /// Indicates 10 minutes.
        /// </summary>
        _10Minutes = 10,

        /// <summary>
        /// Indicates 15 miniutes.
        /// </summary>
        _15Minutes = 15,
    }
}