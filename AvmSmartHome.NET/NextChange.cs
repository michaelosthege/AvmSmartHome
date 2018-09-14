using System;
using System.Xml.Serialization;

namespace AvmSmartHome.NET
{
    [XmlRoot(ElementName = "nextchange")]
    public class NextChange
    {
        /// <summary>
        /// timestamp in Sekunden seit 1970, 0 bei unbekannt
        /// </summary>
        [XmlElement(ElementName = "endperiod")]
        public long EndPeriod { get; set; }

        /// <summary>
        /// Time in UTC
        /// </summary>
        public DateTime EndPeriodTimestamp
        {
            get { return DateTimeOffset.FromUnixTimeSeconds(EndPeriod).DateTime; }
            set { EndPeriod = new DateTimeOffset(value).ToUnixTimeSeconds(); }
        }

        /// <summary>
        /// Zieltemperatur, Wertebereich siehe tsoll(255/0xf ist unbekannt/undefiniert)
        /// </summary>
        [XmlElement(ElementName = "tchange")]
        public int TargetTemperatureRaw { get; set; }

        [XmlIgnore]
        public TemperatureSetting TargetTemperature
        {
            get { return new TemperatureSetting(TargetTemperatureRaw); }
            set { TargetTemperatureRaw = value.Value; }
        }
    }
}