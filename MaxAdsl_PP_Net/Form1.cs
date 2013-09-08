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
using System.Text.RegularExpressions;
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
            string webStartUrl = Properties.Settings.Default.WebMobileStartUrl;
            string webLoginUrl = Properties.Settings.Default.WebLoginUrl;
            string webTrafficUrl = Properties.Settings.Default.WebMobileTraficUrl;
            string webUsernameFieldName = Properties.Settings.Default.WebLoginUsernameFieldName;
            string webPasswordFieldName = Properties.Settings.Default.WebLoginPasswordFieldName;

#if DEBUG
            //webLoginUrl = Properties.Settings.Default.DummyWebLogin;
            //webSecuredUrl = Properties.Settings.Default.DummyWebSecured;
#endif

            // Allow untrusted certificates
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            WebClient client = new Utility.CookieAwareWebClient();
            // Set chrome user agent
            client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/29.0.1547.57 Safari/537.36");

            string webResponse = client.DownloadString(webStartUrl);
            string login_token = Regex.Match(webResponse, 
                "(<input [^>]* name=[\"']login_token[\"'] [^>]* value=[\"'])([a-zA-Z0-9]+)([\"'])")
                .Groups[2].Value;
            string ssoSignature = Regex.Match(webResponse,
                "(<input [^>]* name=[\"']ssoSignature[\"'] [^>]* value=[\"'])([a-zA-Z0-9]+)([\"'])")
                .Groups[2].Value;
            //string conversationId = Regex.Match(webResponse, "(conversationId=)(\\w+)").Groups[2].Value;
            //webLoginUrl = "https://servisi.tportal.hr/servisi/anonymous.jsf2?conversationId=" + conversationId;

            NameValueCollection webLoginCredidentials = new NameValueCollection(){
                {webUsernameFieldName, txtUsername.Text}, 
                {webPasswordFieldName, txtPassword.Text},
                {"login_token", login_token},
                {"ssoSignature", ssoSignature}
                //{"login","login"},
                //{"loggedId", "false"}
            };
            byte[] loginRawResponse = client.UploadValues(webLoginUrl, "POST", webLoginCredidentials);
            webResponse = Encoding.UTF8.GetString(loginRawResponse);
            string serviceId = Regex.Match(webResponse, 
                "(<a [^>]*? href=[\"']/internet/pregled\\?serviceid=)(\\d+)([\"'])")
                .Groups[2].Value;

            webResponse = client.DownloadString(webTrafficUrl + serviceId);
            string trafficData = Regex.Match(webResponse, "<div class=[\"']miData[\"']>.*?</div>").Value;

            Match m = Regex.Match(trafficData, 
                "(<div class=[\"']miData[\"']>[^>]*<p.*?>)([0-9, BKMG]*)(<.*?/p.*?>[^>]*<p.*?>)([0-9, BKMG]*)(<.*?/p.*?>)");

            string sDownloaded = m.Groups[2].Value;
            string sUploaded = m.Groups[4].Value;

            int iDownloaded;
            int.TryParse(Regex.Replace(sDownloaded, "[a-z A-z]", "").Replace(',', '.'), out iDownloaded);
            int iUploaded;
            int.TryParse(Regex.Replace(sUploaded, "[a-z A-z]", "").Replace(',', '.'), out iUploaded);

            lblResponse.Text += "D:" + sDownloaded;
            lblResponse.Text += "U:" + sUploaded;
            lblResponse.Text += "T:" + (iDownloaded + iUploaded);

        }

    }
}
