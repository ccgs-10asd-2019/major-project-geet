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
        

        public MainWindow()
        {
            InitializeComponent();
            //Backend.sendmessage();
            Backend.get_messages(1);
        }

        public void addMessageToChat(String msg)
        {
            chat.Items.Add(msg);
        }

        private void push_send_button(object sender, RoutedEventArgs e)
        {
            //Console.WriteLine(message_textbox.Text);
            Backend.sendmessage(message_textbox.Text);
        }
    }

    public class BackendConnect
    {
        static String ip = "127.0.0.1";
        static String port = "3000";
        static String protocol = "http";
        static String server = protocol + "://" + ip + ":" + port + "/";

        static HttpClient httpClient = new HttpClient();
        static JavaScriptSerializer jss = new JavaScriptSerializer();

        public class send_message_class
        {
            public int chat_id { get; set; }
            public int user_id { get; set; }
            public string message { get; set; } 
        }

        public class get_messages_class
        {
            public int id { get; set; }
            public int user_id { get; set; }
            public int time_submitted { get; set; }
            public string message { get; set; }
        }

        public List<get_messages_class> Get(String request)
        {
            List<get_messages_class> content = null;

            try
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = httpClient.GetAsync(request).Result;

                if (response.IsSuccessStatusCode)   
                {
                    var json = response.Content.ReadAsStringAsync().Result;
                    content = JsonConvert.DeserializeObject<List<get_messages_class>>(json);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return content;
        }

        public async Task<string> Post(send_message_class data, String request)
        {
            string Response = "";
            try
            {
                Console.WriteLine("started try");
                HttpResponseMessage HttpResponseMessage = null;
                Console.WriteLine("set http response to null");
                
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                Console.WriteLine("set header to json");
                var myContent = jss.Serialize(data);
                Console.WriteLine("initalise jss");
                var httpContent = new StringContent(myContent, Encoding.UTF8, "application/json");
                Console.WriteLine("set httpContent to json");
                HttpResponseMessage = await httpClient.PostAsync(request, httpContent);
                Console.WriteLine("sent post request");
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
            catch (Exception ex)
            {
                Console.WriteLine("error with try");
                Console.WriteLine(ex);
            }
            Console.WriteLine("ass");
            return Response;
        }

        public bool sendmessage(String message)
        {
            send_message_class data = new send_message_class() {
                chat_id = 1,
                user_id = 1,
                message = message
            };
            String request = server + "message/1";
            string output = Post(data, request).Result;
            return true;
        }

        public void get_messages(int id)
        {
            String request = server + "messages/" + id.ToString();
            var content = Get(request);

            for (int i = 0; i < content.Count; i++)
            {
                Console.WriteLine(content[i].time_submitted + " | " + content[i].user_id + ": " + content[i].message);
            }
        }
    }
}
