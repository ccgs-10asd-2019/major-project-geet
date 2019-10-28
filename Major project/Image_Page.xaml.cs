using System.Windows;
using System.Windows.Media;

namespace Major_project
{
    public partial class Image_Page : Window
    {
        public Image_Page(ImageSource Source)
        {
            InitializeComponent();
            image.Source = Source;
        }
    }
}
