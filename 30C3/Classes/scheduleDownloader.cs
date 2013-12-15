using SharpGIS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Threading;
using _30C3.scheduleModel;
using System.Text.RegularExpressions;

namespace _30C3
{
    public class scheduleDownloader
    {
        /// <summary>
        /// Propety which indicates if the class is already downloading something
        /// </summary>
        public bool Downloading { get; private set; }

        /// <summary>
        /// Property which holds the schedules URL
        /// </summary>
        private static readonly string scheduleURL = "http://events.ccc.de/congress/2013/Fahrplan/schedule.xml";

        /// <summary>
        /// Property which holds the schedules version URL
        /// </summary>
        private static readonly string scheduleVersionURL = "http://events.ccc.de/congress/2013/Fahrplan/schedule.html";

        private Action<Exception, schedule> DownloadScheduleCallback;
        private Action<Exception, string> DownloadScheduleVersionCallback;

        private WebClient _WebClient;

        /// <summary>
        /// Constructor which initializes the WebClient object
        /// </summary>
        public scheduleDownloader()
        {
            _WebClient = new GZipWebClient();
        }

        /// <summary>
        /// Property which indicates if the schedule needs to be downloaded or not.
        /// </summary>
        public bool DownloadRequired
        {
            get
            {
                AppSettings AppSettings = new AppSettings();
                if ((AppSettings.LastTimeDownloaded + AppSettings.ScheduleDownloadInterval.Value) <= DateTime.Now)
                {
                    if (AppSettings.JustWifiDownload == true)
                    {
                        return NetworkUtilty.IsWiFiInternetConnection;
                    }
                    else
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Method which gets the current schedule version
        /// </summary>
        /// <param name="DownloadScheduleVersionCallback"></param>
        public void GetScheduleVersion(Action<Exception, string> DownloadScheduleVersionCallback)
        {
            if (DownloadScheduleVersionCallback == null)
                throw new ArgumentNullException("Callback");


            this.DownloadScheduleVersionCallback = DownloadScheduleVersionCallback;
            this._WebClient.DownloadStringCompleted += delegate(object sender, DownloadStringCompletedEventArgs e)
            {
                try
                {
                    this.Downloading = false;
                    Match match = Regex.Match(e.Result, @"<p\sclass=""release"">.*?Version\s(.*?)\n</p>",RegexOptions.Singleline);
                    this.DownloadScheduleVersionCallback(null, match.Groups[1].Value);
                }
                catch (WebException ex)
                {
                    this.Downloading = false;
                    this.DownloadScheduleVersionCallback(ex, null);
                }
            };
            this.Downloading = true;
            this._WebClient.DownloadStringAsync(new Uri(scheduleVersionURL, UriKind.Absolute));
        }


        /// <summary>
        /// Method which downloads the current schedule
        /// </summary>
        /// <param name="DownloadScheduleCallback"></param>
        public void Download(Action<Exception, schedule> DownloadScheduleCallback)
        {
            if (DownloadScheduleCallback == null)
                throw new ArgumentNullException("Callback");

            ManualResetEvent Waiter = new ManualResetEvent(false);

            string versionGlobal = "";
            this.GetScheduleVersion((ex, version) =>
                {
                    versionGlobal = version;
                    Waiter.Set();
                });
            
            Waiter.WaitOne();
            
            this.DownloadScheduleCallback = DownloadScheduleCallback;
            this._WebClient.DownloadStringCompleted += delegate(object sender, DownloadStringCompletedEventArgs e)
            {
                try
                {
                    this.Downloading = false;
                    schedule tempschedule = DeserializeToSchedule(e.Result);
                    this.DownloadScheduleCallback(null, tempschedule);
                }
                catch (WebException ex)
                {
                    this.Downloading = false;
                    this.DownloadScheduleCallback(ex, null);
                }
            };
            this.Downloading = true;
            this._WebClient.DownloadStringAsync(new Uri(scheduleURL, UriKind.Absolute));
        }

        /// <summary>
        /// Method which deserializes the download XML schedule
        /// </summary>
        /// <param name="rawXML"></param>
        /// <returns></returns>
        private schedule DeserializeToSchedule(string rawXML)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(schedule));
            XDocument document = XDocument.Parse(rawXML);
            schedule tempschedule = (schedule)serializer.Deserialize(document.CreateReader());
            tempschedule.Clean();
            return tempschedule;
        }

    }
}
