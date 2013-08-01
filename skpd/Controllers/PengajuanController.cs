using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using skpd.DTO;
using skpd.Models;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Globalization;
using System.Transactions;
using System.Data;
using System.Configuration;
using skpd.Services;
using System.Web.Routing;
using System.Web.SessionState;
using System.Data.Entity;
using skpd.Attributes;
using Calabonga.Mvc.PagedListExt;
using System.IO;
using skpd.Codaxy.WkHtmlToPdf;


namespace skpd.Controllers
{
    //[Authorize]
    [HandleError]
    public class PengajuanController : Controller
    {
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
        // GET: /Pengajuan/

        public ActionResult Index()
        {
            //Request request = new Request();
            return View();
        }

        [HttpPost]
        public ActionResult Index(string[] trans,Request request, RequestKadivDTO requestkadiv)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var vwrequest = db.vwRequests
                                    .Where(a=>a.Username == User.Identity.Name);
                    int? check = vwrequest.Where(a => a.StartDate >= request.StartDate || a.EndDate >= request.EndDate).Select(a=>a.RequestID).FirstOrDefault();
                    if (check != 0)
                    {
                        //TempData["ModelName"] = vwrequest;
                        return RedirectToAction("NotAvailable", new { id = check });
                    }
                    //Transaction
                    using (TransactionScope transaction = new TransactionScope())
                    {
                        vwUserProfile vwUserProfile =  db.vwUserProfiles.Where(a => a.Username == User.Identity.Name).FirstOrDefault();
                        // Declaration Variabel//
                        var StartDate = Convert.ToDateTime(request.StartDate).Date;
                        var EndDate = Convert.ToDateTime(request.EndDate).Date;
                        var PositionID = vwUserProfile.PositionID;
                        var UserID = vwUserProfile.ID;
                        var CreatedBy = User.Identity.Name;
                        var ApprovalPositionID = vwUserProfile.AssignID;
                        DateTime DateNow = DateTime.Now;
                        string hostUrl = Request.Url.GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped);

                        // Maping To Request Model For New Request And Save  //
                        request.FlagID = 1;//NEW REQUEST
                        request.StartDate = StartDate;
                        request.EndDate = EndDate;
                        request.PositionID = PositionID;
                        request.UserID = UserID;
                        request.CreatedBy = CreatedBy;
                        request.CreatedDate = DateNow;
                        request.FlagCreatedDate = DateNow;
                        if (requestkadiv.sendirilain == 2)
                        {
                            request.ApprovalPositionID = vwUserProfile.PositionID;
                        }
                        if (requestkadiv.sendirilain == 0 || requestkadiv.sendirilain != 2)
                        {
                            request.ApprovalPositionID = ApprovalPositionID;
                        }
                        
                        db.Requests.Add(request);
                        db.SaveChanges();

                        // Mapping To Request Model For New Request And Save  //
                        request.FlagID = 5;//REQUEST APPROVAL
                        db.Entry(request).State = EntityState.Modified;
                        db.SaveChanges();


                        // Mapping To RequestInTransport Model For List Transportaions And Save  //
                        foreach (var transID in trans)
                        {
                            var AdCategory = new RequestInTransport()
                            {
                                RequestID = request.RequestID,
                                TransportID = Convert.ToInt16(transID)
                            };
                            db.RequestInTransports.Add(AdCategory);
                        }
                        db.SaveChanges();

                        if (requestkadiv.ProgramID ==0)
                        {
                            //PAKE INTERNET
                            //Sending Email To Approver From Requester
                            if (ServiceSkpd.SendEmailToApproverFromRequester(hostUrl, request, db, MessengerService))
                            {
                                //Comitt If This Transaction is Success
                                transaction.Complete();
                                return RedirectToAction("Thanks", "Pengajuan");
                            }
                            else
                            {
                                ModelState.AddModelError("", "Gagal proses pengajuan");
                                ModelState.AddModelError("", "Gagal mengirim email");
                                ModelState.AddModelError("", "Pastikan anda sedang terhubung koneksi internet");
                            }
                        }

                        //MY BUDGET
                        if (requestkadiv.sendirilain != 2)
                        {
                            //request.FlagID = 2;
                            //request.FlagCreatedDate = DateNow;
                            //db.Entry(request).State = EntityState.Modified;
                            //db.SaveChanges();

                            //request.FlagID = 5;
                            //request.ApprovalPositionID = ApprovalPositionID;
                            //request.FlagCreatedDate = DateNow;
                            //db.Entry(request).State = EntityState.Modified;
                            //db.SaveChanges();

                            RequestInProgram requestinprogram = new RequestInProgram();
                            requestinprogram.RequestID = request.RequestID;
                            requestinprogram.ProgramID = requestkadiv.ProgramID;
                            db.RequestInPrograms.Add(requestinprogram);
                            db.SaveChanges();

                            //PAKE INTERNET
                            //Sending Email To Approver From Requester
                            if (ServiceSkpd.SendEmailToApproverFromRequester(hostUrl, request, db, MessengerService))
                            {
                                //Comitt If This Transaction is Success
                                transaction.Complete();
                                return RedirectToAction("Thanks", "Pengajuan");
                            }
                            else
                            {
                                ModelState.AddModelError("", "Gagal proses pengajuan");
                                ModelState.AddModelError("", "Gagal mengirim email");
                                ModelState.AddModelError("", "Pastikan anda sedang terhubung koneksi internet");
                            }
                            //Comitt If This Transaction is Success
                            //transaction.Complete();
                            //return RedirectToAction("Thanks", "Pengajuan");
                        }
                        else
                        {
                            request.FlagID = 2;
                            request.FlagCreatedDate = DateNow;
                            db.Entry(request).State = EntityState.Modified;
                            db.SaveChanges();

                            var AssignIDForBudget = db.vwPositionInPrograms.Where(a => a.UnitID == requestkadiv.UnitID).Select(a => a.PositionID).FirstOrDefault();
                            request.FlagID = 9;
                            request.FlagCreatedDate = DateNow;
                            request.ApprovalPositionID = AssignIDForBudget;
                            db.Entry(request).State = EntityState.Modified;
                            db.SaveChanges();

                            if (ServiceSkpd.SendEmailRequestBudgetToDivisionFromRequester(hostUrl, request, db, MessengerService))
                            {
                                transaction.Complete();
                                return RedirectToAction("SuccessSendRequestBudget");
                            }
                            else
                            {
                                ModelState.AddModelError("", "Gagal proses pengajuan");
                                ModelState.AddModelError("", "Gagal mengirim email");
                                ModelState.AddModelError("", "Pastikan anda sedang terhubung koneksi internet");
                            }
                        }
                        #region
                        ////string resetUrl = hostUrl + VirtualPathUtility.ToAbsolute("~/Account/PasswordReset?resetToken=" + HttpUtility.UrlEncode(resetToken));

                        //var fromFirstName = db.Users.Where(a => a.Username == request.CreatedBy).Select(a => a.FirstName).Single();
                        //var fromLastName = db.Users.Where(a => a.Username == request.CreatedBy).Select(a => a.LastName).Single();
                        //var fromfullName = string.Format("{0}.{1}", fromFirstName.ToUpper(), fromLastName.ToUpper());
                        //var fromAddress = ConfigurationManager.AppSettings["Sender"];
                        //var fromPositionName = db.Positions.Where(a => a.PositionID == request.PositionID).Select(a => a.PositionName).Single();
                        //var skpdDestination = request.Destination;
                        //var skpdStarDate = request.StartDate.Date.ToShortDateString();
                        //var skpdEventName= request.EventName;
                        //var skpdEndDate = request.EndDate.Date.ToShortDateString();
                        //var toAddress = db.Users.Where(a => a.PositionID == request.ApprovalPositionID).Select(a => a.Email).Single();
                        //var toName = db.Users.Where(a => a.PositionID == request.ApprovalPositionID).Select(a => a.Username).Single();
                        //var skpdRequestID = lastReuestID.ToString();
                        //var QueryParam = string.Format("UserName={0}&RequestID={1}", toName, skpdRequestID);
                        //var QueryParamEncrypt = ServiceSkpd.QueryStringEncrypt(QueryParam);
                        //var approveUrl = hostUrl + VirtualPathUtility.ToAbsolute(string.Format("~/Persetujuan/ApproveFromEmail{0}", QueryParamEncrypt));
                        //var rejectUrl = hostUrl + VirtualPathUtility.ToAbsolute(string.Format("~/Persetujuan/RejectFromEmail{0}", QueryParamEncrypt));
                        ////var approveUrl = hostUrl + VirtualPathUtility.ToAbsolute(string.Format("~/Persetujuan/ApproveFromEmail?UserName={0}&id={1}", HttpUtility.UrlEncode(toName), HttpUtility.UrlEncode(skpdRequestID)));
                        ////var rejectUrl = hostUrl + VirtualPathUtility.ToAbsolute(string.Format("~/Persetujuan/RejectFromEmail?UserName={0}&id={1}", HttpUtility.UrlEncode(toName), HttpUtility.UrlEncode(skpdRequestID)));
                        //var subject = string.Format("Request from {0}, for approval of official travel", fromfullName);
                        //var body = string.Format("Dear, Mr./Mrs. {0} <br /><br/>" +
                        //                        "Request for approval of official travel : " +
                        //                        "<br/><br/>RequestID: {1} " +
                        //                        "<br/>Name: {2} " +
                        //                        "<br/>Position: {3} " +
                        //                        "<br/>Destination : {4} " +
                        //                        "<br/>EventName : {5} " +
                        //                        "<br/>Star Date : {6} " +
                        //                        "<br/>End Date : {7} " +
                        //                        "<br/><br /> whether you would approve or reject this request ? " +
                        //                        "<br/><br/><p><strong><a href=\"{8}\">APPROVE</a></strong></p> " +
                        //                        "<br/><br/><p><strong><a href=\"{9}\">REJECT</a></strong></p>", toName, lastReuestID, fromfullName, fromPositionName, skpdDestination, skpdEventName, skpdStarDate, skpdEndDate, approveUrl, rejectUrl);

                        //if (MessengerService.Send(fromAddress, toAddress, subject, body, true))
                        //{
                        //    transaction.Complete();
                        //    return RedirectToAction("Thanks", "Pengajuan");
                        //}
                        //else
                        //{
                        //    ModelState.AddModelError("", "Gagal mengirim Email, kontak IT Service di wilayah anda");
                        //    return View(request);
                        //}
                    }
                }
                //catch (DbEntityValidationException dbEx)
                //{
                //    foreach (var validationErrors in dbEx.EntityValidationErrors)
                //    {
                //        foreach (var validationError in validationErrors.ValidationErrors)
                //        {
                //            Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                //            ModelState.AddModelError("", string.Format("{0} >> {1}", validationError.PropertyName, validationError.ErrorMessage));
                //        }
                //    }
                //}
                        #endregion
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(request);
        }

        public IEnumerable<Trans> GetAllTransport()
        {
            var c = from transport in db.Transports
                    select new Trans { Id = transport.TransportID, Name = transport.TransportName };
            return c;
        }

        public JsonResult GetDestination(int id)
        {
            var des = new List<DestinationDTO> {
			                      	new DestinationDTO {ID =1,Name="MEDAN"},
			                      	new DestinationDTO {ID =1,Name="PEKANBARU"},
                                    new DestinationDTO {ID =1,Name="PALEMBANG"},
                                    new DestinationDTO {ID =1,Name="BANDUNG"},
                                    new DestinationDTO {ID =1,Name="SEMARANG"},
                                    new DestinationDTO {ID =1,Name="SURABAYA"},
                                    new DestinationDTO {ID =1,Name="BALIKPAPAN"},
                                    new DestinationDTO {ID =1,Name="MAKASSAR"},
                                    new DestinationDTO {ID =1,Name="MANADO"},
                                    new DestinationDTO {ID =1,Name="DENPASAR"},
                                    new DestinationDTO {ID =2,Name="CINA"},
                                    new DestinationDTO {ID =2,Name="AMERIKA"},
                                    new DestinationDTO {ID =2,Name="ARAB"},
                                    new DestinationDTO {ID =2,Name="DUBAI"},
                                    new DestinationDTO {ID =2,Name="SINGAPURA"},
                                    new DestinationDTO {ID =2,Name="KOREA"},
                                    new DestinationDTO {ID =2,Name="MALAYSIA"},
                                    new DestinationDTO {ID =2,Name="VIETNAM"},
                                    new DestinationDTO {ID =2,Name="BRUNAI DARUSSALAM"},
                                    new DestinationDTO {ID =2,Name="BAGHDAD"},
                                    new DestinationDTO {ID =2,Name="AFRIKA"},
			                      };

            return Json(des.Where(a => a.ID == id),JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRegion()
        {
            var regions = (from region in db.Regions
                          select new
                          {
                              RegionID = region.RegionID,
                              RegionName = region.RegionName.ToUpper()
                          }).Distinct();
            return Json(regions, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCountry(int RegionID)
        {
            var countries = (from country in db.CountryInRegions.Include("Country")
                             where country.RegionID == RegionID
                             select new
                             {
                                 CountryID = country.Country.CountryID,
                                 CountryName = country.Country.CountryName.ToUpper()
                             }).Distinct();
            return Json(countries, JsonRequestBehavior.AllowGet);
        }

        public ViewResult Thanks()
        {
            return View();
        }

        public ViewResult Lacak(int? id, string currentFilter, string searchString)
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
            ViewBag.Message = "Lacak Pengajuan";
            using (var db = new ESKAPEDEContext())
            {
                var requests = db.vwRequests.Where(a => a.Username == User.Identity.Name).OrderByDescending(a => a.RequestID).ToList();
                if (!String.IsNullOrEmpty(searchString))
                {
                    requests = requests.Where(s => s.EventName.ToUpper().Contains(searchString.ToUpper())).OrderByDescending(a => a.RequestID).ToList();
                }

                int pageSize = 5;
                int pageNumber = (id ?? 1);

                return View(requests.ToPagedList(pageNumber, pageSize));
            }
            //return View(db.Requests.Where(a => a.CreatedBy == User.Identity.Name).ToList());
        }

        public ViewResult Details(int id)
        {
            vwRequest request = db.vwRequests
                                   .Where(a => a.RequestID == id).SingleOrDefault();
            return View(request);
        }

        public ViewResult PrintPage(int id)
        {
            vwRequest printRequest = db.vwRequests
                                        .Where(a => a.RequestID == id).SingleOrDefault();
            return View(printRequest);
        }

        public ViewResult NotAvailable(int id)
        {
            //var vwreq = TempData["ModelName"];
            vwRequest vwreq = db.vwRequests
                                        .Where(a => a.RequestID == id).SingleOrDefault();
            return View(vwreq);
        }

        public ActionResult Print(int id)
        {
            MemoryStream memory = new MemoryStream();
            PdfDocument document = new PdfDocument() { Url = "http://localhost:52858/Pengajuan/PrintPage/" + id };
            PdfOutput output = new PdfOutput() { OutputStream = memory };

            PdfConvert.ConvertHtmlToPdf(document, output);
            memory.Position = 0;

            return File(memory, "application/pdf", Server.UrlEncode("E-SKPD.pdf"));
        }

        public string test()
        {
            IEnumerable<RequestInTransport> x = db.RequestInTransports.Where(a => a.RequestID == 67);
            string s = "<ul>";
            foreach (var item in x)
            {
                s += string.Format("<li>{0}</li>", item.Transport.TransportName);
            }
            s = string.Format("{0}</ul>", s);
            return s;
        }

        public ViewResult Tracking(int id)
        {
            IEnumerable<RequestLog> requestlog = db.RequestLogs
                                                .Include(a => a.Position1)
                                                .Include(a => a.Flag)
                                                .Where(a => a.RequestID == id);
            return View(requestlog);
        }

        public ActionResult SuccessSendRequestBudget()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }
}
