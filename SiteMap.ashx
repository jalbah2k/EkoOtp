<%@ WebHandler Language="C#" Class="SiteMap" %>

using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using System.Xml;

public class SiteMap : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/xml";
        using (XmlTextWriter writer = new XmlTextWriter(context.Response.OutputStream, Encoding.UTF8))
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("urlset");
            writer.WriteAttributeString("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9");

            string connect = ConfigurationManager.AppSettings["CMServer"];
            //string url = "http://" + ConfigurationManager.AppSettings["SiteUrl"];
            string url = context.Request.Url.Scheme + "://" + context.Request.Url.Host;
            using (SqlConnection conn = new SqlConnection(connect))
            {
                using (SqlCommand cmd = new SqlCommand("GetSiteMapContent", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            writer.WriteStartElement("url");
                            writer.WriteElementString("loc", string.Format("{0}{1}", url, rdr["url"]));
                            
                            if (!String.IsNullOrEmpty(rdr["date"].ToString()))
                                writer.WriteElementString("lastmod", string.Format("{0:yyyy-MM-dd}", rdr["date"]));
                            
                            string priority = "0.5";
                            if (rdr["linkid"].ToString() == "1")
                                priority = "1.0";

                            writer.WriteElementString("changefreq", "weekly");
                            writer.WriteElementString("priority", priority);
                            writer.WriteEndElement();
                            
                        }
                        
                        writer.WriteEndElement();
                        writer.WriteEndDocument();
                        writer.Flush();
                    }
                    context.Response.End();
                }
            }
        }

    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}