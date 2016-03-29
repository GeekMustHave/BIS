using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Microsoft.VisualBasic;
using System.Collections;
using System.Diagnostics;
using System.Configuration;
using System.Data.SqlClient;
/// <summary>
/// Description: Requiremnts Module for a selected Package.
/// History:
/// -------------------------------------------------------------------------------
/// Date:               Name                Description
/// --------------------------------------------------------------------------------
/// 7/10/2015           JHRS                Intial Creation
/// </summary>
public partial class Requirements : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //See if the user has selected a package. If not display error message to select a pacakge first.
            txtSearchReq.Focus();
            if (Session["svSelectedPackage"] == null)
            {
                lblPackageName.Text = "Package: None";
                mvRequirements.SetActiveView(vwNoSelect);
            }
            else
            {
                //User selected a package. Get the PAckage name and bind the requirements to the grid.
                string PrjPkgName = Requirement.GetProjectPackageName(Session["svSelectedPackage"].ToString());
                if (string.IsNullOrEmpty(PrjPkgName))
                {
                    lblError.Text = "Error: No data found for this Project. Cannot View Packages.!";
                }
                else
                {
                    lblPackageName.Text = "Package: " + PrjPkgName;
                    mvRequirements.SetActiveView(vwDefault);
                    BindReqListDetailed(Session["svSelectedPackage"].ToString(), string.Empty);
                }
            }
            // Bind the Custom Button Dopdown for reports
            btnDropDownReports.AddReportToDropDown("Orphaned Requirements Report", "~/ReportPages/OrphanedRequirementsReport.aspx");
            btnDropDownReports.AddReportToDropDown("RVD-Requirement Validation Document", "~/RVDReport.aspx");
            btnDropDownReports.AddReportToDropDown("RTM-Requirement Traceability Matrix", "~/RTMReport.aspx");
        }
    }

    #region "Button Click Events"
    protected void btnAddRequirement_Click(object sender, EventArgs e)
    {
        //reset controls on create requirement popup and show the popup
        PrepareReqCreate();
        upCreateUpdateReq.Update();
        mpCreateUpdateReq.Show();
    }
    protected void btnReorderReq_Click(object sender, EventArgs e)
    {
        //REbind the Requirement order list
        BindReorderReqList(Session["svSelectedPackage"].ToString());
        mpReorderReq.Show();
        upModalReorder.Update();
    }
    protected void btnReferesh_Click(object sender, EventArgs e)
    {
        //Refresh button click. REbind the Requiremnts list and make the search text box to foucs.
        BindReqListDetailed(Session["svSelectedPackage"].ToString(), txtSearchReq.Text.Trim());
        //txtSearchReq.Focus();
        //updatePanelReq.Update();
    }
    protected void btnCreateUpdateReq_Click(object sender, EventArgs e)
    {
        //Button Click event for Create and update a Requiremnt.
        try
        {
            Button btn = (Button)sender;
            switch (btn.CommandName)
            {
                case "CreateRequirement":
                    try
                    {
                        //Create requirement. and rebind the grid to update the created one.
                        Requirement.CreateRequirementDetail(Session["svSelectedPackage"].ToString(), txtReqTitle.Text.Trim(),
                            Server.HtmlDecode(htmlEditorCreateUpdateReqNotes.Text),
                            ((CurrentUser)CurrentUser.GetUserDetails()).User_GUID);
                        mpCreateUpdateReq.Hide();
                        BindReqListDetailed(Session["svSelectedPackage"].ToString(), txtSearchReq.Text.Trim());
                        updatePanelReq.Update();
                        string msg = "Requirement  Created Succesfully. <br />  This message will disappear in 5 seconds.";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "DisplayNotif", "DisplayNotification('" + msg + "', 'success')", true);
                    }
                    catch (Exception ex)
                    {
                        mpCreateUpdateReq.Hide();
                        string msg = "Creation Failed:" + ex.Message.ToString();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "DisplayNotif", "DisplayNotification('" + msg + "', 'error')", true);
                    }
                    break;
                case "UpdateRequirement":
                    try
                    {
                        //USe sp to update requirement  and rebind the grid to update the Updated REquirement.
                        Requirement.UpdateRequirementDetail(hfCurentReqId.Value, txtReqTitle.Text.Trim(),
                            Server.HtmlDecode(htmlEditorCreateUpdateReqNotes.Text), ddlReqComplexity.SelectedValue, ddlReqType.SelectedValue, ddlReqStatus.SelectedValue,
                            ((CurrentUser)CurrentUser.GetUserDetails()).User_GUID);
                        mpCreateUpdateReq.Hide();
                        BindReqListDetailed(Session["svSelectedPackage"].ToString(), txtSearchReq.Text.Trim());
                        updatePanelReq.Update();
                        string msg = "Succesfully Updated Requirement. <br />  This message will disappear in 5 seconds.";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "DisplayNotif", "DisplayNotification('" + msg + "', 'success')", true);
                    }
                    catch (Exception ex)
                    {
                        mpCreateUpdateReq.Hide();
                        string msg = "Update Failed:" + ex.Message.ToString();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "DisplayNotif", "DisplayNotification('" + msg + "', 'error')", true);
                    }
                    break;
            }
            lblError.Text = "";
            // To supress re creating/updating Req on F5.
            //Page.Response.Redirect(Page.Request.Url.ToString(), true);
        }
        catch (Exception ex)
        {
            lblErrorCreateUpdateReq.Text = ex.Message.ToString();
            lblErrorCreateUpdateReq.ForeColor = System.Drawing.Color.Red;
        }
    }
    protected void btnCloseReorderReq_Click(object sender, EventArgs e)
    {
        //After closing Reorder popup rebind the Grid on page.
        mpReorderReq.Hide();
        BindReqListDetailed(Session["svSelectedPackage"].ToString(), txtSearchReq.Text.Trim());
        //Page.Response.Redirect(Page.Request.Url.ToString(), true);
    }

    protected void btnCopyPasteCancel_Click(object sender, EventArgs e)
    {
        //After closing Reorder popup rebind the Grid on page.
        mpCopyPasteImg.Hide();
        BindReqListDetailed(Session["svSelectedPackage"].ToString(), txtSearchReq.Text.Trim());
        //Page.Response.Redirect(Page.Request.Url.ToString(), true);
    }
    protected void btnViewEditReq_Click(object sender, EventArgs e)
    {
        DoRequirementEdit(hfCurentReqId.Value);
    }

    #endregion

    #region "Code to update the the order of the reqirements "
    //Grag Drop Code - Ajax Call.
    [System.Web.Services.WebMethod(EnableSession = true)]
    [System.Web.Script.Services.ScriptMethod()]
    public static string AutoUpdateGrid(string newOrder)
    {
        try
        {
            dynamic instance = new Requirements();
            instance.DoUpdate(newOrder);
            return "Successful!";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
    }
    public void DoUpdate(string newOrder)
    {
        int[] locationIds = StringToIntList(newOrder).ToArray(); //(from p in newOrder.Split(",")int.Parse(p)).ToArray();
        int preference = 1;
        foreach (int locationId in locationIds)
        {
            this.UpdatePreference(locationId, preference);
            preference += 1;
        }
        //Me.BindGrid()
    }

    public static IEnumerable<int> StringToIntList(string str)
    {
        if (String.IsNullOrEmpty(str))
            yield break;

        foreach (var s in str.Split(','))
        {
            int num;
            if (int.TryParse(s, out num))
                yield return num;
        }
    }
    private void UpdatePreference(int Object_pkid, int newTPos)
    {
        string constr = ConfigurationManager.ConnectionStrings["DefaultConnString"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("UPDATE dbo.bis_object SET TPos = @newTPos WHERE Object_PK = @Object_pkid"))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Object_pkid", Object_pkid);
                    cmd.Parameters.AddWithValue("@newTPos", newTPos);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
    }
    protected void upModalReorder_Load(object sender, EventArgs e)
    {
        if (Request["__EVENTARGUMENT"] == "FromJavaScript")
        {
            BindReorderReqList(Session["svSelectedPackage"].ToString());
        }
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

        Requirements newClass = new Requirements();
        DataTable dtLabels = Common.GetUniqueLabels(searchText).Tables[0];
        int attendeeCount = dtLabels.Rows.Count;

        return newClass.ConvertToJson(dtLabels);
    }

    //code to get all the labels for the curent requirement. called from javascript .
    [System.Web.Services.WebMethod(EnableSession = true)]
    [System.Web.Script.Services.ScriptMethod()]
    public static string GetRequrementLabels(string currentReqID)
    {
        //Get the paged results and the total count of the results for this query. 
        //AttendeeRepository ar = new AttendeeRepository();
        //List<Attendee> attendees = ar.GetAttendees(searchTerm, pageSize, pageNum);

        Requirements newClass = new Requirements();
        DataTable dtLabels = Requirement.GetReqLabels(currentReqID).Tables[0];
        int attendeeCount = dtLabels.Rows.Count;

        return newClass.ConvertToJson(dtLabels);
    }

    //code to update labels for the curent requirement. called from javascript ajax post
    [System.Web.Services.WebMethod(EnableSession = true)]
    [System.Web.Script.Services.ScriptMethod()]
    public static string UpdateRequrementLabels(string currentReqID, string actionEvent, string tagName)
    {
        /*actinEvent will be select or unselect  - check javascript code*/
        try
        {
            Requirement.AddRemoveLabesForRequirement(currentReqID, actionEvent, tagName);
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

    #region "Code for Copy Paste Image upload"

    [System.Web.Services.WebMethod(EnableSession = true)]
    [System.Web.Script.Services.ScriptMethod()]
    public static string UploadCopyPasteImage(string Based64BinaryString, string dataType, int dataSize, string fileName, string objectID)
    {
        try
        {
            string flName = fileName.Replace(" ", "_");
            if (String.IsNullOrEmpty(flName))
                flName = "dummyName";
            //APPDMCAL.InsertToHImages("TempImg_", dataType, dataSize, Based64BinaryString);
            Common.AddNewImage(objectID, flName, dataType, dataSize, Based64BinaryString);
            return "Image Inserted to SQLServer Successfully.!";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
    }

    #endregion

    #region "Grid Events"
    protected void DataListRequirements_ItemCommand(object source, DataListCommandEventArgs e)
    {

        if (e.CommandName == "EditRequirement")
        {
            string guid = e.CommandArgument.ToString();
            DoRequirementEdit(guid);
        }
        else if (e.CommandName == "OpenCopyPasteImagePopup")
        {
            mpCopyPasteImg.Show();
            hfImageObjectPK.Value = e.CommandArgument.ToString();
        }
        else if (e.CommandName == "DeleteImageFromReq")
        {
            Common.DeleteImageFromReq(Convert.ToInt32(e.CommandArgument.ToString()));
            BindReqListDetailed(Session["svSelectedPackage"].ToString(), txtSearchReq.Text.Trim());
            //txtSearchReq.Focus();
            updatePanelReq.Update();
        }
        else if (e.CommandName == "ShowHistoryRequirement")
        {
            string guid = e.CommandArgument.ToString();
            //Open the Proj Pckg in Edit Mode (after inserting the new record).            
            DataTable dtReqDeetails = Requirement.GetRequestDetailView(guid).Tables[0];
            if (dtReqDeetails.Rows.Count == 0)
            {
                lblError.Text = "No data found for Details";
            }
            else
            {
                lblCurrentReqIDHist.Text = dtReqDeetails.Rows[0]["UV_ID"].ToString().Trim();
                lblReqNameHist.Text = dtReqDeetails.Rows[0]["Name"].ToString().Trim();
                hfCurentReqId.Value = guid;
                GridReqHist.PageIndex = 0;
                BindReqHistory(hfCurentReqId.Value);
                upModalReqHist.Update();
                mpViewReqHist.Show();
            }
        }
    }

    protected void DataListRequirements_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (chkshowDetails.Checked)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                System.Web.UI.HtmlControls.HtmlTableRow rowDetails = e.Item.FindControl("trReqDetailsinList") as System.Web.UI.HtmlControls.HtmlTableRow;
                rowDetails.Visible = true;
                Label lblName = e.Item.FindControl("lblReqDetailsNameinList") as Label;
                lblName.Visible = false;
            }
        }
        //For the Image for each Req.
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            //look for Image id 
            int ImageId = (DataBinder.Eval(e.Item.DataItem, "ImageID") == System.DBNull.Value ? 0 : Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "ImageID").ToString()));
            //int ImageId = Convert.ToInt32((DataBinder.Eval(e.Item.DataItem, "ImageID").ToString());
            if (ImageId == 0)
            {
                System.Web.UI.HtmlControls.HtmlGenericControl imgDropBox = e.Item.FindControl("imgDropDiv") as System.Web.UI.HtmlControls.HtmlGenericControl;
                imgDropBox.Visible = true;
                System.Web.UI.HtmlControls.HtmlGenericControl imgShowBox = e.Item.FindControl("imgShowDiv") as System.Web.UI.HtmlControls.HtmlGenericControl;
                imgShowBox.Visible = false;
            }
            else
            {
                //Has image 
                System.Web.UI.HtmlControls.HtmlGenericControl imgDropBox = e.Item.FindControl("imgDropDiv") as System.Web.UI.HtmlControls.HtmlGenericControl;
                imgDropBox.Visible = false;
                System.Web.UI.HtmlControls.HtmlGenericControl imgShowBox = e.Item.FindControl("imgShowDiv") as System.Web.UI.HtmlControls.HtmlGenericControl;
                imgShowBox.Visible = true;

                Image reqImg = e.Item.FindControl("imgReqirementDetail") as Image;
                reqImg.Visible = true;
                reqImg.ImageUrl = "~/Handlers/GetFileHandler.ashx?DocId=" + ImageId.ToString();
            }

            //Ajaxify the Edit and History Links  btnEditProject  btnShowHistReq
            Button btnEditProject = e.Item.FindControl("btnEditProject") as Button;
            Button btnShowHistReq = e.Item.FindControl("btnShowHistReq") as Button;
            ImageButton trashImg = e.Item.FindControl("trashImg") as ImageButton;
            ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(btnEditProject);
            ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(btnShowHistReq);
            ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(trashImg);
        }
    }

    protected void GridReqHist_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridReqHist.PageIndex = e.NewPageIndex;
        BindReqHistory(hfCurentReqId.Value);
        //mpViewPackageHist.Show();
    }
    protected void GridReqHist_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = Convert.ToInt32(e.CommandArgument) - GridReqHist.PageSize * GridReqHist.PageIndex;

        if (e.CommandName == "ViewReqHistInfo")
        {
            //Get the value of guid from the DataKeys using the RowIndex.
            string eguid = GridReqHist.DataKeys[rowIndex]["ea_guid"].ToString();
            int versionNum = Convert.ToInt32(GridReqHist.DataKeys[rowIndex]["Version"].ToString());

            DataTable dt = Requirement.GetReqHistoryDetails(eguid, versionNum).Tables[0];
            if (dt.Rows.Count == 0)
            {
                lblError.Text = "Error: No data found for this History Item.";
            }
            else
            {
                mpViewReqHist.Hide();
                //PrepareForEditProject(eguid, dt.Rows[0]);
                DataRow dr = dt.Rows[0];
                lblViewReqDetailsComplexity.Text = dr["Complexity"].ToString();
                lblViewReqDetailsCreatedBy.Text = dr["FullName"].ToString();
                lblViewReqDetailsCreatedDate.Text = dr["CreatedDate"].ToString();
                lblViewReqDetailsLabel.Text = dr["Labels"].ToString();
                lblViewReqDetailsStatus.Text = dr["Status"].ToString();
                lblViewReqDetailsType.Text = dr["TypeIs"].ToString();
                lblViewReqDetailsVersion.Text = dr["Version"].ToString();
                lblViewReqName.Text = dr["Name"].ToString();
                litViewReqTextNotes.Text = dr["Note"].ToString();
                btnViewEditReq.Visible = ((rowIndex == 0) && (GridReqHist.PageIndex == 0)) ? true : false;
                mpViewReqDetail.Show();
            }
        }
    }

    #endregion

    #region "Supporting sub routines"
    private void BindReqListDetailed(string eaguid, string searchText)
    {
        DataListRequirements.DataSource = Requirement.GetRequestListDetailed(eaguid, searchText);
        DataListRequirements.DataBind();
    }
    private void DoRequirementEdit(string guid)
    {
        DataTable dtReqDeetails = Requirement.GetRequestDetailView(guid).Tables[0];
        if (dtReqDeetails.Rows.Count == 0)
        {
            lblError.Text = "No data found for Details";
        }
        else
        {
            hfCurentReqId.Value = guid;
            PrepareReqUpdate(dtReqDeetails.Rows[0]);
            mpCreateUpdateReq.Show();
            /* BIS -59 Show Labels even for Basic Requiremnt. Register Client Script for CSS and Data */
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "CallIntialLoadForLabels", "initBindLabels('" + guid + "')", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "CallIntialLoadForLabels", "initBindLabels('" + guid + "')", true);
            upCreateUpdateReq.Update();
        }
    }
    private void BindReqHistory(string eguid)
    {
        DataTable dtHistory = Requirement.GetRequirementHistory(eguid).Tables[0];
        GridReqHist.DataSource = dtHistory;
        GridReqHist.DataBind();
    }
    private void BindReorderReqList(string eaguid)
    {
        GridReqReorder.DataSource = Requirement.GetRequestListDefault(eaguid);
        GridReqReorder.DataBind();
    }
    private void PrepareReqCreate()
    {
        //Show the Details TAble
        tblRequirement.Visible = true;
        hfCurentReqId.Value = string.Empty;
        trReqDetails.Visible = true;
        trReqAdditionals.Visible = false;
        trReqLabels.Visible = false;
        txtReqTitle.Text = String.Empty;
        lblReqVersion.Text = "0";
        lblReqCreatedDate.Text = DateTime.Now.ToString();
        lblReqCreatedBy.Text = ((CurrentUser)CurrentUser.GetUserDetails()).FullName;

        htmlEditorCreateUpdateReqNotes.Text = string.Empty;
        //bind 4 dropdowns.
        BindComplexityDropDown(ddlReqComplexity);
        //BindLabelsDropDown(ddlReqLabel);
        BindStatusDropDown(ddlReqStatus);
        BindTypeDropDown(ddlReqType);
        btnCreateUpdateReq.Visible = true;
        lblErrorCreateUpdateReq.Text = "";
        btnCreateUpdateReq.CommandName = "CreateRequirement";
        txtReqTitle.Focus();
    }
    private void PrepareReqUpdate(DataRow dr)
    {
        //Show the Details Table
        tblRequirement.Visible = true;
        //Display additonials on when show details is checked.
        trReqAdditionals.Visible = chkshowDetails.Checked;
        trReqAdditionals2.Visible = chkshowDetails.Checked;

        //Labels and Title wil  be shown on edit.
        trReqLabels.Visible = true;

        txtReqTitle.Text = dr["Name"].ToString();
        lblReqVersion.Text = Convert.ToString(Convert.ToInt32(dr["Version"].ToString()) + 1);
        lblReqCreatedDate.Text = DateTime.Now.ToString();
        lblReqCreatedBy.Text = ((CurrentUser)CurrentUser.GetUserDetails()).FullName;

        htmlEditorCreateUpdateReqNotes.Text = dr["Note"].ToString();
        //bind 4 dropdowns.
        BindComplexityDropDown(ddlReqComplexity);
        ddlReqComplexity.Items.FindByValue(dr["Complexity_GUID"].ToString()).Selected = true;

        BindStatusDropDown(ddlReqStatus);
        ddlReqStatus.Items.FindByValue(dr["Status_GUID"].ToString()).Selected = true;

        BindTypeDropDown(ddlReqType);
        ddlReqType.Items.FindByValue(dr["NType_GUID"].ToString()).Selected = true;

        //BindLabelsDropDown(ddlReqLabel);
        btnCreateUpdateReq.Visible = true;
        lblErrorCreateUpdateReq.Text = "";
        btnCreateUpdateReq.CommandName = "UpdateRequirement";
        txtReqTitle.Focus();
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
    private void BindTypeDropDown(DropDownList dropdownList)
    {
        dropdownList.Items.Clear();
        dropdownList.DataSource = Common.GetTypeCodes().Tables[0];
        dropdownList.DataTextField = "name";
        dropdownList.DataValueField = "Type_GUID";
        dropdownList.DataBind();
    }
    private void BindComplexityDropDown(DropDownList dropdownList)
    {
        dropdownList.Items.Clear();
        dropdownList.DataSource = Common.GetComplexityCodes().Tables[0];
        dropdownList.DataTextField = "name";
        dropdownList.DataValueField = "Complexity_GUID";
        dropdownList.DataBind();
    }
    private void BindLabelsDropDown(DropDownList dropdownList)
    {
        dropdownList.Items.Clear();
        dropdownList.DataSource = Common.GetLabelCodes().Tables[0];
        dropdownList.DataTextField = "name";
        dropdownList.DataValueField = "Label_GUID";
        dropdownList.DataBind();
    }
    #endregion
    protected void btnCreateUpdateReqClose_Click(object sender, EventArgs e)
    {
        mpCreateUpdateReq.Hide();
        //BindReqListDetailed(Session["svSelectedPackage"].ToString(), txtSearchReq.Text.Trim());
    }
}
