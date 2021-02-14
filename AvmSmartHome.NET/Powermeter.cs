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

        [XmlIgnore]
        public PowerValue Power
        {
            get { return new PowerValue(PowerRaw); }
        }

        /// <summary>
        /// Absoluter Verbrauch seit Inbetriebnahme in Wh
        /// </summary>
        [XmlElement(ElementName = "energy")]
        public double EnergyRaw { get; set; }
        [XmlIgnore]
        public EnergyValue Energy
        {
            get { return new EnergyValue(EnergyRaw); }
        }


        [XmlElement(ElementName = "voltage")]

        /// <summary>
        /// Aktuelle Spannung in mV (wird etwa alle 2 Minuten aktualisiert)
        /// </summary>
        public double VoltageRaw { get; set; }

        [XmlIgnore]
        public VoltageValue Voltage
        {
            get { return new VoltageValue(VoltageRaw); }
        }

        public Powermeter()
        {

        }
    }
}
