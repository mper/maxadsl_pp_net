using MaxAdsl_PP_Net.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Threading;
namespace MaxAdsl_PP_Net.Utility
{
    abstract class WebParser
    {
        // Public
        public virtual System.Net.WebClient WebClient { get; set; }

        public virtual bool AbortAction { get; set; }
        public bool IsRunning { get; set; }

        public class ActionEventArgs : EventArgs
        {
            public string Action { get; set; }
            public string Message { get; set; }

            public ActionEventArgs() { }
            public ActionEventArgs(string action, string message)
            {
                Action = action;
                Message = message;
            }
        }
        public class TrafficInfoEventArgs : EventArgs
        {
            public TrafficInfo TrafficInfo { get; set; }
            public TrafficInfoEventArgs() { }
            public TrafficInfoEventArgs(TrafficInfo t)
            {
                TrafficInfo = t;
            }
        }
        public event EventHandler<ActionEventArgs> OnActionStart;
        public event EventHandler<ActionEventArgs> OnActionEnd;
        public event EventHandler<TrafficInfoEventArgs> OnTrafficInfoComplete;



        // Private
        protected string webStartUrl;
        protected string webLoginUrl;
        protected string webTrafficUrl;
        protected NameValueCollection webLoginCredentials;
        protected string serviceId;


        // Public Method Definitions
        public abstract NameValueCollection GetLoginTokensStep();

        public abstract string LoginAndGetServiceIdStep(NameValueCollection webLoginCredentials);

        public abstract MaxAdsl_PP_Net.Model.TrafficInfo GetTrafficInfoStep(string serviceId);
        


        // Implemented Methods
        protected WebParser()
        {
            webStartUrl = Properties.Settings.Default.WebStartUrl;
            webLoginUrl = Properties.Settings.Default.WebLoginUrl;
            webTrafficUrl = Properties.Settings.Default.WebTrafficUrl;

            WebClient = new CookieAwareWebClient();
            WebClient.Encoding = Encoding.UTF8;
            // Allow untrusted certificates
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            // Set chrome user agent
            WebClient.Headers.Add(HttpRequestHeader.UserAgent, Properties.Settings.Default.EmulateUserAgent);

//#if DEBUG
//            webStartUrl = Properties.Settings.Default.MockStartUrl;
//            webLoginUrl = Properties.Settings.Default.MockLoginUrl;
//            webTrafficUrl = Properties.Settings.Default.MockTrafficUrl;
//#endif
        }

        public TrafficInfo GetTrafficInfo(string username, string password)
        {
            AbortAction = false;
            IsRunning = true;
            TrafficInfo retVal;

            if (webLoginCredentials == null)
            {
                webLoginCredentials = GetLoginTokensStep();
                webLoginCredentials.Add(Properties.Settings.Default.WebLoginUsernameFieldName, username);
                webLoginCredentials.Add(Properties.Settings.Default.WebLoginPasswordFieldName, password);
            }

            if (serviceId == null)
                serviceId = LoginAndGetServiceIdStep(webLoginCredentials);

            retVal = GetTrafficInfoStep(serviceId);
            TrafficInfoCompleteEvent(retVal);
            IsRunning = false;
            return retVal;
        }

        public void GetTrafficInfoAsync(string username, string password)
        {
            Thread t = new Thread(delegate(object o)
            {
                string[] s = (string[])o;
                GetTrafficInfo(s[0], s[1]);
            });
            t.Start(new string[]{username, password});
        }

        // Private Methods
        protected virtual void ActionStartEvent(ActionEventArgs e)
        {
            EventHandler<ActionEventArgs> handler = OnActionStart;
            if (handler != null)
                handler(this, e);
        }

        protected virtual void ActionEndEvent(ActionEventArgs e)
        {
            EventHandler<ActionEventArgs> handler = OnActionEnd;
            if (handler != null)
                handler(this, e);
        }

        protected virtual void TrafficInfoCompleteEvent(TrafficInfo trafficInfo)
        {
            EventHandler<TrafficInfoEventArgs> handler = OnTrafficInfoComplete;
            if (handler != null)
                handler(this, new TrafficInfoEventArgs(trafficInfo));
        }

    }
}
