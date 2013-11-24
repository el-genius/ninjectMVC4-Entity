using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NowOnline.AppHarbor.WebSite.Models
{
    public class ApplicationViewModel
    {
        public string Name { get; set; }
        public string TeamName { get; set; }
    }

    public class ApplicationsViewModel
    {
        public ApplicationsViewModel()
        {
            this.Applications = new List<ApplicationViewModel>();
        }

        public List<ApplicationViewModel> Applications { get; set; }
    }
}
