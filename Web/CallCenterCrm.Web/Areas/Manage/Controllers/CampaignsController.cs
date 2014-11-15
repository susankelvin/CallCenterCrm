namespace CallCenterCrm.Web.Areas.Manage.Controllers
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Web.Mvc;
    using CallCenterCrm.Data;
    using CallCenterCrm.Data.Models;
    using CallCenterCrm.Web.Areas.Manage.Models.Campaign;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using Microsoft.AspNet.Identity;

    [Authorize(Roles = "Admin, Manager")]
    public class CampaignsController : Controller
    {
        //private ApplicationDbContext context = new ApplicationDbContext();
        private readonly ICallCenterCrmData data;

        public CampaignsController(ICallCenterCrmData data)
            : base()
        {
            this.data = data;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        }

        // GET: Manage/Campaigns
        public ActionResult Index([DataSourceRequest]DataSourceRequest request)
        {
            return View();
        }

        // AJAX update
        [HttpPost]
        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            string managerId = this.User.Identity.GetUserId();
            var campaigns = this.data.Campaigns.All().Where(c => c.ManagerId == managerId)
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
        public ActionResult Edit([DataSourceRequest]DataSourceRequest request, IndexViewModel model)
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

            return Json(new[] { model }.ToDataSourceResult(request));
        }

        // POST: Manage/Campaigns/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed([DataSourceRequest]DataSourceRequest request, IndexViewModel model)
        {
            Campaign campaign = this.data.Campaigns.Find(model.CampaignId);
            if (campaign != null)
            {
                this.data.Campaigns.Delete(campaign);
                this.data.SaveChanges();
            }

            return Json(new[] { model }.ToDataSourceResult(request));
        }
    }
}