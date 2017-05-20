using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace AvmSmartHome.NET
{
    [XmlRoot(ElementName = "devicelist")]
    public class DeviceList
    {
        [XmlElement(ElementName = "device")]
        public List<Device> Device { get; set; }

        [XmlAttribute(AttributeName = "version")]
        public string Version { get; set; }

        public DeviceList()
        {

        }
    }
}
