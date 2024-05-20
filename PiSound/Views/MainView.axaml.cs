﻿using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using PiSound.Views.Music;
using System;
using System.ComponentModel;

namespace PiSound.Views
{
    public partial class MainView : UserControl
    {
        public static MainView Instance;
        public MainView()
        {
            InitializeComponent();
            Instance = this;
            StatusShow.Content = new Statusbar();
        }

        private void Musicbox_Click(object sender, RoutedEventArgs e)
        {
            PageViewer.Content = new MusicSplash();
            Statusbar.Instance.RemoveStatus();
        }

        public void UnhideMusicWidget()
        {
            Instructions.Opacity = 0;
            SongTitle.Opacity = 1;
            SongArtist.Opacity = 1;
            CoverArt.Opacity = 1;
            PlayerControls.Opacity = 1;
        }

        public void SetMusicWidget(String Title, String Artist, bool Live, Bitmap Cover, TimeSpan? SongSpan = null)
        {
            var imageBrush = new ImageBrush(Cover);
            if (Live == true)
            {
                CoverArt.Background = imageBrush;

                CurrentTime.Opacity = 0;
                EndTime.Opacity = 0;
                BackgroundBar.Opacity = 0;
                LiveLbl.Opacity = 1;
            }
            else
            {
                CurrentTime.Opacity = 1;
                EndTime.Opacity = 1;
                BackgroundBar.Opacity = 1;
                LiveLbl.Opacity = 0;

                EndTime.Text = SongSpan.ToString();
            }

            Instructions.Opacity = 0;
            SongTitle.Opacity = 1;
            SongArtist.Opacity = 1;
            CoverArt.Opacity = 1;
            Play.Opacity = 1;
            PlayingStat.Opacity = 1;

            SongTitle.Text = Title;
            SongArtist.Text = Artist;

            CoverArt.Background = imageBrush;
        }

        private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (MusicHandler.IsPlaying == false)
            {
                MusicHandler.ResumeMusic();

                using (var stream = AssetLoader.Open(new Uri("avares://PiSound/Assets/Icons/Light/Pause.png")))
                {
                    Play.Source = new Avalonia.Media.Imaging.Bitmap(stream);
                }

                PlayingStat.Text = "Pauzeren";

                return;
            }
            else
            {
                MusicHandler.PauseMusic();
                using (var stream = AssetLoader.Open(new Uri("avares://PiSound/Assets/Icons/Light/Play.png")))
                {
                    Play.Source = new Avalonia.Media.Imaging.Bitmap(stream);
                }

                PlayingStat.Text = "Afspelen";

                return;
            }
        }
    }
}