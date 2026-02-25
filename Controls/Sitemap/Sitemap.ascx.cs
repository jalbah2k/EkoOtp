using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

public partial class Controls_Sitemap_Sitemap : System.Web.UI.UserControl
{
    public DataSet ds = new DataSet();

	private string param;
	public Controls_Sitemap_Sitemap()
	{
		
	}

	public Controls_Sitemap_Sitemap(string s)
	{
        param = s;
	}

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CreateMap(param);
        }
    }

    private void CreateMap(string menu)
    {
        string s = "";
        //string menu = param;
        SqlDataAdapter dapt =
            new SqlDataAdapter(
                "select menuitems.*,menus.menucss from menuitems,menus where menuitems.menuid=" + menu 
                + " and menus.id=menuitems.menuid order by parentid,priority" 
                + " select * from menus where id = " + menu,
                ConfigurationManager.AppSettings["CMServer"]);

        dapt.Fill(ds);
        //ds.Relations.Add("relate", ds.Tables[0].Columns["id"], ds.Tables[0].Columns["parentid"]);

        if (ds.Tables[1].Rows.Count > 0)
        {
            s = "<h3>" + ds.Tables[1].Rows[0]["name"].ToString() + "</h3>";
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr["parentid"].ToString() == "0")
                {
                    if (Session["LoggedInID"] != null || (bool)dr["enabled"])
                    {
                        //s += "<a href=\"" + dr["navigateurl"].ToString() + "\"><h2 style='display:inline;'>" + dr["text"].ToString() + "</h2></a>";
                        s += "<a href=\"" + dr["navigateurl"].ToString() + "\">" + dr["text"].ToString() + "</a>";
                        s += submap(dr, 1);

                        //Response.Write(dr["parentid"].ToString());
                    }
                }
            }
            litMap.Text = s + "<br><br>";
        }
    }

    private string submap(DataRow dr, int spaces)
    {
        //string s = "<div style='position:relative; top:-8px;'><ul>";
        string s = "<ul>";

        //foreach (DataRow cr in dr.GetChildRows("relate"))
        foreach (DataRow cr in ds.Tables[0].Rows)
        {
            if (cr["parentid"].ToString() == dr["id"].ToString())
                if (Session["LoggedInID"] != null || (bool)cr["visible"])
                {
                    for (int i = 0; i < spaces; i++)
                    {
                        s += "";
                    }
                    //s += "<li><a href=\"" + cr["navigateurl"].ToString() + "\"><h3 style='display:inline;'>" + cr["text"].ToString() + "</h3></a></li>";
                    //s += "<li><a href=\"" + cr["navigateurl"].ToString() + "\">" + cr["text"].ToString() + "</a></li>";
                    s += "<li><a href=\"" + cr["navigateurl"].ToString() + "\">" + cr["text"].ToString() + "</a>" + submap(cr, spaces + 1) + "</li>";
                }
        }
        //s += "</ul></div>";
        s += "</ul>";

        return s;
    }
}

