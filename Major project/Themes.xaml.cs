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
        }

        private void Colour1_click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Colour2 = "#FF9900";
            Properties.Settings.Default.Colour2 = "#3DD3E9";
        }

        private void Colour2_click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Colour2 = "#00270d";
            Properties.Settings.Default.Colour2 = "#00ff04";
        }

        private void Colour3_click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Colour1 = "#00060f";
            Properties.Settings.Default.Colour2 = "#6edeff";
        }

        private void Background1_clicked(object sender, RoutedEventArgs e)
        {
            this.Background = new ImageBrush(new BitmapImage(new Uri(@"C:\User Program Files\ccgs-10asd-2019\major-project-geet\Major project\images\blue.jpg")));
            Properties.Settings.Default.BackgroundUrl = (@"C:\User Program Files\ccgs-10asd-2019\major-project-geet\Major project\images\blue.jpg");
            Properties.Settings.Default.Save();
        }

        private void Background2_clicked(object sender, RoutedEventArgs e)
        {
            this.Background = new ImageBrush(new BitmapImage(new Uri(@"C:\User Program Files\ccgs-10asd-2019\major-project-geet\Major project\images\Bluebackground1.jpg")));
            Properties.Settings.Default.BackgroundUrl = (@"C:\User Program Files\ccgs-10asd-2019\major-project-geet\Major project\images\Bluebackground1.jpg");
            Properties.Settings.Default.Save();
        }

        private void Background3_clicked(object sender, RoutedEventArgs e)
        {
            this.Background = new ImageBrush(new BitmapImage(new Uri(@"C:\User Program Files\ccgs-10asd-2019\major-project-geet\Major project\images\orange.jpg")));
            Properties.Settings.Default.BackgroundUrl = (@"C:\User Program Files\ccgs-10asd-2019\major-project-geet\Major project\images\orange.jpg");
            Properties.Settings.Default.Save();
            //(Application.Current.MainWindow as MainWindow).Change_colours();
        }

        public void Change_colour()
        {
            var converter = new BrushConverter();
            var brush1 = (Brush)converter.ConvertFromString(Properties.Settings.Default.Colour1);
            var brush2 = (Brush)converter.ConvertFromString(Properties.Settings.Default.Colour2);
            Rectangle1.Fill = brush1;
            header_block.Fill = brush2;
    
        }

        private void Exit_themes(object sender, RoutedEventArgs e)
        {
            this.Close();
            MainWindow mainwindow = new MainWindow();
            mainwindow.Show();
        }
    }
}