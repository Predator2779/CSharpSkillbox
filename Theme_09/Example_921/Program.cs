using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Example_921
{
    class Program
    {
        static void Main(string[] args)
        {
            // http://t.me/BotFather
            // @BotFather
            //
            // https://core.telegram.org/bots/api
            // string token =  "4159465546AAEG3gCROdn7xLKAEpsTeRucVnMBEm12WcT";
            string token = File.ReadAllText(@"D:\ Work\SkillBox\token");

            // https://telegram.org/
            // https://core.telegram.org/api
            // https://core.telegram.org/bots
            // https://core.telegram.org/bots/samples How do I create a bot?
            // https://core.telegram.org/bots/api How do bots work?
            WebClient wc = new WebClient() { Encoding = Encoding.UTF8 };

            int update_id = 0;
            string startUrl = $@"https://api.telegram.org/bot{token}/";

            while (true)
            {
                string url = $"{startUrl}getUpdates?offset={update_id}";
                var r = wc.DownloadString(url);

                //Console.WriteLine(r);
                //Console.ReadLine();

                var msgs = JObject.Parse(r)["result"].ToArray();

                foreach (dynamic msg in msgs)
                {
                    update_id = Convert.ToInt32(msg.update_id) + 1;

                    string userMessage = msg.message.text;
                    string userId = msg.message.from.id;
                    string useFirstrName = msg.message.from.first_name;
                    
                    string text = $"{useFirstrName} {userId} {userMessage}";

                    Console.WriteLine(text);

                    if( userMessage == "hi")
                    {
                        string responseText = $"Здравствуйте, {useFirstrName}";
                        url = $"{startUrl}sendMessage?chat_id={userId}&text={responseText}";
                        //Console.WriteLine("+");
                        Console.WriteLine(wc.DownloadString(url));
                    }
                }


                Thread.Sleep(100);
            }

            Console.WriteLine("++++");

        }

      
    }
}
