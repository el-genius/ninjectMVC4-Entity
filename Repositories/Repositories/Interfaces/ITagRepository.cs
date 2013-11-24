using System;
using System.Collections.Generic;

namespace NowOnline.AppHarbor.Repositories
{
    public interface ITagRepository : IGenericRepository<Tag>
    {
        IEnumerable<Tag> GetByBitBucketName(string bitBucketName);
    }
}
