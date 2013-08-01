using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using skpd.Models;
using skpd.DTO;
using skpd.Services;
using System.Web.Routing;
using System.Data;
using System.Transactions;
using System.Configuration;
using System.Collections.Specialized;

namespace skpd.Controllers
{
    //[Authorize]
    [HandleError]
    public class PersetujuanController : Controller
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
        // GET: /Persetujuan/

        public ActionResult Index()
        {
            var positionID = db.Users.Where(a => a.Username == User.Identity.Name).Select(a => a.PositionID).Single();
            //return View(db.Requests
            //            .Include("Position").Include("Country")
            //            .Where(a => a.ApprovalPositionID == positionID && a.FlagID == 5));

            return View(db.vwRequests.Where(a => a.ApprovalPositionID == positionID && a.FlagID == 5));
        }

        public ActionResult Approve(int id)
        {
            try
            {
                var RequestID = id;
                vwUserProfile vwUserProfile = db.vwUserProfiles.Where(a => a.Username == User.Identity.Name).FirstOrDefault();
                var ApprovalPositionID = vwUserProfile.PositionID;
                var ApprovalPositionTypeName = vwUserProfile.PositionTypeName;
                //var NextApprovalPositionID = ServiceSkpd.isApproverExclusive(ApprovalPositionID) == true ? 34 : ServiceSkpd.ParentPositionID(ApprovalPositionID);

                if (ServiceSkpd.isApproverExclusive(ApprovalPositionID)) return RedirectToAction("RequestInProgram", new { id = RequestID });
                using (TransactionScope transaction = new TransactionScope())
                {
                    if (ServiceSkpd.isAlreadyApproveOrReject(RequestID, ApprovalPositionID, 2))
                    {
                        ModelState.AddModelError("", "Request ini sudah pernah di setujui oleh anda!");
                    }
                    else
                    {
                        DateTime DateTimeNow = DateTime.Now;
                        Request request = db.Requests.Where(a => a.RequestID == RequestID).Single();

                        if (ApprovalPositionTypeName == "DIREKTUR")
                        {
                            request.FlagID = 2;//APPROVED
                            request.FlagCreatedDate = DateTimeNow;
                            db.Entry(request).State = EntityState.Modified;
                            db.SaveChanges();

                            request.FlagID = 7;//REQUEST RELEASE
                            request.ApprovalPositionID = 194;
                            request.FlagCreatedDate = DateTimeNow;
                            db.Entry(request).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        else 
                        {
                            request.FlagID = 2;//APPROVED
                            request.FlagCreatedDate = DateTimeNow;
                            db.Entry(request).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        if (ServiceSkpd.SendEmailResultToRequesterCcApprover(request, db, MessengerService))
                        {
                            if (ApprovalPositionTypeName != "DIREKTUR")
                            {
                                request.FlagID = 5;
                                request.ApprovalPositionID = vwUserProfile.AssignID;
                                db.Entry(request).State = EntityState.Modified;
                                db.SaveChanges();
                                string hostUrl = Request.Url.GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped);
                                if (ServiceSkpd.SendEmailToApproverFromRequester(hostUrl, request, db, MessengerService))
                                {
                                    transaction.Complete();
                                    return View("SuccessApprove");
                                }
                            }
                            transaction.Complete();
                        }
                        else
                        {
                            ModelState.AddModelError("", "Gagal proses Persetujuan");
                            ModelState.AddModelError("", "Gagal mengirim email");
                            ModelState.AddModelError("", "Pastikan anda sedang terhubung koneksi internet");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View();
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
            //Request request = db.Requests
            //                    .Include("Position")
            //                    .Include("User")
            //                    .Include("RequestInTransports.Transport")
            //                    .Include("Country")
            //                    .Where(a => a.RequestID == id).SingleOrDefault();
            vwRequest request = db.vwRequests
                                    .Where(a => a.RequestID == id).SingleOrDefault();
            return View(request);
        }

        public ActionResult RequestInProgram(int id)
        {
            RequestInProgramDTO requestinprogram = new RequestInProgramDTO();
            requestinprogram.RequestID = id;
            return View(requestinprogram);
        }

        [HttpPost]
        public ActionResult RequestInProgram(RequestInProgramDTO model)
        {
            try
            {
                using (TransactionScope transaction = new TransactionScope())
                {

                    var ApprovalPositionID = db.Users.Where(a => a.Username == User.Identity.Name).Select(a => a.PositionID).Single();
                    //var ApprovalPositionInProgram = db.vwPositionInPrograms
                    //                .Where(a => a.DivisionID == model.DivisionID && a.KdProg==model.BudgetID)
                    //                .Select(a => a.PositionID).SingleOrDefault();
                    DateTime DateTimeNow = DateTime.Now;

                    //UPDATE REQUEST
                    Request request = db.Requests.Include("RequestInTransports").Where(a => a.RequestID == model.RequestID).First();
                    RequestInProgram requestinprogram = new RequestInProgram();
                    string hostUrl = Request.Url.GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped);

                    //MY BUDGET
                    if (model.sendirilain != 2)
                    {
                        //APPROVED
                        request.FlagID = 2;
                        request.FlagCreatedDate = DateTimeNow;
                        db.Entry(request).State = EntityState.Modified;
                        db.SaveChanges();

                        if (ServiceSkpd.SendEmailResultToRequesterCcApprover(request, db, MessengerService))
                        {
                            //APPROVE REQUESTINPROGRAM
                            requestinprogram.RequestID = model.RequestID;
                            requestinprogram.ProgramID = model.ProgramID;

                            db.RequestInPrograms.Add(requestinprogram);
                            db.SaveChanges();

                            request.FlagID = 7;
                            request.ApprovalPositionID = 194;
                            request.FlagCreatedDate = DateTimeNow;
                            db.Entry(request).State = EntityState.Modified;
                            db.SaveChanges();

                            

                            //if (request.ApprovalPositionID == ApprovalPositionID)
                            //{
                            //    request.FlagID = 8;
                            //    request.FlagCreatedDate = DateTime.Now;
                            //    db.Entry(request).State = EntityState.Modified;
                            //    db.SaveChanges();
                            //}

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
                    //BUDGET ANOTHER DIVISION
                    else
                    {
                        //APPROVED
                        request.FlagID = 2;
                        request.FlagCreatedDate = DateTimeNow;
                        db.Entry(request).State = EntityState.Modified;
                        db.SaveChanges();

                        if (ServiceSkpd.SendEmailResultToRequesterCcApprover(request, db, MessengerService))
                        {
                            var AssignIDForBudget = db.vwPositionInPrograms.Where(a => a.UnitID == model.UnitID).Select(a => a.PositionID).FirstOrDefault();
                            request.FlagID = 9;
                            request.FlagCreatedDate = DateTimeNow;
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
                        else
                        {
                            ModelState.AddModelError("", "Gagal proses pengajuan");
                            ModelState.AddModelError("", "Gagal mengirim email");
                            ModelState.AddModelError("", "Pastikan anda sedang terhubung koneksi internet");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.InnerException);
            }
            return View(model);
        }


        public ActionResult SuccessApprove()
        {
            return View();
        }
        public ActionResult SuccessReject()
        {
            return View();
        }
        public ActionResult SuccessAddBudget()
        {
            return View();
        }
        public ActionResult SuccessSendRequestBudget()
        {
            return View();
        }

        public JsonResult GetDivision()
        {
            var division = (from PositionInPrograms in db.vwPositionInPrograms
                           select new
                           {
                               UnitID = PositionInPrograms.UnitID,
                               UnitName = PositionInPrograms.UnitName.ToUpper()
                           }).Distinct();
            return Json(division,JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetProgramDivision(int id)
        {
            var program = from PositionInPrograms in db.vwPositionInPrograms
                          where PositionInPrograms.UnitID == id
                           select new
                           {
                               ProgramID = PositionInPrograms.ProgramID,
                               ProgramName = PositionInPrograms.ProgramName.ToUpper()
                           };
            return Json(program, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
