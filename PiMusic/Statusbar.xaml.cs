using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ManagedNativeWifi;
using System.Net.NetworkInformation;
using System.Threading;
using System.ComponentModel;
using System.Windows.Media.Animation;

namespace PiMusic
{
    /// <summary>
    /// Interaction logic for Statusbar.xaml
    /// </summary>
    public partial class Statusbar : Page
    {
        System.Windows.Threading.DispatcherTimer Timer = new System.Windows.Threading.DispatcherTimer();
        public static Statusbar Instance;

        public Statusbar()
        {
            InitializeComponent();
            Instance = this;

            SignalStrength();

            Timer.Tick += new EventHandler(DefaultTimer);
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Start();
        }

        private void DefaultTimer(object sender, EventArgs e)
        {
            Time.Text = DateTime.Now.ToString("HH:mm");
        }

        public void SwitchColor(int ColorMode) //0 = Zwart, 1 = Wit
        {
            string ColorCode;
            if(ColorMode == 0)
            {
                ColorCode = "#000000";
            }
            else
            {
                ColorCode = "#FFFFFF";
            }

            ColorAnimation TextColorTransition = new ColorAnimation()
            {
                To = (Color)ColorConverter.ConvertFromString(ColorCode),
                Duration = TimeSpan.FromSeconds(0.2)
            };

            SolidColorBrush Object = new SolidColorBrush();
            Time.Foreground = Object;
            Object.BeginAnimation(SolidColorBrush.ColorProperty, TextColorTransition);

            ConnectionType.Foreground = Object;
            Object.BeginAnimation(SolidColorBrush.ColorProperty, TextColorTransition);

            CurrentApp.Foreground = Object;
            Object.BeginAnimation(SolidColorBrush.ColorProperty, TextColorTransition);

            Signal1.Background = Object;
            Object.BeginAnimation(SolidColorBrush.ColorProperty, TextColorTransition);

            Signal2.Background = Object;
            Object.BeginAnimation(SolidColorBrush.ColorProperty, TextColorTransition);

            Signal3.Background = Object;
            Object.BeginAnimation(SolidColorBrush.ColorProperty, TextColorTransition);

            Signal4.Background = Object;
            Object.BeginAnimation(SolidColorBrush.ColorProperty, TextColorTransition);
        }

        private void SignalStrength()
        {
            string targetInterfaceDescription = "Realtek RTL8852BE WiFi 6 802.11ax PCIe Adapter";
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            var targetAdapter = adapters.FirstOrDefault(adapter => adapter.Description == targetInterfaceDescription && adapter.NetworkInterfaceType == NetworkInterfaceType.Wireless80211);

            if (targetAdapter != null)
            {
                if (targetAdapter.OperationalStatus == OperationalStatus.Up)
                {
                    ConnectionType.Text = ("WiFi");
                }
                else
                {
                    ConnectionType.Text = ("Geen verbinding");
                    Signal1.Opacity = 0.5;
                    Signal2.Opacity = 0.5;
                    Signal3.Opacity = 0.5;
                    Signal4.Opacity = 0.5;
                }
            }
            else
            {
                ConnectionType.Text = ("Geen driver");
                Signal1.Opacity = 0.5;
                Signal2.Opacity = 0.5;
                Signal3.Opacity = 0.5;
                Signal4.Opacity = 0.5;
            }
        }
    }
}
