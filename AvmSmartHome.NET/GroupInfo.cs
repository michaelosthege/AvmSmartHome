using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AvmSmartHome.NET
{
    [XmlRoot(ElementName = "groupinfo")]
    public class GroupInfo
    {
        /// <summary>
        /// interne id des Master/Chef-Schalters, 0 bei "keiner gesetzt"
        /// </summary>
        [XmlElement(ElementName = "masterdeviceid")]
        public int MasterDeviceId { get; set; }
        /// <summary>
        /// interne ids der Gruppenmitglieder, kommasepariert
        /// </summary>
        [XmlElement(ElementName = "members")]
        public String MemberIds { get; set; }

        [XmlIgnore]
        public List<int> Member
        {
            get
            {
                String[] ids = MemberIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                List<int> idList = new List<int>();
                foreach (String id in ids)
                {
                    idList.Add(Int32.Parse(id));
                }
                return idList;
            }
        }
    }
}