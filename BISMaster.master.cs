using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
/// <summary>
/// Description: Master Page For all the Pages in BIS
/// History:
/// -------------------------------------------------------------------------------
/// Date:               Name                Description
/// --------------------------------------------------------------------------------
/// 7/10/2015           JHRS                Intial Creation
/// </summary>
public partial class BISMaster : System.Web.UI.MasterPage
{
    protected override void OnInit(EventArgs e)
    {
        //base.OnInit(e);
        //Start pinging server every (Session TimeOut -1) minutes for continous authentaication
        //Will work only when the user keeps the browser open with BIS in it(with active Internet Connection).
        //Check IIS app pool time out if they are less than 20 min.(default for weeb config session time out).
        if (HttpContext.Current.User.Identity.IsAuthenticated && ! Page.IsPostBack)
        {
            string KeepSessionPageUrl = this.ResolveClientUrl("~/keepSessionAlive.aspx");
            Page.ClientScript.RegisterClientScriptInclude("keepSessionAlive", this.ResolveClientUrl("~/Scripts/keepSessionAlive.js"));
            Page.ClientScript.RegisterStartupScript(GetType(), "SessionKeepAlive", "SessionKeepAlive.start(" + (HttpContext.Current.Session.Timeout - 1).ToString() + ", '" + KeepSessionPageUrl + "');", true);            
        }
        if (HttpContext.Current.User.Identity.IsAuthenticated) AccountMenu.Visible = true;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        //Display the Logged in User's name when the user is autenticated
        if (HttpContext.Current.User.Identity.IsAuthenticated)
        {
            Label fullName = (Label)HeadLoginView.FindControl("lblUserFullName");
            fullName.Text = ((CurrentUser)CurrentUser.GetUserDetails()).FullName;
            if (string.IsNullOrEmpty(fullName.Text.Trim()))
            {                
                //Some thing is wrong. Redurect to login page.
                // its just that session is expired.
                //REdo the session and store the old CurrentUser object to session again.user id is found in httpcontet current user identity name
                try
                {
                    Common.PersistsCurrentUsrInfoNRetnRoles(HttpContext.Current.User.Identity.Name);
                }
                catch (Exception)
                {
                    LogoutUser();
                }                
                fullName.Text = HttpContext.Current.User.Identity.Name;                
            }
        }
    }

    /// <summary>
    /// Data Bound Menu item. If there is a image url in the site map. Add it to item imageUrl Property.    
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void NavigationMenu_MenuItemDataBound(object sender, MenuEventArgs e)
    {
        // Reference the underlying SiteMapNode object...
        SiteMapNode nodeFromSiteMap = (SiteMapNode)e.Item.DataItem;

        // If we have an imageUrl value, assign it to the Menu node's ImageUrl property
        if (nodeFromSiteMap["imageUrl"] != null)
            e.Item.ImageUrl = nodeFromSiteMap["imageUrl"];        
    }
    protected void AccountMenu_MenuItemClick(object sender, MenuEventArgs e)
    {
        if (e.Item.Value == "logout")
        {
            LogoutUser();
        }
    }

    private void LogoutUser()
    {
        //clear any other tickets that are already in the response
        Response.Cookies.Clear(); 
        FormsAuthentication.SignOut();
        FormsAuthentication.RedirectToLoginPage();
    }
}
