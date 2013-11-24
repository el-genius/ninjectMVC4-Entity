using System;
using System.Linq;
using System.Collections.Generic;

namespace NowOnline.AppHarbor.Repositories
{
    public class TagRepository : GenericRepository<Tag>, ITagRepository
    {
        public TagRepository(IDataContext dataContext)
            : base(dataContext)
        {
        }

        public virtual IEnumerable<Tag> GetByBitBucketName(string bitBucketName)
        {
            return base.DataSource()
                .Where(p => p.Application.BitBucketName == bitBucketName)
                .OrderBy(p => p.Name);
        }
    }
}
