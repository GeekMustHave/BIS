using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CustomUser where we can store this into session and retrieve when ever possible.
/// </summary>
public class CurrentUser
{
    public string User_GUID { get; set; } /* 40 char unique ID*/
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string[] Roles { get; set; }
    public CurrentUser()
    {
        //
        // TODO: Add constructor logic here
        //
        User_GUID = String.Empty;
        FirstName = String.Empty;
        LastName = String.Empty;
        FullName = String.Empty;
        Email = String.Empty;
        Roles = new string[0];
        HttpContext.Current.Session["svCurrentUser"] = this;
    }
    public CurrentUser(string userGuid, string fname, string lname, string email, string roles)
    {
        User_GUID = userGuid;
        FirstName = fname;
        LastName = lname;
        FullName = LastName + ", " + FirstName;
        Email = email;
        Roles = roles.Split(';');
        HttpContext.Current.Session["svCurrentUser"] = this;
    }

    public static CurrentUser GetUserDetails()
    {
        if (HttpContext.Current.Session["svCurrentUser"] == null)
        {
            return new CurrentUser();
        }
        else
        {
            return (CurrentUser)HttpContext.Current.Session["svCurrentUser"];
        }

    }
}