using System.Data.Entity;
using System.Data.SqlClient;

namespace NowOnline.AppHarbor.Repositories
{
    public class DataContextInitializer : IDatabaseInitializer<DataContext>
    {
        public void InitializeDatabase(DataContext context)
        {
            context.Database.CreateIfNotExists();
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DataContext, Configuration>());
            context.Database.Initialize(false);
        }
    }
}