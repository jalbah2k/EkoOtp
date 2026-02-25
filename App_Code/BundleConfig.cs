using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

/// <summary>
/// Summary description for BundleConfig
/// </summary>
public class BundleConfig
{
    // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkID=303951

    //-----------------------------------------------------------------------------
    // https://benjaminray.com/codebase/css-files-and-bundling-in-asp-net-webforms/
    //this link applies only to CSS file. The definitions are done in Bundle.config
    //-----------------------------------------------------------------------------


    //On the other hand the following function applies to js files
    public static void RegisterBundles(BundleCollection bundles)
    {

        bundles.UseCdn = true;

        bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
           "~/Libraries/jquery/jquery-3.7.1.min.js"));
        bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
            "~/Libraries/jquery-ui-1.14.1/jquery-ui.min.js"));
        bundles.Add(new ScriptBundle("~/bundles/jqueryeasing").Include(
            "~/Libraries/jquery/jquery.easing.1.3.js"
            ));
        bundles.Add(new ScriptBundle("~/bundles/site").Include(
           "~/JS/layout/site.js",
           "~/JS/layout/menu.js"
           ));
        bundles.Add(new ScriptBundle("~/bundles/forum").Include(
          "~/Membership/Scripts/bootstrap.bundle.js",
          "~/Membership/Scripts/bootbox.js"
          ));
        bundles.Add(new ScriptBundle("~/bundles/fitvids").Include(
          "~/Libraries/FitVids.js-master/jquery.fitvids.js"
          ));
        bundles.Add(new ScriptBundle("~/bundles/backtotop").Include(
          "~/JS/fontdetect.min.js",
          "~/JS/jquery.topLink.js"
          ));
        bundles.Add(new ScriptBundle("~/bundles/slick").Include(
          "~/Libraries/slick-1.8.1/slick/slick.js"
          ));

    }
}
