using Avalonia.Animation;
using Avalonia;
using Avalonia.Controls;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;

namespace SoundOS6.Views.Music
{
    public partial class MusicSplash : UserControl
    {
        public List<string> filePaths = new List<string>();
        public static MusicSplash Instance;

        public MusicSplash()
        {
            InitializeComponent();
            Instance = this;
            Init();
        }

        async void Init()
        {
            var transition = new DoubleTransition
            {
                Duration = TimeSpan.FromSeconds(0.3),
                Easing = new Avalonia.Animation.Easings.QuadraticEaseInOut(),
                Property = Visual.OpacityProperty,
            };

            Bar.Transitions = new Transitions() { transition };

            Bar.Opacity = 0;
            Bar.ApplyTemplate();
            await Task.Delay(500);
            Bar.Opacity = 1;

            await CheckDirsAsync();
        }

        private async Task CheckDirsAsync()
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();

            foreach (DriveInfo d in allDrives)
            {
                string dirPath = d.Name;
                Status.Text = "Opslagmedia controleren (" + d.Name + ")";

                try
                {
                    if (d.Name != @"C:\" || d.Name != @"D:\")
                    {
                        int FilesFound = 0;
                        string[] files = await Task.Run(() => System.IO.Directory.GetFiles(dirPath, "*.mp3", System.IO.SearchOption.AllDirectories));
                        foreach (string file in files)
                        {
                            filePaths.Add(file);
                            Status.Text = "Opslagmedia controleren (" + file + ")";
                            FilesFound++;
                            Aantal.Text = FilesFound + " Bestanden gevonden";
                        }
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    // Handle exceptions appropriately
                }
                catch (IOException)
                {
                    // Handle exceptions appropriately
                }
                catch (Exception)
                {
                    // Handle exceptions appropriately
                }
            }

            string json = JsonConvert.SerializeObject(filePaths, Newtonsoft.Json.Formatting.Indented);
            string exeDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string configDirectory = Path.Combine(exeDirectory, "Config");

            if (!Directory.Exists(configDirectory))
            {
                Directory.CreateDirectory(configDirectory);
            }

            string filePath = Path.Combine(configDirectory, "SongIndex.json");
            await File.WriteAllTextAsync(filePath, json);

            await Task.Delay(300);

            FinishHandler();
        }

        private async void FinishHandler()
        {
            Status.Text = "Starten...";

            var transition = new DoubleTransition
            {
                Duration = TimeSpan.FromSeconds(0.3),
                Easing = new Avalonia.Animation.Easings.QuadraticEaseInOut(),
                Property = Visual.OpacityProperty,
            };

            Aantal.Transitions = new Transitions() { transition };
            Status.Transitions = new Transitions() { transition };
            MusicIcon.Transitions = new Transitions() { transition };

            Aantal.ApplyTemplate();
            await Task.Delay(500);
            Aantal.Opacity = 0;
            Status.Opacity = 0;
            MusicIcon.Opacity = 0;

            await Task.Delay(400);

            MainView.Instance.PageViewer.Content = new MusicHome(filePaths);
        }
    }
}
