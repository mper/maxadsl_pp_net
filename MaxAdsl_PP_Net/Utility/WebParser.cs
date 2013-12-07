using System;
namespace MaxAdsl_PP_Net.Utility
{
    abstract class WebParser
    {
        public virtual System.Net.WebClient WebClient { get; set; }

        private bool abortAction = false;
        public virtual bool AbortAction { get { return abortAction; } set { abortAction = value; } }

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
        public delegate void OnActionHandler(object sender, ActionEventArgs e);
        public event EventHandler<ActionEventArgs> ActionStart;
        public event EventHandler<ActionEventArgs> ActionEnd;

        public abstract System.Collections.Specialized.NameValueCollection GetLoginTokens();

        public abstract string LoginAndGetServiceId(System.Collections.Specialized.NameValueCollection webLoginCredidentials);

        public abstract MaxAdsl_PP_Net.Model.TrafficInfo GetTrafficInfo(string serviceId);

        protected virtual void OnActionStart(ActionEventArgs e)
        {
            EventHandler<ActionEventArgs> handler = ActionStart;
            if (handler != null)
                handler(this, e);
        }

        protected virtual void OnActionEnd(ActionEventArgs e)
        {
            EventHandler<ActionEventArgs> handler = ActionEnd;
            if (handler != null)
                handler(this, e);
        }
    }
}
