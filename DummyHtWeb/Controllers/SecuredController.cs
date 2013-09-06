using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DummyHtWeb2.Controllers
{
    [DummyAuthentication]
    public class SecuredController : Controller
    {
        //
        // GET: /Secured/

        public ActionResult Index()
        {
            return View();
        }

    }
}
