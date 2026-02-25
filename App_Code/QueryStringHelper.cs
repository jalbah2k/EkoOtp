using System;
using System.Web;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using System.Web.Security.AntiXss;

public static class QueryStringHelper
{
    public static string SanitizeQueryValue(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;

        // Trim whitespace
        value = value.Trim();

        // Remove HTML tags (Basic XSS protection)
        value = Regex.Replace(value, "<.*?>", string.Empty);

        // Remove SQL injection risk characters
        value = value.Replace("'", "").Replace("--", "").Replace(";", "");

        // Encode the value to prevent injection attacks
        return HttpUtility.UrlEncode(value);
    }

    public static string ModifyAndSanitizeQuery(string url, NameValueCollection newParameters)
    {
        UriBuilder uriBuilder = new UriBuilder(url);
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);

        foreach (string key in newParameters)
        {
            if (key != null)
            {
                string sanitizedValue = SanitizeQueryValue(newParameters[key]);
                query[key] = sanitizedValue;
            }
        }

        uriBuilder.Query = query.ToString();
        return uriBuilder.ToString();
    }

    public static string SanitizeAndModifyUrl(string url)
    {
        if (string.IsNullOrEmpty(url))
            return url;

        UriBuilder uriBuilder = new UriBuilder(url);
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);

        // Sanitize each query string parameter
        foreach (string key in query.AllKeys)
        {
           // if (key != null)
            {
                try { query[key] = AntiXssEncoder_HtmlEncode(query[key], true); }
                catch { continue; }
            }
        }

        // Rebuild the URL with sanitized query string
        uriBuilder.Query = query.ToString();
        return uriBuilder.ToString().Replace(":443", "").Replace(":80", "");
    }
    static public string AntiXssEncoder_HtmlEncode(string input, bool useNamedEntities)
    {
        return AntiXssEncoder.HtmlEncode(input, useNamedEntities);
    }
}
