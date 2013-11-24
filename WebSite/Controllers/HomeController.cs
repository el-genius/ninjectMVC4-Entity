using NowOnline.AppHarbor.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NowOnline.AppHarbor.WebSite.Models;

namespace NowOnline.AppHarbor.WebSite.Controllers
{
    public class HomeController : ControllerBase
    {
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Application");
        }
    }
}
