using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows.Media.Imaging;
using System.Windows;
using _30C3.scheduleModel;
using System.Xml.Serialization;

namespace _30C3
{
    public class IsolatedStorageFileHandler
    {
        private IsolatedStorageFile myStorage;

        /// <summary>
        /// Property which holds the schedule file name in the isolated storage
        /// </summary>
        public readonly string ScheduleFileName = "schedule.en.xml";

        /// <summary>
        /// Constructor which initializes the isolated storage object
        /// </summary>
        public IsolatedStorageFileHandler()
        {
            myStorage = IsolatedStorageFile.GetUserStoreForApplication();
        }

        /// <summary>
        /// Checks if a file is available in the isolated storage
        /// </summary>
        /// <param name="Folder">folder name, just "" if no folder</param>
        /// <param name="FileName">file name</param>
        /// <returns></returns>
        public Boolean IsFileAvailable(string Folder, string FileName)
        {
            string PathToFile = string.IsNullOrEmpty(Folder) ? FileName : Folder + "\\" + FileName;
            return myStorage.FileExists(PathToFile);
        }

        /// <summary>
        /// Saves the schedule to the isolated storage
        /// </summary>
        /// <param name="schedule">schedule</param>
        public void SaveSchedule(schedule schedule)
        {
            using (IsolatedStorageFileStream stream = this.myStorage.OpenFile(ScheduleFileName, FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(schedule));
                serializer.Serialize(stream, schedule);
            }
        }

        /// <summary>
        /// Gets the schedule from the isolated storage
        /// </summary>
        /// <returns>schedule</returns>
        public schedule GetSchedule()
        {
            using (IsolatedStorageFileStream stream = this.myStorage.OpenFile(ScheduleFileName, FileMode.Open))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(schedule));
                schedule tempschedule = (schedule)serializer.Deserialize(stream);
                return tempschedule;
            }
        }

        /// <summary>
        /// Saves a image to the isolated storage
        /// </summary>
        /// <param name="Folder">The folder in the isolated storage</param>
        /// <param name="FileName">The filename in the isolated storage</param>
        /// <param name="BitMapFileStream">The image file as a Stream</param>
        public void SaveImageFile(string FolderName, string FileName, Stream BitMapFileStream)
        {
            string PathToFile = string.IsNullOrEmpty(FolderName) ? FileName : FolderName + "\\" + FileName;

            if (!myStorage.DirectoryExists(FolderName))
                myStorage.CreateDirectory(FolderName);

            if (myStorage.FileExists(PathToFile))
                myStorage.DeleteFile(PathToFile);

            Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    try
                    {
                        IsolatedStorageFileStream fileStream = myStorage.CreateFile(PathToFile);
                    BitmapImage BitmapImage = new BitmapImage();
                    BitmapImage.SetSource(BitMapFileStream);
                    WriteableBitmap wb = new WriteableBitmap(BitmapImage);
                    Extensions.SaveJpeg(wb, fileStream, wb.PixelWidth, wb.PixelHeight, 0, 100);
                    fileStream.Close();
                    }
                    catch { }
                });    
        }

        /// <summary>
        /// Get a image from the isolated storage
        /// </summary>
        /// <param name="Folder">The folder in the isolated storage</param>
        /// <param name="FileName">The filename in the isolated storage</param>
        /// <returns>Image as BitmapImage</returns>
        public BitmapImage GetImageFile(string FolderName, string FileName)
        {
            string PathToFile = string.IsNullOrEmpty(FolderName) ? FileName : FolderName + "\\" + FileName;

            IsolatedStorageFileStream fileStream = myStorage.OpenFile(PathToFile, FileMode.Open, FileAccess.Read);
            BitmapImage BitmapImage = new BitmapImage();
            BitmapImage.SetSource(fileStream);
            return BitmapImage;
        }

        /// <summary>
        /// Deletes a folder in the isolated storage
        /// </summary>
        /// <param name="FolderName">The folder name</param>
        public void DeleteFolder(string FolderName)
        {
            if (myStorage.DirectoryExists(FolderName))
                myStorage.DeleteDirectory(FolderName);
        }

        /// <summary>
        /// Deletes a file in the isolated storage
        /// </summary>
        /// <param name="FolderName">The folder name</param>
        /// <param name="FileName">The file name</param>
        public void DeleteFile(string FolderName, string FileName)
        {
            string PathToFile = string.IsNullOrEmpty(FolderName) ? FileName : FolderName + "\\" + FileName;

            if (myStorage.FileExists(PathToFile))
                myStorage.DeleteFile(PathToFile);

        }

    }
}
