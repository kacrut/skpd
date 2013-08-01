using System;
using System.Web.Security;
using WebMatrix.WebData;
using WebMatrix.Data;

namespace skpd.Services
{
	public class WebSecurityService : IWebSecurityService
	{
		#region IWebSecurityService Members

		public bool ChangePassword(string userName, string currentPassword, string newPassword)
		{
			return WebSecurity.ChangePassword(userName, currentPassword, newPassword);
		}

		public bool ConfirmAccount(string accountConfirmationToken)
		{
			return WebSecurity.ConfirmAccount(accountConfirmationToken);
		}

		public string CreateAccount(string userName, string password, bool requireConfirmationToken = false)
		{
			return WebSecurity.CreateAccount(userName, password, requireConfirmationToken);
		}

        public string CreateUserAndAccount(string userName, string password, string nIK, string firstName, string lastName, string rekening, string email, int positionID, object propertyValues = null, bool requireConfirmationToken = false)
        {
            return WebSecurity.CreateUserAndAccount(userName, password, new { NIK = nIK ,FirstName = firstName, LastName = lastName, Rekening = rekening, Email = email, PositionID = positionID }, requireConfirmationToken);
        }

		public string GeneratePasswordResetToken(string userName, int tokenExpirationInMinutesFromNow = 0x5a0)
		{
			return WebSecurity.GeneratePasswordResetToken(userName, tokenExpirationInMinutesFromNow);
		}

		public DateTime GetCreateDate(string userName)
		{
			return WebSecurity.GetCreateDate(userName);
		}

		public DateTime GetLastPasswordFailureDate(string userName)
		{
			return WebSecurity.GetLastPasswordFailureDate(userName);
		}

		public DateTime GetPasswordChangedDate(string userName)
		{
			return WebSecurity.GetPasswordChangedDate(userName);
		}

		public int GetPasswordFailuresSinceLastSuccess(string userName)
		{
			return WebSecurity.GetPasswordFailuresSinceLastSuccess(userName);
		}

		public int GetUserId(string userName)
		{
			return WebSecurity.GetUserId(userName);
		}

		public int GetUserIdFromPasswordResetToken(string token)
		{
			return WebSecurity.GetUserIdFromPasswordResetToken(token);
		}

		public void InitializeDatabaseConnection(string connectionStringName, string userTableName, string userIdColumn, string userNameColumn, bool autoCreateTables)
		{
			WebSecurity.InitializeDatabaseConnection(connectionStringName, userTableName, userIdColumn, userNameColumn, autoCreateTables);
		}

		public void InitializeDatabaseConnection(string connectionString, string providerName, string userTableName, string userIdColumn, string userNameColumn, bool autoCreateTables)
		{
			WebSecurity.InitializeDatabaseConnection(connectionString, providerName, userTableName, userIdColumn, userNameColumn, autoCreateTables);
		}

		public bool IsAccountLockedOut(string userName, int allowedPasswordAttempts, int intervalInSeconds)
		{
			return WebSecurity.IsAccountLockedOut(userName, allowedPasswordAttempts, intervalInSeconds);
		}

		public bool IsAccountLockedOut(string userName, int allowedPasswordAttempts, TimeSpan interval)
		{
			return WebSecurity.IsAccountLockedOut(userName, allowedPasswordAttempts, interval);
		}

		public bool IsConfirmed(string userName)
		{
			return WebSecurity.IsConfirmed(userName);
		}

		public bool IsCurrentUser(string userName)
		{
			return WebSecurity.IsCurrentUser(userName);
		}

		public bool Login(string userName, string password, bool persistCookie = false)
		{
			return WebSecurity.Login(userName, password, persistCookie);
		}

		public void Logout()
		{
			WebSecurity.Logout();
		}

		public void RequireAuthenticatedUser()
		{
			WebSecurity.RequireAuthenticatedUser();
		}

		public void RequireRoles(params string[] roles)
		{
			WebSecurity.RequireRoles(roles);
		}

		public void RequireUser(int userId)
		{
			WebSecurity.RequireUser(userId);
		}

		public void RequireUser(string userName)
		{
			WebSecurity.RequireUser(userName);
		}

		public bool ResetPassword(string passwordResetToken, string newPassword)
		{
			return WebSecurity.ResetPassword(passwordResetToken, newPassword);
		}

		public bool UserExists(string userName)
		{
			return WebSecurity.UserExists(userName);
		}

		public int CurrentUserId
		{
			get { return WebSecurity.CurrentUserId; }
		}

		public string CurrentUserName
		{
			get { return WebSecurity.CurrentUserName; }
		}

		public bool HasUserId
		{
			get { return WebSecurity.HasUserId; }
		}

		public bool IsAuthenticated
		{
			get { return WebSecurity.IsAuthenticated; }
		}

		public int MinPasswordLength
		{
			get { return Membership.Provider.MinRequiredPasswordLength; }
		}

		#endregion

        #region Customs
        public string GetUserEmail(string userName)
        {
            dynamic email;
            using (var db = Database.Open("ESKAPEDEContext"))
            {
                email = db.QueryValue("SELECT email FROM [User] where Username = @0",userName);
                return email;
            }
        }

        public string GetUserFromToken(string token)
        {
            dynamic Username;
            using (var db = Database.Open("ESKAPEDEContext"))
            {
                Username = db.QueryValue("SELECT Username FROM [User] a "+
                                        "inner join webpages_Membership b "+
                                        "on b.UserID=a.ID where b.ConfirmationToken = @0", token);
                return Username;
            }
        }

        public string GetUserDivision(string userName)
        {
            dynamic Divison;
            using (var db = Database.Open("ESKAPEDEContext"))
            {
                Divison = db.QueryValue("select c.DivisionName from [user] u " +
                                            "inner join skpdPosition a on u.PositionID=a.PositionID " +
                                            "inner join organization.Area b on a.AreaID=b.AreaID " +
                                            "inner join organization.Division c on a.DivisionID=c.DivisionID " +
                                            "where u.Username= @0", userName);
                return Divison;
            }
        }
        #endregion

    }
}