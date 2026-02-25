//#define MULTI_LANGUAGE
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System;
using System.IO;
using System.Web.UI;
using System.Linq;
using System.Web.Routing;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;

/// <summary>
/// Summary description for Helper
/// </summary>
public class CMSHelper
{
    public CMSHelper()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public static string GetCleanQueryString(string QueryStringVar)
    {
        return GetCleanQueryString(QueryStringVar, string.Empty);
    }
    public static string GetCleanQueryString(string QueryStringVar, string defaultValue)
    {
        string _queryStringVar = defaultValue;
        if (HttpContext.Current.Request.QueryString[QueryStringVar] != null)
        {
            string[] qslist = HttpContext.Current.Request.QueryString[QueryStringVar].Split(',');
            if (qslist.Length > 1)
                _queryStringVar = qslist[qslist.Length - 1];
            else
                _queryStringVar = qslist[0];
        }
        return _queryStringVar;
    }
    public static string GetFullQueryString()
    {
        string qs = string.Empty;

        foreach (String key in HttpContext.Current.Request.QueryString.AllKeys)
        {
            if (key != "" && key != "seo" && key != "lang" && key != "pid" && key != "red")
                qs += key + "=" + HttpContext.Current.Request.QueryString[key] + "&";
        }
        qs = qs.TrimEnd('&');
        if (qs == "=")
            qs = string.Empty;

        return qs;
    }

    /*public static string GetLanguagePrefix()
    {
        return GetLanguagePrefix(true);
    }
    public static string GetLanguagePrefix(bool multilanguage)
    {
        if (multilanguage)
        {
            string _prefix = "/en/";
            if (GetCleanQueryString("lang", "1") == "2")
                _prefix = "/fr/";

            return _prefix;
        }
        else
        {
            return "/";
        }
    }*/

    public static string GetSeo()
    {
        string seo = string.Empty;

        //////if (HttpContext.Current.Request.QueryString["seo"] != null)
        //////{
        //////    seo = CMSHelper.GetCleanQueryString("seo");
        //////}
        //////else if (HttpContext.Current.Request.QueryString["pid"] != null)
        //////{
        //////    using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]))
        //////    {
        //////        SqlCommand cmd = new SqlCommand("select seo from pages where id=@id", conn);
        //////        cmd.Parameters.AddWithValue("@id", CMSHelper.GetCleanQueryString("pid"));
        //////        conn.Open();
        //////        object o = cmd.ExecuteScalar();
        //////        conn.Close();
        //////        if (o != null && o != DBNull.Value)
        //////            seo = o.ToString();
        //////    }
        //////}

        if (HttpContext.Current.Request.RequestContext.RouteData.Values.Count > 0 && HttpContext.Current.Request.RequestContext.RouteData.Values["seo"] != null)
        {
            seo = HttpContext.Current.Request.RequestContext.RouteData.Values["seo"].ToString(); 
        }
        else
        {
            seo = HttpContext.Current.Request.Url.AbsolutePath.Replace(".aspx", "").Replace("/", "");
        }
        return seo;
    }
    public static string ApplicationPath
    {
        get
        {
            string applicationPath = HttpContext.Current.Request.ApplicationPath;
            if (applicationPath.EndsWith("/") == false)
                applicationPath = applicationPath + "/";

            return applicationPath;
        }
    }
    public static string ApplicationFolder
    {
        get
        {
            string AppFolder = HttpContext.Current.Server.MapPath("~/").TrimEnd('\\');
            AppFolder = AppFolder.Substring(AppFolder.LastIndexOf('\\') + 1);

            return AppFolder;
        }
    }
    public static bool isMultilingual
    {
        get
        {
#if MULTI_LANGUAGE
            return true;   //// HttpContext.Current.Session != null && HttpContext.Current.Session["Multilingual"] != null ? (bool)HttpContext.Current.Session["Multilingual"] : false;
#else
            return false;
#endif
        }
    }
    public static bool AlwaysWithEN
    {
        get
        {
            bool _alwaysWithEN = false;
            try { _alwaysWithEN = Convert.ToBoolean(ConfigurationManager.AppSettings["AlwaysWithEN"].ToString()); }
            catch { }

            return false;   //// _alwaysWithEN;
        }
    }
    public static string SeoPrefixEN
    {
        get
        {
            return "/";     ////isMultilingual || AlwaysWithEN ? "/en/" : "/";
        }
    }
    public static string GetSeoWithLanguagePrefix()
    {
        return GetLanguagePrefix(AlwaysWithEN) + GetSeo();
    }
    public static string GetSeoWithLanguagePrefix(bool alwaysWithEN)
    {
        return GetLanguagePrefix(alwaysWithEN) + GetSeo();
    }
    public static string GetLanguagePrefix()
    {
        return GetLanguagePrefix(AlwaysWithEN);
    }
    public static string GetLanguagePrefix(bool alwaysWithEN)
    {
        string _prefix = "/";       // SeoPrefixEN;

#if MULTI_LANGUAGE

        string lang = "en";
        try
        {
            lang = HttpContext.Current.Request.RequestContext.RouteData.Values["lang"].ToString();
        }
        catch { }

        if ( lang.ToLower() == "fr")
        {
            if (isMultilingual || alwaysWithEN)
            {
                //if (CMSHelper.GetCleanQueryString("lang", "1") == "1")
                //    _prefix = "/en/";
                //else
                //    _prefix = "/fr/";

                try { _prefix = "/" + HttpContext.Current.Request.RequestContext.RouteData.Values["lang"].ToString() + "/"; }
                catch { _prefix = "/"; }
            }
        }
#endif
        return _prefix;
    }
    public static string GetLanguageNumber()
    {
        //return HttpContext.Current.Session != null && HttpContext.Current.Session["Language"] != null ? HttpContext.Current.Session["Language"].ToString() : "1";
        return GetLanguage().ToLower().Contains("fr") ? "2" : "1";
    }
    public static string GetLanguage()
    {
        return GetLanguage(AlwaysWithEN);
    }
    public static string GetLanguage(bool alwaysWithEN)
    {
        string _prefix = "en";
        if (isMultilingual || alwaysWithEN)
        {
            try { _prefix = HttpContext.Current.Request.RequestContext.RouteData.Values["lang"].ToString(); }
            catch { _prefix = "en"; }
        }

        return _prefix;
    }
    static public long getDirectorySize(string path)
    {
        string[] files = Directory.GetFiles(path);
        string[] subdirectories = Directory.GetDirectories(path);

        long size = files.Sum(x => new FileInfo(x).Length);
        foreach (string s in subdirectories)
            size += getDirectorySize(s);

        return size;
    }

    static public void ShowMessage(Page page, string message)
    {
        page.ClientScript.RegisterStartupScript(page.GetType(), "message", "alert('" + message + "')", true);
    }

    static public void RedirectPermanent(string newPath)
    {
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.Status = "301 Moved Permanently";
        HttpContext.Current.Response.AddHeader("Location", newPath);
        HttpContext.Current.Response.End();
    }
    static public void Redirect404(string newPath)
    {
        ////HttpContext.Current.Server.ClearError();
        //HttpContext.Current.Server.TransferRequest(newPath, false);

        if (newPath.Contains("/error") && (HttpContext.Current.Session["MemberId"] != null || HttpContext.Current.Session["LoggedInID"] != null))
            newPath = ConfigurationManager.AppSettings["MembersErrorPage"];

        HttpContext.Current.Response.Redirect(newPath);

    }
    static public string GenerateRandomString(int length, string charsToUse)
    {
        Random randomNumber = new Random();

        StringBuilder randomString = new StringBuilder();
        char appendedChar;

        for (int i = 0; i <= length; i++)
        {
            int characterIndex = Convert.ToInt32(randomNumber.Next(0, charsToUse.Length - 1));
            appendedChar = charsToUse[characterIndex];
            randomString.Append(appendedChar);
        }
        return randomString.ToString();
    }
    static public string HashString(string Value)
    {
        System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] data = System.Text.Encoding.ASCII.GetBytes(Value);
        data = x.ComputeHash(data);
        string ret = "";
        for (int i = 0; i < data.Length; i++)
            ret += data[i].ToString("x2").ToLower();
        return ret;
    }
    static public string GetPagerInfo(GridView grid, int totalItems)
    {
        int startItem = 1 + grid.PageIndex * grid.PageSize;
        int endItem = grid.Rows.Count + grid.PageIndex * grid.PageSize;

        return string.Format("Showing {0}-{1} of {2} items", startItem, endItem, totalItems);
    }
    static public int GetColumnIndex(GridView grid, string SortExpression)
    {
        int i = 0;
        try
        {
            foreach (DataControlField c in grid.Columns)
            {
                if (c.SortExpression == SortExpression)
                    break;
                i++;
            }
        }
        catch { }
        if (i >= grid.Columns.Count)
            i = -1;
        return i;
    }
    static public void CreatePrivateConfigFile(string group, string path)
    {
        string webConfigTemplate = @"<?xml version=""1.0"" ?>
<configuration>
    <system.web>
        <authorization>
            <allow roles=""{0}"" />
            <deny users=""*"" />
        </authorization>
    </system.web>
</configuration>
                                    ";

        string fileName = path.TrimEnd('/') + @"/web.config";
        using (StreamWriter outputFile = new StreamWriter(fileName))
        {
            outputFile.Write(string.Format(webConfigTemplate, group));
        }
    }

    static public void CreatePrivateConfigFile(string group, string path, string id)
    {
        string webConfigTemplate = @"<?xml version=""1.0"" ?>
<configuration>
    <system.web>
        <authorization>
            <allow roles=""{0}, {1}"" />
            <allow users=""admin"" />
            <deny users=""*"" />
        </authorization>
    </system.web>
</configuration>
                                    ";

        string fileName = path.TrimEnd('/') + @"/web.config";
        using (StreamWriter outputFile = new StreamWriter(fileName))
        {
            outputFile.Write(string.Format(webConfigTemplate, group, "g" + id));
        }
    }
    static public void ClearLoginSession()
    {
        //clear main login session variable
        if (HttpContext.Current.Session != null)
        {
            HttpContext.Current.Session.Remove("LoggedInID");
            HttpContext.Current.Session.Remove("MemberId");
            //Add any other session variable used for membership

        }
    }
    static public void AutoLogin(bool redirect = false)
    {
        if (HttpContext.Current.User != null)
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated && HttpContext.Current.Session["LoggedInID"] == null)
            {
                if (HttpContext.Current.User.Identity is FormsIdentity)
                {
                    FormsIdentity id = (FormsIdentity)HttpContext.Current.User.Identity;
                    FormsAuthenticationTicket ticket = id.Ticket;
                    //string userData = ticket.UserData;

                    DataTable dt = new DataTable();
                    using (SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]))
                    {
                        SqlDataAdapter dapt = new SqlDataAdapter("select * from users where username=@username and status='active'", sqlConn);
                        dapt.SelectCommand.CommandType = CommandType.Text;
                        dapt.SelectCommand.Parameters.AddWithValue("@username", ticket.Name);

                        dapt.Fill(dt);
                    }

                    if (dt.Rows.Count > 0)
                    {
                        HttpContext.Current.Session["LoggedInID"] = dt.Rows[0]["id"].ToString();
                        if (redirect)
                        {
                            if (HttpContext.Current.Request.QueryString["c"] == null || HttpContext.Current.Request.QueryString["c"] == "")
                            {
                                HttpContext.Current.Response.Redirect(CMSHelper.GetLanguagePrefix() + "home", false);
                            }
                            else
                            {
                                HttpContext.Current.Response.Redirect("/admin.aspx?c=" + HttpContext.Current.Request.QueryString["c"], false);
                            }
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                        }
                    }
                }
            }
        }
    }
    static public string StripHtmlTags(string html)
    {
        if (String.IsNullOrEmpty(html)) return "";
        HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
        doc.LoadHtml(html);
        return HttpUtility.HtmlDecode(doc.DocumentNode.InnerText);
    }
    static public void IncludeStylesheet(Page page, string href, string styleID)
    {
        //Prevent the stylesheet being included more than once
        styleID = "_" + styleID;
        if (HttpContext.Current.Items[styleID] == null)
        {
            HtmlLink htmlLink = new HtmlLink();
            htmlLink.Href = href;
            htmlLink.Attributes.Add("rel", "stylesheet");
            htmlLink.Attributes.Add("type", "text/css");
            page.Header.Controls.Add(htmlLink);
            HttpContext.Current.Items[styleID] = true;
        }
    }
 static public bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,63}$";
        return Regex.IsMatch(email, pattern);
    }
    public enum counters { accordion_header, accordion_footer, banner_header, banner_footer, table_header, table_footer, calendar_header, calendar_footer , search_footer, photolight_header, photolight_footer };
}
public static class MyExtension
{
    public enum SizeUnits
    {
        Byte, KB, MB, GB, TB, PB, EB, ZB, YB
    }

    public static string ToSize(this Int64 value, SizeUnits unit)
    {
        return (value / (double)Math.Pow(1024, (Int64)unit)).ToString("0.0");           // ("0.00");
    }
}
