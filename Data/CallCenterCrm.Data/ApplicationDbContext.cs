namespace CallCenterCrm.Data
{
    using System.Data.Entity;
    using CallCenterCrm.Data.Models;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Office> Offices { get; set; }

        public DbSet<Campaign> Campaigns { get; set; }

        public DbSet<CallResult> CallResults { get; set; }

        public DbSet<Status> Statuses { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
