//#define TEXT_ANGULAR
//#define MULTI_LANGUAGE

using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Routing;
using Microsoft.AspNet.FriendlyUrls;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Http;

public static class RouteConfig
{
    public static void RegisterRoutes(RouteCollection routes)
    {
        var settings = new FriendlyUrlSettings();
        settings.AutoRedirectMode = RedirectMode.Permanent;
        routes.EnableFriendlyUrls(settings);

        routes.Clear();

        routes.Ignore("{resource}.axd/{*pathInfo}");

        routes.Ignore("api");

        RouteTable.Routes.MapHttpRoute(
                name: "Api",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = System.Web.Http.RouteParameter.Optional }
                );

        RouteTable.Routes.MapHttpRoute(
                name: "Api-lang",
                routeTemplate: "api/{controller}/{id}/{lang}"
                );

        RouteTable.Routes.MapHttpRoute(
                name: "Api-news",
                routeTemplate: "api/{controller}/{offset}/{records}/{publish}/{category}"
                );

        RouteTable.Routes.MapHttpRoute(
               name: "Api-searches",
               routeTemplate: "api/{controller}/{offset}/{records}/{category}"
               );

        RouteTable.Routes.MapHttpRoute(
               name: "Api-session",
               routeTemplate: "api/{controller}/{session}"
               );

        routes.MapPageRoute(
                "Logout",                  // Route name
                "Logout",         // URL 
                "~/Logout.aspx", false,
                    new RouteValueDictionary { });

#if MULTI_LANGUAGE
        if (isMultilingual || CMSHelper.AlwaysWithEN)
        {


            routes.MapPageRoute(
                   "Default",                  // Route name
                   "{seo}/{id}",         // URL with parameters
                   "~/Default.aspx", false,
                       new RouteValueDictionary {   // Parameter defaults                           
                            { "seo", "Home" },
                            { "id", System.Web.Http.RouteParameter.Optional }
                       }
                   );

            routes.MapPageRoute(
                "Localization",                  // Route name
                "{lang}/{seo}",         // URL with parameters
                "~/Default.aspx", false,
                    new RouteValueDictionary {   // Parameter defaults
                            { "lang", "en" },
                            { "seo", "Home" }
                    }
                );

        }
        else
#endif

        {

#if TEXT_ANGULAR
            //routes.MapPageRoute(
            //"SubmitEvent",                  // Route name
            //   "SubmitEvent",         // URL with parameters
            //   "~/SubmitArticle.aspx", false,
            //       new RouteValueDictionary {   // Parameter defaults
            //                { "seo", "SubmitEvent" }
            //       }
            //   );

            //routes.MapPageRoute(
            //"SubmitArticle",                  // Route name
            //   "SubmitArticle",         // URL with parameters
            //   "~/SubmitArticle.aspx", false,
            //       new RouteValueDictionary {   // Parameter defaults
            //                { "seo", "SubmitArticle" }
            //       }
            //   );
#endif

            routes.MapPageRoute(
                "French",                  // Route name
                "fr/{seo}",         // URL with parameters
                "~/Default.aspx", false,
                    new RouteValueDictionary {   // Parameter defaults
                            { "seo", "Home" }
                    }
                );
            routes.MapPageRoute(
                "English",                  // Route name
                "en/{seo}",         // URL with parameters
                "~/Default.aspx", false,
                    new RouteValueDictionary {   // Parameter defaults
                            { "seo", "Home" }
                    }
                );

            routes.MapPageRoute(
                    "Default",                  // Route name
                    "{seo}/{id}",         // URL with parameters
                    "~/Default.aspx", false,
                        new RouteValueDictionary {   // Parameter defaults                           
                            { "seo", "Home" },
                            { "id", System.Web.Http.RouteParameter.Optional }
                        }
                    );
        }
    }

    public static bool isMultilingual
    {
        get
        {
            bool isMultilingual = false;

            string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
            string commandString = " select cast(case when count(id) > 1 then 1 else 0 end as bit) from languages where enabled = 1 ";

            using (SqlConnection connection = new SqlConnection(strConnectionString))
            {
                SqlCommand cmd = new SqlCommand(commandString, connection);

                connection.Open();
                isMultilingual = Convert.ToBoolean(cmd.ExecuteScalar());
                connection.Close();
            }

            return isMultilingual;
        }
    }

    public static string GetLanguagePrefix(string langid)
    {
        string prefix = CMSHelper.SeoPrefixEN;

        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = " select prefix from languages where id = @id and enabled = 1 ";

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@id", langid);

            connection.Open();
            prefix = Convert.ToString(cmd.ExecuteScalar());
            connection.Close();

        }
        return prefix;
    }

}
