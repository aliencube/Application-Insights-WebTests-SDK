namespace Aliencube.Azure.Insights.WebTests.Models.Options
{
    /// <summary>
    /// This specifies the authentication type of the HTTP request header.
    /// </summary>
    public enum AuthType
    {
        /// <summary>
        /// Identifies no authentication is declared.
        /// </summary>
        None = 0,

        /// <summary>
        /// Identifies Basic authentication is declared.
        /// </summary>
        Basic = 1,

        /// <summary>
        /// Identifies Bearer authentication is declared.
        /// </summary>
        Bearer = 2
    }
}