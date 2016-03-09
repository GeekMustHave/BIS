using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

/// <summary>
/// Created the user control as we can use the same component over all the pages where "Report" button is needed
/// </summary>
public partial class Plugins_UserControls_buttonDropDown : System.Web.UI.UserControl
{
    /// <summary>
    /// Adds the dowpdown List item for the Button Dropdown
    /// </summary>
    /// <param name="displayName">dropdown item display name</param>
    /// <param name="absoluteUrl">report page url eg:- ~/RVDReport.aspx </param>
    public void AddReportToDropDown(string displayName, string absoluteUrl)
    {
        HtmlGenericControl li = new HtmlGenericControl("li");
        li.InnerText = displayName;
        li.Attributes.Add("onclick", "GoToReportsPage('" + absoluteUrl + "');");
        ddlCustomDropDown.Controls.Add(li);        
    }
}