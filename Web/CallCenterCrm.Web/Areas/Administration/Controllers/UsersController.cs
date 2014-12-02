namespace CallCenterCrm.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using CallCenterCrm.Data;
    using CallCenterCrm.Data.Models;
    using CallCenterCrm.Web.Areas.Administration.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity.Owin;
    using AutoMapper.QueryableExtensions;

    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private const int PAGE_SIZE = 10;
        private readonly ICallCenterCrmData data;

        public UsersController(ICallCenterCrmData data)
        {
            this.data = data;
        }

        // GET: Administration/Users
        public ActionResult Index()
        {
            var users = this.FilterUsers("", 0);
            return View(users);
        }

        // Ajax update
        public ActionResult Update(string tbSearch, int? pageIndex)
        {
            var result = FilterUsers(tbSearch, pageIndex);
            return PartialView("_UsersTable", result);
        }

        // GET: Administration/Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // GET: Administration/Users/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            ApplicationUser user = this.data.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var offices = this.GetOffices();
            var roles = this.GetRoles();
            string roleId = user.Roles.First().RoleId;
            EditUserViewModel model = new EditUserViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                OfficeId = user.OfficeId ?? 0,
                Offices = offices,
                RoleId = roleId,
                Roles = roles
            };
            return View(model);
        }

        // POST: Administration/Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = this.data.Users.Find(model.Id);
                if (user == null)
                {
                    this.TempData["ErrorMessage"] = "Invalid user id";
                    this.RedirectToAction("Index");
                }

                var role = this.data.Context.Roles.Find(model.RoleId);
                if (role == null)
                {
                    this.TempData["ErrorMessage"] = "Invalid role";
                    this.RedirectToAction("Index");
                }

                Office office = this.data.Offices.Find(model.OfficeId);
                if (office == null)
                {
                    this.TempData["ErrorMessage"] = "Invalid office";
                    this.RedirectToAction("Index");
                }

                model.Email = model.Email.ToLower().Trim();
                ApplicationUser userWithEmail = this.data.Users.All()
                    .FirstOrDefault(u => u.Email == model.Email);
                if ((userWithEmail != null) && (userWithEmail.Id != user.Id))
                {
                    this.TempData["ErrorMessage"] = "Email is already taken by another user";
                    this.RedirectToAction("Index");
                }

                user.Roles.Clear();
                user.PhoneNumber = model.PhoneNumber;
                user.OfficeId = model.OfficeId;
                this.data.Users.Update(user);
                this.data.SaveChanges();

                var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                userManager.AddToRole(user.Id, role.Name);

                return RedirectToAction("Index");
            }

            return View(model);
        }

        // POST: Administration/Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ApplicationUser applicationUser = this.data.Users.Find(id);
            this.data.Users.Delete(applicationUser);
            this.data.SaveChanges();
            return RedirectToAction("Index");
        }

        private IndexViewModel FilterUsers(string tbSearch, int? pageIndex)
        {
            string search = tbSearch.ToLower();
            var users = this.data.Users.All()
                .Include("Office")
                .Where(u => u.UserName.Contains(search) || u.Email.Contains(search));

            int usersCount = users.Count();
            int pageCount = usersCount / PAGE_SIZE;
            if (usersCount % PAGE_SIZE != 0)
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

            var usersList = users.OrderBy(u => u.Id)
                .Skip((int)pageIndex * PAGE_SIZE)
                .Take(PAGE_SIZE)
                .Select(UserViewModel.FromUser)
                .ToList();
            foreach (var user in usersList)
            {
                IdentityRole role = this.data.Context.Roles.Find(user.Role);
                user.Role = role != null ? role.Name : "";
            }

            IndexViewModel result = new IndexViewModel()
            {
                ActivePage = pageIndex ?? 0,
                PageCount = pageCount,
                Users = usersList
            };
            return result;
        }

        private List<SelectListItem> GetOffices()
        {
            List<SelectListItem> offices = this.data.Offices.All()
                .Select(o => new SelectListItem()
                {
                    Text = o.Name,
                    Value = o.OfficeId.ToString()
                })
                .ToList();
            offices.Insert(0, new SelectListItem()
            {
                Selected = true,
                Text = "Select office",
                Value = "0"
            });
            return offices;
        }

        private List<SelectListItem> GetRoles()
        {
            List<SelectListItem> roles = this.data.Context.Roles
                .Select(r => new SelectListItem()
                {
                    Text = r.Name,
                    Value = r.Id
                })
                .ToList();
            return roles;
        }
    }
}