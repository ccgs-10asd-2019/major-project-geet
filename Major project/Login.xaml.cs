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
using System.Windows.Media.Animation;
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
        MainWindow mainWindow;

        public bool dontclose = true;

        public Login(MainWindow window)
        {
            InitializeComponent();
            login_error.Visibility = Visibility.Hidden;
            mainWindow = window;
            MoveTo1(Background1, -1200, 625);
        }

        public static void MoveTo1(Image target, double newX, double newY)
        {
            Vector offset = VisualTreeHelper.GetOffset(target);
            var top = offset.Y;
            var left = offset.X;
            TranslateTransform trans = new TranslateTransform();
            target.RenderTransform = trans;
            DoubleAnimation anim1 = new DoubleAnimation(0, newY - top, TimeSpan.FromSeconds(80));
            DoubleAnimation anim2 = new DoubleAnimation(0, newX - left, TimeSpan.FromSeconds(80));
            trans.BeginAnimation(TranslateTransform.YProperty, anim1);
            trans.BeginAnimation(TranslateTransform.XProperty, anim2);
        }

        private void Enter_Login(object sender, KeyEventArgs e)
        {
            {
                if (e.Key == Key.Return)
                {
                    Try_Login();
                    e.Handled = true;
                }
            }
        }

        private void Click_Login(object sender, RoutedEventArgs e)
        {
            Try_Login();
        }

        private async void Try_Login()
        {
            BackendConnect.Post_message_class data = new BackendConnect.Post_message_class()
            {
                Username = text_username.Text
            };
            string request = BackendConnect.server + "auth/login";
            var content = await Backend.Post(data, request);

            //try
            //{
                var user_id = content[0].Id;
                Properties.Settings.Default.id = Int32.Parse(user_id);
                Properties.Settings.Default.Save();
                mainWindow.LoggedIn();
                mainWindow.Show();
                dontclose = false;
                this.Close();
            //}
            //catch
            //{
            //    login_error.Visibility = Visibility.Visible;
            //}
        }

        private void Closed_Login(object sender, EventArgs e)
        {
            if (dontclose)
            {
                Application.Current.Shutdown();
            }
        }
    }
}
