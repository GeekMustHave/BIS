using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// This is a helper class to process Crystal reports across the app
/// </summary>
public class CrystalReportHelper
{
    private ReportDocument cryRpt;
    private ParameterFields myParams;
    private string connString;
    private System.Data.SqlClient.SqlConnectionStringBuilder conStringBuilder;

    public CrystalReportHelper()
    {
        //
        // TODO: Add constructor logic here
        connString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnString"].ConnectionString;
        conStringBuilder = new System.Data.SqlClient.SqlConnectionStringBuilder(connString);
        cryRpt = new ReportDocument();
        myParams = new ParameterFields();
    }
    /// <summary>
    /// Prepare the report source by passing in the path
    /// </summary>
    /// <param name="reportPath">Absoulte report physical  path</param>
    /// <returns>report document</returns>
    public ReportDocument PrepareReportSource(string reportPath)
    {
        cryRpt.Load(HttpContext.Current.Server.MapPath(reportPath));
        SetCRLogOnInfo(cryRpt, conStringBuilder.InitialCatalog, conStringBuilder.DataSource, conStringBuilder.UserID, conStringBuilder.Password);
        return cryRpt;
    }
    /// <summary>
    /// Returns the Currently prepared report document
    /// </summary>
    /// <returns></returns>
    public ReportDocument GetReportDocument()
    {
        return cryRpt;
    }
    /// <summary>
    /// Gets he parameter field info
    /// </summary>
    /// <returns></returns>
    public ParameterFields ParameterFieldInfo()
    {
        return myParams;
    }

    /// <summary>
    /// This will add parameter to the Main report.
    /// </summary>
    /// <param name="paramName">Name of the Input param used</param>
    /// <param name="paramValue">value - can be int, string double etc</param>
    public void AddParamater(string paramName, object paramValue)
    {
        // Create parameter objects        
        ParameterField myParam = new ParameterField();
        ParameterDiscreteValue myDiscreteValue = new ParameterDiscreteValue();
        
        myDiscreteValue.Value = paramValue;
        myParam.ParameterFieldName = paramName;                
        myParam.CurrentValues.Add(myDiscreteValue);

        // Add param object to params collection
        myParams.Add(myParam);
    }

    /// <summary>
    /// Recursive function to set logon creds for all the tables inside te Crystal report - incuding all the sub(sub) reports.
    /// </summary>
    /// <param name="inRptDoc"></param>
    /// <param name="databasename"></param>
    /// <param name="dataSource"></param>
    /// <param name="userId"></param>
    /// <param name="pwd"></param>
    private static void SetCRLogOnInfo(ReportDocument inRptDoc, string databasename, string dataSource, string userId, string pwd)
    {
        //mainInRD.SetDatabaseLogon(userId, pwd, dataSource, databasename, false);
        //do the main reports database
        TableLogOnInfo logonInfo = null;
        foreach (CrystalDecisions.CrystalReports.Engine.Table table in inRptDoc.Database.Tables)
        {
            logonInfo = table.LogOnInfo;
            logonInfo.ConnectionInfo.ServerName = dataSource;
            logonInfo.ConnectionInfo.DatabaseName = databasename;
            logonInfo.ConnectionInfo.UserID = userId;
            logonInfo.ConnectionInfo.Password = pwd;
            logonInfo.ConnectionInfo.Type = ConnectionInfoType.SQL;
            logonInfo.ConnectionInfo.IntegratedSecurity = false;

            table.ApplyLogOnInfo(logonInfo);
        }
        try
        {
            //now update logon info for all sub-reports
            if (!inRptDoc.IsSubreport && inRptDoc.Subreports != null && inRptDoc.Subreports.Count > 0)
            {
                foreach (ReportDocument rd in inRptDoc.Subreports)
                {
                    SetCRLogOnInfo(rd, databasename, dataSource, userId, pwd);
                }
            }
        }
        catch
        {
        }
    }
}