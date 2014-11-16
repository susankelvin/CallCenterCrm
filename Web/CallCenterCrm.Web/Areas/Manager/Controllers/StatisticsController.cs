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
    using CallCenterCrm.Data.Models;
    using Microsoft.AspNet.Identity.EntityFramework;

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
            result.OperatorsStatistics = this.GetOperatorStatistics(campaingId ?? 0);
            return View(result);
        }

        private CampaignStatisticsModel GetCampaignStatistics(int campaignId)
        {
            var calls = this.data.CallResults.All()
                            .Where(c => c.CampaignId == campaignId);
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
            decimal price = this.data.Campaigns.Find(campaignId).Price * soldCount;
            campaignStatistics.TotelSoldPriceToTotalTime = callsTotalTime / price;
            int notAnswered = calls.Where(c => c.Status.Description == "No answer")
                                  .Count();
            campaignStatistics.NotAnsweredToTotalCalls = (decimal)notAnswered / callsCount;
            return campaignStatistics;
        }

        private IEnumerable<OperatorStatisticsModel> GetOperatorStatistics(int campaignId)
        {
            List<OperatorStatisticsModel> result = new List<OperatorStatisticsModel>();
            Campaign campaign = this.data.Campaigns.Find(campaignId);
            int officeId = campaign.OfficeId;
            string roleOperatorId = this.data.Context.Roles.First(r => r.Name == "Operator").Id;
            var operators = this.data.Users.All()
                                .Where(o => o.OfficeId == officeId)
                                .Where(o => o.Roles.Any(r => r.RoleId == roleOperatorId))
                                .ToList();
            foreach (var user in operators)
            {
                string userId = user.Id;
                var userCalls = this.data.CallResults.All()
                                    .Include("Status")
                                    .Where(c => c.OperatorId == userId)
                                    .ToList();
                int soldCount = userCalls.Where(c => c.Status.Description == "Sold")
                                    .Count();
                int totalTime = userCalls.Sum(c => c.Duration);
                var campaignCalls = userCalls.Where(c => c.CampaignId == campaignId)
                                        .ToList();
                int soldCampaign = campaignCalls.Where(c => c.Status.Description == "Sold")
                                       .Count();
                int campaignTime = campaignCalls
                                       .Sum(c => c.Duration);
                int campaignCallsCount = campaignCalls.Count();
                decimal averageTime = campaignCallsCount != 0 ? (decimal)campaignTime / campaignCallsCount : 0;                
                OperatorStatisticsModel operatorStatistics = new OperatorStatisticsModel()
                {
                    Name = user.UserName,
                    SoldToTotalTimeAll = soldCount != 0 ? (decimal)totalTime / soldCount : 0,
                    SoldToTotalTimeCampaign = soldCampaign != 0 ? (decimal)campaignTime / soldCampaign : 0,
                    AverageDuration = averageTime
                };
                result.Add(operatorStatistics);
            }

            return result;
        }
    }
}