using ChristiaanVerwijs.MvcSiteWithEntityFramework.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChristiaanVerwijs.MvcSiteWithEntityFramework.WebSite.Models
{
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
            model.Id = this.Id;
            model.Name = this.Name;
            model.Description = this.Description;
            model.TeamId = this.TeamId;
            return model;
        }
    }
}
