<%@ WebHandler Language="C#" Class="Robots" %>

using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;

public class Robots : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        StringBuilder sb = new StringBuilder();

        HttpResponse response = context.Response;
        HttpRequest request = context.Request;

        try
        {
            response.ContentType = "text/plain";
            response.Clear();
            response.BufferOutput = true;

            if (request.Url.Host == null) return;

            //switch (request.Url.Host)
            //{
            //    case "xxx.com":
            //        robotsContent = "XXX";
            //        break;

            //    case "yyy.xxx.com":
            //        robotsContent = "YYY";
            //        break;

            //    default:
            //        robotsContent = "XXX";
            //        break;
            //}

            string url = context.Request.Url.Scheme + "://" + context.Request.Url.Host;

            sb.AppendLine("user-agent: *");
            sb.AppendLine("disallow: /images/");
            sb.AppendLine("disallow: /js/");
            sb.AppendLine("disallow: /Login.aspx");
            sb.AppendLine("disallow: /Logout.aspx");
            sb.Append("sitemap: ");
            sb.AppendLine(url + "/sitemap.xml");
        }
        catch
        {
            sb.Clear();
            sb.AppendLine("An error occured.");
        }

        response.Write(sb.ToString());
    }
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}