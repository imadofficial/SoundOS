using NAudio.Wave;
using PiMusic.Music;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using Windows.Devices.Geolocation;

namespace PiMusic
{
    internal class MusicHandler
    {
        private static WaveOut waveOut;
        public static String Title;
        public static String Artist;

        public static bool IsPlaying;

        public static void StartMusic(string Path)
        {
            StopMusic();

            var reader = new Mp3FileReader(Path);
            waveOut = new WaveOut();
            waveOut.Init(reader);
            waveOut.Play();

            IsPlaying = true;
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
