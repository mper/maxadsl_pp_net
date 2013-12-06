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
            Mobile
        }

        public static IWebParser GetWebParser(WebParserTypes type){
            switch (type)
            {
                default:
                case WebParserTypes.Full:
                    return new WebParserFull();
                case WebParserTypes.Mobile:
                    return new WebParserMobile();
            }
        }
    }
}
