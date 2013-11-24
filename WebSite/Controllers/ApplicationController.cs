using NowOnline.AppHarbor.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NowOnline.AppHarbor.WebSite.Models;

namespace NowOnline.AppHarbor.WebSite.Controllers
{
    public class ApplicationController : ControllerBase
    {
        internal readonly IApplicationRepository applicationRepository;
        internal readonly ITeamRepository teamRepository;

        public ApplicationController(IApplicationRepository applicationRepository, ITeamRepository teamRepository)
        {
            this.applicationRepository = applicationRepository;
            this.teamRepository = teamRepository;
        }

        public ActionResult Index()
        {
            var viewModel = new ApplicationListViewModel();
            var applications = applicationRepository.GetAll().OrderBy(p => p.Name);
            return View(new ApplicationListViewModel(applications));
        }

        public ActionResult Create()
        {
            LoadTeamsInViewData();
            return View(new ApplicationManageViewModel());
        }

        [HttpPost]
        public ActionResult Create(ApplicationManageViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var application = model.ToDalEntity();
                    applicationRepository.InsertAndSubmit(application);

                    base.SetSuccessMessage("The application [{0}] has been created.", application.Name);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    base.SetErrorMessage("Whoops! Couldn't create the new application. The error was [{0}]", ex.Message);
                }
            }

            LoadTeamsInViewData();
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var application = applicationRepository.GetById(id);

            if (application == null)
            {
                base.SetErrorMessage("Application with Id [{0}] does not exist", id.ToString());
                RedirectToAction("Index");
            }

            LoadTeamsInViewData(application.TeamId);
            return View(new ApplicationManageViewModel(application));
        }

        [HttpPost]
        public ActionResult Edit(ApplicationManageViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var application = applicationRepository.GetById(model.Id);
                    if (application == null)
                    {
                        base.SetErrorMessage("Application with Id [{0}] does not exist", model.Id.ToString());
                        RedirectToAction("Index");
                    }

                    application.Name = model.Name;
                    application.TeamId = model.TeamId;
                    applicationRepository.UpdateAndSubmit(application);

                    base.SetSuccessMessage("The application [{0}] has been updated.", application.Name);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    base.SetErrorMessage("Whoops! Couldn't update the application. The error was [{0}]", ex.Message);
                }
            }

            LoadTeamsInViewData();
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            var application = applicationRepository.GetById(id);

            if (application == null)
            {
                base.SetErrorMessage("Application with Id [{0}] does not exist", id.ToString());
                RedirectToAction("Index");
            }

            return View(new ApplicationViewModel(application));
        }

        [HttpPost]
        public ActionResult Delete(ApplicationViewModel model)
        {
            try
            {
                var application = applicationRepository.GetById(model.Id);
                if (application == null)
                {
                    base.SetErrorMessage("Application with Id [{0}] does not exist", model.Id.ToString());
                    RedirectToAction("Index");
                }

                application.Deleted = DateTime.Now;
                applicationRepository.UpdateAndSubmit(application);

                base.SetSuccessMessage("The application has been (soft) deleted.");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                base.SetErrorMessage("Whoops! Couldn't delete the application. The error was [{0}]", ex.Message);
            }

            return View(model);
        }

        #region Private Helpers
        private void LoadTeamsInViewData(object selectedValue = null)
        {
            ViewBag.TeamId = new SelectList(teamRepository.GetAll(), "Id", "Name", selectedValue);
        }
        #endregion
    }
}
