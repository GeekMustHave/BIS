using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_UserMaintenance : System.Web.UI.Page
{
    private string gridUserSortDirection
    {
        get
        {
            if (ViewState["gridUserSortDirection"] == null)
            {
                return "ASC";
            }
            else
            {
                return ViewState["gridUserSortDirection"].ToString();
            }
        }
        set { ViewState["gridUserSortDirection"] = value; }
    }
    /// <summary>
    /// Property to store Users grid sort expression.
    /// </summary>
    private string gridUserSortExpression
    {
        get
        {
            if (ViewState["gridUserSortExpression"] == null)
            {
                return "FullName";
            }
            else
            {
                return ViewState["gridUserSortExpression"].ToString();
            }
        }
        set { ViewState["gridUserSortExpression"] = value; }
    }
    #region "Page Events"

    protected void Page_Load(object sender, EventArgs e)
    {
        // On page load, set the active view to USers List
        if (!Page.IsPostBack)
        {
            ViewUsersListView();
        }
    }
    #endregion

    #region "Suports"
    private void ViewUsersListView()
    {
        mvUsers.SetActiveView(vwUserList);
        BindUsersGrid();

    }

    private void BindUsersGrid()
    {
        try
        {
            lblErrorVwList.Text = "";
            DataView dv = Admin.GetAllUsers().DefaultView;

            dv.Sort = gridUserSortExpression + " " + gridUserSortDirection;
            GridUsersList.DataSource = dv;
            GridUsersList.DataBind();
        }
        catch (Exception ex)
        {
            GridUsersList.DataSource = new DataTable();
            GridUsersList.DataBind();
            lblErrorVwList.Text = "Error :" + ex.Message.ToString();
        }
    }
    #endregion

    #region "Grid Events"
    protected void GridUsersList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //Grid Item Command.
        //When user clicked on edit load Edit view so that USer can update the Users.
        if (e.CommandName == "UserEdit")
        {
            lblErrorVwList.Text = "";
            try
            {
                lblErrorVwEdit.Text = String.Empty;
                int rowIndex = Convert.ToInt32(e.CommandArgument) - GridUsersList.PageSize * GridUsersList.PageIndex;
                string userName = GridUsersList.DataKeys[rowIndex]["UserName"].ToString();
                string usrEmail = GridUsersList.DataKeys[rowIndex]["Email"].ToString();
                string FirstName = GridUsersList.DataKeys[rowIndex]["FirstName"].ToString();
                string LastName = GridUsersList.DataKeys[rowIndex]["LastName"].ToString();
                bool ActiveFlag = (GridUsersList.DataKeys[rowIndex]["Active"].ToString() == "Yes" ? true : false);
                string Department = GridUsersList.DataKeys[rowIndex]["Department"].ToString();
                string LastLoginDate = GridUsersList.DataKeys[rowIndex]["LastLoginDate"].ToString();
                string Roles = GridUsersList.DataKeys[rowIndex]["Roles"].ToString();
                string FullName = GridUsersList.DataKeys[rowIndex]["FullName"].ToString();
                btnUpdateUser.CommandArgument = GridUsersList.DataKeys[rowIndex]["User_GUID"].ToString();

                lblUserFullName.Text = FullName;
                string[] rolesIndiv = Roles.Split(';');
                foreach (ListItem item in cblRoles.Items)
                {
                    item.Selected = false;
                }
                foreach (string role in rolesIndiv)
                {
                    foreach (ListItem item in cblRoles.Items)
                    {
                        if (item.Value == role) { item.Selected = true; break; }
                    }
                }
                lblLastLogin.Text = LastLoginDate;
                txtUserName.Text = userName;
                txtFirstName.Text = FirstName;
                txtLastName.Text = LastName;
                chkActive.Checked = ActiveFlag;
                ddlDepartment.SelectedValue = Department;
                txtEmail.Text = usrEmail;
                mvUsers.SetActiveView(vwUserVIEWEDIT);
            }
            catch (Exception ex)
            {
                lblErrorVwList.Text = "Error :" + ex.Message.ToString();
            }
        }
        else if (e.CommandName == "UserResetPassword")
        {
            lblErrorVwList.Text = "";
            try
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument) - GridUsersList.PageSize * GridUsersList.PageIndex;
                lblErrorVwList.Text = String.Empty;
                string userName = GridUsersList.DataKeys[rowIndex]["UserName"].ToString();
                string usrEmail = GridUsersList.DataKeys[rowIndex]["Email"].ToString();

                // Change the Code in Password Reset Page too.
                //Do reset and send email to user the new temp password. and store the salt instead of the temp password.
                int lengthOfPassword = 8;
                string guid = Guid.NewGuid().ToString().Replace("-", "");
                string tempPassword = guid.Substring(0, lengthOfPassword);
                DataTable dtUserDetails = new DataTable();
                string response = Common.ResetPassword(userName, usrEmail, tempPassword, ref dtUserDetails);
                if (response == "Success")
                {
                    Common.SendPasswordResetEmailToUser(userName, usrEmail, tempPassword);
                    string notificationMsg = "Password reset successfull for <b>" + userName + "</b>. <br /> User has been notified via email."
                        + "<br />  This message will disappear in 5 seconds.";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenSuccessNotification", "OpenSuccessNotification('" + notificationMsg + "')", true);

                    //lblErrorVwList.ForeColor = Color.Green;
                    //lblErrorVwList.Text = "Password reset successfull for '" + userName + "', User has been notified via email.";
                }
                else
                {
                    lblErrorVwList.ForeColor = Color.Red;
                    lblErrorVwList.Text = "Error: " + response;
                }
            }
            catch (Exception Ex)
            {
                lblErrorVwList.ForeColor = Color.Red;
                lblErrorVwList.Text = "Error :" + Ex.Message.ToString();
            }
        }
    }
    protected void GridUsersList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //Move to the new page
        GridUsersList.PageIndex = e.NewPageIndex;
        GridUsersList.SelectedIndex = -1;
        BindUsersGrid();
    }
    protected void GridUsersList_Sorting(object sender, GridViewSortEventArgs e)
    {
        //Go back to the first page
        GridUsersList.PageIndex = 0;
        GridUsersList.SelectedIndex = -1;

        //Set the new sort field
        gridUserSortExpression = e.SortExpression;

        //Flip the sort direction to be the opposite of the previous sort direction
        if (gridUserSortDirection == "DESC")
            gridUserSortDirection = "ASC";
        else
            gridUserSortDirection = "DESC";

        //REbind the Grid View to persist the changes 
        BindUsersGrid();
    }
    #endregion

    protected void btnUpdateUser_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            bool atleastOneRole = false;
            foreach (ListItem item in cblRoles.Items)
            {
                if (item.Selected)
                {
                    atleastOneRole = true;
                    break;
                }
            }
            if (!atleastOneRole)
            {
                lblErrorVwEdit.Text = "Error : Please select at least one role";
            }
            else
            {
                String allRoles = "";
                foreach (ListItem item in cblRoles.Items)
                {
                    if (item.Selected)
                    {
                        allRoles += item.Value + ";";
                    }
                }
                if (!String.IsNullOrEmpty(allRoles))
                {
                    allRoles = allRoles.Substring(0, allRoles.Length - 1);
                }
                string response = Common.UpdateUser(btnUpdateUser.CommandArgument, txtUserName.Text.Trim(), txtFirstName.Text.Trim(), txtLastName.Text.Trim(),
                                txtEmail.Text.Trim(), ddlDepartment.SelectedValue.ToString(), chkActive.Checked, allRoles);
                if (response == "Success")
                {
                    string notificationMsg = "User info updated successfully for <b>" + txtUserName.Text + "</b>."
                                    + "<br />  This message will disappear in 5 seconds.";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenSuccessNotification", "OpenSuccessNotification('" + notificationMsg + "')", true);
                    ViewUsersListView();
                }
                else
                {
                    lblErrorVwEdit.Text = "Error: " + response;
                }
            }
        }
    }
    protected void btnReturnToList_Click(object sender, EventArgs e)
    {
        ViewUsersListView();
    }
}