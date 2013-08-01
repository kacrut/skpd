using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using skpd.Models;
using skpd.Services;
using System.Web.Routing;

namespace skpd.Controllers
{
    public class HomeController : Controller
    {
        #region Initialize RequestContext
        public ESKAPEDEContext db = new ESKAPEDEContext();
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
            bool x = Request.IsAuthenticated;
            if (User.Identity.Name != string.Empty)
            {
                vwUserProfile user = db.vwUserProfiles.Where(a => a.Username == User.Identity.Name).SingleOrDefault();
                ViewBag.fullname = user.fullname;
                ViewBag.PositionName = user.PositionName;
                ViewBag.UnitName = user.UnitName;
                ViewBag.CountryName = user.CountryName;
                ViewBag.Assign = user.Assign;
                ViewBag.Email = user.Email;
                ViewBag.FaceUrl = user.FaceUrl;
            }
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
