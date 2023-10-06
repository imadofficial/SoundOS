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

namespace PiMusic
{
    /// <summary>
    /// Interaction logic for Navbar.xaml
    /// </summary>
    public partial class Navbar : Page
    {

        public static Navbar Instance;
        public Navbar()
        {
            InitializeComponent();
            Instance = this;
        }

        public void BarExpand()
        {
            QuinticEase c = new QuinticEase();
            c.EasingMode = EasingMode.EaseInOut;

            DoubleAnimation Width = new DoubleAnimation()
            {
                To = 400,
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = c
            };

            DoubleAnimation Height = new DoubleAnimation()
            {
                To = 30,
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = c
            };

            BarBG.BeginAnimation(Grid.HeightProperty, Height);
            BarBG.BeginAnimation(Grid.WidthProperty, Width);
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
    }
}
