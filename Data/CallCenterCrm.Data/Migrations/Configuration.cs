namespace CallCenterCrm.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using CallCenterCrm.Data;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
        }
    }
}
