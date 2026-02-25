using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MenuNoBlueBox : System.Web.UI.UserControl
{
    private void BindMenu()
    {
        SqlDataAdapter dapt = new SqlDataAdapter("select * from adminmenu where parentid is NULL and (visible is NULL or visible=1)  order by priority", ConfigurationManager.AppSettings["CMServer"]);
        DataSet ds = new DataSet();
        dapt.Fill(ds);

        repMenu.DataSource = ds.Tables[0];
        repMenu.DataBind();
    }

    public void BindMenuItem(object o, RepeaterItemEventArgs e)
    {
        Literal ll = (Literal)e.Item.FindControl("litLinks");
     
        
        DataSet ds = new DataSet();
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]))
        {
            string sqlcmd = " declare @temp Table(id int) ";
            sqlcmd += " insert into @temp select id from adminmenu where parentid=@parentid and (id in (select adminmenuid from users_adminmenu where userid=@userid) or 1=@userid) and (visible is NULL or visible=1)";
            sqlcmd += " select * from adminmenu where id in (select id from @temp) order by priority ";
            sqlcmd += " select * from adminmenu where parentid in (select id from @temp) and (id in (select adminmenuid from users_adminmenu where userid=@userid) or 1=@userid) order by priority ";
            SqlDataAdapter dapt = new SqlDataAdapter(sqlcmd, conn);
            dapt.SelectCommand.Parameters.AddWithValue("@parentid", ((DataRowView)e.Item.DataItem)["id"]);
            dapt.SelectCommand.Parameters.AddWithValue("@userid", Session["LoggedInID"].ToString());
            dapt.Fill(ds);
        }

        if (ds.Tables[0].Rows.Count > 0)
        {
            ll.Text = "<div class=\"sdt_box\">";
        }

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            ll.Text += "<div class=\"sdt_box_item\">";
            ll.Text += "<a href=\"" + dr["navigateurl"].ToString() + "\" target=\"_parent\" >" + "<img style=\"width:16px;height:16px;position:relative;left:0px;\" src=\"/images/lemonaid/menuicons/" + dr["icon"].ToString() + ".png\"/> " + dr["text"].ToString() + "</a>";

            DataRow[] drSub = ds.Tables[1].Select("parentid = " + dr["id"].ToString());
            if (drSub.Length > 0)
                ll.Text += "<div class=\"sdt_boxsub\">";
            foreach (DataRow drsub in drSub)
            {
                ll.Text += "<a href=\"" + drsub["navigateurl"].ToString() + "\">" + "<img style=\"width:16px;height:16px;position:relative;left:0px;\" src=\"/images/lemonaid/menuicons/" + drsub["icon"].ToString() + ".png\"/> " + drsub["text"].ToString() + "</a>";
            }
            if (drSub.Length > 0)
                ll.Text += "</div>";
            ll.Text += "</div>";
            
        }

        if (ds.Tables[0].Rows.Count > 0)
        {
            ll.Text += "</div>";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
		if(!IsPostBack)
		{
			//bind();
            BindMenu();
		}

        

        if (Session["LoggedInID"] == null)
        {
            Response.Redirect("/login.aspx?c=" + Request.QueryString["c"]);
        }

        if (Request.QueryString["c"] != null)
        {
            SqlDataAdapter dapt = new SqlDataAdapter("BASE_AdminControl_Get", ConfigurationManager.AppSettings["CMServer"]);
            dapt.SelectCommand.CommandType = CommandType.StoredProcedure;
            dapt.SelectCommand.Parameters.AddWithValue("@seo", Request.QueryString["c"]);

            DataSet ds = new DataSet();
            dapt.Fill(ds);

        }
      
     
    }

    protected override void OnPreRender(EventArgs e)
    {
        ScriptManager.RegisterClientScriptInclude(this, this.GetType(), "gAJS", ResolveClientUrl("/js/greybox/AJS.js"));
        ScriptManager.RegisterClientScriptInclude(this, this.GetType(), "gAJSfx", ResolveClientUrl("/js/greybox/AJS_fx.js"));
        ScriptManager.RegisterClientScriptInclude(this, this.GetType(), "gScripts", ResolveClientUrl("/js/greybox/gb_scripts.js"));

        base.OnPreRender(e);
    }


    public void ClickAdminArea(object sender, EventArgs e)
    {
        Response.Redirect("/admin.aspx?c=dash"); 
    }
}
