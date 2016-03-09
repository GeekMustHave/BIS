using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

/// <summary>
/// Description: Use case Module for a selected Package.
/// History:
/// -------------------------------------------------------------------------------
/// Date:               Name                Description
/// --------------------------------------------------------------------------------
/// 7/10/2015           JHRS                Intial Creation
/// </summary>
public partial class UseCases : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //See if the user has selected a package. If not display error message to select a pacakge first.
        if (!Page.IsPostBack)
        {
            if (Session["svSelectedPackage"] == null)
            {
                lblPackageName.Text = "Package: None";
                mvUseCases.SetActiveView(vwNoSelect);
            }
            else
            {
                //User selected a package. Get the PAckage name and bind the UseCases to the grid.
                string PrjPkgName = Requirement.GetProjectPackageName(Session["svSelectedPackage"].ToString());
                if (string.IsNullOrEmpty(PrjPkgName))
                {
                    lblError.Text = "Error: No data found for this Project. Cannot View Packages.!";
                }
                else
                {
                    lblPackageName.Text = "Package: " + PrjPkgName;
                    mvUseCases.SetActiveView(vwDefault);
                    BindUseCaseList(Session["svSelectedPackage"].ToString(), string.Empty, string.Empty, chkshowHidden.Checked);
                    BindReqListDropDown(Session["svSelectedPackage"].ToString());
                }
            }
            // Bind the Custom Button Dopdown for reports
            btnDropDownReports.AddReportToDropDown("Use-Case Details Report", "~/UCDReport.aspx");
            btnDropDownReports.AddReportToDropDown("Use-Cases Document Report", "~/UCDocumentReport.aspx");
        }
    }

    #region "post back events"
    protected void btnReferesh_Click(object sender, EventArgs e)
    {
        BindUseCaseList(Session["svSelectedPackage"].ToString(), (ddlRequirements.SelectedIndex == 0 ? string.Empty : ddlRequirements.SelectedValue)
                        , txtSearchUseCase.Text.Trim(), chkshowHidden.Checked);
    }
    protected void btnAddUsecase_Click(object sender, EventArgs e)
    {
        PrepareUseCaseCreate();
        mpCreateUpdateUC.Show();
    }
    protected void GridUseCaseList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = Convert.ToInt32(e.CommandArgument);
        //Get the value of guid from the DataKeys using the RowIndex.
        string eguid = GridUseCaseList.DataKeys[rowIndex]["ea_guid"].ToString();
        if (e.CommandName == "UseCaseListEdit")
        {
            DoUseCaseEdit(eguid);
        }
        else if (e.CommandName == "UseCaseListHist")
        {
            DataTable dtUCDeetails = UseCase.GetUseCaseDetailView(eguid).Tables[0];
            if (dtUCDeetails.Rows.Count == 0)
            {
                lblError.Text = "No data found for Details";
            }
            else
            {
                lblCurrentUCIDHist.Text = dtUCDeetails.Rows[0]["UV_ID"].ToString().Trim();
                lblUCNameHist.Text = dtUCDeetails.Rows[0]["Name"].ToString().Trim();
                hfCurrentUseCaseID.Value = eguid;
                GridUCHist.PageIndex = 0;
                BindUCHistory(hfCurrentUseCaseID.Value);
                upModalUCHist.Update();
                mpViewUCHist.Show();
            }
        }
        else if (e.CommandName == "UseCaseListReq")
        {
            DataTable dtUCDeetails = UseCase.GetUseCaseDetailView(eguid).Tables[0];
            if (dtUCDeetails.Rows.Count == 0)
            {
                lblError.Text = "No data found for Details";
            }
            else
            {
                lblEditUCReqID.Text = dtUCDeetails.Rows[0]["UV_ID"].ToString().Trim();
                lblEditUCReqName.Text = dtUCDeetails.Rows[0]["Name"].ToString().Trim();
                hfCurrentUseCaseID.Value = eguid;
                GridEditUCReq.PageIndex = 0;
                BindGridUCReq(hfCurrentUseCaseID.Value);
                BindUseCaseReqEditDropDown(hfCurrentUseCaseID.Value);
                lblErrorEditUCReq.Text = "";
                upEditUCReq.Update();
                mpEditUCReq.Show();
            }
        }
        else if (e.CommandName == "UseCaseListFlows")
        {
            //store the use case in session and redirect to usecase-flows page.
            Session["svSelectedUseCase"] = eguid;
            Response.Redirect("~/UseCaseFlows.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }

    /// <summary>
    /// Create or Update Use Case.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCreateUpdateUCNew_Click(object sender, EventArgs e)
    {
        //Create Actual PAckage Or Project.
        Button btn = (Button)sender;
        switch (btn.CommandName)
        {
            case "CreateUseCase":
                //Code to Create USe Case..  While the USE case is created, a flow step is intialated via the sp within sp spUC_FlowStepInital.
                UseCase.CreateUseCase(Session["svSelectedPackage"].ToString(), txtCreateUpdateUCName.Text.Trim(), Server.HtmlDecode(htmlEditorCreateUpdateUCNotes.Text),
                                      ((CurrentUser)CurrentUser.GetUserDetails()).User_GUID);
                mpCreateUpdateUC.Hide();
                BindUseCaseList(Session["svSelectedPackage"].ToString(), (ddlRequirements.SelectedIndex == 0 ? string.Empty : ddlRequirements.SelectedValue)
                                , txtSearchUseCase.Text.Trim(), chkshowHidden.Checked);
                break;
            case "UpdateUseCase":
                UseCase.UpdateUseCase(hfCurrentUseCaseID.Value, txtCreateUpdateUCName.Text.Trim(), Server.HtmlDecode(htmlEditorCreateUpdateUCNotes.Text),
                                      ddlCreateUpdateUCStatus.SelectedValue, ((CurrentUser)CurrentUser.GetUserDetails()).User_GUID, chkCreateUpdateUCHidden.Checked);
                mpCreateUpdateUC.Hide();
                BindUseCaseList(Session["svSelectedPackage"].ToString(), (ddlRequirements.SelectedIndex == 0 ? string.Empty : ddlRequirements.SelectedValue)
                                , txtSearchUseCase.Text.Trim(), chkshowHidden.Checked);
                break;
        }
        //For not to duplicate data on browser refresh F5 after insert
        Page.Response.Redirect(Page.Request.Url.ToString(), true);
    }
    protected void GridUCHist_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridUCHist.PageIndex = e.NewPageIndex;
        BindUCHistory(hfCurrentUseCaseID.Value);
        //mpViewPackageHist.Show();
    }
    protected void GridUCHist_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = Convert.ToInt32(e.CommandArgument) - GridUCHist.PageSize * GridUCHist.PageIndex;

        if (e.CommandName == "ViewUCHistInfo")
        {
            //Get the value of guid from the DataKeys using the RowIndex.
            string eguid = GridUCHist.DataKeys[rowIndex]["ea_guid"].ToString();
            int versionNum = Convert.ToInt32(GridUCHist.DataKeys[rowIndex]["Version"].ToString());

            DataTable dt = UseCase.GetUseCaseHistoryDetails(eguid, versionNum).Tables[0];
            if (dt.Rows.Count == 0)
            {
                lblError.Text = "Error: No data found for this History Item.";
            }
            else
            {
                mpViewUCHist.Hide();
                //PrepareForEditProject(eguid, dt.Rows[0]);
                DataRow dr = dt.Rows[0];
                lblViewUCHistDetailsComplexity.Text = dr["Complexity"].ToString();
                lblViewUCHistDetailsCreatedBy.Text = dr["FullName"].ToString();
                lblViewUCHistDetailsCreatedDate.Text = dr["CreatedDate"].ToString();
                lblViewUCHistDetailsLabel.Text = dr["Labels"].ToString();
                lblViewUCHistDetailsStatus.Text = dr["Status"].ToString();
                lblViewUCHistDetailsType.Text = dr["TypeIs"].ToString();
                lblViewUCHistDetailsVersion.Text = dr["Version"].ToString();
                lblViewUCHistDetailName.Text = dr["Name"].ToString();
                litViewUCHistDetailTextNotes.Text = dr["Note"].ToString();
                btnViewEditUCHistDetail.Visible = ((rowIndex == 0) && (GridUCHist.PageIndex == 0)) ? true : false;
                mpViewUCHistDetail.Show();
            }
        }
    }
    protected void btnViewEditUCHistDetail_Click(object sender, EventArgs e)
    {
        DoUseCaseEdit(hfCurrentUseCaseID.Value);
    }
    protected void GridEditUCReq_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridEditUCReq.PageIndex = e.NewPageIndex;
        BindGridUCReq(hfCurrentUseCaseID.Value);
    }
    protected void GridEditUCReq_RowCommand(object sender, GridViewCommandEventArgs e)
    {       
        //For delete comand 
        if (e.CommandName == "DeleteEditUCReq")
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument) - GridEditUCReq.PageSize * GridEditUCReq.PageIndex;
            if (GridEditUCReq.Rows.Count == 1)
            {
                lblErrorEditUCReq.Text = "* You can NOT delete the only Requirement for this use case. ";
                mpEditUCReq.Show();
            }
            else
            {
                //Delete the Requiremnt

                ////Get the value of guid from the DataKeys using the RowIndex.
                string reqguid = GridEditUCReq.DataKeys[rowIndex]["REQ_GUID"].ToString();
                string ucguid = GridEditUCReq.DataKeys[rowIndex]["UC_GUID"].ToString();
                UseCase.DeleteUcReqRelation(ucguid, reqguid, ((CurrentUser)CurrentUser.GetUserDetails()).User_GUID);

                //Re bind the Grid and show it again.
                BindGridUCReq(hfCurrentUseCaseID.Value);
                BindUseCaseReqEditDropDown(hfCurrentUseCaseID.Value);
                lblErrorEditUCReq.Text = "";
                upEditUCReq.Update();
                mpEditUCReq.Show();
            }
        }
    }
    protected void btnCloseEditUCReq_Click(object sender, EventArgs e)
    {
        mpEditUCReq.Hide();
    }
    //Add Requiremnet to use case.
    protected void btnUpdateEditUCReq_Click(object sender, EventArgs e)
    {
        //DoUseCaseEdit(hfCurrentUseCaseID.Value);
        if (ddlRequirementEditUCReq.SelectedIndex == 0)
        {
            lblErrorEditUCReq.Text = "* Select a requirement";
        }
        else
        {

            UseCase.AddUcReqRelation(hfCurrentUseCaseID.Value, ddlRequirementEditUCReq.SelectedValue, ((CurrentUser)CurrentUser.GetUserDetails()).User_GUID);
            GridEditUCReq.PageIndex = 0;
            //Re bind the Grid and show it again.
            BindGridUCReq(hfCurrentUseCaseID.Value);
            BindUseCaseReqEditDropDown(hfCurrentUseCaseID.Value);
            lblErrorEditUCReq.Text = "";
            upEditUCReq.Update();
            //mpEditUCReq.Show();
        }
    }

    /// <summary>
    /// Event to fire when the requiremnt dorpdown is changed. USe cases grid will be updated as per the selected Req.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlRequirements_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindUseCaseList(Session["svSelectedPackage"].ToString(), (ddlRequirements.SelectedIndex == 0 ? string.Empty : ddlRequirements.SelectedValue)
                        , txtSearchUseCase.Text.Trim(), chkshowHidden.Checked);
    }
    #endregion
    #region "Code for Select2 plugin dropdown - Labels"
    //Code to get all the unique labels from the system to load for the labels select2 plugin
    [System.Web.Services.WebMethod(EnableSession = true)]
    [System.Web.Script.Services.ScriptMethod()]
    public static string GetAllUniqueLabels(string searchText)//, int pageSize, int pageNum)
    {
        //Get the paged results and the total count of the results for this query. 
        //AttendeeRepository ar = new AttendeeRepository();
        //List<Attendee> attendees = ar.GetAttendees(searchTerm, pageSize, pageNum);

        UseCases newClass = new UseCases();
        DataTable dtLabels = Common.GetUniqueLabels(searchText).Tables[0];
        int attendeeCount = dtLabels.Rows.Count;

        return newClass.ConvertToJson(dtLabels);
    }

    //code to get all the labels for the curent USe CAse. called from javascript .
    [System.Web.Services.WebMethod(EnableSession = true)]
    [System.Web.Script.Services.ScriptMethod()]
    public static string GetUseCaseLabels(string currentUCID)
    {
        //Get the paged results and the total count of the results for this query. 
        //AttendeeRepository ar = new AttendeeRepository();
        //List<Attendee> attendees = ar.GetAttendees(searchTerm, pageSize, pageNum);

        UseCases newClass = new UseCases();
        DataTable dtLabels = Common.GetObjectLabels(currentUCID).Tables[0];
        int attendeeCount = dtLabels.Rows.Count;

        return newClass.ConvertToJson(dtLabels);
    }

    //code to update labels for the curent USe Case. called from javascript ajax post
    [System.Web.Services.WebMethod(EnableSession = true)]
    [System.Web.Script.Services.ScriptMethod()]
    public static string UpdateUCLabels(string currentUCID, string actionEvent, string tagName)
    {
        /*actinEvent will be select or unselect  - check javascript code*/
        try
        {
            Common.UpdateObjectLabels(currentUCID, actionEvent, tagName);
            return "Successful!";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
    }
    private string ConvertToJson(DataTable dt)
    {
        System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        Dictionary<string, object> row = new Dictionary<string, object>();
        Random rnd = new Random();

        foreach (DataRow item in dt.Rows)
        {
            row = new Dictionary<string, object>();
            // made id and text same so that we can leverage the duplication elimination function that comes with the select2.
            //less work for us to maintain the id's
            row.Add("id", item["LabelName"].ToString());
            row.Add("text", item["LabelName"].ToString());
            rows.Add(row);
        }
        return serializer.Serialize(rows).ToString();
    }
    #endregion
    #region "supports"
    private void BindUseCaseList(string eaguid, string reqGuid, string searchText, bool showHidden)
    {
        GridUseCaseList.DataSource = UseCase.GetUseCaseList(eaguid, reqGuid, searchText, showHidden);
        GridUseCaseList.DataBind();
    }
    private void BindReqListDropDown(string eguid)
    {
        ddlRequirements.Items.Clear();
        DataTable dt = Common.GetReqComboList(eguid).Tables[0];
        if (dt.Rows.Count == 0)
        {
            ddlRequirements.Items.Insert(0, new ListItem("-- No Requirements found", "None"));
        }
        else
        {
            ddlRequirements.DataSource = dt;
            ddlRequirements.DataTextField = "Requirement";
            ddlRequirements.DataValueField = "ea_guid";
            ddlRequirements.DataBind();
            ddlRequirements.Items.Insert(0, new ListItem("*All", "*All"));
        }

        //dropdownList.Items.FindByValue(statusGuID).Selected = true;
    }
    private void PrepareUseCaseCreate()
    {
        hfCurrentUseCaseID.Value = string.Empty;
        txtCreateUpdateUCName.Text = String.Empty;
        lblCreateUpdateUCVersion.Text = "0";
        lblCreateUpdateUCCreatedDate.Text = DateTime.Now.ToString();
        lblCreateUpdateUCCreatedBy.Text = ((CurrentUser)CurrentUser.GetUserDetails()).FullName;

        htmlEditorCreateUpdateUCNotes.Text = string.Empty;
        btnCreateUpdateUCNew.CommandName = "CreateUseCase";
        txtCreateUpdateUCName.Focus();
        trCreateUpdateUC.Visible = false;
        trCreateUpdateUCLabels.Visible = false;
    }
    private void PrepareUseCaseUpdate(string eguid, DataRow dataRow)
    {
        hfCurrentUseCaseID.Value = eguid;
        txtCreateUpdateUCName.Text = dataRow["Name"].ToString();
        lblCreateUpdateUCVersion.Text = (Convert.ToInt32(dataRow["Version"].ToString()) + 1).ToString();
        lblCreateUpdateUCCreatedBy.Text = dataRow["FullName"].ToString();
        lblCreateUpdateUCCreatedDate.Text = dataRow["CreatedDate"].ToString();
        htmlEditorCreateUpdateUCNotes.Text = dataRow["Note"].ToString();

        trCreateUpdateUC.Visible = true;
        trCreateUpdateUCLabels.Visible = true;
        BindStatusDropDown(ddlCreateUpdateUCStatus);
        ddlCreateUpdateUCStatus.SelectedValue = dataRow["Status"].ToString();
        chkCreateUpdateUCHidden.Checked = Convert.ToBoolean(dataRow["Hidden"]);
        btnCreateUpdateUCNew.CommandName = "UpdateUseCase";
        txtCreateUpdateUCName.Focus();
    }

    private void BindStatusDropDown(DropDownList dropdownList)
    {
        dropdownList.Items.Clear();
        dropdownList.DataSource = Common.GetStatusCodes().Tables[0];
        dropdownList.DataTextField = "name";
        dropdownList.DataValueField = "Status_GUID";
        dropdownList.DataBind();

        //dropdownList.Items.FindByValue(statusGuID).Selected = true;
    }
    private void BindUCHistory(string eguid)
    {
        DataTable dtHistory = UseCase.GetUseCaseHistory(eguid).Tables[0];
        GridUCHist.DataSource = dtHistory;
        GridUCHist.DataBind();
    }
    private void BindGridUCReq(string eguid)
    {
        DataTable dtHistory = UseCase.GetUseRequirements(eguid).Tables[0];
        GridEditUCReq.DataSource = dtHistory;
        GridEditUCReq.DataBind();
    }
    private void DoUseCaseEdit(string eguid)
    {
        DataTable dtUCDeetails = UseCase.GetUseCaseDetailView(eguid).Tables[0];
        if (dtUCDeetails.Rows.Count == 0)
        {
            lblError.Text = "No data found for Details";
        }
        else
        {
            hfCurrentUseCaseID.Value = eguid;
            PrepareUseCaseUpdate(eguid, dtUCDeetails.Rows[0]);
            mpCreateUpdateUC.Show();
            /* register startup script for labes */
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallIntialLoadForLabels", "initBindLabels('" + eguid + "')", true);
        }
    }
    private void BindUseCaseReqEditDropDown(string eguid)
    {
        ddlRequirementEditUCReq.Items.Clear();
        DataTable dt = UseCase.GetUCRequirements(eguid).Tables[0];
        if (dt.Rows.Count == 0)
        {
            ddlRequirementEditUCReq.Items.Insert(0, new ListItem("-- No Requirements found", "None"));
        }
        else
        {
            ddlRequirementEditUCReq.DataSource = dt;
            ddlRequirementEditUCReq.DataTextField = "Requirement";
            ddlRequirementEditUCReq.DataValueField = "Req_GUID";
            ddlRequirementEditUCReq.DataBind();
            ddlRequirementEditUCReq.Items.Insert(0, new ListItem("**Select Requirement", "None"));
        }
    }
    #endregion
}
