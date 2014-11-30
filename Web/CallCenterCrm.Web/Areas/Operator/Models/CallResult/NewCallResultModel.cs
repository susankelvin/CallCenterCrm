namespace CallCenterCrm.Web.Areas.Operator.Models.CallResult
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class NewCallResultModel
    {
        public int CallResultId { get; set; }

        [Display(Name = "Campaign")]
        public int CampaignId { get; set; }

        public IEnumerable<SelectListItem> Campaigns { get; set; }

        [Display(Name = "Status")]
        public int StatusId { get; set; }

        public IEnumerable<SelectListItem> Statuses { get; set; }

        [Required]
        [Range(1, 250)]
        public int Duration { get; set; }

        [AllowHtml]
        public string Notes { get; set; }

        [AllowHtml]
        [Required]
        public string Customer { get; set; }
    }
}
