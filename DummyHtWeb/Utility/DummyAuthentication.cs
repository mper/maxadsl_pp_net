using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DummyHtWeb2
{
    public class DummyAuthentication : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return httpContext.Session["Authorized"] != null && (bool)httpContext.Session["Authorized"];
        }
    }
}