<%@ WebHandler Language="C#" Class="Document" %>

using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.SessionState;

public class Document : IHttpHandler, IRequiresSessionState {

    public void ProcessRequest (HttpContext context){

        string fileName = "";
        int docid = 0;
        int userid = 0;

        if (context.Request.QueryString["id"] != null)
        {

            try { docid = int.Parse(context.Request.QueryString["id"]); }
            catch { context.Response.Write("<script>window.close();</script>"); return; }
        }
        else
        {
            context.Response.Write("<script>window.close();</script>");
            return;
        }

        SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);

        string sql = @"select d.* from documents d inner join DocumentGroups dg on d.groupid = dg.id
                            where d.id = @docid and dg.groupid in 
		                            (select id from groups where private = 0 
		                            or id in (select  Group_id from Users_Groups_Access where User_id = @userid)
		                            )";

        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter(sql, conn);
        da.SelectCommand.CommandType = CommandType.Text;
        da.SelectCommand.Parameters.AddWithValue("@docid", docid);

        if (context.Session["LoggedInID"] != null)
        {
            try { userid = int.Parse(context.Session["LoggedInID"].ToString()); }
            catch { }
        }

        da.SelectCommand.Parameters.AddWithValue("@userid", userid);
        da.Fill(dt);

        if (dt.Rows.Count == 1)
        {
            DataRow dr = dt.Rows[0];
            fileName = dr["filename"].ToString();
            string contenttype = "text/plain";

            switch (dr["mime"].ToString())
            {
                case ".pdf":
                    contenttype = "application/pdf";
                    break;

                case ".doc":
                case ".docx":
                    contenttype = "application/ms-word";
                    break;

                case ".xls":
                    contenttype = "application/vnd.xls";
                    break;

                default:
                    contenttype = "application/octet-stream";
                    break;
            }

            System.Web.HttpResponse response = context.Response;
            response.ClearContent();
            response.Clear();
            response.ContentType = contenttype;
            response.AddHeader("Content-Disposition",
                               "attachment; filename=\"" + fileName + "\";");
            try { response.TransmitFile(context.Server.MapPath("~/Documents/" + dr["groupid"].ToString() + "/"  + fileName)); }
            catch { return; }

            if (response.IsClientConnected)
                response.Flush();
            response.End();
        }
        else
        {
            //context.Response.ContentType = "text/plain";
            //context.Response.Write("");
            context.Response.Write("<script>window.close();</script>");

        }

    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}