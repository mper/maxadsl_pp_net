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
    class WebParserFull : MaxAdsl_PP_Net.Utility.WebParser
    {
        
        public WebParserFull() : base()
        {
            WebClient = new CookieAwareWebClient();
            WebClient.Encoding = Encoding.UTF8;
            // Allow untrusted certificates
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            // Set chrome user agent
            WebClient.Headers.Add(HttpRequestHeader.UserAgent, Properties.Settings.Default.EmulateUserAgent);
        }

        public override NameValueCollection GetLoginTokens()
        {
            OnActionStart(new ActionEventArgs("GetLoginTokens", "Getting login tokens..."));
            if (AbortAction)
            {
                OnActionEnd(new ActionEventArgs("GetLoginTokens", "Aborted"));
                AbortAction = false;
                return null;
            }

            string webResponse = WebClient.DownloadString(webStartUrl);
            string login_token = Regex.Match(webResponse,
                    "(<input [^>]*name=[\"']login_token[\"'] [^>]*value=[\"'])([a-zA-Z0-9]+)([\"'])")
                    .Groups[2].Value;
            string ssoSignature = Regex.Match(webResponse,
                    "(<input [^>]*name=[\"']ssoSignature[\"'] [^>]*value=[\"'])([a-zA-Z0-9]+)([\"'])")
                    .Groups[2].Value;

            NameValueCollection webLoginTokens = new NameValueCollection(){
                {"login_token", login_token},
                {"ssoSignature", ssoSignature}
            };
            OnActionEnd(new ActionEventArgs("GetLoginTokens", "Done"));
            return webLoginTokens;
        }

        public override string LoginAndGetServiceId(NameValueCollection webLoginCredidentials)
        {
            OnActionStart(new ActionEventArgs("LoginAndGetServiceId", "Logging in and getting service id..."));
            if (AbortAction)
            {
                OnActionEnd(new ActionEventArgs("LoginAndGetServiceId", "Aborted"));
                AbortAction = false;
                return null;
            }

            byte[] loginRawResponse = WebClient.UploadValues(webLoginUrl, "POST", webLoginCredidentials);
            string webResponse = Encoding.UTF8.GetString(loginRawResponse);
            string serviceId = Regex.Match(webResponse,
                    "(<a [^>]*?href=[\"']/internet/ispis-spajanja\\?serviceid=)(\\d+)([\"'])")
                    .Groups[2].Value;
            OnActionEnd(new ActionEventArgs("LoginAndGetServiceId", "Done"));
            return serviceId;
        }

        public override TrafficInfo GetTrafficInfo(string serviceId)
        {
            OnActionStart(new ActionEventArgs("GetTrafficInfo", "Getting traffic info..."));
            if (AbortAction)
            {
                OnActionEnd(new ActionEventArgs("GetTrafficInfo", "Aborted"));
                AbortAction = false;
                return null;
            }

            string webResponse = WebClient.DownloadString(webTrafficUrl + serviceId);
            //webResponse = Encoding.GetEncoding(1250).GetEncoder().enc
            
            if (webResponse.Contains("Učitavam podatke"))
                //WaitTrafficInfoReadyWebService(serviceId, ref webResponse);
                WaitTrafficInfoReadyPage(serviceId, ref webResponse);
            OnActionEnd(new ActionEventArgs("GetTrafficInfo", "Done"));

            OnActionStart(new ActionEventArgs("GetTrafficInfo", "Parsing traffic data..."));
            string trafficData = Regex.Match(webResponse, "<table.*?>.+?</table>", RegexOptions.Singleline).Value;

            Match m = Regex.Match(trafficData,
                "(Ostvareno za.*?<td.*?>)(.*?)(</.*?<td>)(.*?)(</)(.*?)(Ukupni promet.*?<td.*?><b>)(.*?)(</)", 
                RegexOptions.Singleline);

            string downloaded = m.Groups[2].Value;
            string uploaded = m.Groups[4].Value;
            string total = m.Groups[8].Value;

            TrafficInfo retVal = new TrafficInfo
            {
                Downloaded = downloaded,
                Uploaded = uploaded,
                Total = total
            };
            OnActionEnd(new ActionEventArgs("GetTrafficInfo", "Done"));
            return retVal;
        }


        private void WaitTrafficInfoReadyPage(string serviceId, ref string webResponse)
        {
            do
            {
                if (AbortAction)
                {
                    OnActionEnd(new ActionEventArgs("WaitTrafficInfoReadyPage", "Aborted"));
                    AbortAction = false;
                    return;
                }

                OnActionEnd(new ActionEventArgs("WaitTrafficInfoReadyPage", "Not ready"));
                System.Threading.Thread.Sleep(2000);
                OnActionStart(new ActionEventArgs("WaitTrafficInfoReadyPage", "Getting traffic info..."));
                webResponse = WebClient.DownloadString(webTrafficUrl + serviceId);
            } while (webResponse.Contains("Učitavam podatke"));
        }

        // This method is not yet used...
        protected virtual void WaitTrafficInfoReadyWebService(string serviceId, ref string webResponse)
        {
            string pageId = Regex.Match(webResponse, "(<form.*?requestedPageId=)(\\d*?)([\"'].*?>)").Groups[2].Value;
            string serviceIdToken = Regex.Match(webResponse, "(_serviceIdToken.*?=.*?[\"'])([\\w-]*?)([\"'])").Groups[2].Value;
            
            
            string verifierUrl = "https://moj.hrvatskitelekom.hr/App_Modules__SnT.THTCms.CSC.Modules.Package__SnT.THTCms.CSC.Modules.Profile.MojTProfileService.asmx/VerifyService";

            NameValueCollection verifyService = new NameValueCollection()
            {
                {"requestedPageId", pageId},
                {"serviceIdToken", serviceIdToken}
            };

            WebHeaderCollection headerParams = WebClient.Headers;
            WebClient.Headers = new WebHeaderCollection();
            WebClient.Headers.Add("User-Agent", headerParams[HttpRequestHeader.UserAgent]);
            WebClient.Headers.Add("Pragma", "no-cache");
            WebClient.Headers.Add("Accept-Language", "en-US,en;q=0.5");
            WebClient.Headers.Add("Accept-Encoding", "gzip,deflate");
            //WebClient.Headers.Add("If-Modified-Since", "0");
            WebClient.Headers.Add("X-Requested-With", "XMLHttpRequest");
            WebClient.Headers.Add("Cache-Control", "no-cache");

            //WebClient.Headers.Add("Origin", "https://moj.hrvatskitelekom.hr");
            //WebClient.Headers.Add("Accept", "application/json, text/javascript, */*; q=0.01");
            //client.Headers.Add("Connection", "keep-alive");
            //client.Headers.Add("Content-Type", "application/json; charset=UTF-8");
            //client.Headers.Add("Host", "m.moj.hrvatskitelekom.hr");
            //client.Headers.Add("If-Modified-Since", "0");
            //client.Headers.Add("Origin", "https://moj.hrvatskitelekom.hr");
            //WebClient.Headers.Add("Referer", "https://moj.hrvatskitelekom.hr/internet/ispis-spajanja?serviceid=2664593");
            //client.Headers.Add("Referer", "https://moj.hrvatskitelekom.hr/internet/pregled?serviceid=2664593");

            //((CookieAwareWebClient)WebClient).RequestHeaderValues.Add("Accept", "application/json, text/javascript, */*; q=0.01");
            

            //CookieContainer cookie = client.CookieContainer;
            //client.CookieContainer = null;

            do
            {
                System.Threading.Thread.Sleep(1000);
                byte[] loginRawResponse = WebClient.UploadValues(verifierUrl, "POST", verifyService);
#if DEBUG
                // TODO: remove response decoding, for debugging purposes only
                webResponse = Encoding.UTF8.GetString(loginRawResponse);
#endif
                //if(client.ResponseHeaders["Content-Length"] == "10")
                //    break;

                File.WriteAllText("debug.txt", WebClient.ToString());
                System.Threading.Thread.Sleep(15000);
                break;
            } while (true);

            WebClient.Headers = headerParams;
            //client.CookieContainer = cookie;

            webResponse = WebClient.DownloadString(webTrafficUrl + serviceId);
        }
    }
}
