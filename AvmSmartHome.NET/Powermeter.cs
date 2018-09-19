using System.Xml.Serialization;

namespace AvmSmartHome.NET
{
    [XmlRoot(ElementName = "powermeter")]
    public class Powermeter
    {
        [XmlElement(ElementName = "power")]
        /// <summary>
        /// Aktuelle Leistung in mW (wird etwa alle 2 Minuten aktualisiert)
        /// </summary>
        public double PowerRaw { get; set; }
        public double Power
        {
            get { return PowerRaw * 0.001; }
            set { PowerRaw = value / 0.001; }
        }

        /// <summary>
        /// Absoluter Verbrauch seit Inbetriebnahme in Wh
        /// </summary>
        [XmlElement(ElementName = "energy")]
        public double Energy { get; set; }


        [XmlElement(ElementName = "voltage")]

        /// <summary>
        /// Aktuelle Spannung in mV (wird etwa alle 2 Minuten aktualisiert)
        /// </summary>
        public double VoltageRaw { get; set; }
        public double Voltage
        {
            get { return VoltageRaw * 0.001; }
            set { VoltageRaw = value / 0.001; }
        }

        public Powermeter()
        {

        }
    }
}
