using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.Media;
using Avalonia.Platform;
using System;
using SoundOS6.Views.Music;
using Avalonia.Threading;
using AvaloniaProgressRing;
using System.Net.Http;
using System.Threading.Tasks;
using SoundOS6.Views.Apps.Weather;

namespace SoundOS6.Views
{
    public partial class MainView : UserControl
    {
        public static MainView Instance;

        public static class GlobalStrings
        {
            public static String WeatherResponse;
        }

        public MainView()
        {
            InitializeComponent();
            Instance = this;
            StatusShow.Content = new Statusbar();
            Init();
        }

        public async void Init()
        {
            await Task.Delay(500);
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
                WeerStatus.Opacity = 0;
            }
            catch (HttpRequestException e)
            {
                WeerStatus.Text = "Kon niet met de server verbinden.";
                RetryBTN.Opacity = 1;
                RetryBTN.IsEnabled = true;
            }
            finally
            {
                client.Dispose();
            }

        }

        private void Musicbox_Click(object sender, RoutedEventArgs e)
        {
            PageViewer.Content = new MusicSplash();
            Statusbar.Instance.RemoveStatus();
        }

        private void Weather_Click(object sender, RoutedEventArgs e)
        {
            PageViewer.Content = new MainWeather();
            Statusbar.Instance.RemoveStatus();
        }

        public void UnhideMusicWidget()
        {
            Instructions.Opacity = 0;
            SongTitle.Opacity = 1;
            SongArtist.Opacity = 1;
            CoverArt.Opacity = 1;
            PlayerControls.Opacity = 1;
        }

        public async void SetMusicWidget(String Title, String Artist, bool Live, Bitmap Cover, TimeSpan? SongSpan = null)
        {
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                var imageBrush = new ImageBrush(Cover);
                if (Live == true)
                {
                    CoverArt.Background = imageBrush;

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
                    NextSong.Opacity = 1;
                    PreviousSong.Opacity = 1;
                }

                Instructions.Opacity = 0;
                SongTitle.Opacity = 1;
                SongArtist.Opacity = 1;
                CoverArt.Opacity = 1;
                Play.Opacity = 1;
                PlayingStat.Opacity = 1;

                SongTitle.Text = Title;
                SongArtist.Text = Artist;

                CoverArt.Background = imageBrush;
            });
        }


        private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (MusicHandler.IsPlaying == false)
            {
                MusicHandler.ResumeMusic();

                using (var stream = AssetLoader.Open(new Uri("avares://SoundOS6/Assets/Icons/Light/Pause.png")))
                {
                    Play.Source = new Avalonia.Media.Imaging.Bitmap(stream);
                }

                PlayingStat.Text = "Pauzeren";

                return;
            }
            else
            {
                MusicHandler.PauseMusic();
                using (var stream = AssetLoader.Open(new Uri("avares://SoundOS6/Assets/Icons/Light/Play.png")))
                {
                    Play.Source = new Avalonia.Media.Imaging.Bitmap(stream);
                }

                PlayingStat.Text = "Afspelen";

                return;
            }
        }

        private async void BackSong(object? sender, Avalonia.Input.TappedEventArgs e)
        {
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                MusicHandler._stream.Stop();
                int index = MusicSplash.Instance.filePaths.IndexOf(MusicHandler.CurrentSongPath);

                try
                {
                    MusicHandler.StartMusic(MusicSplash.Instance.filePaths[index - 1], false);
                    index = index - 1;

                    TagLib.File tagFile = TagLib.File.Create(MusicSplash.Instance.filePaths[index + 1]);
                }
                catch (ArgumentOutOfRangeException)
                {
                    int KeyAmount = MusicSplash.Instance.filePaths.Count;
                    MusicHandler.StartMusic(MusicSplash.Instance.filePaths[KeyAmount - 1], false);
                    index = KeyAmount - 1;

                    TagLib.File tagFile = TagLib.File.Create(MusicSplash.Instance.filePaths[0]);
                }

                MusicHome.Instance.PausePic.Opacity = 1;
                MainView.Instance.PlayCommandBtn.IsEnabled = true;



                MainView.Instance.UnhideMusicWidget();

                using (var stream = AssetLoader.Open(new Uri("avares://SoundOS6/Assets/Icons/Light/Pause.png")))
                {
                    MusicHome.Instance.PausePic.Source = new Avalonia.Media.Imaging.Bitmap(stream);
                }

                MusicHandler._TimeElapsed = new DispatcherTimer();
                MusicHandler._TimeElapsed.Interval = TimeSpan.FromMilliseconds(500);
                MusicHandler._TimeElapsed.Tick += MusicLocal.ElapsedProcess;
                MusicHandler._TimeElapsed.Start();
            });
        }

        private async void ForwardSong(object? sender, Avalonia.Input.TappedEventArgs e)
        {
            MusicHandler._stream.Stop();
            int index = MusicSplash.Instance.filePaths.IndexOf(MusicHandler.CurrentSongPath);
            if (index != -1 && index + 1 < MusicSplash.Instance.filePaths.Count)
            {
                MusicHandler.StartMusic(MusicSplash.Instance.filePaths[index + 1], false);

                TagLib.File tagFile = TagLib.File.Create(MusicSplash.Instance.filePaths[index + 1]);

                MusicHome.Instance.PausePic.Opacity = 1;
                MainView.Instance.PlayCommandBtn.IsEnabled = true;

                MainView.Instance.UnhideMusicWidget();

                using (var stream = AssetLoader.Open(new Uri("avares://SoundOS6/Assets/Icons/Light/Pause.png")))
                {
                    MusicHome.Instance.PausePic.Source = new Avalonia.Media.Imaging.Bitmap(stream);
                }
            }
            else
            {
                MusicHandler.StartMusic(MusicSplash.Instance.filePaths[0], false);

                TagLib.File tagFile = TagLib.File.Create(MusicSplash.Instance.filePaths[0]);

                MusicHome.Instance.PausePic.Opacity = 1;
                MainView.Instance.PlayCommandBtn.IsEnabled = true;

                MainView.Instance.UnhideMusicWidget();

                using (var stream = AssetLoader.Open(new Uri("avares://SoundOS6/Assets/Icons/Light/Pause.png")))
                {
                    MusicHome.Instance.PausePic.Source = new Avalonia.Media.Imaging.Bitmap(stream);
                }
            }

            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                MusicHandler._TimeElapsed = new DispatcherTimer();
                MusicHandler._TimeElapsed.Interval = TimeSpan.FromMilliseconds(500);
                MusicHandler._TimeElapsed.Tick += MusicLocal.ElapsedProcess;
                MusicHandler._TimeElapsed.Start();
            });
        }

        private void Button_Tapped_1(object? sender, Avalonia.Input.TappedEventArgs e)
        {
            Init();
            RetryBTN.Opacity = 0;
            RetryBTN.IsEnabled = false;
            WeerStatus.Text = "Laden...";
        }
    }
}