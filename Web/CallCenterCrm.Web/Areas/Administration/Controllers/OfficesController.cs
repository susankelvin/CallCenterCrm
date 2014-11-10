namespace CallCenterCrm.Web.Areas.Administration.Controllers
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
    using CallCenterCrm.Data.Models;

    public class OfficesController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();

        // GET: Administration/Offices
        public ActionResult Index()
        {
            var offices = context.Offices.Include(o => o.Manager);
            return View(offices.ToList());
        }

        // GET: Administration/Offices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Office office = context.Offices.Find(id);
            if (office == null)
            {
                return HttpNotFound();
            }
            return View(office);
        }

        // GET: Administration/Offices/Create
        public ActionResult Create()
        {
            ViewBag.ManagerId = new SelectList(context.Users, "Id", "Email");
            return View();
        }

        // POST: Administration/Offices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OfficeId,Name,ManagerId,Address,PhoneNumber,Email")] Office office)
        {
            if (ModelState.IsValid)
            {
                context.Offices.Add(office);
                context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ManagerId = new SelectList(context.Users, "Id", "Email", office.ManagerId);
            return View(office);
        }

        // GET: Administration/Offices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Office office = context.Offices.Find(id);
            if (office == null)
            {
                return HttpNotFound();
            }
            ViewBag.ManagerId = new SelectList(context.Users, "Id", "Email", office.ManagerId);
            return View(office);
        }

        // POST: Administration/Offices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OfficeId,Name,ManagerId,Address,PhoneNumber,Email")] Office office)
        {
            if (ModelState.IsValid)
            {
                context.Entry(office).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ManagerId = new SelectList(context.Users, "Id", "Email", office.ManagerId);
            return View(office);
        }

        // GET: Administration/Offices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Office office = context.Offices.Find(id);
            if (office == null)
            {
                return HttpNotFound();
            }
            return View(office);
        }

        // POST: Administration/Offices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Office office = context.Offices.Find(id);
            context.Offices.Remove(office);
            context.SaveChanges();
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
