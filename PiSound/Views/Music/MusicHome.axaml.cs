using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Platform;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Avalonia.Animation;

namespace PiSound.Views.Music
{
    public partial class MusicHome : UserControl
    {
        public static MusicHome Instance;
        public MusicHome(List<string> AllSongs)
        {
            InitializeComponent();
            PageViewer.Content = new MusicLocal(AllSongs);
            Instance = this;
            StatusCheck();
        }

        public async void StatusCheck()
        {
            if (MusicHandler.IsPlaying == true || MusicHandler.MusicMetadata.Title != "")
            {
                MusicTitle.Text = MusicHandler.MusicMetadata.Title;
                MusicArtist.Text = MusicHandler.MusicMetadata.Artist;

                MusicTitle.Margin = new Thickness(125, 0, 0, 58);
                MusicArtist.Opacity = 1;

                //CoverArt.ImageSource = MusicHandler.MusicMetadata.Cover;
                PausePic.Opacity = 1;
            }

            if(MusicHandler.IsPlaying == false)
            {
                using (var stream = AssetLoader.Open(new Uri("avares://PiSound/Assets/Icons/Light/Play.png")))
                {
                    PausePic.Source = new Avalonia.Media.Imaging.Bitmap(stream);
                }
            }
        }

        private void PauseClick(object sender, RoutedEventArgs e)
        {
            if (MusicHandler.IsPlaying == false)
            {
                MusicHandler.ResumeMusic();

                using (var stream = AssetLoader.Open(new Uri("avares://PiSound/Assets/Icons/Light/Pause.png")))
                {
                    PausePic.Source = new Avalonia.Media.Imaging.Bitmap(stream);
                }

                return;
            }
            else
            {
                MusicHandler.PauseMusic();
                using (var stream = AssetLoader.Open(new Uri("avares://PiSound/Assets/Icons/Light/Play.png")))
                {
                    PausePic.Source = new Avalonia.Media.Imaging.Bitmap(stream);
                }

                return;
            }
        }

        private async void BackTap(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var transition = new DoubleTransition
            {
                Duration = TimeSpan.FromSeconds(0.3),
                Easing = new Avalonia.Animation.Easings.QuadraticEaseInOut(),
                Property = Visual.OpacityProperty,
            };

            MainView.Instance.PageViewer.Transitions = new Transitions() { transition };

            MainView.Instance.PageViewer.Opacity = 1;
            MainView.Instance.PageViewer.ApplyTemplate();
            await Task.Delay(300);
            MainView.Instance.PageViewer.Opacity = 0;
            await Task.Delay(300);
            MainView.Instance.PageViewer.Opacity = 1;
            MainView.Instance.PageViewer.Content = null;
            Statusbar.Instance.AppearStatus();
        }
    }
}
