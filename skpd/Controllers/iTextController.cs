using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using skpd.Models;
using skpd.Services;
using System.Web.Routing;
using System.Data.Entity;
using System.Text;
using System.IO;
using skpd.Codaxy.WkHtmlToPdf;
using System.Web.UI;

namespace skpd.Controllers
{

    public class iTextController : Controller
    {
        //
        // GET: /iText/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult sa()
        {
            return View();
        }

        public ActionResult wkhtml()
        {
            MemoryStream memory = new MemoryStream();
            PdfDocument document = new PdfDocument() { Url = "http://localhost:52858/paging/Index/2?currentFilter=kadiv" };
            PdfOutput output = new PdfOutput() { OutputStream = memory };

            PdfConvert.ConvertHtmlToPdf(document, output);
            memory.Position = 0;

            return File(memory, "application/pdf", Server.UrlEncode("dsdas.pdf"));
        }

        //
        // GET: /iText/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /iText/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /iText/Create

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
        // GET: /iText/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /iText/Edit/5

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
        // GET: /iText/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /iText/Delete/5

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

        public ActionResult xls()
        {
            using (ESKAPEDEContext db = new ESKAPEDEContext())
            {
                IQueryable<vwRequest> results = db.vwRequests;

                System.Web.UI.WebControls.GridView grd = new System.Web.UI.WebControls.GridView();
                grd.DataSource = db.vwRequests.ToList();
                grd.DataBind();

                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment; filename=ReportSKPD.xls");
                Response.ContentType = "application/ms-excel";

                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);

                // Render the grid contents => the writer objects => Response object
                grd.RenderControl(htw);
                Response.Write(sw.ToString());

                Response.End();
                return View("Index"); 
            }
        }

    }
}
