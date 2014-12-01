namespace CallCenterCrm.Web.Areas.Operator.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Web.Mvc;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using CallCenterCrm.Data;
    using CallCenterCrm.Data.Models;
    using CallCenterCrm.Web.Areas.Operator.Models.CallResult;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using Microsoft.AspNet.Identity;

    [Authorize(Roles = "Operator")]
    public class CallResultsController : Controller
    {
        private readonly ICallCenterCrmData data;

        public CallResultsController(ICallCenterCrmData data)
            : base()
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
        public ActionResult Read([DataSourceRequest] DataSourceRequest request)
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
            NewCallResultModel model = new NewCallResultModel();
            this.SetCampaignsAndStatuses(model);
            return View(model);
        }

        // POST: Operator/CallResults/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NewCallResultModel model)
        {
            if (ModelState.IsValid)
            {
                if (this.data.Statuses.Find(model.StatusId) == null)
                {
                    this.TempData["ErrorMessage"] = "Invalid status";
                }

                if (this.GetCampaignsForUser().FirstOrDefault(c => c.CampaignId == model.CampaignId) == null)
                {
                    this.TempData["ErrorMessage"] = "Invalid campaign";
                }

                if (this.TempData["ErrorMessage"] != null)
                {
                    this.SetCampaignsAndStatuses(model);
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

            this.SetCampaignsAndStatuses(model);
            return View(model);
        }

        private void SetCampaignsAndStatuses(NewCallResultModel model)
        {
            model.Statuses = this.GetStatuses();
            model.Campaigns = this.GetCampaigns();
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
            var campaigns = GetCampaignsForUser();
            var result = campaigns.Select(c => new SelectListItem()
            {
                Text = c.Description,
                Value = c.CampaignId.ToString()
            })
                .ToList();
            return result;
        }

        private IQueryable<Campaign> GetCampaignsForUser()
        {
            string operatorId = this.User.Identity.GetUserId();
            ApplicationUser user = this.data.Users.Find(operatorId);
            int officeId = user.OfficeId ?? 0;
            var campaigns = this.data.Campaigns.All()
                .Where(c => c.OfficeId == officeId && c.Active);
            return campaigns;
        }
    }
}
