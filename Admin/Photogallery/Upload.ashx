<%@ WebHandler Language="C#" Class="Upload" %>

using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Web.SessionState;
using System.IO;

public class Upload : IHttpHandler, IRequiresSessionState, IReadOnlySessionState
{

	protected string _connection = ConfigurationManager.AppSettings.Get("CMServer");
	
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        context.Response.Expires = -1;
        try
        {
            HttpPostedFile postedFile = context.Request.Files["Filedata"];
            
            string savepath = "";
            string tempPath = "";
            //tempPath = System.Configuration.ConfigurationManager.AppSettings["FolderPath"]; 
			tempPath = "/data/photos/" + HttpContext.Current.Session["docgroup"].ToString();
            savepath = context.Server.MapPath(tempPath);
            string filename = postedFile.FileName;
            if (!Directory.Exists(savepath))
                Directory.CreateDirectory(savepath);

            postedFile.SaveAs(savepath + @"\" + filename);


			
		SqlConnection sqlConn = new SqlConnection(_connection);
		SqlCommand sqlComm = new SqlCommand("insert into photos(name,filename,groupid,mime) values(@name,@filename,@groupid,@mime)", sqlConn);
		sqlComm.Parameters.AddWithValue("@name", postedFile.FileName.Split('.')[0]);
		sqlComm.Parameters.AddWithValue("@groupid", HttpContext.Current.Session["docgroup"].ToString());
		sqlComm.Parameters.AddWithValue("@filename", postedFile.FileName);
		sqlComm.Parameters.AddWithValue("@mime", System.IO.Path.GetExtension(postedFile.FileName));
		sqlConn.Open();
		sqlComm.ExecuteNonQuery();
		sqlConn.Close();


            context.Response.Write(tempPath + "/" + filename);
            context.Response.StatusCode = 200;

        }
        catch (Exception ex)
        {
            context.Response.Write("Error: " + ex.Message);
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }
}