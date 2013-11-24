using NowOnline.AppHarbor.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NowOnline.AppHarbor.WebSite.Models
{
    public static class ViewModelAdapters
    {
        public static List<ApplicationViewModel> ToViewModels(this IEnumerable<Application> entities)
        {
            var viewModels = new List<ApplicationViewModel>();
            foreach(var entity in entities)
            {
                viewModels.Add(entity.ToViewModel());
            }

            return viewModels;
        }

        public static ApplicationViewModel ToViewModel(this Application entity)
        {
            var viewModel = new ApplicationViewModel();
            viewModel.Name = entity.Name;
            viewModel.TeamName = entity.Team.Name;
            return viewModel;
        }
    }
}
