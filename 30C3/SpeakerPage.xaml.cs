using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using _30C3.scheduleModel;

namespace _30C3
{
    public partial class SpeakerPage : PhoneApplicationPage
    {
        int ID = 0;
        person Speaker;
        List<@event> Events;

        public SpeakerPage()
        {
            InitializeComponent();
            this.Loaded += SpeakerPage_Loaded;
        }

        void SpeakerPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.Speaker = (App.Current as App).schedule.Speaker.Single(_speaker => _speaker.ID == this.ID);
            this.Events = (App.Current as App).schedule.Events.Where(_event => _event.Persons.Contains(this.Speaker)).ToList<@event>();
            this.Dispatcher.BeginInvoke(() =>
            {
                this.LayoutRoot.DataContext = this.Speaker;
                this.lB_EventList.ItemsSource = this.Events;
            });
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string IDString = "";
            NavigationContext.QueryString.TryGetValue("id", out IDString);
            this.ID = Convert.ToInt32(IDString);
        }

        private void Grid_Event_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var ID = ((Grid)sender).Tag;
            NavigationService.Navigate(new Uri("/EventPage.xaml?id=" + ID.ToString(), UriKind.Relative));
        }
    }
}