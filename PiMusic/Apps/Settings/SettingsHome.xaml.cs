using System;
using System.Collections.Generic;
using System.Linq;
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

namespace PiMusic.Apps.Settings
{
    /// <summary>
    /// Interaction logic for SettingsHome.xaml
    /// </summary>
    public partial class SettingsHome : Page
    {
        public SettingsHome()
        {
            InitializeComponent();
            Navbar.Instance.ColorSwitch(1);
            Init();
        }

        private void Init()
        {
            if (MusicHandler.IsPlaying == true)
            {
                SongTitle.Text = MusicHandler.MusicMetadata.Title;
                SongArtist.Text = MusicHandler.MusicMetadata.Artist;
                ISNPlusSource.ImageSource = MusicHandler.MusicMetadata.Cover;
            }
            else
            {
                SongTitle.Opacity = 0;
                SongArtist.Opacity = 0;
                ISNPlusSource.Opacity = 0;
                Error.Opacity = 1;
            }
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            HandlePane.Width = SettingsApp.ActualWidth - 350;
        }

        public async void Closure()
        {
            QuinticEase c = new QuinticEase();
            c.EasingMode = EasingMode.EaseOut;

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

            FullSettings.BeginAnimation(Grid.OpacityProperty, Dissapear);

            Statusbar.Instance.CurrentApp.BeginAnimation(TextBlock.OpacityProperty, Appear);
            Statusbar.Instance.Icon.BeginAnimation(Image.OpacityProperty, Appear);

            await Task.Delay(700);

            DoubleAnimation ExpandBoxW = new DoubleAnimation()
            {
                To = 80,
                Duration = TimeSpan.FromSeconds(0.7),
                EasingFunction = c
            };

            DoubleAnimation ExpandBoxH = new DoubleAnimation()
            {
                To = 80,
                Duration = TimeSpan.FromSeconds(0.7),
                EasingFunction = c
            };

            DoubleAnimation AppearLogo = new DoubleAnimation()
            {
                To = 1,
                Duration = TimeSpan.FromSeconds(0.3),
                EasingFunction = c
            };

            ThicknessAnimation CorrectPos = new ThicknessAnimation()
            {
                To = new Thickness(100, 0, 0, -170),
                Duration = TimeSpan.FromSeconds(0.7),
                EasingFunction = c
            };

            Home.Instance.Instellingen.BeginAnimation(Grid.HeightProperty, ExpandBoxH);
            Home.Instance.Instellingen.BeginAnimation(Grid.WidthProperty, ExpandBoxW);
            Home.Instance.Instellingen.BeginAnimation(Grid.MarginProperty, CorrectPos);
            Home.Instance.SettingsIcon.BeginAnimation(Image.OpacityProperty, AppearLogo);

            MainWindow.Instance.ConfirmClosure();
        }

        private void Network_Click(object sender, RoutedEventArgs e)
        {
            ActualPanel.Content = new Network();
            Instructies.Text = "";
        }

        private void DateTime_Click(object sender, RoutedEventArgs e)
        {
            ActualPanel.Content = new TimeDate();
            Instructies.Text = "";
        }

        private void Dev_Click(object sender, RoutedEventArgs e)
        {
            ActualPanel.Content = new TimeDate();
            Instructies.Text = "";
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            ActualPanel.Content = new About();
            Instructies.Text = "";
        }
    }
}
