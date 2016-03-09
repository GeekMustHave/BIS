using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UnAuthorized : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            lblUSerDetails.Text = "Yo.. '" + ((CurrentUser)CurrentUser.GetUserDetails()).FullName + "'";
        }
        
    }
}