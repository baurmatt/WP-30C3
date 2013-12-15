using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;
using SharpGIS;

namespace _30C3.scheduleModel
{
    [XmlRoot("schedule")]
    public class schedule
    {
        /// <summary>
        /// Property which holds the schedule version
        /// </summary>
        [XmlElement("version")]
        public string Version { get; set; }

        /// <summary>
        /// Property which holds all infos about the conference itself
        /// </summary>
        [XmlElement("conference")]
        public conference Conference { get; set; }
        
        /// <summary>
        /// Property which holds all days of the conference
        /// </summary>
        [XmlElement("day")]
        public List<day> Days { get; set; }

        /// <summary>
        /// Property which returns all speakers in the schedule
        /// </summary>
        [XmlIgnore]
        public List<person> Speaker
        {
            get
            {
                return this.Days.SelectMany(day => day.Rooms.SelectMany(@event => @event.Events.SelectMany(person => person.Persons))).ToList<person>().Distinct().ToList<person>();
            }
        }

        /// <summary>
        /// Property which returns all events in the schedule
        /// </summary>
        [XmlIgnore]
        public List<@event> Events
        {
            get
            {
                return this.Days.SelectMany(obj => obj.Events).ToList();
            }
        }

        /// <summary>
        /// Methode which puts the events after 0 o'clock at the end of the schedule and adds the date to each event
        /// </summary>
        public void Clean()
        {
            foreach (day _day in this.Days)
            {
                foreach (room _room in _day.Rooms)
                {
                    if (_room.Events.Count > 0)
                    {
                        List<@event> TempEventList = new List<@event>();
                        for (int i = 0; i < _room.Events.Count; i++)
                        {
                            // Add Date to event
                            _room.Events[i].Date = _day.Date;
                            if (_room.Events[i].Start.Hour == 00)
                            {
                                @event tempevent = _room.Events[i];
                                _room.Events.Remove(tempevent);
                                _room.Events.Insert(_room.Events.Count, tempevent);
                            }
                        }
                    }
                }
            }
        }
    }
}
