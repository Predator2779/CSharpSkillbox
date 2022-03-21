using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace Example_922
{
    class Program
    {
        static void Main(string[] args)
        {
            // https://core.telegram.org/bots/api

            // https://telegram.org/
            // https://core.telegram.org/api
            // https://core.telegram.org/bots
            // https://core.telegram.org/bots/samples How do I create a bot?
            // https://core.telegram.org/bots/api How do bots work?
            // string token =  "4159465546AAEG3gCROdn7xLKAEpsTeRucVnMBEm12WcT";

            Thread task = new Thread(BackgrundBot);
            task.Start();

            for (int i = 0; i < 500; i++)
            {
                Console.Write("+ ");
                Thread.Sleep(50);
            }
        }

        static void BackgrundBot()
        {
            string token = File.ReadAllText(@"D:\ Work\SkillBox\token");

            WebClient wc = new WebClient() { Encoding = Encoding.UTF8 };

            int update_id = 0;
            string startUrl = $@"https://api.telegram.org/bot{token}/";

            while (true)
            {
                string url = $"{startUrl}getUpdates?offset={update_id}";
                var r = wc.DownloadString(url);

                var msgs = JObject.Parse(r)["result"].ToArray();

                foreach (dynamic msg in msgs)
                {
                    update_id = Convert.ToInt32(msg.update_id) + 1;

                    string userMessage = msg.message.text;
                    string userId = msg.message.from.id;
                    string useFirstrName = msg.message.from.first_name;

                    string text = $"{useFirstrName} {userId} {userMessage}";

                    Console.WriteLine(text);

                    if (userMessage == "hi")
                    {
                        string responseText = $"Здравствуйте, {useFirstrName}";
                        url = $"{startUrl}sendMessage?chat_id={userId}&text={responseText}";
                        Console.WriteLine("+");
                        wc.DownloadString(url);
                    }
                }
                Thread.Sleep(10);
            }
        }       
    }
}
