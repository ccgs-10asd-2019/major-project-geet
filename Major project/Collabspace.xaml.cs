using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
    /// Interaction logic for Collabspace.xaml
    /// </summary>
    public partial class Collabspace : Window
    {

        //setup
        //download full text
        //set textbox as user version
        //loop
        //compare user version to true version to find differences
        //send differences to server, server updates lines
        //get request the latest version



        internal BackendConnect Backend = new BackendConnect();

        public System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();

        class Collab
        {
            public int chat { get; set; }
            public int id { get; set; }
            public string text { get; set; }
            public string[] true_text { get; set; }
            public string[] edited_text { get; set; }
            public string[] new_text { get; set; }
            public string lastedited { get; set; }
        }

        Collab collab = new Collab();


        public Collabspace(int chat_id, int id)
        {
            InitializeComponent();

            collab.chat = chat_id;
            collab.id = id;
            collab.lastedited = "";
            GetCollab();

            
            dispatcherTimer.Tick += Auto_Collab;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();

        }

        private void Auto_Collab(object sender, EventArgs e)
        {
            var request = BackendConnect.server + "collab/last-edited/" + collab.chat + "/" + collab.id;
            Console.WriteLine(request);

            var response = Backend.Get(request);

            if (collab.lastedited != response[0].Collab_lastedit)
            {
                collab.lastedited = response[0].Collab_lastedit;
                CollabLoop();
            }
        }

        public async void CollabLoop()
        {

            var request = BackendConnect.server + "collab/full/" + collab.chat + "/" + collab.id;
            Console.WriteLine(request);

            //collab.new_text = StringArrayOfNewlines(Backend.Get(request));
            //collab.true_text = StringArrayOfNewlines(collab.text);
            //collab.edited_text = StringArrayOfNewlines(Collab_textbox.Text);

            BackendConnect.Post_message_class data = new BackendConnect.Post_message_class()
            {
                Chat_id = collab.chat,
                Id = collab.id,
                Collab = Collab_textbox.Text
                //Collab = collab.edited_text.Except(collab.true_text) //.ToList()
            };

            request = BackendConnect.server + "collab/full/" + collab.chat + "/" + collab.id;
            Console.WriteLine(request);
            await Task.Run(async () => await Backend.Post(data, request));
        }

        public void GetCollab()
        {
            var request = BackendConnect.server + "collab/full/" + collab.chat + "/" + collab.id;
            Console.WriteLine(request);

            var response = Backend.Get(request);

            Collab_textbox.Text = response[0].Collab;
            collab.text = response[0].Collab;
        }

        private void TextChanged(object sender, KeyEventArgs e)
        {
            CollabLoop();
        }

        private void WindowClosed(object sender, EventArgs e)
        {
            dispatcherTimer.Stop();
        }

        public string[] StringArrayOfNewlines(string text)
        {
            return text.Split(
                new[] { Environment.NewLine },
                StringSplitOptions.None
            );
        }
    }
}
