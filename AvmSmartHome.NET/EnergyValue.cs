namespace AvmSmartHome.NET
{
    public class EnergyValue : ValueBase
    {

        public override string Unit => "Wh";

        public EnergyValue() { }

        public EnergyValue(string rawValue) : base(rawValue) { }

        public EnergyValue(double rawValue) : base(rawValue) { }
    }
}