using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using _30C3.scheduleModel;

namespace _30C3
{
    public partial class DayPage : PhoneApplicationPage
    {
        int Index = 0;
        day ThisDay;
        
        public DayPage()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(DayPage_Loaded);
        }

        void DayPage_Loaded(object sender, RoutedEventArgs e)
        {
            ThisDay = (App.Current as App).schedule.Days[this.Index-1];
            this.DataContext = ThisDay;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string IndexString = "";
            NavigationContext.QueryString.TryGetValue("index", out IndexString);
            this.Index = Convert.ToInt32(IndexString);
        }

        private void Grid_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var ID = ((Grid)sender).Tag;
            NavigationService.Navigate(new Uri("/EventPage.xaml?id=" + ID.ToString(), UriKind.Relative));
        }
    }
}