namespace NowOnline.AppHarbor.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.SqlTypes;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(DataContext context)
        {
            var seededBefore = context.Applications.Any();
            if (seededBefore) { return; }

            var digiflexTeam = new Team() { Name = "Digiflex" };
            var nekoTeam = new Team() { Name = "Neko" };

            context.Applications.Add(new Application() { Name = "Plug And Payroll", BitBucketName = "plugandpayroll", Team = nekoTeam });
            context.Applications.Add(new Application() { Name = "Digiflex", BitBucketName = "digiflex", Team = digiflexTeam });
            context.Applications.Add(new Application() { Name = "WubHub", BitBucketName = "wubhub", Team = digiflexTeam });

            context.SaveChanges();
        }
    }
}
