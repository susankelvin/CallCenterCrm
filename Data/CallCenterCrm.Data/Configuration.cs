namespace CallCenterCrm.Data
{
    using CallCenterCrm.Data.Models;
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            if (!context.Statuses.Any())
            {
                Status[] statuses = new[]
                {
                    new Status()
                    {
                        Description = "Sold"
                    },
                    new Status()
                    {
                        Description = "Not interested"
                    },
                    new Status()
                    {
                        Description = "Hesitating"
                    },
                    new Status()
                    {
                        Description = "Callback"
                    },
                    new Status()
                    {
                        Description = "No answer"
                    },
                    new Status()
                    {
                        Description = "Rival producted"
                    }
                };

                context.Statuses.AddRange(statuses);
                context.SaveChanges();
            }

            if (!context.Roles.Any())
            {
                RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                roleManager.Create(new IdentityRole("Operator"));
                roleManager.Create(new IdentityRole("Manager"));
                roleManager.Create(new IdentityRole("Admin"));
                roleManager.Create(new IdentityRole("Customer"));
            }

            if (!context.Users.Any())
            {
                ApplicationUser admin = new ApplicationUser()
                {
                    Email = @"admin@a.b",
                    UserName = @"admin@a.b"
                };
                string password = @"adminpassword";
                UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                userManager.Create(admin, password);
                userManager.AddToRole(admin.Id, "Admin");
            }
        }
    }
}
