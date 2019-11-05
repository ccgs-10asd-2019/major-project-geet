using System;
using System.IO;
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
    /// Interaction logic for Themes.xaml
    /// </summary>
    public partial class Themes : Window
    {
        public Themes()
        {
            InitializeComponent();
            Change_theme_page();
        }

        private void BlueRed_click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Colour1 = "#813A47";
            Properties.Settings.Default.Colour2 = "#172E64";
            Properties.Settings.Default.BackgroundUrl = ("pack://application:,,,/Major project;component/images/blue.jpg");
            Properties.Settings.Default.Save();
            Change_theme_page();
        }

        private void DarkScheme_click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Colour1 = "#1c1d1e";
            Properties.Settings.Default.Colour2 = "#424445";
            Properties.Settings.Default.BackgroundUrl = ("pack://application:,,,/Major project;component/images/black.jpg");
            Properties.Settings.Default.Save();
            Change_theme_page();
        }

        private void Colour3_click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Colour1 = "#00060f";
            Properties.Settings.Default.Colour2 = "#6edeff";
            Properties.Settings.Default.BackgroundUrl = ("pack://application:,,,/Major project;component/images/orange.jpg");
            Properties.Settings.Default.Save();
            Change_theme_page();
        }

        private void Text_colour1(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.TextColour = ("#ffffff");
            Properties.Settings.Default.Save();
            Change_theme_page();
        }

        private void Text_Colour2(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.TextColour = ("#000000");
            Properties.Settings.Default.Save();
            Change_theme_page();
        }

        private void Text_colour3(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.TextColour = ("#ffffff");
            Properties.Settings.Default.Save();
            Change_theme_page();
        }

        public void Change_theme_page()
        {
            this.Background = new ImageBrush(new BitmapImage(new Uri(Properties.Settings.Default.BackgroundUrl)));
            var converter = new BrushConverter();
            var brush1 = (Brush)converter.ConvertFromString(Properties.Settings.Default.Colour1);
            var brush2 = (Brush)converter.ConvertFromString(Properties.Settings.Default.Colour2);
            var TextColourBrush = (Brush)converter.ConvertFromString(Properties.Settings.Default.TextColour);
            header_block.Fill = brush2;
            Themes_title.Foreground = TextColourBrush;
            Properties.Settings.Default.Save();

        }

        private void Exit_themes(object sender, RoutedEventArgs e)
        {
            this.Close();
            MainWindow mainwindow = new MainWindow();
            mainwindow.Show();
        }
    }
}