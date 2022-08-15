using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Telegram.Bot;

namespace Example_941
{
    public class Program
    {
        static readonly string darkknightID = "684165898";
        static readonly string userName = Environment.UserName;
        static readonly string root_path = @"C:\Users\" + userName + @"\Documents\MonkeyBot\";
        static readonly string token_path = root_path + "token.txt";
        static readonly string logs_path = root_path + "Logs" + @"\";
        static readonly string images_path = root_path + "Images" + @"\";
        static readonly string startUrl = $"https://api.telegram.org/bot{Token()}/";
        static readonly TelegramBotClient bot = new TelegramBotClient(Token());

        static private string Token()
        {
            return File.ReadAllText(token_path);
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
                        DownLoadImage(photo_id, photo_unique_id);
                        UploadImage(darkknightID);
                    }

                    //Thread.Sleep(50);
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

        static void UploadImage(string user_id)
        {
            Random random = new Random();

            Console.WriteLine("Start upload...");

            List<string> listImgs = OpenImageList();

            string photo_id = listImgs[random.Next(0, listImgs.Count)];

            bot.SendPhotoAsync(user_id, GetPhotoID(photo_id));

            Console.WriteLine("Image uploaded.");
        }

        static async void DownLoadImage(string photo_id, string photo_unique_id)
        {
            FileInfo imageInfo = new FileInfo($"{images_path}{photo_unique_id}.png");

            if (!imageInfo.Exists)
            {
                Console.WriteLine("Start download...");

                using (FileStream fileStream = new FileStream($"{images_path}{photo_unique_id}.png", FileMode.Create))
                {
                    var file = await bot.GetFileAsync(photo_id, default);

                    await bot.DownloadFileAsync(file.FilePath, fileStream);
                }

                AddPhotoToList(photo_id, photo_unique_id);

                Console.WriteLine("Image downloaded.");
            }
            else
            {
                Console.WriteLine("Image exists.");
            }
        }

        static void AddPhotoToList(string photo_id, string photo_unique_id)
        {
            FileInfo listInfo = new FileInfo(logs_path + "listImgs.txt");

            if (!listInfo.Exists)
            {
                CreateImageList();
            }

            List<string> listImgs = OpenImageList();

            listImgs.Add($"{photo_id}#{photo_unique_id}");

            using (StreamWriter streamWriter = new StreamWriter(logs_path + "listImgs.txt"))
            {
                foreach (var item in listImgs)
                {
                    streamWriter.WriteLine(item);
                }
            }
        }

        static void CreateImageList()
        {
            using (StreamWriter streamWriter = new StreamWriter(File.Create(logs_path + "listImgs.txt")))
            {

            }
        }

        static List<string> OpenImageList()
        {
            string line;

            List<string> listImgs = new List<string>();

            ///Read > List > Add

            using (StreamReader streamReader = new StreamReader(logs_path + "listImgs.txt"))
            {
                while ((line = streamReader.ReadLine()) != null)
                {
                    listImgs.Add(line);
                }
            }

            return listImgs;
        }

        static string GetPhotoID(string photoName)
        {
            string id = "";

            foreach (char item in photoName)
            {
                if (item == '#')
                    break;

                id += item;
            }

            return id;
        }
    }
}
