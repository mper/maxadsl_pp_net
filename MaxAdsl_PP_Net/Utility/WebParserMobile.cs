﻿using MaxAdsl_PP_Net.Model;
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
    class WebParserMobile : WebParser
    {
        private string webStartUrl = Properties.Settings.Default.WebMobileStartUrl;
        private string webLoginUrl = Properties.Settings.Default.WebLoginUrl;
        private string webTrafficUrl = Properties.Settings.Default.WebMobileTraficUrl;


        public WebParserMobile() : base() { }
        
        public TrafficInfo GetTrafficInfo(string serviceId)
        {
            string webResponse = WebClient.DownloadString(webTrafficUrl + serviceId);
            WaitTrafficInfoReady(serviceId, ref webResponse);

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

    }
}
