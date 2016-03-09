using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using System.Configuration;
using System.Data.SqlClient;

/// <summary>
/// Reports Page.
/// History:
/// -----------
/// GS    8/15/2015     Proof Of Concept for Crystal Reports in BIS Web App.
/// </summary>
public partial class Admin_Reports : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string connString = ConfigurationManager.ConnectionStrings["DefaultConnString"].ConnectionString;
        SqlConnectionStringBuilder conStringBuilder =  new SqlConnectionStringBuilder(connString);

        ReportDocument cryRpt = new ReportDocument();
        cryRpt.Load(Server.MapPath("~/Reports/Packages.rpt"));
        cryRpt.SetDatabaseLogon(conStringBuilder.UserID, conStringBuilder.Password);//, conStringBuilder.DataSource, conStringBuilder.InitialCatalog);
        CrystalReportViewer1.ReportSource = cryRpt;  
    }
}