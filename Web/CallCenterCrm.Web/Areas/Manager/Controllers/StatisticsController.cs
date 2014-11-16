namespace CallCenterCrm.Web.Areas.Manage.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using CallCenterCrm.Data;
    using CallCenterCrm.Web.Areas.Manager.Models.Statistics;

    public class StatisticsController : Controller
    {
        private readonly ICallCenterCrmData data;

        public StatisticsController(ICallCenterCrmData data)
        {
            this.data = data;
        }

        // GET: Manager/Status
        public ActionResult Index(int? campaingId)
        {
            if (campaingId == null)
            {
                campaingId = 1;
            }

            CampaignStatisticsModel campaignStatistics = GetCampaignStatistics(campaingId ?? 0);
            IndexStatisticsModel result = new IndexStatisticsModel();
            result.CampaignStatistics = new[] { campaignStatistics };

            var campaings = this.data.Campaigns.All()
                                .Select(c => new SelectListItem()
                                       {
                                           Text = c.Description,
                                           Value = c.CampaignId.ToString()
                                       })
                                .ToList();
            result.CampaingId = campaingId ?? 0;
            result.Campaigns = campaings;
            return View(result);
        }

        private CampaignStatisticsModel GetCampaignStatistics(int campaingId)
        {
            var calls = this.data.CallResults.All()
                           .Where(c => c.CampaignId == campaingId);
            int callsCount = calls.Count();
            int statusSoldId = this.data.Statuses.All()
                               .Where(s => s.Description == "Sold")
                               .FirstOrDefault()
                               .StatusId;
            var soldCalls = calls.Where(c => c.StatusId == statusSoldId);
            int soldCount = soldCalls.Count();
            CampaignStatisticsModel campaignStatistics = new CampaignStatisticsModel();
            campaignStatistics.SoldToTotalCalls = (decimal)soldCount / callsCount;
            int callsTotalTime = calls.Sum(c => c.Duration);
            campaignStatistics.SoldToTotalTime = (decimal)soldCount / callsTotalTime;
            decimal price = this.data.Campaigns.Find(campaingId).Price * soldCount;
            campaignStatistics.TotelSoldPriceToTotalTime = callsTotalTime / price;
            int notAnswered = calls.Where(c => c.Status.Description == "No answer")
                                  .Count();
            campaignStatistics.NotAnsweredToTotalCalls = (decimal)notAnswered / callsCount;
            return campaignStatistics;
        }
    }
}
