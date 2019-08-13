using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Major_project
{
    public partial class MainWindow : Window
    {
        BackendConnect Backend = new BackendConnect();
        Tools Tools = new Tools();

        Current_User current_User = new Current_User()
        {
            Chat_id = 0,
            User_id = Properties.Settings.Default.id,
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
            }
            else
            {
                GetChats(current_User.User_id);
            }
        }

        public void LoggedIn()
        {
            current_User.User_id = Properties.Settings.Default.id;
            GetChats(current_User.User_id);
        }


        public class Current_User
        {
            public int Chat_id { get; set; }
            public int User_id { get; set; }
            public int Lastest_message { get; set; }
        }

        public void GetChats(int id)
        {
        string request = BackendConnect.server + "chats/" + id.ToString();
        var content = Backend.Get(request);
            for (int i = 0; i < content.Count; i++)
            {
                request = BackendConnect.server + "info/name/" + content[i].Chat.ToString();
                var ListChats = Backend.Get(request);

                Button b = new Button();
                b.Content = ListChats[0].Name;
                b.Click += new RoutedEventHandler(Chats_Click);
                b.Tag = content[i].Chat.ToString();
                Server_list.Items.Add(b);
            }
        }

        void Chats_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            chat.Items.Clear();
            current_User.Chat_id = Int32.Parse(b.Tag.ToString());
            current_User.Lastest_message = 0;
            GetMessages(Int32.Parse(b.Tag.ToString()));

            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += Auto_GetMessages;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();

        }

        private void Auto_GetMessages(object sender, EventArgs e)
        {
            GetMessages(current_User.Chat_id);
        }

        public void GetMessages(int id)
        {
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
                        chat.Items.Add(Tools.ConvertFromUnixTimestamp(content[i].Time_submitted) + " | " + Users_Name + ": " + content[i].Message);
                    }
                }
            }
        }

        

        private void Button_SendMessage(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }

        public async void SendMessage()
        {
            DateTime time = DateTime.UtcNow;
            BackendConnect.Send_message_class data = new BackendConnect.Send_message_class()
            {
                Chat_id = current_User.Chat_id,
                User_id = current_User.User_id,
                Message = message_textbox.Text,
                Current_time = ((DateTimeOffset)time).ToUnixTimeSeconds()
            };
            String request = BackendConnect.server + "message";
            var post = await Backend.Post(data, request);
        }

        private void Logout_clicked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.id = 0;
            Properties.Settings.Default.Save();
            current_User.User_id = 0;
            Server_list.Items.Clear();
            LogIn();
        }

        private void Enter_clicked(object sender, System.Windows.Input.KeyEventArgs e)
        {
            {
                if (e.Key == Key.Return)
                {
                    SendMessage();
                    e.Handled = true;
                }
            }
        }
    }
}
