namespace CallCenterCrm.Web.Areas.Manage.Models.Campaign
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;

    public class NewCampaignViewModel
    {
        [Required]
        [Display(Name="Start date")]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "End date")]
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

        [Required]
        public bool Active { get; set; }
    }
}
