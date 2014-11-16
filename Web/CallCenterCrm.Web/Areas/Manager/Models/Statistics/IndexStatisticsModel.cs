namespace CallCenterCrm.Web.Areas.Manager.Models.Statistics
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class IndexStatisticsModel
    {
        public IEnumerable<CampaignStatisticsModel> CampaignStatistics { get; set; }

        [Display(Name="Campaigns")]
        public int CampaingId { get; set; }

        public IEnumerable<SelectListItem> Campaigns { get; set; }
    }
}
