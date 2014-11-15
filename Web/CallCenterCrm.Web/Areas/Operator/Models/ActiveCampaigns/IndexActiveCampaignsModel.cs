namespace CallCenterCrm.Web.Areas.Operator.Models.ActiveCampaigns
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class IndexActiveCampaignsModel
    {
        [Required]
        [Display(Name="Start date")]
        [DisplayFormat(DataFormatString="{0:D}")]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "End date")]
        [DisplayFormat(DataFormatString = "{0:D}")]
        public DateTime EndDate { get; set; }

        [Required]
        public string Product { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Script { get; set; }
    }
}
