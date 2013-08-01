using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace skpd.Attributes
{
    public class AJIIAuthorize : ActionFilterAttribute
    {
        public string division;
        public override void OnActionExecuting(ActionExecutingContext filterContext) 
        {
            HttpCookie cookie = filterContext.HttpContext.Request.Cookies.Get(".COOKIESMONSTER");
            FormsAuthenticationTicket tiket = FormsAuthentication.Decrypt(cookie.Value);

            if (cookie != null)
            {
                if (tiket.UserData == division)
                {
                    return;
                }
                else
                {
                    filterContext.Result = new RedirectResult("~/Error/AccessDenied");
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}