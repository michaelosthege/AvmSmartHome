using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AvmSmartHome.NET
{
    public class Temperature : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        private int _Value;
        private int tempwert;

        public Temperature(int tempwert)
        {
            this.tempwert = tempwert;
        }

        public int Value { get { return _Value; } set { _Value = value; OnPropertyChanged(); } }

        public string Name
        {
            get
            {
                switch (Value)
                {
                    case 253: return "Aus";
                    case 254: return "An";
                    case 16: return "<= 8°C";
                    case 56: return ">= 28°C";
                    default:
                        double temp = 8;
                        temp += (_Value - 16) * 0.5;
                        return $"{temp:.1f}°C";
                }
            }
        }

        public bool IsOn { get { return _Value != 253; } }

        public override string ToString()
        {
            return base.ToString();
        }
    }

}
