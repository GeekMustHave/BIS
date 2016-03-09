using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Configuration;
using System.Net.Mail;
using System.Data;
using System.IO;
using System.Reflection;

/// <summary>
/// Simple Clas to send emails. Genrelise if needed.. when more Emails has to be sent.
/// </summary>
public class Email
{
    /// <summary>
    /// Sends email from BIs Server- Uses BIsAdmin mail Account.
    /// </summary>
    /// <param name="AdminEmailTo">To Address</param>
    /// <param name="emailSubject">Email Subject</param>
    /// <param name="emailBody">Email Body</param>
    /// <returns></returns>
    public static bool SendEmailToAdmins(string AdminEmailTo, string emailSubject, string emailBody)
    {
        try
        {
            //Gets From Address from web.config
            System.Configuration.Configuration configurationFile = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~/web.config");
            MailSettingsSectionGroup mailSettings = (MailSettingsSectionGroup)configurationFile.GetSectionGroup("system.net/mailSettings");
            System.Net.NetworkCredential basicAuthInfo = new System.Net.NetworkCredential(mailSettings.Smtp.Network.UserName, mailSettings.Smtp.Network.Password);

            //*--- Create the message instance
            MailMessage mm = new MailMessage();
            string FromAddress = mailSettings.Smtp.Network.UserName;
            string FromName = "BIS Admin";
            MailAddress EmailFromAddress = new MailAddress(FromAddress, FromName);
            mm.From = EmailFromAddress;

            //*-- make sure there's an address to email to first!
            if (AdminEmailTo == null)
            {
                throw new Exception("User Email is MISSING.  Please contact admin!");
            }
            else
            {
                mm.To.Add(AdminEmailTo);
            }

            //*-- Assign the MailMessage's properties
            mm.Subject = emailSubject;
            mm.Body = emailBody;
            mm.IsBodyHtml = true;

            //*-- Create the SmtpClient object
            SmtpClient smtp = new SmtpClient();
            if (!(mailSettings.Smtp.Network.UserName == null))
            {
                // Email server specified in web.config has credentials to use
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = basicAuthInfo;
                smtp.Port = mailSettings.Smtp.Network.Port;
            }

            //*-- Send the MailMessage
            smtp.Send(mm);
            return true;
        }
        catch (Exception)
        {
            //record rhe exception in future
            return false;
        }
    }

    /// <summary>
    /// Returns a HTML Email Body. From a hard File from the system.Move to DataBase later.
    /// </summary>
    /// <param name="issueTracID"></param>
    /// <param name="issueReporter"></param>
    /// <param name="issuetype"></param>
    /// <param name="issueCreatedDate"></param>
    /// <param name="issuePriority"></param>
    /// <param name="issueDesc"></param>
    /// <param name="issueSummary"></param>
    /// <returns>string - email body content</returns>
    public static string PrepareITCreateEmailBody(string issueTracID, string issueReporter, string issuetype, string issueCreatedDate, string issuePriority
                                                  , string issueDesc, string issueSummary)
    {
        string path = HttpContext.Current.Server.MapPath("~/App_Code/ITCreateEmailBody.txt");

        string contents = File.ReadAllText(path);
        contents = contents.Replace("BIS-XXX", issueTracID);
        contents = contents.Replace("BISISSUEREPORTER", issueReporter);
        contents = contents.Replace("BISISSUETYPE", issuetype);
        contents = contents.Replace("BISISSUECREATED", issueCreatedDate);
        contents = contents.Replace("BISISSUEPRIORTY", issuePriority);
        contents = contents.Replace("BISISSUEDESC", issueDesc);
        contents = contents.Replace("BISISSUESUMMARY", issueSummary);
        return contents;
    }
}