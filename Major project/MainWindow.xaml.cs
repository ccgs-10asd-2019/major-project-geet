using System;
using System.Collections.Generic;
using System.Windows;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Script.Serialization;
using System.Threading.Tasks;
using System.Text;
using System.Windows.Controls;

namespace Major_project
{
    public partial class MainWindow : Window
    {

        BackendConnect Backend = new BackendConnect();

        public MainWindow()
        {
            InitializeComponent();
            //Backend.sendmessage();
        }

        public static void addMessageToChat(String msg)
        {
            //chat.Items.Add(msg);
            Console.WriteLine(msg);
        }

        private void push_send_button(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(message_textbox.Text);
            Backend.sendmessage(message_textbox.Text);
        }
    }

    public class BackendConnect
    {
        static String ip = "127.0.0.1";
        static String port = "3000";
        static String protocol = "http";
        static String server = protocol + "://" + ip + ":" + port + "/";
        String responseString;

        static HttpClient client = new HttpClient();

        private async void get(String request)
        {
            responseString = await client.GetStringAsync(request);
            //client.AsyncWaitHandle.WaitOne();   
        }

        public String return_get(String request)
        {
            get(request);
            return responseString;
        }

        public String get_messages(int id)
        {
            String request = server + "server/messages/" + id.ToString();
            return_get(request);
            Console.WriteLine("yonk: " + responseString);
            return responseString;
        }

        public class Message
        {
            public int chat_id { get; set; }
            public int user_id { get; set; }
            public string message { get; set; } 
        }

        public async Task<string> Post(Message data, String request)
        {
            string Response = "";
            try
            {
                HttpResponseMessage HttpResponseMessage = null;
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //application/xml

                    
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    // serialize into json string
                    var myContent = jss.Serialize(data);

                    var httpContent = new StringContent(myContent, Encoding.UTF8, "application/json");

                    HttpResponseMessage = await httpClient.PostAsync(request, httpContent);

                    if (HttpResponseMessage.StatusCode == HttpStatusCode.OK)
                    {
                        Response = HttpResponseMessage.Content.ReadAsStringAsync().Result;
                        Console.WriteLine("good" + Response);
                    }
                    else
                    {
                        Response = "Some error occured." + HttpResponseMessage.StatusCode;
                        Console.WriteLine("bad" + Response);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            
            return Response;
        }

        public bool sendmessage(String message)
        {
            Message data = new Message() {
                chat_id = 1,
                user_id = 1,
                message = message
            };
            String request = server + "message/1";
            string output = Post(data, request).Result;
            return true;
        }
    }
}
