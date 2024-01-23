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
using static PiMusic.Home;

namespace PiMusic.Apps.Weather
{
    /// <summary>
    /// Logique d'interaction pour MainWeather.xaml
    /// </summary>
    public partial class MainWeather : Page
    {
        public MainWeather()
        {
            InitializeComponent();
            Statusbar.Instance.SwitchColor(1);
            Statusbar.Instance.StatusText("Hudige Weer");
            Statusbar.Instance.IcoModify("PiMusic.Assets.Icons.Light.WeerIcon.png");

            Init();
        }

        public async void Init()
        {
            await Home.Instance.GetWeather();

            var WeatherData = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(GlobalStrings.WeatherResponse);


            Stad.Text = WeatherData["location"]["name"];
            Graad.Text = WeatherData["current"]["temp_c"] + "°";
            Conditie.Text = WeatherData["current"]["condition"]["text"];
        }

        public async void Closure()
        {
            //MainPanel.BeginAnimation(Grid.OpacityProperty, Dissapear);
            MainPanel.Opacity = 0;
            await Task.Delay(700);

            MainWindow.Instance.ConfirmClosure();
        }
    }
}
