using System;
using System.Configuration;
using System.Web;

/// <summary>
/// Shared URLs and page detection for the Who's Who / Member Directory.
/// </summary>
public static class WhoisWhoPage
{
    /// <summary>CMS page SEO slug (Pages.seo).</summary>
    public const string Seo = "whos-who";

    /// <summary>Canonical public URL shown in the address bar / nav.</summary>
    public const string Url = "/member-directory";

    /// <summary>Legacy CMS SEO URL; redirected to <see cref="Url"/>.</summary>
    public const string LegacyUrl = "/whos-who";

    public const string NavText = "Who’s Who";
    public const string Title = "Member Directory";

    public static string DashboardUrl
    {
        get
        {
            string seo = ConfigurationManager.AppSettings["EKOMembersSeo"];
            if (string.IsNullOrWhiteSpace(seo))
            {
                seo = "EKOMembers";
            }
            return "/" + seo.Trim('/');
        }
    }

    public static bool IsCurrent()
    {
        string seo = "";
        try
        {
            seo = (CMSHelper.GetSeo() ?? "").Trim().ToLowerInvariant();
        }
        catch
        {
        }

        string path = "";
        try
        {
            path = (HttpContext.Current.Request.Path ?? "").Trim().ToLowerInvariant();
        }
        catch
        {
        }

        return seo == "whos-who"
            || seo == "whoswho"
            || seo == "member-directory"
            || seo == "memberdirectory"
            || path == "/whos-who"
            || path == "/whoswho"
            || path == "/member-directory"
            || path == "/memberdirectory";
    }

    /// <summary>
    /// Prefer /member-directory over the CMS slug /whos-who in the address bar.
    /// </summary>
    public static void EnsureCanonicalUrl()
    {
        if (HttpContext.Current == null || HttpContext.Current.Request == null)
        {
            return;
        }

        string path = (HttpContext.Current.Request.Path ?? "").TrimEnd('/').ToLowerInvariant();
        if (path == "/whos-who" || path == "/whoswho")
        {
            string query = HttpContext.Current.Request.Url.Query;
            HttpContext.Current.Response.Redirect(Url + query, true);
        }
    }

    public static bool MenuAlreadyHasItem(string html)
    {
        if (string.IsNullOrEmpty(html))
        {
            return false;
        }

        return html.IndexOf("Who's Who", StringComparison.OrdinalIgnoreCase) >= 0
            || html.IndexOf("Who’s Who", StringComparison.OrdinalIgnoreCase) >= 0;
    }

    public static string BreadcrumbHtml()
    {
        return
            "<div class=\"breadcrumbs eko-who-breadcrumbs\">" +
                "<a href=\"" + DashboardUrl + "\">Dashboard</a>" +
                "<span class=\"eko-who-crumb-sep\"> &gt; </span>" +
                "<span class=\"eko-who-crumb-current\">" + Title + "</span>" +
            "</div>" +
            "<div class=\"div-page-title\"><h1>" + Title + "</h1></div>";
    }
}