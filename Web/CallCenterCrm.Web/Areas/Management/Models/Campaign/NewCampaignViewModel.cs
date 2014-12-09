namespace CallCenterCrm.Web.Areas.Management.Models.Campaign
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class NewCampaignViewModel
    {
        [Required]
        [Display(Name="Start date")]
        [UIHint("Date")]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "End date")]
        [UIHint("Date")]
        public DateTime EndDate { get; set; }

        [AllowHtml]
        [Required]
        public string Product { get; set; }

        [Required]
        public decimal Price { get; set; }

        [AllowHtml]
        [Required]
        public string Description { get; set; }

        [AllowHtml]
        [Required]
        public string Script { get; set; }

        public bool Active { get; set; }
    }
}
