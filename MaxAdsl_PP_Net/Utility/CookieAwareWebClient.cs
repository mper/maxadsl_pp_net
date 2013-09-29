using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;

namespace MaxAdsl_PP_Net.Utility
{
    internal class CookieAwareWebClient : WebClient
    {
        public CookieAwareWebClient() : this(new CookieContainer()) { }

        public CookieAwareWebClient(CookieContainer c)
        {
            this.CookieContainer = c;
        }

        public CookieContainer CookieContainer { get; set; }

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = base.GetWebRequest(address);
            if (request != null)
            {
                ((HttpWebRequest)request).CookieContainer = this.CookieContainer;
            }

            return request;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            //sb.AppendFormat("", this.si

            sb.AppendLine("Cookies:");
            Hashtable table = (Hashtable)CookieContainer.GetType().InvokeMember("m_domainTable",
                    BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance, null,
                    CookieContainer, new object[] { });

            foreach (string key in table.Keys)
            {
                string procKey = key.Trim();
                procKey = procKey.Trim('.');
                sb.AppendFormat("URI:{1}{0}", Environment.NewLine, procKey);
                try
                {
                    foreach (Cookie cookie in CookieContainer.GetCookies(new Uri(string.Concat("http://", procKey, "/"))))
                    {
                        sb.AppendFormat("{1}:{2} ; Domain = {3}{0}",
                                Environment.NewLine, cookie.Name, cookie.Value, cookie.Domain);
                    }
                    sb.AppendLine();
                }
                catch (UriFormatException)
                {
                    sb.AppendFormat("Skipping invalid uri:'{1}'...{0}", Environment.NewLine, procKey);
                }
            }
            return sb.ToString();
        }

    }
}
