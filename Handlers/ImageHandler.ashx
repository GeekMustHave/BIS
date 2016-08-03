<%@ WebHandler Language="C#" Class="ImageHandler" %>

using System;
using System.Web;

public class ImageHandler : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        if (context.Request.Files.Count > 0)
        {
            HttpFileCollection files = context.Request.Files;
            if (files.Count == 1)
            {
                int objectID = Convert.ToInt32(context.Request.QueryString["ObjectPKID"].ToString());
                foreach (string key in files)
                {
                    HttpPostedFile file = files[key];
                    string fileName = file.FileName;
                    if (file.ContentType.Contains("image/"))
                    {
                        //fileName = context.Server.MapPath("~/uploads/" + fileName);
                        byte[] imgBytes = new byte[file.ContentLength];
                        file.InputStream.Read(imgBytes, 0, Convert.ToInt32(file.ContentLength));
                        Common.InsertImageToReq(objectID.ToString(), file.FileName, file.ContentType, file.ContentLength, imgBytes);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write("File uploaded successfully!");
                    }
                    else
                    {
                        context.Response.ContentType = "text/plain";
                        context.Response.Write("Only images are allowed.");
                    }

                    //file.SaveAs(fileName);
                }
            }
            else
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write(" Only Single Image is allowed.");
            }
        }

    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}