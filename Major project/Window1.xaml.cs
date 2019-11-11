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

        internal BackendConnect Backend = new BackendConnect();
        public int Chat_id { get; set; }
        public Search_Users(int chatID)
        {
            InitializeComponent();
            SetUp();

            Chat_id = chatID;

        }

        private void SetUp()
        {
            Properties.Settings.Default.SearchOn = true;
            var imgBrush = new ImageBrush();
            imgBrush.ImageSource = new BitmapImage(new Uri(Properties.Settings.Default.BackgroundUrl));
            BackgroundGrid.Background = imgBrush;
        }

        private void TextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox txtBox = sender as TextBox;
            if (txtBox.Text == "Search for people here...")
                txtBox.Text = string.Empty;
        }

        private void keyDown(object sender, KeyEventArgs e)
        {
            TextBox txtBox = sender as TextBox;

            searchUser(txtBox.Text);
        }

        private void keyUp(object sender, KeyEventArgs e)
        {
            TextBox txtBox = sender as TextBox;

            searchUser(txtBox.Text);
        }

        private void searchUser(string user)
        {
            Console.WriteLine(user);

            var request = BackendConnect.server + "users/search/" + user.ToString();
            Console.WriteLine(request);
            var response = Backend.Get(request);

            if (response != null)
            {
                users.Items.Clear();

                for (int i = 0; i < response.Count; i++)
                {

                    var converter = new System.Windows.Media.BrushConverter();

                    Button Add_User = new Button
                    {
                        Content = response[i].Username,
                        Background = (Brush)converter.ConvertFromString("transparent"),
                        BorderBrush = (Brush)converter.ConvertFromString("transparent"),
                        Foreground = (Brush)converter.ConvertFromString("black")
                    };
                    Add_User.Click += new RoutedEventHandler(Add_User_action);
                    Add_User.Tag = response[i].Id;

                    users.Items.Add(Add_User);
                }
            }
        }

        internal async void Add_User_action(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            Console.WriteLine(Chat_id);
            Console.WriteLine(b.Tag);

            BackendConnect.Post_message_class data = new BackendConnect.Post_message_class()
            {
                Chat_id = Chat_id,
                User_id = Int32.Parse(b.Tag.ToString()),
            };

            String request = BackendConnect.server + "chat/user/add";

            await Task.Run(async () => await Backend.Post(data, request));

            ///Chat_ListBox.Items.Clear();
            ///current_User.Lastest_message = 0;
            ///GetMessages();
        }
    }

}
