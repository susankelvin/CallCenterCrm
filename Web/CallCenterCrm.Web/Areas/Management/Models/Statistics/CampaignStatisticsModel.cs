namespace CallCenterCrm.Web.Areas.Management.Models.Statistics
{
    using System.ComponentModel.DataAnnotations;

    public class CampaignStatisticsModel
    {
        [Display(Name="Sold percent")]
        public decimal SoldToTotalCalls { get; set; }

        [Display(Name = "Time per sell")]
        public decimal SoldToTotalTime { get; set; }

        [Display(Name = "Time per price")]
        public decimal TotelSoldPriceToTotalTime { get; set; }

        [Display(Name = "Unanswered percent")]
        public decimal NotAnsweredToTotalCalls { get; set; }
    }
}
