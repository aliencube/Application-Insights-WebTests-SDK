using System.Xml.Serialization;

namespace Aliencube.Azure.Insights.WebTests.Models.Serialisation
{
    /// <summary>
    /// This represents the web test item request header entity to be serialised in the <see cref="WebTestConfiguration"/> class.
    /// </summary>
    public class WebTestItemRequestHeader
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="WebTestItemRequestHeader"/> class.
        /// </summary>
        public WebTestItemRequestHeader()
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="WebTestItemRequestHeader"/> class.
        /// </summary>
        /// <param name="name">Header name.</param>
        /// <param name="value">Header value.</param>
        public WebTestItemRequestHeader(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }

        /// <summary>
        /// Gets or sets the header name.
        /// </summary>
        [XmlAttribute()]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the header value.
        /// </summary>
        [XmlAttribute()]
        public string Value { get; set; }
    }
}