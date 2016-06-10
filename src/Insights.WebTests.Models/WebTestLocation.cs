using System;

namespace Aliencube.Azure.Insights.WebTests.Models
{
    /// <summary>
    /// This represents the location entity for web test.
    /// </summary>
    public class WebTestLocation
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="WebTestLocation"/> class.
        /// </summary>
        /// <param name="id">Location Id.</param>
        public WebTestLocation(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            this.Id = id;
        }

        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public string Id { get; set; }
    }
}