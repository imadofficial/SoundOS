using PiMusic.Apps.Settings;
using PiMusic.Apps.Weather;
using PiMusic.Music;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PiMusic
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        public static Home Instance;

        public static class GlobalStrings
        {
            public static String WeatherResponse;
        }


        public Home()
        {
            InitializeComponent();
            Instance = this;
            Init();
        }

        public async void Init()
        {
            await GetWeather();
        }

        public async Task GetWeather()
        {
            HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage response = await client.GetAsync("https://api.weatherapi.com/v1/current.json?key=b48046722eb448daafa173827211511&q=51.00365190200678,%204.308994029394593&aqi=no&lang=nl");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                GlobalStrings.WeatherResponse = responseBody;

                var WeatherData = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(GlobalStrings.WeatherResponse);

                Stad.Text = WeatherData["location"]["name"];
                Graad.Text = WeatherData["current"]["temp_c"] + "°";
                Conditie.Text = WeatherData["current"]["condition"]["text"];
                LaatstGeupdate.Text = WeatherData["current"]["name"];

                Stad.Opacity = 1;
                Graad.Opacity = 1;
                Conditie.Opacity = 1;
                LaatstGeupdate.Opacity = 1;

                ProgressRing.IsActive = false;
            }
            catch (HttpRequestException e)
            {
                
            }
            finally
            {
                client.Dispose();
            }

        }

        public void SetMusicWidget(String Title, String Artist, bool Live, BitmapImage Cover, TimeSpan? SongSpan = null)
        {
            if (Live == true)
            {
                CoverSource.ImageSource = Cover;
                CurrentTime.Opacity = 0;
                EndTime.Opacity = 0;
                BackgroundBar.Opacity = 0;
                LiveLbl.Opacity = 1;
            }
            else
            {
                CurrentTime.Opacity = 1;
                EndTime.Opacity = 1;
                BackgroundBar.Opacity = 1;
                LiveLbl.Opacity = 0;

                EndTime.Text = SongSpan.ToString();
            }

            Instructions.Opacity = 0;
            SongTitle.Opacity = 1;
            SongArtist.Opacity = 1;
            CoverArt.Opacity = 1;
            Play.Opacity = 1;
            PlayingStat.Opacity = 1;

            SongTitle.Text = Title;
            SongArtist.Text = Artist;
            CoverSource.ImageSource = Cover;
        }

        private void Musicbox_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        public void UnhideMusicWidget()
        {
            Instructions.Opacity = 0;
            SongTitle.Opacity = 1;
            SongArtist.Opacity = 1;
            CoverArt.Opacity = 1;
            PlayerControls.Opacity = 1;
        }

        private async void Musicbox_Click(object sender, RoutedEventArgs e)
        {
            //Musicbox.IsEnabled = false;
            Statusbar.Instance.StatusText("Muziek");

            QuinticEase c = new QuinticEase();
            c.EasingMode = EasingMode.EaseOut;

            DoubleAnimation Contract = new DoubleAnimation()
            {
                To = 340,
                Duration = TimeSpan.FromSeconds(0.3),
                EasingFunction = c
            };

            DoubleAnimation Return = new DoubleAnimation()
            {
                To = 350,
                Duration = TimeSpan.FromSeconds(0.3),
                EasingFunction = c
            };

            ColorAnimation AppAniColor = new ColorAnimation()
            {
                From = (Color)ColorConverter.ConvertFromString("#394342"),
                To = (Color)ColorConverter.ConvertFromString("#FA1F39"),
                Duration = TimeSpan.FromSeconds(0.5)
            };

            MusicBlok.BeginAnimation(Grid.WidthProperty, Contract);
            MusicBlok.BeginAnimation(Grid.HeightProperty, Contract);

            await Task.Delay(100);

            MusicBlok.BeginAnimation(Grid.WidthProperty, Return);
            MusicBlok.BeginAnimation(Grid.HeightProperty, Return);

            MainWindow.Instance.MainContent.Content = new MusicSplash(); //Muziek App Openen

            Statusbar.Instance.RemoveStatus();
            Statusbar.Instance.SwitchColor(1);
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void EnterSettings_Click(object sender, RoutedEventArgs e)
        {
            Statusbar.Instance.RemoveStatus();

            QuinticEase c = new QuinticEase();
            c.EasingMode = EasingMode.EaseOut;

            DoubleAnimation ExpandBoxW = new DoubleAnimation()
            {
                To = HomeScreen.ActualWidth,
                Duration = TimeSpan.FromSeconds(0.7),
                EasingFunction = c
            };

            DoubleAnimation ExpandBoxH = new DoubleAnimation()
            {
                To = HomeScreen.ActualHeight,
                Duration = TimeSpan.FromSeconds(0.7),
                EasingFunction = c
            };

            DoubleAnimation DissapearLogo = new DoubleAnimation()
            {
                To = 0,
                Duration = TimeSpan.FromSeconds(0.3),
                EasingFunction = c
            };

            ThicknessAnimation CorrectPos = new ThicknessAnimation()
            {
                To = new Thickness (0),
                Duration = TimeSpan.FromSeconds(0.7),
                EasingFunction = c
            };

            Instellingen.BeginAnimation(Grid.HeightProperty, ExpandBoxH);
            Instellingen.BeginAnimation(Grid.WidthProperty, ExpandBoxW);
            Instellingen.BeginAnimation(Grid.MarginProperty, CorrectPos);
            SettingsIcon.BeginAnimation(Image.OpacityProperty, DissapearLogo);

            //Statusbar.Instance.RemoveStatus();
            Statusbar.Instance.SwitchColor(1);
            Statusbar.Instance.StatusText("Instellingen");

            await Task.Delay(1000);

            MainWindow.Instance.MainContent.Content = new SettingsHome();
        }

        private void WeatherClick(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.MainContent.Content = new MainWeather();

        }
    }
}
