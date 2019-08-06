using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Major_project
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        BackendConnect Backend = new BackendConnect();

        public Login()
        {
            InitializeComponent();
        }

        public async Task<string> Auth(string username)
        {
            BackendConnect.Send_message_class data = new BackendConnect.Send_message_class()
            {
                Username = username
            };
            string request = BackendConnect.server + "auth/login";
            var content = await Backend.Post(data, request);
            return content;
        }

        private async void Send_login(object sender, RoutedEventArgs e)
        {
            var user_id = await Auth(text_username.Text);
            if(user_id != "")
            {
                Properties.Settings.Default.id = Int32.Parse(user_id);
                this.Close();
            }
        }
    }
}
