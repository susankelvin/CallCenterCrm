namespace CallCenterCrm.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using CallCenterCrm.Data.Models.Base;

    public class Campaign : BaseModel
    {
        [Key]
        public int CampaignId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public string Product { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Script { get; set; }

        public string ManagerId { get; set; }

        public virtual ApplicationUser Manager { get; set; }

        [Required]
        public bool Active { get; set; }

        public virtual ICollection<Office> Offices { get; set; }

        public virtual ICollection<CallResult> CallResults { get; set; }

        public Campaign()
        {
            this.CallResults = new HashSet<CallResult>();
            this.Offices = new HashSet<Office>();
        }
    }
}
