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
    /// Interaction logic for Themes.xaml
    /// </summary>
    public partial class Themes : Window
    {
        public Themes()
        {
            InitializeComponent();
            Change_theme_page();
        }

        private void Colour1_click(object sender, RoutedEventArgs e)
        {
            ColourChange("#FF9900", "#3DD3E9", 1);
        }

        private void Colour2_click(object sender, RoutedEventArgs e)
        {
            ColourChange("#00270d", "#00ff04", 2);
        }

        private void Colour3_click(object sender, RoutedEventArgs e)
        {
            ColourChange("#00060f", "#6edeff", 3);
        }

        public void ColourChange(string Colour1, string Colour2, int ColourNum)
        {
            Properties.Settings.Default.Colour1 = Colour1;
            Properties.Settings.Default.Colour2 = Colour2;
            Properties.Settings.Default.ColourNum = ColourNum;
            Change_theme_page();
        }

        private void Background1_clicked(object sender, RoutedEventArgs e)
        {
            BackgroundChange(@"C:\User Program Files\ccgs-10asd-2019\major-project-geet\Major project\images\blue.jpg", 1);
        }

        private void Background2_clicked(object sender, RoutedEventArgs e)
        {
            BackgroundChange(@"C:\User Program Files\ccgs-10asd-2019\major-project-geet\Major project\images\Bluebackground1.jpg", 2);
        }

        private void Background3_clicked(object sender, RoutedEventArgs e)
        {
            BackgroundChange(@"C:\User Program Files\ccgs-10asd-2019\major-project-geet\Major project\images\orange.jpg", 3);
        }

        private void BackgroundChange(string BackGroundUrl, int BackgroundNum)
        {
            Properties.Settings.Default.BackgroundNum = BackgroundNum;
            Properties.Settings.Default.BackgroundUrl = (BackGroundUrl);
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
            Properties.Settings.Default.TextColour = ("#ff0000");
            Properties.Settings.Default.Save();
            Change_theme_page();
        }

        public void Change_theme_page()
        {
            //if (Properties.Settings.Default.ColourNum == 1)
            //{
            //    Colour1.BorderThickness = new Thickness(4, 4, 4, 4);
            //    Colour1.BorderBrush = Brushes.Red;
            //}
            //else
            //{
              //  Colour1.BorderThickness = new Thickness(4, 4, 4, 4);
            //    Colour1.BorderBrush = Brushes.Red;
            //}

            this.Background = new ImageBrush(new BitmapImage(new Uri(Properties.Settings.Default.BackgroundUrl)));
            var converter = new BrushConverter();
            var brush1 = (Brush)converter.ConvertFromString(Properties.Settings.Default.Colour1);
            var brush2 = (Brush)converter.ConvertFromString(Properties.Settings.Default.Colour2);
            var TextColourBrush = (Brush)converter.ConvertFromString(Properties.Settings.Default.TextColour);
            Rectangle1.Fill = brush1;
            header_block.Fill = brush2;
            Themes_title.Foreground = TextColourBrush;
            Background_text.Foreground = TextColourBrush;
            Text_font_text.Foreground = TextColourBrush;
            Colour_scheme_text.Foreground = TextColourBrush;
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