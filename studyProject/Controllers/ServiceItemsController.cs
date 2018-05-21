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

namespace studyProject.Controllers
{
    public class ServiceItemsController : Controller
    {
        private ApplicationDbContext _applicationbContext = new ApplicationDbContext();

        // GET: ServiceItems
        public async Task<ActionResult> Index()
        {
            return View(await _applicationbContext.ServiceItems.ToListAsync());
        }

        // GET: ServiceItems/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceItem serviceItem = await _applicationbContext.ServiceItems.FindAsync(id);
            if (serviceItem == null)
            {
                return HttpNotFound();
            }
            return View(serviceItem);
        }

        // GET: ServiceItems/Create
        public ActionResult Create()
        {
            return View();
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ServiceItemId,ServiceItemCode,ServiceItemName,Rate")] ServiceItem serviceItem)
        {
            if (ModelState.IsValid)
            {
                _applicationbContext.ServiceItems.Add(serviceItem);
                await _applicationbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(serviceItem);
        }

       
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceItem serviceItem = await _applicationbContext.ServiceItems.FindAsync(id);
            if (serviceItem == null)
            {
                return HttpNotFound();
            }
            return View(serviceItem);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ServiceItemId,ServiceItemCode,ServiceItemName,Rate")] ServiceItem serviceItem)
        {
            if (ModelState.IsValid)
            {
                _applicationbContext.Entry(serviceItem).State = EntityState.Modified;
                await _applicationbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(serviceItem);
        }

        // GET: ServiceItems/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceItem serviceItem = await _applicationbContext.ServiceItems.FindAsync(id);
            if (serviceItem == null)
            {
                return HttpNotFound();
            }
            return View(serviceItem);
        }

        // POST: ServiceItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ServiceItem serviceItem = await _applicationbContext.ServiceItems.FindAsync(id);
            _applicationbContext.ServiceItems.Remove(serviceItem);
            await _applicationbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _applicationbContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
