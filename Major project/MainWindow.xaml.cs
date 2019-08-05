using System;
using System.Collections.Generic;
using System.Windows;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Script.Serialization;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json;
using System.Windows.Controls;

namespace Major_project
{
    public partial class MainWindow : Window
    {

        BackendConnect Backend = new BackendConnect();
        Tools Tools = new Tools();
        
        public MainWindow()
        {
            InitializeComponent();

            Current_User current_User = new Current_User()
            {
                Chat_id = 1,
                User_id = 1,
            };
            Console.WriteLine(current_User.User_id);
            GetChats(current_User.User_id);
            GetMessages(current_User.Chat_id);
            //GetChats(1);
            //GetMessages(1);
        }

        public class Current_User
        {
            public int Chat_id { get; set; }
            public int User_id { get; set; }
        }

        public void GetChats(int id)
        {
            String request = BackendConnect.server + "chats/" + id.ToString();
            var content = Backend.Get(request);
            for (int i = 0; i < content.Count; i++)
            {
                request = BackendConnect.server + "info/name/" + content[i].Chat.ToString();
                var ListChats = Backend.Get(request);
                Server_list.Items.Add(ListChats[0].Name);
            }
        }

        public void GetMessages(int id)
        {
            String request = BackendConnect.server + "messages/" + id.ToString();
            var content = Backend.Get(request);

            for (int i = 0; i < content.Count; i++)
            {
                String Users_Name = BackendConnect.server + "user/" + content[i].User_id.ToString();
                var ListUsers_Name = Backend.Get(Users_Name);
                Users_Name = ListUsers_Name[0].Username;
                chat.Items.Add(Tools.ConvertFromUnixTimestamp(content[i].Time_submitted) + " | " + Users_Name + ": " + content[i].Message);
            }
        }

        private void SendMessage(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(message_textbox.Text);
            BackendConnect.Send_message_class data = new BackendConnect.Send_message_class()
            {
                Chat_id = 1,
                User_id = 1,
                Message = message_textbox.Text
            };
            String request = BackendConnect.server + "message/1";
            Backend.Post(data, request);
        }

    }

    public class Tools
    {
        public static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return origin.AddSeconds(timestamp);
        }
    }

    public class BackendConnect
    {
        static readonly string ip = "127.0.0.1";
        static readonly string port = "3000";
        static readonly string protocol = "http";
        public static readonly string server = protocol + "://" + ip + ":" + port + "/";

        static HttpClient httpClient = new HttpClient();
        static JavaScriptSerializer jss = new JavaScriptSerializer();

        public class Send_message_class
        {
            public int Chat_id { get; set; }
            public int User_id { get; set; }
            public string Message { get; set; } 
        }

        public class Get_messages_class
        {
            public int Id { get; set; }
            public int User_id { get; set; }
            public int Time_submitted { get; set; }
            public string Message { get; set; }
            public string Username { get; set; }
            public string Chat { get; set; }
            public string Name { get; set; }
        }

        public List<Get_messages_class> Get(String request)
        {
            List<Get_messages_class> content = null;

            try
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = httpClient.GetAsync(request).Result;

                if (response.IsSuccessStatusCode)   
                {
                    var json = response.Content.ReadAsStringAsync().Result;
                    content = JsonConvert.DeserializeObject<List<Get_messages_class>>(json);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return content;
        }

        public void Post(Send_message_class data, String request)
        {
            try
            {                
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var myContent = jss.Serialize(data);
                var httpContent = new StringContent(myContent, Encoding.UTF8, "application/json");
                httpClient.PostAsync(request, httpContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
