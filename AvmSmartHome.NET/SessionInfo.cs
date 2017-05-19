using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml.Schema;

namespace AvmSmartHome.NET
{
    public class SessionInfo : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Properties
        private string _Hostname = "fritz.box";
        public string Hostname { get { return _Hostname; } set { _Hostname = value; OnPropertyChanged(); } }

        private string BaseURL { get { return $"http://{_Hostname}"; } }

        private string _Username = "iot";
        public string Username { get { return _Username; } set { _Username = value; OnPropertyChanged(); } }

        private string _Password;
        public string Password { get { return _Password; } set { _Password = value; OnPropertyChanged(); } }

        private string _SID;
        [XmlElement]
        public string SID { get { return _SID; } set { _SID = value; OnPropertyChanged(); } }

        private string _Challenge;
        [XmlElement]
        public string Challenge { get { return _Challenge; } set { _Challenge = value; OnPropertyChanged(); } }

        private int _BlockTime;
        [XmlElement]
        public int BlockTime { get { return _BlockTime; } set { _BlockTime = value; OnPropertyChanged(); } }


        private List<Right> _Rights;
        [XmlArray]
        public List<Right> Rights { get { return _Rights; } set { _Rights = value; OnPropertyChanged(); } }


        [XmlIgnore]
        public bool HasSID { get { return SID != "0000000000000000"; } }
        #endregion

        public SessionInfo()
        {

        }

        public SessionInfo(string username, string password, string hostname = "fritz.box")
        {
            this.Hostname = hostname;
            this.Username = username;
            this.Password = password;
        }


        /// <summary>
        /// Acquire a session ID (SID) according to the procedure described in
        /// https://avm.de/fileadmin/user_upload/Global/Service/Schnittstellen/AVM_Technical_Note_-_Session_ID.pdf
        /// </summary>
        /// <returns>SessionInfo</returns>
        public async Task AuthenticateAsync()
        {
            string doc = await Helpers.GetAsync($"{BaseURL}/login_sid.lua");
            SessionInfo intermediate = SessionInfo.FromXML(doc);
            string sid = intermediate.SID;
            SessionInfo final = null;
            if (!intermediate.HasSID && intermediate.BlockTime == 0)
            {
                string response = $"{intermediate.Challenge}-{Helpers.ComputeMD5($"{intermediate.Challenge}-{Password}", Encoding.Unicode)}";
                string uri = $"{BaseURL}/login_sid.lua?page=&username={Username}&response={response}";
                doc = await Helpers.GetAsync(uri);
                final = SessionInfo.FromXML(doc);
            }
            this.BlockTime = final.BlockTime;
            this.Rights = final.Rights;
            this.SID = final.SID;
            this.Challenge = final.Challenge;
        }

        /// <summary>
        /// Deserialize a SessionInfo from an XML string.
        /// </summary>
        /// <param name="xml">string of XML, downloaded from the server</param>
        /// <returns>SessionInfo</returns>
        private static SessionInfo FromXML(string xml)
        {
            xml = xml.Replace("<Name>", "<Right><Name>");
            xml = xml.Replace("</Access>", "</Access></Right>");
            return Helpers.DeserializeXML<SessionInfo>(xml);
        }

        /// <summary>
        /// Liefert die kommaseparierte AIN/MAC Liste aller bekannten Steckdosen.
        /// </summary>
        /// <returns>Liste von AIN/MAC</returns>
        public async Task<string[]> GetSwitchesAsync()
        {
            string url = $"{BaseURL}/webservices/homeautoswitch.lua?switchcmd=getswitchlist&sid={SID}";
            // kommaseparierte AIN/MAC-Liste, leer wenn keine Steckdose bekannt
            string result = await Helpers.GetAsync(url);
            return result.Trim().Split(',');
        }

        /// <summary>
        /// Liefert die grundlegenden Informationen aller SmartHome-Geräte.
        /// </summary>
        /// <returns>XML</returns>
        public async Task<string> GetDeviceListInfosAsync()
        {
            string url = $"{BaseURL}/webservices/homeautoswitch.lua?switchcmd=getdevicelistinfos&sid={SID}";
            // Energie in Wh, "inval" wenn unbekannt
            string result = await Helpers.GetAsync(url);
            return result;
        }

        /// <summary>
        /// Ermittelt Verbindungsstatus des Aktors.
        /// </summary>
        /// <param name="ain">AIN/MAC</param>
        /// <returns>Verbindungsstatus (bool)</returns>
        public async Task<bool> GetSwitchPresentAsync(string ain)
        {
            string url = $"{BaseURL}/webservices/homeautoswitch.lua?ain={ain}&switchcmd=getswitchpresent&sid={SID}";
            string result = await Helpers.GetAsync(url);
            // "0" oder "1" für Gerät nicht verbunden bzw. verbunden.
            // Bei Verbindungsverlust wechselt der Zustand erst mit einigen Minuten Verzögerung zu "0".
            return result == "1";
        }

        /// <summary>
        /// Ermittelt Schaltzustand der Steckdose.
        /// </summary>
        /// <param name="ain">AIN/MAC</param>
        /// <returns>Schaltzustand (bool)</returns>
        public async Task<bool?> GetSwitchStateAsync(string ain)
        {
            string url = $"{BaseURL}/webservices/homeautoswitch.lua?ain={ain}&switchcmd=getswitchstate&sid={SID}";
            // "1" or "0" or "inval"
            string result = await Helpers.GetAsync(url);
            switch (result.Trim())
            {
                case "inval": return null;
                case "0": return false;
                case "1": return true;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Schaltet eine Steckdose EIN/AUS.
        /// </summary>
        /// <param name="ain">AIN/MAC</param>
        /// <param name="setOn">neuer Zustand</param>
        /// <returns>neuer Zustand (bool)</returns>
        public async Task<bool> SetSwitchAsync(string ain, bool setOn)
        {
            string onoff = setOn ? "on" : "off";
            string url = $"{BaseURL}/webservices/homeautoswitch.lua?ain={ain}&switchcmd=setswitch{onoff}&sid={SID}";
            // "1" or "0"
            string result = await Helpers.GetAsync(url);
            return result == (setOn ? "1" : "0");
        }

        /// <summary>
        /// Invertiert den Zustand einer Steckdose.
        /// </summary>
        /// <param name="ain">AIN/MAC</param>
        /// <returns>neuer Zustand (bool)</returns>
        public async Task<bool> ToggleSwitchAsync(string ain)
        {
            string url = $"{BaseURL}/webservices/homeautoswitch.lua?ain={ain}&switchcmd=setswitchtoggle&sid={SID}";
            // "1" or "0"
            string result = await Helpers.GetAsync(url);
            return result == "1";
        }

        /// <summary>
        /// Ermittelt aktuell über die Steckdose entnommene Leistung.
        /// </summary>
        /// <param name="ain">AIN/MAC</param>
        /// <returns>Leistung in mW</returns>
        public async Task<double> GetSwitchPowerAsync(string ain)
        {
            string url = $"{BaseURL}/webservices/homeautoswitch.lua?ain={ain}&switchcmd=getswitchpower&sid={SID}";
            // Leistung in mW, "inval" wenn unbekannt
            string result = await Helpers.GetAsync(url);
            try
            {
                return Convert.ToDouble(result);
            }
            catch
            {
                return double.NaN;
            }
        }

        /// <summary>
        /// Liefert die über die Steckdose entnommene Ernergiemenge seit
        /// Erstinbetriebnahme oder Zurücksetzen der Energiestatistik.
        /// </summary>
        /// <param name="ain">AIN/MAC</param>
        /// <returns>Leistung in mW</returns>
        public async Task<double> GetSwitchEnergyAsync(string ain)
        {
            string url = $"{BaseURL}/webservices/homeautoswitch.lua?ain={ain}&switchcmd=getswitchenergy&sid={SID}";
            // Energie in Wh, "inval" wenn unbekannt
            string result = await Helpers.GetAsync(url);
            try
            {
                return Convert.ToDouble(result);
            }
            catch
            {
                return double.NaN;
            }
        }

        /// <summary>
        /// Liefert Bezeichner des Aktors.
        /// </summary>
        /// <param name="ain">AIN/MAC</param>
        /// <returns>Name</returns>
        public async Task<string> GetSwitchNameAsync(string ain)
        {
            string url = $"{BaseURL}/webservices/homeautoswitch.lua?ain={ain}&switchcmd=getswitchname&sid={SID}";
            // Energie in Wh, "inval" wenn unbekannt
            string result = await Helpers.GetAsync(url);
            return result.Trim();
        }

        /// <summary>
        /// Letzte Temperaturinformation des Aktors.
        /// </summary>
        /// <param name="ain">AIN/MAC</param>
        /// <returns>Temperatur in °C</returns>
        public async Task<double> GetSwitchTemperatureAsync(string ain)
        {
            string url = $"{BaseURL}/webservices/homeautoswitch.lua?ain={ain}&switchcmd=gettemperature&sid={SID}";
            // Temperatur-Wert in 0,1 °C, negative und positive Werte möglich Bsp. „200“  bedeutet 20°C
            string result = await Helpers.GetAsync(url);
            try
            {
                return Convert.ToDouble(result) / 10;
            }
            catch
            {
                return double.NaN;
            }
        }

        /// <summary>
        /// Für HKR aktuell eingestellte Solltemperatur.
        /// </summary>
        /// <returns>Temperatur</returns>
        public async Task<Temperature> GetHKRSollAsync()
        {
            string url = $"{BaseURL}/webservices/homeautoswitch.lua?&switchcmd=gethkrtsoll&sid={SID}";
            // Temperatur-Wert in  0,5 °C
            //   Wertebereich: 16 – 56 
            //                  8 - 28 °C
            //  16 : <= 8°C
            //  17 :    8.5°C
            //  18 :    9.0°C
            //  ......
            //  56 : >= 28°C
            //  254: ON
            //  253: OFF
            string result = await Helpers.GetAsync(url);
            int tempwert = Convert.ToInt16(result);
            Temperature temp = new Temperature(tempwert);
            return temp;
        }

        /// <summary>
        /// Für HKR-Zeitschaltung eingestellte Komforttemperatur.
        /// </summary>
        /// <returns>Temperatur</returns>
        public async Task<Temperature> GetHKRKomfortAsync()
        {
            string url = $"{BaseURL}/webservices/homeautoswitch.lua?&switchcmd=gethkrkomfort&sid={SID}";
            // Temperatur-Wert in  0,5 °C
            //   Wertebereich: 16 – 56 
            //                  8 - 28 °C
            //  16 : <= 8°C
            //  17 :    8.5°C
            //  18 :    9.0°C
            //  ......
            //  56 : >= 28°C
            //  254: ON
            //  253: OFF
            string result = await Helpers.GetAsync(url);
            int tempwert = Convert.ToInt16(result);
            Temperature temp = new Temperature(tempwert);
            return temp;
        }

        /// <summary>
        /// Für HKR-Zeitschaltung eingestellte Spartemperatur.
        /// </summary>
        /// <returns>Temperatur</returns>
        public async Task<Temperature> GetHKRSparAsync()
        {
            string url = $"{BaseURL}/webservices/homeautoswitch.lua?&switchcmd=gethkrabsenk&sid={SID}";
            // Temperatur-Wert in  0,5 °C
            //   Wertebereich: 16 – 56 
            //                  8 - 28 °C
            //  16 : <= 8°C
            //  17 :    8.5°C
            //  18 :    9.0°C
            //  ......
            //  56 : >= 28°C
            //  254: ON
            //  253: OFF
            string result = await Helpers.GetAsync(url);
            int tempwert = Convert.ToInt16(result);
            Temperature temp = new Temperature(tempwert);
            return temp;
        }

        /// <summary>
        /// HKR Solltemperatur einstellen.
        /// </summary>
        /// <returns>Temperatur</returns>
        public async Task SetHKRSollAsync(Temperature temp)
        {
            int tempwert = temp.Value;
            string url = $"{BaseURL}/webservices/homeautoswitch.lua?&param={tempwert}&switchcmd=sethkrtsoll&sid={SID}";
            string result = await Helpers.GetAsync(url);
        }


    }

}
