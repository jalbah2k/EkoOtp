//#define MULTI_LANGUAGE
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Web.Services;
using System.Threading;
using System.Web.Routing;
using System.Web.Security;
using System.Text.RegularExpressions;


public partial class _Default : System.Web.UI.Page
{
    [WebMethod(true)]
    public static long GetDirectorySize(string path)
    {
        long size = CMSHelper.getDirectorySize(HttpContext.Current.Request.MapPath(path));
        return size;

        //string dir = HttpContext.Current.Request.MapPath(path);
        //if (!Directory.Exists(dir))
        //{
        //    //Directory.CreateDirectory(dir);
        //    return -1;
        //}
        //long size = CMSHelper.getDirectorySize(dir);
        //return size;
    }

    //[WebMethod]
    //public static string ValidatePassword(string Password)
    //{
    //    string encryptedPassw = EncDec.DESEncrypt(Password);
    //    string result = PasswordRules.Validate(encryptedPassw);

    //    if (result.IndexOf("\"") == 0)
    //        result = result.Substring(1);

    //    if (result.LastIndexOf("\"") == result.Length - 1)
    //        result = result.Substring(0, result.Length - 1);

    //    return result;
    //}

    #region WidgetToolbar

    [WebMethod(true)]
    public static string BannerGalleryNew()
    {
        string f = "temp-" + Guid.NewGuid().ToString();
        string tempDir = HttpContext.Current.Request.MapPath(string.Format("~/uploads/banners/{0}/", f));

        try
        {
            if (Directory.Exists(tempDir))
                Directory.Delete(tempDir, true);
        }
        catch { }
        Directory.CreateDirectory(tempDir);

        return f;
    }

    [WebMethod(true)]
    public static string PhotoGalleryNew()
    {
        string f = "temp-" + Guid.NewGuid().ToString();
        string tempDir = HttpContext.Current.Request.MapPath(string.Format("~/Photos/{0}/", f));

        try
        {
            if (Directory.Exists(tempDir))
                Directory.Delete(tempDir, true);
        }
        catch { }
        Directory.CreateDirectory(tempDir + "thumbs\\");

        return f;
    }

    [WebMethod(true)]
    public static string DocumentsNew()
    {
        string f = "temp-" + Guid.NewGuid().ToString();
        string tempDir = HttpContext.Current.Request.MapPath(string.Format("~/Data/Documents/{0}/", f));

        try
        {
            if (Directory.Exists(tempDir))
                Directory.Delete(tempDir, true);
        }
        catch { }
        Directory.CreateDirectory(tempDir);

        return f;
    }

    [WebMethod(true)]
    public static string AddWidgetToPage(string PageId, string ContentId, string ZoneId, int Priority)
    //public static string AddWidgetToPage(string PageId, string ContentId, int Priority)
    {
        //string retVal = "Widget has been added successfully.";
        string retVal = "";

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]))
        {
            SqlCommand cmd = new SqlCommand("WidgetToolbar_Ins", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Page_ID", PageId);
            cmd.Parameters.AddWithValue("@Content_ID", ContentId);
            cmd.Parameters.AddWithValue("@Zone_ID", ZoneId);
            cmd.Parameters.AddWithValue("@Priority", Priority);
            cmd.Parameters.AddWithValue("@userid", HttpContext.Current.Session["LoggedInID"]);
            cmd.Parameters.AddWithValue("@lang", CMSHelper.GetLanguageNumber());

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (SqlException ex)
            {
                retVal = ex.Message;
            }
        }

        return retVal;
    }

    [WebMethod(true)]
    public static string RemoveWidgetFromPage(string PageId, string ZoneId, string Control, string WidgetId)
    //public static string RemoveWidgetFromPage(string PageId, string Control, string WidgetId)
    {
        //string retVal = "Widget has been remove successfully.";
        string retVal = "";

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]))
        {
            //string sqlstr = " declare @Zone_ID int declare @Content_ID int declare @id int declare @Priority int ";
            string sqlstr = " declare @Content_ID int declare @id int declare @Priority int ";
            //sqlstr += " select @Zone_ID = id from Zones where name='Content' ";
            sqlstr += " select @Content_ID = id from Content where control=@Control and param=@WidgetId ";
            sqlstr += " select @id = id, @Priority = Priority from Pages_Content_Zone where Page_ID = @Page_ID and Zone_ID = @Zone_ID and Content_ID = @Content_ID ";
            //sqlstr += " delete from Pages_Content_Zone where Page_ID = @Page_ID and Zone_ID = @Zone_ID and Content_ID = @Content_ID ";
            sqlstr += " delete from Pages_Content_Zone where id = @id ";
            sqlstr += " update Pages_Content_Zone set Priority = Priority - 1 where Page_ID = @Page_ID and Zone_ID = @Zone_ID and Priority > @Priority ";
            SqlCommand cmd = new SqlCommand(sqlstr, conn);
            cmd.Parameters.AddWithValue("@Page_ID", PageId);
            cmd.Parameters.AddWithValue("@Zone_ID", ZoneId);
            cmd.Parameters.AddWithValue("@Control", Control);
            cmd.Parameters.AddWithValue("@WidgetId", WidgetId);

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (SqlException ex)
            {
                retVal = ex.Message;
            }
        }

        return retVal;
    }

    [WebMethod(true)]
    //public static string ReorderWidgetsInPage(string PageId, string Reorder)
    public static string ReorderWidgetsInPage(string PageId, string ZoneId, string Reorder)
    {
        string retVal = "";

        string[] ListID = Reorder.Split(',');

        //string sqlstr = " declare @Zone_ID int ";
        string sqlstr = "";
        //sqlstr += " select @Zone_ID = id from Zones where name='Content' ";
        /*sqlstr += " declare @TimedContent bit = 1 ";
        sqlstr += " IF COL_LENGTH('Pages_Content_Zone','StartDate') IS NULL ";
        sqlstr += " BEGIN ";
        sqlstr += "     SET @TimedContent = 0 ";
        sqlstr += " END ";*/
        sqlstr += " declare @LinkId int = 0 ";

        for (int i = 0; i < ListID.Length; i++)
        {
            if (ListID[i] != "" && ListID[i] != null)
            {
                string[] item = ListID[i].Split('_');
                //sqlstr += " update Pages_Content_Zone set priority=" + (i + 1).ToString() + " where Page_ID = @Page_ID and Zone_ID = @Zone_ID and Content_ID = (select id from Content where control = '" + item[0] + "' and param = '" + item[1] + "') ";
                sqlstr += " select @LinkId = LinkId from Pages_Content_Zone where Page_ID = @Page_ID and Zone_ID = @Zone_ID and Content_ID = (select id from Content where control = '" + item[0] + "' and param = '" + item[1] + "') ";
                sqlstr += " update Pages_Content_Zone set priority=" + (i + 1).ToString() + " where LinkId = @LinkId ";
            }
        }

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]))
        {
            SqlCommand cmd = new SqlCommand(sqlstr, conn);
            cmd.Parameters.AddWithValue("@Page_ID", PageId);
            cmd.Parameters.AddWithValue("@Zone_ID", ZoneId);

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (SqlException ex)
            {
                retVal = ex.Message;
            }
        }

        return retVal;
    }

    #endregion WidgetToolbar

    #region Timed Content

    [WebMethod(true)]
    public static string GetTimedContentList(string PageID)
    {
        string cl = string.Empty;

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("CMServer")))
        {
            SqlCommand cmd = new SqlCommand("select dbo.fGetTimedContentList(@PageID) as ContentList", conn);
            cmd.Parameters.AddWithValue("@PageID", PageID);

            conn.Open();
            cl = cmd.ExecuteScalar().ToString();
            conn.Close();
        }

        return cl;
    }

    #endregion Timed Content

    protected override void OnPreRender(EventArgs e)
    {       
        // CultureInfo ci = CultureInfo.InvariantCulture;
        //Response.Write((DateTime.Parse(DateTime.Now.Subtract(((DateTime)HttpContext.Current.Items["startTime"])).ToString())).ToString("ss.fff",ci) + "s<br />");
        base.OnPreRender(e);
    }

    //protected override void Render(HtmlTextWriter writer)
    //{

    //    if (!Response.IsClientConnected)
    //    {
    //        writer = null;
    //        return;
    //    }
    //    writer.InnerWriter.Close();
    //    StringWriter sw = new StringWriter();
    //    writer.InnerWriter = sw;
    //    base.Render(writer);
    //    writer.Close();
    //    //To make Recaptcha WCAG compliant
    //    //Response.Write(sw.ToString().Replace("<textarea name=\"recaptcha_challenge_field\"", "<textarea name=\"recaptcha_challenge_field\" id=\"recaptcha_challenge_field\""));

    //    //Run replacements
    //    string htmlContent = sw.ToString()
    //                            .Replace("<textarea name=\"recaptcha_challenge_field\"", "<textarea name=\"recaptcha_challenge_field\" id=\"recaptcha_challenge_field\"", StringComparison.CurrentCultureIgnoreCase)
    //                            .Replace("<input name=\"ctl00$PhotoGalleryAdd1$ImagesUploader$File\"", "<input name=\"ctl00$PhotoGalleryAdd1$ImagesUploader$File\" id=\"ctl00_PhotoGalleryAdd1_ImagesUploader_File\" title=\"Upload Photos\"", StringComparison.CurrentCultureIgnoreCase);
    //    //.Replace("<script type='text/javascript' language='JavaScript' src='/asb_includes/AutoSuggestBox.js'>", "<script defer='defer' type='text/javascript' language='JavaScript' src='/asb_includes/AutoSuggestBox.js'>", StringComparison.CurrentCultureIgnoreCase)
    //    //.Replace("<script src=\"/js/webkit.js\" type=\"text/javascript\">", "<script defer=\"defer\" src=\"/js/webkit.js\" type=\"text/javascript\">", StringComparison.CurrentCultureIgnoreCase)

    //    try
    //    {
    //        if (CMSHelper.isMultilingual && _language == "fr")
    //        {
    //            MatchCollection matches = Regex.Matches(htmlContent, @"</?html\s+[^<>]*>", RegexOptions.IgnoreCase);
    //            foreach (Match m in matches)
    //            {
    //                htmlContent = htmlContent.Replace(m.Value, m.Value.Replace("lang=\"en\"", "lang=\"fr\"", StringComparison.CurrentCultureIgnoreCase), StringComparison.CurrentCultureIgnoreCase);
    //            }
    //        }
    //    }
    //    catch { }


    //    //Output replacements
    //    Response.Write(htmlContent);

    //    //StringBuilder pageSource = new StringBuilder();
    //    //StringWriter sw = new StringWriter(pageSource);
    //    //HtmlTextWriter htmlWriter = new HtmlTextWriter(sw);
    //    //base.Render(htmlWriter);

    //    ////Run replacements
    //    //pageSource.Replace("<textarea name=\"recaptcha_challenge_field\"", "<textarea name=\"recaptcha_challenge_field\" id=\"recaptcha_challenge_field\"");
    //    //pageSource.Replace("<input name=\"ctl00$PhotoGalleryAdd1$ImagesUploader$File\"", "<input name=\"ctl00$PhotoGalleryAdd1$ImagesUploader$File\" id=\"ctl00_PhotoGalleryAdd1_ImagesUploader_File\" title=\"Upload Photos\"");
    //    //if (CMSHelper.isMultilingual && _language == "fr")
    //    //{
    //    //    //pageSource.Replace("lang=\"en\"", "lang=\"fr\"");
    //    //    MatchCollection matches = Regex.Matches(pageSource.ToString(), @"</?html\s+[^<>]*>", RegexOptions.IgnoreCase);
    //    //    foreach (Match m in matches)
    //    //    {
    //    //        //pageSource.Append(m.Value);
    //    //        pageSource.Replace(m.Value, m.Value.Replace("lang=\"en\"", "lang=\"fr\"", StringComparison.CurrentCultureIgnoreCase));
    //    //    }
    //    //}

    //    ////Output replacements
    //    //writer.Write(pageSource.ToString());
    //}

    private string _lang;
    public string _language { set; get; }
    public string _seo { set; get; }
    public string _id { set; get; }
    public string _topmenuid { set; get; }
    public _Default()
    {
        _lang = "1";
        _language = "en";
        _seo = "";
        _id = "";
        _topmenuid = "1";
    }

    protected override void OnPreInit(EventArgs e)
    {

        try
        {
            _seo = RouteData.Values["seo"].ToString().ToLower();
        }
        catch
        {
            if (Request.QueryString["seo"] != null)
                _seo = Request.QueryString["seo"];
            else
                _seo = "/error";
        }


        if (_seo == "error")
            Response.Redirect("/error.aspx");
			
		UrlParameterWhitelistValidator validator = new UrlParameterWhitelistValidator();
        var filteredQueryString = validator.ValidateAndFilter(this, "/" + _seo);	

        if (!IsPostBack && Session["Redirected"] == null)
        {
            string originalUrl = HttpUtility.UrlDecode(Request.Url.AbsoluteUri);

            //Perform URL sanitization here
            string sanitizedUrl = HttpUtility.UrlDecode(QueryStringHelper.SanitizeAndModifyUrl(originalUrl));

            if (!string.Equals(originalUrl, sanitizedUrl, StringComparison.OrdinalIgnoreCase))
            {
                Session["Redirected"] = true;
                Response.Redirect(sanitizedUrl, false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }
        else
        {
            Session["Redirected"] = null; // Reset after successful load
        }


        // check if user is still authenticated but session variable is expired
        CMSHelper.AutoLogin();

        //Response.Write("<br /><br /><br /><br /><br />User: " + User.Identity.Name);
        //Response.Write("<br />User ID: " + Session["LoggedInID"]);

        //////string lang = CMSHelper.GetCleanQueryString("lang", "1");

        //////string seo = CMSHelper.GetSeo();

#if MULTI_LANGUAGE
        if (RouteData.Values["lang"] != null)
        {
            try
            {
                _language = RouteData.Values["lang"].ToString().ToLower();
            }
            catch { }
        }
#else
        if (Request.Url.LocalPath.Contains("/fr/"))
            _language = "fr";
#endif
        if (Request.Url.LocalPath.ToLower().Contains("/en/memberdirectory"))
            Response.Redirect("/memberdirectory");


        if (RouteData.Values["id"] != null)
        {
            try { _id = RouteData.Values["id"].ToString().ToLower(); }
            catch { }
        }


        SqlDataAdapter cmserver;
        DataSet ds = new DataSet();

        cmserver = new SqlDataAdapter("BASE_PageFromSeo_New", ConfigurationManager.AppSettings["CMServer"]);
        cmserver.SelectCommand.CommandType = CommandType.StoredProcedure;
        cmserver.SelectCommand.Parameters.AddWithValue("@seo", _seo);
        cmserver.SelectCommand.Parameters.AddWithValue("@language", _language);

        bool autoLogin = false;
        if(Session["MemberId"] == null && Request.QueryString["zd"] != null)
        {
            autoLogin = true;
            string sessionid = Request.QueryString["zd"].Replace(" ", "+");
            sessionid = EncryptionHelper.Decrypt(sessionid);
            cmserver.SelectCommand.Parameters.AddWithValue("@session", sessionid);
        }
        else if ((Session["MemberId"] != null || Session["LoggedInID"] != null) && Request.QueryString["p"] != null && Request.QueryString["p"] == "0")
        {
            SelfLogout();

            Response.Redirect(Request.Url.AbsolutePath);
        }
        else if (Session["MemberId"] == null && Session["LoggedInID"] != null)
        {
            DataTable dtu = new DataTable();

            SqlDataAdapter da = new SqlDataAdapter("select yaf_userid from users where id=@id", ConfigurationManager.AppSettings["CMServer"]);
            da.SelectCommand.CommandType = CommandType.Text;
            da.SelectCommand.Parameters.AddWithValue("@id", Session["LoggedInID"].ToString());
            da.Fill(dtu);
            if(dtu.Rows.Count > 0)
                Session["MemberID"] = dtu.Rows[0]["yaf_userid"].ToString();
        }
        cmserver.Fill(ds);

        try { Session["Multilingual"] = (bool)ds.Tables[2].Rows[0]["isMultilingual"]; }
        catch { Session["Multilingual"] = false; }

        if (ds.Tables[0].Rows.Count > 0) // else there was no page found
        {
            if (autoLogin && ds.Tables.Count > 4)
            {
                SelfLogin(ds.Tables[4]);

                if (Request.QueryString["g"] != null)
                {
                    string g = Request.QueryString["g"];
                    string[] gs = g.Split(new char[] { ',' });
                    foreach (string s in gs)
                    {
                        if (s.ToLower() == "account_login")
                            continue;

                        g = s;
                    }

                    if (Request.QueryString["ap"] != null && Request.QueryString["ap"].ToLower() == "cms")
                    {
                        #region Redirect to CMS page
                        string newurl = "/" + g;
                        string param = Request.QueryString["t"] != null ? Request.QueryString["t"] : "";

                        #region QueryString
                        string qstring = "";
                        string[] qs = Request.QueryString.ToString().Split(new char[] { '&' });
                        foreach (string s in qs)
                        {
                            if (s.ToLower().Contains("g=") ||
                                s.ToLower().Contains("t=") ||
                                //  s.ToLower().Contains("q=") ||
                                s.ToLower().Contains("zd=") ||
                                s.ToLower().Contains("ap=")
                                )
                                continue;

                            string p = s;
                            if (s.ToLower().Contains("q="))
                                p = Request.QueryString["q"];

                            if (qstring != "")
                                qstring += "&" + p;
                            else
                                qstring = p;
                        }
                        #endregion

                        if (g != "admin")
                        {
                            if (param != "")
                                newurl += "/" + param;
                        }
                        else
                        {
                            newurl = "/admin.aspx";
                        }


                        if (qstring != "")
                            newurl += "?" + qstring;

                        Response.Redirect(newurl);
                        #endregion
                    }
                    else
                    {
                        #region Redirect to Forum

                        string newurl = g.Replace("_", "/").Replace("$", "&");

                        Response.Redirect(newurl);
                        #endregion
                    }
                }
            }


            if (_seo == ConfigurationManager.AppSettings["EKOMembersSeo"].ToLower() && Session["LoggedInID"] != null && Session["LoggedInID"].ToString() == "2114") //username: CISProject
            {
                Response.Redirect("/CISProject");
            }

            if (_seo == ConfigurationManager.AppSettings["EKOMembersSeo"].ToLower() && Session["MemberID"] != null)
            {
                DataTable dtm = new DataTable();

                SqlDataAdapter dap = new SqlDataAdapter("select User_id from Users_Groups_Access where Group_id = @groupid and User_id=@id", ConfigurationManager.AppSettings["CMServer"]);
                dap.SelectCommand.CommandType = CommandType.Text;
                dap.SelectCommand.Parameters.AddWithValue("@id", Session["LoggedInID"].ToString());
                dap.SelectCommand.Parameters.AddWithValue("@groupid", (int)Groups.EKO_PNCA);      //(int)Groups.EKOMembers);
                dap.Fill(dtm);
                if(dtm.Rows.Count == 0)
                    Response.Redirect (ConfigurationManager.AppSettings["BuildingCapacityPage"]);

            }

            PageFound(ds);
            SetCulture(ds);
        }
        else
        {
            //if (Request.QueryString["pid"] != null && HttpContext.Current.Request.Url.AbsoluteUri.ToLower().Contains("default.aspx"))
            //{
            //    cmserver = new SqlDataAdapter("BASE_PageFromID", ConfigurationManager.AppSettings["CMServer"]);
            //    cmserver.SelectCommand.CommandType = CommandType.StoredProcedure;
            //    cmserver.SelectCommand.Parameters.AddWithValue("@id", CMSHelper.GetCleanQueryString("pid", "1"));

            //    if (ds.Tables[0].Rows.Count > 0) // else there was no page found
            //    {
            //        PageFound(ds);
            //        SetCulture(ds);
            //    }
            //    else
            //    {
            //        Go404();
            //        return;
            //    }
            //}
            //else
            {
                //Then search in Redirect table
                string param;
                if ((CMSHelper.isMultilingual || CMSHelper.AlwaysWithEN)
                    && RouteData.Values["lang"] != null
                    && RouteData.Values["lang"].ToString() != "en"
                    && RouteData.Values["lang"].ToString() != "fr"
                    )
                    param = RouteData.Values["lang"].ToString();              //First parameter
                else
                    param = RouteData.Values["seo"].ToString();

                cmserver = new SqlDataAdapter("select top 1 newseo from redirect where oldseo = @oldseo", ConfigurationManager.AppSettings["CMServer"]);
                cmserver.SelectCommand.CommandType = CommandType.Text;
                cmserver.SelectCommand.Parameters.AddWithValue("@oldseo", param);
                DataSet red = new DataSet();

                cmserver.Fill(red);
                if (red.Tables[0].Rows.Count > 0)
                {
                    if (red.Tables[0].Rows[0][0].ToString() != "")
                    {
                        Response.Redirect(red.Tables[0].Rows[0][0].ToString().Trim());
                        return;
                    }
                    else
                    {
                        Go404();
                        return;
                    }
                }
                else
                {
                    Go404();
                    return;
                }
            }
        }

        base.OnPreInit(e);
        
    }

    private void SelfLogout()
    {
        CMSHelper.ClearLoginSession();

        FormsAuthentication.SignOut();
        Session.Abandon();

        // clear authentication cookie
        HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
        cookie1.Expires = DateTime.Now.AddYears(-1);
        Response.Cookies.Add(cookie1);

        // clear session cookie (not necessary for your current problem but i would recommend you do it anyway)
        System.Web.Configuration.SessionStateSection sessionStateSection = (System.Web.Configuration.SessionStateSection)System.Web.Configuration.WebConfigurationManager.GetSection("system.web/sessionState");
        HttpCookie cookie2 = new HttpCookie(sessionStateSection.CookieName, "");
        cookie2.Expires = DateTime.Now.AddYears(-1);
        Response.Cookies.Add(cookie2);
    }

    private void SelfLogin (DataTable dbUsers)
    {
        if (dbUsers.Rows.Count ==  1)
        {
            DataRow dr = dbUsers.Rows[0];
            string username = dr["username"].ToString();

            string roles = string.Join(",", Roles.GetRolesForUser(username));
            roles = roles.Replace("Forum: ", "");       // "F:");



            FormsAuthenticationTicket tkt =
               new FormsAuthenticationTicket(
                    2,                                  // version
                    username,                           
                    DateTime.Now,                       // issue time is now
                    DateTime.Now.AddMinutes(30),        // expires in 60 minutes
                    false,          //cbRem.Checked,                      // is cookie persistent?
                    roles                               // role assignment is stored in userData
                    );
            string cookiestr = FormsAuthentication.Encrypt(tkt);
            HttpCookie ck = new HttpCookie(FormsAuthentication.FormsCookieName, cookiestr);
            ck.Path = FormsAuthentication.FormsCookiePath;
            Response.Cookies.Add(ck);


            Session["LoggedInID"] = dr["id"].ToString();
            Session["MemberID"] = dr["yaf_userid"].ToString();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("CMServer")))
            {
                // try
                {
                    SqlCommand cmd = new SqlCommand(@"update users set SessionId=null where yaf_userid=@id
                                                      update eko.Members set EkoWalkStatus=1 where yaf_userid=@id and EkoWalkStatus=2
                                                      update eko.Members set EkoWalkStep='' where yaf_userid=@id ", conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@id", dr["yaf_userid"].ToString());
                    cmd.Connection.Open();
                    string ret = Convert.ToString(cmd.ExecuteScalar());
                    cmd.Connection.Close();
                }
                //  catch { }
            }

            if (username == "Member: EKOSS2025" && _seo != "ekoss2025-ondemand")
                Response.Redirect("/ekoss2025-ondemand");
        }
        //else
        //{
        //    Session["LoggedInID"] = null;
        //   // Session["MemberID"] = null;

        //}
    }

    private void Go404()
    {
        if (CMSHelper.isMultilingual || CMSHelper.AlwaysWithEN)
            CMSHelper.Redirect404("/" + _language + "/error");
        else
            CMSHelper.Redirect404("/error");
    }

    private void SetCulture(DataSet ds)
    {
        string UserCulture = "en-US";
        if (ds.Tables[3].Rows.Count > 0)
        {
            UserCulture = ds.Tables[3].Rows[0]["locale"].ToString();
        }

        Thread.CurrentThread.CurrentUICulture = new CultureInfo(UserCulture);
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(UserCulture);

    }
   
    enum Groups { Common = 1, EKOMembers, EKO_PNCA=82 }
    private void PageFound(DataSet ds)
    {
        ContentPlaceHolder placeHolder;
        Control control;

        string seo = ds.Tables[0].Rows[0]["seo"].ToString();
        Session["Language"] = _lang = ds.Tables[0].Rows[0]["language"].ToString();
        //try { Session["Multilingual"] = (bool)ds.Tables[2].Rows[0]["isMultilingual"]; }
        //catch { Session["Multilingual"] = false; }

        //if (Session["MemberID"] != null)
        //    Response.Write(Session["MemberID"].ToString());

        if (Session["LoggedInID"] != null)
        {
            //Response.Write("<br>" + Session["LoggedInID"].ToString());
            if ((ds.Tables[0].Rows[0]["active"].ToString() == "0" || ds.Tables[0].Rows[0]["active"].ToString() == "False") && Permissions.Get(Convert.ToInt32(Session["LoggedInID"].ToString()), Convert.ToInt32(ds.Tables[0].Rows[0]["id"].ToString())) < 2)
            {
                Response.Redirect("/");
                return;
            }
        }
        else
        {
            if (ds.Tables[0].Rows[0]["active"].ToString() == "0" || ds.Tables[0].Rows[0]["active"].ToString() == "False")
            {
                Response.Redirect("/");
                return;
            }
        }

        if (ds.Tables[0].Rows[0]["private"].ToString() == "1" || ds.Tables[0].Rows[0]["private"].ToString() == "True")
        {
            string group = ds.Tables[0].Rows[0]["Group_id"].ToString();
            if (Session["LoggedInID"] != null)
            {
                if (Permissions.Get(Convert.ToInt32(Session["LoggedInID"].ToString()), Convert.ToInt32(ds.Tables[0].Rows[0]["id"].ToString())) < 1)
                {
                    if (group == ((int)Groups.EKOMembers).ToString() || group == ConfigurationManager.AppSettings["BuildingCapacityGroup"]
                        || group == ((int)Groups.EKO_PNCA).ToString())
                        Response.Redirect("/Membership/Account/Login" );
                    else
                        Response.Redirect("/login?r=" + seo);      //// + "&l=" + _lang);
                    return;
                }
            }
            else
            {
                if (group == ((int)Groups.EKOMembers).ToString() || group == ConfigurationManager.AppSettings["BuildingCapacityGroup"]
                     || group == ((int)Groups.EKO_PNCA).ToString())
                {
                    Response.Redirect(String.Format("/Membership/Account/Login?ap=cms&g={0}&t={1}{2}", _seo, _id,
                        !String.IsNullOrEmpty(Request.QueryString.ToString()) ? "&q=" + Request.QueryString.ToString() : ""));
                }
                else
                {
                    //Response.Redirect("/login?r=" + seo);              //// + "&l=" + _lang);
                    Response.Redirect(String.Format("/Membership/Account/Login?ap=cms&g={0}&t={1}{2}", _seo, _id, 
                        !String.IsNullOrEmpty(Request.QueryString.ToString()) ? "&q=" + Request.QueryString.ToString() : ""));
                }
                return;
            }
        }

        Session.Add("PageID", ds.Tables[0].Rows[0]["id"].ToString());
        Session.Add("PageId", ds.Tables[0].Rows[0]["id"].ToString());
        Session["PageID"] = ds.Tables[0].Rows[0]["id"].ToString();
        Session["PageId"] = ds.Tables[0].Rows[0]["id"].ToString();

        MasterPageFile = "~/" + ds.Tables[0].Rows[0]["name"].ToString() + ".master";
        string prefix = "";
        if (_lang == "1")
        {
            prefix = ConfigurationManager.AppSettings["SiteTitle"];
        }
        else
        {
            prefix = ConfigurationManager.AppSettings["SiteTitleFR"];
        }
        this.Title = prefix + " - " + ds.Tables[0].Rows[0]["title"].ToString();

        string hostUrl = string.Format("{0}://{1}", Request.Url.Scheme, Request.Url.Host);

        placeHolder = (ContentPlaceHolder)Master.FindControl("head");
        StringBuilder metas = new StringBuilder();
        // BLM metas
        metas.Append("<meta name=\"keywords\" content=\""); metas.Append(ds.Tables[0].Rows[0]["keywords"].ToString()); metas.Append("\"/>");
        //metas.Append("<meta name=\"description\" content=\""); metas.Append(ds.Tables[0].Rows[0]["description"].ToString()); metas.Append("\"/>");
        metas.Append("<meta name=\"copyright\" content=\"Blue Lemon Media Inc.\"/>");
        metas.Append("<meta name=\"robots\" content=\"noindex\"/>");
        // Open Graph metas
        metas.Append("\n\t<!-- Open Graph Metas\n\t–––––––––––––––––––––––––––––––––––––––––––––––––– -->");
       // metas.Append("\n\t<meta property=\"og:title\" content=\""); metas.Append(this.Title); metas.Append("\"/>");
        metas.Append("\n\t<meta property=\"og:type\" content=\"website\"/>");


        string mytitle = this.Title;
        string mydescrip = ds.Tables[0].Rows[0]["description"].ToString();
        string myurl = hostUrl + "/" + seo;
        string myqs = "";
        string myimg = "/favicon.ico";

        if (Request.ServerVariables["QUERY_STRING"] != "")
        {
            myqs = "?" + Request.ServerVariables["QUERY_STRING"];

            if ((seo == "calendar" || seo == "events") && Request.QueryString["eventid"] != null)
            {
                int id = 0;
                if (int.TryParse(Request.QueryString["eventid"], out id))
                {
                    cEvents mEvents = new cEvents();
                    DataTable mdt = new DataTable();
                    mdt = mEvents.mGet_One(id);

                    if (mdt.Rows.Count > 0)
                    {
                        DataRow drn = mdt.Rows[0];
                        mytitle = drn["Event_Name"].ToString();

                        if (!String.IsNullOrEmpty(drn["MIMEType"].ToString()))
                            myimg = String.Format("/Controls/CalendarNew/ThumbNail.ashx?PictureID={0}&maxsz=300", drn["Event_id"].ToString());
                        else
                        {
                            string stemp = GetImageSrc(drn["Event_Desc"].ToString(), "\"");
                            if (stemp == "")
                                stemp = GetImageSrc(drn["Event_Desc"].ToString(), "'");

                            // Response.Write(stemp);
                            if (stemp != "")
                                myimg = stemp.Replace(" ", "%20");
                        }
                    }
                }
            }

            else if (seo == "newsroom" && Request.QueryString["newsid"] != null)
            {
                DataTable ndt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter("NewsRoomSelect_New", ConfigurationManager.AppSettings["CMServer"]);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                int id = 0;
                if (int.TryParse(Request.QueryString["newsid"], out id))
                {
                    da.SelectCommand.Parameters.AddWithValue("@id", id);
                    da.Fill(ndt);

                    if (ndt.Rows.Count > 0)
                    {
                        DataRow drn = ndt.Rows[0];
                        mytitle = drn["Title"].ToString();

                        if (!String.IsNullOrEmpty(drn["MIMEType"].ToString()))
                            myimg = String.Format("/Controls/Newsroom/ThumbNail.ashx?PictureID={0}&maxsz=300", drn["id"].ToString());
                        else
                        {
                            string stemp = GetImageSrc(drn["Details"].ToString(), "\"");
                            if (stemp == "")
                                stemp = GetImageSrc(drn["Details"].ToString(), "'");

                            // Response.Write(stemp);
                            if (stemp != "")
                                myimg = stemp.Replace(" ", "%20");
                        }
                    }
                }
            }            
        }

        if( seo != "calendar" 
            && seo != "newsroom")
        {
            DataTable hdt = new DataTable();
            ////string mysql = @"select html from html where id = (select top 1 [param] from Content where control='content' and id in (select top 1 content_id from Pages_Content_Zone where zone_id = 1 and page_id in (select top 1 id from pages where seo=@seo)))";
            string mysql = @"select image from pages where seo=@seo";
            SqlDataAdapter da = new SqlDataAdapter(mysql, ConfigurationManager.AppSettings["CMServer"]);
            da.SelectCommand.CommandType = CommandType.Text;
            da.SelectCommand.Parameters.AddWithValue("@seo", seo);
            da.Fill(hdt);

            if (hdt.Rows.Count > 0)
            {
                ////DataRow drh = hdt.Rows[0];

                ////string stemp = GetImageSrc(drh["html"].ToString(), "\"");
                ////if (stemp == "")
                ////    stemp = GetImageSrc(drh["html"].ToString(), "'");

                //if (stemp != "")
                //    myimg = stemp.Replace(" ", "%20");

                if (hdt.Rows[0]["image"].ToString() != "")
                    myimg = hdt.Rows[0]["image"].ToString();
                //Response.Write(myimg);

            }
        }

        if (myimg.Contains("https://") || myimg.Contains("https://"))
            hostUrl = "";

        if (seo == "memberdirectory" && Page.RouteData.Values["id"].ToString() != "")
        {
            DataTable dt = GetMemberDirectory(Page.RouteData.Values["id"].ToString());
            if (dt.Rows.Count > 0)
            {
                this.Title = mytitle = prefix + " - " + dt.Rows[0]["name"].ToString();
                string descrip = dt.Rows[0]["aboutus"].ToString();
                int max = 160;
                if (descrip.Length > max)
                    descrip = descrip.Remove(max) + "...";

                mydescrip = descrip;
            }
        }


        WriteMetas(metas, mytitle, mydescrip, myurl + myqs, hostUrl + myimg);


        //Canonical Link
        metas.Append("\n\t<!-- Canonical Link\n\t–––––––––––––––––––––––––––––––––––––––––––––––––– -->");
        metas.Append("\n\t<link rel=\"canonical\" href=\""); metas.Append(hostUrl + (_lang == "1" && seo == "home" ? "" : CMSHelper.GetSeoWithLanguagePrefix())); metas.Append("\">");

        control = new LiteralControl(metas.ToString());
        placeHolder.Controls.Add(control);


        // Structured Data
        StringBuilder StructuredData = new StringBuilder();
        StructuredData.Append("\n\t<!-- Structured Data\n\t–––––––––––––––––––––––––––––––––––––––––––––––––– -->");
        StructuredData.Append("\n\t<script type=\"application/ld+json\">");
        StructuredData.Append("\n\t{");
        StructuredData.Append("\n\t\t\"@context\": \"https://schema.org\",");
        StructuredData.Append("\n\t\t\"@type\": \"Organization\",");
        StructuredData.Append("\n\t\t\"url\": \""); StructuredData.Append(hostUrl); StructuredData.Append("\",");
        StructuredData.Append("\n\t\t\"logo\": \""); StructuredData.Append(hostUrl); StructuredData.Append("/favicon.ico\","); //The image must be 112x112px, at minimum.
        
        //StructuredData.Append("\n\t\t\"name\": \""); StructuredData.Append(prefix); StructuredData.Append("\",");
        StructuredData.Append("\n\t\t\"name\": \"Empowered Kids Ontario\""); 
        
        StructuredData.Append("\n\t\t\"alternateName\": \"EKO\",");
        StructuredData.Append("\n\t\t\"description\": \""); StructuredData.Append(ds.Tables[0].Rows[0]["description"].ToString()); StructuredData.Append("\",");
        StructuredData.Append("\n\t\t\"contactPoint\": [");
        StructuredData.Append("\n\t\t\t{ \"@type\" : \"ContactPoint\",");
        StructuredData.Append("\n\t\t\t  \"phone\" : \"(416) 424-3864\",");
        StructuredData.Append("\n\t\t\t  \"fax\" : \"\",");
        StructuredData.Append("\n\t\t\t  \"email\" : \"info@empoweredkidsontario.ca\",");
        StructuredData.Append("\n\t\t\t  \"contactType\" : \"customer service\"");
        StructuredData.Append("\n\t\t\t}");
        //StructuredData.Append("\n\t\t\t,{ \"@type\" : \"ContactPoint\",");
        //StructuredData.Append("\n\t\t\t  \"telephone\" : \"+1-613-562-6262 ext 1699\",");
        ////StructuredData.Append("\n\t\t\t  \"faxNumber\" : \"+1-519-524-5579\",");
        //StructuredData.Append("\n\t\t\t  \"email\" : \"dmartin@bruyere.org\",");
        //StructuredData.Append("\n\t\t\t  \"contactType\" : \"customer service\"");
        //StructuredData.Append("\n\t\t\t}");
        //StructuredData.Append("\n\t\t\t,{ \"@type\" : \"ContactPoint\",");
        //StructuredData.Append("\n\t\t\t  \"telephone\" : \"+1-877-240-3941\",");
        //StructuredData.Append("\n\t\t\t  \"contactOption\" : \"TollFree\",");
        //StructuredData.Append("\n\t\t\t  \"contactType\" : \"customer service\"");
        //StructuredData.Append("\n\t\t\t}");
        StructuredData.Append("\n\t\t]");
        StructuredData.Append("\n\t}");
        StructuredData.Append("\n\t</script>");
        control = new LiteralControl(StructuredData.ToString());
        placeHolder.Controls.Add(control);

#region DroppablePanel
        foreach (DataRow dr in ds.Tables[1].Rows)
        {
            placeHolder = (ContentPlaceHolder)Master.FindControl(dr["name"].ToString());
            if (placeHolder == null)
            {
              //   Response.Write(dr["name"].ToString() + "<br />");
                continue;
            }

            Panel DroppablePanel = null;
           ////string[] DroppableZones = { "content", "content2", "banner", "mainmenu", "leftmenu", "rightmenu" };
            string[] DroppableZones = { "content", "banner", "leftmenu"};

            // Opens wrapping div to make control sortable
            if (Session["LoggedInID"] != null && Permissions.Get(int.Parse(Session["LoggedInID"].ToString()), int.Parse(Session["PageID"].ToString())) > 1 
                && DroppableZones.Contains(dr["name"].ToString().ToLower())
                && _seo.ToLower() != "login"
                && _seo.ToLower() != "changepassword"
                && _seo.ToLower() != "forgotpassword"
                )
            {
                if ((DroppablePanel = (Panel)placeHolder.FindControl("DroppableContent")) == null)
                {
                   // Response.Write(dr["id"].ToString() + "<br />");
                    DroppablePanel = new Panel();
                    DroppablePanel.ID = "DroppableContent";
                    DroppablePanel.CssClass = "DroppableContent DC_" + dr["name"].ToString();
                    DroppablePanel.Attributes.Add("ZoneId", dr["id"].ToString());
                    placeHolder.Controls.Add(DroppablePanel);
                }

                if (!string.IsNullOrEmpty(dr["control"].ToString()))
                {
                    string WidgetIcon = string.Empty;
                    switch (dr["control"].ToString().ToLower())
                    {
                        case "content":
                            WidgetIcon = "pages.png";
                            break;
                        case "banners":
                            WidgetIcon = "bannergallery.png";
                            break;
                        case "photos":
                            WidgetIcon = "photogallery.png";
                            break;
                        case "documents":
                            WidgetIcon = "documents.png";
                            break;
                        case "eform":
                            WidgetIcon = "eforms.png";
                            break;
                        case "popupmessages":
                            WidgetIcon = "sharepage.png";
                            break;
                        case "menu":
                            WidgetIcon = "menu.png";
                            break;
                        default:
                            WidgetIcon = "widgets_18x18.png";
                            break;
                    }

                    //string temp = "<div id=\"widget_" + dr["control"].ToString() + "_" + dr["param"].ToString() + "\" class=\"Draggable_Widget\">";
                    string temp = "<div id=\"widget_" + dr["control"].ToString() + "_" + dr["param"].ToString() + "\" class=\"Draggable_Widget\" ZoneId=\"" + dr["id"].ToString() + "\">";
                    temp += "<div class=\"widget_handle\" style=\"display:none;" + (dr["name"].ToString().ToLower() == "mainmenu" ? "top:-25px;" : "") + "\" title=\"" + dr["control"].ToString() + "\"><img src=\"/images/lemonaid/menuicons/" + WidgetIcon + "\" class=\"widget_handle_icon\" alt=\"\" /></div>";
                    if (string.IsNullOrEmpty(dr["control"].ToString()))
                    {
                        temp += "</div>";
                    }
                    
                    control = new LiteralControl(temp);
                    if (DroppablePanel == null)
                        placeHolder.Controls.Add(control);
                    else
                        DroppablePanel.Controls.Add(control);
                }
                else
                {
                    string WidgetIcon = string.Empty;
                    WidgetIcon = "widgets_18x18.png";

                    string temp = "<div id=\"widget_" + dr["control"].ToString() + "_" + dr["param"].ToString() + "\" class=\"Draggable_Widget\" ZoneId=\"" + dr["id"].ToString() + "\">";
                    temp += "<div class=\"widget_handle\" style=\"display:none;" + (dr["name"].ToString().ToLower() == "mainmenu" ? "top:-25px;" : "") + "\" title=\"" + dr["control"].ToString() + "\"><img src=\"/images/lemonaid/menuicons/" + WidgetIcon + "\" class=\"widget_handle_icon\" alt=\"\" /></div>";
                    if (string.IsNullOrEmpty(dr["control"].ToString()))
                    {
                        temp += "</div>";
                    }
                        
                    control = new LiteralControl(temp);
                    if (DroppablePanel == null)
                        placeHolder.Controls.Add(control);
                    else
                        DroppablePanel.Controls.Add(control);
                }
            }

            if (!string.IsNullOrEmpty(dr["control"].ToString()))
            {
              //  Response.Write(dr["control"].ToString() + "<br />");
                //  try
                {

                    UserControl userControl = LoadControlExtension.LoadControl(this, "~/Controls/" + dr["control"].ToString() + "/" + dr["control"].ToString() + ".ascx", dr["param"].ToString());
                    if (DroppablePanel == null)
                        placeHolder.Controls.Add(userControl);
                    else
                        DroppablePanel.Controls.Add(userControl);
                }
                //   catch { }

             
                // Closes wrapping div to make control sortable
                if (Session["LoggedInID"] != null && Permissions.Get(int.Parse(Session["LoggedInID"].ToString()), int.Parse(Session["PageID"].ToString())) > 1 
                    && DroppableZones.Contains(dr["name"].ToString().ToLower())
                    && _seo.ToLower() != "login"
                    && _seo.ToLower() != "changepassword"
                    && _seo.ToLower() != "forgotpassword"
                 )
                {
                    control = new LiteralControl("</div>");
                    if (DroppablePanel == null)
                        placeHolder.Controls.Add(control);
                    else
                        DroppablePanel.Controls.Add(control);
                }
            }
        }
#endregion

#region Timed Content

        if (ds.Tables[0].Rows.Count > 0 && (bool)ds.Tables[0].Rows[0]["AutoRefresh"])
        {
            //Response.AddHeader("Refresh", "300"); // Refresh page every 5 min
            string ContentList = ds.Tables[0].Rows[0]["ContentList"].ToString();

            StringBuilder sb = new StringBuilder();

            sb.Append(Environment.NewLine);
            sb.Append("    function GetTimedContentList(){" + Environment.NewLine);
            sb.Append("        $.ajax({" + Environment.NewLine);
            sb.Append("            async : true," + Environment.NewLine);
            sb.Append("            type: \"POST\"," + Environment.NewLine);
            sb.Append("            url: \"/default.aspx/GetTimedContentList\"," + Environment.NewLine);
            sb.Append("            data: \"{'PageID':'" + Session["PageID"].ToString() + "'}\"," + Environment.NewLine);
            sb.Append("            contentType: \"application/json; charset=utf-8\"," + Environment.NewLine);
            sb.Append("            dataType: \"json\"," + Environment.NewLine);
            sb.Append("            success: function(response) {" + Environment.NewLine);
            sb.Append("                 var cl = response.d;" + Environment.NewLine);
            //sb.Append("                 alert(cl);" + Environment.NewLine);
            sb.Append("                 if (cl != '" + ContentList + "')" + Environment.NewLine);
            sb.Append("                     location.reload();" + Environment.NewLine);
            sb.Append("            }," + Environment.NewLine);
            sb.Append("            error: function(xhr, status, errorThrown) {" + Environment.NewLine);
            sb.Append("                 alert(status + \" | \" + xhr.responseText);" + Environment.NewLine);
            sb.Append("            }" + Environment.NewLine);
            sb.Append("        });" + Environment.NewLine);
            sb.Append("    }" + Environment.NewLine);
            sb.Append("    $(document).ready(function () {" + Environment.NewLine);
            sb.Append("        setInterval(\"GetTimedContentList()\", 60000);" + Environment.NewLine);
            sb.Append("    });" + Environment.NewLine);

            Page.ClientScript.RegisterStartupScript(GetType(), "timedContent", sb.ToString(), true);
        }

#endregion Timed Content
    }

    private DataTable GetMemberDirectory(string seo)
    {
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]))
        {
            string sqlstr = @"select * from eko.Organizations where active=1 and deleted=0 and seo=@seo";
            SqlDataAdapter dapt = new SqlDataAdapter(sqlstr, ConfigurationManager.AppSettings["CMServer"]);
            dapt.SelectCommand.Parameters.Add(new SqlParameter("@seo", seo));

            DataTable dt = new DataTable();
            dapt.Fill(dt);

            return dt;
        }
    }
    private string GetImageSrc(string content , string quote)
    {
        string src = "";
        int npos;
        if (content.Contains("<img "))
        {
            npos = content.IndexOf("<img ");
            src = content.Substring(npos + ("<img ").Length);
            npos = content.IndexOf(" src=" + quote);
            src = content.Substring(npos + (" src=" + quote).Length);

            npos = src.IndexOf(quote);
            src = src.Substring(0, npos);
        }
        return src;
    }

    private void WriteMetas(StringBuilder metas, string title, string description, string url, string img)
    {
        //metas.Append("<meta name=\"description\" content=\""); metas.Append(""); metas.Append("\"/>"); //
        metas.Append("<meta name=\"description\" content=\""); metas.Append(description); metas.Append("\"/>");
        metas.Append("\n\t<meta property=\"og:title\" content=\""); metas.Append(title); metas.Append("\"/>");       
        //metas.Append("\n\t<meta property=\"og:description\" content=\""); metas.Append(""); metas.Append("\"/>");
        metas.Append("\n\t<meta property=\"og:description\" content=\""); metas.Append(description); metas.Append("\"/>");
        metas.Append("\n\t<meta property=\"og:url\" content=\""); metas.Append(url); metas.Append("\"/>");
        metas.Append("\n\t<meta property=\"og:image\" content=\""); metas.Append(img); metas.Append("\"/>");

    }

    private void LoadFixZone(string zone, string control, string param)
    {
        try
        {
            ContentPlaceHolder placeHolder = (ContentPlaceHolder)Master.FindControl(zone);
            if (placeHolder != null)
            {
                UserControl userControl = LoadControlExtension.LoadControl(this, "~/Controls/" + control + "/" + control + ".ascx", param);
                placeHolder.Controls.Add(userControl);
            }
        }
        catch { }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        CheckIP.ValidateIP();

        ContentPlaceHolder placeHolder;
		placeHolder = (ContentPlaceHolder) Master.FindControl("admin");
		placeHolder.Controls.Clear();

        if (Session["LoggedInID"] != null)
        {
            //ClientScript.RegisterStartupScript(this.GetType(),"asd","alert('" + Session["LoggedInID"].ToString() + "');",true);
            //TODO: Add actual permission checking

            //if (Permissions.Get(Convert.ToInt32(Session["LoggedInID"].ToString()), Convert.ToInt32(Session["PageID"].ToString())) > 1
            //    )
            if (Permissions.Get(Convert.ToInt32(Session["LoggedInID"].ToString())) > 1 )
            {
                if(MasterPageFile.Contains("Admin.master") 
                    || !(Permissions.Get(Convert.ToInt32(Session["LoggedInID"].ToString()), Convert.ToInt32(Session["PageID"].ToString())) > 1)
                  )
                    placeHolder.Controls.Add(LoadControl("/Admin/topmen/MenuNoBlueBox.ascx"));
                else
                    placeHolder.Controls.Add(LoadControl("/Admin/topmen/Menu.ascx"));
            }

           
        }
        else
        {
            //ClientScript.RegisterStartupScript(this.GetType(),"asd","alert('no');",true);
        }

#region Responsive Videos
        // To make embeded videos responsive
        string script = string.Empty;
        script = "\n$(document).ready(function () {";
        script += "\n   $('.Responsive-Video').each(function () {";
        script += "\n       $(this).wrapAll('<div class=\"Responsive-Video-Wrapper\"></div>');";
        script += "\n   });";
        script += "\n});";

        Page.ClientScript.RegisterStartupScript(GetType(), "ResponsiveVideo", script, true);
#endregion Responsive Videos

        //Page.ClientScript.RegisterClientScriptInclude(GetType(), "CollapsiblePanel", "/js/CollapsiblePanel.js");
        //Page.ClientScript.RegisterClientScriptInclude(GetType(), "table-heading", "/js/table.heading.js");

        if (CMSHelper.GetSeo() == "error")
        {
            //HttpContext.Current.Response.Clear();
            //HttpContext.Current.Server.ClearError();
            HttpContext.Current.Response.Status = "404 Page Not Found";
            HttpContext.Current.Response.StatusCode = 404;
            HttpContext.Current.Response.StatusDescription = "Page not found";
        }

        //if (HttpContext.Current.User != null)
        //{
        //    if (HttpContext.Current.User.Identity.IsAuthenticated)
        //    {
        //        //if (User.IsInRole("TestPrivate"))
        //        {
        //            Response.Write("<br /><br /><br /><br /><br />Roles for " + User.Identity.Name);
        //            foreach (string role in Roles.GetRolesForUser())
        //            {
        //                Response.Write("<br />" + role);
        //            }
        //        }
        //        if (HttpContext.Current.User.Identity is FormsIdentity)
        //        {
        //            FormsIdentity id = (FormsIdentity)HttpContext.Current.User.Identity;
        //            FormsAuthenticationTicket ticket = id.Ticket;
        //            string userData = ticket.UserData;
        //            Response.Write("<br />userData: " + userData);
        //            //string[] roles = userData.Split(',');
        //            //HttpContext.Current.User = new GenericPrincipal(id, roles);
        //        }
        //    }
        //}

    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //if (!IsPostBack)
        {
            ////InjectContent("head", Environment.NewLine + "$(document).ready(function (){" + Environment.NewLine +
            ////    " setTimeout(function () { $(window).scrollTop(0); $('#miniLogin').addClass('delayLoad');}, 10); " + Environment.NewLine +
            ////     "});" + Environment.NewLine, true);

            //InjectContent("Scripts", Environment.NewLine + "$(document).ready(function (){" + Environment.NewLine +
            //  //" setTimeout(function () { $(window).scrollTop(0);}, 600); " + Environment.NewLine +
            //  " setTimeout(function () { $('#ctl05_SearchText1_tbSearch').focus();}, 600); " + Environment.NewLine +
            //    "});" + Environment.NewLine, true);
        }

        if (Session["LoggedInID"] != null)
        {

            DataSet ds = new DataSet();
            SqlDataAdapter dap = new SqlDataAdapter(@"eko.GetMembershipType", ConfigurationManager.AppSettings["CMServer"]);
            dap.SelectCommand.CommandType = CommandType.StoredProcedure;
            dap.SelectCommand.Parameters.AddWithValue("@uid", Session["LoggedInID"].ToString());

            dap.Fill(ds);
            DataTable dtu = ds.Tables[0];

            if (dtu.Rows.Count > 0)
            {
                string myclass = "";
                switch (dtu.Rows[0]["FinalType"].ToString())
                {
                    case "1":
                        myclass = "TYPE_EKO";
                        break;
                    case "2":
                        myclass = "TYPE_EKO-BC";
                        break;
                    case "3":
                        myclass = "TYPE_PNCA";
                        break;
                    case "4":
                        myclass = "TYPE_BOTH";
                        break;
                    default:
                        break;

                }

                if (myclass != "")
                {
                    StringBuilder script = new StringBuilder();
                    script.Append(Environment.NewLine + "$(document).ready(function () {" + Environment.NewLine);
                    script.Append(Environment.NewLine + String.Format("$(body).addClass(\"{0}\");", myclass) + Environment.NewLine);
                    script.Append(Environment.NewLine + "});" + Environment.NewLine);

                    InjectContent("Scripts", script.ToString(), true);
                }
            }

        }

    }

    public void InjectContent(string placeholder, string content, bool addScriptTags = false)
    {
        Literal lit = new Literal();
        if(addScriptTags)
            lit.Text = "<script>" + content + "</script>";
        else
            lit.Text = content;

        ContentPlaceHolder  placeHolder = (ContentPlaceHolder)Master.FindControl(placeholder);
        placeHolder.Controls.Add(lit);
    }
    public void InjectContent(string placeholder, string content, int counter, bool addScriptTags = false)
    {
        if (counters[counter] == 0)
        {
            InjectContent(placeholder, content, addScriptTags);
        }

        counters[counter]++;
    }

    int[] counters = new int[11];

}
