namespace CallCenterCrm.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CallCenterCrm.Data.Models.Base;

    public class Campaign : BaseModel
    {
        public int CampaignId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Product { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public string Script { get; set; }

        public string ManagerId { get; set; }

        public virtual ApplicationUser Manager { get; set; }

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
