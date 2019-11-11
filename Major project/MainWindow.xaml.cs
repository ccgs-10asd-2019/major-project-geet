namespace Major_project

{
    using Microsoft.Win32;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// Defines the <see cref="MainWindow" />
    /// </summary>
    public partial class MainWindow : Window
    {
        //Mr. Nolan adding a comment to demo something hi guys

        /// <summary>
        /// Defines the Backend
        /// </summary>
        internal BackendConnect Backend = new BackendConnect();

        /// <summary>
        /// Defines the Tools
        /// </summary>
        internal Tools Tools = new Tools();

        /// <summary>
        /// Defines the current_User
        /// </summary>
        internal Current_User current_User = new Current_User()
        {
            Chat_id = 0,
            User_id = Properties.Settings.Default.id,
            Lastest_message = 0,
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            LogIn();
        }

        /// <summary>
        /// The LogIn
        /// </summary>
        public void LogIn()
        {
            if (Properties.Settings.Default.id == 0)
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

        /// <summary>
        /// The LoggedIn
        /// </summary>
        public void LoggedIn()
        {
            current_User.User_id = Properties.Settings.Default.id;
            Properties.Settings.Default.SearchOn = false;
            Properties.Settings.Default.NameChatOn = false;

            var request = BackendConnect.server + "user/" + current_User.User_id.ToString();
            var User = Backend.Get(request);

            if (User != null)
            {
                Username_TextBlock.Text = User[0].Username;

                GetChats(current_User.User_id);

                Change_themes();
            } else
            {
                Logout();
            };

            
        }

        /// <summary>
        /// The Logout
        /// </summary>
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

        /// <summary>
        /// Defines the <see cref="Current_User" />
        /// </summary>
        public class Current_User
        {
            /// <summary>
            /// Gets or sets the Chat_id
            /// </summary>
            public int Chat_id { get; set; }

            /// <summary>
            /// Gets or sets the User_id
            /// </summary>
            public int User_id { get; set; }

            /// <summary>
            /// Gets or sets the Lastest_message
            /// </summary>
            public long Lastest_message { get; set; }
        }

        /// <summary>
        /// The GetChats
        /// </summary>
        /// <param name="id">The id<see cref="int"/></param>
        public void GetChats(int id)
        {
            Chats_ListBox.Items.Clear();
            string request = BackendConnect.server + "chats/" + id.ToString();
            Console.WriteLine(request);
            var content = Backend.Get(request);
            if (content != null)
            {
                for (int i = 0; i < content.Count; i++)
                {
                    request = BackendConnect.server + "info/name/" + content[i].Chat.ToString();
                    Console.WriteLine(request);
                    var ListChats = Backend.Get(request);

                    if (ListChats != null)
                    {

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
        }

        /// <summary>
        /// The Chats_Click
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="RoutedEventArgs"/></param>
        internal void Chats_Click(object sender, RoutedEventArgs e)
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

            Scroll();
        }

        /// <summary>
        /// The Auto_GetMessages
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void Auto_GetMessages(object sender, EventArgs e)
        {
            GetMessages();
        }

        /// <summary>
        /// The GetMessages
        /// </summary>
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
                        else if(content[i].File_id != null)
                        {

                            List<string> ImageExtensions = new List<string> { ".JPG", ".JPE", ".BMP", ".GIF", ".PNG" };
                            var Source = BackendConnect.server + "file/" + current_User.Chat_id + '/' + content[i].File_id;

                            Canvas all = new Canvas
                            {
                            };

                            Button download_button = new Button
                            {
                                Content = time + " | " + Users_Name + ": " + content[i].File_name,
                                Background = (Brush)converter.ConvertFromString("transparent"),
                                BorderBrush = (Brush)converter.ConvertFromString("transparent"),
                                Foreground = (Brush)converter.ConvertFromString("white"),
                                Margin = new Thickness(20, 0, 0, 0)
                            };
                            download_button.Click += new RoutedEventHandler(Download_click);
                            download_button.Tag = content[i];



                            if (ImageExtensions.Contains(Path.GetExtension(content[i].File_name).ToUpperInvariant()))
                            {
                                all.Height = 125;
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

                                all.Children.Add(image);
                            }
                            else
                            {
                                all.Height = 25;

                            }

                            all.Children.Add(Delete_button);
                            all.Children.Add(download_button);


                            Chat_ListBox.Items.Add(all);
                        }
                        else if (content[i].Collab != null)
                        {
                            
                            Canvas all = new Canvas
                            {
                                Height = 25
                            };

                            Button Collab_button = new Button
                            {
                                Content = time + " | " + Users_Name + ": " + content[i].File_name,
                                Background = (Brush)converter.ConvertFromString("transparent"),
                                BorderBrush = (Brush)converter.ConvertFromString("transparent"),
                                Foreground = (Brush)converter.ConvertFromString("white"),
                                Margin = new Thickness(20, 0, 0, 0)
                            };
                            Collab_button.Click += new RoutedEventHandler(Collab_click);
                            Collab_button.Tag = content[i];

                            all.Children.Add(Delete_button);
                            all.Children.Add(Collab_button);

                            Chat_ListBox.Items.Add(all);
                        }
                    }

                }
            }
        }

        /// <summary>
        /// The Message_Click
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="RoutedEventArgs"/></param>
        internal async void Message_Click(object sender, RoutedEventArgs e)
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

        /// <summary>
        /// The Download_click
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="RoutedEventArgs"/></param>
        internal void Download_click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;

            BackendConnect.Get_messages_class file = (BackendConnect.Get_messages_class)b.Tag;

            SaveFileDialog dialog = new SaveFileDialog()
            {
                Filter = "All(*.*)|*",
                FileName = file.File_name
            };

            if (dialog.ShowDialog() == true)
            {
                WebClient Client = new WebClient();
                var Source = BackendConnect.server + "file/" + current_User.Chat_id + '/' + file.File_id; ;
                Client.DownloadFile(Source, dialog.FileName);
            }
        }

        internal void Collab_click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;

            BackendConnect.Get_messages_class file = (BackendConnect.Get_messages_class)b.Tag;

            Collabspace collabspace_page = new Collabspace(current_User.Chat_id, file.Id);
            collabspace_page.Show();

        }

        /// <summary>
        /// The Button_SendMessage
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="RoutedEventArgs"/></param>
        private void Button_SendMessage(object sender, RoutedEventArgs e)
        {
            //MainBackground.Background = new ImageBrush(new BitmapImage(new Uri(@"C:\User Program Files\ccgs-10asd-2019\major-project-geet\Major project\images\orange.jpg")));
            SendMessage();
        }

        /// <summary>
        /// The Enter_SendMessage
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="KeyEventArgs"/></param>
        private void Enter_SendMessage(object sender, KeyEventArgs e)
        {
            {
                if (e.Key == Key.Return)
                {
                    SendMessage();
                    e.Handled = true;
                }
            }

            Scroll();
        }

        /// <summary>
        /// The SendMessage
        /// </summary>
        public async void SendMessage()
        {
            if (current_User.Chat_id != 0)
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

            Scroll();

            };
        }

        /// <summary>
        /// The Open_Settings
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="RoutedEventArgs"/></param>
        private void Open_Settings(object sender, RoutedEventArgs e)
        {
            Settings Settings = new Settings(this);
            Settings.Show();
            this.Hide();
        }

        /// <summary>
        /// The GetUsersInAChat
        /// </summary>
        public void GetUsersInAChat()
        {
            String request = BackendConnect.server + "users/" + current_User.Chat_id.ToString();
            Console.WriteLine(request);
            var content = Backend.Get(request);

            if (content != null)
            {
                for (int i = 0; i < content.Count; i++)
                {
                    String user_request = BackendConnect.server + "user/" + content[i].User_id.ToString();
                    var ListUsers_Name = Backend.Get(user_request);
                    var Users_Name = ListUsers_Name[0].Username;
                    var Users_Role = content[i].Role;
                    Users_ListBox.Items.Add("[" + Users_Role + "] " + Users_Name);
                }
            }
            
        }

        /// <summary>
        /// The ImagePanel_Drop
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="DragEventArgs"/></param>
        private async void ImagePanel_Drop(object sender, DragEventArgs e)
        {

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                if (current_User.Chat_id != 0)
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
        public void Change_themes()
        {
            var imgBrush = new ImageBrush();
            imgBrush.ImageSource = new BitmapImage(new Uri(Properties.Settings.Default.BackgroundUrl));
            MainBackground.Background = imgBrush;
            var converter = new BrushConverter();
            var brush1 = (Brush)converter.ConvertFromString(Properties.Settings.Default.Colour1);
            var brush2 = (Brush)converter.ConvertFromString(Properties.Settings.Default.Colour2);
            var TextColour = (Brush)converter.ConvertFromString(Properties.Settings.Default.TextColour);
            Username_TextBlock.Foreground = TextColour;
            Chat_TextBlock.Foreground = TextColour;
            Chats_TextBlock.Foreground = TextColour;
            Users_TextBlock.Foreground = TextColour;
            Settings_Button.Foreground = TextColour;
            Add_Chat.Foreground = TextColour;
            Add_User.Foreground = TextColour;
            Send_Button.Foreground = TextColour;
            Message_TextBox.Background = brush1;
            Users_ListBox.Background = brush1;
            Lower_Red_Border.Background = brush1;
            Users_TextBlock.Background = brush1;
            Upper_Red_Border.Background = brush1;
            Send_Button.Background = brush2;
            Chats_ListBox.Background = brush2;
            Upper_Blue_Border.Background = brush2;
            Lower_Blue_Border.Background = brush2;
            Settings_Button.Background = brush2;
            Chats_TextBlock.Background = brush2;
        }

        private void Scroll()
        {
            Chat_ListBox.SelectedIndex = Chat_ListBox.Items.Count - 1;
            Chat_ListBox.ScrollIntoView(Chat_ListBox.SelectedItem);
        }

        private void Search_for_Users(object sender, RoutedEventArgs e)
        {
            if (current_User.Chat_id != 0)
            {
                Search_Users Search_Users1 = new Search_Users(current_User.Chat_id);
                Search_Users1.Show();
            }
        }

        private void Add_Chat_Clicked(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.NameChatOn == false)
            {
                AddChat Add_chat1 = new AddChat(this);
                Add_chat1.Show();
                //this.Hide();
            }
        }
    }
}
