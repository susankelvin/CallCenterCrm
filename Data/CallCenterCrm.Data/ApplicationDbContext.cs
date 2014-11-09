using System;
using System.Linq;
using CallCenterCrm.Data.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CallCenterCrm.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
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
