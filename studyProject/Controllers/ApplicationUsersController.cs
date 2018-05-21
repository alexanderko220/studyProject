using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using studyProject.DataLayer;
using studyProject.Models;
using Microsoft.AspNet.Identity.Owin;

namespace studyProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ApplicationUsersController : Controller
    {
        //private ApplicationDbContext db = new ApplicationDbContext();

        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        public ApplicationUsersController() { }
        public ApplicationUsersController(ApplicationUserManager userManger, ApplicationRoleManager roleManager)
        {
            UserManager = userManger;
            RoleManager = roleManager;

        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            set
            {
                _userManager = value;
            }
        }

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            set
            {
                _roleManager = value;
            }
        }


        // GET: ApplicationUsers
        public async Task<ActionResult> Index()
        {
            return View(await UserManager.Users.ToListAsync());
        }

        //// GET: ApplicationUsers/Details/5
        //public async Task<ActionResult> Details(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ApplicationUser applicationUser = await db.ApplicationUsers.FindAsync(id);
        //    if (applicationUser == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(applicationUser);
        //}

        //// GET: ApplicationUsers/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        // POST: ApplicationUsers/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Create([Bind(Include = "Id,FirstName,LastName,Address,City,State,ZipCode,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] ApplicationUser applicationUser)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.ApplicationUsers.Add(applicationUser);
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }

        //    return View(applicationUser);
        //}

        // GET: ApplicationUsers/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = await UserManager.FindByIdAsync(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }

            var userRoles = await UserManager.GetRolesAsync(applicationUser.Id);
            applicationUser.RolesList = RoleManager.Roles.ToList().Select(r => new SelectListItem
            {
                Selected = userRoles.Contains(r.Name),
                Text = r.Name,
                Value = r.Name
            });

            return View(applicationUser);
        }

        // POST: ApplicationUsers/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id")] ApplicationUser applicationUser, params string[] rolesSelectedOnView)
        {
            if (ModelState.IsValid)
            {
                // If the user is currently stored having the Admin role,
                var rolesCurrentlyPersistedForUser = await UserManager.GetRolesAsync(applicationUser.Id);
                bool isThisUserAnAdmin = rolesCurrentlyPersistedForUser.Contains("Admin");

                // and the user did not have the Admin role checked,
                rolesSelectedOnView = rolesSelectedOnView ?? new string[] { };
                bool isThisUserAdminDeselected = !rolesSelectedOnView.Contains("Admin");

                // and the current stored count of users with the Admin role == 1,
                var role = await RoleManager.FindByNameAsync("Admin");
                bool isOnlyOneUserAnAdmin = role.Users.Count == 1;

                // (populate the roles list in case we have to return to the Edit view)
                applicationUser = await UserManager.FindByIdAsync(applicationUser.Id);
                applicationUser.RolesList = RoleManager.Roles.ToList().Select(x => new SelectListItem()
                {
                    Selected = rolesCurrentlyPersistedForUser.Contains(x.Name),
                    Text = x.Name,
                    Value = x.Name
                });
                // then prevent the removal of the Admin role.
                if (isThisUserAnAdmin && isThisUserAdminDeselected && isOnlyOneUserAnAdmin)
                {
                    ModelState.AddModelError("", "At least one user must retain the Admin role; you are attempting to delete the Admin role from the last user who has been assigned to it.");
                    return View(applicationUser);
                }

                var result = await UserManager.AddToRolesAsync(
                    applicationUser.Id,
                    rolesSelectedOnView.Except(rolesCurrentlyPersistedForUser).ToArray());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View(applicationUser);
                }

                result = await UserManager.RemoveFromRolesAsync(
                    applicationUser.Id,
                    rolesCurrentlyPersistedForUser.Except(rolesSelectedOnView).ToArray());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View(applicationUser);
                }

                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Something failed.");
            return View(applicationUser);
        }

        //// GET: ApplicationUsers/Delete/5
        //public async Task<ActionResult> Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ApplicationUser applicationUser = await db.ApplicationUsers.FindAsync(id);
        //    if (applicationUser == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(applicationUser);
        //}

        //// POST: ApplicationUsers/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(string id)
        //{
        //    ApplicationUser applicationUser = await db.ApplicationUsers.FindAsync(id);
        //    db.ApplicationUsers.Remove(applicationUser);
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}


        public async Task<ActionResult> LockAccount([Bind(Include = "Id")] string id)
        {
            await UserManager.ResetAccessFailedCountAsync(id);
            await UserManager.SetLockoutEndDateAsync(id, DateTime.UtcNow.AddYears(100));
            return RedirectToAction("Index");
        }


        public async Task<ActionResult> UnlockAccount([Bind(Include = "Id")] string id)
        {
            await UserManager.ResetAccessFailedCountAsync(id);
            await UserManager.SetLockoutEndDateAsync(id, DateTime.UtcNow.AddYears(-1));
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                UserManager.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
