using System.Xml.Serialization;

namespace AvmSmartHome.NET
{
    [XmlRoot(ElementName = "temperature")]
    public class Temperature
    {
        [XmlElement(ElementName = "celsius")]
        public double CurrentValueRaw { get; set; }

        [XmlIgnore]
        public TemperatureValue CurrentValue
        {
            get { return new TemperatureValue(CurrentValueRaw); }
        }

        [XmlElement(ElementName = "offset")]
        public double OffsetRaw { get; set; }

        [XmlIgnore]
        public TemperatureValue Offset
        {
            get { return new TemperatureValue(OffsetRaw); }
        }

        public Temperature()
        {

        }
    }
}
