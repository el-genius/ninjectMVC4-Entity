using NowOnline.AppHarbor.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NowOnline.AppHarbor.WebSite.Models;

namespace NowOnline.AppHarbor.WebSite.Controllers
{
    public class HomeController : Controller
    {
        internal readonly IApplicationRepository applicationRepository;

        public HomeController(IApplicationRepository applicationRepository)
        {
            this.applicationRepository = applicationRepository;
        }

        public ActionResult Index()
        {
            var viewModel = new ApplicationsViewModel();
            var applications = applicationRepository.GetAll();
            return View(applications.ToViewModels());
        }
    }
}
