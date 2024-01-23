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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PiMusic.Music
{
    /// <summary>
    /// Interaction logic for MusicRadio.xaml
    /// </summary>
    public partial class MusicRadio : Page
    {
        public MusicRadio()
        {
            InitializeComponent();
        }

        private void RadioFomix_Click(object sender, RoutedEventArgs e)
        {
            MusicHandler.StartRadioPayload("Radio Fomix", "Fomixmedia", "https://stream.radiofomix.nl/listen/fomix/stream.mp3");
            MusicHome.Instance.MusicTitle.Text = MusicHandler.MusicMetadata.Title;
            MusicHome.Instance.MusicArtist.Text = MusicHandler.MusicMetadata.Artist;
            MusicHandler.MusicMetadata.Cover = new BitmapImage(new Uri("https://streamapi.radio.fomixmedia.nl/static/uploads/fomix/album_art.1700317533.png"));

            MusicHandler.MusicMetadata.Apply();
            MusicHome.Instance.BarAni();
        }

        private void QClick(object sender, RoutedEventArgs e)
        {
            MusicHandler.StartRadioPayload("QMusic (BE)", "DPG Media / Medialaan", "https://icecast-qmusicbe-cdp.triple-it.nl/qmusic.mp3");
            MusicHome.Instance.MusicTitle.Text = MusicHandler.MusicMetadata.Title;
            MusicHome.Instance.MusicArtist.Text = MusicHandler.MusicMetadata.Artist;
            MusicHandler.MusicMetadata.Cover = new BitmapImage(new Uri("https://th.bing.com/th/id/R.7bf382268cb80cbaf7c987e060ffd1cf?rik=bqAXBSnLuI6D3Q&riu=http%3a%2f%2fcdn-profiles.tunein.com%2fs87683%2fimages%2flogog.png%3ft%3d1&ehk=wAnaSnFT5ECpcbWsR1yySwnuIbklCQETSdLej9DOHcU%3d&risl=&pid=ImgRaw&r=0"));

            MusicHandler.MusicMetadata.Apply();
            MusicHome.Instance.BarAni();
        }

        private void VRTRADIO1(object sender, RoutedEventArgs e)
        {
            MusicHandler.StartRadioPayload("VRT Radio 1", "VRT", "http://icecast.vrtcdn.be/radio1-high.mp3");
            MusicHome.Instance.MusicTitle.Text = MusicHandler.MusicMetadata.Title;
            MusicHome.Instance.MusicArtist.Text = MusicHandler.MusicMetadata.Artist;
            MusicHandler.MusicMetadata.Cover = new BitmapImage(new Uri("https://upload.wikimedia.org/wikipedia/commons/thumb/0/08/VRT_Radio_1_logo.svg/2162px-VRT_Radio_1_logo.svg.png"));

            MusicHandler.MusicMetadata.Apply();
            MusicHome.Instance.BarAni();
        }

        private void STUBRU(object sender, RoutedEventArgs e)
        {
            MusicHandler.StartRadioPayload("VRT Studio Brussel", "VRT", "http://icecast.vrtcdn.be/stubru-high.mp3");
            MusicHome.Instance.MusicTitle.Text = MusicHandler.MusicMetadata.Title;
            MusicHome.Instance.MusicArtist.Text = MusicHandler.MusicMetadata.Artist;
            MusicHandler.MusicMetadata.Cover = new BitmapImage(new Uri("https://upload.wikimedia.org/wikipedia/commons/3/39/Studio_Brussel_logo.png"));

            MusicHandler.MusicMetadata.Apply();
            MusicHome.Instance.BarAni();
        }
    }
}
