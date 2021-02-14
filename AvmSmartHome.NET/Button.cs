using System.Xml.Serialization;

namespace AvmSmartHome.NET
{
    /// <summary>
    /// Taster
    /// </summary>
    [XmlRoot(ElementName = "button")]
    public class Button
    {
        /// <summary>
        /// Eindeutige ID, AIN (Optional)
        /// </summary>
        [XmlAttribute(AttributeName = "identifier")]
        public string Identifier { get; set; }

        /// <summary>
        /// Interne Geräteid (Optional)
        /// </summary>
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Name (Optional)
        /// </summary>
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Zeitpunkt des letzten Tastendrucks, timestamp in Sekunden seit 1970, 0 oder leer bei unbekannt
        /// </summary>
        [XmlElement(ElementName = "lastpressedtimestamp")]
        public string LastPressedTimestamp { get; set; }

        public Button()
        {

        }
    }
}
