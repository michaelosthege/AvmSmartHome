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

        private int temperatureRaw;

        public TemperatureSetting(int temperatureRaw)
        {
            this.temperatureRaw = temperatureRaw;
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
        public int Value { get { return temperatureRaw; } set { temperatureRaw = value; OnPropertyChanged(); } }

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
                        double temp = temperatureRaw * 0.5;
                        return $"{temp}°C";
                }
            }
        }

        public bool IsOn { get { return temperatureRaw != 253; } }

        public override string ToString()
        {
            return Name;
        }
    }

}
