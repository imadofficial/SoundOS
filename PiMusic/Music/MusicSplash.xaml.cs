using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Formats.Tar;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace PiMusic.Music
{
    /// <summary>
    /// Interaction logic for MusicSplash.xaml
    /// </summary>
    public partial class MusicSplash : Page
    {
        public List<string> filePaths = new List<string>();
        public static MusicSplash Instance;

        public MusicSplash()
        {
            InitializeComponent();
            Instance = this;
            Init();
            UpdateBar();
        }

        private async void UpdateBar()
        {
            QuinticEase c = new QuinticEase();
            c.EasingMode = EasingMode.EaseInOut;

            ThicknessAnimation PosCorrect = new ThicknessAnimation()
            {
                To = new Thickness(125, 0, 0, 58),
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = c
            };

            DoubleAnimation Appear = new DoubleAnimation()
            {
                To = 1,
                Duration = TimeSpan.FromSeconds(0.5)
            };

            if (MusicHandler.IsPlaying == true)
            {
                MusicTitle.Text = MusicHandler.MusicMetadata.Title;
                MusicArtist.Text = MusicHandler.MusicMetadata.Artist;

                MusicTitle.BeginAnimation(TextBlock.MarginProperty, PosCorrect);
                await Task.Delay(500);
                MusicArtist.BeginAnimation(TextBlock.OpacityProperty, Appear);

                Cover.ImageSource = MusicHandler.MusicMetadata.Cover;
            }
        }

        private async void Init()
        {
            QuinticEase c = new QuinticEase();
            c.EasingMode = EasingMode.EaseOut;

            DoubleAnimation Appear = new DoubleAnimation()
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.4)
            };

            ThicknessAnimation FlyIn = new ThicknessAnimation()
            {
                From = new Thickness(0,0,0,-100),
                To = new Thickness(0, 0, 0, -25),
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = c
            };

            Load.BeginAnimation(Grid.OpacityProperty, Appear);
            Navbar.Instance.ColorSwitch(1);
            await Task.Delay(500);

            Bar.BeginAnimation(Grid.MarginProperty, FlyIn);

            await Task.Delay(1000);
            Status.Text = "Opslagmedia controleren";

            await CheckDirsAsync();
        }

        private async Task CheckDirsAsync() //Zoekt naar .mp3 bestanden op de schijf
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            

            foreach (DriveInfo d in allDrives)
            {
                string dirPath = d.Name;
                Status.Text = "Opslagmedia controleren (" + d.Name + ")";

                try
                {
                    if(d.Name != @"C:\")
                    {
                        int FilesFound = 0;
                        string[] files = await Task.Run(() => System.IO.Directory.GetFiles(dirPath, "*.mp3", System.IO.SearchOption.AllDirectories));
                        foreach (string file in files)
                        {
                            filePaths.Add(file);
                            Status.Text = "Opslagmedia controleren (" + file + ")";
                            FilesFound = FilesFound + 1;
                            Aantal.Text = FilesFound + " Bestanden gevonden";
                        }
                    }
                }
                catch (Exception)
                {
                    continue;
                }
            
            }
            string json = JsonConvert.SerializeObject(filePaths, Newtonsoft.Json.Formatting.Indented);
            string exeDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string configDirectory = System.IO.Path.Combine(exeDirectory, "Config");

            if (!Directory.Exists(configDirectory))
            {
                Directory.CreateDirectory(configDirectory);
            }

            string filePath = System.IO.Path.Combine(configDirectory, "SongIndex.json");
            File.WriteAllText(filePath, json);


            await Task.Delay(300);

            FinishHandler();
        }

        private async void FinishHandler()
        {
            Status.Text = "Starten...";

            DoubleAnimation Dissapear = new DoubleAnimation()
            {
                To = 0,
                Duration = TimeSpan.FromSeconds(0.4)
            };

            Aantal.BeginAnimation(TextBlock.OpacityProperty, Dissapear);
            Status.BeginAnimation(TextBlock.OpacityProperty, Dissapear);
            MusicIcon.BeginAnimation(Image.OpacityProperty, Dissapear);

            Statusbar.Instance.CurrentApp.BeginAnimation(TextBlock.OpacityProperty, Dissapear);
            Statusbar.Instance.Icon.BeginAnimation(Image.OpacityProperty, Dissapear);

            await Task.Delay(400);

            MainWindow.Instance.MainContent.Content = new MusicHome();
        }
    }
}
