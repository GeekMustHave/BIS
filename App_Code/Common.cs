using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using System.Web.Security;

/// <summary>
/// Summary description for Common
/// </summary>
public class Common
{
    public static DataSet GetStatusCodes()
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spStatusCombo");
        return db.ExecuteDataSet(cmd);
    }
    public static DataSet GetUniqueLabels(string searchText)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spGetUniqueLabels");
        db.AddInParameter(cmd, "sSearchText", DbType.String, searchText);
        return db.ExecuteDataSet(cmd);
    }
    public static DataSet GetTypeCodes()
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spTypeCombo");
        return db.ExecuteDataSet(cmd);
    }
    public static DataSet GetComplexityCodes()
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spComplexityCombo");
        return db.ExecuteDataSet(cmd);
    }
    public static DataSet GetLabelCodes()
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spLabelCombo");
        return db.ExecuteDataSet(cmd);
    }
    public static DataSet GetCurrentUserInfo(string userIdentityName)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spUserDetails");
        db.AddInParameter(cmd, "sUserLogin", DbType.String, userIdentityName);
        return db.ExecuteDataSet(cmd);
    }
    /// <summary>
    /// Sub to run when user logins or reauthenticates.
    /// </summary>
    /// <param name="userIdentityName"></param>
    public static void UpdateCurrentUserLastLogin(string userIdentityName)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spUserUpdateLastLogin");

        db.AddInParameter(cmd, "sUserLogin", DbType.String, userIdentityName);
        db.ExecuteNonQuery(cmd);
    }
    /// <summary>
    /// validates the username and password 
    /// </summary>
    /// <param name="userIdentityName"></param>
    /// <param name="password"></param>
    /// <returns>1-Validated , 0 - invalidated, 2 for inactive, -1 -username not found</returns>
    public static int ValidateLoginCredentials(string userIdentityName, string password)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spUserValidate");
        db.AddInParameter(cmd, "sUserLogin", DbType.String, userIdentityName);
        db.AddInParameter(cmd, "sPassword", DbType.String, password);

        DataTable dtUserCred = db.ExecuteDataSet(cmd).Tables[0];
        if (dtUserCred.Rows.Count == 0)
        {
            return -1;
        }
        else
        {
            return Convert.ToInt32(dtUserCred.Rows[0]["Validated"]);
        }
    }
    /// <summary>
    /// Sub to Create a new user from User Registration Page
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="password"></param>
    /// <param name="firstName"></param>
    /// <param name="lastName"></param>
    /// <param name="Email"></param>
    /// <param name="Department"></param>
    /// <returns>status message - "Sucess" or some other Error.</returns>
    public static string CreateNewUser(string userName, string password, string firstName, string lastName, string Email, string Department)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spUserNew");
        db.AddInParameter(cmd, "sUserLoginNew", DbType.String, userName);
        db.AddInParameter(cmd, "sPassword", DbType.String, password);
        db.AddInParameter(cmd, "sFirstName", DbType.String, firstName);
        db.AddInParameter(cmd, "sLastName", DbType.String, lastName);
        db.AddInParameter(cmd, "sEmail", DbType.String, Email);
        db.AddInParameter(cmd, "sDepartment", DbType.String, Department);
        db.AddOutParameter(cmd, "sResponseMessage", DbType.String, 250);
        db.ExecuteNonQuery(cmd);

        return db.GetParameterValue(cmd, "@sResponseMessage").ToString();
    }
    public static string UpdateUser(string userGuid, string userName, string firstName, string lastName, string Email, string Department, bool Active, string roles)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spUserUpdate");
        db.AddInParameter(cmd, "sUserGUID", DbType.String, userGuid);
        db.AddInParameter(cmd, "sUserLoginNew", DbType.String, userName);
        db.AddInParameter(cmd, "sFirstName", DbType.String, firstName);
        db.AddInParameter(cmd, "sLastName", DbType.String, lastName);
        db.AddInParameter(cmd, "sEmail", DbType.String, Email);
        db.AddInParameter(cmd, "sDepartment", DbType.String, Department);
        db.AddInParameter(cmd, "bActiveFlag", DbType.Boolean, Active);
        db.AddInParameter(cmd, "sRoles", DbType.String, roles);
        db.AddOutParameter(cmd, "sResponseMessage", DbType.String, 250);
        db.ExecuteNonQuery(cmd);

        return db.GetParameterValue(cmd, "@sResponseMessage").ToString();
    }
    /// <summary>
    /// Sub to REset users passwrd
    /// </summary>
    /// <param name="userName">users username</param>
    /// <param name="Email">users email</param>
    /// <param name="passwordToken">Temoporary token generated and emailed</param>
    /// <returns></returns>
    public static string ResetPassword(string userName, string Email, string passwordToken, ref DataTable UserDetails)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spUserPasswordReset");
        if (!String.IsNullOrEmpty(userName))
            db.AddInParameter(cmd, "sUserLogin", DbType.String, userName);
        db.AddInParameter(cmd, "sPasswordToken", DbType.String, passwordToken);
        if (!String.IsNullOrEmpty(Email))
            db.AddInParameter(cmd, "sEmail", DbType.String, Email);
        db.AddOutParameter(cmd, "sResponseMessage", DbType.String, 250);
        //db.ExecuteNonQuery(cmd);
        DataSet ds = db.ExecuteDataSet(cmd);
        if (ds.Tables.Count > 0)
        {
            UserDetails = ds.Tables[0];
        }

        return db.GetParameterValue(cmd, "@sResponseMessage").ToString();
    }
    public static void SendPasswordResetEmailToUser(string UserName, string inEmail, string TempPassword)
    {
        try
        {
            string emailSubject = "BIS Temporary Password";
            string emailBody = "Your password has been reset temporarily. <br />";
            emailBody += "UserName: " + UserName + "<br /> Temporary Password : " + TempPassword;
            emailBody += "<br /> Please change your password after you login.";
            emailBody += "<br /> <br /> - BIS-Administrator";
            Email.SendEmail(inEmail, emailSubject, emailBody);
        }
        catch (Exception)
        {
            throw;
        }
    }
    /// <summary>
    /// Function to Get ADMIN user's EMailID from the daatabase
    /// </summary>
    /// <returns></returns>
    public static string GetAdminEmailID()
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spUserGetAdminEmail");        
        db.AddOutParameter(cmd, "sAdminEmail", DbType.String, 100);
        db.ExecuteNonQuery(cmd);

        return db.GetParameterValue(cmd, "@sAdminEmail").ToString();
    }

    /// <summary>
    /// Sub to update the users password
    /// </summary>
    /// <param name="userName">username</param>
    /// <param name="oldPassword">old password</param>
    /// <param name="newPassword">new password</param>
    /// <returns>response as success or error message</returns>
    public static string ChangePassword(string userGUID, string oldPassword, string newPassword)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spUserPasswordUpdate");
        db.AddInParameter(cmd, "sUserGUID", DbType.String, userGUID);
        db.AddInParameter(cmd, "sPasswordOld", DbType.String, oldPassword);
        db.AddInParameter(cmd, "sPasswordNew", DbType.String, newPassword);
        db.AddOutParameter(cmd, "sResponseMessage", DbType.String, 250);
        db.ExecuteNonQuery(cmd);

        return db.GetParameterValue(cmd, "@sResponseMessage").ToString();
    }
    public static void DoLogin(string username, string password, bool rememberMeChecked, ref string outMessage)
    {

        string usrName = username;
        int validationResult = ValidateUser(usrName, password);
        if (validationResult == 1)
        {
            //clear any other tickets that are already in the response
            HttpContext.Current.Response.Cookies.Clear();
            // Get the User Info and store it in session.
            string usrRoles = string.Empty;

            usrRoles = Common.PersistsCurrentUsrInfoNRetnRoles(usrName);
            //usrRoles = (string.IsNullOrEmpty(usrRoles)) ? "Open" : usrRoles; //CompileUseRoles(usrName);

            FormsAuthenticationTicket tkt;
            string cookiestr;
            HttpCookie ck;
            tkt = new FormsAuthenticationTicket(2, usrName, DateTime.Now, DateTime.Now.AddDays(365),
                                                rememberMeChecked, usrRoles, FormsAuthentication.FormsCookiePath);

            cookiestr = FormsAuthentication.Encrypt(tkt);
            ck = new HttpCookie(FormsAuthentication.FormsCookieName, cookiestr);
            if (rememberMeChecked)
                ck.Expires = tkt.Expiration;
            else
                ck.Expires = DateTime.Now.AddMinutes(HttpContext.Current.Session.Timeout);
            ck.Path = FormsAuthentication.FormsCookiePath;
            HttpContext.Current.Response.Cookies.Add(ck);

            //Add  roles to thePrincipal in Global Asax.
            string strRedirect;
            strRedirect = HttpContext.Current.Request.QueryString["ReturnUrl"];
            if (strRedirect == null)
                strRedirect = "~/Packages.aspx";
            HttpContext.Current.Response.Redirect(strRedirect, true);
        }
        else if (validationResult == 0)
        {
            outMessage = "Invalid Username or Password. Try again.";
            //Response.Redirect("Login.aspx", true);
        }
        else if (validationResult == -1)
        {
            outMessage = "UserName not Recognized. Try registering first.";
        }
        else if (validationResult == 2)
        {
            outMessage = "UserID is no longer active. Contact the Site Administrator.";
        }
        else if (validationResult == -99)
        {
            outMessage = "Cannot Connect to the DataBase. Contact the Site Administrator.";
        }
        else
        {
            HttpContext.Current.Response.Redirect("~/UnAuthorized.aspx", true);
        }
    }
    private static int ValidateUser(string username, string password)
    {
        try
        {
            return Common.ValidateLoginCredentials(username, password);
        }
        catch (Exception)
        {
            return -99;
        }

    }

    public static DataSet GetProjectsList()
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spProjectList");
        return db.ExecuteDataSet(cmd);
    }

    public static void AddNewImage(string objectguid, string imgName, string imgType, int imgSize, string imageData, string Note = "")
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spImageAddNew");
        db.AddInParameter(cmd, "sObject_PK", DbType.String, objectguid);
        db.AddInParameter(cmd, "sName", DbType.String, imgName);
        db.AddInParameter(cmd, "sType", DbType.String, imgType);
        db.AddInParameter(cmd, "sNote", DbType.String, Note);
        db.AddInParameter(cmd, "iSize", DbType.Int64, imgSize);
        int index = imageData.IndexOf("base64,") + 7;
        string baseed64 = imageData.Substring(index);
        byte[] bytes = Convert.FromBase64String(baseed64);

        db.AddInParameter(cmd, "sImageIs", DbType.Binary, bytes);
        db.ExecuteNonQuery(cmd);
    }

    public static void InsertImageToReq(string objectguid, string imgName, string imgType, int imgSize, byte[] imageData, string Note = "")
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spImageAddNew");
        db.AddInParameter(cmd, "sObject_PK", DbType.String, objectguid);
        db.AddInParameter(cmd, "sName", DbType.String, imgName);
        db.AddInParameter(cmd, "sType", DbType.String, imgType);
        db.AddInParameter(cmd, "sNote", DbType.String, Note);
        db.AddInParameter(cmd, "iSize", DbType.Int64, imgSize);
        db.AddInParameter(cmd, "sImageIs", DbType.Binary, imageData);
        db.ExecuteNonQuery(cmd);
    }
    public static DataTable GetAFile(int DocId)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spImageGetDetails");
        db.AddInParameter(cmd, "iImageID", DbType.Int64, DocId);

        return db.ExecuteDataSet(cmd).Tables[0];
    }

    public static void DeleteImageFromReq(int objectPKId)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spImageDeleteFromObject");
        db.AddInParameter(cmd, "sObject_PK", DbType.Int32, objectPKId);
        db.ExecuteNonQuery(cmd);
    }

    public static DataSet GetReqComboList(string eaguid)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spReqCombo");
        db.AddInParameter(cmd, "sEA_GUID", DbType.String, eaguid);
        return db.ExecuteDataSet(cmd);
    }

    public static DataSet GetObjectLabels(string eaGuid)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spLabelObject");
        db.AddInParameter(cmd, "sReq_GUID", DbType.String, eaGuid);
        return db.ExecuteDataSet(cmd);
    }

    public static void UpdateObjectLabels(string currentobjguID, string actionEvent, string tagName)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spLabelObjectUpdate");

        db.AddInParameter(cmd, "sCurrentObjID", DbType.String, currentobjguID);
        db.AddInParameter(cmd, "sActionEvent", DbType.String, actionEvent);
        db.AddInParameter(cmd, "sTagName", DbType.String, tagName);
        db.ExecuteNonQuery(cmd);
    }
    /// <summary>
    /// Fucntin to clear reports sessions used on reports pages.
    /// </summary>
    public static void ClearReportSessions()
    {
        HttpContext.Current.Session.Remove("svReportUCDocument");
        HttpContext.Current.Session.Remove("svReportRTM");
        HttpContext.Current.Session.Remove("svReportRVD");
        HttpContext.Current.Session.Remove("svReportUCD");
    }
    /// <summary>
    /// Use this as a login compoment for User. Get the Authenticated User Details and update the LastLogin Date
    /// </summary>
    /// <param name="usrName">USerName</param>
    /// <returns>User Roles</returns>
    public static string PersistsCurrentUsrInfoNRetnRoles(string usrName)
    {
        DataTable dtUsrInfo = Common.GetCurrentUserInfo(usrName).Tables[0];
        Common.UpdateCurrentUserLastLogin(usrName);
        CurrentUser usr;
        string usrRoles = string.Empty;
        if (dtUsrInfo.Rows.Count == 0)
        {
            usr = new CurrentUser();
        }
        else
        {
            usrRoles = dtUsrInfo.Rows[0]["Roles"].ToString().Trim();
            usr = new CurrentUser(dtUsrInfo.Rows[0]["User_GUID"].ToString().Trim(), dtUsrInfo.Rows[0]["FirstName"].ToString().Trim(),
                                  dtUsrInfo.Rows[0]["Surname"].ToString().Trim(), dtUsrInfo.Rows[0]["E_Mail"].ToString().Trim(), usrRoles);

        }
        return usrRoles;
    }
}