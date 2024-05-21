using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Platform;
using System.Threading.Tasks;
using System.Threading;
using System;
using LibVLCSharp.Shared;

namespace SoundOS6.Views.Music
{
    public partial class TVMode : UserControl
    {
        public static TVMode Instance;
        public bool NotDragged = true;
        private bool isUserInteracting;

        public TVMode()
        {
            InitializeComponent();
            StatusCheck();
            Instance = this;
            TimerEnd.Text = MusicHandler.EndTimes;
            Slider.Maximum = MusicHandler.EndTimesInSeconds;
            if(MusicHandler.IsPlayingRadio)
            {
                BackABit.Opacity = 0;
                ForwardABit.Opacity = 0;
            }
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

        private void Slider_PointerPressed(object sender, VectorEventArgs e)
        {
            NotDragged = false;
            TimerElapsed.Opacity = 0.5;
        }

        private void Slider_PointerReleased(object sender, VectorEventArgs e)
        {
            NotDragged = true;
        }

        private CancellationTokenSource cts = new CancellationTokenSource();


        private async void Slider_ValueChanged(object? sender, Avalonia.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            if (isUserInteracting)
            {
                NotDragged = false;
                TimerElapsed.Opacity = 0.5;

                cts.Cancel();
                cts = new CancellationTokenSource();

                try
                {
                    await Task.Delay(500, cts.Token);

                    NotDragged = true;
                    TimerElapsed.Opacity = 1;
                }
                catch (TaskCanceledException)
                {
                    // The delay was cancelled, so we don't do anything
                }
            }

        }

        private void Slider_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
        {
            isUserInteracting = true;
        }

        private void Slider_PointerReleased(object? sender, Avalonia.Input.PointerReleasedEventArgs e)
        {
            isUserInteracting = false;

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

        private void BackSong(object? sender, Avalonia.Input.TappedEventArgs e)
        {
            MusicHandler._stream.Time -= 15000;
        }

        private void ForwardSong(object? sender, Avalonia.Input.TappedEventArgs e)
        {
            MusicHandler._stream.Time += 15000;
        }
    }
}
