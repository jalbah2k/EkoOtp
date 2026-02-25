using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.IO;

public partial class SampleUtil
{
    static public HttpContext Context
    {
        get
        {
            return HttpContext.Current;
        }
    }

    static public string GetAjaxMode()
    {
        HttpCookie cookie;
        return GetAjaxMode(out cookie);
    }
    static public string GetAjaxMode(out HttpCookie cookie)
    {
        cookie = Context.Request.Cookies["AjaxMode"];
        if (cookie != null)
        {
            switch (cookie.Value)
            {
                case "Atlas":
                    return "Atlas";
                case "Magic":
                    return "Magic";
                case "None":
                    return "None";
                default:
                    break;
            }
        }
        return "Atlas";
    }

    static public void SetAjaxMode(string mode)
    {
        HttpCookie cookie = new HttpCookie("AjaxMode");
        cookie.Path = "/";
        cookie.Expires = DateTime.Now.AddYears(1);
        cookie.Value = mode;
        Context.Response.Cookies.Add(cookie);
    }

    static public void SetPageCache()
    {
        if (Context.Request.Browser.Browser == "Opera")
        {
            Context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        }
    }

    static public DateTime ToUserLocalTime(DateTime time)
    {
        HttpCookie cookie = Context.Request.Cookies["TimeZone"];
        if (cookie == null)
            return time;
        int tzo = int.Parse(cookie.Value);
        return time.ToUniversalTime().AddHours(tzo);
    }
}
