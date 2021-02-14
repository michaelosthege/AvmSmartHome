using System;
using System.Xml.Serialization;

namespace AvmSmartHome.NET
{
    [XmlRoot(ElementName = "device")]
    public class Device
    {
        [XmlElement(ElementName = "present")]
        public string Present { get; set; }

        [XmlIgnore]
        public bool IsPresent { get { return Present == "1"; } }

        [XmlElement(ElementName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Batterieladezustand in Prozent (Optional)
        /// </summary>
        [XmlElement(ElementName = "battery")]
        public int Battery { get; set; }

        /// <summary>
        /// 0 oder 1: Batterieladezustand niedrig - bitte Batterie wechseln (Optional)
        /// </summary>
        [XmlElement(ElementName = "batterylow")]
        public State BatteryLow { get; set; }

        [XmlElement(ElementName = "etsiunitinfo")]
        public HanfunDevice HanfunDevice { get; set; }

        [XmlElement(ElementName = "hkr")]
        public RadiatorController RadiatorController { get; set; }

        [XmlElement(ElementName = "switch")]
        public Switch Switch { get; set; }

        [XmlElement(ElementName = "powermeter")]
        public Powermeter Powermeter { get; set; }

        [XmlElement(ElementName = "temperature")]
        public Temperature Temperature { get; set; }

        [XmlElement(ElementName = "alert")]
        public Alert Alert { get; set; }
      
        [XmlElement(ElementName = "simpleonoff")]
        public SimpleOnOff SimpleOnOff { get; set; }

        [XmlElement(ElementName = "levelcontrol")]
        public LevelControl LevelControl { get; set; }

        [XmlElement(ElementName = "colorcontrol")]
        public ColorControl ColorControl { get; set; }

        /// <summary>
        /// Taster
        /// ACHTUNG: Ein <device> kann gegebenenfalls mehrere <button>-Knoten haben. Der FRITZ!DECT 400 hat <button>-Knoten.
        /// </summary>
        [XmlElement(ElementName = "button")]
        public Button[] Button { get; set; }

        [XmlAttribute(AttributeName = "identifier")]
        public string Identifier { get; set; }

        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }

        [XmlAttribute(AttributeName = "functionbitmask")]
        public string FunctionBitmask { get; set; }

        [XmlAttribute(AttributeName = "fwversion")]
        public string FirmwareVersion { get; set; }

        [XmlAttribute(AttributeName = "manufacturer")]
        public string Manufacturer { get; set; }

        [XmlAttribute(AttributeName = "productname")]
        public string ProductName { get; set; }

        public DeviceFunctionClass DeviceFunctionClass
        {
            get
            {
                return (DeviceFunctionClass)Enum.ToObject(typeof(DeviceFunctionClass), Int32.Parse(FunctionBitmask));
            }
        }

        public Device()
        {

        }

        public override string ToString()
        {
            return Name;
        }
    }
}
