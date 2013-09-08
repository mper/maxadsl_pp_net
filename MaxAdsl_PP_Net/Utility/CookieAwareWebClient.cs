using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

    }
}
