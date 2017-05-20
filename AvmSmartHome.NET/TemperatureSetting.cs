using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AvmSmartHome.NET
{
    public class TemperatureSetting : INotifyPropertyChanged
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

        public TemperatureSetting(int tempwert)
        {
            this.tempwert = tempwert;
        }

        ///<summary>
        /// Temperatur-Wert in  0,5 °C
        ///   Wertebereich: 16 – 56 
        ///                  8 - 28 °C
        ///  16 : <= 8°C
        ///  17 :    8.5°C
        ///  18 :    9.0°C
        ///  ......
        ///  56 : >= 28°C
        ///  254: ON
        ///  253: OFF
        ///</summary>
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
