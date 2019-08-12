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



        public void Change_colour(string Scheme1, string Scheme2)
        {
            var converter = new BrushConverter();
            var brush1 = (Brush)converter.ConvertFromString(Scheme1);
            var brush2 = (Brush)converter.ConvertFromString(Scheme2);
            Rectangle1.Fill = brush1;
            header_block.Fill = brush2;
        }

        private void Background1_clicked(object sender, RoutedEventArgs e)
        {
            string Background = "images/background.png";
            Change_background(Background);
        }

        public void Change_background(string Background)
        {
            var converter1 = new BrushConverter();
            var Background_set = (ImageBrush)converter1.ConvertFromString(Background);
            //ImageBrush ImageSource = Background_set;
            Grid1.Background = Background_set;
        }
    }
}
