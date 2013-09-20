using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
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

        string webStartUrl = Properties.Settings.Default.WebMobileStartUrl;
        string webLoginUrl = Properties.Settings.Default.WebLoginUrl;
        string webTrafficUrl = Properties.Settings.Default.WebMobileTraficUrl;
        string webUsernameFieldName = Properties.Settings.Default.WebLoginUsernameFieldName;
        string webPasswordFieldName = Properties.Settings.Default.WebLoginPasswordFieldName;

        private WebClient client;

        private class TrafficInfo
        {
            public string Downloaded { get; set; }
            public string Uploaded { get; set; }
            public string Total { get; set; }
        }

        public Form1()
        {
            InitializeComponent();

#if DEBUG
            //webLoginUrl = Properties.Settings.Default.DummyWebLogin;
            //webSecuredUrl = Properties.Settings.Default.DummyWebSecured;
#endif
            // Allow untrusted certificates
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            client = new Utility.CookieAwareWebClient();
            // Set chrome user agent
            client.Headers.Add("User-Agent", ConfigurationManager.AppSettings["emulate_user_agent"]);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NameValueCollection webLoginCredidentials = GetLoginTokens();
            webLoginCredidentials.Add(webUsernameFieldName, txtUsername.Text);
            webLoginCredidentials.Add(webPasswordFieldName, txtPassword.Text);

            string serviceId = LoginAndGetServiceId(webLoginCredidentials);

            TrafficInfo trafficInfo = GetTrafficInfo(serviceId);

            lblResponse.Text += "D:" + trafficInfo.Downloaded;
            lblResponse.Text += "U:" + trafficInfo.Uploaded;
            lblResponse.Text += "T:" + trafficInfo.Total;

        }

        private string LoginAndGetServiceId(NameValueCollection webLoginCredidentials)
        {
            byte[] loginRawResponse = client.UploadValues(webLoginUrl, "POST", webLoginCredidentials);
            string webResponse = Encoding.UTF8.GetString(loginRawResponse);
            string serviceId = Regex.Match(webResponse,
                    "(<a [^>]*? href=[\"']/internet/pregled\\?serviceid=)(\\d+)([\"'])")
                    .Groups[2].Value;
            return serviceId;
        }

        private NameValueCollection GetLoginTokens()
        {
            string webResponse = client.DownloadString(webStartUrl);
            string login_token = Regex.Match(webResponse,
                    "(<input [^>]* name=[\"']login_token[\"'] [^>]* value=[\"'])([a-zA-Z0-9]+)([\"'])")
                    .Groups[2].Value;
            string ssoSignature = Regex.Match(webResponse,
                    "(<input [^>]* name=[\"']ssoSignature[\"'] [^>]* value=[\"'])([a-zA-Z0-9]+)([\"'])")
                    .Groups[2].Value;

            NameValueCollection webLoginTokens = new NameValueCollection(){
                {"login_token", login_token},
                {"ssoSignature", ssoSignature}
            };
            return webLoginTokens;
        }

        private TrafficInfo GetTrafficInfo(string serviceId)
        {
            string webResponse = client.DownloadString(webTrafficUrl + serviceId);
            string trafficData = Regex.Match(webResponse, "<div class=[\"']miData[\"']>.*?</div>").Value;

            Match m = Regex.Match(trafficData,
                "(<div class=[\"']miData[\"']>[^>]*<p.*?>)([0-9, BKMG]*)(<.*?/p.*?>[^>]*<p.*?>)([0-9, BKMG]*)(<.*?/p.*?>)");

            string sDownloaded = m.Groups[2].Value;
            string sUploaded = m.Groups[4].Value;

            int iDownloaded, iUploaded;
            int.TryParse(Regex.Replace(sDownloaded, "[a-z A-z]", "").Replace(',', '.'), out iDownloaded);
            int.TryParse(Regex.Replace(sUploaded, "[a-z A-z]", "").Replace(',', '.'), out iUploaded);


            return new TrafficInfo
            {
                Downloaded = sDownloaded,
                Uploaded = sUploaded,
                Total = (iDownloaded + iUploaded).ToString()
            };
        }

    }
}
