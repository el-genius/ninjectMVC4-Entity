using System.Data.Entity;
using System.Data.SqlClient;

namespace NowOnline.AppHarbor.Repositories
{
    public class DatabaseInititializer : IDatabaseInitializer<DataContext>
    {
        public void InitializeDatabase(DataContext context)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DataContext, Configuration>());
            context.Database.Initialize(false);
        }
    }
}