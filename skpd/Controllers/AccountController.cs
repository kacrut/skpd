using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using skpd.Models;
using skpd.Services;
using System.Web.Routing;
using System.Configuration;
using System.Transactions;
using System;
using System.Web.SessionState;
using System.Web.Security;
using skpd.DTO;

namespace skpd.Controllers
{
    [SessionState(SessionStateBehavior.Disabled)]
	public class AccountController : Controller
	{
        #region Initialize RequestContext
        public ESKAPEDEContext db { get; set; }
        public IWebSecurityService WebSecurityService { get; set; }
        public IMessengerService MessengerService { get; set; }
        protected override void Initialize(RequestContext requestContext)
        {
            if (db == null) { db = new ESKAPEDEContext(); }
            if (WebSecurityService == null) { WebSecurityService = new WebSecurityService(); }
            if (MessengerService == null) { MessengerService = new MessengerService(); }

            base.Initialize(requestContext);
        }
        #endregion
        //
        // GET: /FromEmail/

		// **************************************
		// URL: /Account/LogOn
		// **************************************

		public ActionResult LogOn()
		{
			return View();
		}

		[HttpPost]
		public ActionResult LogOn(LogOnModel model, string returnUrl)
		{
			if (ModelState.IsValid)
			{
				if (WebSecurityService.Login(model.UserName, model.Password, model.RememberMe))
				{

                    //var division = (from x in db.Users.Where(a => a.Username == model.UserName)
                    //                join y in db.Positions on x.PositionID equals y.PositionID
                    //                select y.DivisionID).SingleOrDefault();


                    //var user = db.webpages_UsersInRoles
                    //            .Where(a=>a.User.Username == model.UserName).Select(a=>a.webpages_Roles.RoleName).SingleOrDefault();


                    //FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                    //model.UserName,
                    //DateTime.Now,
                    //DateTime.Now.AddMinutes(FormsAuthentication.Timeout.TotalMinutes),
                    //model.RememberMe,
                    //user,
                    //FormsAuthentication.FormsCookiePath);

                    //// Encrypt the ticket.
                    //string encTicket = FormsAuthentication.Encrypt(ticket);

                    //// Create the cookie.
                    //Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

					if (Url.IsLocalUrl(returnUrl))
					{
						return Redirect(returnUrl);
					}
					else
					{
                        return RedirectToAction("Index", "Home");
					}
				}
				else
				{
                    if (WebSecurityService.UserExists(model.UserName))
                    {
                        if (WebSecurityService.IsConfirmed(model.UserName))
                        {
                            if (WebSecurityService.IsAccountLockedOut(model.UserName, 2, 6000))
                            {
                                var isValid = false;
			                    var resetToken = string.Empty;

                                if (ModelState.IsValid)
                                {
                                    if (WebSecurityService.GetUserId(model.UserName) > -1 && WebSecurityService.IsConfirmed(model.UserName))
                                    {
                                        resetToken = WebSecurityService.GeneratePasswordResetToken(model.UserName);
                                        isValid = true;
                                    }

                                    if (isValid)
                                    {
                                        string hostUrl = Request.Url.GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped);
                                        string resetUrl = hostUrl + VirtualPathUtility.ToAbsolute("~/Account/PasswordReset?resetToken=" + HttpUtility.UrlEncode(resetToken));

                                        var fromAddress = ConfigurationManager.AppSettings["Sender"];
                                        var toAddress = WebSecurityService.GetUserEmail(model.UserName);
                                        var subject = "Permintaan reset password";
                                        var body = string.Format("Hi, {0} <br />Gunakan token reset ini untuk mereset password anda. <br/><br />Token : {1}<br/><br />Klik >> <a href='{2}'>{2}</a>", model.UserName, resetToken, resetUrl);

                                        MessengerService.Send(fromAddress, toAddress, subject, body, true);
                                    }
                                    return RedirectToAction("ForgotPasswordMessage");
                                }
                                else
                                {
                                    // Navigate back to the homepage and exit
                                    WebSecurityService.Login(model.UserName, model.Password);
                                    return RedirectToAction("Index", "Home");
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("", "Password salah!");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "Akun anda belum aktif!..");
                            ModelState.AddModelError("", "Kami sudah mengirimkan kode pengaktifan");
                            ModelState.AddModelError("", "Cek Email Anda!");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Anda belum terdaftar!");
                    }
				}
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}

		// **************************************
		// URL: /Account/LogOff
		// **************************************

		public ActionResult LogOff()
		{
			WebSecurityService.Logout();

			return RedirectToAction("Index", "Home");
		}

		// **************************************
		// URL: /Account/Register
		// **************************************

		public ActionResult Register()
		{
			ViewBag.PasswordLength = WebSecurityService.MinPasswordLength;
			return View();
		}

		[HttpPost]
		public ActionResult Register(RegisterModel model)
		{
			if (ModelState.IsValid)
			{
				// Attempt to register the user
                //var requireEmailConfirmation = true;
                //var token = WebSecurityService.CreateUserAndAccount(model.UserName, model.Password,requireConfirmationToken: requireEmailConfirmation);

                //if (requireEmailConfirmation)
                //{
                //    // Send email to user with confirmation token
                //    //string hostUrl = Request.Url.GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped);
                //    //string confirmationUrl = hostUrl + VirtualPathUtility.ToAbsolute("~/Account/Confirm?confirmationCode=" + HttpUtility.UrlEncode(token));

                //    //var fromAddress = ConfigurationManager.AppSettings["Sender"];
                //    //var toAddress = model.Email;
                //    //var subject = "Thanks for registering but first you need to confirm your registration...";
                //    //var body = string.Format("Your confirmation code is: {0}. Visit <a href=\"{1}\">{1}</a> to activate your account.", token, confirmationUrl);
					
                //    // NOTE: This is just for sample purposes
                //    // It's generally a best practice to not send emails (or do anything on that could take a long time and potentially fail)
                //    // on the same thread as the main site
                //    // You should probably hand this off to a background MessageSender service by queueing the email, etc.
                //    //if (!MessengerService.Send(fromAddress, toAddress, subject, body, true))
                //    //{
                //    //    return RedirectToAction("NoThanks", "Account");
                //    //}
					
                //    // Thank the user for registering and let them know an email is on its way
                //    return RedirectToAction("Thanks", "Account");
                //}
                //else
                //{
                //    // Navigate back to the homepage and exit
                //    WebSecurityService.Login(model.UserName, model.Password);
                //    return RedirectToAction("Index", "Home");
                //}
			}

			// If we got this far, something failed, redisplay form
			ViewBag.PasswordLength = WebSecurityService.MinPasswordLength;
			return View(model);
		}
		
		public ActionResult Confirm()
		{
			string confirmationToken = Request.QueryString["confirmationCode"];
			WebSecurityService.Logout();

			if (!string.IsNullOrEmpty(confirmationToken)) 
			{
				if (WebSecurityService.ConfirmAccount(confirmationToken)) 
				{
					ViewBag.Message = "Registration Confirmed! Click on the login link at the top right of the page to continue.";
				} else {
					ViewBag.Message = "Could not confirm your registration info";
				}
			}

			return View();
		}

        public ActionResult ConfirmAndSetNewPassword()
        {
            return View();
        }


        // **************************************
        // URL: /Account/ConfirmAndSetNewPassword
        // **************************************
        [HttpPost]
        public ActionResult ConfirmAndSetNewPassword(FirstTimeLogin model)
        {
            try
            {
                string confirmationToken = model.Token;
                string password = model.NewPassword;

                if (!string.IsNullOrEmpty(confirmationToken))
                {
                    if (ModelState.IsValid)
                    {
                        if (WebSecurityService.ConfirmAccount(confirmationToken))
                        {
                            string Username = WebSecurityService.GetUserFromToken(model.Token);
                            string resetToken = WebSecurityService.GeneratePasswordResetToken(Username);

                            if (WebSecurityService.ResetPassword(resetToken, password))
                            {
                                return RedirectToAction("ConfirmAndSetNewPasswordSuccess");
                            }
                            else
                            {
                                ModelState.AddModelError("", "Token tidak valid!");
                            }
                            ViewBag.Message = "Registration Confirmed! Click on the login link at the top right of the page to continue.";
                        }
                        else
                        {
                            ModelState.AddModelError("", "Token tidak valid!");
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Kami tidak menemukan token konfirmasi anda");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View();
        }

        public ActionResult ConfirmAndSetNewPasswordSuccess()
        {
            return View();
        }



		// **************************************
		// URL: /Account/ChangePassword
		// **************************************

		[Authorize]
		public ActionResult ChangePassword()
		{
			ViewBag.PasswordLength = WebSecurityService.MinPasswordLength;
			return View();
		}

		[Authorize]
		[HttpPost]
		public ActionResult ChangePassword(ChangePasswordModel model)
		{
			if (ModelState.IsValid)
			{
				if (WebSecurityService.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword))
				{
					return RedirectToAction("ChangePasswordSuccess");
				}
				else
				{
					ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
				}
			}

			// If we got this far, something failed, redisplay form
			ViewBag.PasswordLength = WebSecurityService.MinPasswordLength;
			return View(model);
		}
		
		public ActionResult ForgotPassword()
		{
			return View();
		}

		[HttpPost]
		public ActionResult ForgotPassword(ForgotPasswordModel model)
		{
			var isValid = false;
			var resetToken = string.Empty;

			if (ModelState.IsValid)
			{
				if (WebSecurityService.GetUserId(model.UserName) > -1 && WebSecurityService.IsConfirmed(model.UserName))
				{
                    if (WebSecurityService.GetUserEmail(model.UserName) == model.Email)
                    {
                        resetToken = WebSecurityService.GeneratePasswordResetToken(model.UserName);
                        isValid = true;
                    }
                    else
                    {
                        ModelState.AddModelError("", "Email username tidak sesuai");
                    }
				}

				if (isValid)
				{
					string hostUrl = Request.Url.GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped);
					string resetUrl = hostUrl + VirtualPathUtility.ToAbsolute("~/Account/PasswordReset?resetToken=" + HttpUtility.UrlEncode(resetToken));

                    var fromAddress = ConfigurationManager.AppSettings["Sender"];
					var toAddress = model.Email;
					var subject = "Password reset request";
					var body = string.Format("Use this password reset token to reset your password. <br/>The token is: {0}<br/>Visit <a href='{1}'>{1}</a> to reset your password.<br/>", resetToken, resetUrl);

					MessengerService.Send(fromAddress, toAddress, subject, body, true);
                    return RedirectToAction("ForgotPasswordMessage");
				}
			}
			return View(model);
		}

		public ActionResult ForgotPasswordMessage()
		{
			return View();
		}

		public ActionResult PasswordReset()
		{
			return View();
		}

		[HttpPost]
		public ActionResult PasswordReset(PasswordResetModel model)
		{
			if (ModelState.IsValid)
			{
				if (WebSecurityService.ResetPassword(model.ResetToken, model.NewPassword))
				{
					return RedirectToAction("PasswordResetSuccess");
				}
				else
				{
					ModelState.AddModelError("","The password reset token is invalid.");
				}
			}

			return View(model);
		}

		public ActionResult PasswordResetSuccess()
		{
			return View();
		}

		// **************************************
		// URL: /Account/ChangePasswordSuccess
		// **************************************

		public ActionResult ChangePasswordSuccess()
		{
			return View();
		}

		public ActionResult Thanks()
		{
			return View();
		}

        public ActionResult NoThanks()
        {
            return View();
        }
	}
}
