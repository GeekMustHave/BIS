<%@ WebHandler Language="C#" Class="GetFileHandler" %>

using System;
using System.Web;
using System.Data;
public class GetFileHandler : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        int DocId = Convert.ToInt32(context.Request.QueryString["DocId"].ToString());
        DataTable dt = Common.GetAFile(DocId);
        string fileType = dt.Rows[0]["MimeType"].ToString();
        string fileName = dt.Rows[0]["FileName"].ToString();
        string fileSize = dt.Rows[0]["FileSize"].ToString();

        context.Response.Clear();
        context.Response.ClearHeaders();
        if (fileType.Contains("pdf") || fileType.Contains("image") || fileType.Contains("text"))
        {
            context.Response.AddHeader("Content-Type", fileType);
            context.Response.AddHeader("content-length", fileSize);
            context.Response.AddHeader("Content-Disposition", "inline;filename=" + fileName);

            context.Response.BinaryWrite((byte[])dt.Rows[0]["FileData"]);
        }
        else
        {
            //context.Response.Write(" <span style=""color:blue;""> <b> ! This file cannot be previewed. </b></span> <br /> <br /> <span style=""color:blue;""> Please dowload it instead. </span>")
            context.Response.AddHeader("Content-Type", "image/png");
            context.Response.TransmitFile("~/images/NoPreview.png");
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}