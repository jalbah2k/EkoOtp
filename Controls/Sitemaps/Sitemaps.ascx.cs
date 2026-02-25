using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;


public partial class Controls_Sitemaps_Sitemaps : System.Web.UI.UserControl
{
    public DataSet ds = new DataSet();

	private string param;
	public Controls_Sitemaps_Sitemaps()
	{
		
	}

    public Controls_Sitemaps_Sitemaps(string s)
	{
        param = s;
	}

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]))
            {
                //string sqlcmd = "select * from menus where id<>1";
                //string sqlcmd = "select id from menus where orientation = 'Vertical' and id > 57 and id <> 76";
                string sqlcmd = @"select m.id from menus m 
                                    inner join (select distinct mi.menuid from MenuItems mi inner join Menu_Group mg on mi.id=mg.MenuItem_id inner join Groups g on mg.Group_id=g.id where mi.parentid=0 and g.private=0) t on m.id=t.menuid
                                    where m.orientation = 'Vertical' and m.name <> 'Left Menu' and m.language = @language";
                SqlDataAdapter dapt = new SqlDataAdapter(sqlcmd, conn);
                //dapt.SelectCommand.Parameters.AddWithValue("@language", CMSHelper.GetCleanQueryString("lang", "1"));
                dapt.SelectCommand.Parameters.AddWithValue("@language", CMSHelper.GetLanguageNumber());
                dapt.Fill(dt);
            }

            foreach (DataRow dr in dt.Rows)
            {
                //Response.Write(dr["id"].ToString() + "<br>");
                UserControl userControl = LoadControlExtension.LoadControl(this, "~/Controls/Sitemap/Sitemap.ascx", dr["id"].ToString());
                placeHolder.Controls.Add(userControl);
            }
        }
    }
}