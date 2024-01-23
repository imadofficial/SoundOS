using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Net;
using System.Security.Policy;
using System.Net.Http;
using TagLib.Flac;
using PiMusic.Music;

namespace PiMusic
{
    internal class MusicHandler
    {
        private static WaveOut waveOut;
        public static bool IsPlaying;
        public static bool IsPlayingRadio;
        public static String CurrentPath = "";

        public static class MusicMetadata
        {
            public static String Title = "";
            public static String Artist = "";

            public static BitmapImage Cover;

            public static void Apply()
            {
                Home.Instance.SetMusicWidget(Title, Artist, IsPlayingRadio, Cover);
                MusicHome.Instance.StatusCheck();
                MusicBackground.Instance.CoverImage.Source = Cover;
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
                MemoryStream ms = new MemoryStream(bin);

                BitmapImage albumCover = new BitmapImage();
                albumCover.BeginInit();
                albumCover.StreamSource = ms;
                albumCover.EndInit();

                MusicMetadata.Cover = albumCover;
            }

            MusicMetadata.Apply();
        }


        public static void StartRadioPayload(String RadioName, String RadioCompany, String URL)
        {
            StopMusic();

            MusicMetadata.Artist = RadioCompany;
            MusicMetadata.Title = RadioName;

            using (var mediaStream = new MediaFoundationReader(URL))
            {
                waveOut = new WaveOut();
                waveOut.Init(mediaStream);
                waveOut.Play();
                IsPlayingRadio = true;
                IsPlaying = true;
            }
        }

        public static void StartMusic(string Path)
        {
            StopMusic();

            var reader = new Mp3FileReader(Path);
            waveOut = new WaveOut();
            waveOut.Init(reader);
            waveOut.Play();

            CurrentPath = Path;
            IsPlaying = true;

            CollectMetadata(Path);
        }

        public static void PauseMusic()
        {
            waveOut.Pause();
            IsPlaying = false;
            return;
        }

        public static void ResumeMusic()
        {
            waveOut.Resume();
            IsPlaying = true;
            return;
        }

        public static void StopMusic()
        {
            if (waveOut != null)
            {
                waveOut.Stop();
                waveOut.Dispose();
                waveOut = null;
                IsPlaying = false;
            }
        }
    }
}
