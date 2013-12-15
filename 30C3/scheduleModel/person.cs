using System;
using System.Net;
using System.Xml.Serialization;
using System.IO;
using System.Windows.Media.Imaging;
using SharpGIS;
using System.Collections;
using System.Collections.Generic;

namespace _30C3.scheduleModel
{
    public class person : IEquatable<person>
    {
        public static readonly string PictureURL = "http://events.ccc.de/congress/2012/Fahrplan/images/";
        public static readonly string PictureSaveFolder = "persons";

        /// <summary>
        /// Property which holds the persons ID
        /// </summary>
        [XmlAttribute("id")]
        public int ID { get; set; }

        /// <summary>
        /// Property which holds the persons name
        /// </summary>
        [XmlText]
        public string Name { get; set; }

        /// <summary>
        /// Returns the potential event picture name (event-$ID-128x128.png)
        /// </summary>
        [XmlIgnore]
        public string PictureName { get { return "person-" + this.ID + "-128x128.png";} }

        /// <summary>
        /// Indicates if the picture is locally stored in the isolated storage
        /// </summary>
        [XmlIgnore]
        public Boolean IsPictureLocallyAvailable { 
            get { 
                IsolatedStorageFileHandler isfh = new IsolatedStorageFileHandler();
                return isfh.IsFileAvailable(PictureSaveFolder, this.PictureName);
            } 
        }

        /// <summary>
        /// Returns the actual picture
        /// </summary>
        [XmlIgnore]
        public BitmapImage Picture
        {
            get
            {
                if (!this.IsPictureLocallyAvailable)
                {
                    Uri uri = new Uri("/images/person-0-128x128.png", UriKind.Relative);
                    BitmapImage BitmapImage = new BitmapImage(uri);
                    return BitmapImage;
                }

                BitmapImage image = new BitmapImage();
                IsolatedStorageFileHandler isfh = new IsolatedStorageFileHandler();
                return isfh.GetImageFile(PictureSaveFolder, this.PictureName);
            }
        }

        /// <summary>
        /// Methode which downloads the persons picture
        /// </summary>
        public void DownloadPicture()
        {
            WebClient DownloadPictureWC = new GZipWebClient();
            DownloadPictureWC.OpenReadCompleted += new OpenReadCompletedEventHandler(DownloadPictureWC_OpenReadCompleted);
            Uri _Uri = new Uri(person.PictureURL + this.PictureName, UriKind.Absolute);
            System.Diagnostics.Debug.WriteLine(_Uri.ToString());
            DownloadPictureWC.OpenReadAsync(_Uri);
        }

        /// <summary>
        /// Event handler for the downloaded person picture
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DownloadPictureWC_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            IsolatedStorageFileHandler isfh = new IsolatedStorageFileHandler();
            try
            {
                isfh.SaveImageFile(PictureSaveFolder, this.PictureName, e.Result);
            }
            catch (System.Net.WebException ex)
            {
            }
        }

        /// <summary>
        /// Determines whether the given person equals this. 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Equals(person obj)
        {
            if (this.Name == obj.Name && this.ID == obj.ID)
                return true;

            return false;
        }

        /// <summary>
        /// Returns the HashCode of this person
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            int HashPersonName = this.Name == null ? 0 : this.Name.GetHashCode();
            int HashPersonID = this.ID.GetHashCode();
            return HashPersonName ^ HashPersonID;
        }
    }
}
