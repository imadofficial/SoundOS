using Avalonia;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using System.IO;
using System.Net.Http;

namespace SoundOS6.Views.Music
{
    public partial class MusicRadio : UserControl
    {
        public MusicRadio()
        {
            InitializeComponent();
        }

        private async void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            MusicHandler.StartRadioPayload("MPB Radio 1", "MPBNL", "https://radiozender.mpbnetwerken.nl/radio1.mp3");
            MusicHome.Instance.MusicTitle.Text = MusicHandler.MusicMetadata.Title;
            MusicHome.Instance.MusicArtist.Text = MusicHandler.MusicMetadata.Artist;

            var client = new HttpClient();
            var response = await client.GetAsync("https://plus.imadsnetwork.net/icon/Radio/STUBRU.png");
            var bytes = await response.Content.ReadAsByteArrayAsync();
            using var ms = new MemoryStream(bytes);
            var bitmap = new Bitmap(ms);
            MusicHandler.MusicMetadata.Cover = bitmap;


            MusicHandler.MusicMetadata.Apply();
            MusicHome.Instance.MusicTitle.Margin = new Thickness(125, 0, 0, 58);
            MusicHome.Instance.MusicArtist.Opacity = 1;
            MusicHome.Instance.PausePic.Opacity = 1;
            MusicHome.Instance.ExpandBTN.IsEnabled = true;
        }
    }
}
