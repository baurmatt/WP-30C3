using System;
using System.Net;
using System.Xml.Serialization;
using System.Linq;

namespace _30C3.scheduleModel
{
    public class links
    {
        /// <summary>
        /// Property which holds the actual link
        /// </summary>
        [XmlAttribute("href")]
        public string Link { get; set; }

        /// <summary>
        /// Property which holds the links name
        /// </summary>
        [XmlText]
        public string LinkName { get; set; }
    }
}
