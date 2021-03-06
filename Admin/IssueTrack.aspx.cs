﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.VisualBasic;
using System.Data;
using System.Diagnostics;
using JiraITClient;
using RestSharp;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary: Page to allow users to add issue tracks to JIRA.
/// History:
/// -------------------------------------------------------------------------------
/// Date:               Name                Description
/// --------------------------------------------------------------------------------
/// 7/10/2015           JHRS                Intial Creation 
/// </summary>
public partial class Admin_IssueTrack : System.Web.UI.Page
{

    JiraITClient.JiraClient jClient = new JiraClient();
    /// <summary>
    /// Property to store Issue track grid sort direction
    /// </summary>
    private string gridITSortDirection
    {
        get
        {
            if (ViewState["gridITSortDirection"] == null)
            {
                return "ASC";
            }
            else
            {
                return ViewState["gridITSortDirection"].ToString();
            }
        }
        set { ViewState["gridITSortDirection"] = value; }
    }
    /// <summary>
    /// Property to store issue track grid sort expression.
    /// </summary>
    private string gridITSortExpression
    {
        get
        {
            if (ViewState["gridITSortExpression"] == null)
            {
                return "id,created";
            }
            else
            {
                return ViewState["gridITSortExpression"].ToString();
            }
        }
        set { ViewState["gridITSortExpression"] = value; }
    }

    #region "Page Events"
    protected void Page_Load(object sender, System.EventArgs e)
    {
        // On page load, set the active view to Issue track List and Get all the Issue track for the User.
        if (!Page.IsPostBack)
        {
            ViewIssueTrackListView();
        }
        //Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenSuccessNotification", "OpenITCreateNotification('BIS-99900')", true);
    }
    #endregion

    #region "Grid Events"
    protected void GridIssueTrackList_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
    {
        //Move to the new page
        GridIssueTrackList.PageIndex = e.NewPageIndex;
        GridIssueTrackList.SelectedIndex = -1;
        BindIssueTrackGrid();
    }

    protected void GridIssueTrackList_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
    {
        //Grid Item Command.
        //When user clicked on edit load Edit view so that USer can update the Issue Track.
        if (e.CommandName == "editIT")
        {
            try
            {
                lblErrorVwEdit.Text = String.Empty;
                ViewState["SortExpression"] = null;
                string itkey = e.CommandArgument.ToString();
                EditITDetails(itkey);
                mvIssueTrack.SetActiveView(vwITVIEWEDIT);
            }
            catch (Exception ex)
            {
                lblErrorVwEdit.Text = "Error :" + ex.Message.ToString();
            }
        }
    }

    protected void GridIssueTrackList_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
    {
        //Go back to the first page
        GridIssueTrackList.PageIndex = 0;
        GridIssueTrackList.SelectedIndex = -1;

        //Set the new sort field
        gridITSortExpression = e.SortExpression;

        //Flip the sort direction to be the opposite of the previous sort direction
        if (gridITSortDirection == "DESC")
            gridITSortDirection = "ASC";
        else
            gridITSortDirection = "DESC";

        //REbind the Grid View to persist the changes 
        BindIssueTrackGrid();
    }

    #endregion

    #region "Buttton Events"
    /// <summary>
    /// Button Event - when user cicked on return to list from Edit View
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnReturnFromViewEdit_Click(object sender, System.EventArgs e)
    {
        ViewIssueTrackListView();
    }
    /// <summary>
    /// Code to Add a Comment to an Issue Track
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAddComment_Click(object sender, System.EventArgs e)
    {
        try
        {
            lblErrorVwEdit.Text = "";
            string comment = jClient.AddComment(btnAddComment.CommandArgument, txtAddCommentEdit.Text.Trim(), this.User.Identity.Name);
            if (!string.IsNullOrEmpty(comment))
            {
                EditITDetails(btnAddComment.CommandName);
                mvIssueTrack.SetActiveView(vwITVIEWEDIT);
            }
            SendEmailToSSOAuthor(btnAddComment.CommandArgument, "New Comment Added");
        }
        catch (Exception ex)
        {
            lblErrorVwEdit.Text = "Error :" + ex.Message.ToString();
        }
    }
    /// <summary>
    /// COde to Upate the Issue Track with the User Edited Fields.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnUpdateIssueTRackEdit_Click(object sender, System.EventArgs e)
    {
        try
        {
            // MOst od the fields are self explanatory.
            lblErrorVwEdit.Text = "";
            string curIt = ViewState["CurrentIssueTrack"].ToString();
            Issue it = jClient.GetIssueTrack(curIt);
            if (!string.IsNullOrEmpty(lblSummaryEdit.Text.Trim()))
            {
                it.fields.summary = lblSummaryEdit.Text.Trim();
            }
            if (!string.IsNullOrEmpty(txtDescriptionEdit.Text.Trim()))
            {
                it.fields.description = txtDescriptionEdit.Text.Trim();
            }

            //Update issue Type
            it.fields.issuetype.name = ddlITTypeEdit.SelectedValue.ToString();
            it.fields.priority.name = ddlITPriorityEdit.SelectedValue.ToString();
            it.fields.status.name = ddlITStatusEdit.SelectedValue.ToString();
            if (!(ddlITResolutionTypeEdit.SelectedIndex == 0))
            {
                it.fields.resolution.name = ddlITResolutionTypeEdit.SelectedValue.ToString();
            }
            Issue updatedIT = jClient.UpdateIssue(it);
            EditITDetails(updatedIT.key);
            //BindIssueTrackGrid()
            //mvIssueTrack.SetActiveView(vwITList)
            lblErrorVwEdit.ForeColor = System.Drawing.Color.Green;
            lblErrorVwEdit.Text = "Issue Track Updated Succesfully.";
            SendEmailToSSOAuthor(btnAddComment.CommandArgument, "Issue Track Updated");
        }
        catch (Exception ex)
        {
            lblErrorVwEdit.ForeColor = System.Drawing.Color.Red;
            lblErrorVwEdit.Text = "Error :" + ex.Message.ToString();
        }
    }

    /// <summary>
    /// Event for changing Status (Transisiotn) of the Issue.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <remarks></remarks>
    protected void ITActionbtn_click(object sender, System.EventArgs e)
    {
        LinkButton lb = (LinkButton)sender;
        //Go to Dll and change the Status resoluton - Transisiton of the Issue.        
        string curIt = ViewState["CurrentIssueTrack"].ToString();
        bool aa = jClient.UpdateIssueTransition(curIt, Convert.ToInt32(lb.CommandArgument), ddlITResolutionTypeEdit.SelectedItem.Text);
        EditITDetails(curIt);
        SendEmailToSSOAuthor(btnAddComment.CommandArgument, "Issue Track Status Updated");
    }

    /// <summary>
    /// Button Even to Prepre the Screen for Creating a new Issue Track for the USer.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCreateIT_Click(object sender, System.EventArgs e)
    {
        try
        {
            // Bind Priority and Assign To DropDowns and Set active view to Create new IT.
            lblErrorVwList.Text = string.Empty;
            txtDescriptionNew.Text = string.Empty;
            txtSummaryNew.Text = string.Empty;
            //BindPriority ddl
            BindPriorityDropDown(jClient, ddlPriorityNew);
            BindAssignToDropdown(ddlAssigneeNew);
            mvIssueTrack.SetActiveView(vwITCREATE);
        }
        catch (Exception ex)
        {
            lblErrorVwList.Text = "Error :" + ex.Message.ToString();
        }
    }
    /// <summary>
    /// Add New Issue Track to Jira Interface with all the fields user has enetered.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAddIssueTrack_Click(object sender, System.EventArgs e)
    {
        try
        {
            lblErrorVwNew.Text = string.Empty;
            //string iTkey = jClient.CreateNewIssue("BIS", txtSummaryNew.Text, txtDescriptionNew.Text, ddlPriorityNew.SelectedValue, ddlAssigneeNew.SelectedValue, this.User.Identity.Name);
            string iTkey = jClient.CreateNewIssue("BIS", txtSummaryNew.Text, txtDescriptionNew.Text, ddlPriorityNew.SelectedValue,
                                                    "jschust2", this.User.Identity.Name, ((CurrentUser)CurrentUser.GetUserDetails()).Email);
            ViewIssueTrackListView();
            Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenSuccessNotification", "OpenITCreateNotification('" + iTkey + "')", true);

            //Send Email to John mindspring Account when a new IT is created.
            string emailSubject = "[JIRA] (" + iTkey + ") " + txtSummaryNew.Text.Trim();
            string emailBody = Email.PrepareITCreateEmailBody(iTkey, ((CurrentUser)CurrentUser.GetUserDetails()).FullName, "Task", DateTime.Now.ToString(),
                                ddlPriorityNew.SelectedValue, txtSummaryNew.Text.Trim(), txtDescriptionNew.Text.Trim());
            Email.SendEmail("john.schuster@mindspring.com", emailSubject, emailBody);
        }
        catch (Exception ex)
        {
            lblErrorVwNew.Text = "Error :" + ex.Message.ToString();
        }
    }

    /// <summary>
    /// Button Even to Upload a new document (file) to an exisiting Issue Track to JIRA INterface
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnUploadNewFile_Click(object sender, System.EventArgs e)
    {
        if (fileUpload.PostedFile != null && !string.IsNullOrEmpty(fileUpload.PostedFile.FileName))
        {
            try
            {
                string curIt = ViewState["CurrentIssueTrack"].ToString();
                lblErrorVwEdit.Text = "";
                jClient.AddNewAttachment(curIt, fileUpload.PostedFile.InputStream, fileUpload.FileName);
                EditITDetails(curIt);
                lblErrorVwEdit.ForeColor = System.Drawing.Color.Green;
                lblErrorVwEdit.Text = "Attachment added Successfully.";
                SendEmailToSSOAuthor(btnAddComment.CommandArgument, "New Document Uploaded");
            }
            catch (Exception ex)
            {
                lblErrorVwEdit.ForeColor = System.Drawing.Color.Red;
                lblErrorVwEdit.Text = "Error: " + ex.Message.ToString();
            }
        }
        else
        {
            lblErrorVwEdit.ForeColor = System.Drawing.Color.Red;
            lblErrorVwEdit.Text = "Document cannot be empty.";
        }
    }
    #endregion

    #region "Supporting subs"

    /// <summary>
    /// 
    /// </summary>
    /// <param name="issueTrackID"></param>
    private void SendEmailToSSOAuthor(string issueTrackID, string updateReason)
    {
        try
        {
            Issue it = jClient.GetIssueTrack(issueTrackID);
            if (!String.IsNullOrEmpty(it.fields.customfield_11000))
            {
                string emailSubject = "BIS-JIRA (" + it.key + ") Update - " + it.fields.summary;
                string emailBody = "Issue Track : "+ it.key + " <br /> Update Status : " + updateReason + " by BIS-Administrator";
                Email.SendEmail(it.fields.customfield_11000, emailSubject, emailBody);
            }
        }
        catch (Exception)
        {
            lblErrorVwEdit.Text = "Issue Track Updated successfully. But cannot Send email";
        }
    }
    /// <summary>
    /// Repetative methods to use when we need to switch to IT Lsit view 
    /// </summary>
    private void ViewIssueTrackListView()
    {
        mvIssueTrack.SetActiveView(vwITList);
        BindIssueTrackGrid();
        BindFilterByddls();
    }

    /// <summary>
    /// Function to bind Issue Track Grid. Connects to Jira Interface and Gets All the user created IT's in BIS with "ISSUE-TRACK" label
    /// </summary>
    private void BindIssueTrackGrid()
    {
        try
        {
            lblErrorVwList.Text = "";
            string curStatusValue = (string.IsNullOrEmpty(ddlSearchByStatus.SelectedValue.ToString()) ? "" : ddlSearchByStatus.SelectedValue);
            //get issue tracks with selected criteria from the user.
            DataView dv = jClient.GetIssues("BIS", "ISSUE-TRACK", ddlSearchByType.SelectedValue.ToString(), curStatusValue).DefaultView;
            //BIS 95 - Any user of BIS should be able to see issue tracks, we are promoting transparency
            //dv.RowFilter = "SSoUserID = '" + User.Identity.Name.ToString() + "'";

            //if all(status) is selected, filter "All Except Done"
            if (string.IsNullOrEmpty(curStatusValue)) dv.RowFilter = "Status <> 'Done'";

            dv.Sort = gridITSortExpression + " " + gridITSortDirection;
            GridIssueTrackList.DataSource = dv;
            GridIssueTrackList.DataBind();
        }
        catch (Exception ex)
        {
            GridIssueTrackList.DataSource = new DataTable();
            GridIssueTrackList.DataBind();
            lblErrorVwList.Text = "Error :" + ex.Message.ToString();
        }
    }
    /// <summary>
    /// Funtion to bind issue tracks filter dropdpwns, filter by type and status.
    /// </summary>
    private void BindFilterByddls()
    {
        try
        {
            ddlSearchByStatus.Items.Clear();
            //ddlSearchByStatus.DataSource = jClient.GetIssueStatusCodes();
            //ddlSearchByStatus.DataTextField = "Text";
            //ddlSearchByStatus.DataValueField = "Value";
            //ddlSearchByStatus.DataBind();
            ddlSearchByStatus.Items.Insert(0, new ListItem("All except done", ""));
            ddlSearchByStatus.Items.Add(new ListItem("In Progress", "In Progress"));
            ddlSearchByStatus.Items.Add(new ListItem("To Do", "To Do"));
            ddlSearchByStatus.Items.Add(new ListItem("Done", "Done"));
            ddlSearchByStatus.Items.Add(new ListItem("In Review", "In Review"));

            ddlSearchByType.Items.Clear();
            ddlSearchByType.DataSource = jClient.GetIssueTypeCodes();
            ddlSearchByType.DataTextField = "Text";
            ddlSearchByType.DataValueField = "Value";
            ddlSearchByType.DataBind();
            ddlSearchByType.Items.Insert(0, new ListItem("*All", ""));
        }
        catch (Exception ex)
        {

            lblErrorVwList.Text = "Error :" + ex.Message.ToString();
        }

    }
    /// <summary>
    /// Sub To Prepare Screen for Eiditng an IT.
    /// </summary>
    /// <param name="key"></param>
    private void EditITDetails(string key)
    {
        BindDropDowns(jClient);
        Issue it = jClient.GetIssueTrack(key);
        ViewState["CurrentIssueTrack"] = it.id;
        lblITID.Text = key;
        lblSummaryEdit.Text = it.fields.summary;
        lblCreatedOn.Text = DateTime.Parse(it.fields.created).ToString("MM/dd/yyyy");

        //lblITLabelsEdit.Text = ""
        //For Each s As String In it.fields.labels
        //    lblITLabelsEdit.Text += s & Convert.ToString(",")
        //Next
        txtDescriptionEdit.Text = it.fields.description;

        txtAddCommentEdit.Text = "";
        lblCreatebySSO.Text = it.fields.customfield_10900;
        //Comments
        divCommentsEdit.InnerHtml = "";
        for (int i = 0; i <= it.fields.comment.total - 1; i++)
        {
            //divCommentsEdit.InnerHtml += "User: " + it.fields.comment.comments[i].author.displayName + " - " + DateTime.Parse(it.fields.comment.comments[i].created);
            divCommentsEdit.InnerHtml += "JIRA User added comment on - " + DateTime.Parse(it.fields.comment.comments[i].created);
            divCommentsEdit.InnerHtml += "<br />Comment :- " + it.fields.comment.comments[i].body + " <hr />";
        }

        ddlITPriorityEdit.SelectedValue = it.fields.priority.name;
        ddlITStatusEdit.SelectedValue = it.fields.status.name;
        ddlITTypeEdit.SelectedValue = it.fields.issuetype.name;
        lblITtypeEdit.Text = it.fields.issuetype.name;
        lblITStatusEdit.Text = it.fields.status.name;
        if (it.fields.resolution == null)
        {
            lblResolutionEdit.Text = "Unresolved";
        }
        else
        {
            lblResolutionEdit.Text = it.fields.resolution.name;
        }

        //Add Action/transition buttons
        if (it.transitions.Count > 0)
        {
            if (it.transitions.Count == 1)
            {
                btnITAction_1.Text = it.transitions[0].name;
                btnITAction_1.CommandArgument = it.transitions[0].id.ToString();
                btnITAction_1.Visible = true;
                btnITAction_2.Visible = false;
                btnITAction_3.Visible = false;
            }
            else if (it.transitions.Count == 2)
            {
                btnITAction_1.Text = it.transitions[0].name;
                btnITAction_1.CommandArgument = it.transitions[0].id.ToString();
                btnITAction_1.Visible = true;
                btnITAction_2.Text = it.transitions[1].name;
                btnITAction_2.CommandArgument = it.transitions[1].id.ToString();
                btnITAction_2.Visible = true;
                btnITAction_3.Visible = false;
            }
            else
            {
                btnITAction_1.Text = it.transitions[0].name;
                btnITAction_1.CommandArgument = it.transitions[0].id.ToString();
                btnITAction_1.Visible = true;
                btnITAction_2.Text = it.transitions[1].name;
                btnITAction_2.CommandArgument = it.transitions[1].id.ToString();
                btnITAction_2.Visible = true;
                btnITAction_3.Text = it.transitions[2].name;
                btnITAction_3.CommandArgument = it.transitions[2].id.ToString();
                btnITAction_3.Visible = true;
            }
        }

        //finally add attachments
        phAttachmentsEdit.Controls.Clear();
        for (int i = 0; i <= it.fields.attachment.Count - 1; i++)
        {
            LinkButton btnITAttachment = new LinkButton();
            btnITAttachment.ID = "btnITAttachmentWithID" + i.ToString();
            btnITAttachment.Text = it.fields.attachment[i].filename;
            btnITAttachment.CommandName = it.fields.attachment[i].mimeType;
            btnITAttachment.CommandArgument = it.fields.attachment[i].content;
            btnITAttachment.Attributes.Add("onclick", "javascript:return DownloadFile('" + it.fields.attachment[i].content + "')");
            phAttachmentsEdit.Controls.Add(btnITAttachment);
            //var uiBreak = new HtmlGenericControl("br");
            phAttachmentsEdit.Controls.Add(new LiteralControl("&nbsp;&nbsp;&nbsp;"));
        }

        btnAddComment.CommandName = it.key;
        btnAddComment.CommandArgument = it.id.ToString();

        //Get the Sprints IT belogns to
        lblSprint.Text = jClient.GetSprintsForIssue(it.fields.customfield_10007);
    }
    /// <summary>
    /// Fucntion to Bind ITStatus, Resuloutin  and priority  dowpdowns for an Issue Track (From JIRA).
    /// </summary>
    /// <param name="jClient"></param>
    private void BindDropDowns(JiraClient jClient)
    {
        ddlITStatusEdit.Items.Clear();
        ddlITStatusEdit.DataSource = jClient.GetIssueStatusCodes();
        ddlITStatusEdit.DataTextField = "Text";
        ddlITStatusEdit.DataValueField = "Value";
        ddlITStatusEdit.DataBind();

        ddlITResolutionTypeEdit.Items.Clear();
        ddlITResolutionTypeEdit.DataSource = jClient.GetResolutionStatusCodes();
        ddlITResolutionTypeEdit.DataTextField = "Text";
        ddlITResolutionTypeEdit.DataValueField = "Value";
        ddlITResolutionTypeEdit.DataBind();

        BindPriorityDropDown(jClient, ddlITPriorityEdit);

        ddlITTypeEdit.Items.Clear();
        ddlITTypeEdit.DataSource = jClient.GetIssueTypeCodes();
        ddlITTypeEdit.DataTextField = "Text";
        ddlITTypeEdit.DataValueField = "Value";
        ddlITTypeEdit.DataBind();

    }
    /// <summary>
    /// Function to bind Isse Track Priority Dopdown.
    /// </summary>
    /// <param name="jClient"></param>
    /// <param name="ddlPriority"></param>
    private void BindPriorityDropDown(JiraClient jClient, DropDownList ddlPriority)
    {
        ddlPriority.Items.Clear();
        ddlPriority.DataSource = jClient.GetPriorityTypes();
        ddlPriority.DataTextField = "Text";
        ddlPriority.DataValueField = "Value";
        ddlPriority.DataBind();
    }

    /// <summary>
    /// Fcuntion to bind Issue track Assign TO Dropdown.
    /// </summary>
    /// <param name="ddlAssignTo"></param>
    private void BindAssignToDropdown(DropDownList ddlAssignTo)
    {
        ddlAssignTo.Items.Clear();
        ddlAssignTo.DataSource = jClient.GetAssignToUsers("BIS");
        ddlAssignTo.DataTextField = "Text";
        ddlAssignTo.DataValueField = "Value";
        ddlAssignTo.DataBind();
    }

    //public Admin_IssueTrack()
    //{
    //    Load += Page_Load;
    //}

    #endregion

    protected void ddlSearchByStatusNType_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindIssueTrackGrid();
    }
}