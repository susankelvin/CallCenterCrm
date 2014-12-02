using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace CallCenterCrm.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using CallCenterCrm.Data;
    using CallCenterCrm.Data.Models;
    using CallCenterCrm.Web.Areas.Administration.Models.Office;

    [Authorize(Roles = "Admin")]
    public class OfficesController : Controller
    {
        private const int PAGE_SIZE = 10;
        private readonly ICallCenterCrmData data;

        public OfficesController(ICallCenterCrmData data)
        {
            this.data = data;
        }

        // GET: Administration/Offices
        public ActionResult Index()
        {
            var offices = this.FilterOffices("", 0);
            return View(offices);
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
                ApplicationUser manager = this.data.Users.Find(model.ManagerId);
                if (manager == null)
                {
                    this.TempData["ErrorMessage"] = "Invalid manager";
                    this.RedirectToAction("Index");
                }

                Office office = new Office();
                Mapper.Map(model, office);
                this.data.Offices.Add(office);
                this.data.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: Administration/Offices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Office office = this.data.Offices.Find(id);
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
                Office office = this.data.Offices.Find(model.OfficeId);
                if (office == null)
                {
                    this.TempData["ErrorMessage"] = "Invalid office id";
                }

                ApplicationUser manager = this.data.Users.All()
                    .FirstOrDefault(m => m.Id == model.ManagerId);
                if (manager != null)
                {
                    var userManager = this.HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                    if (!userManager.IsInRole(manager.Id, "Manager"))
                    {
                        this.TempData["ErrorMessage"] = "Invalid manager";
                    }
                }
                else
                {
                    this.TempData["ErrorMessage"] = "Invalid manager";
                }

                if (this.TempData["ErrorMessage"] == null)
                {
                    Mapper.Map(model, office);
                    this.data.Offices.Update(office);
                    this.data.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            model.Managers = this.GetManagers();
            return View(model);
        }

        // POST: Administration/Offices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Office office = this.data.Offices.Find(id);
            if (office != null)
            {
                this.data.Offices.Delete(office);
                this.data.SaveChanges();
            }
            else
            {
                this.TempData["ErrorMessage"] = "Invalid office id";
            }

            return RedirectToAction("Index");
        }

        private IEnumerable<SelectListItem> GetManagers()
        {
            string roleManagerId = this.data.Context.Roles.First(r => r.Name == "Manager").Id;
            List<SelectListItem> managers = this.data.Users.All()
                .Where(u => u.Roles.FirstOrDefault().RoleId == roleManagerId)
                .Select(u => new SelectListItem()
                {
                    Text = u.UserName,
                    Value = u.Id
                })
                .ToList();
            managers.Insert(0, new SelectListItem()
            {
                Text = "Select manager",
                Value = "0"
            });
            return managers;
        }

        private IndexViewModel FilterOffices(string tbSearch, int? pageIndex)
        {
            string search = tbSearch.ToLower();
            var offices = this.data.Offices.All()
                .Include(o => o.Manager)
                .Where(o => o.Name.Contains(search) || o.Manager.UserName.Contains(search));

            int officesCount = offices.Count();
            int pageCount = officesCount / PAGE_SIZE;
            if (officesCount % PAGE_SIZE != 0)
            {
                pageCount++;
            }

            if ((pageIndex == null) || (pageIndex < 0))
            {
                pageIndex = 0;
            }

            if ((pageIndex > 0) && (pageIndex >= pageCount))
            {
                pageIndex = pageCount - 1;
            }

            var officesList = offices.OrderBy(o => o.OfficeId)
                .Skip((int)pageIndex * PAGE_SIZE)
                .Take(PAGE_SIZE)
                .Project()
                .To<IndexOfficeViewModel>()
                .ToList();

            IndexViewModel result = new IndexViewModel()
            {
                ActivePage = pageIndex ?? 0,
                PageCount = pageCount,
                Offices = officesList
            };
            return result;
        }
    }
}