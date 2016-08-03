using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;

/// <summary>
/// Summary: Page to display all Packages. It's the default page when a user Logs in.
/// History:
/// -------------------------------------------------------------------------------
/// Date:               Name                Description
/// --------------------------------------------------------------------------------
/// 7/10/2015           JHRS                Intial Creation
/// </summary>
public partial class _Packages : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // For temp. REmove the following LOC after publish to PROD.
        lblRoles.Text = (HttpContext.Current.User.IsInRole("Admin")) ? "User has Admin Role" : "User Has Open Role";
        if (!Page.IsPostBack)
        {
            //Bind the packages to the grid for the first time.
            BindPackages();
        }
    }

    #region "Button Events"
    protected void btnSearchProjPackage_Click(object sender, EventArgs e)
    {
        BindPackages();
    }
    protected void btnAddProject_Click(object sender, EventArgs e)
    {
        //reset the controls for the user to create new Project. and show the popup
        PrepareForCreateProject();
        mpCreatePrjPkg.Show();
    }
    protected void btnAddPackage_Click(object sender, EventArgs e)
    {
        //reset the controls for the user to create new Package. and show the popup
        PrepareForCreatePackage();
        mpCreatePrjPkg.Show();
    }
    protected void btnCreatePrjPkg_Click(object sender, EventArgs e)
    {
        //Create Actual PAckage Or Project.
        Button btn = (Button)sender;
        switch (btn.CommandName)
        {
            case "CreateNewProject":
                //Create Project and rebind the grid
                Package.CreatePackage(txtCreatePrjPkgName.Text.Trim(), Server.HtmlDecode(htmlEditorCreatePrjPkgNotes.Text),
                                      String.Empty, ((CurrentUser)CurrentUser.GetUserDetails()).User_GUID);
                mpCreatePrjPkg.Hide();
                BindPackages();
                break;
            case "CreateNewPackage":
                // Create Package and rebind the grid          
                Package.CreatePackage(txtCreatePrjPkgName.Text.Trim(), Server.HtmlDecode(htmlEditorCreatePrjPkgNotes.Text),
                                      ddlCreatePkgProjectList.SelectedValue, ((CurrentUser)CurrentUser.GetUserDetails()).User_GUID);
                mpCreatePrjPkg.Hide();
                BindPackages();
                break;
        }
        //For not to duplicate data on browser refresh F5 after insert
        Page.Response.Redirect(Page.Request.Url.ToString(), true);
    }
    protected void btnEditPackage_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        if (btn.CommandName.ToString().Equals("EditPrjPkg"))
        {
            EditAndPrepareForUpdateProject();
            mpViewPackageNote.Show();
        }
        else if (btn.CommandName.ToString().Equals("UpdatePrjPkg"))
        {
            //USe sp PackageUpdateNoteUpdate to update the notes.
            //Package.UpdatePacakgeNote(hfCurProjPackGuid.Value, Server.HtmlDecode(htmlEditorPrjNotes.Text),
            //                          ((CurrentUser)CurrentUser.GetUserDetails()).User_GUID);
            //changed requirement to update thr project name too.
            Package.UpdatePackage(hfCurProjPackGuid.Value, txtviewUpdatePrjName.Text.Trim(), Server.HtmlDecode(htmlEditorPrjNotes.Text),
                          ((CurrentUser)CurrentUser.GetUserDetails()).User_GUID);

            mpViewPackageNote.Hide();
            BindPackages();
            //For not to duplicate data on browser refresh F5 after update
            Page.Response.Redirect(Page.Request.Url.ToString(), true);
        }
    }
    #endregion

    #region "Grid Events"
    protected void GridPackages_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    //Find the DropDownList in the Row
        //    DropDownList ddlCountries = (e.Row.FindControl("ddlPrjPkgStatusName") as DropDownList);
        //    ddlCountries.DataSource = GetPrjPkgStatusCodes();
        //    ddlCountries.DataTextField = "name";
        //    ddlCountries.DataValueField = "Status_GUID";
        //    ddlCountries.DataBind();

        //    //// Select the Country of Customer in DropDownList

        //    string statusGuid = GridPackages.DataKeys[e.Row.RowIndex]["Status_Guid"].ToString();
        //    ddlCountries.Items.FindByValue(statusGuid).Selected = true;
        //}
    }
    protected void GridPackages_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = Convert.ToInt32(e.CommandArgument);
        //Get the value of guid from the DataKeys using the RowIndex.
        string prjGuID = GridPackages.DataKeys[rowIndex]["Project_GUID"].ToString();
        string pkgGuID = GridPackages.DataKeys[rowIndex]["Package_GUID"].ToString();
        string eguid = getEGuid(prjGuID, pkgGuID);
        btnEditPackage.Visible = true;
        if (e.CommandName == "ViewPackageInfo")
        {
            //View Package Information
            DataTable dt = Package.GePackageNoteDetails(eguid).Tables[0];
            if (dt.Rows.Count == 0)
            {
                lblError.Text = "Error: No data found for this Project.";
            }
            else
            {
                PrepareForEditProject(eguid, dt.Rows[0]);
                mpViewPackageNote.Show();
            }
        }
        else if (e.CommandName == "EDITPROJPKG")
        {
            //Open the Proj Pckg in Edit Mode (after inserting the new record).            
            DataTable dt = Package.GePackageNoteDetails(eguid).Tables[0];
            if (dt.Rows.Count == 0)
            {
                lblError.Text = "Error: No data found for this Project. Cannot Edit.";
            }
            else
            {
                PrepareForEditProject(eguid, dt.Rows[0]);
                EditAndPrepareForUpdateProject();
                mpViewPackageNote.Show();
            }
        }
        else if (e.CommandName == "SELECT")
        {
            // Package is selected .Navigate to Requirements page
            Session["svSelectedPackage"] = eguid;
            Response.Redirect("~/Requirements.aspx", true);
        }
        else if (e.CommandName == "HISTORY")
        {
            //Open the Proj Pckg in Edit Mode (after inserting the new record).            
            DataTable dt = Package.GePackageNoteDetails(eguid).Tables[0];
            if (dt.Rows.Count == 0)
            {
                lblError.Text = "Error: No data found for this Project. Cannot View History.";
            }
            else
            {
                lblPrjPkgNameHist.Text = dt.Rows[0]["Name"].ToString().Trim();
                hfCurProjPackGuid.Value = eguid;
                GridProjPkgHist.PageIndex = 0;
                BindPrjPkgHistory(hfCurProjPackGuid.Value);
                upModal.Update();
                mpViewPackageHist.Show();
            }
        }
        else if (e.CommandName == "PrepareToUpdateStatus")
        {
            //User clicked on Status Link to update Status. 
            //Display the dropdown and hide the link in the grid for this row.
            GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            LinkButton lnkStatus = (LinkButton)row.FindControl("lnkPrjPkgStatusName");
            DropDownList ddStatus = (DropDownList)row.FindControl("ddlPrjPkgStatusName");
            string statusGuID = GridPackages.DataKeys[rowIndex]["Status_Guid"].ToString();
            BindStatusDropDown(ddStatus, statusGuID);
            lnkStatus.Visible = false;
            ddStatus.Visible = true;
        }
    }
    protected void GridProjPkgHist_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //Change the page index and rebind the history grid
        GridProjPkgHist.PageIndex = e.NewPageIndex;
        BindPrjPkgHistory(hfCurProjPackGuid.Value);
        //mpViewPackageHist.Show();
    }
    protected void GridProjPkgHist_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = Convert.ToInt32(e.CommandArgument) - GridProjPkgHist.PageSize * GridProjPkgHist.PageIndex;

        if (e.CommandName == "ViewPrjPkgHistInfo")
        {
            //Get the value of guid from the DataKeys using the RowIndex.
            string eguid = GridProjPkgHist.DataKeys[rowIndex]["ea_guid"].ToString();
            int versionNum = Convert.ToInt32(GridProjPkgHist.DataKeys[rowIndex]["Version"].ToString());

            DataTable dt = Package.GePackageNoteHistory(eguid, versionNum).Tables[0];
            if (dt.Rows.Count == 0)
            {
                lblError.Text = "Error: No data found for this History Item.";
            }
            else
            {
                mpViewPackageHist.Hide();
                PrepareForEditProject(eguid, dt.Rows[0]);
                btnEditPackage.Visible = ((rowIndex == 0) && (GridProjPkgHist.PageIndex == 0)) ? true : false;
                mpViewPackageNote.Show();
            }
        }
    }

    #endregion

    #region "Supports"
    /// <summary>
    /// Metod to Bind Project/Packages Grid
    /// </summary>
    private void BindPackages()
    {
        try
        {
            lblError.Text = "";
            Dictionary<string, int> kvpairList = new Dictionary<string, int>();            
            DataTable dt = Package.GetPackages().Tables[0];
            if (!string.IsNullOrEmpty(txtSearchProjPackage.Text.Trim()))
            {                
                for (int i = dt.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = dt.Rows[i];
                    if (!kvpairList.ContainsKey(dr["Project_GUID"].ToString()))                      
                    {
                        kvpairList.Add(dr["Project_GUID"].ToString(), Convert.ToInt32(dr["PackageCount"].ToString()));
                    }
                    if (dr["DisplayName"].ToString().ToLower().Contains("--") && !(dr["DisplayName"].ToString().ToLower().Trim().Contains(txtSearchProjPackage.Text.ToLower().Trim())))
                    {
                        kvpairList[dr["Project_GUID"].ToString()] = kvpairList[dr["Project_GUID"].ToString()] -1;                       
                        dr.Delete();                        
                    }
                }
                dt.AcceptChanges();
                //delete the Projects if there are no packages
                for (int i = dt.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = dt.Rows[i];
                    if (string.IsNullOrEmpty(dr["Package_GUID"].ToString().Trim()) && kvpairList[dr["Project_GUID"].ToString()] ==0)
                    {
                        dr.Delete();    
                    }
                }
            }
            DataView dv = dt.DefaultView;
            GridPackages.DataSource = dv;
            GridPackages.DataBind();
        }
        catch (Exception ex)
        {
            lblError.Text = "Error :" + ex.Message.ToString();
        }
    }
    private void AddOrUpdateKeyValPair(Dictionary<string, int> dict, string key, int value)
    {
        if (dict.ContainsKey(key))
        {
            dict[key] = value;
        }
        else
        {
            dict.Add(key, value);
        }
    }

    /// <summary>
    /// Method to bind Package History Grid
    /// </summary>
    /// <param name="eguid"></param>
    private void BindPrjPkgHistory(string eguid)
    {
        DataTable dtHistory = Package.GePackageHistory(eguid).Tables[0];
        GridProjPkgHist.DataSource = dtHistory;
        GridProjPkgHist.DataBind();
    }

    private void PrepareForEditProject(string eguid, DataRow dataRow)
    {
        hfCurProjPackGuid.Value = eguid;
        lblviewPrjName.Text = dataRow["Name"].ToString();
        lblviewPrjVersion.Text = dataRow["Version"].ToString();
        lblviewPrjCreatedBy.Text = dataRow["CreatedByName"].ToString();
        lblviewPrjCreatedDate.Text = dataRow["CreatedDate"].ToString();
        litviewPrjNotes.Text = dataRow["Notes"].ToString();
        htmlEditorPrjNotes.Text = litviewPrjNotes.Text.ToString();
        divNotes.Visible = true;
        lblviewPrjName.Visible = true;
        htmlEditorPrjNotes.Visible = false;
        txtviewUpdatePrjName.Visible = false;
        btnEditPackage.Text = "Edit";
        btnEditPackage.CommandName = "EditPrjPkg";
    }

    private void EditAndPrepareForUpdateProject()
    {
        //Edit the Package. Connect to database and edit the Proj/Pack (insert new one with current USer and update Versino)
        Package.UpdatePackage(hfCurProjPackGuid.Value, lblviewPrjName.Text, Server.HtmlDecode(htmlEditorPrjNotes.Text),
                              ((CurrentUser)CurrentUser.GetUserDetails()).User_GUID);
        //Update the new verson num

        //Update date and updated by text and the version number
        lblviewPrjVersion.Text = Convert.ToString((Convert.ToInt32(lblviewPrjVersion.Text.Trim()) + 1));
        lblviewPrjCreatedDate.Text = System.DateTime.Now.ToString();
        lblviewPrjCreatedBy.Text = ((CurrentUser)CurrentUser.GetUserDetails()).FullName;
        divNotes.Visible = false;
        htmlEditorPrjNotes.Visible = true;
        txtviewUpdatePrjName.Visible = true;
        lblviewPrjName.Visible = false;
        txtviewUpdatePrjName.Text = lblviewPrjName.Text;
        btnEditPackage.Text = "Update";
        btnEditPackage.CommandName = "UpdatePrjPkg";
    }
    /// <summary>
    /// Method to reset controls when user tries to create project
    /// </summary>
    private void PrepareForCreateProject()
    {
        trPckgCreate.Visible = false;
        lblCreatePrjPkgTitle.Text = "Project Title: ";

        //Update date and updated by text and the version number
        lblCreatePkgPrjVersion.Text = "0";
        lblCreatePrjPkgCreatedDate.Text = System.DateTime.Now.ToString();
        lblCreatePrjPkgCreatedBy.Text = ((CurrentUser)CurrentUser.GetUserDetails()).FullName;
        htmlEditorCreatePrjPkgNotes.Text = "";
        txtCreatePrjPkgName.Text = "";
        btnCreatePrjPkg.Text = "Update";
        btnCreatePrjPkg.CommandName = "CreateNewProject";
    }
    /// <summary>
    /// Method to reset controls when user tries to create Package
    /// </summary>
    private void PrepareForCreatePackage()
    {
        trPckgCreate.Visible = true;
        lblCreatePrjPkgTitle.Text = "Package Title: ";
        BindProjectsDropDown();

        //Update date and updated by text and the version number
        lblCreatePkgPrjVersion.Text = "0";
        lblCreatePrjPkgCreatedDate.Text = System.DateTime.Now.ToString();
        lblCreatePrjPkgCreatedBy.Text = ((CurrentUser)CurrentUser.GetUserDetails()).FullName;
        htmlEditorCreatePrjPkgNotes.Text = "";
        txtCreatePrjPkgName.Text = "";
        btnCreatePrjPkg.Text = "Update";
        btnCreatePrjPkg.CommandName = "CreateNewPackage";
    }

    private void BindStatusDropDown(DropDownList ddStatus, string statusGuID)
    {
        ddStatus.DataSource = GetPrjPkgStatusCodes();
        ddStatus.DataTextField = "name";
        ddStatus.DataValueField = "Status_GUID";
        ddStatus.DataBind();

        ddStatus.Items.FindByValue(statusGuID).Selected = true;
    }
    private void BindProjectsDropDown()
    {
        ddlCreatePkgProjectList.DataSource = Common.GetProjectsList();
        ddlCreatePkgProjectList.DataTextField = "DisplayName";
        ddlCreatePkgProjectList.DataValueField = "Project_GUID";
        ddlCreatePkgProjectList.DataBind();
    }
    /// <summary>
    /// Update the Project/Package details upon chaing the Dropdown status value from the Project/Packages grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void UpdatePrjPkgStatus(object sender, EventArgs e)
    {
        try
        {
            DropDownList ddl = (DropDownList)sender;
            // get reference to the row
            GridViewRow gvr = (GridViewRow)(((Control)sender).NamingContainer);
            //Get the value of guid from the DataKeys using the RowIndex.
            string eguid = getEGuid(GridPackages.DataKeys[gvr.RowIndex]["Project_GUID"].ToString(), GridPackages.DataKeys[gvr.RowIndex]["Package_GUID"].ToString());
            Package.UpdatePacakgeStatus(eguid, ddl.SelectedValue, ((CurrentUser)CurrentUser.GetUserDetails()).User_GUID);
            BindPackages();
        }
        catch (Exception ex)
        {
            lblError.Text = "Error updating Status: " + ex.Message.ToString();
        }
    }
    /// <summary>
    /// Methid to get the Project/Status Status Codes when user clicks "Status" link from the grid
    /// </summary>
    /// <returns></returns>
    private DataTable GetPrjPkgStatusCodes()
    {
        DataTable dt;
        if (ViewState["vsPrjStatusCodes"] == null)
        {
            dt = Common.GetStatusCodes().Tables[0];
            ViewState["vsPrjStatusCodes"] = dt;
        }
        else
        {
            dt = (DataTable)ViewState["vsPrjStatusCodes"];
        }
        return dt;
    }

    private string getEGuid(string prjGuid, string pkgGuid)
    {
        return (string.IsNullOrEmpty(pkgGuid)) ? prjGuid : pkgGuid;
    }
    #endregion

}
