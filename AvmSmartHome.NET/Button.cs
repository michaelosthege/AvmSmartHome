using System.Xml.Serialization;

namespace AvmSmartHome.NET
{
    [XmlRoot(ElementName = "button")]
    public class Button
    {
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
