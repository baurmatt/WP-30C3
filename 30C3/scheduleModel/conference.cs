using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Xml.Serialization;
using System.Linq;

namespace _30C3.scheduleModel
{
    public class conference 
    {
        /// <summary>
        /// Property which holds the acronym of the conference
        /// </summary>
        [XmlElement("acronym")]
        public string Acronym { get; set; }

        /// <summary>
        /// Property which holds the title of the conference
        /// </summary>
        [XmlElement("title")]
        public string Title { get; set; }

        /// <summary>
        /// Property which holds the start date of the conference
        /// </summary>
        [XmlElement("start")]
        public DateTime Start { get; set; }

        /// <summary>
        /// Property which holds the end date of the conference
        /// </summary>
        [XmlElement("end")]
        public DateTime End { get; set; }

        /// <summary>
        /// Property which holds the amount of days the conference is running
        /// </summary>
        [XmlElement("days")]
        public int Days { get; set; }

        /// <summary>
        /// Property which holds the timeslot duration of the conference
        /// </summary>
        [XmlElement("timeslot_duration")]
        public TimeSpan TimeslotDuration { get; set; }
    }
}
