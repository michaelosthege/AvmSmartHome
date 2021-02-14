using System;

namespace AvmSmartHome.NET
{
    public class StatisticValue<T> where T : ValueBase
    {
        public DateTime Time { get; set; }

        public T Value { get; set; }

        public override string ToString()
        {
            return Time.ToString("d") + " " + Time.ToString("T") + " - " + Value.ToString();
        }
    }
}