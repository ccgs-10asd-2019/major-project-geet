using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Major_project
{
    public partial class MainWindow : Window
    {
        BackendConnect Backend = new BackendConnect();
        Tools Tools = new Tools();

        Current_User current_User = new Current_User()
        {
            Chat_id = 0,
            User_id = 0,
            Lastest_message = 0,
        };

        public MainWindow()
        {
            InitializeComponent();

            LogIn();
            
        }

        public void LogIn()
        {
            if (current_User.User_id == 0)
            {
                Login Login_Page = new Login(this);
                Login_Page.Show();
                this.Hide();
            }
            else
            {
                LoggedIn();
            }
        }

        public void LoggedIn()
        {
            current_User.User_id = Properties.Settings.Default.id;

            var request = BackendConnect.server + "user/" + current_User.User_id.ToString();
            var Username = Backend.Get(request);

            Username_TextBlock.Text = Username[0].Username;

            GetChats(current_User.User_id);
        }

        public void Logout()
        {
            Properties.Settings.Default.id = 0;
            Properties.Settings.Default.Save();
            current_User.User_id = 0;
            Chats_ListBox.Items.Clear();
            Chat_ListBox.Items.Clear();
            Chat_TextBlock.Text = "No Chat Selected";
            Users_ListBox.Items.Clear();
            LogIn();
        }


        public class Current_User
        {
            public int Chat_id { get; set; }
            public int User_id { get; set; }
            public long Lastest_message { get; set; }
        }

        public void GetChats(int id)
        {
        string request = BackendConnect.server + "chats/" + id.ToString();
        var content = Backend.Get(request);
            if (content != null)
            {
                for (int i = 0; i < content.Count; i++)
                {
                    request = BackendConnect.server + "info/name/" + content[i].Chat.ToString();
                    var ListChats = Backend.Get(request);

                    var converter = new System.Windows.Media.BrushConverter();

                    Button b = new Button
                    {
                        Content = ListChats[0].Name,
                        Background = (Brush)converter.ConvertFromString("transparent"),
                        BorderBrush = (Brush)converter.ConvertFromString("transparent"),
                        Foreground = (Brush)converter.ConvertFromString("white")
                    };
                    b.Click += new RoutedEventHandler(Chats_Click);
                    b.Tag = content[i].Chat.ToString();
                    Chats_ListBox.Items.Add(b);
                }
            }
        }

        void Chats_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            Chat_TextBlock.Text = b.Content.ToString();
            Chat_ListBox.Items.Clear();
            current_User.Chat_id = Int32.Parse(b.Tag.ToString());
            current_User.Lastest_message = 0;
            GetMessages();

            Users_ListBox.Items.Clear();
            GetUsersInAChat();

            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += Auto_GetMessages;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();

        }

        private void Auto_GetMessages(object sender, EventArgs e)
        {
            GetMessages();
        }

        public void GetMessages()
        {
            var id = current_User.Chat_id;
            if (id != 0)
            {
                String request = BackendConnect.server + "messages/" + id.ToString() + "/since/" + current_User.Lastest_message.ToString();
                var content = Backend.Get(request);
                if (content != null)
                {
                    current_User.Lastest_message = content[content.Count - 1].Time_submitted;

                    for (int i = 0; i < content.Count; i++)
                    {
                        String Users_Name = BackendConnect.server + "user/" + content[i].User_id.ToString();
                        var ListUsers_Name = Backend.Get(Users_Name);
                        Users_Name = ListUsers_Name[0].Username;

                        var time = Tools.ConvertFromUnixTimestamp(content[i].Time_submitted);

                            var converter = new System.Windows.Media.BrushConverter();

                            Button Delete_button = new Button
                            {
                                Content = "[x]",
                                Background = (Brush)converter.ConvertFromString("transparent"),
                                BorderBrush = (Brush)converter.ConvertFromString("transparent"),
                                Foreground = (Brush)converter.ConvertFromString("white")
                            };
                            Delete_button.Click += new RoutedEventHandler(Message_Click);
                            Delete_button.Tag = content[i].Id;

                            if (content[i].Message != null)
                            {

                                TextBlock Message = new TextBlock
                                {
                                    Text = time + " | " + Users_Name + ": " + content[i].Message,
                                    Margin = new Thickness(20, 0, 0, 0)
                                };

                                Canvas all = new Canvas
                                {
                                    Height = 25
                                };

                                all.Children.Add(Delete_button);
                                all.Children.Add(Message);

                                Chat_ListBox.Items.Add(all);
                            }
                            else
                            {

                                TextBlock Message = new TextBlock
                                {
                                    Text = time + " | " + Users_Name + ": " + content[i].File_name,
                                    Margin = new Thickness(20, 0, 0, 0)
                                };

                                //Chat_ListBox.Items.Add(time + " | " + Users_Name + ": " + content[i].File_name);
                                var Source = BackendConnect.server + "file/" + current_User.Chat_id + '/' + content[i].File_id;
                                Uri uri = new Uri(Source, UriKind.Absolute);
                                ImageSource imgSource = new BitmapImage(uri);

                                Image image = new Image
                                {
                                    Source = imgSource,
                                    Height = 100,
                                    Margin = new Thickness(0, 20, 0, 0)
                                };

                                image.MouseLeftButtonUp += (s, e) =>
                                {
                                    Image image_clicked = (Image)s;
                                    Image_Page image_Page = new Image_Page(image_clicked.Source);
                                    image_Page.Show();
                                };

                                Canvas all = new Canvas
                                {
                                    Height = 125
                                };

                                all.Children.Add(Delete_button);
                                all.Children.Add(Message);
                                all.Children.Add(image);

                                Chat_ListBox.Items.Add(all);
                            }
                        
                    }
                        
                }
            }
        }

        async void Message_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;

            BackendConnect.Post_message_class data = new BackendConnect.Post_message_class()
            {
                Chat_id = current_User.Chat_id,
                User_id = current_User.User_id,
                Message_id = Int32.Parse(b.Tag.ToString()),
            };
            String request = BackendConnect.server + "delete";

            await Task.Run(async () => await Backend.Post(data, request));

            Chat_ListBox.Items.Clear();
            current_User.Lastest_message = 0;
            GetMessages();

        }

        private void Button_SendMessage(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }

        private void Enter_SendMessage(object sender, KeyEventArgs e)
        {
            {
                if (e.Key == Key.Return)
                {
                    SendMessage();
                    e.Handled = true;
                }
            }
        }

        public async void SendMessage()
        {
            DateTime time = DateTime.UtcNow;
            BackendConnect.Post_message_class data = new BackendConnect.Post_message_class()
            {
                Chat_id = current_User.Chat_id,
                User_id = current_User.User_id,
                Message = Message_TextBox.Text,
                Current_time = ((DateTimeOffset)time).ToUnixTimeSeconds()
            };
            String request = BackendConnect.server + "message";
            //var post = await Backend.Post(data, request);
            await Task.Run(async () => await Backend.Post(data, request));
            Message_TextBox.Text = String.Empty;
            GetMessages();
        }

        private void Open_Settings(object sender, RoutedEventArgs e)
        {
            Settings Settings = new Settings(this);
            Settings.Show();
            this.Hide();
        }

        public void GetUsersInAChat()
        {
            String request = BackendConnect.server + "users/" +  current_User.Chat_id.ToString();
            var content = Backend.Get(request);

            for (int i = 0; i < content.Count; i++)
            {
                String user_request = BackendConnect.server + "user/" + content[i].User_id.ToString();
                var ListUsers_Name = Backend.Get(user_request);
                var Users_Name = ListUsers_Name[0].Username;
                var Users_Role = content[i].Role;
                Users_ListBox.Items.Add("[" + Users_Role + "] " + Users_Name);
            }
        }

        private async void ImagePanel_Drop(object sender, DragEventArgs e)
        {

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {

                DateTime time = DateTime.UtcNow;
                BackendConnect.Post_message_class data = new BackendConnect.Post_message_class()
                {
                    Chat_id = current_User.Chat_id,
                    User_id = current_User.User_id,
                    Current_time = ((DateTimeOffset)time).ToUnixTimeSeconds(),
                };

                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                for (int i = 0; i < files.Length; i++)
                {
                    await Task.Run(async () => await Backend.UploadFile(data, files[i]));
                }
            }
        }
    }
}
