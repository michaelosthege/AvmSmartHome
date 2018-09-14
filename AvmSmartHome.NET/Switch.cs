using System;
using System.Xml.Serialization;

namespace AvmSmartHome.NET
{
    [XmlRoot(ElementName = "switch")]
    public class Switch
    {
        [XmlElement(ElementName = "state")]
        public State State { get; set; }
        [XmlIgnore]
        public bool? IsOn
        {
            get
            {
                switch (State)
                {
                    case State.On: return true;
                    case State.Off: return false;
                    case State.Unknown: return null;
                    default:
                        throw new NotImplementedException();
                }
            }
            set
            {
                switch (value)
                {
                    case true: State = State.On; break;
                    case false: State = State.Off; break;
                    case null: State = State.Unknown; break;
                }
            }
        }

        [XmlElement(ElementName = "mode")]
        public Mode Mode { get; set; }

        [XmlElement(ElementName = "lock")]
        public State Lock { get; set; }

        [XmlElement(ElementName = "devicelock")]
        public State DeviceLock { get; set; }
        [XmlIgnore]
        public bool? IsLocked
        {
            get
            {
                switch (DeviceLock)
                {
                    case State.On: return true;
                    case State.Off: return false;
                    case State.Unknown: return null;
                    default:
                        throw new NotImplementedException();
                }
            }
            set
            {
                switch (value)
                {
                    case true: DeviceLock = State.On; break;
                    case false: DeviceLock = State.Off; break;
                    case null: DeviceLock = State.Unknown; break;
                }
            }
        }

        public Switch()
        {

        }
    }
}
