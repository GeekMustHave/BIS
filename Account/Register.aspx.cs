﻿using System;
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
                        Email.Text.Trim(), Department.SelectedValue.ToString());
                    if (response == "Success")
                    {
                        string outMessage = "";
                        Common.DoLogin(UserName.Text.Trim(), Password.Text.Trim(), false, ref outMessage);
                        if (!string.IsNullOrEmpty(outMessage))
                        {
                            ErrorMessage.Text = "Account created, but cannot log you at this time. Please contact Admin.";
                        }
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
}
