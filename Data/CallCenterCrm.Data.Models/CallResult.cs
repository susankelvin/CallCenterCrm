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

        [Required]
        public int CampaignId { get; set; }

        public virtual Campaign Campaign { get; set; }

        public string OperatorId { get; set; }

        public virtual ApplicationUser Operator { get; set; }

        [Required]
        public int StatusId { get; set; }

        public Status Status { get; set; }

        [Required]
        public DateTime CallDate { get; set; }

        [Required]
        public int Duration { get; set; }

        public string Notes { get; set; }

        [Required]
        public string Customer { get; set; }
    }
}
