using NowOnline.AppHarbor.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NowOnline.AppHarbor.WebSite.Models;
using System.Web.Routing;

namespace NowOnline.AppHarbor.WebSite.Controllers
{
    public class ControllerBase : Controller
    {
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            ViewData["SuccessMessage"] = GetSuccessMessage();
            ViewData["ErrorMessage"] = GetErrorMessage();
        }

        #region Set Error And Success Messages
        protected void SetSuccessMessage(string message, params string[] args)
        {
            Session["SuccessMessage"] = string.Format(message, args);
        }

        protected void SetErrorMessage(string message, params string[] args)
        {
            Session["ErrorMessage"] = string.Format(message, args);
        }

        protected string GetSuccessMessage()
        {
            if (Session["SuccessMessage"] != null)
            {
                string value = Session["SuccessMessage"].ToString();
                Session["SuccessMessage"] = null;
                return value;
            }

            return null;
        }

        protected string GetErrorMessage()
        {
            if (Session["ErrorMessage"] != null)
            {
                string value = Session["ErrorMessage"].ToString();
                Session["ErrorMessage"] = null;
                return value;
            }

            return null;
        }
        #endregion
    }
}
