using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using skpd.Models;
using skpd.Services;
using System.Web.Routing;
using System.IO;
using System.Web.UI;
using skpd.DTO;
using System.Web.UI.WebControls;
using System.Text;
using System.Configuration;
using skpd.Codaxy.WkHtmlToPdf;
using System.Xml.Serialization;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Data;

namespace skpd.Controllers
{
    public class LaporanController : Controller
    {
        //
        // GET: /Laporan/

        #region Initialize RequestContext
        public ESKAPEDEContext db { get; set; }
        public IWebSecurityService WebSecurityService { get; set; }
        public IMessengerService MessengerService { get; set; }
        public IServiceSkpd ServiceSkpd { get; set; }
        protected override void Initialize(RequestContext requestContext)
        {
            if (db == null) { db = new ESKAPEDEContext(); }
            if (WebSecurityService == null) { WebSecurityService = new WebSecurityService(); }
            if (MessengerService == null) { MessengerService = new MessengerService(); }
            if (ServiceSkpd == null) { ServiceSkpd = new ServiceSkpd(); }

            base.Initialize(requestContext);
        }
        #endregion

        public ActionResult Index()
        {
            return View("Index");
        }

        [HttpPost]
        public ActionResult Index(RangeReleaseReport model)
        {
            //using (ESKAPEDEContext db = new ESKAPEDEContext())
            //{
            //    var filename = string.Format("{0}{1}-{2}", "ReportSKPD", model.Start.ToShortDateString().ToString().Replace("/", ""), model.From.ToShortDateString().Replace("/", ""));
            //    //GridView grid = new GridView();
            //    //grid.DataSource = db.ReportRequests.ToList();
            //    ////.Where(a => a.TGL_SKPD >= model.Start && a.TGL_SKPD <= model.From).ToList();
            //    //grid.DataBind();

            //    //Response.ClearContent();
            //    //Response.AddHeader("content-disposition", "attachment; filename=" + filename + ".xls");
            //    //Response.ContentType = "application/vnd.ms-excel";

            //    //StringWriter sw = new StringWriter();
            //    //HtmlTextWriter htw = new HtmlTextWriter(sw);
            //    //// Render the grid contents => the writer objects => Response object
            //    //grd.RenderControl(htw);
            //    //Response.Write(sw.ToString());

            //    //Response.End();
            //    //return View("Index");
            //    //Create a response stream to create and write the Excel file
            //    GridView grid = new GridView();
            //    grid.DataSource = db.ReportRequests.ToList();
            //    grid.DataBind();
            //    System.Web.HttpContext curContext = System.Web.HttpContext.Current;
            //    curContext.Response.Clear();
            //    curContext.Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            //    curContext.Response.Charset = "";
            //    curContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //    Response.ContentType = "application/excel";

            //    //Convert the rendering of the gridview to a string representation 
            //    StringWriter sw = new StringWriter();
            //    HtmlTextWriter htw = new HtmlTextWriter(sw);
            //    grid.RenderControl(htw);

            //    //Open a memory stream that you can use to write back to the response
            //    byte[] byteArray = Encoding.ASCII.GetBytes(sw.ToString());
            //    MemoryStream s = new MemoryStream(byteArray);
            //    StreamReader sr = new StreamReader(s, Encoding.ASCII);

            //    //Write the stream back to the response
            //    curContext.Response.Write(sr.ReadToEnd());
            //    curContext.Response.End();
            //    return View("Index");
            //}
            //var filename = string.Format("{0}{1}-{2}.pdf", "ReportSKPD", model.Start.ToShortDateString().ToString().Replace("/", ""), model.From.ToShortDateString().Replace("/", ""));
            //IEnumerable<ReportRequest> rr = db.ReportRequests;

            //if (rr == null)
            //{
            //    return RedirectToAction("PerjadinNotRelease");
            //}
            //else
            //{
            //    MemoryStream memory = new MemoryStream();
            //    var PrintPageUrl = ConfigurationManager.AppSettings["Report"];
            //    var PrintReportUrl = string.Format("{0}?start={1}&end={2}", PrintPageUrl, model.Start.Ticks, model.From.Ticks);
            //    PdfDocument document = new PdfDocument() { Url = PrintReportUrl };
            //    PdfOutput output = new PdfOutput() { OutputStream = memory };

            //    PdfConvert.ConvertHtmlToPdf(document, output);
            //    memory.Position = 0;

            //    return File(memory, "application/pdf", Server.UrlEncode(filename));
            //}
            //var contacts = db.RequestReleases;
            //var grid = new System.Web.UI.WebControls.GridView();

            //grid.DataSource = contacts.ToList();
            //grid.DataBind();

            //Response.ClearContent();
            //Response.AddHeader("content-disposition", "attachment; filename=YourFileName.xls");
            //Response.ContentType = "application/excel";
            //StringWriter sw = new StringWriter();
            //HtmlTextWriter htw = new HtmlTextWriter(sw);
            //grid.RenderControl(htw);
            //Response.Write(sw.ToString());
            //Response.End();
            //return View("Index");

            //Create a workbook
            using (ExcelPackage pck = new ExcelPackage())
            {
                var filename = string.Format("{0}{1}-{2}", "ReportSKPD", model.Start.ToShortDateString().ToString().Replace("/", ""), model.From.ToShortDateString().Replace("/", ""));
                Int64 start = model.Start.Ticks;
                Int64 end = model.From.Ticks;
                DateTime DTstart = new DateTime(start);
                DateTime DTend = new DateTime(end);
                var contacts = db.ReportRequests.Where(a => a.TGL_SKPD >= DTstart && a.TGL_SKPD <= DTend);
                if (contacts.Count() != 0)
                {
                    var grid = new System.Web.UI.WebControls.GridView();
                    System.Data.DataTable dt = new System.Data.DataTable();
                    grid.DataSource = contacts.ToList();
                    grid.DataBind();

                    if (grid.HeaderRow != null)
                    {

                        for (int i = 0; i < grid.HeaderRow.Cells.Count; i++)
                        {
                            dt.Columns.Add(grid.HeaderRow.Cells[i].Text);
                        }
                    }

                    //  add each of the data rows to the table
                    foreach (GridViewRow row in grid.Rows)
                    {
                        DataRow dr;
                        dr = dt.NewRow();

                        for (int i = 0; i < row.Cells.Count; i++)
                        {
                            dr[i] = row.Cells[i].Text;//.Replace(" ", "");
                        }
                        dt.Rows.Add(dr);
                    }

                    //Create the worksheet
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

                    //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1
                    ws.Cells["A1"].LoadFromDataTable(dt, true);

                    //Format the header for column 1-3
                    //using (ExcelRange rng = ws.Cells["A1:C1"])
                    //{
                    //    rng.Style.Font.Bold = true;
                    //    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;                      //Set Pattern for the background to Solid
                    //    rng.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(79, 129, 189));  //Set color to dark blue
                    //    rng.Style.Font.Color.SetColor(Color.White);
                    //}

                    //Example how to Format Column 1 as numeric 
                    //using (ExcelRange col = ws.Cells[2, 1, 2 + dt.Rows.Count, 1])
                    //{
                    //    col.Style.Numberformat.Format = "#,##0.00";
                    //    col.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    //}

                    //Write it back to the client
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;  filename=" + filename);
                    Response.BinaryWrite(pck.GetAsByteArray());
                    return View("Index");
                }
                else
                {
                    return View("Index");
                }
            }

        }

        public ViewResult Report(Int64 start,Int64 end)
        {
            DateTime DTstart = new DateTime(start);
            DateTime DTend = new DateTime(end);
            IEnumerable<ReportRequest> rr = db.ReportRequests.Where(a => a.TGL_SKPD >= DTstart && a.TGL_SKPD <= DTend);
            return View(rr);
        }

        public ActionResult PrintReport(RangeReleaseReport model)
        {
            var filename = string.Format("{0}{1}-{2}.pdf", "ReportSKPD", model.Start.ToShortDateString().ToString().Replace("/", ""), model.From.ToShortDateString().Replace("/", ""));
            IEnumerable<ReportRequest> rr = db.ReportRequests;

            if (rr == null)
            {
                return RedirectToAction("PerjadinNotRelease");
            }
            else
            {
                MemoryStream memory = new MemoryStream();
                var PrintPageUrl = ConfigurationManager.AppSettings["Report"];
                var PrintReportUrl = string.Format("{0}?start={1}&end={2}", PrintPageUrl, model.Start.ToShortDateString().ToString(), model.From.ToShortDateString().ToString());
                PdfDocument document = new PdfDocument() { Url = PrintReportUrl };            
                PdfOutput output = new PdfOutput() { OutputStream = memory };

                PdfConvert.ConvertHtmlToPdf(document, output);
                memory.Position = 0;

                return File(memory, "application/pdf", Server.UrlEncode(filename));
            }
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
