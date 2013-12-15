using System;
using System.Linq;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using _30C3.scheduleModel;

namespace _30C3
{
    public partial class EventsOverview : PhoneApplicationPage
    {
        public EventsOverview()
        {
            InitializeComponent();
            this.Dispatcher.BeginInvoke(() =>
            {
                this.lB_Events.ItemsSource = (App.Current as App).schedule.Events.OrderBy(ev => ev.Title).ToList<@event>();
            });
        }

        private void Grid_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var ID = ((Grid)sender).Tag;
            NavigationService.Navigate(new Uri("/EventPage.xaml?id=" + ID.ToString(), UriKind.Relative));
        }
    }
}