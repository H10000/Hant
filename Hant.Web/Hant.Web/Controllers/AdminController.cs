using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hant.Web.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult RoleAdmin()
        {
            return View();
        }

        public ActionResult UserAdmin()
        {
            return View();
        }

        public ActionResult OuAdmin()
        {
            return View();
        }

        public ActionResult SetAdmin()
        {
            return View();
        }
    }
}