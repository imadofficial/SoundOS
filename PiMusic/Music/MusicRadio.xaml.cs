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
            MusicHandler.MusicMetadata.Cover = new BitmapImage(new Uri("https://fomix.nl/storage/files/V09c8QKGOmOfOC7c07andTbEVpMz3et0bItlV4Ft.png"));

            MusicHandler.MusicMetadata.Apply();
            MusicHome.Instance.BarAni();
        }
    }
}
