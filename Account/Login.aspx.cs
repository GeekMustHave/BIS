using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Security;
using System.Data;

public partial class Account_Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Request.IsAuthenticated && !string.IsNullOrEmpty(Request.QueryString["ReturnUrl"]))
            {
                // This is an unauthorized, authenticated request...
                Response.Redirect("~/Unauthorized.aspx");
            }
        }
        //u comment when the register is implemented.
        //RegisterHyperLink.NavigateUrl = "Register.aspx?ReturnUrl=" + HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
    }
    protected void LoginButton_Click(object sender, EventArgs e)
    {
        string usrName = UserName.Text.Trim();
        int validationResult = ValidateUser(usrName, Password.Text);
        if (validationResult == 1)
        {
            // Get the User Info and store it in session.
            DataTable dtUsrInfo = Common.GetCurrentUserInfo(usrName).Tables[0];
            CurrentUser usr;
            string usrRoles = string.Empty;
            if (dtUsrInfo.Rows.Count == 0)
            {
                usr = new CurrentUser();
            }
            else
            {
                usrRoles = dtUsrInfo.Rows[0]["Roles"].ToString().Trim();
                usr = new CurrentUser(dtUsrInfo.Rows[0]["User_GUID"].ToString().Trim(), dtUsrInfo.Rows[0]["FirstName"].ToString().Trim(),
                                      dtUsrInfo.Rows[0]["Surname"].ToString().Trim(), dtUsrInfo.Rows[0]["E_Mail"].ToString().Trim(),
                                      (string.IsNullOrEmpty(usrRoles)) ? "Open" : usrRoles);

            }
            usrRoles = (string.IsNullOrEmpty(usrRoles)) ? "Open" : usrRoles; //CompileUseRoles(usrName);

            FormsAuthenticationTicket tkt;
            string cookiestr;
            HttpCookie ck;
            tkt = new FormsAuthenticationTicket(1, usrName, DateTime.Now, DateTime.Now.AddMinutes(30),
                                                RememberMe.Checked, usrRoles, FormsAuthentication.FormsCookiePath);

            cookiestr = FormsAuthentication.Encrypt(tkt);
            ck = new HttpCookie(FormsAuthentication.FormsCookieName, cookiestr);
            if (RememberMe.Checked)
                ck.Expires = tkt.Expiration;
            ck.Path = FormsAuthentication.FormsCookiePath;
            Response.Cookies.Add(ck);

            //Add  roles to thePrincipal in Global Asax.

            string strRedirect;
            strRedirect = Request.QueryString["ReturnUrl"];
            if (strRedirect == null)
                strRedirect = "~/Packages.aspx";
            Response.Redirect(strRedirect, true);
        }
        else if (validationResult == 0)
        {
            FailureText.Text = "Invalid Username or Password. Try again.!";
            //Response.Redirect("Login.aspx", true);
        }
        else if (validationResult == -1)
        {
            FailureText.Text = "UserName not Recognized. Try registering first.!";
        }
        else if (validationResult == -99)
        {
            FailureText.Text = "Cannot Connect to the DataBase. Contact the Site Administrator.";
        }
        else
        {
            Response.Redirect("UnAuthorized.aspx", true);
        }
    }

    private int ValidateUser(string username, string password)
    {
        try
        {
            return Common.ValidateLoginCredentials(username, password);
        }
        catch (Exception)
        {
            return -99;
        }

    }

    private string CompileUseRoles(string username)
    {
        string CombinedRoles = "Open";
        if (string.Equals(username, "sgreg6"))
        {
            CombinedRoles += ";Admin";
        }
        else
        {
            CombinedRoles += ";Regular";
        }
        return CombinedRoles;
    }
}
