using System.Xml.Serialization;

namespace AvmSmartHome.NET
{
    [XmlRoot(ElementName = "devicestats")]
    public class DeviceStatistics
    {
        [XmlElement(ElementName = "temperature")]
        public TemperatureStatistics Temperature { get; set; }

        [XmlElement(ElementName = "voltage")]
        public VoltageStatistics Voltage { get; set; }

        [XmlElement(ElementName = "power")]
        public PowerStatistics Power { get; set; }
    }
}