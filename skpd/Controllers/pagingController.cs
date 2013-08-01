using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Calabonga.Mvc.PagedListExt;
using skpd.Models;

namespace skpd.Controllers
{
    public class pagingController : Controller
    {
        //
        // GET: /paging/
        public ViewResult Index(int? id, string currentFilter, string searchString)
        {
            if (Request.HttpMethod == "GET")
            {
                searchString = currentFilter;
            }
            else
            {
                id = 1;
            }

            ViewBag.CurrentFilter = searchString;

            int currentPage = id ?? 1;
            ViewBag.Message = "Position List";
            using (var db = new ESKAPEDEContext())
            {
                var positions = db.Positions.ToList();
                if (!String.IsNullOrEmpty(searchString))
                {
                    positions = positions.Where(s => s.PositionName.ToUpper().Contains(searchString.ToUpper())).ToList();
                }

                int pageSize = 5;
                int pageNumber = (id ?? 1);

                return View(positions.OrderBy(a => a.PositionID).ToPagedList(pageNumber, pageSize));
            }
            
        }

        //
        // GET: /paging/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /paging/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /paging/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /paging/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /paging/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /paging/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /paging/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
