using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using _30C3.scheduleModel;

namespace _30C3
{
    public partial class Settings : PhoneApplicationPage
    {
        AppSettings AppSettings;

        public Settings()
        {
            InitializeComponent();
            AppSettings = new AppSettings();
        }

        private void ToggleSwitch_Wifi_Checked(object sender, RoutedEventArgs e)
        {
            AppSettings.JustWifiDownload = true;
        }

        private void ToggleSwitch_Wifi_Unchecked(object sender, RoutedEventArgs e)
        {
            AppSettings.JustWifiDownload = false;
        }

        private void ListPicker_DownloadInterval_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ListPicker).SelectedItem != null && AppSettings != null)
            {
                if (AppSettings.ScheduleDownloadInterval.GetHashCode() != ((KeyValuePair<string, TimeSpan>)((sender as ListPicker)).SelectedItem).GetHashCode())
                    AppSettings.ScheduleDownloadInterval = ((KeyValuePair<string, TimeSpan>)((sender as ListPicker)).SelectedItem);
            }
        }

        private void btDownloadSpeakerPics_Click(object sender, RoutedEventArgs e)
        {
            foreach (person p in (App.Current as App).schedule.Speaker)
            {
                p.DownloadPicture();
            }
        }
    }
}