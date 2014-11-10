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

    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();

        // GET: Administration/Users
        public ActionResult Index()
        {
            var x = context.Users.Include("Office").OrderBy(u => u.Email).Skip(2).First();
            var users = context.Users.Include("Office").Select(UserViewModel.FromUser).ToList();
            foreach (var user in users)
            {
                IdentityRole role = context.Roles.Find(user.Role);
                user.Role = role != null ? role.Name : "";
            }
           
            return View(users);
        }

        // GET: Administration/Users/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = context.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // GET: Administration/Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Administration/Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,OfficeId,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                context.Users.Add(applicationUser);
                context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(applicationUser);
        }

        // GET: Administration/Users/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = context.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            List<SelectListItem> offices = context.Offices.Select(o => new SelectListItem()
            {
                Text = o.Name,
                Value = o.OfficeId.ToString()
            }).ToList();

            List<SelectListItem> roles = context.Roles.Select(r => new SelectListItem()
            {
                Text = r.Name,
                Value = r.Id
            }).ToList();

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
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = this.context.Users.Find(model.Id);
                if (user == null)
                {
                    this.TempData["ErrorMessage"] = "Invalid user id";
                    this.RedirectToAction("Index");
                }

                var role = this.context.Roles.Find(model.RoleId);
                if (role == null)
                {
                    this.TempData["ErrorMessage"] = "Invalid role";
                    this.RedirectToAction("Index");
                }

                user.Roles.Clear();

                user.PhoneNumber = model.PhoneNumber;
                user.OfficeId = model.OfficeId;
                this.context.Entry(user).State = EntityState.Modified;
                context.SaveChanges();

                var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                userManager.AddToRole(user.Id, role.Name);

                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: Administration/Users/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = context.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // POST: Administration/Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ApplicationUser applicationUser = context.Users.Find(id);
            context.Users.Remove(applicationUser);
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
