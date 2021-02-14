using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AvmSmartHome.NET
{
    [XmlRoot(ElementName = "temperature")]
    public class TemperatureStatistics
    {
        [XmlElement(ElementName = "stats")]
        public Statistics<TemperatureValue> Stats { get; set; }
    }

    [XmlRoot(ElementName = "voltage")]
    public class VoltageStatistics
    {
        [XmlElement(ElementName = "stats")]
        public Statistics<VoltageValue> Stats { get; set; }
    }

    [XmlRoot(ElementName = "power")]
    public class PowerStatistics
    {
        [XmlElement(ElementName = "stats")]
        public Statistics<PowerValue> Stats { get; set; }
    }

    [XmlRoot(ElementName = "stats")]
    public class Statistics<T> where T : ValueBase
    {
        /// <summary>
        /// This timestamp is used as base of the raw values to enable calculating the correct time of each value in <see cref="CreateStatisticValueList"/>.
        /// </summary>
        private DateTime TimeOfCreation = DateTime.Now;

        /// <summary>
        /// Anzahl der Werte
        /// </summary>
        [XmlAttribute(AttributeName = "count")]
        public int Count { get; set; }

        /// <summary>
        /// zeitlicher Abstand/Auflösung in Sekunden
        /// </summary>
        [XmlAttribute(AttributeName = "grid")]
        public int Grid { get; set; }

        /// <summary>
        /// Der Inhalt ist eine kommaseparierte Liste von Werten. Werte mit „-“ sind unbekannt/undefiniert.
        /// </summary>
        [XmlText]
        public String RawValue { get; set; }

        private List<StatisticValue<T>> values = null;

        [XmlIgnore]
        public List<StatisticValue<T>> Values
        {
            get
            {
                if (values == null)
                {
                    values = CreateStatisticValueList();
                }
                return values;
            }
        }

        private List<StatisticValue<T>> CreateStatisticValueList()
        {
            if (string.IsNullOrEmpty(RawValue))
            {
                return new List<StatisticValue<T>>();
            }

            string[] rawValues = RawValue.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            List<StatisticValue<T>> values = new List<StatisticValue<T>>();

            // raw values are time DESC ordered, afterwards the list will be time ASC ordered
            for (int i = rawValues.Length - 1; i >= 0; i--)
            {
                StatisticValue<T> statisticValue = new StatisticValue<T>();
                // first item of the raw values (newest value) will use TimeOfCreation as is
                statisticValue.Time = TimeOfCreation.Subtract(new TimeSpan(0, 0, i * Grid));
                // use the constructor for parsing a string into the target value
                statisticValue.Value = (T)Activator.CreateInstance(typeof(T), new[] { rawValues[i] });
                values.Add(statisticValue);
            }
            return values;
        }
    }
}