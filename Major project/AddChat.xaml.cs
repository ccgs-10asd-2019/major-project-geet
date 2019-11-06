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
    /// Interaction logic for AddChat.xaml
    /// </summary>
    public partial class AddChat : Window
    {
        public AddChat()
        {
            InitializeComponent();
            StartUpAddChat();
        }

        private void StartUpAddChat()
        {
            Properties.Settings.Default.NameChatOn = true;
            var imgBrush = new ImageBrush();
            imgBrush.ImageSource = new BitmapImage(new Uri(Properties.Settings.Default.BackgroundUrl));
            BackgroundGrid.Background = imgBrush;
        }

        private void TextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox txtBox = sender as TextBox;
            if (txtBox.Text == "Name your chat here...")
                txtBox.Text = string.Empty;
        }
    }
}
