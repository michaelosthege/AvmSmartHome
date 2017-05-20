using System.Xml.Serialization;

namespace AvmSmartHome.NET
{
    [XmlRoot(ElementName = "powermeter")]
    public class Powermeter
    {
        [XmlElement(ElementName = "power")]
        /// <summary>
        /// Derzeitige Leistung in mW
        /// </summary>
        public double Power { get; set; }
        
        [XmlElement(ElementName = "energy")]
        
        /// <summary>
        /// Energieverbrauch in Wh.
        /// </summary>
        public double Energy { get; set; }
        
        public Powermeter()
        {

        }
    }
}
