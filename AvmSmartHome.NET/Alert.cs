using System.Xml.Serialization;

namespace AvmSmartHome.NET
{
    [XmlRoot(ElementName = "alert")]
    public class Alert
    {
        [XmlElement(ElementName = "state")]
        public State State { get; set; }

        public Alert()
        {

        }
    }
}
