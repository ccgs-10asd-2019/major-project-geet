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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Search_Users : Window
    {
        public Search_Users()
        {
            InitializeComponent();
            SetUp();
        }

        private void SetUp()
        {
            var imgBrush = new ImageBrush();
            imgBrush.ImageSource = new BitmapImage(new Uri(Properties.Settings.Default.BackgroundUrl));
            BackgroundGrid.Background = imgBrush;
        }
    }
}
