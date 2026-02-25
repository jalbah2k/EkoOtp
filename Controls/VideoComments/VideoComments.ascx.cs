using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class VideoComments : System.Web.UI.UserControl
{
    public string _parameter;
    public VideoComments() { }
    public VideoComments(string p) { _parameter = p; }
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    public void Build(bool is_admin = true, int parentid = 0)
    {
        //https://www.webdevsplanet.com/post/jquery-not-working-on-dynamic-elements#google_vignette

        VideoComment_Search comments = new VideoComment_Search(Convert.ToInt32(_parameter), "video.Search_Comments", System.Data.CommandType.StoredProcedure, 1);
        string res = comments.GetHeader(is_admin, parentid);
        res += comments.GetResults(is_admin, parentid);
        litResults.Text += res;
    }
}