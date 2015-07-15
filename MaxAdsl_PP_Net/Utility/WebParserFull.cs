using MaxAdsl_PP_Net.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace MaxAdsl_PP_Net.Utility
{
    class WebParserFull : MaxAdsl_PP_Net.Utility.WebParser
    {

        private int checkTrafficInfoCount;
        string trafficReadyUrl = MaxAdsl_PP_Net.Properties.Settings.Default.WebTrafficReadyUrl;
        
        public WebParserFull() : base()
        {
        }

        public override NameValueCollection GetLoginTokensStep()
        {
            ActionStartEvent(new ActionEventArgs("GetLoginTokens", "Getting login tokens..."));
            if (AbortAction)
            {
                ActionEndEvent(new ActionEventArgs("GetLoginTokens", "Aborted"));
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
            ActionEndEvent(new ActionEventArgs("GetLoginTokens", "Done"));
            return webLoginTokens;
        }

        public override string LoginAndGetServiceIdStep(NameValueCollection webLoginCredentials)
        {
            ActionStartEvent(new ActionEventArgs("LoginAndGetServiceId", "Logging in and getting service id..."));
            if (AbortAction)
            {
                ActionEndEvent(new ActionEventArgs("LoginAndGetServiceId", "Aborted"));
                AbortAction = false;
                return null;
            }

            byte[] loginRawResponse = WebClient.UploadValues(webLoginUrl, "POST", webLoginCredentials);
            string webResponse = Encoding.UTF8.GetString(loginRawResponse);
            string serviceId = Regex.Match(webResponse,
                    "(<a [^>]*?href=[\"']/internet/ispis-spajanja\\?serviceid=)(\\d+)([\"'])")
                    .Groups[2].Value;
            ActionEndEvent(new ActionEventArgs("LoginAndGetServiceId", "Done"));
            return serviceId;
        }

        public override TrafficInfo GetTrafficInfoStep(string serviceId)
        {
            ActionStartEvent(new ActionEventArgs("GetTrafficInfo", "Getting traffic info..."));
            if (AbortAction)
            {
                ActionEndEvent(new ActionEventArgs("GetTrafficInfo", "Aborted"));
                AbortAction = false;
                return null;
            }

            checkTrafficInfoCount = 1;
            string webResponse = WebClient.DownloadString(webTrafficUrl + serviceId);
            //webResponse = Encoding.GetEncoding(1250).GetEncoder().enc
            
            if (webResponse.Contains("Učitavam podatke"))
                //WaitTrafficInfoReadyWebService(serviceId, ref webResponse);
                WaitTrafficInfoReadyWebService(serviceId, ref webResponse);

            webResponse = WebClient.DownloadString(webTrafficUrl + serviceId);

            ActionEndEvent(new ActionEventArgs("GetTrafficInfo", "Done"));

            ActionStartEvent(new ActionEventArgs("GetTrafficInfo", "Parsing traffic data..."));
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
            ActionEndEvent(new ActionEventArgs("GetTrafficInfo", "Done"));
            return retVal;
        }


        private void WaitTrafficInfoReadyPage(string serviceId, ref string webResponse)
        {
            do
            {
                if (AbortAction)
                {
                    ActionEndEvent(new ActionEventArgs("WaitTrafficInfoReadyPage", "Aborted"));
                    AbortAction = false;
                    return;
                }

                ActionEndEvent(new ActionEventArgs("WaitTrafficInfoReadyPage", "Not ready"));
                System.Threading.Thread.Sleep((3 / ++checkTrafficInfoCount) * 1000);
                ActionStartEvent(new ActionEventArgs("WaitTrafficInfoReadyPage", "Getting traffic info..."));
                webResponse = WebClient.DownloadString(webTrafficUrl + serviceId);
            } while (webResponse.Contains("Učitavam podatke"));
        }

        // This method is not yet used...
        protected virtual void WaitTrafficInfoReadyWebService(string serviceId, ref string webResponse)
        {
            string pageId = Regex.Match(webResponse, "(<form.*?requestedPageId=)(\\d*?)([\"'].*?>)").Groups[2].Value;
            string serviceIdToken = Regex.Match(webResponse, "(_serviceIdToken.*?=.*?[\"'])([\\w-]*?)([\"'])").Groups[2].Value;
            
            string verifyServiceJson = string.Format("{{serviceIdToken:\"{0}\", requestedPageId:{1}}}", serviceIdToken, pageId);

            WebHeaderCollection headerParams = WebClient.Headers;
            ////WebClient.Headers = new WebHeaderCollection();
            //WebClient.Headers.Add("Accept", "application/json, text/javascript, */*; q=0.01");
            //WebClient.Headers.Add("Accept-Encoding", "gzip,deflate,sdch");
            //WebClient.Headers.Add("Accept-Language", "en-US,en;q=0.8,bs;q=0.6,hr;q=0.4");
            //WebClient.Headers.Add("Cache-Control", "max-age=0");
            ////WebClient.Headers.Add("Connection", "keep-alive");
            //WebClient.Headers[HttpRequestHeader.ContentType] = "application/json; charset=UTF-8";
            ////WebClient.Headers.Add("Host", "moj.hrvatskitelekom.hr");
            //WebClient.Headers.Add("Origin", "https://moj.hrvatskitelekom.hr");
            //WebClient.Headers.Add("Referer", "https://moj.hrvatskitelekom.hr/internet/ispis-spajanja?serviceid=" + serviceId);
            //WebClient.Headers.Add("User-Agent", headerParams[HttpRequestHeader.UserAgent]);
            //WebClient.Headers.Add("X-AjaxRequest", "true");
            //WebClient.Headers.Add("X-Requested-With", "XMLHttpRequest");

            do
            {
                System.Threading.Thread.Sleep((3 / ++checkTrafficInfoCount) * 1000);
                //byte[] loginRawResponse = WebClient.UploadData(trafficReadyUrl, "POST", System.Text.Encoding.UTF8.GetBytes(verifyServiceJson));
                //byte[] loginRawResponse = WebClient.UploadValues(trafficReadyUrl, "POST", verifyService);
                try
                {
                    //webResponse = WebClient.UploadString(trafficReadyUrl, "POST", verifyServiceJson);
                    // HACK: use reflection to set Host header
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(trafficReadyUrl));
                    request.Method = "POST";
                    request.Accept = "application/json, text/javascript, */*; q=0.01";
                    request.Headers["Accept-Encoding"] = "gzip,deflate,sdch";
                    request.Headers["Accept-Language"] = "en-US,en;q=0.8,bs;q=0.6,hr;q=0.4";
                    request.Headers["Cache-Control"] = "max-age=0";
                    //WebClient.Headers["Connection"] = "keep-alive");
                    request.ContentType = "application/json; charset=UTF-8";
                    //WebClient.Headers["Host"] = "moj.hrvatskitelekom.hr");
                    request.Headers["Origin"] = "https://moj.hrvatskitelekom.hr";
                    request.Referer = "https://moj.hrvatskitelekom.hr/internet/ispis-spajanja?serviceid=" + serviceId;
                    request.UserAgent = headerParams[HttpRequestHeader.UserAgent];
                    request.Headers["X-AjaxRequest"] = "true";
                    request.Headers["X-Requested-With"] = "XMLHttpRequest";
                    request.CookieContainer = ((CookieAwareWebClient)WebClient).CookieContainer;
                    //request.Headers.GetType().InvokeMember("ChangeInternal",
                    //    BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod,
                    //    null, request.Headers, new object[] { "Host", "moj.hrvatskitelekom.hr" });
                    //request.Headers.GetType().InvokeMember("ChangeInternal",
                    //    BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod,
                    //    null, request.Headers, new object[] { "Connection", "keep-alive" });

                    TransportContext context;
                    Stream s = request.GetRequestStream(out context);
                    byte[] payload = Encoding.UTF8.GetBytes(verifyServiceJson);
                    s.Write(payload, 0, payload.Length);
                    s.Close();
                    
                    WebResponse response = request.GetResponse();
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        webResponse = sr.ReadToEnd();
                    }
                }
                catch (WebException e)
                {
                    if (e.Status == WebExceptionStatus.ProtocolError)
                    {
                        HttpWebResponse response = (HttpWebResponse)e.Response;
                        if ((int)response.StatusCode == 500)
                        {
                            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                            {
                                webResponse = sr.ReadToEnd();
                            }
                        }
                    }
                }
                //long length = request.GetResponse().ContentLength;
#if DEBUG
                // TODO: remove response decoding, for debugging purposes only
                //webResponse = Encoding.UTF8.GetString(loginRawResponse);
#endif
                if (webResponse.Contains("true"))
                    break;

            } while (true);

            //WebClient.Headers = headerParams;
            //webResponse = WebClient.DownloadString(webTrafficUrl + serviceId);
        }
    }
}
