using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Platform;
using System.Threading.Tasks;
using System.Threading;
using System;
using LibVLCSharp.Shared;
using System.IO;
using Avalonia.Threading;

namespace SoundOS6.Views.Music
{
    public partial class TVMode : UserControl
    {
        public static TVMode Instance;

        public TVMode()
        {
            InitializeComponent();
            StatusCheck();
            Instance = this;
            TimerEnd.Text = MusicHandler.EndTimes;
            Slider.Maximum = MusicHandler.EndTimesInSeconds;
            if (MusicHandler.IsPlayingRadio)
            {
                BackABit.Opacity = 0;
                ForwardABit.Opacity = 0;
            }
            VolumeSlider.Value = MusicHandler._stream.Volume;
        }

        public void SetBar(Avalonia.Media.Imaging.Bitmap Cover)
        {
            var imageBrush = new ImageBrush(Cover);
            CoverArt.Background = imageBrush;
        }

        public void StatusCheck()
        {
            if (MusicHandler.IsPlaying == true || MusicHandler.MusicMetadata.Title != "")
            {
                SetBar(MusicHandler.MusicMetadata.Cover);
                MusicTitle.Text = MusicHandler.MusicMetadata.Title;
                MusicArtist.Text = MusicHandler.MusicMetadata.Artist;
            }
            else
            {
                //MusicTitle.Text = "Selecteer iets om af te spelen.";
            }
        }

        private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            MusicHome.Instance.StatusShow.Content = null;
        }

        private void PauseBTN(object? sender, Avalonia.Input.TappedEventArgs e)
        {
            if (MusicHandler.IsPlaying == false)
            {
                MusicHandler.ResumeMusic();

                using (var stream = AssetLoader.Open(new Uri("avares://SoundOS6/Assets/Icons/Light/Pause.png")))
                {
                    Pause.Source = new Avalonia.Media.Imaging.Bitmap(stream);
                }

                return;
            }
            else
            {
                MusicHandler.PauseMusic();
                using (var stream = AssetLoader.Open(new Uri("avares://SoundOS6/Assets/Icons/Light/Play.png")))
                {
                    Pause.Source = new Avalonia.Media.Imaging.Bitmap(stream);
                }

                return;
            }
        }

        private void ScrubBack(object? sender, Avalonia.Input.TappedEventArgs e)
        {
            MusicHandler._stream.Time -= 15000;
        }

        private void ScrubForward(object? sender, Avalonia.Input.TappedEventArgs e)
        {
            MusicHandler._stream.Time += 15000;
        }

        private async void BackSong(object? sender, Avalonia.Input.TappedEventArgs e)
        {
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                MusicHandler._stream.Stop();
                int index = MusicSplash.Instance.filePaths.IndexOf(MusicHandler.CurrentSongPath);

                try
                {
                    MusicHandler.StartMusic(MusicSplash.Instance.filePaths[index - 1], false);
                    index = index - 1;

                    TagLib.File tagFile = TagLib.File.Create(MusicSplash.Instance.filePaths[index + 1]);
                }
                catch (ArgumentOutOfRangeException)
                {
                    int KeyAmount = MusicSplash.Instance.filePaths.Count;
                    MusicHandler.StartMusic(MusicSplash.Instance.filePaths[KeyAmount - 1], false);
                    index = KeyAmount - 1;

                    TagLib.File tagFile = TagLib.File.Create(MusicSplash.Instance.filePaths[0]);
                }

                MusicHome.Instance.PausePic.Opacity = 1;
                MainView.Instance.PlayCommandBtn.IsEnabled = true;



                MainView.Instance.UnhideMusicWidget();

                using (var stream = AssetLoader.Open(new Uri("avares://SoundOS6/Assets/Icons/Light/Pause.png")))
                {
                    MusicHome.Instance.PausePic.Source = new Avalonia.Media.Imaging.Bitmap(stream);
                }

                MusicHandler._TimeElapsed = new DispatcherTimer();
                MusicHandler._TimeElapsed.Interval = TimeSpan.FromMilliseconds(500);
                MusicHandler._TimeElapsed.Tick += MusicLocal.ElapsedProcess;
                MusicHandler._TimeElapsed.Start();
            });
        }

        private async void ForwardSong(object? sender, Avalonia.Input.TappedEventArgs e)
        {
            MusicHandler._stream.Stop();
            int index = MusicSplash.Instance.filePaths.IndexOf(MusicHandler.CurrentSongPath);
            if (index != -1 && index + 1 < MusicSplash.Instance.filePaths.Count)
            {
                MusicHandler.StartMusic(MusicSplash.Instance.filePaths[index + 1], false);

                TagLib.File tagFile = TagLib.File.Create(MusicSplash.Instance.filePaths[index + 1]);

                MusicHome.Instance.PausePic.Opacity = 1;
                MainView.Instance.PlayCommandBtn.IsEnabled = true;

                MainView.Instance.UnhideMusicWidget();

                using (var stream = AssetLoader.Open(new Uri("avares://SoundOS6/Assets/Icons/Light/Pause.png")))
                {
                    MusicHome.Instance.PausePic.Source = new Avalonia.Media.Imaging.Bitmap(stream);
                }
            }
            else
            {
                MusicHandler.StartMusic(MusicSplash.Instance.filePaths[0], false);

                TagLib.File tagFile = TagLib.File.Create(MusicSplash.Instance.filePaths[0]);

                MusicHome.Instance.PausePic.Opacity = 1;
                MainView.Instance.PlayCommandBtn.IsEnabled = true;

                MainView.Instance.UnhideMusicWidget();

                using (var stream = AssetLoader.Open(new Uri("avares://SoundOS6/Assets/Icons/Light/Pause.png")))
                {
                    MusicHome.Instance.PausePic.Source = new Avalonia.Media.Imaging.Bitmap(stream);
                }
            }

            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                MusicHandler._TimeElapsed = new DispatcherTimer();
                MusicHandler._TimeElapsed.Interval = TimeSpan.FromMilliseconds(500);
                MusicHandler._TimeElapsed.Tick += MusicLocal.ElapsedProcess;
                MusicHandler._TimeElapsed.Start();
            });
        }

        private void VolChanged(object? sender, Avalonia.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            MusicHandler._stream.Volume = Convert.ToInt32(VolumeSlider.Value);
        }
    }
}
