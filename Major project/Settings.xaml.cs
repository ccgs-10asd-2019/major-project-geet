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
using System.Windows.Shapes;

namespace Major_project
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {

        MainWindow mainWindow;

        public Settings(MainWindow window)
        {
            InitializeComponent();
            mainWindow = window;
        }

        private void Close_Settings(object sender, EventArgs e)
        {
            mainWindow.Show();
            this.Close();
        }

        private void Logout_Clicked(object sender, RoutedEventArgs e)
        {
            this.Close();
            mainWindow.Logout();
        }

        private void To_themes(object sender, RoutedEventArgs e)
        {
            this.Close();
            Themes Themes_Page = new Themes();
            Themes_Page.Show();
            mainWindow.Hide();
        }
    }
}
