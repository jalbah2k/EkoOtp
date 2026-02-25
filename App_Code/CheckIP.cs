using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CheckIP
/// </summary>
public static class CheckIP
{
    public static void ValidateIP()
    {
        //return;
        string Remote_Addr = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString().Trim();

        //if (!Remote_Addr.Contains("69.158.67.241")                 //Vivian
        //    && !Remote_Addr.Contains("216.8.160.210")               //???
        //    && !Remote_Addr.Contains("72.141.12.19")               //Juan
        //    && !Remote_Addr.Contains("70.38.11.199")               //main server   
        //    && !Remote_Addr.Contains("184.107.76.5")               //main server   
        //   )
        //{
        //    HttpContext.Current.Response.Redirect("https://erieshores.bluelemonmedia.com");
        //}
    }
}