using System;
using System.IO.IsolatedStorage;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace _30C3
{
    public class AppSettings
    {
        private IsolatedStorageSettings settings;

        // The key names of our settings
        const string FirstStartKeyName = "FirstStart";
        const string JustWifiDownloadKeyName = "JustWifiDownload";
        const string LastTimeDownloadedKeyName = "LastTimeDownloaded";
        const string ScheduleDownloadIntervalKeyName = "ScheduleDownloadInterval";
        const string ScheduleVersionDownloadedKeyName = "ScheduleVersionDownloaded";

        // The default value of our settings
        const bool FirstStartDefault = true;
        const bool JustWifiDownloadDefault = true;
        DateTime LastTimeDownloadedDefault = new DateTime(2012, 12, 20, 08, 38, 0);
        KeyValuePair<string, TimeSpan> ScheduleDownloadIntervalDefault = new KeyValuePair<string, TimeSpan>("On Startup", new TimeSpan(0));
        const string ScheduleVersionDownloadedDefault = "0.99";

        /// <summary>
        /// Constructor that gets the application settings.
        /// </summary>
        public AppSettings()
        {
            // Get the settings for this application.
            if(!System.ComponentModel.DesignerProperties.IsInDesignTool)
                settings = IsolatedStorageSettings.ApplicationSettings;
        }

        /// <summary>
        /// Update a setting value for our application. If the setting does not
        /// exist, then add the setting.
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool AddOrUpdateValue(string Key, Object value)
        {
            bool valueChanged = false;

            // If the key exists
            if (settings.Contains(Key))
            {
                // If the value has changed
                if (settings[Key] != value)
                {
                    // Store the new value
                    settings[Key] = value;
                    valueChanged = true;
                }
            }
            // Otherwise create the key.
            else
            {
                settings.Add(Key, value);
                valueChanged = true;
            }
            return valueChanged;
        }

        /// <summary>
        /// Get the current value of the setting, or if it is not found, set the 
        /// setting to the default setting.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T GetValueOrDefault<T>(string Key, T defaultValue)
        {
            T value;

            // If the key exists, retrieve the value.
            if (settings.Contains(Key))
            {
                value = (T)settings[Key];
            }
            // Otherwise, use the default value.
            else
            {
                value = defaultValue;
            }
            return value;
        }

        /// <summary>
        /// Save the settings.
        /// </summary>
        public void Save()
        {
            settings.Save();
        }

        /// <summary>
        /// Property to get and set if program is started for the first time. 
        /// </summary>
        public bool FirstStart
        {
            get
            {
                return GetValueOrDefault<bool>(FirstStartKeyName, FirstStartDefault);
            }
            set
            {
                AddOrUpdateValue(FirstStartKeyName, value);
            }
        }

        /// <summary>
        /// Property to get and set if the schedule should only be downloaded through WiFi Connection
        /// </summary>
        public bool JustWifiDownload
        {
            get
            {
                return GetValueOrDefault<bool>(JustWifiDownloadKeyName, JustWifiDownloadDefault);
            }
            set
            {
                AddOrUpdateValue(JustWifiDownloadKeyName, value);
            }
        }

        /// <summary>
        /// Property to get and set the time the schedule was last downloaded
        /// </summary>
        public DateTime LastTimeDownloaded
        {
            get
            {
                return GetValueOrDefault<DateTime>(LastTimeDownloadedKeyName, LastTimeDownloadedDefault);
            }
            set
            {
                AddOrUpdateValue(LastTimeDownloadedKeyName, value);
            }
        }

        /// <summary>
        /// Property to get and set the download interval for the schedule
        /// </summary>
        public KeyValuePair<string, TimeSpan> ScheduleDownloadInterval
        {
            get
            {
                return GetValueOrDefault<KeyValuePair<string, TimeSpan>>(ScheduleDownloadIntervalKeyName, ScheduleDownloadIntervalDefault);
            }
            set
            {
                AddOrUpdateValue(ScheduleDownloadIntervalKeyName, value);
            }
        }

        /// <summary>
        /// Property which holds the currently downloaded schedule version
        /// </summary>
        public string ScheduleVersionDownloaded
        {
            get
            {
                return GetValueOrDefault<string>(ScheduleVersionDownloadedKeyName, ScheduleVersionDownloadedDefault);
            }
            set
            {
                AddOrUpdateValue(ScheduleVersionDownloadedKeyName, value);
            }

        }
    }
}
