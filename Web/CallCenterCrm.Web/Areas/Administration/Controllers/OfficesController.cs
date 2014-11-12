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
    using CallCenterCrm.Web.Areas.Administration.Models.Office;

    [Authorize(Roles="Admin")]
    public class OfficesController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();

        // GET: Administration/Offices
        public ActionResult Index()
        {
            var offices = context.Offices.Include(o => o.Manager).Select(o =>  new IndexOfficeViewModel()
            {
                Address = o.Address,
                Email = o.Email,
                ManagerName = o.Manager.UserName,
                Name = o.Name,
                OfficeId = o.OfficeId,
                PhoneNumber = o.PhoneNumber
            });
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
            NewOfficeViewModel model = new NewOfficeViewModel();
            IEnumerable<SelectListItem> managers = GetManagers();
            model.Managers = managers;
            return View(model);
        }


        // POST: Administration/Offices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NewOfficeViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser manager = this.context.Users.Find(model.ManagerId);
                if (manager == null)
                {
                    this.TempData["ErrorMessage"] = "Invalid manager";
                    this.RedirectToAction("Index");
                }

                Office office = new Office()
                {
                    Address = model.Address,
                    Email = model.Email,
                    Name = model.Name,
                    PhoneNumber = model.PhoneNumber,
                    ManagerId = model.ManagerId
                };
                context.Offices.Add(office);
                context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
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

            EditOfficeViewModel model = new EditOfficeViewModel()
            {
                OfficeId = office.OfficeId,
                Address = office.Address,
                Email = office.Email,
                Name = office.Name,
                PhoneNumber = office.PhoneNumber,
                //ManagerId = office.Manager.Id,
                Managers = this.GetManagers()
            };
            
            return View(model);
        }

        // POST: Administration/Offices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditOfficeViewModel model)
        {
            if (ModelState.IsValid)
            {
                Office office = this.context.Offices.Find(model.OfficeId);
                if (office == null)
                {
                    this.TempData["ErrorMessage"] = "Invalid office id";
                    this.RedirectToAction("Index");
                }

                office.Address = model.Address;
                office.Email = model.Email;
                office.ManagerId = model.ManagerId;
                office.PhoneNumber = model.PhoneNumber;
                context.Entry(office).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
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

        private IEnumerable<SelectListItem> GetManagers()
        {
            string roleManagerId = this.context.Roles.Where(r => r.Name == "Manager").First().Id;
            List<SelectListItem> managers = this.context.Users.Where(u => u.Roles.FirstOrDefault().RoleId == roleManagerId)
                                                .Select(u => new SelectListItem()
                                                {
                                                    Text = u.UserName,
                                                    Value = u.Id
                                                })
                                                .ToList();
            return managers;
        }
    }
}