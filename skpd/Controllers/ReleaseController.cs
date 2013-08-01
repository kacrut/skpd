using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using skpd.Models;
using skpd.Services;
using System.Web.Routing;
using System.Transactions;
using System.Data;

namespace skpd.Controllers
{
    public class ReleaseController : Controller
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
        // GET: /Release/

        public ActionResult Index()
        {
            //var positionID = db.Users.Where(a => a.Username == User.Identity.Name).Select(a => a.PositionID).SingleOrDefault();
            return View(db.RequestReleases.Where(a => a.FlagID == 0));
        }

        public ActionResult Details(int id)
        {
            RequestRelease _RequestRelease = db.RequestReleases.Where(a => a.RequestID == id).SingleOrDefault();
            return View(_RequestRelease);
        }

        public ActionResult Edit(int id)
        {
            RequestRelease _RequestRelease = db.RequestReleases.Where(a => a.RequestID == id).SingleOrDefault();
            if (_RequestRelease == null)
            {
                return HttpNotFound();
            }
            return View(_RequestRelease);
        }
        [HttpPost]
        public ActionResult Edit(RequestRelease request)
        {
            try
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    RequestRelease model = db.RequestReleases.Where(a => a.RequestID == request.RequestID).SingleOrDefault();
                    model.StartDate = request.StartDate;
                    model.EndDate = request.EndDate;
                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();

                    transaction.Complete();
                }
                return View("SuccessEdit");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ViewResult NoSuccessEdit()
        {
            return View();
        }

        public ViewResult SuccessEdit()
        {
            return View();
        }

        public ActionResult Approve(int id)
        {
            try
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    RequestRelease request = db.RequestReleases.Where(a => a.RequestID == id).SingleOrDefault();
                    request.FlagID = 1;
                    request.FlagCreatedDate = DateTime.Now;
                    db.Entry(request).State = EntityState.Modified;
                    db.SaveChanges();

                    transaction.Complete();

                    return RedirectToAction("SuccessApprove", "Release");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View();
        }

        public ActionResult SuccessApprove()
        {
            return View();
        }

    }
}
