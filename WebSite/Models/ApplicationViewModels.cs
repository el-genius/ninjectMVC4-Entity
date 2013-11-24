using NowOnline.AppHarbor.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NowOnline.AppHarbor.WebSite.Models
{
    public class ApplicationViewModel
    {
        public ApplicationViewModel()
        {
        }
        public ApplicationViewModel(Application application)
        {
            this.Id = application.Id;
            this.Name = application.Name;
            this.TeamName = application.Team != null ? application.Team.Name : string.Empty;
            this.TeamId = application.TeamId;
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        public string TeamName { get; set; }

        [Required(ErrorMessage = "Team is required")]
        public int TeamId { get; set; }
    }

    public class ApplicationListViewModel
    {
        public ApplicationListViewModel()
        {
            this.Applications = new List<ApplicationViewModel>();
        }

        public ApplicationListViewModel(IEnumerable<Application> applications)
        {
            this.Applications = new List<ApplicationViewModel>();

            foreach (var application in applications)
            {
                this.Applications.Add(new ApplicationViewModel(application));
            }
        }

        public List<ApplicationViewModel> Applications { get; set; }
    }

    public class ApplicationManageViewModel : ApplicationViewModel
    {
        public ApplicationManageViewModel()
            : base()
        {
        }
        public ApplicationManageViewModel(Application application)
            : base(application)
        {
        }

        public Application ToDalEntity()
        {
            var model = new Application();
            model.Name = this.Name;
            model.TeamId = this.TeamId;
            return model;
        }
    }
}
