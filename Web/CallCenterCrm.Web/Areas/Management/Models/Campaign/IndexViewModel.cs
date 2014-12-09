namespace CallCenterCrm.Web.Areas.Management.Models.Campaign
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class IndexViewModel
    {
        [Required]
        [HiddenInput(DisplayValue=false)]
        public int CampaignId { get; set; }

        [Required]
        [Display(Name = "Start date")]
        [UIHint("Date")]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "End date")]
        [UIHint("Date")]
        public DateTime EndDate { get; set; }

        [Required]
        public string Product { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Script { get; set; }

        [Required]
        [UIHint("Bool")]
        public bool Active { get; set; }
    }
}