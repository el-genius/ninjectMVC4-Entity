namespace NowOnline.AppHarbor.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(DataContext context)
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