using System;
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

            Change_themes();
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
                        Chat_ListBox.Items.Add(Tools.ConvertFromUnixTimestamp(content[i].Time_submitted) + " | " + Users_Name + ": " + content[i].Message);
                    }
                }
            }
        }

        private void Button_SendMessage(object sender, RoutedEventArgs e)
        {
            //MainBackground.Background = new ImageBrush(new BitmapImage(new Uri(@"C:\User Program Files\ccgs-10asd-2019\major-project-geet\Major project\images\orange.jpg")));
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

        public void Change_themes()
        {
            //this.Background = new ImageBrush(new BitmapImage(new Uri(Properties.Settings.Default.BackgroundUrl)));
            //this.BackgroundImageUrl = Properties.Settings.Default.BackgroundUrl;
            var imgBrush = new ImageBrush();
            imgBrush.ImageSource = new BitmapImage(new Uri(Properties.Settings.Default.BackgroundUrl, UriKind.Relative));
            Console.WriteLine(Properties.Settings.Default.BackgroundUrl);
            MainBackground.Background = imgBrush;
            var converter = new BrushConverter();
            var brush1 = (Brush)converter.ConvertFromString(Properties.Settings.Default.Colour1);
            var brush2 = (Brush)converter.ConvertFromString(Properties.Settings.Default.Colour2);

            //MainBackground.Background = new ImageBrush(new BitmapImage(new Uri(@"C:\User Program Files\ccgs-10asd-2019\major-project-geet\Major project\images\orange.jpg")));
        }
    }
}
