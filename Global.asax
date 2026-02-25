<%@ Application Language="C#" %>
<%@ Import Namespace="System.Data"%>
<%@ Import Namespace="System.Data.SqlClient"%>
<%@ Import Namespace="System.Web.Routing"%>
<%@ Import Namespace="System.Security.Principal"%>
<%@ Import Namespace="System.Net"%>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="Microsoft.Web.Administration" %>
<%@ Import Namespace="System.Reflection" %>

<script runat="server">

    void Application_BeginRequest(object sender, EventArgs e)
    {
        string[] folders = { "/uploads/", "/Data/", "/Documents/", "/eFormsUploads/", "/ImageCropper/",
                     "/NewsroomFiles/", "/Photos/", "/Scripts/", "/UploaderTemp/", "/js/",
                     "/VideoFiles/", "/Fonts/", "/Libraries/" };
        foreach (string s in folders)
        {
            string restrictedFolder = s;
            if (Request.Url.AbsolutePath.StartsWith(restrictedFolder, StringComparison.OrdinalIgnoreCase))
            {
                Response.StatusCode = 403; // Forbidden
                Response.End();
                break;
            }
        }

        com.flajaxian.FileUploader.RegisterAspCookies();
    }

    protected void Application_EndRequest(Object sender, EventArgs e)
    {
        // Iterate through any cookies found in the Response object.
        foreach (string cookieName in Response.Cookies.AllKeys)
        {
            //// --- For C# 6:
            //Response.Cookies[cookieName]?.Secure = true;  

            //// --- For previous versions of C# (e.g c# 5) then:
            if (Response.Cookies[cookieName] != null)
                Response.Cookies[cookieName].Secure = true;
        }
    }

    void Application_Start(object sender, EventArgs e)
    {

        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        RouteConfig.RegisterRoutes(RouteTable.Routes);
        BundleConfig.RegisterBundles(BundleTable.Bundles);
        BundleTable.EnableOptimizations = true;

        Application["usersonline"] = 0;

        // To be able to delete folders without loosing session variables
        System.Reflection.PropertyInfo p = typeof(System.Web.HttpRuntime).GetProperty("FileChangesMonitor", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
        object o = p.GetValue(null, null);
        System.Reflection.FieldInfo f = o.GetType().GetField("_dirMonSubdirs", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.IgnoreCase);
        object monitor = f.GetValue(o);
        System.Reflection.MethodInfo m = monitor.GetType().GetMethod("StopMonitoring", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        m.Invoke(monitor, new object[] { });
    }

    void Application_End(object sender, EventArgs e)
    {

    }

    void Application_Error(object sender, EventArgs e)
    {
        Exception ex = Server.GetLastError().GetBaseException();
        //Server.ClearError();
        if (!Request.Url.ToString().ToLower().Contains("get_aspx_ver.aspx") && !Request.Url.ToString().ToLower().Contains("scriptresource.axd") && !Request.Url.ToString().ToLower().Contains("webresource.axd"))
        {

            if (ex.Message.Contains("does not exist"))
            {
                //Response.Redirect("404.htm");
                //CMSHelper.Redirect404("404.aspx");
                CMSHelper.Redirect404(CMSHelper.GetLanguagePrefix() + "error");
                //Response.Redirect("/en/error");
                return;
            }

            string AppFolder = CMSHelper.ApplicationFolder;
            string IP = Request.UserHostAddress;
            int id = 0;

            try
            {
                string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
                string commandString = " insert into LogError (Application,Page,ClientIP,UserHostName,ErrorMessage,StackTrace,UserAgent) values(@Application,@Page,@ClientIP,@UserHostName,@ErrorMessage,@StackTrace,@UserAgent) ";
                commandString += " select SCOPE_IDENTITY() ";


                using (SqlConnection connection = new SqlConnection(strConnectionString))
                {
                    SqlCommand cmd = new SqlCommand(commandString, connection);
                    cmd.Parameters.AddWithValue("@Application", AppFolder);
                    cmd.Parameters.AddWithValue("@Page", Request.Url.ToString());
                    cmd.Parameters.AddWithValue("@ClientIP", IP);
                    cmd.Parameters.AddWithValue("@UserHostName", Request.UserHostName);
                    cmd.Parameters.AddWithValue("@ErrorMessage", ex.Message);
                    cmd.Parameters.AddWithValue("@StackTrace", ex.StackTrace);
                    cmd.Parameters.AddWithValue("@UserAgent", Request.ServerVariables["HTTP_USER_AGENT"]);

                    connection.Open();
                    id = Convert.ToInt32(cmd.ExecuteScalar());
                    connection.Close();
                }
            }
            catch { }

            //try
            //{
            //    System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage(Email.FromName + "<" + Email.FromAddress + ">", "juan@bluelemonmedia.com", "New Server: " + AppFolder + " - Server Error", 
            //        ex.Message + Environment.NewLine + Environment.NewLine + ex.StackTrace
            //        + Environment.NewLine + Environment.NewLine + IP 
            //        + Environment.NewLine + Environment.NewLine + Request.Url.ToString());

            //    System.Net.Mail.SmtpClient emailClient = new System.Net.Mail.SmtpClient(Email.SmtpServer, Email.SmtpPort);
            //    if (Email.UseSmtpCredentials)
            //    {
            //        emailClient.Credentials = new System.Net.NetworkCredential(Email.SmtpUsername, Email.SmtpPassword);
            //        //emailClient.UseDefaultCredentials = true;
            //    }
            //    emailClient.EnableSsl = Email.EnableSsl;
            //    emailClient.Send(message);
            //}
            //catch { }

            #region Block IP
            //try
            //{
            //    int qty = 0, sec = 0;
            //    using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["CMServer"].ToString()))
            //    {
            //        SqlDataAdapter dapt = new SqlDataAdapter("LogErrorByIP", connection);
            //        dapt.SelectCommand.CommandType = CommandType.StoredProcedure;
            //        dapt.SelectCommand.Parameters.AddWithValue("@id", id);

            //        DataSet ds = new DataSet();
            //        dapt.Fill(ds);

            //        DataRow dr = ds.Tables[0].Rows[0];
            //        qty = Convert.ToInt32(dr["qty"]);
            //        sec = Convert.ToInt32(dr["seconds"]);
            //    }

            //    //if quantity of errors is greater than 3 then check if the error message suggests an attack or if the average timestamp is less than 3 seconds 
            //     if ((qty > 2 && (ex.Message.ToLower().Contains("dangerous request") || sec <= 10))
            // || Request.Url.ToString().Contains( "script>")
            // || Request.Url.ToString().Contains("<")
            // || Request.Url.ToString().Contains(">")
            // || Request.Url.ToString().Contains(".php")
            // || Request.Url.ToString().Contains("$_POST")
            // || Request.Url.ToString().Contains("xbshell")
            //)            
            //    {
            //        using (ServerManager serverManager = new ServerManager())
            //        {
            //            //Folder 'C:\Windows\System32\inetsrv\config' :                         User 'iis apppool\sitename
            //            Site defaultSite = serverManager.Sites[System.Configuration.ConfigurationManager.AppSettings["SiteName"]];

            //            Microsoft.Web.Administration.Configuration config = defaultSite.GetWebConfiguration();
            //            Microsoft.Web.Administration.ConfigurationSection ipSecuritySection = config.GetSection("system.webServer/security/ipSecurity");
            //            Microsoft.Web.Administration.ConfigurationElementCollection ipSecurityCollection = ipSecuritySection.GetCollection();

            //            Microsoft.Web.Administration.ConfigurationElement addElement = ipSecurityCollection.CreateElement("add");
            //            addElement["ipAddress"] = IP;
            //            addElement["allowed"] = false;
            //            try
            //            {
            //                ipSecurityCollection.Add(addElement);
            //                serverManager.CommitChanges();
            //            }
            //            catch { return;  }


            //        }

            //        System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage(Email.FromName + "<" + Email.FromAddress + ">", "juan@bluelemonmedia.com",
            //            AppFolder + " - Server Error: IP Banned",
            //            "IP:" + IP + Environment.NewLine + Environment.NewLine + ex.Message + Environment.NewLine + Environment.NewLine + ex.StackTrace);

            //        System.Net.Mail.SmtpClient emailClient = new System.Net.Mail.SmtpClient(Email.SmtpServer, Email.SmtpPort);
            //        if (Email.UseSmtpCredentials)
            //        {
            //            emailClient.Credentials = new System.Net.NetworkCredential(Email.SmtpUsername, Email.SmtpPassword);
            //            //emailClient.UseDefaultCredentials = true;
            //        }
            //        emailClient.EnableSsl = Email.EnableSsl;
            //        emailClient.Send(message);
            //    }
            //}
            //catch { }
            #endregion

        }

        CMSHelper.Redirect404(CMSHelper.GetLanguagePrefix() + "error");
    }


    void Application_AcquireRequestState(object sender, EventArgs e)
    {
        //clears login session variables if form authentication cookie expired
        if (this.Context.User.Identity.Name == string.Empty
            && !this.Context.User.Identity.IsAuthenticated)
        {
            CMSHelper.ClearLoginSession();
        }
    }

    void Session_Start(object sender, EventArgs e)
    {
        Application.Lock();
        Application["usersonline"] = (int)Application["usersonline"] + 1;
        Application.UnLock();
    }

    void Session_End(object sender, EventArgs e)
    {
        Application.Lock();
        Application["usersonline"] = (int)Application["usersonline"] - 1;
        Application.UnLock();
    }

</script>
