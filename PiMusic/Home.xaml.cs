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
        public Home()
        {
            InitializeComponent();
        }

        private void Musicbox_MouseDown(object sender, MouseButtonEventArgs e) //Right before the transition to
        {
            QuinticEase c = new QuinticEase();
            c.EasingMode = EasingMode.EaseOut;

            DoubleAnimation BoxCompress = new DoubleAnimation()
            {
                To = 340,
                Duration = TimeSpan.FromSeconds(0.3),
                EasingFunction = c
            };

            MusicBlok.BeginAnimation(Grid.HeightProperty, BoxCompress);
            MusicBlok.BeginAnimation(Grid.WidthProperty, BoxCompress);
        }

        private async void Musicbox_Click(object sender, RoutedEventArgs e)
        {
            QuinticEase c = new QuinticEase();
            c.EasingMode = EasingMode.EaseOut;

            DoubleAnimation ExpandH = new DoubleAnimation()
            {
                To = 460,
                Duration = TimeSpan.FromSeconds(0.7),
                EasingFunction = c
            };

            DoubleAnimation ExpandW = new DoubleAnimation()
            {
                To = 786,
                Duration = TimeSpan.FromSeconds(0.7),
                EasingFunction = c
            };

            DoubleAnimation Hide = new DoubleAnimation()
            {
                To = 0,
                Duration = TimeSpan.FromSeconds(0.2),
            };

            DoubleAnimation Appear = new DoubleAnimation()
            {
                To = 1,
                Duration = TimeSpan.FromSeconds(0.2),
            };

            ThicknessAnimation Reposition = new ThicknessAnimation()
            {
                To = new Thickness(0, 0, 0, 0),
                Duration = TimeSpan.FromSeconds(0.5),
                EasingFunction = c
            };

            ColorAnimation AppAniColor = new ColorAnimation()
            {
                From = (Color)ColorConverter.ConvertFromString("#8B0000"),
                To = (Color)ColorConverter.ConvertFromString("#FA1F39"),
                Duration = TimeSpan.FromSeconds(0.5)
            };

            MusicBlok.BeginAnimation(Grid.HeightProperty, ExpandH);
            MusicBlok.BeginAnimation(Grid.WidthProperty, ExpandW);
            MusicBlok.BeginAnimation(Grid.MarginProperty, Reposition);

            PlayerControls.BeginAnimation(Grid.OpacityProperty, Hide);
            AirPlay.BeginAnimation(Image.OpacityProperty, Hide);
            SongTitle.BeginAnimation(TextBlock.OpacityProperty, Hide);
            SongArtist.BeginAnimation(TextBlock.OpacityProperty, Hide);
            CoverArt.BeginAnimation(Rectangle.OpacityProperty, Hide) ;
            WeerBlok.BeginAnimation(Grid.OpacityProperty, Hide);

            //Deel 2 vn de animatie
            MusicBlokBG.BeginAnimation(Border.OpacityProperty, Appear);
            SolidColorBrush brush = new SolidColorBrush();
            MusicBlokBG.Background = brush;
            brush.BeginAnimation(SolidColorBrush.ColorProperty, AppAniColor);

            await Task.Delay(200);
            Statusbar.Instance.SwitchColor(1);
        }
    }
}
