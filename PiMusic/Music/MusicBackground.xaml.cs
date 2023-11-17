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
    /// Interaction logic for MusicBackground.xaml
    /// </summary>
    public partial class MusicBackground : Page
    {
        public static MusicBackground Instance;
        public MusicBackground()
        {
            InitializeComponent();
            Instance = this;
            Init();
        }

        private async void Init()
        {
            await Task.Delay(1000);
            DoubleAnimation Appear = new DoubleAnimation()
            {
                To = 0.7,
                Duration = TimeSpan.FromSeconds(2)
            };

            CoverImage.BeginAnimation(Image.OpacityProperty, Appear);
        }


    }
}
