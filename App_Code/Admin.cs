using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using System.Web.Security;

/// <summary>
/// Summary description for Admin
/// </summary>
public class Admin
{
    public static DataTable GetAllUsers()
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spUserList");
        return db.ExecuteDataSet(cmd).Tables[0];

    }
}