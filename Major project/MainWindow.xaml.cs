using System;
using System.Net.Http;
using System.Windows;

namespace Major_project
{
    public partial class MainWindow : Window
    {

        BackendConnect Backend = new BackendConnect();

        public MainWindow()
        {
            InitializeComponent();
            Console.WriteLine(Backend.retrieveMessages(1));
            chat.Items.Add(Backend.retrieveMessages(1));
        }

        public static void addMessageToChat(String msg)
        {
            ///chat.Items.Add(msg);
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

        private async void getMessages(int id)
        {
            String request = server + "getmessagess/" + id.ToString();
            responseString = await client.GetStringAsync(request);
        }

        public String retrieveMessages(int id)
        {
            getMessages(1);
            Console.WriteLine("yonk: " + responseString);
            return responseString;
        }
    }
}
