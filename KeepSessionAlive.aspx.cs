using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
/// <summary>
/// Description: This is a placeholder page used to just extend the session of the user.
///              Will be called from Master page evey (SessionTimeout -1) minutes, once the user is authenticated.
/// History:
/// -------------------------------------------------------------------------------
/// Date:               Name                Description
/// --------------------------------------------------------------------------------
/// 7/10/2015           JHRS                Intial Creation
/// </summary>
public partial class KeepSessionAlive : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // NOthing to Do.
    }
}