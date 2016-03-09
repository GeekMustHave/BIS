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
/// DAL Class to get the required Data Componenets for Packages Page.
/// </summary>
public class Package
{
    /// <summary>
    /// Function to Get all the Pacakges in the system
    /// </summary>
    /// <returns>List of all Packages in the system</returns>
    public static DataSet GetPackages()
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spProjectPackageList");
        //db.AddInParameter(cmd, "App_ID", DbType.String, AppId);
        return db.ExecuteDataSet(cmd);
    }
    /// <summary>
    /// Gets all the packages in the system for a DropDown
    /// </summary>
    /// <returns>A data set with all the packages with their name and projectguid.</returns>
    public static DataSet GetPackagesCombo()
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spPackagesCombo");
        return db.ExecuteDataSet(cmd);
    }

    /// <summary>
    /// Function to Get all the Pacakge Note Details
    /// </summary>
    /// <returns>Package Note Details</returns>
    public static DataSet GePackageNoteDetails(string eaGuid)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spPackageNote");
        db.AddInParameter(cmd, "ea_guid", DbType.String, eaGuid);
        return db.ExecuteDataSet(cmd);
    }
    /// <summary>
    /// Function to Get Package History
    /// </summary>
    /// <returns>List of all Packages in the system</returns>
    public static DataSet GePackageHistory(string eaGuid)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spPackageHistory");
        db.AddInParameter(cmd, "ea_guid", DbType.String, eaGuid);
        return db.ExecuteDataSet(cmd);
    }
    /// <summary>
    /// Function to Get Package Details of a History item
    /// </summary>
    /// <param name="eaGuid">package guid</param>
    /// <param name="versionNum">history version number</param>
    /// <returns>Package Details of history item</returns>
    public static DataSet GePackageNoteHistory(string eaGuid, int versionNum)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spPackageNoteHistory");
        db.AddInParameter(cmd, "ea_guid", DbType.String, eaGuid);
        db.AddInParameter(cmd, "lVersion", DbType.Int32, versionNum);
        return db.ExecuteDataSet(cmd);
    }
    /// <summary>
    /// Method to Update Package Details.
    /// </summary>
    /// <param name="eaGuid">pacakge guid</param>
    /// <param name="prkPkgName">package title</param>
    /// <param name="notes">package noes</param>
    /// <param name="updatedbyUId">updated by user guid</param>
    public static void UpdatePackage(string eaGuid, string prkPkgName, string notes, string updatedbyUId)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spPackageUpdate");
        db.AddInParameter(cmd, "sEA_GUID", DbType.String, eaGuid);
        db.AddInParameter(cmd, "sName", DbType.String, prkPkgName);
        db.AddInParameter(cmd, "sNotes", DbType.String, notes);
        db.AddInParameter(cmd, "sCreatedBy", DbType.String, updatedbyUId);
        db.ExecuteNonQuery(cmd);
    }

    /// <summary>
    /// Method to Create a New Package
    /// </summary>
    /// <param name="prkPkgName">Package title</param>
    /// <param name="notes">package notes</param>
    /// <param name="parentGuid">parent guid</param>
    /// <param name="updatedbyUId">user guid</param>
    public static void CreatePackage(string prkPkgName, string notes, string parentGuid, string updatedbyUId)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spPackageNew");
        if (!String.IsNullOrEmpty(parentGuid))
            db.AddInParameter(cmd, "sParent_GUID", DbType.String, parentGuid);
        db.AddInParameter(cmd, "sName", DbType.String, prkPkgName);
        db.AddInParameter(cmd, "sNotes", DbType.String, notes);
        db.AddInParameter(cmd, "sCreatedBy", DbType.String, updatedbyUId);
        db.ExecuteNonQuery(cmd);
    }
    /// <summary>
    /// Method to Update Package Notes.
    /// </summary>
    /// <param name="eaGuid">package guid</param>
    /// <param name="notes">pacakga notes</param>
    /// <param name="updatedbyUId">updated  by user guid</param>
    public static void UpdatePacakgeNote(string eaGuid, string notes, string updatedbyUId)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spPackageNoteUpdate");
        db.AddInParameter(cmd, "sEA_GUID", DbType.String, eaGuid);
        db.AddInParameter(cmd, "sNote", DbType.String, notes);
        db.AddInParameter(cmd, "sCreatedBy", DbType.String, updatedbyUId);
        db.ExecuteNonQuery(cmd);
    }
    /// <summary>
    /// Method to Update Package Status in the Grid
    /// </summary>
    /// <param name="eaGuid">project/package guid</param>
    /// <param name="statusGuid">satus guid</param>
    /// <param name="updatedbyUId">last updated  by guid</param>
    public static void UpdatePacakgeStatus(string eaGuid, string statusGuid, string updatedbyUId)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spPackageStatusUpdate");
        db.AddInParameter(cmd, "sEA_GUID", DbType.String, eaGuid);
        db.AddInParameter(cmd, "sStatus_GUID", DbType.String, statusGuid);
        db.AddInParameter(cmd, "sCreatedBy", DbType.String, updatedbyUId);
        db.ExecuteNonQuery(cmd);
    }
    /// <summary>
    /// function to get All the Business Rules in a package
    /// </summary>
    /// <param name="pkg_Guid">PAckage Guid</param>
    /// <returns>Data Set of Business rules for combo</returns>
    public static DataSet GetPackageBusinessRules(string pkg_Guid)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spBusinessRulesCombo");
        db.AddInParameter(cmd, "sPackage_GUID", DbType.String, pkg_Guid);
        return db.ExecuteDataSet(cmd);
    }    
    /// <summary>
    /// function to get All the Error Messages in a package
    /// </summary>
    /// <param name="pkg_Guid">PAckage Guid</param>
    /// <returns>Data Set of Error Messages for combo</returns>
    public static DataSet GetPackageErrorMessages(string pkg_Guid)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spErrorMessagesCombo");
        db.AddInParameter(cmd, "sPackage_GUID", DbType.String, pkg_Guid);
        return db.ExecuteDataSet(cmd);
    }
}