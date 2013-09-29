using MaxAdsl_PP_Net.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace MaxAdsl_PP_Net.Utility
{
    class WebParser
    {

        private string webStartUrl = Properties.Settings.Default.WebStartUrl;
        private string webLoginUrl = Properties.Settings.Default.WebLoginUrl;
        private string webTrafficUrl = Properties.Settings.Default.WebTrafficUrl;
        
        public WebClient WebClient { get; set; }

        public WebParser()
        {
            WebClient = new CookieAwareWebClient();
            // Allow untrusted certificates
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            // Set chrome user agent
            WebClient.Headers.Add(HttpRequestHeader.UserAgent, Properties.Settings.Default.EmulateUserAgent);
        }

        public string LoginAndGetServiceId(NameValueCollection webLoginCredidentials)
        {
            byte[] loginRawResponse = WebClient.UploadValues(webLoginUrl, "POST", webLoginCredidentials);
            string webResponse = Encoding.UTF8.GetString(loginRawResponse);
            string serviceId = Regex.Match(webResponse,
                    "(<a [^>]*? href=[\"']/internet/ispis-spajanja\\?serviceid=)(\\d+)([\"'])")
                    .Groups[2].Value;
            return serviceId;
        }

        public NameValueCollection GetLoginTokens()
        {
            string webResponse = WebClient.DownloadString(webStartUrl);
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

        public TrafficInfo GetTrafficInfo(string serviceId)
        {
            string webResponse = WebClient.DownloadString(webTrafficUrl + serviceId); 
            WaitTrafficInfoReady(serviceId, ref webResponse);

            string trafficData = Regex.Match(webResponse, "<table.*?>.+?</table>", RegexOptions.Singleline).Value;

            Match m = Regex.Match(trafficData,
                "(Ukupni promet.*?<td.*?><b>)(.*?)(</)", RegexOptions.Singleline);

            //string downloaded = m.Groups[2].Value;
            //string uploaded = m.Groups[4].Value;
            string total = m.Groups[2].Value;

            TrafficInfo retVal = new TrafficInfo
            {
                //Downloaded = downloaded,
                //Uploaded = uploaded,
                Total = total
            };
            return retVal;
        }

        protected void WaitTrafficInfoReady(string serviceId, ref string webResponse)
        {
            if (webResponse.Contains("UÄŤitavam podatke"))
            {
                string pageId = Regex.Match(webResponse, "(<form.*?requestedPageId=)(\\d*?)([\"'].*?>)").Groups[2].Value;
                string serviceIdToken = Regex.Match(webResponse, "(_serviceIdToken.*?=.*?[\"'])([\\w-]*?)([\"'])").Groups[2].Value;
                CheckTrafficInfoReady(pageId, serviceIdToken);
                webResponse = WebClient.DownloadString(webTrafficUrl + serviceId);
            }
        }

        protected void CheckTrafficInfoReady(string pageId, string serviceIdToken)
        {

            string verifierUrl = "https://moj.hrvatskitelekom.hr/App_Modules__SnT.THTCms.CSC.Modules.Package__SnT.THTCms.CSC.Modules.Profile.MojTProfileService.asmx/VerifyService";

            NameValueCollection verifyService = new NameValueCollection()
            {
                {"requestedPageId", pageId},
                {"serviceIdToken", serviceIdToken}
            };

            WebHeaderCollection headerParams = WebClient.Headers;
            WebClient.Headers = new WebHeaderCollection();
            WebClient.Headers.Add(HttpRequestHeader.Pragma, "no-cache");
            WebClient.Headers.Add("Origin", "https://moj.hrvatskitelekom.hr");
            WebClient.Headers.Add("X-Requested-With", "XMLHttpRequest");
            WebClient.Headers.Add("Accept", "application/json, text/javascript, */*; q=0.01");
            WebClient.Headers.Add("Accept-Encoding", "gzip,deflate,sdch");
            WebClient.Headers.Add("Accept-Language", "v");
            WebClient.Headers.Add("Cache-Control", "max-age=0");
            //client.Headers.Add("Connection", "keep-alive");
            //client.Headers.Add("Content-Type", "application/json; charset=UTF-8");
            //client.Headers.Add("Host", "m.moj.hrvatskitelekom.hr");
            //client.Headers.Add("If-Modified-Since", "0");
            //client.Headers.Add("Origin", "https://moj.hrvatskitelekom.hr");
            //client.Headers.Add("Referer", "https://moj.hrvatskitelekom.hr/internet/pregled?serviceid=2664593");
            WebClient.Headers.Add("X-Requested-With", "XMLHttpRequest");


            //CookieContainer cookie = client.CookieContainer;
            //client.CookieContainer = null;

            do
            {
                System.Threading.Thread.Sleep(1000);
                byte[] loginRawResponse = WebClient.UploadValues(verifierUrl, "POST", verifyService);
#if DEBUG
                string webResponse = Encoding.UTF8.GetString(loginRawResponse);
#endif
                //if(client.ResponseHeaders["Content-Length"] == "10")
                //    break;

                File.WriteAllText("debug.txt", WebClient.ToString());
                System.Threading.Thread.Sleep(15000);
                break;
            } while (true);

            WebClient.Headers = headerParams;
            //client.CookieContainer = cookie;
        }
    }
}
