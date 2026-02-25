using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;

public partial class Admin : System.Web.UI.Page
{
    #region WebMethods

    [WebMethod]
    public static void AwardImages_Reorders(string Reorder)
    {
        string[] ListID = Reorder.Split('|');

        string sqlstr = string.Empty;

        for (int i = 0; i < ListID.Length; i++)
        {
            if (ListID[i] != "" && ListID[i] != null)
            {
                sqlstr += " update AwardImages set priority=" + (i + 1).ToString() + " where id=" + ListID[i];
            }
        }

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]))
        {
            SqlCommand cmd = new SqlCommand(sqlstr, conn);

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }

    [WebMethod]
    public static void WidgetImages_Reorders(string Reorder)
    {
        string[] ListID = Reorder.Split('|');

        string sqlstr = string.Empty;

        for (int i = 0; i < ListID.Length; i++)
        {
            if (ListID[i] != "" && ListID[i] != null)
            {
                sqlstr += " update WidgetImages set priority=" + (i + 1).ToString() + " where id=" + ListID[i];
            }
        }

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]))
        {
            SqlCommand cmd = new SqlCommand(sqlstr, conn);

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }

    [WebMethod]
    public static void QuickLinks_Reorders(string Reorder)
    {
        string[] ListID = Reorder.Split('|');

        string sqlstr = string.Empty;

        for (int i = 0; i < ListID.Length; i++)
        {
            if (ListID[i] != "" && ListID[i] != null)
            {
                sqlstr += " update QuickLinks set priority=" + (i + 1).ToString() + " where id=" + ListID[i];
            }
        }

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]))
        {
            SqlCommand cmd = new SqlCommand(sqlstr, conn);

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }

    [WebMethod]
    public static void PhotosReorders(string Reorder)
    {
        string[] ListID = Reorder.Split('|');

        string sqlstr = string.Empty;

        for (int i = 0; i < ListID.Length; i++)
        {
            if (ListID[i] != "" && ListID[i] != null)
            {
                sqlstr += " update Photos set priority=" + (i + 1).ToString() + " where id=" + ListID[i];
            }
        }

        if (ListID.Length > 0)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]))
            {
                SqlCommand cmd = new SqlCommand(sqlstr, conn);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
    }

    [WebMethod]
    public static void DocumentsReorders(string Reorder)
    {
        string[] ListID = Reorder.Split('|');

        string sqlstr = string.Empty;

        for (int i = 0; i < ListID.Length; i++)
        {
            if (ListID[i] != "" && ListID[i] != null)
            {
                sqlstr += " update Documents set priority=" + (i + 1).ToString() + " where id=" + ListID[i];
            }
        }

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]))
        {
            SqlCommand cmd = new SqlCommand(sqlstr, conn);

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }

    [WebMethod]
    public static void BannersReorders(string Reorder)
    {
        string[] ListID = Reorder.Split('|');

        string sqlstr = string.Empty;

        for (int i = 0; i < ListID.Length; i++)
        {
            if (ListID[i] != "" && ListID[i] != null)
            {
                sqlstr += " update Banners set BannerPriority=" + (i + 1).ToString() + " where BannerID=" + ListID[i];
            }
        }

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]))
        {
            SqlCommand cmd = new SqlCommand(sqlstr, conn);

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }

    [WebMethod]
    public static void LogosReorders(string Reorder)
    {
        string[] ListID = Reorder.Split('|');

        string sqlstr = string.Empty;

        for (int i = 0; i < ListID.Length; i++)
        {
            if (ListID[i] != "" && ListID[i] != null)
            {
                sqlstr += " update Logos set Priority=" + (i + 1).ToString() + " where id=" + ListID[i];
            }
        }

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]))
        {
            SqlCommand cmd = new SqlCommand(sqlstr, conn);

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }

    [WebMethod]
    public static string ValidateUsername(string username)
    {
        string result = "";
        string sqlstr = "select * from users where username=@user";

        SqlDataAdapter dapt = new SqlDataAdapter(sqlstr, ConfigurationManager.AppSettings["CMServer"].ToString());
        dapt.SelectCommand.Parameters.AddWithValue("@user", username);
        DataSet ds = new DataSet();

        dapt.Fill(ds);

        if (ds.Tables[0].Rows.Count > 0)
            result = "This username is already taken";

        return result;
    }

    //[WebMethod]
    //public static string ValidatePassword(string Password)
    //{
    //    string encryptedPassw = EncDec.DESEncrypt(Password);
    //    string result = PasswordRules.Validate ( encryptedPassw);

    //    if (result.IndexOf("\"") == 0)
    //        result = result.Substring(1);

    //    if (result.LastIndexOf("\"") == result.Length - 1)
    //        result = result.Substring(0, result.Length - 1);

    //     return result;
    //}

    [WebMethod]
    public static string ReordereFormFields(string Reorder)
    {
        string retVal = "";

        string[] ListID = Reorder.Split(',');

        string sqlstr = string.Empty;

        for (int i = 0; i < ListID.Length; i++)
        {
            if (ListID[i] != "" && ListID[i] != null)
            {
                sqlstr += " update eFormFields set priority=" + (i + 1).ToString() + " where id=" + ListID[i];
            }
        }

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]))
        {
            SqlCommand cmd = new SqlCommand(sqlstr, conn);

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (SqlException ex)
            {
                retVal = ex.Message;
            }
        }

        return retVal;
    }

    [WebMethod]
    public static void Table_ColumnsReorders(string Reorder)
    {
        string[] ListID = Reorder.Split('|');

        string sqlstr = string.Empty;

        for (int i = 0; i < ListID.Length; i++)
        {
            if (ListID[i] != "" && ListID[i] != null)
            {
                sqlstr += " update TableWidget.Columns set Priority=" + (i + 1).ToString() + " where id=" + ListID[i];
            }
        }

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]))
        {
            SqlCommand cmd = new SqlCommand(sqlstr, conn);

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }

    [WebMethod]
    public static void Table_RowsReorders(string Reorder)
    {
        string[] ListID = Reorder.Split('|');

        string sqlstr = string.Empty;

        for (int i = 0; i < ListID.Length; i++)
        {
            if (ListID[i] != "" && ListID[i] != null)
            {
                sqlstr += " update TableWidget.Rows set Priority=" + (i + 1).ToString() + " where id=" + ListID[i];
            }
        }

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]))
        {
            SqlCommand cmd = new SqlCommand(sqlstr, conn);

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
    #endregion WebMethods

    protected override void OnPreRender(EventArgs e)
	{
		ScriptManager.RegisterClientScriptInclude(this, this.GetType(), "gAJS", ResolveClientUrl("/js/greybox/AJS.js"));
		ScriptManager.RegisterClientScriptInclude(this, this.GetType(), "gAJSfx", ResolveClientUrl("/js/greybox/AJS_fx.js"));
		ScriptManager.RegisterClientScriptInclude(this, this.GetType(), "gScripts", ResolveClientUrl("/js/greybox/gb_scripts.js"));

		base.OnPreRender(e);
	}

    protected void Page_Load(object sender, EventArgs e)
    {
        UrlParameterWhitelistValidator validator = new UrlParameterWhitelistValidator();
        var filteredQueryString = validator.ValidateAndFilter(this);
        // check if user is still authenticated but session variable is expired
        CMSHelper.AutoLogin();

        //if (Session["Multilingual"] == null)
        //{
        //SqlDataAdapter daptLang = new SqlDataAdapter("select * from languages", ConfigurationManager.AppSettings["CMServer"]);
        SqlDataAdapter daptLang = new SqlDataAdapter("select * from languages where enabled = 1", ConfigurationManager.AppSettings["CMServer"]);
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
            //Response.Redirect("/login?c=" + Request.QueryString["c"]);
            Response.Redirect(String.Format("/Membership/Account/Login?ap=cms&g=admin&t={0}{1}", Request.QueryString["c"], 
                !String.IsNullOrEmpty(Request.QueryString.ToString()) ? "&q=" + Request.QueryString.ToString() : ""));

        }

        string seo = "dash";
        if (Request.QueryString["c"] != null)
            seo = Request.QueryString["c"];

        SqlDataAdapter dapt = new SqlDataAdapter("BASE_AdminControl_Get", ConfigurationManager.AppSettings["CMServer"]);
		dapt.SelectCommand.CommandType = CommandType.StoredProcedure;
		dapt.SelectCommand.Parameters.AddWithValue("@seo", seo);
		dapt.SelectCommand.Parameters.AddWithValue("@userid", Session["LoggedInID"].ToString());

		DataSet ds = new DataSet();
		dapt.Fill(ds);

		//lblControlName.Text = ds.Tables[0].Rows[0]["name"].ToString();

        if(ds.Tables[1].Rows.Count > 0 
            || Session["LoggedInID"].ToString() == "1"
            )
        {
            pnlControl.Controls.Add(LoadControl("~/admin/" + ds.Tables[0].Rows[0]["control"].ToString()));
        }
        else
            Response.Redirect("/login?c=" + Request.QueryString["c"]);

			
		if (!IsPostBack)
		{
			//GetMenu();  //Old Menu
            BindMenu(); //New Menu
		}
    }

    private void BindMenu()
    {
        SqlDataAdapter dapt = new SqlDataAdapter("select * from adminmenu where (visible is NULL or visible=1) and (parentid is NULL) order by parentid,priority", ConfigurationManager.AppSettings["CMServer"]);
        DataSet ds = new DataSet();
        dapt.Fill(ds);

        repMenu.DataSource = ds.Tables[0];
        repMenu.DataBind();
    }

    public void BindMenuItem(object o, RepeaterItemEventArgs e)
    {
        Literal ll = (Literal)e.Item.FindControl("litLinks");
       /* SqlDataAdapter dapt;
        if(Session["LoggedInId"].ToString()!="1")
        dapt = new SqlDataAdapter("select * from adminmenu where parentid=" + ((DataRowView)e.Item.DataItem)["id"] + " and id in (select adminmenuid from users_adminmenu where userid=" + Session["LoggedInID"] + ") order by priority", ConfigurationManager.AppSettings["CMServer"]);
        else
        dapt = new SqlDataAdapter("select * from adminmenu where parentid=" + ((DataRowView)e.Item.DataItem)["id"] + " order by priority", ConfigurationManager.AppSettings["CMServer"]);
        DataSet ds = new DataSet();
        dapt.Fill(ds);*/
        
        
        DataSet ds = new DataSet();
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]))
        {
            //string sqlcmd += " select * from adminmenu where parentid=@parentid and (id in (select adminmenuid from users_adminmenu where userid=@userid) or 1=@userid) order by priority ";
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
         //   ll.Text += "<a href=\"" + dr["navigateurl"].ToString() + "\">" + "<img style=\"width:16px;height:16px;position:relative;left:0px;\" src=\"/images/lemonaid/menuicons/" + dr["icon"].ToString() + ".png\"/> " + dr["text"].ToString() + "</a>";
         
          ll.Text += "<div class=\"sdt_box_item\">";
            ll.Text += "<a href=\"" + dr["navigateurl"].ToString() + "\">" + "<img style=\"width:16px;height:16px;position:relative;left:0px;\" src=\"/images/lemonaid/menuicons/" + dr["icon"].ToString() + ".png\"/> " + dr["text"].ToString() + "</a>";

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

    private void GetMenu()
	{
		SqlDataAdapter dapt;

		if(Session["LoggedInId"].ToString()!="1")
			dapt = new SqlDataAdapter("select * from adminmenu where parentid is NULL or id in (select adminmenuid from users_adminmenu where userid=" + Session["LoggedInID"] + ") order by parentid,priority", ConfigurationManager.AppSettings["CMServer"]);
		else
		{
			dapt = new SqlDataAdapter("select * from adminmenu order by parentid,priority", ConfigurationManager.AppSettings["CMServer"]);
		}
		DataSet ds = new DataSet();

		dapt.Fill(ds);
		ds.Relations.Add("relate",ds.Tables[0].Columns["id"], ds.Tables[0].Columns["parentid"]);

		//TheMenu.Items.AddSpacer(33, "menu_left");

		foreach (DataRow dr in ds.Tables[0].Rows)
		{
			if (dr.IsNull("parentid"))
			{
				skmMenu.MenuItem item = CreateItem(dr);
				PopulateSubMenu(dr, item);
				TheMenu.Items.Add(item);
				//TheMenu.Items.AddSpacer(33, "menu_spacer");
				//mnuMenu.Items.Add(newItem);
				
			}
		}

		//TheMenu.Items.RemoveAt(TheMenu.Items.Count-1);
		//TheMenu.Items.AddSpacer(33, "menu_right");

    	TheMenu.Layout = skmMenu.MenuLayout.Horizontal;
        //TheMenu.HighlightTopMenu = true;
        TheMenu.EnableViewState = true;
        TheMenu.MenuFadeDelay = 1;

		
		
        
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
			
			item.LeftImage = "~/images/lemonaid/menuicons/" + dbRow["icon"].ToString() + ".png";
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

    #region MenuItems
     [WebMethod(true)]
    public static void DeleteNodes(string id)
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        //string commandString = "delete from MenuItems where id in (select id from fGetChildrenNodes(@id))";
        string commandString = "delete from MenuItems where linkid in (select id from fGetChildrenNodes(@id))";

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {

            //---------- Delete ------------
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@id", id);

            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
        }
    }

    [WebMethod(true)]
    public static string MoveNode(string parentid, string id)
    {
        string menuid = "menuid_";
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = "MenuItemMove";

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {

            //---------- Update ------------
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;

            if (parentid.Contains(menuid))
            {
                menuid = parentid.Substring(parentid.IndexOf("_") + 1);

                cmd.Parameters.AddWithValue("@menuid", menuid);
                cmd.Parameters.AddWithValue("@parentid", "0");
            }
            else
            {
                cmd.Parameters.AddWithValue("@parentid", parentid);
            }

            cmd.CommandText = commandString;
            cmd.Parameters.AddWithValue("@id", id);
            cmd.CommandType = CommandType.StoredProcedure;

            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
            //----------------------------------

            //the following loop it's not necessary anymore becuase all children are updated in the stored procedure, called just one time
            
            //////---------Loop --------------------
            //////to update menuid in children
            ////DataTable dt = new DataTable();
            ////dt = mGet_MenuItems_ByMenuid(int.Parse(id));
            ////foreach (DataRow dr in dt.Rows)
            ////{
            ////    MoveNode(id, dr["id"].ToString());
            ////}

        }

        return commandString;
    }
    #region DAL
    public static DataTable mGet_MenuItems_ByMenuid(int ID)
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = "select id from Menuitems where parentid=@ID";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@ID", ID);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            try
            {
                da.Fill(ds, "table1");
            }
            catch (Exception ex)
            {
                string kk = ex.Message;
            }
        }

        return ds.Tables[0];

    }
    #endregion
    #endregion

}
