using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Example_913
{
    class Program
    {
        static void Main(string[] args)
        {

            var client = new WebClient();

            using (Stream stream = client.OpenRead("http://ksergey.ru/sb/voina-i-mir.txt"))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string line = "";
                    while ((line = reader.ReadLine()) != null)
                    {
                        Console.WriteLine(reader.ReadToEnd());
                    }
                }
            }

            Console.WriteLine("Файл загружен");
            Console.ReadLine();
        }
    }
}
