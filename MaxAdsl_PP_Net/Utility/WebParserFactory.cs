using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MaxAdsl_PP_Net.Utility
{
    static class WebParserFactory
    {
        public enum WebParserTypes
        {
            Full,
            //Mobile
        }

        public static WebParser GetWebParser(WebParserTypes type){
            switch (type)
            {
                default:
                case WebParserTypes.Full:
                    return new WebParser();
                //case WebParserTypes.Mobile:
                //    return new WebParserMobile();
            }
        }
    }
}
