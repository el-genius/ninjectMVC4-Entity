using System;
using System.Linq;
using System.Collections.Generic;

namespace NowOnline.AppHarbor.Repositories
{
    public class ApplicationRepository : GenericRepository<Application>, IApplicationRepository
    {
        public ApplicationRepository(IDataContext dataContext)
            : base(dataContext)
        {
        }

    }
}
