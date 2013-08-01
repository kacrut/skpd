using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using skpd.Models;
using skpd.Services;
using System.Web.Routing;
using skpd.DTO;
using System.Collections.Specialized;
using System.Transactions;
using System.Data;

namespace skpd.Controllers
{
    public class FromEmailController : Controller
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
            if (WebSecurityService.HasUserId) WebSecurityService.Logout();

            base.Initialize(requestContext);
        }
        #endregion
        //
        // GET: /FromEmail/

        /// <summary>
        /// APPROVE AND REJECT REQUEST
        /// </summary>
        /// <returns></returns>
        public ActionResult Approve()
        {
            try
            {
                if (Request.QueryString["tiket"] == null)
                {
                    return new HttpNotFoundResult("404 - NotFoundBro");
                }
                else
                {
                    string tiket = Request.QueryString["tiket"];
                    string QueryStringModuleDecrypt = QueryStringModule.Decrypt(tiket);
                    NameValueCollection NameValue = HttpUtility.ParseQueryString(QueryStringModuleDecrypt);
                    var RequestID = Convert.ToInt32(NameValue["RequestID"]);
                    var UserName = NameValue["UserName"];
                    var ApprovalID = db.Users.Where(a => a.Username == UserName).Select(a => a.PositionID).SingleOrDefault();

                    vwRequest request = db.vwRequests
                                        .Where(a => a.RequestID == RequestID).SingleOrDefault();

                    if (!ServiceSkpd.isRequestIDExist(RequestID))
                    {
                        return View("RequestIsNotExist");
                    }
                    else if (ServiceSkpd.isAlreadyApproveOrReject(RequestID, ApprovalID, 2))
                    {
                        return View("RequestIsAlreadyApproved");
                    }
                    else if (ServiceSkpd.isAlreadyApproveOrReject(RequestID, ApprovalID, 3))
                    {
                        return View("RequestIsAlreadyRejected");
                    }
                    else
                    {
                        if (ServiceSkpd.isApproverExclusive(Convert.ToInt16(request.ApprovalPositionID)))
                        {
                            return RedirectToAction("RequestInProgram", new { tiket = tiket });
                        }
                    }
                    return View(request);
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return View(Request);
        }
        [HttpPost]
        public ActionResult Approve(vwRequest model)
        {
            if (ModelState.IsValid)
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    string tiket = Request.QueryString["tiket"];
                    string QueryStringModuleDecrypt = QueryStringModule.Decrypt(tiket);
                    NameValueCollection NameValue = HttpUtility.ParseQueryString(QueryStringModuleDecrypt);
                    var RequestID = Convert.ToInt32(NameValue["RequestID"]);
                    var UserName = NameValue["UserName"];

                    vwUserProfile vwUserProfile = db.vwUserProfiles.Where(a => a.Username == UserName).FirstOrDefault();
                    var ApprovalPositionID = vwUserProfile.PositionID;
                    var ApprovalPositionTypeName = vwUserProfile.PositionTypeName;
                    DateTime DateTimeNow = DateTime.Now;
                    Request request = db.Requests.Where(a => a.RequestID == model.RequestID).Single();

                    //var NextApprovalPositionID = 0;
                    //if (ServiceSkpd.isApproverExclusive(model.ApprovalPositionID)) NextApprovalPositionID = 8;

                    //NextApprovalPositionID = ServiceSkpd.ParentPositionID(model.ApprovalPositionID);
                    if (ApprovalPositionTypeName == "DIREKTUR")
                    {
                        request.FlagID = 2;//APPROVED
                        request.FlagCreatedDate = DateTimeNow;
                        db.Entry(request).State = EntityState.Modified;
                        db.SaveChanges();

                        request.FlagID = 7;//REQUEST RELEASE
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
                        return View("SuccessApprove");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Gagal proses Persetujuan");
                        ModelState.AddModelError("", "Gagal mengirim email");
                        ModelState.AddModelError("", "Pastikan anda sedang terhubung koneksi internet");
                    }
                }
            }
            return View(model);
        }


        public ActionResult Reject()
        {
            RejectRequestFromEmail model = new RejectRequestFromEmail();
            try
            {
                if (Request.QueryString["tiket"] == null)
                {
                    return new HttpNotFoundResult("404 - NotFoundBro");
                }
                else
                {
                    string tiket = Request.QueryString["tiket"];
                    string QueryStringModuleDecrypt = QueryStringModule.Decrypt(tiket);
                    NameValueCollection NameValue = HttpUtility.ParseQueryString(QueryStringModuleDecrypt);
                    var RequestID = Convert.ToInt32(NameValue["RequestID"]);
                    var UserName = NameValue["UserName"];
                    var ApprovalID = db.Users.Where(a => a.Username == UserName).Select(a => a.PositionID).SingleOrDefault();
                    model.RequestID = Convert.ToInt16(RequestID);
                    model.UserName = UserName;

                    if (!ServiceSkpd.isRequestIDExist(RequestID))
                    {
                        return View("RequestIsNotExist");
                    }
                    else if (ServiceSkpd.isAlreadyApproveOrReject(RequestID, ApprovalID, 2))
                    {
                        return View("RequestIsAlreadyApproved");
                    }
                    else if (ServiceSkpd.isAlreadyApproveOrReject(RequestID, ApprovalID, 3))
                    {
                        return View("RequestIsAlreadyRejected");
                    }
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult Reject(RejectRequestFromEmail model)
        {
            if (ModelState.IsValid)
            {
                var ApprovalPosistionID = db.Users.Where(a => a.Username == model.UserName).Select(a => a.PositionID).Single();

                if (!ServiceSkpd.isRequestIDExist(model.RequestID))
                {
                    ModelState.AddModelError("", "RequestID tidak ditemukan!");
                }
                else
                {
                    if (ServiceSkpd.isAlreadyApproveOrReject(model.RequestID, ApprovalPosistionID, 3))
                    {
                        ModelState.AddModelError("", "Request ini sudah pernah di tolak oleh anda!");
                    }
                    else
                    {
                        using (TransactionScope transaction = new TransactionScope())
                        {
                            var FlagCreatedDate = DateTime.Now;

                            Request request = db.Requests.Where(a => a.RequestID == model.RequestID).Single();

                            request.FlagID = 3;
                            request.FlagCreatedDate = FlagCreatedDate;
                            request.RejectedReason = model.Reason;
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


        /// <summary>
        /// APPROVE AND REJECT BUDGET
        /// </summary>
        /// <returns></returns>
        public ActionResult ApproveBudget()
        {
            ApproveBudgetFromEmail model = new ApproveBudgetFromEmail();
            try
            {
                if (Request.QueryString["tiket"] == null)
                {
                    return new HttpNotFoundResult("404 - NotFoundBro");
                }
                else
                {
                    string tiket = Request.QueryString["tiket"];
                    string QueryStringModuleDecrypt = QueryStringModule.Decrypt(tiket);
                    NameValueCollection NameValue = HttpUtility.ParseQueryString(QueryStringModuleDecrypt);
                    var RequestID = NameValue["RequestID"];
                    var UserName = NameValue["UserName"];
                    model.RequestID = Convert.ToInt16(RequestID);
                    model.UserName = UserName;
                    var BudgetID = db.RequestInPrograms.Where(a => a.RequestID == model.RequestID).Select(a => a.ProgramID).Single();
                    var BudgetName = db.RequestInPrograms.Where(a => a.RequestID == model.RequestID).Select(a => a.Program.ProgramName).Single();
                    model.BudgetID = BudgetID.ToString();
                    model.BudgetName = BudgetName;
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult ApproveBudget(ApproveBudgetFromEmail model)
        {
            if (ModelState.IsValid)
            {
                var ApprovalPosistionID = db.Users.Where(a => a.Username == model.UserName).Select(a => a.PositionID).Single();

                if (!ServiceSkpd.isRequestIDExist(model.RequestID))
                {
                    ModelState.AddModelError("", "RequestID tidak ditemukan!");
                }
                else
                {
                    if (ServiceSkpd.isAlreadyApproveOrReject(model.RequestID, ApprovalPosistionID, 2))
                    {
                        ModelState.AddModelError("", "Request ini sudah pernah di setujui oleh anda!");
                    }
                    else
                    {
                        using (TransactionScope transaction = new TransactionScope())
                        {
                            var FlagCreatedDate = DateTime.Now;
                            //var ApprovalPositionID = db.Users.Where(a => a.Username == model.UserName).Select(a => a.PositionID).Single();
                            var NextApprovalPositionID = db.Positions.Where(a => a.PositionID == ApprovalPosistionID).Select(a => a.AssignID).Single();

                            Request request = db.Requests.Where(a => a.RequestID == model.RequestID).Single();
                            RequestInProgram requestinprogram = db.RequestInPrograms.Where(a => a.RequestID == model.RequestID).Single();

                            request.FlagID = 2;
                            request.FlagCreatedDate = FlagCreatedDate;
                            db.Entry(request).State = EntityState.Modified;
                            db.SaveChanges();

                            string hostUrl = Request.Url.GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped);
                            if (ServiceSkpd.SendEmailResultToRequesterCcApprover(request, db, MessengerService))
                            {
                                request.FlagID = 7;
                                request.ApprovalPositionID = 194;
                                request.FlagCreatedDate = FlagCreatedDate;

                                db.Entry(request).State = EntityState.Modified;
                                db.SaveChanges();


                                db.RequestInPrograms.Add(requestinprogram);
                                db.SaveChanges();

                                transaction.Complete();
                                return RedirectToAction("SuccessApprove");
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


        public ActionResult RejectBudget()
        {
            RejectBudgetFromEmail model = new RejectBudgetFromEmail();
            try
            {
                if (Request.QueryString["tiket"] == null)
                {
                    return new HttpNotFoundResult("404 - NotFoundBro");
                }
                else
                {
                    string tiket = Request.QueryString["tiket"];
                    string QueryStringModuleDecrypt = QueryStringModule.Decrypt(tiket);
                    NameValueCollection NameValue = HttpUtility.ParseQueryString(QueryStringModuleDecrypt);
                    var RequestID = NameValue["RequestID"];
                    var UserName = NameValue["UserName"];
                    int intRequestID = Convert.ToInt16(RequestID);
                    model.RequestID = intRequestID;
                    model.UserName = UserName;
                    var BudgetID = db.RequestInPrograms.Where(a => a.RequestID == model.RequestID).Select(a => a.ProgramID).FirstOrDefault();
                    var BudgetName = db.RequestInPrograms.Where(a => a.RequestID == model.RequestID).Select(a => a.Program.ProgramName).FirstOrDefault();
                    model.BudgetID = BudgetID.ToString();
                    model.BudgetName = BudgetName;
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult RejectBudget(RejectBudgetFromEmail model)
        {
            if (ModelState.IsValid)
            {
                var ApprovalPosistionID = db.Users.Where(a => a.Username == model.UserName).Select(a => a.PositionID).Single();

                if (!ServiceSkpd.isRequestIDExist(model.RequestID))
                {
                    ModelState.AddModelError("", "RequestID tidak ditemukan!");
                }
                else
                {
                    if (ServiceSkpd.isAlreadyApproveOrReject(model.RequestID, ApprovalPosistionID, 3))
                    {
                        ModelState.AddModelError("", "Request ini sudah pernah di tolak oleh anda!");
                    }
                    else
                    {
                        using (TransactionScope transaction = new TransactionScope())
                        {
                            var FlagCreatedDate = DateTime.Now;
                            RequestInProgram requestinprogram = db.RequestInPrograms.Where(a => a.RequestID == model.RequestID).FirstOrDefault();
                            Request request = db.Requests.Where(a => a.RequestID == model.RequestID).FirstOrDefault();

                            request.FlagID = 3;
                            request.FlagCreatedDate = FlagCreatedDate;
                            request.RejectedReason = model.Reason;
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


        public ActionResult RequestInProgram(string tiket)
        {
            string QueryStringModuleDecrypt = QueryStringModule.Decrypt(tiket);
            NameValueCollection NameValue = HttpUtility.ParseQueryString(QueryStringModuleDecrypt);
            var RequestID = NameValue["RequestID"];
            var UserName = NameValue["UserName"];
            int intRequestID = Convert.ToInt16(RequestID);
            var ApprovalPositionID = db.Requests.Where(a => a.RequestID == intRequestID).Select(a => a.ApprovalPositionID).SingleOrDefault();
            var ApprovalDivisionID = db.vwUserProfiles.Where(a => a.PositionID == ApprovalPositionID).Select(a => a.DivisionID).SingleOrDefault();
            RequestInProgramDTO requestinprogram = new RequestInProgramDTO();
            requestinprogram.RequestID = intRequestID;
            //requestinprogram.ApproverPositionID = ApprovalPositionID;
            //requestinprogram.DivisionID = ApprovalDivisionID;
            return View(requestinprogram);
        }
        [HttpPost]
        public ActionResult RequestInProgram(RequestInProgramDTO model)
        {
            try
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    var AssignID = db.vwPositionInPrograms.Where(a => a.DivisionID == model.UnitID).Select(a => a.PositionID).FirstOrDefault();
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
                            //REQUEST RELEASE TO DIVISI SDM
                            request.FlagID = 7;
                            request.ApprovalPositionID = 8;
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
                            return View("SuccessApprove");
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
                            var AssignIDForBudget = db.vwUserProfiles.Where(a => a.DivisionID == model.UnitID && a.LevelPositionID == 2).Select(a => a.PositionID).FirstOrDefault();
                            request.FlagID = 9;
                            request.FlagCreatedDate = DateTimeNow;
                            request.ApprovalPositionID = AssignID;
                            db.Entry(request).State = EntityState.Modified;
                            db.SaveChanges();

                            if (ServiceSkpd.SendEmailRequestBudgetToDivisionFromRequester(hostUrl, request, db, MessengerService))
                            {
                                transaction.Complete();
                                return View("SuccessSendRequestBudget");
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
                ModelState.AddModelError("", ex.Message);
            }
            return View(model);
        }


        public ViewResult SuccessApprove()
        {
            return View();
        }
        public ViewResult SuccessAddBudget()
        {
            return View();
        }
        public ViewResult SuccessReject()
        {
            return View();
        }
        public ViewResult SuccessSendRequestBudget()
        {
            return View();
        }


        public ViewResult RequestIsNotExist()
        {
            return View();
        }
        public ViewResult RequestIsAlreadyApproved()
        {
            return View();
        }
        public ViewResult RequestIsAlreadyRejected()
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
