using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

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
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="HttpRequestException">400, 403, 500</exception>
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
                string uri = $"{BaseURL}/login_sid.lua?username={Username}&response={response}";
                doc = await Helpers.GetAsync(uri);
                final = SessionInfo.FromXML(doc);
            }
            this.BlockTime = final.BlockTime;
            this.Rights = final.Rights;
            this.SID = final.SID;
            this.Challenge = final.Challenge;
            if (this.SID == "0000000000000000")
            {
                throw new UnauthorizedAccessException();
            }
        }

        public async Task LogoutAsync()
        {
            if (!HasSID)
            {
                return;
            }
            string uri = $"{BaseURL}/login_sid.lua?logout=test&sid={SID}";
            string doc = await Helpers.GetAsync(uri);
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
        /// <exception cref="HttpRequestException">400, 403, 500</exception>
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
        /// <exception cref="HttpRequestException">400, 403, 500</exception>
        /// <returns>XML</returns>
        public async Task<DeviceList> GetDeviceListInfosAsync()
        {
            string url = $"{BaseURL}/webservices/homeautoswitch.lua?switchcmd=getdevicelistinfos&sid={SID}";
            // Energie in Wh, "inval" wenn unbekannt
            string result = await Helpers.GetAsync(url);
            var deviceList = Helpers.DeserializeXML<DeviceList>(result);
            return deviceList;
        }

        /// <summary>
        /// Ermittelt Verbindungsstatus des Aktors.
        /// </summary>
        /// <param name="ain">AIN/MAC</param>
        /// <exception cref="HttpRequestException">400, 403, 500</exception>
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
        /// <exception cref="HttpRequestException">400, 403, 500</exception>
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
        /// <exception cref="HttpRequestException">400, 403, 500</exception>
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
        /// Gerät/Aktor/Lampe an-/ausschalten oder toggeln
        /// </summary>
        /// <param name="ain">AIN/MAC</param>
        /// <param name="simpleOnOffState">neuer Zustand</param>
        /// <exception cref="HttpRequestException">400, 403, 500</exception>
        /// <returns>neuer Zustand (bool)</returns>
        public async Task<string> SetSimpleOnOffAsync(string ain, SimpleOnOffStates simpleOnOffState)
        {
            string url = $"{BaseURL}/webservices/homeautoswitch.lua?ain={ain}&switchcmd=setsimpleonoff&onoff={simpleOnOffState}&sid={SID}";
            return await Helpers.GetAsync(url);
        }

        /// <summary>
        /// Dimm-, Höhen-, Helligkeitbzw. Niveau-Level einstellen
        /// </summary>
        /// <param name="ain">AIN/MAC</param>
        /// <param name="level">0(0%) bis 255(100%)</param>
        /// <exception cref="HttpRequestException">400, 403, 500</exception>
        /// <returns>neuer Zustand (bool)</returns>
        public async Task<string> SetLevelAsync(string ain, int level)
        {
            string url = $"{BaseURL}/webservices/homeautoswitch.lua?ain={ain}&switchcmd=setlevel&level={level}&sid={SID}";
            return await Helpers.GetAsync(url);
        }

        /// <summary>
        /// HueSaturation-Farbe einstellen
        /// Der HSV-Farbraum wird mit dem HueSaturation-Mode unterstützt. Der Hellwert(Value) kann über setlevel/setlevelpercentage
        /// konfiguriert werden, die Hueund Saturation-Werte sind hier konfigurierbar.
        /// </summary>
        /// <param name="ain">AIN/MAC</param>
        /// <param name="hue">in Grad, 0 bis 359 (0° bis 359°)</param>
        /// <param name="saturation">0(0%) bis 255(100%)</param>
        /// <param name="duration">Schnelligkeit der Änderung in 100ms. 0 sofort</param>
        /// <exception cref="HttpRequestException">400, 403, 500</exception>
        /// <returns>neuer Zustand (bool)</returns>
        public async Task<string> SetColorAsync(string ain, int hue, int saturation, int duration)
        {
            string url = $"{BaseURL}/webservices/homeautoswitch.lua?ain={ain}&switchcmd=setcolor&hue={hue}&saturation={saturation}&duration={duration}&sid={SID}";
            return await Helpers.GetAsync(url);
        }

        /// <summary>
        /// Farbtemperatur einstellen
        /// </summary>
        /// <param name="ain">AIN/MAC</param>
        /// <param name="temperature">in Kelvin, typisch im Bereich 2700 bis 6500</param>
        /// <param name="duration">Schnelligkeit der Änderung in 100ms. 0 sofort</param>
        /// <exception cref="HttpRequestException">400, 403, 500</exception>
        /// <returns>neuer Zustand (bool)</returns>
        public async Task<string> SetColorTemperatureAsync(string ain, int temperature, int duration)
        {
            string url = $"{BaseURL}/webservices/homeautoswitch.lua?ain={ain}&switchcmd=setcolortemperature&temperature={temperature}&duration={duration}&sid={SID}";
            return await Helpers.GetAsync(url);
        }

        /// <summary>
        /// Invertiert den Zustand einer Steckdose.
        /// </summary>
        /// <param name="ain">AIN/MAC</param>
        /// <exception cref="HttpRequestException">400, 403, 500</exception>
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
        /// <exception cref="HttpRequestException">400, 403, 500</exception>
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
        /// <exception cref="HttpRequestException">400, 403, 500</exception>
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
        /// <exception cref="HttpRequestException">400, 403, 500</exception>
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
        /// <exception cref="HttpRequestException">400, 403, 500</exception>
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
        /// <exception cref="HttpRequestException">400, 403, 500</exception>
        /// <returns>Temperatur</returns>
        public async Task<TemperatureSetting> GetHKRSollAsync()
        {
            string url = $"{BaseURL}/webservices/homeautoswitch.lua?&switchcmd=gethkrtsoll&sid={SID}";
            string result = await Helpers.GetAsync(url);
            int tempwert = Convert.ToInt16(result);
            TemperatureSetting temp = new TemperatureSetting(tempwert);
            return temp;
        }

        /// <summary>
        /// Für HKR-Zeitschaltung eingestellte Komforttemperatur.
        /// </summary>
        /// <exception cref="HttpRequestException">400, 403, 500</exception>
        /// <returns>Temperatur</returns>
        public async Task<TemperatureSetting> GetHKRKomfortAsync()
        {
            string url = $"{BaseURL}/webservices/homeautoswitch.lua?&switchcmd=gethkrkomfort&sid={SID}";
            string result = await Helpers.GetAsync(url);
            int tempwert = Convert.ToInt16(result);
            TemperatureSetting temp = new TemperatureSetting(tempwert);
            return temp;
        }

        /// <summary>
        /// Für HKR-Zeitschaltung eingestellte Spartemperatur.
        /// </summary>
        /// <exception cref="HttpRequestException">400, 403, 500</exception>
        /// <returns>Temperatur</returns>
        public async Task<TemperatureSetting> GetHKRSparAsync()
        {
            string url = $"{BaseURL}/webservices/homeautoswitch.lua?&switchcmd=gethkrabsenk&sid={SID}";
            string result = await Helpers.GetAsync(url);
            int tempwert = Convert.ToInt16(result);
            TemperatureSetting temp = new TemperatureSetting(tempwert);
            return temp;
        }

        /// <summary>
        /// HKR Solltemperatur einstellen.
        /// </summary>
        /// <exception cref="HttpRequestException">400, 403, 500</exception>
        /// <returns>Temperatur</returns>
        public async Task SetHKRSollAsync(TemperatureSetting temp)
        {
            int tempwert = temp.Value;
            string url = $"{BaseURL}/webservices/homeautoswitch.lua?&param={tempwert}&switchcmd=sethkrtsoll&sid={SID}";
            string result = await Helpers.GetAsync(url);
        }

        /// <summary>
        /// Liefert die grundlegenden Statistiken (Temperatur, Spannung, Leistung) des Aktors. Für Aktoren mit Temperatursensor gibt es Temperatur-Werte. Für Aktoren mit Energie Messgerät gibt es Spannungs- und Leistungs-Werte.
        /// </summary>
        /// <param name="ain">Der ain-HTTP-Parameter identifiziert den Aktor</param>
        /// <exception cref="HttpRequestException">400, 403, 500</exception>
        /// <returns>Statisitiken</returns>
        public async Task<DeviceStatistics> GetDeviceStatisticsAsync(string ain)
        {
            string url = $"{BaseURL}/webservices/homeautoswitch.lua?&ain={ain}&switchcmd=getbasicdevicestats&sid={SID}";
            string result = await Helpers.GetAsync(url);
            return Helpers.DeserializeXML<DeviceStatistics>(result);
        }
    }

}
