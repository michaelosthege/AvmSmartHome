using System.Xml.Serialization;

namespace AvmSmartHome.NET
{
    [XmlRoot(ElementName = "powermeter")]
    public class Powermeter
    {
        [XmlElement(ElementName = "power")]
        /// <summary>
        /// Wert in 0,001 W (aktuelle Leistung, wird etwa alle 2 Minuten aktualisiert)
        /// </summary>
        public double PowerRaw { get; set; }
        public double Power
        {
            get { return PowerRaw * 0.001; }
            set { PowerRaw = value / 0.001; }
        }

        /// <summary>
        /// Wert in 1.0 Wh (absoluter Verbrauch seit Inbetriebnahme)
        /// </summary>
        [XmlElement(ElementName = "energy")]
        public double Energy { get; set; }


        [XmlElement(ElementName = "voltage")]

        /// <summary>
        /// Wert in 0,001 V (aktuelle Spannung, wird etwa alle 2 Minuten aktualisiert)
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
