using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using skpd.Codaxy.WkHtmlToPdf;
using skpd.Models;
using System.Web.Routing;
using System.Configuration;

namespace skpd.Controllers
{
    public class PrintPageController : Controller
    {
        //
        // GET: /PrintRequest/
        #region Initialize RequestContext
        public ESKAPEDEContext db { get; set; }
        protected override void Initialize(RequestContext requestContext)
        {
            if (db == null) { db = new ESKAPEDEContext(); }
            base.Initialize(requestContext);
        }
        #endregion

        public ActionResult Print(int id)
        {
            RequestRelease printRequest = db.RequestReleases
                                        .Where(a => a.RequestID == id).SingleOrDefault();

            if (printRequest == null)
            {
                return RedirectToAction("PerjadinNotRelease");
            }
            else
            {
                MemoryStream memory = new MemoryStream();
                var PrintPageUrl = ConfigurationManager.AppSettings["PrintRequest"];
                PdfDocument document = new PdfDocument() { Url = PrintPageUrl + id };
                PdfOutput output = new PdfOutput() { OutputStream = memory };

                PdfConvert.ConvertHtmlToPdf(document, output);
                memory.Position = 0;

                return File(memory, "application/pdf", Server.UrlEncode("E-SKPD.pdf"));
            }
        }

        public ViewResult PerjadinRelease(int id)
        {
            RequestRelease printRequest = db.RequestReleases
                                        .Where(a => a.RequestID == id).SingleOrDefault();
            return View(printRequest);
        }

        public ViewResult PerjadinNotRelease()
        {
            return View();
        }

    }
}
