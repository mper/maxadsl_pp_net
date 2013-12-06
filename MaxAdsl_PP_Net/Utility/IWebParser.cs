using System;
namespace MaxAdsl_PP_Net.Utility
{
    interface IWebParser
    {
        System.Net.WebClient WebClient { get; set; }
                
        System.Collections.Specialized.NameValueCollection GetLoginTokens();

        string LoginAndGetServiceId(System.Collections.Specialized.NameValueCollection webLoginCredidentials);

        MaxAdsl_PP_Net.Model.TrafficInfo GetTrafficInfo(string serviceId);
    }
}
