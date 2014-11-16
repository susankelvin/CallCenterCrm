using System.ComponentModel.DataAnnotations;
namespace CallCenterCrm.Web.Areas.Manager.Models.Statistics
{
    public class OperatorStatisticsModel
    {
        public string Name { get; set; }

        [Display(Name="Time per sell")]
        public decimal SoldToTotalTimeAll { get; set; }

        [Display(Name = "Time per sell campaign")]
        public decimal SoldToTotalTimeCampaign { get; set; }

        [Display(Name = "Average call duration")]
        public decimal AverageDuration { get; set; }
    }
}
