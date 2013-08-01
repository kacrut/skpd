using System;
using System.Configuration;
using System.Net.Mail;

namespace skpd.Services
{
	public class MessengerService : IMessengerService
	{
		#region IMessengerService Members

		public bool Send(string from, string to, string subject, string body, bool isBodyHtml,string[] cc = null)
		{
			var isSuccess = false;
			var DisplayName = ConfigurationManager.AppSettings["DisplayEmailName"];
			try
			{
                MailMessage msg = new MailMessage();
                msg.To.Add(new MailAddress(to));
                msg.From = new MailAddress(from, DisplayName);
                msg.Subject = subject;
                msg.Body = body;
                msg.Priority = MailPriority.High;
                if(cc !=null)
                {
                    foreach(var _cc in cc)
                    {
                        msg.CC.Add(new MailAddress(_cc));
                    }                    
                }
				msg.IsBodyHtml = isBodyHtml;
                var smtp = new SmtpClient();
                //smtp.Host = "smtp.live.com";
                //smtp.EnableSsl = true;
                //smtp.Port = 587;
                //smtp.Credentials = new System.Net.NetworkCredential("ahmad.irf4n@live.com", "Irf4n@live");
				smtp.Send(msg);
				isSuccess = true;
			}
			catch (Exception ex)
			{
				//Log exception
                ex.ToString() ;
			}
		
			return isSuccess;
		}

		#endregion
	}
}