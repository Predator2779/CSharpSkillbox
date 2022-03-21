using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Example_941
{
    class Program
    {
        static readonly TelegramBotClient bot = new TelegramBotClient(Token());
        static readonly string darkknightID = "684165898";
        static readonly string botToken = $"bot{Token()}";
        static readonly string startUrl = $"https://api.telegram.org/{botToken}/";
        static readonly string debug_path = @"G:\Work\_VS\VS_HWork\Theme_09\Example_931\Example_941\bin\Debug\";

        static private string Token()
        {
            string token = File.ReadAllText(@"G:\Work\token.txt");
            return token;
        }

        static void Main(string[] args)
        {
            #region Hide

            // Создать бота, позволяющего принимать разные типы файлов, 
            // *Научить бота отправлять выбранный файл в ответ

            // https://data.mos.ru/
            // https://apidata.mos.ru/

            // https://vk.com/dev
            // https://vk.com/dev/manuals

            // https://dev.twitch.tv/
            // https://discordapp.com/developers/docs/intro
            // https://discordapp.com/developers/applications/
            // https://discordapp.com/verification

            // http://t.me/BotFather
            // @BotFather

            // https://core.telegram.org/bots/api

            // https://telegram.org/
            // https://core.telegram.org/api
            // https://core.telegram.org/bots
            // https://core.telegram.org/bots/samples How do I create a bot?
            // https://core.telegram.org/bots/api How do bots work? 

            #endregion

            Parallel.Invoke(() => IncomeMsgs(), () => OutgoingMsgs(darkknightID));
        }

        static void IncomeMsgs()
        {
            WebClient webClient = new WebClient() { Encoding = Encoding.UTF8 };

            int update_id = 0;

            while (true)
            {
                string url = $"{startUrl}getUpdates?offset={update_id}";

                var arrMsgs = webClient.DownloadString(url);

                var msgs = JObject.Parse(arrMsgs)["result"].ToArray();

                foreach (dynamic msg in msgs)
                {
                    update_id = Convert.ToInt32(msg.update_id) + 1;

                    string userMessage = msg.message.text;
                    string userId = msg.message.from.id;
                    string useFirstrName = msg.message.from.first_name;

                    string text = $"{useFirstrName} {userId} {userMessage}";

                    string m = Convert.ToString(msg);

                    if (m.Contains("text"))
                    {
                        Console.WriteLine("_text");
                        Console.WriteLine(Convert.ToString(msg));
                    }
                    if (m.Contains("photo"))
                    {
                        string photo_id = Convert.ToString(msg.message.photo[3].file_id);
                        string photo_unique_id = Convert.ToString(msg.message.photo[3].file_unique_id);

                        Console.WriteLine("_photo");
                        Console.WriteLine(Convert.ToString(msg));
                        //UploadImage(darkknightID, );     ////
                        DownLoadImage(photo_id, photo_unique_id);
                    }

                    Thread.Sleep(50);
                }
            }
        }

        static void OutgoingMsgs(string user_id)
        {
            WebClient webClient = new WebClient() { Encoding = Encoding.UTF8 };

            while (true)
            {
                string responseText = Console.ReadLine();
                string url = $"{startUrl}sendMessage?chat_id={user_id}&text={responseText}";
                Console.WriteLine(url);
                Console.WriteLine(webClient.DownloadString(url));

                Thread.Sleep(50);
            }
        }

        static async void UploadImage(string user_id, string photo_id)
        {
            WebClient webClient = new WebClient() { Encoding = Encoding.UTF8 };

            while (true)
            {
                string url = $"{startUrl}sendMessage?chat_id={user_id}&photo={photo_id}";
                Console.WriteLine(url);
                Console.WriteLine(webClient.DownloadString(url));

                Thread.Sleep(50);
            }
        }   ////

        static async void DownLoadImage(string photo_id, string photo_unique_id)
        {
            Console.WriteLine("start download...");

            var file = await bot.GetFileAsync(photo_id, default);

            FileStream filestream = new FileStream($"{debug_path}{photo_unique_id}.png", FileMode.Create);

            await bot.DownloadFileAsync(file.FilePath, filestream);

            filestream.Close();

            filestream.Dispose();

            Console.WriteLine("image downloaded.");
        }

        //static void ListImage(string photo_unique_id)   ////
        //{
        //    DirectoryInfo listInfo = new DirectoryInfo(debug_path + "listImgs");

        //    if (!listInfo.Exists)
        //    {
        //        ///Create > List > Add > Write

        //        StreamWriter streamWriter = new StreamWriter(Encoding En);
        //    }
        //    else
        //    {
        //        ///Read > List > Add > Write
        //    }
        //}
    }
}
