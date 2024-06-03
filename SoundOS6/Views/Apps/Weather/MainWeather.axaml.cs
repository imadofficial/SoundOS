using Avalonia.Animation;
using Avalonia;
using Avalonia.Controls;
using System.Threading.Tasks;
using System;

namespace SoundOS6.Views.Apps.Weather
{
    public partial class MainWeather : UserControl
    {
        public MainWeather()
        {
            InitializeComponent();
            Init();
        }

        public async void Init()
        {
            await MainView.Instance.GetWeather();

            var WeatherData = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(MainView.GlobalStrings.WeatherResponse);

            Stad.Text = WeatherData["location"]["name"];
            Graad.Text = WeatherData["current"]["temp_c"] + "°";
            Conditie.Text = WeatherData["current"]["condition"]["text"];

            long unixTimestamp = WeatherData["current"]["last_updated_epoch"];
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp);
            DateTime dateTime = dateTimeOffset.DateTime;

            DateTime LocalDateTime = dateTimeOffset.LocalDateTime;
            LaatsteUpdate.Text = LocalDateTime.ToString("HH:mm");

        }

        private async void BackTap(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var transition = new DoubleTransition
            {
                Duration = TimeSpan.FromSeconds(0.3),
                Easing = new Avalonia.Animation.Easings.QuadraticEaseInOut(),
                Property = Visual.OpacityProperty,
            };

            MainView.Instance.PageViewer.Transitions = new Transitions() { transition };

            MainView.Instance.PageViewer.Opacity = 1;
            MainView.Instance.PageViewer.ApplyTemplate();
            await Task.Delay(300);
            MainView.Instance.PageViewer.Opacity = 0;
            await Task.Delay(300);
            MainView.Instance.PageViewer.Opacity = 1;
            MainView.Instance.PageViewer.Content = null;
            Statusbar.Instance.AppearStatus();
        }
    }
}
