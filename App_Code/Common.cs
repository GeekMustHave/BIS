using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;

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
    /// validates the username and password 
    /// </summary>
    /// <param name="userIdentityName"></param>
    /// <param name="password"></param>
    /// <returns>1-Validated , 0 - invalidated, -1 -username not found</returns>
    public static int ValidateLoginCredentials(string userIdentityName, string password)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spCheckUser");
        db.AddInParameter(cmd, "sUserLogin", DbType.String, userIdentityName);
        db.AddInParameter(cmd, "sPassword", DbType.String, password);

        DataTable dtUserCred = db.ExecuteDataSet(cmd).Tables[0];
        if (dtUserCred.Rows.Count == 0)
        {
            return -1;
        }
        else
        {
            if (Convert.ToBoolean(dtUserCred.Rows[0]["Validated"]))
            {
                return 1;
            }
            else
            {
                return 0;
            }
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
    public static string PersistsCurrentUsrInfoNRetnRoles(string usrName)
    {        
        DataTable dtUsrInfo = Common.GetCurrentUserInfo(usrName).Tables[0];
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
                                  dtUsrInfo.Rows[0]["Surname"].ToString().Trim(), dtUsrInfo.Rows[0]["E_Mail"].ToString().Trim(),
                                  (string.IsNullOrEmpty(usrRoles)) ? "Open" : usrRoles);

        }

        return usrRoles;
    }
}