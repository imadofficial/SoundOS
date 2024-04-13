using Avalonia.Media.Imaging;
using Avalonia.Threading;
using CSCore;
using CSCore.Codecs;
using CSCore.SoundOut;
using PiSound.Views.Music;
using System;
using System.Formats.Tar;
using System.IO;
using System.Threading.Tasks;

namespace PiSound.Views
{
    internal class MusicHandler
    {
        public static ISoundOut _mediaPlayer;
        public static IWaveSource _waveSource;
        public static bool IsPlaying;
        public static bool IsPlayingRadio;
        public static String CurrentPath = "";
        private static DispatcherTimer _TimeElapsed;

        public static class MusicMetadata
        {
            public static String Title = "";
            public static String Artist = "";

            public static Bitmap Cover;

            public static void Apply()
            {
                MainView.Instance.SetMusicWidget(Title, Artist, IsPlayingRadio, Cover);
                MusicHome.Instance.StatusCheck();
                MusicHome.Instance.AlbumCoverBG.Source = Cover;
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

            MusicMetadata.Apply();
        }

        public static void StartRadioPayload(String RadioName, String RadioCompany, String URL)
        {
            StopMusic();

            MusicMetadata.Artist = RadioCompany;
            MusicMetadata.Title = RadioName;

            _waveSource = CodecFactory.Instance.GetCodec(URL);
            _mediaPlayer = new DirectSoundOut();
            _mediaPlayer.Initialize(_waveSource);
            _mediaPlayer.Play();

            IsPlayingRadio = true;
            IsPlaying = true;
        }

        public static void StartMusic(string Path)
        {
            StopMusic();

            _waveSource = CodecFactory.Instance.GetCodec(Path);
            _mediaPlayer = new DirectSoundOut();
            _mediaPlayer.Initialize(_waveSource);
            _mediaPlayer.Play();

            CurrentPath = Path;
            IsPlaying = true;

            _TimeElapsed = new DispatcherTimer();
            _TimeElapsed.Interval = TimeSpan.FromMilliseconds(500);
            _TimeElapsed.Tick += ElapsedProcess;
            _TimeElapsed.Start();

            CollectMetadata(Path);
        }
        async static void ElapsedProcess(object sender, EventArgs e)
        {
            // Call your method here
        }


        public static void PauseMusic()
        {
            _mediaPlayer.Pause();
            IsPlaying = false;
            return;
        }

        public static void ResumeMusic()
        {
            _mediaPlayer.Play();
            IsPlaying = true;
            return;
        }

        public static void StopMusic()
        {
            if (_mediaPlayer != null)
            {
                _mediaPlayer.Stop();
                _mediaPlayer.Dispose();
                _mediaPlayer = null;
                _waveSource.Dispose();
                _waveSource = null;
                IsPlaying = false;
            }
        }
    }
}
