using System;
using System.Linq;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using _30C3.scheduleModel;

namespace _30C3
{
    public partial class SpeakersOverview : PhoneApplicationPage
    {
        public SpeakersOverview()
        {
            InitializeComponent();
            this.Dispatcher.BeginInvoke(() =>
                {
                    this.LB_Speakers.ItemsSource = (App.Current as App).schedule.Speaker.OrderBy(p => p.Name).ToList<person>();
                });
        }

        private void Grid_Speaker_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var ID = ((Grid)sender).Tag;
            NavigationService.Navigate(new Uri("/SpeakerPage.xaml?id=" + ID.ToString(), UriKind.Relative));
        }
    }
}