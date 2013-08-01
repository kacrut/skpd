using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using skpd.Models;

namespace skpd.Controllers
{
    public class ReportReleaseController : Controller
    {
        private ESKAPEDEContext db = new ESKAPEDEContext();

        //
        // GET: /ReportRelease/

        public ActionResult Index()
        {
            return View(db.RequestReleases.ToList());
        }

        //
        // GET: /ReportRelease/Details/5

        public ActionResult Details(int id = 0)
        {
            RequestRelease requestrelease = db.RequestReleases.Find(id);
            if (requestrelease == null)
            {
                return HttpNotFound();
            }
            return View(requestrelease);
        }

        //
        // GET: /ReportRelease/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /ReportRelease/Create

        [HttpPost]
        public ActionResult Create(RequestRelease requestrelease)
        {
            if (ModelState.IsValid)
            {
                db.RequestReleases.Add(requestrelease);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(requestrelease);
        }

        //
        // GET: /ReportRelease/Edit/5

        public ActionResult Edit(int id = 0)
        {
            RequestRelease requestrelease = db.RequestReleases.Find(id);
            if (requestrelease == null)
            {
                return HttpNotFound();
            }
            return View(requestrelease);
        }

        //
        // POST: /ReportRelease/Edit/5

        [HttpPost]
        public ActionResult Edit(RequestRelease requestrelease)
        {
            if (ModelState.IsValid)
            {
                db.Entry(requestrelease).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(requestrelease);
        }

        //
        // GET: /ReportRelease/Delete/5

        public ActionResult Delete(int id = 0)
        {
            RequestRelease requestrelease = db.RequestReleases.Find(id);
            if (requestrelease == null)
            {
                return HttpNotFound();
            }
            return View(requestrelease);
        }

        //
        // POST: /ReportRelease/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            RequestRelease requestrelease = db.RequestReleases.Find(id);
            db.RequestReleases.Remove(requestrelease);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}