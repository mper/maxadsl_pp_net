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

        string webStartUrl;
        string webLoginUrl;
        string webTrafficUrl;
        string webUsernameFieldName;
        string webPasswordFieldName;
        string serviceId;

        private WebClient client;

        private Model.UserSettingsData userSettings;

        private class TrafficInfo
        {
            public string Downloaded { get; set; }
            public string Uploaded { get; set; }
            public string Total { get; set; }
        }

        public Form1()
        {
            InitializeComponent();

            lblResponse.Text = "";
            lblSettingsResponse.Text = "";

            userSettings = Model.UserSettingsData.GetUserSettingsInstance();
            txtUsername.Text = userSettings.Username;
            //txtPassword.Text = userSettings.Password;

            webStartUrl = Properties.Settings.Default.WebMobileStartUrl;
            webLoginUrl = Properties.Settings.Default.WebLoginUrl;
            webTrafficUrl = Properties.Settings.Default.WebMobileTraficUrl;
            webUsernameFieldName = Properties.Settings.Default.WebLoginUsernameFieldName;
            webPasswordFieldName = Properties.Settings.Default.WebLoginPasswordFieldName;

#if DEBUG
            //webLoginUrl = Properties.Settings.Default.DummyWebLogin;
            //webSecuredUrl = Properties.Settings.Default.DummyWebSecured;
#endif
            // Allow untrusted certificates
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            client = new Utility.CookieAwareWebClient();
            // Set chrome user agent
            client.Headers.Add(HttpRequestHeader.UserAgent, ConfigurationManager.AppSettings["emulate_user_agent"]);
        }

        private void btnCheckTraffic_Click(object sender, EventArgs e)
        {
            NameValueCollection webLoginCredidentials = GetLoginTokens();
            webLoginCredidentials.Add(webUsernameFieldName, userSettings.Username);
            webLoginCredidentials.Add(webPasswordFieldName, userSettings.Password);

            if (serviceId == null)
                serviceId = LoginAndGetServiceId(webLoginCredidentials);

            TrafficInfo trafficInfo = GetTrafficInfo(serviceId);

            lblResponse.Text += "D:" + trafficInfo.Downloaded + Environment.NewLine;
            lblResponse.Text += "U:" + trafficInfo.Uploaded + Environment.NewLine;
            lblResponse.Text += "T:" + trafficInfo.Total + Environment.NewLine;

        }

        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            userSettings.Username = txtUsername.Text;
            userSettings.Password = txtPassword.Text;
            userSettings.StoreSettings();
            MessageBox.Show("Settings saved.");
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
            if (webResponse.Contains("UÄŤitavam podatke"))
            {
                string pageId = Regex.Match(webResponse, "(<form.*?requestedPageId=)(\\d*?)([\"'].*?>)").Groups[2].Value;
                string serviceIdToken = Regex.Match(webResponse, "(_serviceIdToken.*?=.*?[\"'])([\\w-]*?)([\"'])").Groups[2].Value;
                WaitForTraficInfoReady(pageId, serviceIdToken);
                webResponse = client.DownloadString(webTrafficUrl + serviceId);
            }

            string trafficData = Regex.Match(webResponse, "<div class=[\"']miData[\"']>.*?</div>", RegexOptions.Singleline)
                    .Value;

            Match m = Regex.Match(trafficData,
                "(<div class=[\"']miData[\"']>[^>]*<p.*?>)([0-9, BKMG]*)(<.*?/p.*?>[^>]*<p.*?>)([0-9, BKMG]*)(<.*?/p.*?>)");

            string sDownloaded = m.Groups[2].Value;
            string sUploaded = m.Groups[4].Value;

            float iDownloaded, iUploaded;
            float.TryParse(Regex.Replace(sDownloaded, "[a-z A-z]", ""), out iDownloaded);
            float.TryParse(Regex.Replace(sUploaded, "[a-z A-z]", ""), out iUploaded);


            TrafficInfo retVal = new TrafficInfo
            {
                Downloaded = sDownloaded,
                Uploaded = sUploaded,
                Total = (iDownloaded + iUploaded).ToString()
            };
            return retVal;
        }

        private void WaitForTraficInfoReady(string pageId, string serviceIdToken)
        {

            string verifierUrl = "https://m.moj.hrvatskitelekom.hr/App_Modules__SnT.THTCms.CSC.Modules.Package__SnT.THTCms.CSC.Modules.Profile.MojTProfileService.asmx/VerifyService";

            NameValueCollection verifyService = new NameValueCollection()
            {
                {"requestedPageId", pageId},
                {"serviceIdToken", serviceIdToken}
            };

            client.Headers.Add(HttpRequestHeader.Pragma, "no-cache");
            client.Headers.Add("Origin", "https://m.moj.hrvatskitelekom.hr");
            client.Headers.Add("X-Requested-With", "XMLHttpRequest");
            
            do
            {
                System.Threading.Thread.Sleep(1000);
                byte[] loginRawResponse = client.UploadValues(verifierUrl, "POST", verifyService);
#if DEBUG
                string webResponse = Encoding.UTF8.GetString(loginRawResponse);
#endif                
                //if(client.ResponseHeaders["Content-Length"] == "10")
                //    break;
                System.Threading.Thread.Sleep(15000);
                break;
            } while (true);

            client.Headers.Remove(HttpRequestHeader.Pragma);
            client.Headers.Remove("Origin");
            client.Headers.Remove("X-Requested-With");
        }

    }
}
