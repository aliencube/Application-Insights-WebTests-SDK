using System.IO;
using System.Xml.Serialization;

using Aliencube.Azure.Insights.WebTests.Models.Serialisation;

namespace Aliencube.Azure.Insights.WebTests.Models.Extensions
{
    /// <summary>
    /// This represents the extensions entity for the <see cref="WebTestConfiguration"/> class.
    /// </summary>
    public static class WebTestExtensions
    {
        /// <summary>
        /// Serialises the <see cref="WebTest"/> instance in XML.
        /// </summary>
        /// <param name="value"><see cref="WebTest"/> instance to serialise.</param>
        /// <returns>Returns serialised string.</returns>
        public static string ToXml(this WebTest value)
        {
            if (value == null)
            {
                return null;
            }

            var serialiser = new XmlSerializer(typeof(WebTest));

            using (var stream = new MemoryStream())
            using (var reader = new StreamReader(stream))
            {
                serialiser.Serialize(stream, value);
                stream.Position = 0;

                var xml = reader.ReadToEnd();
                return xml;
            }
        }
    }
}