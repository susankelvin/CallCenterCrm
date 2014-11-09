namespace CallCenterCrm.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class CallResult
    {
        [Key]
        public int CallResultId { get; set; }

        public int CampaignId { get; set; }

        public Campaign Campaign { get; set; }

        public string OperatorId { get; set; }

        public ApplicationUser Operator { get; set; }

        public int StatusId { get; set; }

        public Status Status { get; set; }

        public DateTime CallDate { get; set; }

        public int Duration { get; set; }

        public string Notes { get; set; }

        public string Customer { get; set; }
    }
}
