namespace CallCenterCrm.Web.Areas.Management.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    using System.Web.Routing;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using CallCenterCrm.Data;
    using CallCenterCrm.Data.Models;
    using CallCenterCrm.Web.Areas.Management.Models.Campaign;
    using Kendo.Mvc.Extensions;
    using Microsoft.AspNet.Identity;

    [Authorize(Roles = "Admin, Manager")]
    public class CampaignsController : Controller
    {
        private readonly ICallCenterCrmData data;

        public CampaignsController(ICallCenterCrmData data)
            : base()
        {
            this.data = data;
        }

        // GET: Management/Campaigns
        public ActionResult Index()
        {
            string managerId = this.User.Identity.GetUserId();
            var campaigns = this.data.Campaigns.All()
                .Where(c => c.ManagerId == managerId)
                .Project()
                .To<IndexViewModel>()
                .ToList();
            return View(campaigns);
        }

        // GET: Management/Campaigns/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Campaign campaign = this.data.Campaigns.Find(id);
            if (campaign == null)
            {
                return HttpNotFound();
            }
            return View(campaign);
        }

        // GET: Management/Campaigns/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Management/Campaigns/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NewCampaignViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.StartDate.Date < DateTime.Now.Date)
                {
                    this.TempData["ErrorMessage"] = "Start date cannot be eariler than today";
                }
                else if (model.StartDate.Date > model.EndDate.Date)
                {
                    this.TempData["ErrorMessage"] = "End date cannot be earlier than start date";
                }
                else
                {
                    Campaign campaign = new Campaign();
                    Mapper.Map(model, campaign);
                    ApplicationUser user = this.data.Users.Find(this.User.Identity.GetUserId());
                    campaign.ManagerId = user.Id;
                    campaign.OfficeId = user.OfficeId ?? 0;
                    this.data.Campaigns.Add(campaign);
                    this.data.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(model);
        }

        // POST: Management/Campaigns/Edit/5
        [HttpPost]
        public ActionResult Edit(IndexViewModel model)
        {
            if (ModelState.IsValid)
            {
                Campaign campaign = this.data.Campaigns.Find(model.CampaignId);
                ApplicationUser user = this.data.Users.Find(this.User.Identity.GetUserId());
                if ((campaign != null) && (campaign.ManagerId == user.Id))
                {
                    Mapper.Map(model, campaign);
                    campaign.OfficeId = user.OfficeId ?? 0;
                    this.data.Campaigns.Update(campaign);
                    this.data.SaveChanges();
                }
                else
                {
                    this.TempData["ErrorMessage"] = "Invalid campaign";
                }
            }

            RouteValueDictionary routeValues = this.GridRouteValues();
            return RedirectToAction("Index", routeValues);
        }

        // POST: Management/Campaigns/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int campaignId)
        {
            Campaign campaign = this.data.Campaigns.Find(campaignId);
            if ((campaign != null) && (campaign.ManagerId == this.User.Identity.GetUserId()))
            {
                this.data.Campaigns.Delete(campaign);
                this.data.SaveChanges();
            }
            else
            {
                this.TempData["ErrorMessage"] = "Invalid campaign";
            }

            RouteValueDictionary routeValues = this.GridRouteValues();
            return this.RedirectToAction("Index", routeValues);
        }
    }
}