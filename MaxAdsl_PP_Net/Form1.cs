using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace MaxAdsl_PP_Net
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            WebClient client = new Utility.CookieAwareWebClient();
            // Set chrome user agent
            client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/29.0.1547.57 Safari/537.36");

            // Ht web credidential fields
            //NameValueCollection webLoginCredidentials = new NameValueCollection(){
            //    {"username", ""}, 
            //    {"new-pass", ""}
            //};
            NameValueCollection webLoginCredidentials = new NameValueCollection(){
                {"username", "test"}, 
                {"password", "test1"}
            };
            client.UploadValues("http://localhost/DummyHtWeb/Account/Login", "POST", webLoginCredidentials);

            string resp = client.DownloadString("http://localhost/DummyHtWeb/Secured");
            label1.Text = resp;

            byte [] secret = { 0, 1, 1, 2, 3, 5, 8, 13, 21 };

            byte[] encryptedData = Utility.Tools.EncryptData(secret);
            PrintValues(encryptedData);
        }

        private void PrintValues(Byte[] myArr)
        {
            foreach (Byte i in myArr)
                Console.Write("\t{0}", i);
            Console.WriteLine();
        }
    }
}
