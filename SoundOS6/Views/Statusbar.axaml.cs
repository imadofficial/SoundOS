using Avalonia.Animation;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System;

namespace SoundOS6.Views
{
    public partial class Statusbar : UserControl
    {
        public static Statusbar Instance;
        DispatcherTimer Timer = new DispatcherTimer();
        public Statusbar()
        {
            InitializeComponent();
            Instance = this;

            Timer.Tick += new EventHandler(DefaultTimer);
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Start();
        }

        private void DefaultTimer(object sender, EventArgs e)
        {
            Time.Text = DateTime.Now.ToString("HH:mm");
        }

        public async void RemoveImage()
        {
            Icon.Opacity = 0;
            Icon.Margin = new Thickness(-10, 15, 0, 0);
        }

        public void StatusText(string Title)
        {
            CurrentApp.Text = Title;
        }

        public void IcoModify(string Path)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            Stream iconStream = asm.GetManifestResourceStream(Path);

            var Image = new Avalonia.Media.Imaging.Bitmap(iconStream);
            Icon.Source = Image;
        }



        public async void RemoveStatus()
        {
            var transition = new DoubleTransition
            {
                Duration = TimeSpan.FromSeconds(0.25),
                Easing = new Avalonia.Animation.Easings.QuadraticEaseInOut(),
                Property = Visual.OpacityProperty,
            };

            Icon.Transitions = new Transitions() { transition };
            CurrentApp.Transitions = new Transitions() { transition };

            CurrentApp.Opacity = 1;
            Icon.Opacity = 1;
            Icon.ApplyTemplate();
            await Task.Delay(250);
            CurrentApp.Opacity = 0;
            Icon.Opacity = 0;

            await Task.Delay(250);
            LeftSide.Margin = new Thickness(0, 1000, 0, 0);
        }

        public async void AppearStatus()
        {
            var transition = new DoubleTransition
            {
                Duration = TimeSpan.FromSeconds(0.25),
                Easing = new Avalonia.Animation.Easings.QuadraticEaseInOut(),
                Property = Visual.OpacityProperty,
            };

            Icon.Transitions = new Transitions() { transition };
            CurrentApp.Transitions = new Transitions() { transition };

            LeftSide.Margin = new Thickness(0, 0, 0, 0);

            CurrentApp.Opacity = 0;
            Icon.Opacity = 0;
            Icon.ApplyTemplate();
            await Task.Delay(250);
            CurrentApp.Opacity = 1;
            Icon.Opacity = 1;

            await Task.Delay(250);
        }
    }
}
