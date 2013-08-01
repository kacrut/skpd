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
    public class TransportController : Controller
    {
        private ESKAPEDEContext db = new ESKAPEDEContext();

        //
        // GET: /Transport/

        public ViewResult Index()
        {
            return View(db.Transports.ToList());
        }

        //
        // GET: /Transport/Details/5

        public ViewResult Details(int id)
        {
            Transport transport = db.Transports.Find(id);
            return View(transport);
        }

        //
        // GET: /Transport/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Transport/Create

        [HttpPost]
        public ActionResult Create(Transport transport)
        {
            if (ModelState.IsValid)
            {
                db.Transports.Add(transport);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(transport);
        }
        
        //
        // GET: /Transport/Edit/5
 
        public ActionResult Edit(int id)
        {
            Transport transport = db.Transports.Find(id);
            return View(transport);
        }

        //
        // POST: /Transport/Edit/5

        [HttpPost]
        public ActionResult Edit(Transport transport)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transport).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(transport);
        }

        //
        // GET: /Transport/Delete/5
 
        public ActionResult Delete(int id)
        {
            Transport transport = db.Transports.Find(id);
            return View(transport);
        }

        //
        // POST: /Transport/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Transport transport = db.Transports.Find(id);
            db.Transports.Remove(transport);
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