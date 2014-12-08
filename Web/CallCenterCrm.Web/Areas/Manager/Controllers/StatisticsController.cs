namespace CallCenterCrm.Web.Areas.Manage.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;
    using CallCenterCrm.Data;
    using CallCenterCrm.Data.Models;
    using CallCenterCrm.Web.Areas.Manager.Models.Statistics;

    public class StatisticsController : Controller
    {
        private readonly ICallCenterCrmData data;

        public StatisticsController(ICallCenterCrmData data)
        {
            this.data = data;
        }

        // GET: Manager/Status
        public ActionResult Index(int? campaignId)
        {
            IndexStatisticsModel result = new IndexStatisticsModel();
            if (campaignId != null)
            {
                Campaign campaign = this.data.Campaigns.Find(campaignId);
                if (campaign != null)
                {
                    CampaignStatisticsModel campaignStatistics = GetCampaignStatistics(campaignId ?? 0);
                    result.CampaignStatistics = new[] { campaignStatistics };
                    result.CampaignId = campaign.CampaignId;
                    result.OperatorsStatistics = this.GetOperatorStatistics(campaignId ?? 0);
                }
                else
                {
                    this.TempData["ErrorMessage"] = "Invalid campaign";
                }
            }

            var campaings = this.data.Campaigns.All()
                                .Select(c => new SelectListItem()
                                       {
                                           Text = c.Description,
                                           Value = c.CampaignId.ToString()
                                       })
                                .ToList();
            campaings.Insert(0, new SelectListItem()
            {
                Text = "Select campaign",
                Value = "0"
            });
            result.Campaigns = campaings;
            return View(result);
        }

        private CampaignStatisticsModel GetCampaignStatistics(int campaignId)
        {
            var calls = this.data.CallResults.All()
                            .Where(c => c.CampaignId == campaignId);
            int callsCount = calls.Count();
            int statusSoldId = this.data.Statuses.All()
                                   .First(s => s.Description == "Sold")
                                   .StatusId;
            var soldCalls = calls.Where(c => c.StatusId == statusSoldId);
            int soldCount = soldCalls.Count();
            CampaignStatisticsModel campaignStatistics = new CampaignStatisticsModel();
            campaignStatistics.SoldToTotalCalls = (decimal)soldCount / callsCount;
            int callsTotalTime = calls.Sum(c => c.Duration);
            campaignStatistics.SoldToTotalTime = (decimal)soldCount / callsTotalTime;
            decimal price = this.data.Campaigns.Find(campaignId).Price * soldCount;
            campaignStatistics.TotelSoldPriceToTotalTime = callsTotalTime / price;
            int notAnswered = calls.Count(c => c.Status.Description == "No answer");
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
                int soldCount = userCalls.Count(c => c.Status.Description == "Sold");
                int totalTime = userCalls.Sum(c => c.Duration);
                var campaignCalls = userCalls.Where(c => c.CampaignId == campaignId)
                                        .ToList();
                int soldCampaign = campaignCalls.Count(c => c.Status.Description == "Sold");
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