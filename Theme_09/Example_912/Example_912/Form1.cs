using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Example_912
{
    public partial class Form1 : Form
    {
        string url;
        WebClient client;
        public Form1()
        {
            InitializeComponent();
            url = "http://ksergey.ru/sb/voina-i-mir.txt";
            client = new WebClient();
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            DownloadFile();
        }

        void DownloadFile()
        {
            client.DownloadFile(url, "DownloadFile_voina-i-mir.txt");
        }

        private void btnDownloadAsync_Click(object sender, EventArgs e)
        {
            DownloadFileAsync();
        }

        async void DownloadFileAsync()
        {
            await client.DownloadFileTaskAsync(new Uri(url), "DownloadFileAsync_voina-i-mir.txt");
        }
    }
}
