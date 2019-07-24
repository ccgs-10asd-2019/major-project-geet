using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Major_project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        BackendConnect Backend = new BackendConnect();

        public MainWindow()
        {
            InitializeComponent();
            Backend.getMessages(1);
            //chat.Items.Add("yonk");
        }

        public static void addMessageToChat(String msg)
        {
            chat.Items.Add(msg);
        }
    }

    public class BackendConnect
    {
        static String ip = "127.0.0.1";
        static String port = "3000";
        static String protocol = "http";
        static String server = protocol + "://" + ip + ":" + port + "/";

        static HttpClient client = new HttpClient();

        public async void getMessages(int id)
        {
            String request = server + "getmessages/" + id.ToString();
            String responseString = await client.GetStringAsync(request);
            Console.WriteLine(responseString);
            MainWindow.addMessageToChat(responseString);
        }
    }
}
