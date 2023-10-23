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

namespace PiMusic.Music
{
    /// <summary>
    /// Interaction logic for MusicHome.xaml
    /// </summary>
    public partial class MusicHome : Page
    {
        public MusicHome()
        {
            InitializeComponent();
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
    }
}
