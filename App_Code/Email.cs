using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Email
/// </summary>
public class Email
{
	public Email()
	{

	}

    static public string FromAddress = "noreply@bluelemonmedia.com";

    static public string FromName = System.Configuration.ConfigurationManager.AppSettings["EmailFromName"];

    static public string SmtpUsername = "noreply@bluelemonmedia.com";

	static public string SmtpPassword = "j-59+Xi2z3~TsQzTf5gM";

	static public string SmtpServer = "mail.bluelemonmedia.com";

	static public int SmtpPort = 8889;

    static public bool EnableSsl = false;
    static public bool UseSmtpCredentials = true;
}