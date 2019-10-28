using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Script.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Collections.Specialized;

namespace Major_project
{
    public class BackendConnect
    {
        static readonly string ip ="127.0.0.1"; 
        static readonly string port = "4000";
        static readonly string protocol = "http";
        public static readonly string server = protocol + "://" + ip + ":" + port + "/";

        static HttpClient httpClient = new HttpClient();
        static readonly JavaScriptSerializer jss = new JavaScriptSerializer();

        public class Post_check
        {
            public string Task { get; set; }
            public string Content { get; set; }
        }

        public class Post_return
        {
            public string Id { get; set; }
        }

        public class Post_message_class
        {
            public int Id { get; set; }
            public int Chat_id { get; set; }
            public int User_id { get; set; }
            public int Message_id { get; set; }
            public string Message { get; set; } 
            public long Current_time { get; set; }
            public string Username { get; set; }
            public string File_id { get; set; }
            public string File_name { get; set; }
            public string Collab { get; set; }
            //public List<string> Collab { get; set; }
            //public IEnumerable<string> Collab { get; set; }
        }

        public class Get_messages_class
        {
            public int Id { get; set; }
            public int User_id { get; set; }
            public long Time_submitted { get; set; }
            public long Time_joined { get; set; }
            public string Role { get; set; }
            public string Message { get; set; }
            public string Username { get; set; }
            public string Chat { get; set; }
            public string Name { get; set; }
            public string File_id { get; set; }
            public string File_name { get; set; }
            public bool Pong { get; set; }
            public string Collab_lastedit { get; set; }
            public string Collab { get; set; }
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
                    if (json != "[]")
                    {
                        content = JsonConvert.DeserializeObject<List<Get_messages_class>>(json);
                    } else
                    {
                        content = null;
                    }
                            
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return content;
        }

        public async Task<List<Post_return>> Post(Post_message_class data, String request)
        {
            var response = await httpClient.PostAsJsonAsync(request, data);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();

            var check = JsonConvert.DeserializeObject<List<Post_check>>(json);
            List<Post_return> content;

            if (check[0].Task == "false")
            {
                Console.WriteLine(check[0].Content);
                content = JsonConvert.DeserializeObject<List<Post_return>>("[]");
            }
            else
            {
                content = JsonConvert.DeserializeObject<List<Post_return>>(check[0].Content);
            }
            return content;
        }

        async public Task<bool> UploadFile(Post_message_class data, string FilePath)
        {

            var filetype = MimeTypes.GetMimeType(Path.GetFileName(FilePath));
            byte[] FileData = System.IO.File.ReadAllBytes(FilePath);

            var requestContent = new MultipartFormDataContent();

            var FileContent = new ByteArrayContent(FileData);
            FileContent.Headers.ContentType =
                MediaTypeHeaderValue.Parse(filetype);

            requestContent.Add(FileContent, "file", Path.GetFileName(FilePath));

            var response = await httpClient.PostAsync(server + "upload", requestContent);

            try {
                response.EnsureSuccessStatusCode();
            } catch {
                Console.WriteLine("file to big");
                return false;
            }

            data.File_id = await response.Content.ReadAsStringAsync();
            data.File_name = Path.GetFileName(FilePath);

            if (data.File_id == "fail")
            {
                Console.WriteLine("server error");
                return false;
            }

            await Post(data, server + "addfiletochat");

            return true;
        }
    }
}
