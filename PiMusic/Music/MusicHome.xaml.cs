using NAudio.Wave;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using static PiMusic.MusicHandler;

namespace PiMusic.Music
{
    /// <summary>
    /// Interaction logic for MusicHome.xaml
    /// </summary>
    public partial class MusicHome : Page
    {
        public static MusicHome Instance;

        public Stopwatch stopwatch;
        public DispatcherTimer timer;

        public MusicHome()
        {
            InitializeComponent();
            Instance = this;
            StatusCheck();
            MusicLibPrep();

            CoverBlurBG.Content = new MusicBackground();
        }

        public async void StatusCheck()
        {
            if(MusicHandler.IsPlaying == true)
            {
                MusicTitle.Text = MusicHandler.MusicMetadata.Title;
                MusicArtist.Text = MusicHandler.MusicMetadata.Artist;

                MusicTitle.Margin = new Thickness(125, 0, 0, 58);
                await Task.Delay(500);
                MusicArtist.Opacity = 1;

                CoverArtStraightSource.ImageSource = MusicHandler.MusicMetadata.Cover;
            }
        }

        public async Task MusicLibPrep()
        {
            string exeDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string configDirectory = System.IO.Path.Combine(exeDirectory, "Config");
            string filePath = System.IO.Path.Combine(configDirectory, "SongIndex.json");
            string json = await System.IO.File.ReadAllTextAsync(filePath);
            List<string> Songs = JsonConvert.DeserializeObject<List<string>>(json);

            ScrollViewer songSelection = this.FindName("SongSelection") as ScrollViewer;

            StackPanel songPanel = new StackPanel();

            int Position = 1;
            int Pos1;

            foreach (string song in Songs)
            {
                TagLib.File tagFile = TagLib.File.Create(song);
                TimeSpan duration = tagFile.Properties.Duration;

                if (Position == 1)
                {
                    Pos1 = 70;
                    Position = 0;
                }
                else
                {
                    Pos1 = 20;
                }

                Grid songGrid = new Grid
                {
                    Height = 90,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(30, Pos1, 30, 0)
                };

                string AccentColor = "#000000";
                Color color = (Color)ColorConverter.ConvertFromString(AccentColor);
            
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

                    BitmapImage albumCover = new BitmapImage();
                    albumCover.BeginInit();
                    albumCover.CacheOption = BitmapCacheOption.OnLoad;
                    albumCover.StreamSource = ms;
                    albumCover.EndInit();
                    albumCover.Freeze();

                    Rectangle cover1 = new Rectangle
                    {
                        Margin = new Thickness(30, 0, 0, 0),
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                        RadiusX = 10,
                        RadiusY = 10,
                        Width = 80,
                        Height = 80,

                        Fill = new ImageBrush
                        {
                            ImageSource = albumCover
                        }
                    };

                    songGrid.Children.Add(cover1);
                }

                StackPanel stackPanel = new StackPanel
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(130, 0, 200, 0)
                };

                stackPanel.Children.Add(new TextBlock { Text = tagFile.Tag.Title, Foreground = new SolidColorBrush(Colors.White), Margin = new Thickness(0, -3, 0, 0), TextWrapping = TextWrapping.Wrap, FontSize = 24, FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Assets/Latin/Default.otf#SF Pro Display")
            });
                stackPanel.Children.Add(new TextBlock { Text = tagFile.Tag.FirstPerformer, Foreground = new SolidColorBrush(Colors.White), FontSize = 18, FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Assets/Latin/Default.otf#SF Pro Display") });

                songGrid.Children.Add(stackPanel);

                songGrid.Children.Add(new TextBlock { Text = duration.ToString(@"mm\:ss"), Foreground = new SolidColorBrush(Colors.White), Margin = new Thickness(0, -40, 30, 0), FontSize = 24, VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = System.Windows.HorizontalAlignment.Right, FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Assets/Latin/Default.otf#SF Pro Display") });

                FileInfo fileInfo = new FileInfo(song);
                long fileSizeInBytes = fileInfo.Length;
                double fileSizeInMB = Math.Round((double)fileSizeInBytes / 1048576, 2);

                songGrid.Children.Add(new TextBlock { Text = fileSizeInMB.ToString() + " MB", Foreground = new SolidColorBrush(Colors.White), Margin = new Thickness(0, 10, 30, 0), FontSize = 24, HorizontalAlignment = System.Windows.HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Center, FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Assets/Latin/Default.otf#SF Pro Display") });

                var button = new System.Windows.Controls.Button { Opacity = 0, Content = song };
                button.Click += Button_Click;
                songGrid.Children.Add(button);

                songPanel.Children.Add(songGrid);
            }
            songSelection.Content = songPanel;
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            QuinticEase c = new QuinticEase();
            c.EasingMode = EasingMode.EaseInOut;

            ThicknessAnimation PosCorrect = new ThicknessAnimation()
            {
                To = new Thickness(125, 0, 0, 58),
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = c
            };

            DoubleAnimation Appear = new DoubleAnimation()
            {
                To = 1,
                Duration = TimeSpan.FromSeconds(0.5)
            };

            System.Windows.Controls.Button button = sender as System.Windows.Controls.Button;
            var content = button.Content;

            MusicHandler.StartMusic(content.ToString());
            TagLib.File tagFile = TagLib.File.Create(content.ToString());

            MusicTitle.Text = tagFile.Tag.Title;
            MusicArtist.Text = tagFile.Tag.FirstPerformer;

            MusicTitle.BeginAnimation(TextBlock.MarginProperty, PosCorrect);
            await Task.Delay(500);
            MusicArtist.BeginAnimation(TextBlock.OpacityProperty, Appear);

            if (tagFile.Tag.Pictures.Length >= 1)
            {
                var bin = (byte[])(tagFile.Tag.Pictures[0].Data.Data);
                MemoryStream ms = new MemoryStream(bin);

                BitmapImage albumCover = new BitmapImage();
                albumCover.BeginInit();
                albumCover.StreamSource = ms;
                albumCover.EndInit();

                // Assuming you're in the same class where CoverArt is defined
                ImageBrush brush = (ImageBrush)CoverArt.Fill;
                brush.ImageSource = albumCover;

                ImageBrush brush2 = (ImageBrush)Home.Instance.CoverArt.Fill;
                brush2.ImageSource = albumCover;
            }

            Home.Instance.SongTitle.Text = tagFile.Tag.Title;
            Home.Instance.SongArtist.Text = tagFile.Tag.FirstPerformer;
            PausePic.Opacity = 1;

            TimeSpan duration = tagFile.Properties.Duration;
            Home.Instance.EndTime.Text = duration.ToString(@"m\:ss");

            Home.Instance.UnhideMusicWidget();
        }


        public async void Closure()
        {
            QuinticEase c = new QuinticEase();
            c.EasingMode = EasingMode.EaseIn;

            ThicknessAnimation FlyOut = new ThicknessAnimation()
            {
                From = new Thickness(0, 0, 0, -25),
                To = new Thickness(0, 0, 0, -100),
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = c
            };

            DoubleAnimation Dissapear = new DoubleAnimation()
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.4)
            };

            DoubleAnimation Appear = new DoubleAnimation()
            {
                To = 1,
                Duration = TimeSpan.FromSeconds(0.4)
            };

            Bar.BeginAnimation(Grid.MarginProperty, FlyOut);
            await Task.Delay(1000);

            Statusbar.Instance.CurrentApp.BeginAnimation(TextBlock.OpacityProperty, Appear);
            Statusbar.Instance.Lossless.BeginAnimation(Image.OpacityProperty, Appear);

            MainContent.BeginAnimation(Grid.OpacityProperty, Dissapear);

            await Task.Delay(500);

            MainWindow.Instance.ConfirmClosure();
        }

        private void PauseClick(object sender, RoutedEventArgs e)
        {
            if(MusicHandler.IsPlaying == false)
            {
                MusicHandler.ResumeMusic();
                PausePic.Source = new BitmapImage(new Uri("pack://application:,,,/PiMusic;component/Assets/Icons/Light/Pause.png"));
            }
            else
            {
                MusicHandler.PauseMusic();
                PausePic.Source = new BitmapImage(new Uri("pack://application:,,,/PiMusic;component/Assets/Icons/Light/Play.png"));
            }

            return;
        }
    }
}
