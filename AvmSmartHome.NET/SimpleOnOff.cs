using System.Xml.Serialization;

namespace AvmSmartHome.NET
{
    /// <summary>
    /// An-/Ausschaltbares Gerät/Steckdose/Lampe/Aktor
    /// </summary>
    [XmlRoot(ElementName = "simpleonoff")]
    public class SimpleOnOff
    {
        /// <summary>
        /// Aktueller Schaltzutand, 0:aus, 1:an
        /// </summary>
        [XmlElement(ElementName = "state")]
        public State State { get; set; }

        public SimpleOnOff()
        {

        }
    }
}
