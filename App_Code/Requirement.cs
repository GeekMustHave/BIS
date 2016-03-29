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
/// Summary description for Requirement
/// </summary>
public class Requirement
{
    //spReqListDefault
    public static DataSet GetRequestListDefault(string eaGuid)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spReqListDefault");
        db.AddInParameter(cmd, "sEA_GUID", DbType.String, eaGuid);
        return db.ExecuteDataSet(cmd);
    }
    public static DataSet GetReqLabels(string eaGuid)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spLabelObject");
        db.AddInParameter(cmd, "sReq_GUID", DbType.String, eaGuid);
        return db.ExecuteDataSet(cmd);
    }
    public static DataSet GetRequestDetailView(string eaGuid)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spReqDetailView");
        db.AddInParameter(cmd, "sREQ_GUID", DbType.String, eaGuid);
        return db.ExecuteDataSet(cmd);
    }

    public static DataSet GetRequestListDetailed(string eaGuid, string searchText)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spReqListDetail");
        db.AddInParameter(cmd, "sEA_GUID", DbType.String, eaGuid);
        if (!(string.IsNullOrEmpty(searchText)))
            db.AddInParameter(cmd, "sSearchText", DbType.String, searchText);
        return db.ExecuteDataSet(cmd);
    }
    public static DataSet GetRequirementHistory(string eaGuid)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spReqHistory");
        db.AddInParameter(cmd, "ea_guid", DbType.String, eaGuid);
        return db.ExecuteDataSet(cmd);
    }
    public static void UpdateRequirementDetail(string reqGuid, string reqName, string notes, string complexityGuid, string typeGuid, string statusGuid, string updatedbyUId)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spReqDetailUpdate");
        if (!String.IsNullOrEmpty(reqGuid))
            db.AddInParameter(cmd, "sReq_GUID", DbType.String, reqGuid);
        db.AddInParameter(cmd, "sName", DbType.String, reqName);
        db.AddInParameter(cmd, "sNote", DbType.String, notes);
        db.AddInParameter(cmd, "sComplexity_GUID", DbType.String, complexityGuid);
        db.AddInParameter(cmd, "sNtype_GUID", DbType.String, typeGuid);
        db.AddInParameter(cmd, "sStatus_GUID", DbType.String, statusGuid);
        db.AddInParameter(cmd, "sCreatedBy", DbType.String, updatedbyUId);
        db.ExecuteNonQuery(cmd);
    }
    public static string CreateRequirementDetail(string packageGuid, string reqName, string notes, string updatedbyUId)
    {
        string newRequirementGuid = string.Empty;
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spReqDetailNew");

        db.AddInParameter(cmd, "sPackage_GUID", DbType.String, packageGuid);
        db.AddInParameter(cmd, "sName", DbType.String, reqName);
        db.AddInParameter(cmd, "sNote", DbType.String, notes);
        db.AddInParameter(cmd, "sCreatedBy", DbType.String, updatedbyUId);
        DataTable dt = db.ExecuteDataSet(cmd).Tables[0];
        try
        {
            if (dt.Rows.Count > 0)
                newRequirementGuid = dt.Rows[0]["ReqGuid"].ToString();
        }
        catch (Exception)
        {

        }
        return newRequirementGuid;
    }
    public static void AddRemoveLabesForRequirement(string currentReqID, string actionEvent, string tagName)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spLabelObjectUpdate");

        db.AddInParameter(cmd, "sCurrentObjID", DbType.String, currentReqID);
        db.AddInParameter(cmd, "sActionEvent", DbType.String, actionEvent);
        db.AddInParameter(cmd, "sTagName", DbType.String, tagName);
        db.ExecuteNonQuery(cmd);
    }

    public static string GetProjectPackageName(string eaGuid)
    {
        string prjPkgName = string.Empty;
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spProjPackName");
        db.AddInParameter(cmd, "sPackage_GUID", DbType.String, eaGuid);

        DataTable dt = db.ExecuteDataSet(cmd).Tables[0];
        try
        {
            if (dt.Rows.Count > 0)
                prjPkgName = dt.Rows[0]["ProjPackNamed"].ToString();
        }
        catch (Exception)
        {

        }
        return prjPkgName;
    }

    //spReqHistoryDetail
    public static DataSet GetReqHistoryDetails(string eguid, int versionNum)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spReqHistoryDetail");
        db.AddInParameter(cmd, "ea_guid", DbType.String, eguid);
        db.AddInParameter(cmd, "lVersion", DbType.Int32, versionNum);
        return db.ExecuteDataSet(cmd);
    }
}