using System.Xml.Serialization;

namespace Aliencube.Azure.Insights.WebTests.Models.Serialisation
{
    /// <summary>
    /// This represents the web test validation rule parameter entity to be serialised in the <see cref="WebTestConfiguration"/> class.
    /// </summary>
    public class WebTestValidationRuleParameter
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="WebTestValidationRuleParameter"/> class.
        /// </summary>
        public WebTestValidationRuleParameter()
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="WebTestValidationRuleParameter"/> class.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="value">Parameter value.</param>
        public WebTestValidationRuleParameter(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [XmlAttribute()]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        [XmlAttribute()]
        public string Value { get; set; }
    }
}