using System.Xml.Serialization;

namespace AvmSmartHome.NET
{
    [XmlRoot(ElementName = "etsiunitinfo")]
    public class HanfunDevice
    {
        /// <summary>
        /// interne GeräteID des dezugehörigen HANFUN Gerätes
        /// </summary>
        [XmlElement(ElementName = "etsideviceid")]
        public string DeviceId { get; set; }

        /// <summary>
        /// HANFUN Unit Typ
        /// 273 = SIMPLE_BUTTON
        /// 512 = SIMPLE_DETECTOR
        /// 513 = DOOR_OPEN_CLOSE_DETECTOR
        /// 514 = WINDOW_OPEN_CLOSE_DETECTOR
        /// 515 = MOTION_DETECTOR
        /// 518 = FLOOD_DETECTOR
        /// 519 = GLAS_BREAK_DETECTOR
        /// 520 = VIBRATION_DETECTOR
        /// </summary>
        [XmlElement(ElementName = "unittype")]
        public HanfunUnitTypes UnitType { get; set; }

        /// <summary>
        /// HANFUN Interfaces
        /// 277 = KEEP_ALIVE
        /// 256 = ALERT
        /// 772 = SIMPLE_BUTTON
        /// </summary>
        [XmlElement(ElementName = "interfaces")]
        public HanfunInterfaces Interface { get; set; }

        public HanfunDevice()
        {

        }
    }
}
