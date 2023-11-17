using PiMusic.Music;
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

namespace PiMusic
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        public static Home Instance;
        
        public Home()
        {
            InitializeComponent();
            Instance = this;
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

        private void EnterSettings_Click(object sender, RoutedEventArgs e)
        {
            QuinticEase c = new QuinticEase();
            c.EasingMode = EasingMode.EaseOut;

            DoubleAnimation ExpandBox = new DoubleAnimation()
            {
                To = HomeScreen.ActualWidth,
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = c
            };

            DoubleAnimation DissapearLogo = new DoubleAnimation()
            {
                To = 0,
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = c
            };

            ThicknessAnimation CorrectPos = new ThicknessAnimation()
            {
                To = new Thickness (0),
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = c
            };

            Instellingen.BeginAnimation(Grid.HeightProperty, ExpandBox);
            Instellingen.BeginAnimation(Grid.WidthProperty, ExpandBox);
            Instellingen.BeginAnimation(Grid.MarginProperty, CorrectPos);
            SettingsIcon.BeginAnimation(Image.OpacityProperty, DissapearLogo);
        }
    }
}
