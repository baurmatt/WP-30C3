using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;
using SharpGIS;

namespace _30C3.scheduleModel
{
    public class @event
    {
        /// <summary>
        /// Property which holds the events ID
        /// </summary>
        [XmlAttribute("id")]
        public int ID { get; set; }

        /// <summary>
        /// Property which holds the start date of the event as DateTime
        /// </summary>
        [XmlIgnore]
        public DateTime Start { get; private set; }

        /// <summary>
        /// Property which holds the start date of the event as string
        /// </summary>
        [XmlElement("start")]
        public string StartString
        {
            get
            {
                System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-us");
                return this.Start.ToString(@"HH\:mm", enUS);
            }
            set
            {
                
                this.Start = DateTime.Parse(value);
            }
        }

        /// <summary>
        /// Property which holds the duration of the event as TimeSpan
        /// </summary>
        [XmlIgnore]
        public TimeSpan Duration { get; private set; }

        /// <summary>
        /// Property which holds the duration of the event as string
        /// </summary>
        [XmlElement("duration")]
        public string DurationString
        {
            get
            {
                System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-us");
                return this.Duration.ToString(@"hh\:mm", enUS);
            }
            set
            {
                string[] temparray = value.Split(':');
                this.Duration = new TimeSpan(int.Parse(temparray[0]), int.Parse(temparray[1]), 0);
            }
        }

        /// <summary>
        /// Property which holds the end of the event as DateTime
        /// </summary>
        [XmlIgnore]
        public DateTime End
        {
            get
            {
                return this.Start + this.Duration;
            }
        }

        /// <summary>
        /// Property which holds the end of the event as string
        /// Needs to be added by using the Clean() methode of the schedule
        /// </summary>
        [XmlElement("date", IsNullable=false)]
        public DateTime Date { get; set; }

        /// <summary>
        /// Property which holds the room of the event
        /// </summary>
        [XmlElement("room")]
        public string Room { get; set; }

        /// <summary>
        /// Property which holds the slug of the event
        /// </summary>
        [XmlElement("slug")]
        public string Slug { get; set; }

        /// <summary>
        /// Property which holds the title of the event
        /// </summary>
        [XmlElement("title")]
        public string Title { get; set; }

        /// <summary>
        /// Property which holds the subtitle of the event
        /// </summary>
        [XmlElement("subtitle")]
        public string SubTitle { get; set; }

        /// <summary>
        /// Property which holds the track of the event
        /// </summary>
        [XmlElement("track")]
        public string Track { get; set; }

        /// <summary>
        /// Property which holds the type of the event
        /// </summary>
        [XmlElement("type")]
        public string Type { get; set; }

        /// <summary>
        /// Property which holds the language of the event
        /// </summary>
        [XmlElement("language")]
        public string Language { get; set; }

        /// <summary>
        /// Property which holds the abstract of the event
        /// </summary>
        [XmlElement("abstract")]
        public string Abstract { get; set; }

        /// <summary>
        /// Property which holds the description of the event
        /// </summary>
        [XmlElement("description")]
        public string Description { get; set; }

        /// <summary>
        /// Property which holds the speakers of the event
        /// </summary>
        [XmlArray("persons")]
        [XmlArrayItem("person")]
        public List<person> Persons { get; set; }

        /// <summary>
        /// Property which holds the link of the event
        /// </summary>
        [XmlArray("links")]
        [XmlArrayItem("link")]
        public List<links> Links { get; set; }

        /// <summary>
        /// Returns the events picture name (event-$ID-128x128.png)
        /// </summary>
        [XmlIgnore]
        public string PictureName { get { return "event-" + this.ID + "-128x128.png"; } }

        /// <summary>
        /// Indicates if the picture is locally stored in the isolated storage
        /// </summary>
        [XmlIgnore]
        public Boolean IsPictureLocallyAvailable
        {
            get
            {
                IsolatedStorageFileHandler isfh = new IsolatedStorageFileHandler();
                return isfh.IsFileAvailable("persons", this.PictureName);
            }
        }

        /// <summary>
        /// Returns the actual picture name (event-0-128x128.png or event-$ID-128x128.png)
        /// </summary>
        [XmlIgnore]
        public string Picture { get { return this.IsPictureLocallyAvailable ? this.PictureName : "event-0-128x128.png"; } }

        /// <summary>
        /// Methode which downloads the events picture
        /// </summary>
        public void DownloadPicture()
        {
            WebClient DownloadPictureWC = new GZipWebClient();
            DownloadPictureWC.OpenReadCompleted += new OpenReadCompletedEventHandler(DownloadPictureWC_OpenReadCompleted);
            Uri _Uri = new Uri(person.PictureURL + this.PictureName, UriKind.Absolute);
            DownloadPictureWC.OpenReadAsync(_Uri);
        }

        /// <summary>
        /// Event handler for the downloaded event picture
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DownloadPictureWC_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            IsolatedStorageFileHandler isfh = new IsolatedStorageFileHandler();
            try
            {
                isfh.SaveImageFile("events", this.PictureName, e.Result);
            }
            catch (System.Net.WebException) { }
        }
    }
}
