using ChristiaanVerwijs.MvcSiteWithEntityFramework.Repositories;
using ChristiaanVerwijs.MvcSiteWithEntityFramework.WebSite.Controllers;
using ChristiaanVerwijs.MvcSiteWithEntityFramework.WebSite.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Web.Routing;
using System.Collections.Generic;
using System.Collections;
using System.Web.Mvc;
using System.Linq;
using System;

namespace ChristiaanVerwijs.MvcSiteWithEntityFramework.WebSite.Tests
{
    [TestClass]
    public class ApplicationControllerTests
    {
        private Mock<IApplicationRepository> applicationRepository;
        private Mock<ITeamRepository> teamRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            applicationRepository = new Mock<IApplicationRepository>();
            teamRepository = new Mock<ITeamRepository>();
        }

        [TestMethod]
        public void Index_returns_correct_viewmodel()
        {
            Assert.IsNotInstanceOfType(typeof(ApplicationListViewModel), CreateInstance().Index().GetType());
        }

        [TestMethod]
        public void Index_returns_list_of_all_applications_ordered_by_name()
        {
            // setup
            var application1 = new ApplicationBuilder().WithName("B").Build();
            var application2 = new ApplicationBuilder().WithName("A").Build();
            var application3 = new ApplicationBuilder().WithName("C").Build();
            applicationRepository.Setup(p => p.GetAll()).Returns(new List<Application>() { application1, application2, application3 });

            // act
            var controller = CreateInstance();
            ViewResult model = (ViewResult)controller.Index();
            var result = (ApplicationListViewModel)model.Model;

            // verify
            Assert.AreEqual(3, result.Applications.Count);
            Assert.AreEqual("A", application2.Name);
            Assert.AreEqual("B", application1.Name);
            Assert.AreEqual("C", application3.Name);
        }

        [TestMethod]
        public void Index_returns_empty_list_when_there_are_no_applications()
        {
            // setup
            var controller = CreateInstance();
            ViewResult model = (ViewResult)controller.Index();
            var result = (ApplicationListViewModel)model.Model;

            // verify
            Assert.AreEqual(0, result.Applications.Count);
        }

        [TestMethod]
        public void Create_returns_correct_viewmodel()
        {
            Assert.IsNotInstanceOfType(typeof(Application), CreateInstance().Index().GetType());
        }

        [TestMethod]
        public void Create_returns_view_preloaded_with_teams_ordered_by_name()
        {
            // setup
            var team1 = new TeamBuilder().WithName("B").Build();
            var team2 = new TeamBuilder().WithName("A").Build();
            teamRepository.Setup(p => p.GetAll()).Returns(new List<Team>() { team1, team2 });

            // act
            var controller = CreateInstance();
            ViewResult model = (ViewResult)controller.Create();
            SelectList result = (SelectList)model.ViewBag.TeamId;

            // verify
            Assert.AreEqual(2, result.Items.OfType<Team>().Count());
            Assert.AreEqual("A", result.Items.OfType<Team>().First().Name);
            Assert.AreEqual("B", result.Items.OfType<Team>().Last().Name);
        }

        [TestMethod]
        public void Create_returns_empty_list_of_teams_when_there_are_none()
        {
            // act
            var controller = CreateInstance();
            ViewResult model = (ViewResult)controller.Create();
            SelectList result = (SelectList)model.ViewBag.TeamId;

            // verify
            Assert.AreEqual(0, result.Items.OfType<Team>().Count());
        }

        [TestMethod]
        public void Create_saves_new_application_on_succesful_submit_and_returns_to_index_with_success_message()
        {
            // setup
            var application = new ApplicationBuilder().WithName("New app").WithDefaultTeam().Build();
            var viewModel = new ApplicationManageViewModel(application);
            applicationRepository.Setup(p => p.InsertAndSubmit(It.Is<Application>(x => 
                x.Name == application.Name
                && x.TeamId == application.TeamId))).Verifiable();

            // act
            var controller = CreateInstance();
            var model = (RedirectToRouteResult)controller.Create(viewModel);

            // verify
            Assert.IsTrue(controller.ViewData.ModelState.IsValid);
            applicationRepository.VerifyAll();
            Assert.IsNotNull(controller.TempData["SuccessMessage"]);
            Assert.IsNull(controller.TempData["ErrorMessage"]);
            Assert.AreEqual("Index", model.RouteValues["Action"]);
        }

        [TestMethod]
        public void Create_sets_error_message_on_unhandled_DAL_exception_and_returns_same_view()
        {
            // setup
            var application = new ApplicationBuilder().WithDefaultTeam().Build();
            var viewModel = new ApplicationManageViewModel(application);
            applicationRepository.Setup(p => p.InsertAndSubmit(It.IsAny<Application>())).Throws(new InvalidOperationException());

            // act
            var controller = CreateInstance();
            ViewResult model = (ViewResult)controller.Create(viewModel);

            // verify
            Assert.IsNotNull(controller.TempData["ErrorMessage"]);
            Assert.IsNull(controller.TempData["SuccessMessage"]);
        }

        [TestMethod]
        public void Edit_returns_view_preloaded_with_teams_ordered_by_name_and_with_the_specified_team_selected()
        {
            // setup
            var team1 = new TeamBuilder().WithName("B").Build();
            var team2 = new TeamBuilder().WithName("A").Build();
            teamRepository.Setup(p => p.GetAll()).Returns(new List<Team>() { team1, team2 });
            var application = new ApplicationBuilder().WithTeam(team1).WithName("A").Build();
            applicationRepository.Setup(p => p.GetById(application.Id)).Returns(application);

            // act
            var controller = CreateInstance();
            ViewResult model = controller.Edit(application.Id) as ViewResult;
            SelectList result = (SelectList)model.ViewBag.TeamId;

            // verify if correct application was returned
            Assert.AreEqual(application.Name, ((ApplicationManageViewModel)model.Model).Name);
            Assert.AreEqual(application.Id, ((ApplicationManageViewModel)model.Model).Id);

            // verify teams 
            Assert.AreEqual(2, result.Items.OfType<Team>().Count());
            Assert.AreEqual(application.TeamId, result.SelectedValue); // check if team was selected
            Assert.AreEqual("A", result.Items.OfType<Team>().First().Name);
            Assert.AreEqual("B", result.Items.OfType<Team>().Last().Name);
        }

        [TestMethod]
        public void Edit_throws_exception_if_application_does_not_exist_after_submit()
        {
            // setup
            var application = new ApplicationBuilder().WithDefaultTeam().Build();
            applicationRepository.Setup(p => p.GetById(application.Id)).Verifiable(); // don't return anything

            // act
            var controller = CreateInstance();

            try
            {
                controller.Edit(new ApplicationManageViewModel(application));
                Assert.Fail("ArgumentException was expected but not thrown");
            }
            catch (ArgumentException)
            {
                // should be thrown
            }
            finally
            {
                applicationRepository.VerifyAll();
            }
        }

        [TestMethod]
        public void Edit_returns_to_index_with_error_message_if_application_does_not_exist()
        {
            // setup
            var application = new ApplicationBuilder().WithDefaultTeam().Build();
            applicationRepository.Setup(p => p.GetById(application.Id)).Verifiable(); // don't return anything

            // act
            var controller = CreateInstance();
            var model = (RedirectToRouteResult)controller.Edit(application.Id);

            // verify
            applicationRepository.VerifyAll();
            Assert.AreEqual("Index", model.RouteValues["Action"]);
            Assert.IsNull(controller.TempData["SuccessMessage"]);
            Assert.IsNotNull(controller.TempData["ErrorMessage"]);
        }

        [TestMethod]
        public void Edit_saves_application_changes_on_succesful_and_returns_to_index_with_succes_message()
        {
            // setup
            var team1 = new TeamBuilder().Build();
            var team2 = new TeamBuilder().Build();
            teamRepository.Setup(p => p.GetAll()).Returns(new List<Team>() { team1, team2 });
            var application = new ApplicationBuilder().WithTeam(team1).WithName("A").Build();
            applicationRepository.Setup(p => p.GetById(application.Id)).Returns(application);

            // set up application change for name and team
            application.Name = "New name";
            application.TeamId = team2.Id;
            var viewModel = new ApplicationManageViewModel(application);

            // act
            var controller = CreateInstance();
            var model = (RedirectToRouteResult)controller.Edit(viewModel);

            // verify 
            applicationRepository.Verify(p => p.UpdateAndSubmit(It.Is<Application>(
                x => x.Id == application.Id
                && x.Name == application.Name
                && x.TeamId == application.TeamId)), Times.Once());
            Assert.AreEqual("Index", model.RouteValues["Action"]);
            Assert.IsNotNull(controller.TempData["SuccessMessage"]);
            Assert.IsNull(controller.TempData["ErrorMessage"]);
        }

        [TestMethod]
        public void Edit_sets_error_message_on_unhandled_DAL_exception_and_returns_same_view()
        {
            // setup
            var team1 = new TeamBuilder().WithName("B").Build();
            var team2 = new TeamBuilder().WithName("A").Build();
            teamRepository.Setup(p => p.GetAll()).Returns(new List<Team>() { team1, team2 });
            var application = new ApplicationBuilder().WithTeam(team1).WithName("A").Build();
            applicationRepository.Setup(p => p.GetById(application.Id)).Returns(application);
            applicationRepository.Setup(p => p.UpdateAndSubmit(It.IsAny<Application>())).Throws(new InvalidOperationException());
            var viewModel = new ApplicationManageViewModel(application);

            // act
            var controller = CreateInstance();
            var model = (ViewResult)controller.Edit(viewModel);

            // verify 
            Assert.IsNull(controller.TempData["SuccessMessage"]);
            Assert.IsNotNull(controller.TempData["ErrorMessage"]);
        }

        [TestMethod]
        public void Delete_returns_view_with_application_details_to_confirm_delete()
        {
            // setup
            var application = new ApplicationBuilder().WithDefaultTeam().WithName("A").Build();
            applicationRepository.Setup(p => p.GetById(application.Id)).Returns(application);

            // act
            var controller = CreateInstance();
            ViewResult model = (ViewResult)controller.Delete(application.Id);

            // verify if correct application was returned
            Assert.AreEqual(application.Name, ((ApplicationViewModel)model.Model).Name);
            Assert.AreEqual(application.Id, ((ApplicationViewModel)model.Model).Id);
            Assert.AreEqual(application.TeamId, ((ApplicationViewModel)model.Model).TeamId);
        }

        [TestMethod]
        public void Delete_soft_deletes_application_upon_confirm_and_redirects_to_index_with_success_message()
        {
            // setup
            var application = new ApplicationBuilder().WithDefaultTeam().WithName("A").Build();
            applicationRepository.Setup(p => p.GetById(application.Id)).Returns(application);

            // set up application change for name and team
            var viewModel = new ApplicationViewModel(application);

            // act
            var controller = CreateInstance();
            var model = (RedirectToRouteResult)controller.Delete(viewModel);

            // verify 
            applicationRepository.Verify(p => p.SoftDeleteAndSubmit(It.Is<Application>(
                x => x.Id == application.Id)), Times.Once());
            Assert.AreEqual("Index", model.RouteValues["Action"]);
            Assert.IsNotNull(controller.TempData["SuccessMessage"]);
            Assert.IsNull(controller.TempData["ErrorMessage"]);
        }

        [TestMethod]
        public void Delete_returns_to_index_with_error_message_if_application_does_not_exist()
        {
            // setup
            var application = new ApplicationBuilder().WithDefaultTeam().Build();
            applicationRepository.Setup(p => p.GetById(application.Id)).Verifiable(); // don't return anything

            // act
            var controller = CreateInstance();
            var model = (RedirectToRouteResult)controller.Delete(application.Id);

            // verify
            applicationRepository.VerifyAll();
            Assert.AreEqual("Index", model.RouteValues["Action"]);
            Assert.IsNull(controller.TempData["SuccessMessage"]);
            Assert.IsNotNull(controller.TempData["ErrorMessage"]);
        }

        [TestMethod]
        public void Delete_throws_exception_if_application_does_not_exist_after_submit()
        {
            // setup
            var application = new ApplicationBuilder().WithDefaultTeam().Build();
            applicationRepository.Setup(p => p.GetById(application.Id)).Verifiable(); // don't return anything

            // act
            var controller = CreateInstance();

            try
            {
                controller.Delete(new ApplicationManageViewModel(application));
                Assert.Fail("ArgumentException was expected but not thrown");
            }
            catch (ArgumentException)
            {
                // should be thrown
            }
            finally
            {
                applicationRepository.VerifyAll();
            }
        }

        [TestMethod]
        public void Delete_sets_error_message_on_unhandled_DAL_exception_and_returns_same_view()
        {
            // setup
            var application = new ApplicationBuilder().WithDefaultTeam().WithName("A").Build();
            applicationRepository.Setup(p => p.GetById(application.Id)).Returns(application);
            applicationRepository.Setup(p => p.SoftDeleteAndSubmit(It.IsAny<Application>())).Throws(new InvalidOperationException());
            var viewModel = new ApplicationViewModel(application);

            // act
            var controller = CreateInstance();
            var model = (ViewResult)controller.Delete(viewModel);

            // verify 
            Assert.IsNull(controller.TempData["SuccessMessage"]);
            Assert.IsNotNull(controller.TempData["ErrorMessage"]);
        }

        private ApplicationController CreateInstance()
        {
            return new ApplicationController(applicationRepository.Object, teamRepository.Object);
        }
    }
}
