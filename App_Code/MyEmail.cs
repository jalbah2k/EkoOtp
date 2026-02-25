#define ALLOW_EMAIL
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Net.Mail;

/// <summary>
/// Summary description for MyEmail
/// </summary>
public class MyEmail
{
	public MyEmail()
	{
	}

    public static void SendNotification(string subj, string body, string email, bool IsHtml)
    {
        SendNotification(subj, body, email, "", IsHtml);
    }

    public static void SendNotification(string subj, string body, string email, string att, bool IsHtml)
    {
        Dictionary<string, string> mp = GetMailer();

        string SMTP_Server = mp["SMTP_Server"];
        string SMTP_Port = mp["SMTP_Port"];
        string SMTP_UID = mp["SMTP_UID"];
        string SMTP_PWD = mp["SMTP_PWD"];
        string MAIL_FROM = mp["MAIL_FROM"];
        string MAIL_FROM_Name = mp["MAIL_FROM_Name"];

        SmtpClient smtp = new SmtpClient(SMTP_Server, Int32.Parse(SMTP_Port));
        if (Email.UseSmtpCredentials)
        {
            smtp.Credentials = new System.Net.NetworkCredential(SMTP_UID, SMTP_PWD);
            //smtp.UseDefaultCredentials = true;
        }
        smtp.EnableSsl = Email.EnableSsl;

        MailMessage message = new MailMessage();
        message.From = new MailAddress(MAIL_FROM, MAIL_FROM_Name);
        message.IsBodyHtml = true;
        message.Subject = subj;
        message.Body = body;
        message.To.Clear();

        string[] emails = email.Split(new Char[] { ';', ',' });

        foreach (string s in emails)
            message.To.Add(s);

        string cc = ConfigurationManager.AppSettings["MAIL_CC"];
        if (cc != "")
            message.CC.Add(cc);

        if ((cc = ConfigurationManager.AppSettings["MAIL_BCC"]) != "")
            message.Bcc.Add(cc);

        message.Attachments.Clear();
        message.IsBodyHtml = IsHtml;

#if ALLOW_EMAIL
            smtp.Send(message);
#endif
    }

    public static void SendNotification(string subj, string body, string email, string cc, string bcc, string att, bool IsHtml)
    {
        Dictionary<string, string> mp = GetMailer();

        string SMTP_Server = mp["SMTP_Server"];
        string SMTP_Port = mp["SMTP_Port"];
        string SMTP_UID = mp["SMTP_UID"];
        string SMTP_PWD = mp["SMTP_PWD"];
        string MAIL_FROM = mp["MAIL_FROM"];
        string MAIL_FROM_Name = mp["MAIL_FROM_Name"];

        SmtpClient smtp = new SmtpClient(SMTP_Server, Int32.Parse(SMTP_Port));
        if (Email.UseSmtpCredentials)
        {
            smtp.Credentials = new System.Net.NetworkCredential(SMTP_UID, SMTP_PWD);
            //smtp.UseDefaultCredentials = true;
        }
        smtp.EnableSsl = Email.EnableSsl;


        MailMessage message = new MailMessage();
        message.From = new MailAddress(MAIL_FROM, MAIL_FROM_Name);
        message.IsBodyHtml = true;
        message.Subject = subj;
        message.Body = body;
        message.To.Clear();

        string[] emails = email.Split(new Char[] { ';', ',' });

        foreach (string s in emails)
            message.To.Add(s);

        /*message.To.Add(new MailAddress(emails[0], emails[1]));*/

        if (cc != "")
            message.CC.Add(cc);

        if (bcc != "")
            message.Bcc.Add(bcc);

        message.Attachments.Clear();
        message.IsBodyHtml = IsHtml;

#if ALLOW_EMAIL
        smtp.Send(message);
#endif
    }

    public static Dictionary<string, string> GetMailer()
    {

        Dictionary<string, string> prms = new Dictionary<string, string>();
        prms.Add("SMTP_Server", ConfigurationManager.AppSettings["SMTP_Server"]);
        prms.Add("SMTP_Port", ConfigurationManager.AppSettings["SMTP_Port"]);
        prms.Add("SMTP_UID", ConfigurationManager.AppSettings["SMTP_UID"]);
        prms.Add("SMTP_PWD", ConfigurationManager.AppSettings["SMTP_PWD"]);
        prms.Add("MAIL_FROM", ConfigurationManager.AppSettings["MAIL_FROM"]);
        prms.Add("MAIL_FROM_Name", ConfigurationManager.AppSettings["MAIL_FROM_Name"]);
        return prms;
    }
}

public class MyTemplates
{
    public static string Registration = "<table cellpadding=\"3\" cellspacing=\"0\"  style=\"font-family:Arial; font-size:14px;\"><tr><td>Full Name:</td><td><#Fullname#></td></tr><tr><td>Email:</td><td><#Email#></td></tr><tr><td>Primary Location:</td><td><#PrimaryLocation#></td></tr><tr><td>Join newsletter:</td><td><#JoinNewsletter#></td></tr><tr><td>Are you a Health Care Provider:</td><td><#IsHealthCareProvider#></td></tr><tr><td>Area of Speciality:</td><td><#AreaSpeciality#></td></tr><tr><td>Type of practice:</td><td><#TypePractice#></td></tr></table>";

}