using System;
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

    public enum HanfunInterfaces
    {
        [XmlEnum("277")]
        KeepAlive,
        [XmlEnum("256")]
        Alert,
        [XmlEnum("772")]
        SimpleButton
    }

    public enum HanfunUnitTypes
    {
        [XmlEnum("273")]
        SimpleButton,
        [XmlEnum("512")]
        SimpleDetector,
        [XmlEnum("513")]
        DoorOpenCloseDetector,
        [XmlEnum("514")]
        WindowOpenCloseDetector,
        [XmlEnum("515")]
        MotionDetector,
        [XmlEnum("518")]
        FloodDetector,
        [XmlEnum("519")]
        GlasBreakDetector,
        [XmlEnum("520")]
        VibrationDetector
    }

    /// <summary>
    /// functionbitmask: Bitmaske der Geräte-Funktionsklassen, beginnen mit Bit 0, es können mehrere Bits gesetzt sein
    /// Bit 0: HANFUN Gerät
    /// Bit 4: Alarm-Sensor
    /// Bit 6: Heizkörperregler
    /// Bit 7: Energie Messgerät
    /// Bit 8: Temperatursensor
    /// Bit 9: Schaltsteckdose
    /// Bit 10: AVM DECT Repeater
    /// Bit 11: Mikrofon
    /// Bit 13: HANFUN Unit
    /// Beispiel FD300: binär 101000000(320), Bit6(HKR) und Bit8(Temperatursensor) sind gesetzt
    /// </summary>
    [Flags]
    public enum DeviceFunctionClass
    {
        HanfunDevice = 1,
        AlertSensor = 1 << 4,
        HKR = 1 << 6,
        Powermeter = 1 << 7,
        Temperaturesensor = 1 << 8,
        Switch = 1 << 9,
        DectRepeater = 1 << 10,
        Microfon = 1 << 11,
        HanfunUnit = 1 << 13
    }

    /// <summary>
    /// Fehlercodes die der HKR liefert (bspw. wenn es bei der Installation des HKRs Problem gab):
    /// 0: kein Fehler
    /// 1: Keine Adaptierung möglich.Gerät korrekt am Heizkörper montiert?
    /// 2: Ventilhub zu kurz oder Batterieleistung zu schwach. Ventilstößel per Hand mehrmals öfnen und schließen oder neue Batterien einsetzen.
    /// 3: Keine Ventilbewegung möglich. Ventilstößel frei?
    /// 4: Die Installation wird gerade vorbereitet.
    /// 5: Der Heizkörperregler ist im Installationsmodus und kann auf das Heizungsventil montiert werden.
    /// 6: Der Heizkörperregler passt sich nun an den Hub des Heizungsventils an.
    /// </summary>
    public enum ErrorCodes
    {
        [XmlEnum("0")]
        NoError,
        [XmlEnum("1")]
        NoAdaptionPossible,
        [XmlEnum("2")]
        ShortValveLiftOrBatteryLow,
        [XmlEnum("3")]
        NoValveMovePossible,
        [XmlEnum("4")]
        Installing,
        [XmlEnum("5")]
        InstallationMode,
        [XmlEnum("6")]
        Initialization
    }

    /// <summary>
    /// Bitmaske
    /// 0x01 = HueSaturation-Mode
    /// 0x04 = Farbtemperatur-Mode
    /// </summary>
    [Flags]
    public enum SupportedModes
    {
        HueSaturationMode = 1,
        ColorTemperatureMode = 1 << 4
    }

    /// <summary>
    /// 1 (HueSaturation)
    /// 4 (Farbtemperatur)
    /// "" (leer → unbekannt)
    /// </summary>
    public enum CurrentModes
    {
        [XmlEnum("1")]
        HueSaturation,
        [XmlEnum("4")]
        ColorTemperature,
        [XmlEnum("")]
        Unknown
    }
    public enum SimpleOnOffStates
    {
        Off = 0,
        On = 1,
        Toggle = 2
    }
}
