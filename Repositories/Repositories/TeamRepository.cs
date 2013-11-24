using System;
using System.Linq;
using System.Collections.Generic;

namespace NowOnline.AppHarbor.Repositories
{
    public class TeamRepository : GenericRepository<Team>, ITeamRepository
    {
        public TeamRepository(IDataContext dataContext)
            : base(dataContext)
        {
        }
    }
}
