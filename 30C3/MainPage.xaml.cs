using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using _30C3.scheduleModel;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Windows.Controls.Primitives;
using System.ComponentModel;
using System.Threading;

namespace _30C3
{
    public partial class MainPage : PhoneApplicationPage
    {
        private BackgroundWorker BackgroundWorker;
        private AppSettings AppSettings;
        private Popup Popup;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            this.ShowLoadingPage();
            this.Loaded += MainPage_Loaded;
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.AppSettings = new AppSettings();
            this.LoadData();
            this.Loaded -= MainPage_Loaded;
        }

        public void ShowLoadingPage()
        {
            this.ApplicationBar.IsVisible = false;
            this.Popup = new Popup();
            this.Popup.IsOpen = false;
            this.Popup.Child = new LoadingPage();
            this.Popup.IsOpen = true;
        }

        public void LoadData()
        {
            BackgroundWorker = new BackgroundWorker();
            BackgroundWorker.DoWork += new DoWorkEventHandler(BackgroundWorker_DoWork);
            BackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BackgroundWorker_RunWorkerCompleted);
            BackgroundWorker.RunWorkerAsync();
        }

        void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(() =>
            {
                // Schön ausfliegen lassen?
                this.ApplicationBar.IsVisible = true;
                this.Popup.IsOpen = false;
            }
            );
        }

        void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            ManualResetEvent CompletedNotifier = new ManualResetEvent(false);
            scheduleDownloader scheduleDownloader = new scheduleDownloader();
            IsolatedStorageFileHandler isfh = new IsolatedStorageFileHandler();

            if (scheduleDownloader.DownloadRequired && !AppSettings.FirstStart)
            {
                ManualResetEvent VersionCompletedNotifier = new ManualResetEvent(false);
                string scheduleVersion = AppSettings.ScheduleVersionDownloaded;
                scheduleDownloader.GetScheduleVersion((ex, version) =>
                    {
                        if (ex != null)
                        {
                            this.Dispatcher.BeginInvoke(() =>
                            {
                                MessageBox.Show("Schedule couldn't be retrieved!");
                                VersionCompletedNotifier.Set();
                            });
                        }
                        else
                        {
                            scheduleVersion = version;
                            VersionCompletedNotifier.Set();
                        }
                    });

                VersionCompletedNotifier.WaitOne();

                if (AppSettings.ScheduleVersionDownloaded != scheduleVersion)
                {
                    scheduleDownloader.Download((ex2, _schedule) =>
                    {
                        if (ex2 != null)
                        {
                            this.Dispatcher.BeginInvoke(() =>
                            {
                                MessageBox.Show("Schedule couldn't be retrieved!");
                                CompletedNotifier.Set();
                                return;
                            });
                        }
                        else
                        {

                            try
                            {
                                this.Dispatcher.BeginInvoke(() =>
                                    {
                                        isfh.SaveSchedule(_schedule);
                                        AppSettings.LastTimeDownloaded = DateTime.Now;
                                        AppSettings.ScheduleVersionDownloaded = _schedule.Version;
                                        CompletedNotifier.Set();
                                    });
                            }
                            catch
                            {
                                this.Dispatcher.BeginInvoke(() =>
                                    {
                                        MessageBox.Show("Schedule couldn't be retrieved!");
                                        CompletedNotifier.Set();
                                    });
                            }
                        }
                    });
                    CompletedNotifier.WaitOne();
                }
            }

            this.Dispatcher.BeginInvoke(() =>
            {
                if (AppSettings.FirstStart == true)
                    this.OnFirstStart();

                (App.Current as App).schedule = isfh.GetSchedule();
                setDataContexts();
            });
        }

        public void setDataContexts()
        {
            this.LayoutRoot.DataContext = (App.Current as App).schedule;
            this.Day1_Grid.DataContext = (App.Current as App).schedule.Days[0];
            this.Day2_Grid.DataContext = (App.Current as App).schedule.Days[1];
            this.Day3_Grid.DataContext = (App.Current as App).schedule.Days[2];
            this.Day4_Grid.DataContext = (App.Current as App).schedule.Days[3];
        }

        public void OnFirstStart()
        {
            // Load locally stored schedule
            XmlSerializer serializer = new XmlSerializer(typeof(schedule));
            XDocument document = XDocument.Load("scheduleModel/schedule.en.xml");
            schedule tempschedule = (schedule)serializer.Deserialize(document.CreateReader());
            tempschedule.Clean();
            IsolatedStorageFileHandler isfh = new IsolatedStorageFileHandler();
            isfh.SaveSchedule(tempschedule);
            AppSettings.ScheduleVersionDownloaded = tempschedule.Version;
            AppSettings.FirstStart = false;
        }

        private void Grid_Day_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            int DayIndex = ((day)((Grid)sender).DataContext).Index;
            NavigationService.Navigate(new Uri("/DayPage.xaml?index=" + DayIndex.ToString(), UriKind.Relative));
        }

        private void Grid_Speaker_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/SpeakersOverview.xaml", UriKind.Relative));
        }

        private void Grid_Events_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/EventsOverview.xaml", UriKind.Relative));
        }

        private void ApplicationBarIconButtonSettings_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Settings.xaml", UriKind.Relative));
        }
    }
}