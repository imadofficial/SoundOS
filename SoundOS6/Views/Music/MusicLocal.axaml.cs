using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Threading;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;
using LibVLCSharp.Shared;

namespace SoundOS6.Views.Music
{
    public partial class MusicLocal : UserControl
    {
        public static MusicLocal Instance;
        private static DispatcherTimer _TimeElapsed;

        public MusicLocal(List<string> AllSongs)
        {
            InitializeComponent();
            Init(AllSongs);
            Instance = this;
        }

        async void Init(List<string> AllSongs)
        {
            await Task.Delay(500);
            await MusicLibPrep(AllSongs);
        }

        public async Task MusicLibPrep(List<string> AllSongs)
        {
            //List<string> Songs = JsonConvert.DeserializeObject<List<string>>(AllSongs);
            List<string> Songs = AllSongs;

            int SongAmount = Songs.Count;

            if (SongAmount == 0)
            {
                NoResults.Opacity = 1;
                NoResultsExplanation.Opacity = 1;
            }

            ScrollViewer songSelection = this.FindControl<ScrollViewer>("SongSelection");

            StackPanel songPanel = new StackPanel();

            int Position = 1;
            int Pos1;

            foreach (string song in Songs)
            {
                TagLib.File tagFile = TagLib.File.Create(song);
                TimeSpan duration = tagFile.Properties.Duration;

                if (Position == 1)
                {
                    Pos1 = 0;
                    Position = 0;
                }
                else
                {
                    Pos1 = 10;
                }

                Grid songGrid = new Grid
                {
                    Height = 90,
                    VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top,
                    Margin = new Thickness(30, Pos1, 30, 0)
                };

                string AccentColor = "#000000";
                Avalonia.Media.Color color = Avalonia.Media.Color.Parse(AccentColor);

                Border border = new Border
                {
                    Background = new SolidColorBrush(color),
                    CornerRadius = new CornerRadius(20),
                    Opacity = 0.7
                };

                songGrid.Children.Add(border);

                if (tagFile.Tag.Pictures.Length >= 1)
                {
                    var bin = (byte[])(tagFile.Tag.Pictures[0].Data.Data);
                    MemoryStream ms = new MemoryStream(bin);

                    var albumCover = new Avalonia.Media.Imaging.Bitmap(ms);

                    Image image = new Image
                    {
                        Source = albumCover
                    };

                    Border cover1 = new Border
                    {
                        Margin = new Thickness(30, 0, 0, 0),
                        HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                        CornerRadius = new CornerRadius(30),
                        Width = 80,
                        Height = 80,
                        Child = image
                    };

                    songGrid.Children.Add(cover1);
                }

                StackPanel stackPanel = new StackPanel
                {
                    VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                    Margin = new Thickness(130, 0, 200, 0)
                };

                stackPanel.Children.Add(new TextBlock
                {
                    Text = tagFile.Tag.Title,
                    Foreground = new SolidColorBrush(Colors.White),
                    Margin = new Thickness(0, -3, 0, 0),
                    TextWrapping = TextWrapping.Wrap,
                    FontSize = 24,
                    FontFamily = new FontFamily("avares://SoundOS6/Assets/Latin/Default.otf#SF Pro Display")
                });
                stackPanel.Children.Add(new TextBlock
                {
                    Text = tagFile.Tag.FirstPerformer,
                    Foreground = new SolidColorBrush(Colors.White),
                    FontSize = 18,
                    FontFamily = new FontFamily("avares://SoundOS6/Assets/Latin/Default.otf#SF Pro Display")
                });

                songGrid.Children.Add(stackPanel);

                songGrid.Children.Add(new TextBlock
                {
                    Text = duration.ToString(@"mm\:ss"),
                    Foreground = new SolidColorBrush(Colors.White),
                    Margin = new Thickness(0, -40, 30, 0),
                    FontSize = 24,
                    VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right,
                    FontFamily = new FontFamily("avares://SoundOS6/Assets/Latin/Default.otf#SF Pro Display")
                });

                FileInfo fileInfo = new FileInfo(song);
                long fileSizeInBytes = fileInfo.Length;
                double fileSizeInMB = Math.Round((double)fileSizeInBytes / 1048576, 2);

                songGrid.Children.Add(new TextBlock
                {
                    Text = fileSizeInMB.ToString() + " MB",
                    Foreground = new SolidColorBrush(Colors.White),
                    Margin = new Thickness(0, 10, 30, 0),
                    FontSize = 24,
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right,
                    VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                    FontFamily = new FontFamily("avares://SoundOS6/Assets/Latin/Default.otf#SF Pro Display")
                });

                var button = new Button { Opacity = 0, Content = song, Width = 1920, Height = 90 };
                button.Click += Button_Click;
                songGrid.Children.Add(button);

                songPanel.Children.Add(songGrid);
            }
            songSelection.Content = songPanel;

            LoadAni.IsActive = false;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            var content = button.Content;

            MusicHandler.StartMusic(content.ToString(), true);
            TagLib.File tagFile = TagLib.File.Create(content.ToString());

            MusicHome.Instance.PausePic.Opacity = 1;
            MainView.Instance.PlayCommandBtn.IsEnabled = true;

            MainView.Instance.UnhideMusicWidget();

            using (var stream = AssetLoader.Open(new Uri("avares://SoundOS6/Assets/Icons/Light/Pause.png")))
            {
                MusicHome.Instance.PausePic.Source = new Avalonia.Media.Imaging.Bitmap(stream);
            }

            _TimeElapsed = new DispatcherTimer();
            _TimeElapsed.Interval = TimeSpan.FromMilliseconds(500);
            _TimeElapsed.Tick += ElapsedProcess;
            _TimeElapsed.Start();
        }

        public async static void ElapsedProcess(object sender, EventArgs e)
        {
            var positionInMilliseconds = MusicHandler._stream.Time;
            var currentPosition = TimeSpan.FromMilliseconds(0);

            try
            {
                currentPosition = TimeSpan.FromMilliseconds(positionInMilliseconds);
            }
            catch(OverflowException) {
                currentPosition = TimeSpan.FromMilliseconds(0);
            }
            
            string currentPositionFormatted = string.Format("{0}:{1:D2}", currentPosition.Minutes, currentPosition.Seconds);

            MusicHome.Instance.TimerElapsed.Text = currentPositionFormatted;
            MainView.Instance.CurrentTime.Text = currentPositionFormatted;
            double currentPositionInSeconds = currentPosition.TotalSeconds;

            try
            {
                TVMode.Instance.TimerElapsed.Text = currentPositionFormatted;
                TVMode.Instance.Slider.Value = currentPositionInSeconds;
            }
            catch (Exception)
            {
            }
        }
    }
}
