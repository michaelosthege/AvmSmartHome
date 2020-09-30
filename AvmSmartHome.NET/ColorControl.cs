using System.Xml.Serialization;

namespace AvmSmartHome.NET
{
    /// <summary>
    /// Lampe mit einstellbarer Farbe/Farbtemperatur
    /// </summary>
    [XmlRoot(ElementName = "colorcontrol")]
    public class ColorControl
    {
        /// <summary>
        /// Hue-Wert in Grad, 0 bis 359 (0° bis 359°), Achtung nur, wenn current_mode == 1(HueSaturation) ansonsten leer/undefiniert. Der HSV-Farbraum wird mit dem HueSaturation-Mode unterstützt. Der Hellwert(Value) kann über setlevel/setlevelpercentage konfiguriert werden, die Hue- und Saturation-Werte sind über setcolor konfigurierbar.
        /// </summary>
        [XmlElement(ElementName = "hue")]
        public int Hue { get; set; }

        /// <summary>
        /// Saturation-Wert von 0(0%) bis 255(100%), Achtung nur, wenn current_mode == 1(HueSaturation) ansonsten leer/undefiniert
        /// </summary>
        [XmlElement(ElementName = "saturation")]
        public int Saturation { get; set; }

        /// <summary>
        /// Wert in Kelvin, ein typischer Wertebereich geht von etwa 2700° bis 6500°
        /// </summary>
        [XmlElement(ElementName = "temperature")]
        public int Temperature { get; set; }

        /// <summary>
        /// Bitmaske -- 0x01 = HueSaturation-Mode, 0x04 = Farbtemperatur-Mode
        /// </summary>
        [XmlAttribute(AttributeName = "supported_modes")]
        public SupportedModes SupportedModes { get; set; }

        /// <summary>
        /// 1(HueSaturation), 4 (Farbtemperatur) oder ""(leer → unbekannt)
        /// </summary>
        [XmlAttribute(AttributeName = "current_mode")]
        public CurrentModes CurrentMode { get; set; }

        public ColorControl()
        {

        }
    }
}