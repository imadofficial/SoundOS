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
using Windows.Devices.Geolocation;

namespace PiMusic
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {

        
        public Home()
        {
            InitializeComponent();
        }

        private void Musicbox_MouseDown(object sender, MouseButtonEventArgs e)
        {

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
            Statusbar.Instance.StatusText("Muziek");
            Statusbar.Instance.SwitchColor(1);

        }
    }
}
