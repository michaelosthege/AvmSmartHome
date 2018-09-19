using System.Xml.Serialization;

namespace AvmSmartHome.NET
{
    [XmlRoot(ElementName = "alert")]
    public class Alert
    {
        /// <summary>
        /// 0/1 - letzter übermittelter Alarmzustand (leer bei unbekannt oder Fehler)
        /// </summary>
        [XmlElement(ElementName = "state")]
        public State State { get; set; }

        public Alert()
        {

        }
    }
}
