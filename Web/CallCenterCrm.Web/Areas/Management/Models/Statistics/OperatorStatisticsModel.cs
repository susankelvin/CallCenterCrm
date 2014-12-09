namespace CallCenterCrm.Web.Areas.Management.Models.Statistics
{
    using System.ComponentModel.DataAnnotations;
    public class OperatorStatisticsModel
    {
        public string Name { get; set; }

        [Display(Name = "Time per sell")]
        public decimal SoldToTotalTimeAll { get; set; }

        [Display(Name = "Time per sell campaign")]
        public decimal SoldToTotalTimeCampaign { get; set; }

        [Display(Name = "Average call duration")]
        public decimal AverageDuration { get; set; }
    }
}
