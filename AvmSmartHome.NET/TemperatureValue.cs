namespace AvmSmartHome.NET
{
    public class TemperatureValue : ValueBase
    {
        public override string Unit => "°C";

        public TemperatureValue() { }

        public TemperatureValue(string rawValue) : base(rawValue) { }

        public TemperatureValue(double rawValue) : base(rawValue) { }

        /// <summary>
        /// Converts the raw value from 0.1°C into °C.
        /// </summary>
        public override void Convert(double rawValue)
        {
            Value = rawValue * 0.1;
        }
    }
}