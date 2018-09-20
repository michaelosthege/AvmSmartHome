namespace AvmSmartHome.NET
{
    public class VoltageValue : ValueBase
    {
        public override string Unit => "V";

        public VoltageValue() { }

        public VoltageValue(string rawValue) : base(rawValue) { }

        public VoltageValue(double rawValue) : base(rawValue) { }

        /// <summary>
        /// Converts the raw value from mV into V.
        /// </summary>
        public override void Convert(double rawValue)
        {
            Value = rawValue * 0.001;
        }
    }
}