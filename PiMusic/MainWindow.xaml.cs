using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TagLib.Id3v2;

namespace PiMusic
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Instance;
        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
            HomeScreen.Content = new Home();
            Statebar.Content = new Statusbar();
            Navbar.Content = new Navbar();
        }

        public void ConfirmClosure()
        {
            // Dispose of the content if it's disposable
            if (MainContent.Content is IDisposable disposableContent)
            {
                disposableContent.Dispose();
            }

            MainContent.Content = null;
            MainContent.NavigationService.RemoveBackEntry();

            // Dispose of the content if it's disposable
            if (Navbar.Content is IDisposable disposableNavbar)
            {
                disposableNavbar.Dispose();
            }

            Navbar.Content = null;
            Navbar.Content = new Navbar();

            Statusbar.Instance.SwitchColor(0);

            Statusbar.Instance.AppearStatus();
        }
  
    }
}
