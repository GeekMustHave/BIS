using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Account_ChangePassword : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnUpdatePassword_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            ErrorMessage.Text = "";
            pnlSuccess.Visible = false;
            try
            {
                string curUserId = ((CurrentUser)CurrentUser.GetUserDetails()).User_GUID;
                if (string.IsNullOrEmpty(curUserId))
                {
                    Response.Redirect("~/Account/Login.aspx");
                }
                else
                {
                    string response = Common.ChangePassword(curUserId, CurrentPassword.Text, NewPassword.Text);
                    if (response == "Success")
                    {
                        //Send Emial and Make the Panel Visible. to check Email.
                        SendEmailToUser(((CurrentUser)CurrentUser.GetUserDetails()).Email);
                        pnlSuccess.Visible = true;                        
                    }
                    else
                    {
                        ErrorMessage.Text = "Error: " + response;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage.Text = "Error :" + ex.Message.ToString();
            }
        }
        updatePanelReq.Update();
    }
    private void SendEmailToUser(string inEmail)
    {
        try
        {
            string emailSubject = "BIS Password Updated";
            string emailBody = "Your password has been updated successfully. <br />";
            emailBody += "<br /> <br /> - BIS-Administrator";
            Email.SendEmailToAdmins(inEmail, emailSubject, emailBody);
        }
        catch (Exception)
        {
            throw;
        }
    }
}
