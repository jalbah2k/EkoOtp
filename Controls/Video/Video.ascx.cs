using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Video : System.Web.UI.UserControl
{
    public string _parameter;
    public Video() { }
    public Video(string param) { _parameter = param; }
    protected void Page_Load(object sender, EventArgs e)
    {
        GetJavaScriptCode();
        if(!IsPostBack)
        {
            BuildVideo();
            BindItems();
        }
    }

    private void GetJavaScriptCode()
    {
        using (StreamReader reader = new StreamReader(Server.MapPath("~/Controls/VideoComments/video.js")))
        {
            string script = reader.ReadToEnd();
            script = script.Replace("video_id != _parameter", "video_id != " + _parameter);
            script = script.Replace("_parameter", "_" + _parameter);

            litScript.Text = "<script>" + script + "</script>";
            reader.Close();
        }
    }

    private void BindItems()
    {
        UserControl userControl = LoadControlExtension.LoadControl(this, "~/Controls/VideoComments/VideoComments.ascx", _parameter);
        if (userControl != null)
        {
            placeHolder1.Controls.Add(userControl);
            ((VideoComments)userControl).Build(false, 0);
        }
    }

    private void BuildVideo()
    {
        DataSet ds = new DataSet();
        string sqlstr = @" select * from video.Videos where id=@id and enabled=1;
                            select * from video.Files where VideoId=@id ";
        SqlCommand sq = new SqlCommand(sqlstr);
        sq.Parameters.AddWithValue("@id", _parameter);

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("CMServer")))
        {
            ds = getDataset(sq, conn);
            DataTable dt = ds.Tables[0];
        
            if (dt.Rows.Count > 0)
            {
                DataRow rw = dt.Rows[0];
                if(rw["Name"].ToString() != "")
                litTiltle.Text = "<h2>" + rw["Name"].ToString() + "</h2>";

                dt = ds.Tables[1];
                if (dt.Rows.Count > 0)
                {
                    string source = "";
                    string trackfile = "";
                    string width = "100%";
                    string height = "auto";
                    string mime = "";

                    DataRow[] drs = dt.Select("MIMEType='video/mp4'");
                    if (drs.Length > 0)
                    {
                        source = drs[0]["path"].ToString() + drs[0]["FileName"].ToString();
                        source = source.Replace("//", "/");
                        mime = drs[0]["MIMEType"].ToString();
                    }

                    drs = dt.Select("FileExt='.vtt'");
                    if (drs.Length > 0)
                    {
                        trackfile = drs[0]["path"].ToString() + drs[0]["FileName"].ToString();
                        trackfile = trackfile.Replace("//", "/");
                    }

                    litVideo.Text = String.Format("<div class='row row-video'><video poster=\"{0}\" id=\"video_{1}\" width=\"{4}\" height=\"{5}\" controls><source src=\"{2}\" type=\"{3}\">{6}</video></div>",
                                       "", 
                                       "video_" + rw["id"].ToString(), 
                                       source, 
                                       mime, 
                                       width, 
                                       height,
                                       String.Format("<track default kind=\"subtitles\" srclang=\"en\" src=\"{0}\" />",trackfile)
                                    );
                }
            }
        }
    }

    private DataSet getDataset(SqlCommand cmd, SqlConnection _conn)
    {
        DataSet dt = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.SelectCommand.CommandType = CommandType.Text;
        da.SelectCommand.Connection = _conn;
        da.Fill(dt);
        return dt;
    }

}