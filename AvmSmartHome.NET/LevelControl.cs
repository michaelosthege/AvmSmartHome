using System.Xml.Serialization;

namespace AvmSmartHome.NET
{
    /// <summary>
    /// Gerät mit einstellbarem Dimm-, Höhen-, Helligkeit- bzw. Niveau-Level
    /// </summary>
    [XmlRoot(ElementName = "levelcontrol")]
    public class LevelControl
    {
        /// <summary>
        /// Level/Niveau von 0(0%) bis 255(100%)
        /// </summary>
        [XmlElement(ElementName = "level")]
        public int Level { get; set; }

        /// <summary>
        /// Level/Niveau in Prozent, 0 bis 100 Prozent
        /// </summary>
        [XmlElement(ElementName = "levelpercentage")]
        public int LevelPercentage { get; set; }

        public LevelControl()
        {

        }
    }
}