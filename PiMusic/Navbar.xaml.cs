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
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PiMusic
{
    public partial class Navbar : Page
    {
        public bool ExitAvailable = false;

        public static Navbar Instance;
        private DispatcherTimer timer;

        public String ActiveApp;

        public Navbar()
        {
            InitializeComponent();
            Instance = this;
        }

        public void BarExpand()
        {
            QuinticEase c = new QuinticEase();
            c.EasingMode = EasingMode.EaseOut;

            DoubleAnimation Width = new DoubleAnimation()
            {
                To = 100,
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = c
            };

            DoubleAnimation Height = new DoubleAnimation()
            {
                To = 50,
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = c
            };

            BarBG.BeginAnimation(Grid.HeightProperty, Height);
            BarBG.BeginAnimation(Grid.WidthProperty, Width);
            BorderBG.CornerRadius = new CornerRadius(12);
        }

        public void BarContract()
        {
            QuinticEase c = new QuinticEase();
            c.EasingMode = EasingMode.EaseOut;

            DoubleAnimation Width = new DoubleAnimation()
            {
                To = 100,
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = c
            };

            DoubleAnimation Height = new DoubleAnimation()
            {
                To = 4,
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = c
            };

            BarBG.BeginAnimation(Grid.HeightProperty, Height);
            BarBG.BeginAnimation(Grid.WidthProperty, Width);
            BorderBG.CornerRadius = new CornerRadius(2);
        }

        public void ColorSwitch(int ColorMode)
        {
            string ColorCode;
            if (ColorMode == 0)
            {
                ColorCode = "#000000";
            }
            else
            {
                ColorCode = "#FFFFFF";
            }

            ColorAnimation ColorTransition = new ColorAnimation()
            {
                To = (Color)ColorConverter.ConvertFromString(ColorCode),
                Duration = TimeSpan.FromSeconds(0.2)
            };

            SolidColorBrush Object = new SolidColorBrush();

            BorderBG.Background = Object;
            Object.BeginAnimation(SolidColorBrush.ColorProperty, ColorTransition);
            
        }

        private void HomePress_Click(object sender, RoutedEventArgs e)
        {
            BarExpand();
            if(ExitAvailable == true)
            {
                BarContract();
                Closure();
            }

            if(ExitAvailable == false)
            {
                ExitAvailable = true;
            }
            

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        public void Closure()
        {
            ExitAvailable = false;
            var page = MainWindow.Instance.MainContent.Content as Page;

            if (page is MusicHome MyPage)
            {
                MyPage.Closure();
            }
            else
            {

            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            BarContract();
            timer.Stop(); // stop the timer after contracting the bar
        }

    }
}
