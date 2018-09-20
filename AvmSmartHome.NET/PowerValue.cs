namespace AvmSmartHome.NET
{
    public class PowerValue : ValueBase
    {
        public override string Unit => "W";

        public PowerValue() { }

        public PowerValue(string rawValue) : base(rawValue) { }

        public PowerValue(double rawValue) : base(rawValue) { }

        /// <summary>
        /// Converts the raw value from 0.01W into W.
        /// </summary>
        public override void Convert(double rawValue)
        {
            Value = rawValue * 0.01;
        }
    }
}