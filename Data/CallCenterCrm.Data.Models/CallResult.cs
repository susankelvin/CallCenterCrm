namespace CallCenterCrm.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using CallCenterCrm.Data.Models.Base;

    public class CallResult : BaseModel
    {
        [Key]
        public int CallResultId { get; set; }

        public int CampaignId { get; set; }

        public virtual Campaign Campaign { get; set; }

        public string OperatorId { get; set; }

        public virtual ApplicationUser Operator { get; set; }

        public int StatusId { get; set; }

        public Status Status { get; set; }

        public DateTime CallDate { get; set; }

        public int Duration { get; set; }

        public string Notes { get; set; }

        public string Customer { get; set; }
    }
}
