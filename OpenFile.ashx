<%@ WebHandler Language="C#" Class="OpenFile" %>

using System;
using System.Web;
using System.IO;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System.Web.SessionState;

public class OpenFile : IHttpHandler, IRequiresSessionState {


    public void ProcessRequest (HttpContext context) {

        UrlParameterWhitelistValidator validator = new UrlParameterWhitelistValidator();
        if (!validator.ValidateAndFilter("/OpenFile.ashx", context.Request.QueryString))
            throw new Exception("Invalid parameter.");

        //if (context.Session["MemberID"] == null && context.Session["LoggedInID"] == null)
        //    return;

        //if (context.Session["MemberID"] == null && context.Session["LoggedInID"] != null && context.Session["LoggedInID"].ToString() != "1")
        //    return;

        if ( context.Session["LoggedInID"] == null)
            return;

        int ResourceID = 0;
        if (context.Request.QueryString["id"] != null)
        {
            if (Int32.TryParse(context.Request.QueryString["id"], out ResourceID) == false)
            {
                throw new Exception("QueryString parameter must be an Integer value.");
            }
        }
        else
        {
            return;
        }

        List<SqlParameter> parms = new List<SqlParameter>();
        parms.Add(new SqlParameter("@ResourceId", ResourceID));
        parms.Add(new SqlParameter("@userid", context.Session["LoggedInID"].ToString()));
        DataTable dt = MyDAL_Resources.getSTable("Pie.Document_select", parms);

        if (dt.Rows.Count == 0)
            return;

        DataRow dr = dt.Rows[0];

        context.Response.ClearHeaders();
        context.Response.ClearContent();
        context.Response.Buffer = true;
        context.Response.Charset = "";
        context.Response.Cache.SetCacheability(HttpCacheability.NoCache);

        context.Response.ContentType = dt.Rows[0]["MIMEType"].ToString();            // "application/octet-stream";
        Byte[] bytes = (Byte[])dt.Rows[0]["content"];
        context.Response.BinaryWrite(bytes);
        context.Response.AddHeader("content-disposition", "inline; filename=" + dt.Rows[0]["FileName"].ToString());
        context.Response.Flush();
        context.Response.End();

    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}