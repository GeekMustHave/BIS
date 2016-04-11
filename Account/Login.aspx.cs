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
        DoLogin(UserName.Text.Trim(),Password.Text);
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

    private void DoLogin(string username, string password)
    {

        string usrName = username;
        int validationResult = ValidateUser(usrName, password);
        if (validationResult == 1)
        {
            //clear any other tickets that are already in the response
            Response.Cookies.Clear(); 
            // Get the User Info and store it in session.
            string usrRoles = string.Empty;

            usrRoles = Common.PersistsCurrentUsrInfoNRetnRoles(usrName);
            usrRoles = (string.IsNullOrEmpty(usrRoles)) ? "Open" : usrRoles; //CompileUseRoles(usrName);

            FormsAuthenticationTicket tkt;
            string cookiestr;
            HttpCookie ck;
            tkt = new FormsAuthenticationTicket(2, usrName, DateTime.Now, DateTime.Now.AddDays(365),
                                                RememberMe.Checked, usrRoles, FormsAuthentication.FormsCookiePath);

            cookiestr = FormsAuthentication.Encrypt(tkt);
            ck = new HttpCookie(FormsAuthentication.FormsCookieName, cookiestr);
            if (RememberMe.Checked)
                ck.Expires = tkt.Expiration;
            else
                ck.Expires = DateTime.Now.AddMinutes(HttpContext.Current.Session.Timeout);
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
}
