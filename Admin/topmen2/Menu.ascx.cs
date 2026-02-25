using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Menu_Menu : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
		

		
		if(!IsPostBack)
		{
			bind();

		}

   
        //if (Session["Multilingual"] == null)
        //{
        SqlDataAdapter daptLang = new SqlDataAdapter("select * from languages", ConfigurationManager.AppSettings["CMServer"]);
        daptLang.SelectCommand.CommandType = CommandType.Text;
        DataSet dsLang = new DataSet();
        daptLang.Fill(dsLang);
        if (dsLang.Tables[0].Rows.Count > 1)
            Session.Add("Multilingual", true);
        else
            Session.Add("Multilingual", false);
        //}

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

            //lblControlName.Text = ds.Tables[0].Rows[0]["name"].ToString();

            //pnlControl.Controls.Add(LoadControl("~/admin/" + ds.Tables[0].Rows[0]["control"].ToString()));
        }
        else
        {
            //pnlControl.Controls.Add(LoadControl("~/admin/dash/dash.ascx"));
        }

        //SqlDataAdapter dapt = new SqlDataAdapter("");

        if (!IsPostBack)
        {
            GetMenu();
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        ScriptManager.RegisterClientScriptInclude(this, this.GetType(), "gAJS", ResolveClientUrl("/js/greybox/AJS.js"));
        ScriptManager.RegisterClientScriptInclude(this, this.GetType(), "gAJSfx", ResolveClientUrl("/js/greybox/AJS_fx.js"));
        ScriptManager.RegisterClientScriptInclude(this, this.GetType(), "gScripts", ResolveClientUrl("/js/greybox/gb_scripts.js"));

        base.OnPreRender(e);
    }

    private void GetMenu()
    {
        SqlDataAdapter dapt;

        if (Session["LoggedInId"].ToString() != "1")
            dapt = new SqlDataAdapter("select * from vwAdminMenu where parentid is NULL or id in (select adminmenuid from adminmenu_groups where groupid in (select group_id from Users_Groups_Access where User_id=" + Session["LoggedInID"] + ")) order by parentid,priority", ConfigurationManager.AppSettings["CMServer"]);
        else
        {
            dapt = new SqlDataAdapter("select * from vwAdminMenu order by parentid,priority", ConfigurationManager.AppSettings["CMServer"]);
        }
        DataSet ds = new DataSet();

        dapt.Fill(ds);
        ds.Relations.Add("relate", ds.Tables[0].Columns["id"], ds.Tables[0].Columns["parentid"]);

        //TheMenu.Items.AddSpacer(33, "menu_left");

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            if (dr.IsNull("parentid"))
            {
                skmMenu.MenuItem item = CreateItem(dr);
                PopulateSubMenu(dr, item);
                //TheMenu.Items.Add(item);
                //TheMenu.Items.AddSpacer(33, "menu_spacer");
                //mnuMenu.Items.Add(newItem);

            }
        }

        //TheMenu.Items.RemoveAt(TheMenu.Items.Count-1);
        //TheMenu.Items.AddSpacer(33, "menu_right");

        //TheMenu.Layout = skmMenu.MenuLayout.Horizontal;
        //TheMenu.HighlightTopMenu = true;
       // TheMenu.EnableViewState = true;
       // TheMenu.MenuFadeDelay = 1;




    }

	private void bind()
	{
		SqlDataAdapter dapt = new SqlDataAdapter("select * from pages where id=" + Session["PageID"].ToString() + " select name from users where id=" + Session["LoggedInID"].ToString() + " select id,name from Groups where id in (select group_id from users_groups_access where user_id="+Session["LoggedInId"].ToString() + " and access_level>1) order by name select top 1 group_id from pages_group where page_id=" + Session["PageID"].ToString() + " select adminmenuid from adminmenu_groups where adminmenuid=13 and groupid in (select group_id from Users_Groups_Access where User_id=" + Session["LoggedInId"].ToString() + ") select * from Layouts where available='True' select * from users_groups_access where user_id=" + Session["LoggedInID"].ToString() + " and group_id in(select group_id from pages_group where page_id=" + Session["PageId"].ToString() +") and access_level>2", ConfigurationManager.AppSettings["CMServer"]);
		DataSet ds = new DataSet();
		dapt.Fill(ds);

		txtName.Text = ds.Tables[0].Rows[0]["name"].ToString();
		txtTitle.Text = ds.Tables[0].Rows[0]["title"].ToString();
		txtKeywords.Text = ds.Tables[0].Rows[0]["keywords"].ToString();
		txtDescription.Text = ds.Tables[0].Rows[0]["description"].ToString();
		txtSeo.Text = ds.Tables[0].Rows[0]["seo"].ToString();
		cbEnabled.Checked = bool.Parse(ds.Tables[0].Rows[0]["active"].ToString());

		if (cbEnabled.Checked)
		{
			litLeft.Text = "<img src=\"/images/lemonaid/partials/lemonaid_logo.jpg\" border=\"0\" class=\"togglebutton\" style=\"cursor:pointer;\" id=\"tb2\" runat=\"server\" />";
			//litRight.Text = "<span style=\"color:#99ffac;font-family:Arial;\"><strong>ENABLED</strong></span>";
		}
		else
		{
            litLeft.Text = "<img src=\"/images/lemonaid/partials/lemonaid_logo_2.jpg\" border=\"0\" class=\"togglebutton\" style=\"cursor:pointer;\" id=\"tb2\" runat=\"server\" />";
			//litRight.Text = "<span style=\"color:#5e0000;font-family:Arial;\"><strong>DISABLED</strong></span>";
		}

		litMessage.Text = "Welcome, " + ds.Tables[1].Rows[0]["name"].ToString();

		ddlGroup.DataSource = ds.Tables[2];
		ddlGroup.DataTextField = "name";
		ddlGroup.DataValueField = "id";
		ddlGroup.DataBind();

		try
		{
			ddlGroup.Items.FindByValue(ds.Tables[3].Rows[0][0].ToString()).Selected = true;
			if (Session["LoggedInID"].ToString() == "1")
				litMessage.Text = ddlGroup.SelectedItem.Text;
		}
		catch (Exception)
		{
			//if(Session["LoggedInID"].ToString()!=null)
			tbl.Visible = false;
			//tb2.Visible = false;
			tb1.Visible = false;
			//ddlGroup.Items.Add(ds.Tables[3].Rows[0][0].ToString());
			//ddlGroup.Items.FindByValue(ds.Tables[3].Rows[0][1].ToString()).Selected = true;
			//throw;
		}
		


		ddlLayout.DataSource = ds.Tables[5];
		ddlLayout.DataTextField = "name";
		ddlLayout.DataValueField = "id";
		ddlLayout.DataBind();

		ddlLayout.Items.FindByValue(ds.Tables[0].Rows[0]["layout"].ToString()).Selected = true;

		


		if(ds.Tables[4].Rows.Count > 0 || Session["LoggedInId"].ToString()=="1")
		{
			adminareabutton.Visible = true;
			wizardbutton.Visible = true;
		}

        if (ds.Tables[6].Rows.Count == 0)
        {
            cbEnabled.Enabled = false;
            ddlLayout.Enabled = false;
        }
	}

    	private void PopulateSubMenu(DataRow dr, skmMenu.MenuItem item)
	{
		foreach (DataRow cr in dr.GetChildRows("relate"))
		{
			skmMenu.MenuItem child = CreateItem(cr);
			item.SubItems.Add(child);
			PopulateSubMenu(cr, child);
		}
	}

	private skmMenu.MenuItem CreateItem(DataRow dbRow)
	{
		skmMenu.MenuItem item = new skmMenu.MenuItem();

	    item.SubMenuMode = "popup";

		if(dbRow["level"].ToString()=="1")
		{
			item.CssClass = "admin_menu";
			item.MouseDownCssClass = "admin_menu_over";
			item.MouseOverCssClass = "admin_menu_over";
			item.MouseUpCssClass = "admin_menu_over";
		}
		if(dbRow["level"].ToString()=="2")
		{
			item.CssClass = "admin_submenu";
			item.MouseDownCssClass = "admin_submenu_over";
			item.MouseOverCssClass = "admin_submenu_over";
			item.MouseUpCssClass = "admin_submenu_over";
		}

		item.Text = dbRow["text"].ToString();
	    item.Url = dbRow["navigateurl"].ToString();
        if (dbRow["text"].ToString() == "Mailer"){
            item.Target = "_blank";}
        else{
		item.Target = "_self";//"dbRow["target"].ToString();
        }
		item.ToolTip = "";//"dbRow["tooltip"].ToString();

		if (dbRow["icon"].ToString() != "")
		{
			
			item.LeftImage = "/images/lemonaid/menuicons/" + dbRow["icon"].ToString() + ".png";
			item.LeftImageAlign = ImageAlign.Left;
			Unit pad = new Unit(20);
			item.LeftImageRightPadding = pad;
		}

		if (dbRow["level"].ToString() == "1")
		{
			item.HorizontalAlign = HorizontalAlign.Left;
		}
		if (dbRow["level"].ToString() == "2")
		{
			item.HorizontalAlign = HorizontalAlign.Left;
			item.VerticalAlign = VerticalAlign.Middle;
		}

		//item.Enabled = bool.Parse(dbRow["enabled"].ToString());
		//item.Visible = bool.Parse(dbRow["visible"].ToString());
		
		return item;
	}

	public void ClickedSave(object o, EventArgs e)
	{
		string lang;
		if (Session["Language"].ToString() == "2")
			lang = "/fr/";
		else
		{
			lang = "/en/";
		}
		SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
		SqlCommand sqlComm = new SqlCommand("update pages set name=@name,layout=@layout,title=@title,keywords=@keywords,description=@description,seo=@seo,active=@active where id=" + Session["PageID"].ToString() + " update pages_group set group_id=@group_id where page_id in (select id from pages where linkid=(select linkid from pages where id=" + Session["PageID"].ToString() + ")) update menuitems set visible=@active,navigateurl=@seo2 where pageid=" + Session["PageID"].ToString() + " delete from menu_group where menuitem_id in (select id from menuitems where pageid in (select id from pages where linkid=(select linkid from pages where id=" + Session["PageID"].ToString() + "))) insert into menu_group(menuitem_id,group_id) (select id,@group_id from menuitems where pageid in (select id from pages where linkid=(select linkid from pages where id=" + Session["PageID"].ToString() + ")))",sqlConn);
		sqlComm.Parameters.AddWithValue("@name", txtName.Text);
		sqlComm.Parameters.AddWithValue("@layout", ddlLayout.SelectedValue);
		sqlComm.Parameters.AddWithValue("@title", txtTitle.Text);
		sqlComm.Parameters.AddWithValue("@keywords", txtKeywords.Text);
		sqlComm.Parameters.AddWithValue("@description", txtDescription.Text);
		sqlComm.Parameters.AddWithValue("@seo", txtSeo.Text);
		sqlComm.Parameters.AddWithValue("@group_id", Convert.ToInt32(ddlGroup.SelectedValue));
		sqlComm.Parameters.AddWithValue("@active", cbEnabled.Checked);
		sqlComm.Parameters.AddWithValue("@seo2", lang+txtSeo.Text);
		sqlConn.Open();
		sqlComm.ExecuteNonQuery();
		sqlConn.Close();

		Response.Redirect(lang + txtSeo.Text);
		
		//Page.ClientScript.RegisterClientScriptBlock(GetType(),"closeScript","<script language=\"javascript\">window.parent.parent.reload()</script>");
	}

    public void ClickAdminArea(object sender, EventArgs e)
    {
        Response.Redirect("/admin.aspx?c=dash"); 
    }

	public void Logout(object sender, EventArgs e)
	{
		Session.Remove("LoggedInID");
		Response.Redirect("/");
	}
}
