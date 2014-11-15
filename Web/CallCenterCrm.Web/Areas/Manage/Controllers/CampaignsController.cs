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
    using Microsoft.AspNet.Identity;
    using CallCenterCrm.Data;
    using CallCenterCrm.Data.Models;
    using CallCenterCrm.Web.Areas.Manage.Models.Campaign;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using System.Threading;
    using System.Globalization;

    [Authorize(Roles = "Admin, Manager")]
    public class CampaignsController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();

        public CampaignsController() : base()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        }

        // GET: Manage/Campaigns
        public ActionResult Index([DataSourceRequest]DataSourceRequest request)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            string managerId = this.User.Identity.GetUserId();
            var campaigns = this.context.Campaigns.Where(c => c.ManagerId == managerId)
                                .Select(c => new IndexViewModel()
                                       {
                                           CampaignId = c.CampaignId,
                                           Active = c.Active,
                                           Description = c.Description,
                                           EndDate = c.EndDate,
                                           Price = c.Price,
                                           Product = c.Product,
                                           Script = c.Script,
                                           StartDate = c.StartDate
                                       })
                                .ToList();
            return Json(campaigns.ToDataSourceResult(request));
        }

        // GET: Manage/Campaigns/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Campaign campaign = context.Campaigns.Find(id);
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

                ApplicationUser user = this.context.Users.Find(campaign.ManagerId);
                campaign.OfficeId = user.OfficeId ?? 0;
                context.Campaigns.Add(campaign);
                context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: Manage/Campaigns/Edit/5
        public ActionResult Edit(int? campaignid)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //Campaign campaign = context.Campaigns.Find(id);
            //if (campaign == null)
            //{
            //    return HttpNotFound();
            //}
            //ViewBag.ManagerId = new SelectList(context.Users, "Id", "Email", campaign.ManagerId);
            return View();
        }

        // POST: Manage/Campaigns/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(IndexViewModel campaign)
        {
            //if (ModelState.IsValid)
            //{
            //    context.Entry(campaign).State = EntityState.Modified;
            //    context.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            //ViewBag.ManagerId = new SelectList(context.Users, "Id", "Email", campaign.ManagerId);
            return View(campaign);
        }

        // GET: Manage/Campaigns/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Campaign campaign = context.Campaigns.Find(id);
            if (campaign == null)
            {
                return HttpNotFound();
            }
            return View(campaign);
        }

        // POST: Manage/Campaigns/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed([DataSourceRequest]DataSourceRequest request, IndexViewModel model)
        {
            //Campaign campaign = context.Campaigns.Find(id);
            //context.Campaigns.Remove(campaign);
            //context.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}