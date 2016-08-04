using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Account_Register : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        //Registerd users shouldn't access this page   
        if (!Page.IsPostBack)
        {
            if (Request.IsAuthenticated)
            {
                Response.Redirect("~/Packages.aspx");
            }
        }
    }

    protected void CreateUserButton_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            try
            {
                string EncodedResponse = Request.Form["g-Recaptcha-Response"];
                bool IsCaptchaValid = false;
                bool IgoreCaptha = true;
                try
                {
                    IsCaptchaValid = (GoogleReCaptcha.Validate(EncodedResponse).ToLower() == "true" ? true : false);
                    IgoreCaptha = false;
                }
                catch (Exception)
                {
                    // Send email to admin in future if wanted.
                    IgoreCaptha = true;
                }

                if ((IgoreCaptha == false) && (IsCaptchaValid == false))
                {
                    //In Valid Request
                    ErrorMessage.Text = "Captcha Required.";
                }
                else
                {
                    string response = Common.CreateNewUser(UserName.Text.Trim(), Password.Text.Trim(), FirstName.Text.Trim(), LastName.Text.Trim(),
                        EmailTextBox.Text.Trim(), Department.SelectedValue.ToString());
                    if (response == "Success")
                    {
                        try
                        {
                            //Send Email to User about Registration SuccessFull
                            SendRegstartionSuccessEmail(EmailTextBox.Text.Trim(), UserName.Text.Trim(), FirstName.Text.Trim() + ", " + LastName.Text.Trim());

                            //Send Email To Admin about the new New User Regisration.
                            SendAdminEmailForNewRegistration(EmailTextBox.Text.Trim(), UserName.Text.Trim(), FirstName.Text.Trim() + ", " + LastName.Text.Trim(), Department.SelectedValue.ToString());
                        }
                        catch (Exception)
                        {
                            // ignore Exception and log in furutre.. canot send email to admin and user
                        }
                        //REdirect to RegistrationSuccess Page
                        Response.Redirect("~/Account/RegistrationSuccess.aspx?FromUrl=" + Request.Url.ToString(), true);
                    }
                    else
                    {
                        ErrorMessage.Text = "Error: " + response;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage.Text = "Error while Creating: " + ex.Message.ToString();
            }
        }
        //Load Recaptha again after partial postback
        ScriptManager.RegisterStartupScript(updatePanelReq, updatePanelReq.GetType(), "loadCaptcha", "grecaptcha.render('recaptcha', {'sitekey': '6LfpNyYTAAAAAL2xoyDHp6u7r-3kP2PaUARua-r8' });", true);
        updatePanelReq.Update();
    }
    private void SendRegstartionSuccessEmail(string userEmail, string userName, string userFullName)
    {

        string emailSubject = "BIS Registration Successfull";
        string emailBody = "<p> " + userFullName + ", <br /> You have successfully registered as a new user on the system. " + "<br /> Your UserName : " + userName +
        "<br /> However, your account is <i>NOT</i> active untill the Administrator approves it, you will recieve another email when that has occured.<br /><br />" +
        "-BIS Admin.</p>";
        Email.SendEmail(userEmail, emailSubject, emailBody);
    }
    private void SendAdminEmailForNewRegistration(string userEmail, string userName, string userFullName, string userDepartment)
    {
        string emailSubject = "BIS New User Registered";
        string emailBody = "<p>New User has successfully registered on the system. Below are the details."
            + "<br />  UserName : " + userName
            + "<br />  FullName : " + userFullName
            + "<br />  UserEmail : " + userEmail
            + "<br />  UserDepatment : " + userDepartment
            + "<br /><br /> Please logon to <a href='http://bis.pwc-saas.com/Admin/UserMaintenance.aspx'>BIS User Maint</a> to activate the user.</p>";
        Email.SendEmailToAdmin(emailSubject, emailBody);
    }
}
