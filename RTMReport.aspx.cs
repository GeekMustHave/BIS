using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class RTMReport : System.Web.UI.Page
{
    protected void Page_Init(object sender, EventArgs e)
    {
        //This is needed as the crystal report looses its credentials if there is a sub report on a different page (say page 2)
        if (pnlReportViewer.Visible == true && Session["svReportRTM"] != null)
        {
            ReportDocument reportDocument = (ReportDocument)Session["svReportRTM"];
            CrystalReportViewer1.ReportSource = reportDocument;
        }
    }

    /// <summary>
    /// Page Load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        //Load the drodpown list.
        if (!Page.IsPostBack)
        {
            //clear the other reports sessons whle load this page.
            Common.ClearReportSessions();
            BindPackagesDropDown();
            pnlReportViewer.Visible = false;
            if (Session["svSelectedPackage"] != null)
            {
                //select the current selected package.
                ddlPackage.SelectedValue = Session["svSelectedPackage"].ToString();
            }
        }
    }

    #region "Button Events"
    protected void btnRunReport_Click(object sender, EventArgs e)
    {
        CrystalReportHelper crHelper = new CrystalReportHelper();
        CrystalReportViewer1.ReportSource = crHelper.PrepareReportSource("~/Reports/RequirementsTraceabilityMatrixV6.rpt");
        crHelper.AddParamater("@sPackage_GUID", ddlPackage.SelectedValue.ToString());
        crHelper.AddParamater("VersionDesc", txtSubTitle.Text.Trim());
        CrystalReportViewer1.ParameterFieldInfo = crHelper.ParameterFieldInfo();

        int exportFormatFlags = (int)(CrystalDecisions.Shared.ViewerExportFormats.PdfFormat | CrystalDecisions.Shared.ViewerExportFormats.ExcelFormat);
        CrystalReportViewer1.AllowedExportFormats = exportFormatFlags;

        //Store the report document when the report has any subreports on other than page -1 
        Session["svReportRTM"] = crHelper.GetReportDocument();

        pnlReportViewer.Visible = true;
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        //Clear the I/P fields
        txtSubTitle.Text = String.Empty;
        ddlPackage.SelectedIndex = 0;
        pnlReportViewer.Visible = false;
    }
    #endregion

    #region "Supports"
    private void BindPackagesDropDown()
    {
        ddlPackage.Items.Clear();
        ddlPackage.DataSource = Package.GetPackagesCombo().Tables[0];
        ddlPackage.DataTextField = "PackageName";
        ddlPackage.DataValueField = "Package_GUID";
        ddlPackage.DataBind();

        ddlPackage.Items.Insert(0, new ListItem("** Select a Package", "null"));
    }
    #endregion
}