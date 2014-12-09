﻿namespace CallCenterCrm.Web.Areas.Management.Controllers
{
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    using System.Web.Routing;
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

        // GET: Manage/Campaigns
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

        // GET: Manage/Campaigns/Details/5
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

        // GET: Manage/Campaigns/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Manage/Campaigns/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NewCampaignViewModel model)
        {
            if (ModelState.IsValid && (model.StartDate < model.EndDate))
            {
                Campaign campaign = new Campaign()
                {
                    Active = model.Active,
                    Description = model.Description,
                    EndDate = model.EndDate,
                    ManagerId = this.User.Identity.GetUserId(),
                    Price = model.Price,
                    Product = model.Product,
                    Script = model.Script,
                    StartDate = model.StartDate
                };

                ApplicationUser user = this.data.Users.Find(campaign.ManagerId);
                campaign.OfficeId = user.OfficeId ?? 0;
                this.data.Campaigns.Add(campaign);
                this.data.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // POST: Manage/Campaigns/Edit/5
        [HttpPost]
        public ActionResult Edit(IndexViewModel model)
        {
            if (ModelState.IsValid)
            {
                Campaign campaign = new Campaign()
                {
                    Active = model.Active,
                    CampaignId = model.CampaignId,
                    Description = model.Description,
                    EndDate = model.EndDate,
                    ManagerId = this.User.Identity.GetUserId(),
                    Price = model.Price,
                    Product = model.Product,
                    Script = model.Script,
                    StartDate = model.StartDate
                };
                ApplicationUser user = this.data.Users.Find(campaign.ManagerId);
                campaign.OfficeId = user.OfficeId ?? 0;
                this.data.Campaigns.Update(campaign);
                this.data.SaveChanges();
            }

            RouteValueDictionary routeValues = this.GridRouteValues();
            return RedirectToAction("Index", routeValues);
        }

        // POST: Manage/Campaigns/Delete/5
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