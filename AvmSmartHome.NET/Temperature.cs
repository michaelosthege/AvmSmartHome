using System.Xml.Serialization;

namespace AvmSmartHome.NET
{
    [XmlRoot(ElementName = "temperature")]
    public class Temperature
    {
        [XmlElement(ElementName = "celsius")]
        public double CelsiusSteps { get; set; }
        [XmlIgnore]
        public double Celsius { get { return CelsiusSteps / 10; } set { CelsiusSteps = value * 10; } }

        [XmlElement(ElementName = "offset")]
        public double OffsetSteps { get; set; }
        [XmlIgnore]
        public double Offset { get { return OffsetSteps / 10; } set { OffsetSteps = value * 10; } }

        public Temperature()
        {

        }
    }
}
