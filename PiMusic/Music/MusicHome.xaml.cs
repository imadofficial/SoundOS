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
            Animations();
            StatusCheck();

            CoverBlurBG.Content = new MusicBackground();
            MusicContent.Content = new MusicLocal();
        }

        private async void Animations()
        {
            QuinticEase c = new QuinticEase();
            c.EasingMode = EasingMode.EaseIn;

            DoubleAnimation Appear = new DoubleAnimation()
            {
                To = 1,
                Duration = TimeSpan.FromSeconds(0.4)
            };

            LocalFiles.BeginAnimation(TextBlock.OpacityProperty, Appear);
            await Task.Delay(100);
            OnlineRadio.BeginAnimation(TextBlock.OpacityProperty, Appear);
            BETA.BeginAnimation(TextBlock.OpacityProperty, Appear);

            DoubleAnimation IndicatorExpand = new DoubleAnimation()
            {
                From = 0,
                To = 25,
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = c
            };

            AppletIndicator.BeginAnimation(Border.WidthProperty, IndicatorExpand);
        }

        public async void BarAni()
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

            MusicHome.Instance.MusicTitle.BeginAnimation(TextBlock.MarginProperty, PosCorrect);
            await Task.Delay(500);
            MusicHome.Instance.MusicArtist.BeginAnimation(TextBlock.OpacityProperty, Appear);
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
                PausePic.Opacity = 1;
            }
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

            DoubleAnimation UnExpand = new DoubleAnimation()
            {
                To = 100,
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = c
            };

            Bar.BeginAnimation(Grid.MarginProperty, FlyOut);
            Bar.BeginAnimation(Border.HeightProperty, UnExpand);
            await Task.Delay(1000);

            Statusbar.Instance.CurrentApp.BeginAnimation(TextBlock.OpacityProperty, Appear);
            Statusbar.Instance.Icon.BeginAnimation(Image.OpacityProperty, Appear);

            MainContent.BeginAnimation(Grid.OpacityProperty, Dissapear);
            BarBG.BeginAnimation(Border.OpacityProperty, Dissapear);

            await Task.Delay(1000);

            MainWindow.Instance.ConfirmClosure();
        }

        private void PauseClick(object sender, RoutedEventArgs e)
        {
            if(MusicHandler.IsPlaying == false)
            {
                MusicHandler.ResumeMusic();
                PausePic.Source = new BitmapImage(new Uri("pack://application:,,,/PiMusic;component/Assets/Icons/Light/Pause.png"));
                return;
            }
            else
            {
                MusicHandler.PauseMusic();
                PausePic.Source = new BitmapImage(new Uri("pack://application:,,,/PiMusic;component/Assets/Icons/Light/Play.png"));
                return;
            }
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            QuinticEase c = new QuinticEase();
            c.EasingMode = EasingMode.EaseInOut;

            ThicknessAnimation MoveFly = new ThicknessAnimation()
            {
                To = new Thickness(170, 52, 0, 0),
                Duration = TimeSpan.FromSeconds(0.7),
                EasingFunction = c
            };

            AppletIndicator.BeginAnimation(Border.MarginProperty, MoveFly);

            MusicContent.JournalOwnership = JournalOwnership.OwnsJournal;
            MusicContent.Navigate(new MusicRadio());
            MusicContent.RemoveBackEntry();
        }

        private void LocalButton_Click(object sender, RoutedEventArgs e)
        {
            QuinticEase c = new QuinticEase();
            c.EasingMode = EasingMode.EaseInOut;

            ThicknessAnimation MoveFly = new ThicknessAnimation()
            {
                To = new Thickness(65, 52, 0, 0),
                Duration = TimeSpan.FromSeconds(0.7),
                EasingFunction = c
            };

            AppletIndicator.BeginAnimation(Border.MarginProperty, MoveFly);

            MusicContent.JournalOwnership = JournalOwnership.OwnsJournal;
            MusicContent.Navigate(new MusicLocal());
            MusicContent.RemoveBackEntry();
        }

        private async void BarExpand_Click(object sender, RoutedEventArgs e)
        {
            double pageHeight = this.ActualHeight;

            QuinticEase c = new QuinticEase();
            c.EasingMode = EasingMode.EaseOut;

            QuinticEase b = new QuinticEase();
            b.EasingMode = EasingMode.EaseIn;

            DoubleAnimation Expand = new DoubleAnimation()
            {
                To = pageHeight + 55,
                Duration = TimeSpan.FromSeconds(0.6),
                EasingFunction = c
            };

            ThicknessAnimation CoveraniPart1 = new ThicknessAnimation()
            {
                To = new Thickness(-60, 0, 0, 32),
                Duration = TimeSpan.FromSeconds(0.6),
                EasingFunction = b
            };

            ThicknessAnimation CoveraniPart2 = new ThicknessAnimation()
            {
                To = new Thickness(0, 0, 400, 32),
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = c
            };

            Bar.BeginAnimation(Border.HeightProperty, Expand);
            CoverArt.BeginAnimation(Border.MarginProperty, CoveraniPart1);
            await Task.Delay(600);
            CoverArt.VerticalAlignment = VerticalAlignment.Center;
            CoverArt.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            CoverArt.Margin = new Thickness(-500, 32, 0, 0);
            CoverArt.Height = 300;
            CoverArt.Width = 300;

            CoverArt.BeginAnimation(Border.MarginProperty, CoveraniPart2);
        }
    }
}