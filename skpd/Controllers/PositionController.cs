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
    public class PositionController : Controller
    {
        private ESKAPEDEContext db = new ESKAPEDEContext();

        //
        // GET: /Position/

        public ViewResult Index()
        {
            var positions = db.Positions
                            .Include("Unit")
                            .Include("Country")
                            .Include(p => p.Position1).Include(p => p.Position2);
            return View(positions.ToList());
        }

        //
        // GET: /Position/Details/5

        //public ViewResult Details(int id)
        //{
        //    Position position = db.Positions.Find(id);
        //    return View(position);
        //}

        //
        // GET: /Position/Create

        public ActionResult Create()
        {
            ViewBag.DivisionID = new SelectList(db.Divisions, "DivisionID", "DivisionName");
            ViewBag.PositionID = new SelectList(db.Positions, "PositionID", "PositionName");
            ViewBag.AssignID = new SelectList(db.Positions, "PositionID", "PositionName");
            return View();
        } 

        ////
        //// POST: /Position/Create

        //[HttpPost]
        //public ActionResult Create(Position position)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Positions.Add(position);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");  
        //    }

        //    ViewBag.DivisionID = new SelectList(db.Divisions, "DivisionID", "DivisionName", position.);
        //    ViewBag.PositionID = new SelectList(db.Positions, "PositionID", "PositionName", position.PositionID);
        //    ViewBag.AssignID = new SelectList(db.Positions, "PositionID", "PositionName", position.AssignID);
        //    return View(position);
        //}
        
        ////
        //// GET: /Position/Edit/5
 
        //public ActionResult Edit(int id)
        //{
        //    Position position = db.Positions.Find(id);
        //    ViewBag.DivisionID = new SelectList(db.Divisions, "DivisionID", "DivisionName", position.DivisionID);
        //    ViewBag.PositionID = new SelectList(db.Positions, "PositionID", "PositionName", position.PositionID);
        //    ViewBag.AssignID = new SelectList(db.Positions, "PositionID", "PositionName", position.AssignID);
        //    return View(position);
        //}

        ////
        //// POST: /Position/Edit/5

        //[HttpPost]
        //public ActionResult Edit(Position position)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(position).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.DivisionID = new SelectList(db.Divisions, "DivisionID", "DivisionName", position.DivisionID);
        //    ViewBag.PositionID = new SelectList(db.Positions, "PositionID", "PositionName", position.PositionID);
        //    ViewBag.AssignID = new SelectList(db.Positions, "PositionID", "PositionName", position.AssignID);
        //    return View(position);
        //}

        ////
        //// GET: /Position/Delete/5
 
        //public ActionResult Delete(int id)
        //{
        //    Position position = db.Positions.Find(id);
        //    return View(position);
        //}

        ////
        //// POST: /Position/Delete/5

        //[HttpPost, ActionName("Delete")]
        //public ActionResult DeleteConfirmed(int id)
        //{            
        //    Position position = db.Positions.Find(id);
        //    db.Positions.Remove(position);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}