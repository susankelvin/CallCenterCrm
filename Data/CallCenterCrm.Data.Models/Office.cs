namespace CallCenterCrm.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using CallCenterCrm.Data.Models.Base;

    public class Office : BaseModel
    {
        public int OfficeId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string ManagerId { get; set; }

        public virtual ApplicationUser Manager { get; set; }

        public virtual ICollection<ApplicationUser> Operators { get; set; }

        public virtual ICollection<Campaign> Campaigns { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Email { get; set; }

        public Office()
        {
            this.Operators = new HashSet<ApplicationUser>();
            this.Campaigns = new HashSet<Campaign>();
        }
    }
}
