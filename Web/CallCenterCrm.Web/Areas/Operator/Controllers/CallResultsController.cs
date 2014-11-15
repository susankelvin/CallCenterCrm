namespace CallCenterCrm.Web.Areas.Operator.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Web;
    using System.Web.Mvc;
    using CallCenterCrm.Data;
    using CallCenterCrm.Data.Models;
    using Microsoft.AspNet.Identity;
    using AutoMapper;
    using CallCenterCrm.Web.Areas.Operator.Models.CallResult;
    using AutoMapper.QueryableExtensions;
    using Kendo.Mvc.UI;
    using Kendo.Mvc.Extensions;

    [Authorize]
    public class CallResultsController : Controller
    {
        private readonly ICallCenterCrmData data;

        public CallResultsController(ICallCenterCrmData data) : base()
        {
            this.data = data;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        }

        // GET: Operator/CallResults
        public ActionResult Index()
        {
            string operatorId = this.User.Identity.GetUserId();
            var calls = this.data.CallResults.All()
                            .Where(c => c.OperatorId == operatorId)
                            .Project()
                            .To<IndexCallResultModel>()
                            .ToList();
            return View(calls);
        }

        // AJAX update
        [HttpPost]
        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            string operatorId = this.User.Identity.GetUserId();
            var callResults = this.data.CallResults.All()
                                  .Where(c => c.OperatorId == operatorId)
                                  .Project()
                                  .To<IndexCallResultModel>()
                                  .ToList();
            return Json(callResults.ToDataSourceResult(request));
        }

        // GET: Operator/CallResults/Create
        public ActionResult Create()
        {
            var statuses = GetStatuses();
            var campaigns = GetCampaigns();
            NewCallResultModel model = new NewCallResultModel()
            {
                Campaigns = campaigns,
                Statuses = statuses
            };
            return View(model);
        }

        // POST: Operator/CallResults/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NewCallResultModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Duration <= 0)
                {
                    this.TempData["ErrorMessage"] = "Duration must be positive";
                    var statuses = GetStatuses();
                    var campaigns = GetCampaigns();
                    model.Statuses = statuses;
                    model.Campaigns = campaigns;
                    return View(model);
                }

                CallResult callResult = new CallResult();
                Mapper.Map(model, callResult);
                callResult.OperatorId = this.User.Identity.GetUserId();
                callResult.CallDate = DateTime.Now;
                this.data.CallResults.Add(callResult);
                this.data.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        private List<SelectListItem> GetStatuses()
        {
            var statuses = this.data.Statuses.All()
                               .Select(s => new SelectListItem()
                                      {
                                          Text = s.Description,
                                          Value = s.StatusId.ToString()
                                      })
                               .ToList();
            return statuses;
        }

        private List<SelectListItem> GetCampaigns()
        {
            string operatorId = this.User.Identity.GetUserId();
            ApplicationUser user = this.data.Users.Find(operatorId);
            int officeId = user.OfficeId ?? 0;
            var campaigns = this.data.Campaigns.All()
                                .Where(c => c.OfficeId == officeId && c.Active)
                                .Select(c => new SelectListItem()
                                       {
                                           Text = c.Description,
                                           Value = c.CampaignId.ToString()
                                       })
                                .ToList();
            return campaigns;
        }
    }
}
