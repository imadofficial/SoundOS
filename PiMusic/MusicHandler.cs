using NAudio.Wave;
using PiMusic.Music;
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

namespace PiMusic
{
    internal class MusicHandler
    {
        private static WaveOut waveOut;
        public static bool IsPlaying;
        public static String CurrentPath = "";

        public static class MusicMetadata
        {
            public static String Title = "";
            public static String Artist = "";

            public static BitmapImage Cover;
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

        public static void PauseMusic() //0 = Playing, 1 = Paused
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
