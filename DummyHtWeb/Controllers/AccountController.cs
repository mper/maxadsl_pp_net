using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DummyHtWeb2.Controllers
{
    [DummyAuthentication]
    public class AccountController : Controller
    {
        //
        // GET: /Account/
        [AllowAnonymous]
        public ActionResult Login(string ReturnUrl)
        {
            TempData["ReturnUrl"] = ReturnUrl;
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            Session["Authorized"] = username == "test" && password == "test1";
            return Redirect(((string)TempData["ReturnUrl"]) ?? "~/");
        }

    }
}
