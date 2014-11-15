namespace CallCenterCrm.Web.Areas.Operator.Models.CallResult
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;

    public class IndexCallResultModel
    {
        [Required]
        [HiddenInput(DisplayValue=false)]
        public int CallResultId { get; set; }

        [Required]
        [Display(Name = "Campaign")]
        public string CampaignDescription { get; set; }

        [Required]
        [Display(Name = "Status")]
        public string StatusDescription { get; set; }

        [Required]
        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString="{0:g}")]
        public DateTime CallDate { get; set; }

        [Required]
        public int Duration { get; set; }

        public string Notes { get; set; }

        [Required]
        public string Customer { get; set; }
    }
}