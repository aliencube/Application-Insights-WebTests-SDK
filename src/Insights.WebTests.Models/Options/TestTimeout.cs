namespace Aliencube.Azure.Insights.WebTests.Models.Options
{
    /// <summary>
    /// This specifies the test timeout in seconds.
    /// </summary>
    public enum TestTimeout
    {
        /// <summary>
        /// Indicates 30 seconds.
        /// </summary>
        _30Seconds = 30,

        /// <summary>
        /// Indicates 60 seconds.
        /// </summary>
        _60Seconds = 60,

        /// <summary>
        /// Indicates 90 seconds.
        /// </summary>
        _90Seconds = 90,

        /// <summary>
        /// Indicates 120 seconds.
        /// </summary>
        _120Seconds = 120,
    }
}