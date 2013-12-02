using ChristiaanVerwijs.MvcSiteWithEntityFramework.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChristiaanVerwijs.MvcSiteWithEntityFramework.WebSite.Models;
using System.Web.Routing;

namespace ChristiaanVerwijs.MvcSiteWithEntityFramework.WebSite.Controllers
{
    public class ControllerBase : Controller
    {
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
        }

        #region Set Error And Success Messages
        protected void SetSuccessMessage(string message, params string[] args)
        {
            TempData["SuccessMessage"] = string.Format(message, args);
        }

        protected void SetErrorMessage(string message, params string[] args)
        {
            TempData["ErrorMessage"] = string.Format(message, args);
        }
        #endregion
    }
}
