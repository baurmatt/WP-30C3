using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Net;
using System.Xml.Serialization;
using System.Linq;

namespace _30C3.scheduleModel
{
    public class day
    {
        /// <summary>
        /// Property which holds the days date
        /// </summary>
        [XmlAttribute("date")]
        public DateTime Date { get; set; }

        /// <summary>
        /// Property which holds the days index number
        /// </summary>
        [XmlAttribute("index")]
        public int Index { get; set; }

        /// <summary>
        /// Property which holds all the rooms of this day
        /// </summary>
        [XmlElement("room")]
        public List<room> Rooms { get; set; }

        /// <summary>
        /// Property which returns all events of this day
        /// </summary>
        [XmlIgnore]
        public List<@event> Events
        {
            get
            {
                return this.Rooms.SelectMany(room => room.Events).ToList();
            }
        }
    }
}
