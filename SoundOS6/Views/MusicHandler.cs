using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;
using LibVLCSharp.Shared;
using SoundOS6.Views.Music;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace SoundOS6.Views
{
    internal class MusicHandler
    {
        public static MediaPlayer _stream;
        public static bool IsPlaying;
        public static bool IsPlayingRadio;
        public static String CurrentPath = "";
        public static DispatcherTimer _TimeElapsed;
        public static string EndTimes;
        public static double EndTimesInSeconds;
        public static double TimeElapsedInSeconds;
        public static string CurrentSongPath;

        public static class MusicMetadata
        {
            public static String Title = "";
            public static String Artist = "";

            public static Bitmap Cover;

            public static async void Apply()
            {
                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    MainView.Instance.SetMusicWidget(Title, Artist, IsPlayingRadio, Cover);
                    MusicHome.Instance.StatusCheck();
                    MusicHome.Instance.AlbumCoverBG.Source = Cover;

                    try
                    {
                        MusicHome.Instance.SetBar(Cover);
                        TVMode.Instance.SetBar(Cover);
                        TVMode.Instance.MusicTitle.Text = Title;
                        TVMode.Instance.MusicArtist.Text = Artist;
                    }
                    catch (Exception ex) { }
                });
                
            }
        }

        public static void CollectMetadata(string Path)
        {
            TagLib.File tagFile = TagLib.File.Create(Path);

            MusicMetadata.Title = tagFile.Tag.Title;
            MusicMetadata.Artist = tagFile.Tag.FirstPerformer;

            if (tagFile.Tag.Pictures.Length >= 1)
            {
                var bin = (byte[])(tagFile.Tag.Pictures[0].Data.Data);
                using (MemoryStream ms = new MemoryStream(bin))
                {
                    Bitmap albumCover = new Bitmap(ms);
                    MusicMetadata.Cover = albumCover;
                }
            }

            TimeSpan duration = tagFile.Properties.Duration;
            EndTimes = duration.ToString(@"m\:ss");

            MainView.Instance.EndTime.Text = EndTimes;
            EndTimesInSeconds = duration.TotalSeconds;

            try
            {
                TVMode.Instance.TimerEnd.Text = duration.ToString(@"m\:ss");
            }
            catch (Exception) { };


            MusicMetadata.Apply();
        }

        public static void StartRadioPayload(string radioName, string radioCompany, string url)
        {
            StopMusic();

            MusicMetadata.Artist = radioCompany;
            MusicMetadata.Title = radioName;

            Core.Initialize();

            using var libVLC = new LibVLC();
            _stream = new MediaPlayer(libVLC);

            var media = new Media(libVLC, url, FromType.FromLocation);

            // Check if running on Linux
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux))
            {
                // Optimize for Linux
                media.AddOption(":network-caching=300");
            }

            _stream.Media = media;
            _stream.Play();

            IsPlayingRadio = true;
            IsPlaying = true;

            _TimeElapsed = new DispatcherTimer();
            _TimeElapsed.Interval = TimeSpan.FromMilliseconds(500);
            _TimeElapsed.Tick += ElapsedProcess;
            _TimeElapsed.Start();
        }

        public async static void ElapsedProcess(object sender, EventArgs e)
        {
            var positionInMilliseconds = MusicHandler._stream.Time;
            var currentPosition = TimeSpan.FromMilliseconds(0);

            try
            {
                currentPosition = TimeSpan.FromMilliseconds(positionInMilliseconds);
            }
            catch (OverflowException)
            {
                currentPosition = TimeSpan.FromMilliseconds(0);
            }

            string currentPositionFormatted = string.Format("{0}:{1:D2}", currentPosition.Minutes, currentPosition.Seconds);

            MusicHome.Instance.TimerElapsed.Text = currentPositionFormatted;
            MainView.Instance.CurrentTime.Text = currentPositionFormatted;
            double currentPositionInSeconds = currentPosition.TotalSeconds;

            try
            {
                TVMode.Instance.TimerElapsed.Text = currentPositionFormatted;
                TVMode.Instance.Slider.Value = currentPositionInSeconds;
            }
            catch (Exception)
            {
            }
        }

        public static void StartMusic(string path, bool StopAdvanced)
        {
            CurrentSongPath = path;

            if (StopAdvanced)
            {
                StopMusic();
            }
            
            Core.Initialize();
            
            Debug.WriteLine("Song Started: '" + path + "'");

            using var libVLC = new LibVLC();
            _stream = new MediaPlayer(libVLC);

            var media = new Media(libVLC, path, FromType.FromPath);

            // Check if running on Linux
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux))
            {
                // Optimize for Linux
                media.AddOption(":file-caching=300");
            }

            _stream.Media = media;
            _stream.Play();

            CurrentPath = path;
            IsPlayingRadio = false;
            IsPlaying = true;

            CollectMetadata(path);
            
            _stream.EndReached += OnSongEnd;
        }

        private static async void OnSongEnd(object sender, EventArgs e)
        {
            _stream.Pause();
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                int index = MusicSplash.Instance.filePaths.IndexOf(CurrentSongPath);
                if (index != -1 && index + 1 < MusicSplash.Instance.filePaths.Count)
                {
                    StartMusic(MusicSplash.Instance.filePaths[index + 1], false);

                    TagLib.File tagFile = TagLib.File.Create(MusicSplash.Instance.filePaths[index + 1]);

                    MusicHome.Instance.PausePic.Opacity = 1;
                    MainView.Instance.PlayCommandBtn.IsEnabled = true;

                    MainView.Instance.UnhideMusicWidget();

                    using (var stream = AssetLoader.Open(new Uri("avares://SoundOS6/Assets/Icons/Light/Pause.png")))
                    {
                        MusicHome.Instance.PausePic.Source = new Avalonia.Media.Imaging.Bitmap(stream);
                    }

                    _TimeElapsed = new DispatcherTimer();
                    _TimeElapsed.Interval = TimeSpan.FromMilliseconds(500);
                    _TimeElapsed.Tick += MusicLocal.ElapsedProcess;
                    _TimeElapsed.Start();
                }
                else
                {
                    StartMusic(MusicSplash.Instance.filePaths[0], false);

                    TagLib.File tagFile = TagLib.File.Create(MusicSplash.Instance.filePaths[0]);

                    MusicHome.Instance.PausePic.Opacity = 1;
                    MainView.Instance.PlayCommandBtn.IsEnabled = true;

                    MainView.Instance.UnhideMusicWidget();

                    using (var stream = AssetLoader.Open(new Uri("avares://SoundOS6/Assets/Icons/Light/Pause.png")))
                    {
                        MusicHome.Instance.PausePic.Source = new Avalonia.Media.Imaging.Bitmap(stream);
                    }

                    _TimeElapsed = new DispatcherTimer();
                    _TimeElapsed.Interval = TimeSpan.FromMilliseconds(500);
                    _TimeElapsed.Tick += MusicLocal.ElapsedProcess;
                    _TimeElapsed.Start();
                }
            });

            
        }


        public static void PauseMusic()
        {
            _stream.Pause();
            IsPlaying = false;
            return;
        }

        public static void ResumeMusic()
        {
            _stream.Pause();
            IsPlaying = true;
            return;
        }

        public static void StopMusic()
        {
            if (_stream != null)
            {
                _stream.Stop();
                _stream.Dispose();
                _stream = null;
                IsPlaying = false;
            }
        }
    }
}
