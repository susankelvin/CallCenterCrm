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
    using CallCenterCrm.Web.Areas.Operator.Models;
    using Microsoft.AspNet.Identity;
    using AutoMapper;
    using CallCenterCrm.Web.Areas.Operator.Models.CallResult;

    public class CallResultsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
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
            var calls = this.data.CallResults.All()
                            .ToList();
            return View(calls);
        }

        // GET: Operator/CallResults/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CallResult callResult = db.CallResults.Find(id);
            if (callResult == null)
            {
                return HttpNotFound();
            }
            return View(callResult);
        }

        // GET: Operator/CallResults/Create
        public ActionResult Create()
        {
            var statuses = this.data.Statuses.All()
                               .Select(s => new SelectListItem()
                               {
                                   Text = s.Description,
                                   Value = s.StatusId.ToString()
                               })
                               .ToList();

            string operatorId = this.User.Identity.GetUserId();
            ApplicationUser user = this.data.Users.Find(operatorId);
            int officeId = user.OfficeId ?? 0;

            var campaigns = this.data.Campaigns.All()
                                .Where(c => c.OfficeId == officeId)
                                .Select(c => new SelectListItem()
                                {
                                    Text = c.Description,
                                    Value = c.CampaignId.ToString()
                                })
                                .ToList();

            NewCallResultModel model = new NewCallResultModel()
            {
                Campaigns = campaigns,
                Statuses = statuses,
                CallDate = DateTime.Now
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
                CallResult callResult = new CallResult();
                Mapper.Map(model, callResult);
                callResult.OperatorId = this.User.Identity.GetUserId();
                this.data.CallResults.Add(callResult);
                this.data.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: Operator/CallResults/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CallResult callResult = db.CallResults.Find(id);
            if (callResult == null)
            {
                return HttpNotFound();
            }
            //ViewBag.CampaignId = new SelectList(db.Campaigns, "CampaignId", "Product", callResult.CampaignId);
            //ViewBag.OperatorId = new SelectList(db.ApplicationUsers, "Id", "Email", callResult.OperatorId);
            //ViewBag.StatusId = new SelectList(db.Statuses, "StatusId", "Description", callResult.StatusId);
            return View(callResult);
        }

        // POST: Operator/CallResults/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CallResultId,CampaignId,OperatorId,StatusId,CallDate,Duration,Notes,Customer,IsDeleted,DeletedOn")]
                                 CallResult callResult)
        {
            if (ModelState.IsValid)
            {
                db.Entry(callResult).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.CampaignId = new SelectList(db.Campaigns, "CampaignId", "Product", callResult.CampaignId);
            //ViewBag.OperatorId = new SelectList(db.ApplicationUsers, "Id", "Email", callResult.OperatorId);
            //ViewBag.StatusId = new SelectList(db.Statuses, "StatusId", "Description", callResult.StatusId);
            return View(callResult);
        }

        // GET: Operator/CallResults/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CallResult callResult = db.CallResults.Find(id);
            if (callResult == null)
            {
                return HttpNotFound();
            }
            return View(callResult);
        }

        // POST: Operator/CallResults/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CallResult callResult = db.CallResults.Find(id);
            db.CallResults.Remove(callResult);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}