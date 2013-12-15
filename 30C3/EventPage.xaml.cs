using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using _30C3.scheduleModel;

namespace _30C3
{
    public partial class EventPage : PhoneApplicationPage
    {
        int ID = 0;
        @event Event;

        public EventPage()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(EventPage_Loaded);

        }

        void EventPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.Event = (App.Current as App).schedule.Events.Single(_event => _event.ID == this.ID);
            this.Dispatcher.BeginInvoke(() =>
                {
                    this.ContentPanel.DataContext = this.Event;
                    this.lB_Links.ItemsSource = this.Event.Links;
                });
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string IDString = "";
            NavigationContext.QueryString.TryGetValue("id", out IDString);
            this.ID = Convert.ToInt32(IDString);
        }

        private void Link_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var wbt = new Microsoft.Phone.Tasks.WebBrowserTask();
            wbt.Uri = new Uri(((TextBlock)sender).Tag.ToString(), UriKind.Absolute);
            wbt.Show();
        }
    }
}