using Avalonia.Animation;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Platform;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Avalonia.Threading;

namespace SoundOS6.Views.Music
{
    public partial class MusicHome : UserControl
    {
        public static MusicHome Instance;

        public static List<string> AllPaths;

        public MusicHome(List<string> AllSongs)
        {
            InitializeComponent();
            PageViewer.Content = new MusicLocal(AllSongs);
            AllPaths = AllSongs;
            Instance = this;
            StatusCheck();
        }

        public async void SetBar(Avalonia.Media.Imaging.Bitmap Cover)
        {
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                var imageBrush = new ImageBrush(Cover);
                CoverArt.Background = imageBrush;
            });
        }

        public async void StatusCheck()
        {
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                if (MusicHandler.IsPlaying == true || MusicHandler.MusicMetadata.Title != "")
                {
                    SetBar(MusicHandler.MusicMetadata.Cover);
                    MusicTitle.Text = MusicHandler.MusicMetadata.Title;
                    MusicArtist.Text = MusicHandler.MusicMetadata.Artist;

                    MusicTitle.Margin = new Thickness(125, 0, 0, 58);
                    MusicArtist.Opacity = 1;
                    PausePic.Opacity = 1;
                    ExpandBTN.IsEnabled = true;
                }
                else
                {
                    MusicTitle.Text = "Selecteer iets om af te spelen.";
                }

                if (MusicHandler.IsPlaying == false)
                {
                    using (var stream = AssetLoader.Open(new Uri("avares://SoundOS6/Assets/Icons/Light/Play.png")))
                    {
                        PausePic.Source = new Avalonia.Media.Imaging.Bitmap(stream);
                    }
                }
            });
            
        }

        private void PauseClick(object sender, RoutedEventArgs e)
        {
            if (MusicHandler.IsPlaying == false)
            {
                MusicHandler.ResumeMusic();

                using (var stream = AssetLoader.Open(new Uri("avares://SoundOS6/Assets/Icons/Light/Pause.png")))
                {
                    PausePic.Source = new Avalonia.Media.Imaging.Bitmap(stream);
                }

                return;
            }
            else
            {
                MusicHandler.PauseMusic();
                using (var stream = AssetLoader.Open(new Uri("avares://SoundOS6/Assets/Icons/Light/Play.png")))
                {
                    PausePic.Source = new Avalonia.Media.Imaging.Bitmap(stream);
                }

                return;
            }
        }

        private void ExpandBar(object sender, RoutedEventArgs e)
        {
            StatusShow.Content = new TVMode();
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

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            PageViewer.Content = new MusicRadio();
            LocalFiles.Opacity = 0.5;
            OnlineRadio.Opacity = 1;
        }

        private void LocalButton_Click(object sender, RoutedEventArgs e)
        {
            PageViewer.Content = new MusicLocal(AllPaths);
            OnlineRadio.Opacity = 0.5;
            LocalFiles.Opacity = 1;
        }
    }
}
