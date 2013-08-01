using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using skpd.Models;
using skpd.Services;
using System.Web.Routing;
using System.Data;
using System.Transactions;
using skpd.DTO;

namespace skpd.Controllers
{
    public class PersetujuanAnggaranController : Controller
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
        //
        // GET: /PersetujuanAnggaran/

        public ActionResult Index()
        {
            var positionID = db.Users.Where(a => a.Username == User.Identity.Name).Select(a => a.PositionID).Single();
            var request = db.Requests
                            .Include("Position")
                            .Include("Country")
                            .Where(a => a.ApprovalPositionID == positionID && a.FlagID == 9);
            //var req = from x in db.Requests
            //        join y in db.RequestInPrograms on x.RequestID equals y.RequestID
            //        where y.AssignID == positionID && y.FlagID == 9
            //        select x;
            return View(request);
        }

        public ActionResult Approve(int id)
        {
            //var UserDivisionID = db.vwUserProfiles.Where(a => a.Username == User.Identity.Name).Select(a => a.DivisionID).SingleOrDefault();
            var UnitID = db.vwPositionInPrograms.Where(a => a.fullname == User.Identity.Name).Select(a => a.UnitID).FirstOrDefault();
            RequestInProgram requestinprogram = new RequestInProgram();
            requestinprogram.RequestID = id;
            var programs = db.vwPositionInPrograms.Where(a => a.UnitID == UnitID).ToList();
            IEnumerable<SelectListItem> IEprograms = from s in programs
                                               select new SelectListItem
                                                {
                                                    Value = s.ProgramID.ToString(),
                                                    Text = s.ProgramName.ToUpper()
                                                };
            ViewBag.ProgramID = new SelectList(IEprograms, "Value", "Text");
            return View(requestinprogram);
        }

        [HttpPost]
        public ActionResult Approve(RequestInProgram model)
        {
            try
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    vwUserProfile userprofile = db.vwUserProfiles.Where(a => a.Username == User.Identity.Name).FirstOrDefault();
                    vwRequest vwrequest = db.vwRequests.Where(a => a.RequestID == model.RequestID).FirstOrDefault();
                    var ApprovalPositionID = db.Users.Where(a => a.Username == User.Identity.Name).Select(a => a.PositionID).Single();
                    //var AssignID = db.Positions.Where(a => a.PositionID == userprofi).Select(a => a.PositionID).Single();
                    DateTime DateTimeNow = DateTime.Now;

                    //UPDATE REQUEST
                    Request request = db.Requests.Include("RequestInTransports").Where(a => a.RequestID == model.RequestID).First();
                    RequestInProgram requestinprogram = new RequestInProgram();
                    string hostUrl = Request.Url.GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped);
                    //APPROVED
                    request.FlagID = 2;
                    request.FlagCreatedDate = DateTimeNow;
                    db.Entry(request).State = EntityState.Modified;
                    db.SaveChanges();

                    if (ServiceSkpd.SendEmailResultToRequesterCcApprover(request, db, MessengerService))
                    {
                        //REQUEST RELEASE TO DIVISI SDM
                        request.FlagID = 7;
                        request.ApprovalPositionID = 194;
                        request.FlagCreatedDate = DateTimeNow;
                        db.Entry(request).State = EntityState.Modified;
                        db.SaveChanges();

                        //APPROVE REQUESTINPROGRAM
                        requestinprogram.RequestID = model.RequestID;
                        requestinprogram.ProgramID = model.ProgramID;

                        db.RequestInPrograms.Add(requestinprogram);
                        db.SaveChanges();

                        transaction.Complete();
                        this.HttpContext.Session["RequestID"] = requestinprogram.RequestID;
                        return RedirectToAction("SuccessAddBudget");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Gagal proses pengajuan");
                        ModelState.AddModelError("", "Gagal mengirim email");
                        ModelState.AddModelError("", "Pastikan anda sedang terhubung koneksi internet");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(model);
        }

        public ActionResult Reject(int id)
        {
            RejectRequest rejectreq = new RejectRequest();
            rejectreq.RequestID = db.Requests.Where(a => a.RequestID == id).Select(a => a.RequestID).SingleOrDefault();
            return View(rejectreq);
        }

        [HttpPost]
        public ActionResult Reject(RejectRequest model)
        {
            var ApprovalPositionID = db.Users.Where(a => a.Username == User.Identity.Name).Select(a => a.PositionID).Single();
            if (ModelState.IsValid)
            {
                if (!ServiceSkpd.isRequestIDExist(model.RequestID))
                {
                    ModelState.AddModelError("", "RequestID tidak ditemukan!");
                }
                else
                {
                    if (ServiceSkpd.isAlreadyApproveOrReject(model.RequestID, ApprovalPositionID, 3))
                    {
                        ModelState.AddModelError("", "Request ini sudah pernah di Tolak oleh anda!");
                    }
                    else if (ServiceSkpd.isAlreadyApproveOrReject(model.RequestID, ApprovalPositionID, 2))
                    {
                        ModelState.AddModelError("", "Request ini sudah di Setujui oleh anda!");
                        ModelState.AddModelError("", "Request ini hanya bisa di batalkan oleh posisi diatas anda!");
                    }
                    else
                    {
                        using (TransactionScope transaction = new TransactionScope())
                        {
                            var FlagCreatedDate = DateTime.Now;

                            Request request = db.Requests.Where(a => a.RequestID == model.RequestID).FirstOrDefault();

                            request.FlagID = 3;
                            request.FlagCreatedDate = FlagCreatedDate;
                            request.RejectedReason = model.RejectedReason;
                            db.Entry(request).State = EntityState.Modified;
                            db.SaveChanges();

                            string hostUrl = Request.Url.GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped);
                            if (ServiceSkpd.SendEmailRejectToRequesterCcApprover(request, db, MessengerService))
                            {
                                transaction.Complete();
                                return RedirectToAction("SuccessReject");
                            }
                            else
                            {
                                ModelState.AddModelError("", "Gagal proses pengajuan");
                                ModelState.AddModelError("", "Gagal mengirim email");
                                ModelState.AddModelError("", "Pastikan anda sedang terhubung koneksi internet");
                            }
                        }
                    }
                }
            }
            return View(model);
        }

        public ActionResult Details(int id)
        {
            //RequestInProgram requestinprogram = db.RequestInPrograms
            //                                    .Include("Request.RequestInTransports.Transport")
            //                                    .Include("RefBudget")
            //                                    .Where(a => a.RequestID == id).Single();
            //return View(requestinprogram);
            //Request request = db.Requests
            //                    .Include("Position")
            //                    .Include("User")
            //                    .Include("RequestInTransports.Transport")
            //                    .Where(a => a.RequestID == id).SingleOrDefault();
            //string[] Exclusive;
            //Exclusive = new string[2];
            //Exclusive[0] = "KADIV";
            //Exclusive[1] = "KA. KPM";
            //List<string> _Exclusive = new List<string>(Exclusive);

            //var PositionName = db.Positions.Where(a => a.PositionID == request.ApprovalPositionID).Select(a => a.PositionName).First();

            //if (_Exclusive.Any(s => PositionName.Contains(s)))
            //{
            //    ViewData["ButtonName"] = "Anggaran";
            //}
            //else
            //{
            //    ViewData["ButtonName"] = "Setuju";
            //}
            vwRequest request = db.vwRequests
                                    .Where(a => a.RequestID == id).SingleOrDefault();
            return View(request);
        }

        public ActionResult SuccessReject()
        {
            return View();
        }
        public ActionResult SuccessApprove()
        {
            return View();
        }

        public ActionResult SuccessAddBudget()
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
