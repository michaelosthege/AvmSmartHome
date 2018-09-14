using System.Xml.Serialization;

namespace AvmSmartHome.NET
{
    [XmlRoot(ElementName = "hkr")]
    public class RadiatorController
    {
        [XmlElement(ElementName = "tist")]
        public int CurrentTemperatureRaw { get; set; }

        [XmlIgnore]
        public TemperatureSetting CurrentTemperature
        {
            get { return new TemperatureSetting(CurrentTemperatureRaw); }
            set { CurrentTemperatureRaw = value.Value; }
        }

        [XmlElement(ElementName = "tsoll")]
        public int TargetTemperatureRaw { get; set; }

        [XmlIgnore]
        public TemperatureSetting TargetTemperature
        {
            get { return new TemperatureSetting(TargetTemperatureRaw); }
            set { TargetTemperatureRaw = value.Value; }
        }

        [XmlElement(ElementName = "komfort")]
        public int ComfortTemperatureRaw { get; set; }

        [XmlIgnore]
        public TemperatureSetting ComfortTemperature
        {
            get { return new TemperatureSetting(ComfortTemperatureRaw); }
            set { ComfortTemperatureRaw = value.Value; }
        }

        [XmlElement(ElementName = "absenk")]
        public int LoweringTemperatureRaw { get; set; }

        [XmlIgnore]
        public TemperatureSetting LoweringTemperature
        {
            get { return new TemperatureSetting(LoweringTemperatureRaw); }
            set { LoweringTemperatureRaw = value.Value; }
        }

        /// <summary>
        /// 0 oder 1: Batterieladezustand niedrig - bitte Batterie wechseln
        /// </summary>
        [XmlElement(ElementName = "batterylow")]
        public State BatteryLow { get; set; }

        /// <summary>
        /// 0 oder 1: Fenster-ofen erkannt
        /// </summary>
        [XmlElement(ElementName = "windowopenactiv")]
        public State WindowOpenActive { get; set; }

        /// <summary>
        /// 0/1 - Tastensperre über UI/API ein nein/ja(leer bei unbekannt oder Fehler
        /// </summary>
        [XmlElement(ElementName = "lock")]
        public State Lock { get; set; }

        /// <summary>
        /// 0/1 - Tastensperre direkt am Gerät ein nein/ja(leer bei unbekannt oder Fehler)
        /// </summary>
        [XmlElement(ElementName = "devicelock")]
        public State DeviceLock { get; set; }

        /// <summary>
        /// nächste Temperaturänderung
        /// </summary>
        [XmlElement(ElementName = "nextchange")]
        public NextChange NextChange { get; set; }

        [XmlElement(ElementName = "summeractive")]
        public State SummerActive { get; set; }

        [XmlElement(ElementName = "holidayactive")]
        public State HolidayActive { get; set; }

        [XmlElement(ElementName = "battery")]
        public int Battery { get; set; }

        /// <summary>
        /// Fehlercodes die der HKR liefert (bspw. wenn es bei der Installation des HKRs Problem gab):
        /// 0: kein Fehler
        /// 1: Keine Adaptierung möglich.Gerät korrekt am Heizkörper montiert?
        /// 2: Ventilhub zu kurz oder Batterieleistung zu schwach.Ventilstößel per Hand mehrmals öfnen und schließen oder neue Batterien einsetzen.
        /// 3: Keine Ventilbewegung möglich.Ventilstößel frei?
        /// 4: Die Installation wird gerade vorbereitet.
        /// 5: Der Heizkörperregler ist im Installationsmodus und kann auf das Heizungsventil montiert werden.
        /// 6: Der Heizkörperregler passt sich nun an den Hub des Heizungsventils an.
        /// </summary>
        [XmlElement(ElementName = "errorcode")]
        public ErrorCodes ErrorCode { get; set; }

        /// <summary>
        /// nächste Temperaturänderung
        /// </summary>
        [XmlElement(ElementName = "groupinfo")]
        public GroupInfo GroupInfo { get; set; }

    }
}
