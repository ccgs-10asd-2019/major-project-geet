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
            var Scheme1 = "#FF9900";
            var Scheme2 = "#3DD3E9";
            Change_colour(Scheme1, Scheme2);
        }

        private void Colour2_click(object sender, RoutedEventArgs e)
        {
            var Scheme1 = "#00270d";
            var Scheme2 = "#00ff04";
            Change_colour(Scheme1, Scheme2);
        }

        private void Colour3_click(object sender, RoutedEventArgs e)
        {
            var Scheme1 = "#00060f";
            var Scheme2 = "#6edeff";
            Change_colour(Scheme1, Scheme2);
        }

        private void Background1_clicked(object sender, RoutedEventArgs e)
        {
            this.Background = new ImageBrush(new BitmapImage(new Uri(@"C:\User Program Files\ccgs-10asd-2019\major-project-geet\Major project\images\blue.jpg")));
        }

        private void Background2_clicked(object sender, RoutedEventArgs e)
        {
            this.Background = new ImageBrush(new BitmapImage(new Uri(@"C:\User Program Files\ccgs-10asd-2019\major-project-geet\Major project\images\Bluebackground1.jpg")));
        }

        private void Background3_clicked(object sender, RoutedEventArgs e)
        {
            this.Background = new ImageBrush(new BitmapImage(new Uri(@"C:\User Program Files\ccgs-10asd-2019\major-project-geet\Major project\images\orange.jpg")));
        }

        public void Change_colour(string Scheme1, string Scheme2)
        {
            var converter = new BrushConverter();
            var brush1 = (Brush)converter.ConvertFromString(Scheme1);
            var brush2 = (Brush)converter.ConvertFromString(Scheme2);
            Rectangle1.Fill = brush1;
            header_block.Fill = brush2;
        }
    }
}