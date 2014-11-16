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
    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    [Authorize(Roles = "Admin")]
    public class OfficesController : Controller
    {
        private const int PAGE_SIZE = 10;
        private readonly ApplicationDbContext context = new ApplicationDbContext();

        // GET: Administration/Offices
        public ActionResult Index()
        {
            var offices = this.FilterOffices("", 0);
            return View(offices.ToList());
        }

        // Ajax update
        public ActionResult Update(string tbSearch, int? pageIndex)
        {
            var result = FilterOffices(tbSearch, pageIndex);
            return PartialView("_OfficesTable", result);
        }

        // GET: Administration/Offices/Create
        public ActionResult Create()
        {
            NewOfficeViewModel model = new NewOfficeViewModel();
            IEnumerable<SelectListItem> managers = this.GetManagers();
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

                Office office = new Office();
                Mapper.Map(model, office);
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

            EditOfficeViewModel model = new EditOfficeViewModel();
            Mapper.Map(office, model);
            model.Managers = this.GetManagers();
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

                Mapper.Map(model, office);
                context.Entry(office).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // POST: Administration/Offices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Office office = context.Offices.Find(id);
            if (office != null)
            {
                context.Offices.Remove(office);
                context.SaveChanges();
            }
            else
            {
                this.TempData["ErrorMessage"] = "Invalid office id";
            }

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

        private IEnumerable<IndexOfficeViewModel> FilterOffices(string tbSearch, int? pageIndex)
        {
            string search = tbSearch.ToLower();
            var offices = context.Offices
                              .Include(o => o.Manager)
                              .Where(o => o.Name.Contains(search) || o.Manager.UserName.Contains(search));

            if ((pageIndex == null) || (pageIndex < 0))
            {
                pageIndex = 0;
            }

            if (pageIndex > 0)
            {
                int officesCount = offices.Count();
                int maxPage = officesCount / PAGE_SIZE;
                if (officesCount > (maxPage + 1) * PAGE_SIZE)
                {
                    maxPage++;
                }

                if (pageIndex > maxPage)
                {
                    pageIndex = maxPage;
                }
            }

            offices = offices.OrderBy(o => o.OfficeId)
                          .Skip((int)pageIndex * PAGE_SIZE)
                          .Take(PAGE_SIZE);
            var result = offices.Project()
                             .To<IndexOfficeViewModel>()
                             .ToList();
            return result;
        }
    }
}
