namespace CallCenterCrm.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public class Office
    {
        public int OfficeId { get; set; }

        [Display(Name="Office name")]
        public string Name { get; set; }

        [Required]
        [Display(Name="Manager")]
        public string ManagerId { get; set; }

        public ApplicationUser Manager { get; set; }

        public virtual ICollection<ApplicationUser> Operators { get; set; }

        public virtual ICollection<Campaign> Campaigns { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [Display(Name="Phone number")]
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
