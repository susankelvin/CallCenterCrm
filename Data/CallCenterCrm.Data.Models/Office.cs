namespace CallCenterCrm.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Office
    {
        public int OfficeId { get; set; }

        [Display(Name="Office")]
        public string Name { get; set; }

        public string ManagerId { get; set; }

        public ApplicationUser Manager { get; set; }

        public virtual ICollection<ApplicationUser> Operators { get; set; }

        public virtual ICollection<Campaign> Campaigns { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public Office()
        {
            this.Operators = new HashSet<ApplicationUser>();
            this.Campaigns = new HashSet<Campaign>();
        }
    }
}
