using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Account_PasswordReset : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnResetPassword_Click(object sender, EventArgs e)
    {
        pnlSuccess.Visible = false;
        ErrorMessage.Text = "";
        if (String.IsNullOrEmpty(UserName.Text.Trim()) && String.IsNullOrEmpty(EmailTextBox.Text.Trim()))
        {
            ErrorMessage.Text = "Please enter UserName (OR) Email";
        }
        else
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
                    // Change the Code in Admin User Maitenance Password Reset too.
                    //Do reset and send email to user the new temp password. and store the salt instead of the temp password.
                    int lengthOfPassword = 8;
                    string guid = Guid.NewGuid().ToString().Replace("-", "");
                    string tempPassword = guid.Substring(0, lengthOfPassword);
                    DataTable dtUserDetails = new DataTable();
                    string response = Common.ResetPassword(UserName.Text.Trim(), EmailTextBox.Text.Trim(), tempPassword, ref dtUserDetails);
                    if (response == "Success")
                    {
                        //Send Emial and Make the Panel Visible. to check Email.
                        Common.SendPasswordResetEmailToUser(dtUserDetails.Rows[0]["UserLogin"].ToString(), dtUserDetails.Rows[0]["Email"].ToString(), tempPassword);
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
        //Load Recaptha again after partial postback
        ScriptManager.RegisterStartupScript(updatePanelReq, updatePanelReq.GetType(), "loadCaptcha", "grecaptcha.render('recaptcha', {'sitekey': '6LfpNyYTAAAAAL2xoyDHp6u7r-3kP2PaUARua-r8' });", true);
        updatePanelReq.Update();
    }
}