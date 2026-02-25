#define MY_VERSION
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Configuration;
using System.Security.Policy;

public class UrlParameterWhitelistValidator
{
    private Dictionary<string, HashSet<string>> UrlParameterWhitelist;
    private Dictionary<string, HashSet<string>> UrlPatternWhitelist;

    public UrlParameterWhitelistValidator()
    {
        LoadWhitelistFromConfig();
    }
    private void LoadWhitelistFromConfig()
    {
        UrlParameterWhitelist = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);
        UrlPatternWhitelist = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);

        foreach (string key in ConfigurationManager.AppSettings.AllKeys)
        {
            if (key.StartsWith("Whitelist:"))
            {
                var url = key.Substring("Whitelist:".Length);
                var allowedParams = new HashSet<string>(ConfigurationManager.AppSettings[key].Split(','), StringComparer.OrdinalIgnoreCase);

                if (url.EndsWith("*"))
                {
                    UrlPatternWhitelist[url.TrimEnd('*')] = allowedParams;
                }
                else
                {
                    UrlParameterWhitelist[url] = allowedParams;
                }
            }
        }
    }

#if MY_VERSION
    public NameValueCollection ValidateAndFilter(Page page)
    {
        var currentUrlPath = page.Request.Url.AbsolutePath;
        HashSet<string> allowedParameters = null;

        return ValidateAndFilter(page, currentUrlPath, ref allowedParameters);

    }

    public NameValueCollection ValidateAndFilter(Page page,string seo)
    {
        var currentUrlPath = seo;
        HashSet<string> allowedParameters = null;
        
        return ValidateAndFilter(page, currentUrlPath, ref allowedParameters);
    }

    private NameValueCollection ValidateAndFilter(Page page, string currentUrlPath, ref HashSet<string> allowedParameters)
    {
        var query = page.Request.QueryString;

        if (query == null || query.Count == 0)
        {
            return null;
        }

        if (UrlParameterWhitelist.TryGetValue(currentUrlPath, out allowedParameters))
        {
            var filteredQueryString = FilterQueryString(query, allowedParameters);
            if (filteredQueryString.Count != query.Count)
            {
                SendForbiddenResponse(page);
                return null;
            }
            else
                return filteredQueryString;
        }

        foreach (var pattern in UrlPatternWhitelist)
        {
            if (currentUrlPath.StartsWith(pattern.Key, StringComparison.OrdinalIgnoreCase))
            {
                allowedParameters = pattern.Value;
                var filteredQueryString = FilterQueryString(query, allowedParameters);
                if (filteredQueryString.Count != query.Count)
                {
                    SendForbiddenResponse(page);
                    return null;
                }
                else
                    return filteredQueryString;
            }
        }

        SendForbiddenResponse(page);
        return null;
    }

    public bool ValidateAndFilter(string currentUrlPath, NameValueCollection query)
    {
        HashSet<string> allowedParameters = null;

        if (query == null || query.Count == 0)
        {
            return true;
        }

        if (UrlParameterWhitelist.TryGetValue(currentUrlPath, out allowedParameters))
        {
            var filteredQueryString = FilterQueryString(query, allowedParameters);
            if (filteredQueryString.Count != query.Count)
                return false;
            else
                return true;
        }

        foreach (var pattern in UrlPatternWhitelist)
        {
            if (currentUrlPath.StartsWith(pattern.Key, StringComparison.OrdinalIgnoreCase))
            {
                allowedParameters = pattern.Value;
                var filteredQueryString = FilterQueryString(query, allowedParameters);
                if (filteredQueryString.Count != query.Count)
                    return false;
                else
                    return true;
            }
        }

        return false;

    }


#else
    public NameValueCollection ValidateAndFilter(Page page)
    {
        var currentUrlPath = page.Request.Url.AbsolutePath;
        HashSet<string> allowedParameters = null;

        if (UrlParameterWhitelist.TryGetValue(currentUrlPath, out allowedParameters))
        {
            var filteredQueryString = FilterQueryString(page.Request.QueryString, allowedParameters);
            return filteredQueryString.Count > 0 ? filteredQueryString : null;
        }

        foreach (var pattern in UrlPatternWhitelist)
        {
            if (currentUrlPath.StartsWith(pattern.Key, StringComparison.OrdinalIgnoreCase))
            {
                allowedParameters = pattern.Value;
                var filteredQueryString = FilterQueryString(page.Request.QueryString, allowedParameters);
                return filteredQueryString.Count > 0 ? filteredQueryString : null;
            }
        }

        SendForbiddenResponse(page);
        return null;
    }

    public NameValueCollection ValidateAndFilter(Page page, string Url, string seo)
    {
        var currentUrlPath = seo;
        HashSet<string> allowedParameters = null;

        UriBuilder uriBuilder = new UriBuilder(Url);
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);

        if (UrlParameterWhitelist.TryGetValue(currentUrlPath, out allowedParameters))
        {
            var filteredQueryString = FilterQueryString(query, allowedParameters);
            return filteredQueryString.Count > 0 ? filteredQueryString : null;
        }

        foreach (var pattern in UrlPatternWhitelist)
        {
            if (currentUrlPath.StartsWith(pattern.Key, StringComparison.OrdinalIgnoreCase))
            {
                allowedParameters = pattern.Value;
                var filteredQueryString = FilterQueryString(query, allowedParameters);
                return filteredQueryString.Count > 0 ? filteredQueryString : null;
            }
        }

        SendForbiddenResponse(page);
        return null;
    }



#endif

    private NameValueCollection FilterQueryString(NameValueCollection queryString, HashSet<string> allowedParams)
    {
        var filtered = HttpUtility.ParseQueryString(string.Empty);

        foreach (string key in queryString.AllKeys)
        {
            if (key != null && allowedParams.Contains(key))
            {
                filtered[key] = queryString[key];
            }
        }

        return filtered;
    }

    private void SendForbiddenResponse(Page page)
    {
        page.Response.StatusCode = 403;
        page.Response.StatusDescription = "Forbidden";
        page.Response.Write("403 Forbidden: Invalid URL or Query String Parameters.");
        page.Response.End();
    }
}

