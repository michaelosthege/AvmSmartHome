using System.Xml.Serialization;

namespace AvmSmartHome.NET
{
    public enum Mode
    {
        [XmlEnum("auto")]
        Auto,
        [XmlEnum("manuell")]
        Manual,
        [XmlEnum("")]
        Unknown
    }
    public enum State
    {
        [XmlEnum("1")]
        On,
        [XmlEnum("0")]
        Off,
        [XmlEnum("")]
        Unknown
    }
}
