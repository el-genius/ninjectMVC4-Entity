using System;
using System.Data.Entity;

namespace NowOnline.AppHarbor.Repositories
{
    public interface IDataContext
    {
        IDbSet<T> Set<T>() where T : class;
        int SaveChanges();
        void ExecuteCommand(string command, params object[] parameters);
    }
}
