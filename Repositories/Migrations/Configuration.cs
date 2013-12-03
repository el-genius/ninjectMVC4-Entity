namespace ChristiaanVerwijs.MvcSiteWithEntityFramework.Repositories
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
            var seededBefore = context.Set<Application>().Any();
            if (seededBefore) { return; }

            var teamA = new Team() { Name = "Team A" };
            var teamB = new Team() { Name = "Team B" };

            context.Set<Application>().Add(new Application() { Name = "Application A", Team = teamB, Description = "This is application A" });
            context.Set<Application>().Add(new Application() { Name = "Application B", Team = teamA, Description = "This is application B" });
            context.Set<Application>().Add(new Application() { Name = "Application C", Team = teamA, Description = "This is application C" });

            context.SaveChanges();
        }
    }
}