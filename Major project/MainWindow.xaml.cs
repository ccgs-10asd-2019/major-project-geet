using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Major_project
{
    public partial class MainWindow : Window
    {

        BackendConnect Backend = new BackendConnect();

        public MainWindow()
        {
            InitializeComponent();
        }

        public static void addMessageToChat(String msg)
        {
            //chat.Items.Add(msg);
            Console.WriteLine(msg);
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

        private async void post(String request, Dictionary<string, string> post_data)
        {
            //var values = new Dictionary<string, string>
            //{
            //   { "thing1", "hello" },
            //   { "thing2", "world" }
            //};

            var content = new FormUrlEncodedContent(post_data);
            var response = await client.PostAsync(request, content);
            var responseString = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseString);
        }

        public String return_get(String request)
        {
            get(request);
            return responseString;
        }

        public String return_post(String request, Dictionary<string, string> post_data)
        {
            post(request, post_data);
            return responseString;
        }

        public void send_message()
        {
            String request = server + "message/1";

            var post_data = new Dictionary<string, string>
            {
               { "thing1", "hello" },
               { "thing2", "world" }
            };

            post(request, post_data);
        }

        public String get_messages(int id)
        {
            String request = server + "server/messages/" + id.ToString();
            return_get(request);
            Console.WriteLine("yonk: " + responseString);
            return responseString;
        }
    }
}
