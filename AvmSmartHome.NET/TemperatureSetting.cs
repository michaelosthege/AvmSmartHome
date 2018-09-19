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

        private int _TemperatureRaw;

        ///<summary>
        /// Temperatur-Wert in  0,5 °C
        /// <para>Wertebereich: 16 – 56 (8 – 28 °C)</para>
        /// <para>16 : &lt;= 8°C</para>
        /// <para>17 :    8.5°C</para>
        /// <para>18 :    9.0°C</para>
        /// <para>...</para>
        /// <para>56 : &gt;= 28°C</para>
        /// <para>254: ON</para>
        /// <para>253: OFF</para>
        ///</summary>
        public int Value { get { return _TemperatureRaw; } set { _TemperatureRaw = value; OnPropertyChanged(); } }

        /// <summary>
        /// String des Temperatur-Werts (<see cref="Value"/>)
        /// </summary>
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
                        double temp = _TemperatureRaw * 0.5;
                        return $"{temp}°C";
                }
            }
        }

        public bool IsOn { get { return _TemperatureRaw != 253; } }

        public TemperatureSetting(int temperatureRaw)
        {
            this._TemperatureRaw = temperatureRaw;
        }

        public override string ToString()
        {
            return Name;
        }
    }

}
