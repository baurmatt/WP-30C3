using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Net;
using System.Xml.Serialization;
using System.Linq;

namespace _30C3.scheduleModel
{
    public class room
    {
        /// <summary>
        /// Constructor which initializes the Events list,
        /// so no NullArgument exception gets throw if there are no events  
        /// </summary>
        public room()
        {
            this.Events = new List<@event>();
        }

        /// <summary>
        /// Property which holds the rooms name
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// Property which holds the events of the room
        /// </summary>
        [XmlElement("event")]
        public List<@event> Events { get; set; }
    }
}
