using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using System.Text.Json;

namespace Simple_Paster_KeyAuth
{
    public partial class Form1 : Form
    {
        public string default_url = "https://paster.so/api/v2/publishers/pastes/markdown/create";
        public string Token = "Secret Api Key"; // <------------ Your Paster.so TOKEN (Secret Api Key)
        public int Key_length = 35;
        public string Paster_Title = "Epic Key";
        public string Paster_Message = "Key --> ";
        public string After_Key_Paster_Message = " <-- Key";

        public string Key = String.Empty;

        public class info
        {
            public Dictionary<string, string> paste { get; set; }
        }


        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {

            Key = generate_random_letters(Key_length);

            HttpClient client = new HttpClient();
            var data = new { content = Paster_Message + Key + After_Key_Paster_Message, title = Paster_Title };
            var json_data = JsonSerializer.Serialize(data);
            var content = new StringContent(json_data, Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);


            var response = client.PostAsync(default_url, content).Result;

            if (response.IsSuccessStatusCode)
            {

                var responseContent = response.Content.ReadAsStringAsync().Result;
                var paste_data = JsonSerializer.Deserialize<info>(responseContent);

                Process.Start("explorer", paste_data.paste["url"]);
            }
            else
            {
                Console.WriteLine(response.StatusCode.ToString());
            }
        }

        public string generate_random_letters(int count)
        {
            string chars = "$%#@!*abcdefghijklmnopqrstuvwxyz1234567890?;:ABCDEFGHIJKLMNOPQRSTUVWXYZ^&";
            Random rand = new Random();
            var rngkey = string.Empty;
            for (int i = 0; i < count; i++)
            {
                int num = rand.Next(0, chars.Length);
                rngkey += chars[num];
            }

            return rngkey;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == Key)
            {
                Form2 form2 = new Form2();
                form2.Show();
                Hide();
            }
            else
            {
                textBox1.Text = "WRONG KEY :(";
            }
        }
    }
}
