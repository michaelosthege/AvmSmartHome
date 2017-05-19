using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace AvmSmartHome.NET
{
    public class Right : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        private RightLevel _Name;
        [XmlElement]
        public RightLevel Name { get { return _Name; } set { _Name = value; OnPropertyChanged(); } }

        private int _Access;
        [XmlElement]
        public int Access { get { return _Access; } set { _Access = value; OnPropertyChanged(); } }

        public override string ToString()
        {
            return $"{Name} ({Access})";
        }
    }
    public enum RightLevel
    {
        BoxAdmin,
        Dial,
        Phone,
        HomeAuto,
        NAS,
        App
    }
}
