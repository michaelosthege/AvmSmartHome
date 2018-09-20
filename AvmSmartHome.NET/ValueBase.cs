namespace AvmSmartHome.NET
{
    public abstract class ValueBase
    {
        /// <summary>
        /// Unit of the value. Is used for <see cref="ToString"/>.
        /// </summary>
        public abstract string Unit { get; }

        public double Value { get; set; }

        public ValueBase() { }

        /// <summary>
        /// Use this constructor for initializing the <see cref="Value"/> property after parsing the raw value.
        /// Use the <see cref="Convert(double)"/> method to change the behavior of setting the <see cref="Value"/> property.
        /// </summary>
        public ValueBase(string rawValue)
        {
            if ("-".Equals(rawValue))
            {
                Value = double.NaN;
            }
            else
            {
                Convert(double.Parse(rawValue));
            }
        }

        /// <summary>
        /// Use this constructor for initializing the <see cref="Value"/> property with the specified raw value.
        /// Use the <see cref="Convert(double)"/> method to change the behavior of setting the <see cref="Value"/> property.
        /// </summary>
        public ValueBase(double rawValue)
        {
            Convert(rawValue);
        }

        /// <summary>
        /// Method sets the <see cref="Value"/> property with the specified raw value. Method can be overridden to enable conversion of the raw value.
        /// </summary>
        public virtual void Convert(double rawValue)
        {
            Value = rawValue;
        }

        public override string ToString()
        {
            return Value.ToString() + Unit;
        }
    }
}