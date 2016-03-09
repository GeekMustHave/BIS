<%@ WebHandler Language="C#" Class="JIRADocHandler" %>

using System;
using System.IO;
using System.Web;
using JiraITClient;
using RestSharp;

public class JIRADocHandler : IHttpHandler, System.Web.SessionState.IReadOnlySessionState
{    
    public void ProcessRequest (HttpContext context) {
        string docUrl = context.Request.QueryString["ResourceURL"].ToString();
        JiraITClient.JiraClient jClient = new JiraITClient.JiraClient();        
        context.Response.Clear();
        context.Response.ClearHeaders();
        context.Response.AddHeader("Content-Disposition", "attachment;filename=" + docUrl.Substring(docUrl.LastIndexOf("/") + 1));
        context.Response.BinaryWrite(jClient.GetSingleAttachmentContent(docUrl));
    } 
    public bool IsReusable {
        get {
            return false;
        }
    }

}