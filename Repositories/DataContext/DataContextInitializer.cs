using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace NowOnline.AppHarbor.Repositories
{
    public class DataContextInitializer : IDatabaseInitializer<DataContext>
    {
        public void InitializeDatabase(DataContext context)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DataContext, Configuration>());
            context.Database.Initialize(true);
            Seed(context);
        }

        protected virtual void Seed(DataContext context)
        {
            var seededBefore = context.Applications.Any();
            if (seededBefore) { return; }

            var teamA = new Team() { Name = "Team A" };
            var teamB = new Team() { Name = "Team B" };

            context.Applications.Add(new Application() { Name = "Application A", Team = teamB });
            context.Applications.Add(new Application() { Name = "Application B", Team = teamA });
            context.Applications.Add(new Application() { Name = "Application C", Team = teamA });

            context.SaveChanges();
        }
    }
}