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
      /*  //SqlDataAdapter dapt = new SqlDataAdapter("select * from adminmenu where parentid=" + ((DataRowView)e.Item.DataItem)["id"] + " order by priority", ConfigurationManager.AppSettings["CMServer"]);
        SqlDataAdapter dapt = new SqlDataAdapter("select * from adminmenu where parentid=" + ((DataRowView)e.Item.DataItem)["id"] +
                " and (id in (select adminmenuid from users_adminmenu where userid=" + Session["LoggedInID"].ToString() + ") or 1=" +
                (Session["LoggedInID"].ToString()) + ") order by priority", ConfigurationManager.AppSettings["CMServer"]);
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
            //ll.Text += "<a href=\"" + dr["navigateurl"].ToString() + "\">" + "<img style=\"width:16px;height:16px;position:relative;left:0px;\" src=\"/images/lemonaid/menuicons/" + dr["icon"].ToString() + ".png\"/> " + dr["text"].ToString() + "</a>";
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

    protected void Page_Load(object sender, EventArgs e)
    {
		if(!IsPostBack)
		{
			bind();
            BindMenu();
		}

        ////if (Session["Multilingual"] == null)
        ////{
        //SqlDataAdapter daptLang = new SqlDataAdapter("select * from languages", ConfigurationManager.AppSettings["CMServer"]);
        //daptLang.SelectCommand.CommandType = CommandType.Text;
        //DataSet dsLang = new DataSet();
        //daptLang.Fill(dsLang);
        //if (dsLang.Tables[0].Rows.Count > 1)
        //    Session.Add("Multilingual", true);
        //else
        //    Session.Add("Multilingual", false);
        ////}

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

        //if (!IsPostBack)
        //{
        //    GetMenu();
        //}
    }

    protected override void OnPreRender(EventArgs e)
    {
        ScriptManager.RegisterClientScriptInclude(this, this.GetType(), "gAJS", ResolveClientUrl("/js/greybox/AJS.js"));
        ScriptManager.RegisterClientScriptInclude(this, this.GetType(), "gAJSfx", ResolveClientUrl("/js/greybox/AJS_fx.js"));
        ScriptManager.RegisterClientScriptInclude(this, this.GetType(), "gScripts", ResolveClientUrl("/js/greybox/gb_scripts.js"));

        base.OnPreRender(e);
    }

    //private void GetMenu()
    //{
    //    SqlDataAdapter dapt;

    //    if (Session["LoggedInId"].ToString() != "1")
    //        dapt = new SqlDataAdapter("select * from adminmenu where parentid is NULL or id in (select adminmenuid from adminmenu_groups where groupid in (select group_id from Users_Groups_Access where User_id=" + Session["LoggedInID"] + ")) order by parentid,priority", ConfigurationManager.AppSettings["CMServer"]);
    //    else
    //    {
    //        dapt = new SqlDataAdapter("select * from adminmenu order by parentid,priority", ConfigurationManager.AppSettings["CMServer"]);
    //    }
    //    DataSet ds = new DataSet();

    //    dapt.Fill(ds);
    //    ds.Relations.Add("relate", ds.Tables[0].Columns["id"], ds.Tables[0].Columns["parentid"]);

    //    //TheMenu.Items.AddSpacer(33, "menu_left");

    //    foreach (DataRow dr in ds.Tables[0].Rows)
    //    {
    //        if (dr.IsNull("parentid"))
    //        {
    //            skmMenu.MenuItem item = CreateItem(dr);
    //            PopulateSubMenu(dr, item);
    //            //TheMenu.Items.Add(item);
    //            //TheMenu.Items.AddSpacer(33, "menu_spacer");
    //            //mnuMenu.Items.Add(newItem);

    //        }
    //    }

    //    //TheMenu.Items.RemoveAt(TheMenu.Items.Count-1);
    //    //TheMenu.Items.AddSpacer(33, "menu_right");

    //    //TheMenu.Layout = skmMenu.MenuLayout.Horizontal;
    //    //TheMenu.HighlightTopMenu = true;
    //   // TheMenu.EnableViewState = true;
    //   // TheMenu.MenuFadeDelay = 1;
    //}

	private void bind()
	{
        SqlDataAdapter dapt = new SqlDataAdapter("select *, isnull(IsReviewed, 0) Reviewed from pages where id=" + Session["PageID"].ToString() + 
            " select name,reviewer from users where id=" + Session["LoggedInID"].ToString() + 
            " select id,name from Groups where id in (select group_id from users_groups_access where user_id=" + Session["LoggedInId"].ToString() + " and access_level>1)  or 1= " +
            Session["LoggedInId"].ToString() + " order by name " +
            " select top 1 group_id from pages_group where page_id=" + Session["PageID"].ToString() + " select adminmenuid from adminmenu_groups where adminmenuid=13 and groupid in (select group_id from Users_Groups_Access where User_id=" + Session["LoggedInId"].ToString() + ") select * from Layouts where available='True' select * from users_groups_access where user_id=" + Session["LoggedInID"].ToString() + " and group_id in(select group_id from pages_group where page_id=" + Session["PageId"].ToString() + ") and (access_level>2 or " + Session["LoggedInID"].ToString() + "=1)", ConfigurationManager.AppSettings["CMServer"]);
		DataSet ds = new DataSet();
		dapt.Fill(ds);

        //Response.Write("select *, isnull(IsReviewed, 0) Reviewed from pages where id=" + Session["PageID"].ToString() + " select name,reviewer from users where id=" + Session["LoggedInID"].ToString() + " select id,name from Groups where id in (select group_id from users_groups_access where user_id=" + Session["LoggedInId"].ToString() + " and access_level>1) order by name select top 1 group_id from pages_group where page_id=" + Session["PageID"].ToString() + " select adminmenuid from adminmenu_groups where adminmenuid=13 and groupid in (select group_id from Users_Groups_Access where User_id=" + Session["LoggedInId"].ToString() + ") select * from Layouts where available='True' select * from users_groups_access where user_id=" + Session["LoggedInID"].ToString() + " and group_id in(select group_id from pages_group where page_id=" + Session["PageId"].ToString() + ") and access_level>2");

		txtName.Text = ds.Tables[0].Rows[0]["name"].ToString();
        txtTitle.Text = ds.Tables[0].Rows[0]["title"].ToString().Trim();
        ViewState["Title"] = ds.Tables[0].Rows[0]["title"].ToString().Trim();
        txtKeywords.Text = ds.Tables[0].Rows[0]["keywords"].ToString();
		txtDescription.Text = ds.Tables[0].Rows[0]["description"].ToString();
		txtSeo.Text = ds.Tables[0].Rows[0]["seo"].ToString();
		cbEnabled.Checked = bool.Parse(ds.Tables[0].Rows[0]["active"].ToString());

        ddlBackgroundPosition.SelectedValue = ds.Tables[0].Rows[0]["BackgroundBannerPosition"].ToString();
        ddlBackgroundPosition2.SelectedValue = ds.Tables[0].Rows[0]["BackgroundBannerPosition_Horizontal"].ToString();
        
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

        if (trImage.Visible = ds.Tables[0].Rows[0]["image"].ToString() != "")
        {
            hylImage.Text = ds.Tables[0].Rows[0]["image"].ToString().Replace("/uploads/images/", "");
            hylImage.NavigateUrl = ds.Tables[0].Rows[0]["image"].ToString();
        }

        if (trBanner.Visible = ds.Tables[0].Rows[0]["ImageBanner"].ToString() != "")
        {
            hylBanner.Text = ds.Tables[0].Rows[0]["ImageBanner"].ToString().Replace("/uploads/banners/", "");
            hylBanner.NavigateUrl = ds.Tables[0].Rows[0]["ImageBanner"].ToString();
        }
        
        ddlGroup.DataSource = ds.Tables[2];
		ddlGroup.DataTextField = "name";
		ddlGroup.DataValueField = "id";
		ddlGroup.DataBind();

		try
		{
            ListItem li = ddlGroup.Items.FindByValue(ds.Tables[3].Rows[0][0].ToString());
            if (li != null)
            {
                li.Selected = true;
                if (Session["LoggedInID"].ToString() == "1")
                    litMessage.Text = ddlGroup.SelectedItem.Text;
            }
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

        //cbLeftMenu.SelectedValue = bool.Parse(ds.Tables[0].Rows[0]["InsideMenu"].ToString()) ? "1" : "0";
        cbLeftMenu.SelectedValue = ds.Tables[0].Rows[0]["InsideClass"].ToString();


        if (ds.Tables[4].Rows.Count > 0 || Session["LoggedInId"].ToString()=="1")
		{
			adminareabutton.Visible = true;
			wizardbutton.Visible = true;
		}

        if (ds.Tables[6].Rows.Count == 0)
        {
            cbEnabled.Enabled = false;
            ddlLayout.Enabled = false;
        }

        if(!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["StartDatePage"].ToString()))
            txtStartDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["StartDatePage"]).ToString("yyyy-MM-dd"); ;


        if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["EndDatePage"].ToString()))
            txtEndDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["EndDatePage"]).ToString("yyyy-MM-dd"); ;


        Load_Times();
        ddlStartTime.ClearSelection();
        try
        {
            //ddlStartTime.SelectedValue = Convert.ToDateTime(dt.Rows[0]["StartTime"]).ToString("H:mm");
            ddlStartTime.Items.FindByText(DateTime.Parse(ds.Tables[0].Rows[0]["StartTimePage"].ToString()).ToString("h:mm tt").ToUpper()).Selected = true;
        }
        catch { }

        ddlEndTime.ClearSelection();
        try
        {
            //ddlEndTime.SelectedValue = Convert.ToDateTime(dt.Rows[0]["EndTime"]).ToString("H:mm");
            ddlEndTime.Items.FindByText(DateTime.Parse(ds.Tables[0].Rows[0]["EndTimePage"].ToString()).ToString("h:mm tt").ToUpper()).Selected = true;
        }
        catch { }
 

        Load_DDLReviewers();
        ddlReviewer.SelectedValue = ds.Tables[0].Rows[0]["reviewer"].ToString();

        Load_DDLFrequencies();
        ddlFrequency.SelectedValue = ds.Tables[0].Rows[0]["ReviewFrequency"].ToString();

        ViewState["Reviewed"] = cbReviewed.Checked = bool.Parse(ds.Tables[0].Rows[0]["Reviewed"].ToString());

        _layout = ddlLayout.SelectedItem.Text.ToLower();

        if (!(_mIsReviewer = (bool.Parse(ds.Tables[1].Rows[0]["Reviewer"].ToString())) && (ds.Tables[0].Rows[0]["reviewer"].ToString() == Session["LoggedInId"].ToString()) 
            || Session["LoggedInId"].ToString() == "1"
            ))
            cbReviewed.Enabled = false;
	}

    private void Load_Times()
    {
        DateTime time = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
        DateTime end = time.AddDays(1); ;

        ddlStartTime.Items.Clear();
        ddlEndTime.Items.Clear();

        int Counter = 0;

        ddlStartTime.Items.Add(new ListItem("Select One", ""));
        ddlEndTime.Items.Add(new ListItem("Select One", ""));

        while (time < end && Counter < 97)
        {
            ddlStartTime.Items.Add(new ListItem(time.ToString("h:mm tt"), time.ToString("HH:mm")));
            ddlEndTime.Items.Add(new ListItem(time.ToString("h:mm tt"), time.ToString("HH:mm")));

            time = time.AddMinutes(15);

            Counter++;
        }
    }


    private string _layout;
    public string Layout
    {
        get { return _layout; }
    }

    private bool _mIsReviewer;
    public bool IsReviewer
    {
        get { return _mIsReviewer && Session["LoggedInId"].ToString() != "1"; }
    }

    private void Load_DDLFrequencies()
    {
        DataTable dt = new DataTable();
        dt = mGet_All_Frequencies();
        this.ddlFrequency.DataSource = dt;
        this.ddlFrequency.DataTextField = "name";
        this.ddlFrequency.DataValueField = "id";
        this.ddlFrequency.DataBind();
        this.ddlFrequency.Items.Insert(0, "");

    }

    private void Load_DDLReviewers()
    {
        DataTable dt = new DataTable();
        dt = mGet_All_Reviewers();
        this.ddlReviewer.DataSource = dt;
        this.ddlReviewer.DataTextField = "name";
        this.ddlReviewer.DataValueField = "id";
        this.ddlReviewer.DataBind();
        this.ddlReviewer.Items.Insert(0, "");
    }

    public DataTable mGet_All_Reviewers()
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        //string commandString = "select *, case when name <> '' and name is not null then username + ' (' + name + ')' else username end as fullname from users where reviewer=1 and status='active' order by name";
        string commandString = "select * from users where reviewer=1 and status='active' order by name";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds, "table1");
        }

        return ds.Tables[0];
    }

    public DataTable mGet_All_Frequencies()
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = "select * from PageReviewFrequencies order by Priority";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds, "table1");
        }

        return ds.Tables[0];
    }


 //   	private void PopulateSubMenu(DataRow dr, skmMenu.MenuItem item)
	//{
	//	foreach (DataRow cr in dr.GetChildRows("relate"))
	//	{
	//		skmMenu.MenuItem child = CreateItem(cr);
	//		item.SubItems.Add(child);
	//		PopulateSubMenu(cr, child);
	//	}
	//}

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
        //string lang;
        //if (Session["Language"].ToString() == "2")
        //	lang = "/fr/";
        //else
        //{
        //	lang = "/en/";
        //}
        string lang = CMSHelper.GetLanguagePrefix();

        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);

        if (ViewState["Title"].ToString() != txtTitle.Text.Trim())
        {
            SqlDataAdapter dapt2 = new SqlDataAdapter("select id from pages where title=@title", sqlConn);
            dapt2.SelectCommand.Parameters.AddWithValue("@title", txtTitle.Text);

            DataSet ds2 = new DataSet();
            dapt2.Fill(ds2);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                txtTitle.Text = ViewState["Title"].ToString();
                this.Page.ClientScript.RegisterStartupScript(GetType(), "asdtitle", "alert('A page with this Title already exists');", true);
                return;
            }
        }

        string sql = @"update pages set name=@name,layout=@layout,InsideClass=@InsideClass,title=@title,

                            image=isnull(@image, image),imagename=isnull(@imagename, imagename), 
                            ImageBanner=isnull(@banner, ImageBanner), ImageBannerName=isnull(@bannername, ImageBannerName), 
                            BackgroundBannerPosition=@bannerpos, 
                            BackgroundBannerPosition_Horizontal=@bannerpos_hor, 

                            keywords=@keywords,description=@description,

                            seo=seo /*@seo*/,   
                            active=@active, 

                            StartDatePage=@StartDatePage, EndDatePage=@EndDatePage, StartTimePage=@StartTime, EndTimePage=@EndTime, 
                            Reviewer=@Reviewer, ReviewFrequency=@ReviewFrequency, IsReviewed=@IsReviewed, 
                            NextReviewDate=dbo.fnGetPageNextReviewDate(@PageID, @ReviewFrequency, null) 
                        where id=@PageID;

                        update pages_group set group_id=@group_id 
                        where page_id in (select id from pages where linkid=(select linkid from pages where id=@PageID));

                        update menuitems set visible=@active, enabled=@active, navigateurl=navigateurl /*@seo2*/ where pageid=@PageID;
    
                        delete from menu_group where menuitem_id in (select id from menuitems where pageid in (select id from pages where linkid=(select linkid from pages where id=@PageID )));
            
                        insert into menu_group(menuitem_id,group_id) 
                        (select id,@group_id from menuitems where pageid in (select id from pages where linkid=(select linkid from pages where id=@PageID)))";

        SqlCommand sqlComm = new SqlCommand();

        if (flUpload1.HasFile)
        {
            string imagename = flUpload1.FileName;
            int npos = imagename.LastIndexOf(".");
            if(npos > 0)
            {
                string extname = imagename.Substring(npos);

                string imagefile = "/uploads/images/" + Guid.NewGuid().ToString().Replace("-", "") + extname;
                flUpload1.SaveAs(Server.MapPath(imagefile));

                sqlComm.Parameters.AddWithValue("@image", imagefile);
                sqlComm.Parameters.AddWithValue("@imagename", imagename);
            }
            else
            {
                sqlComm.Parameters.AddWithValue("@image", DBNull.Value);
                sqlComm.Parameters.AddWithValue("@imagename", DBNull.Value);
            }
        }
        else
        {
            sqlComm.Parameters.AddWithValue("@image", DBNull.Value);
            sqlComm.Parameters.AddWithValue("@imagename", DBNull.Value);
        }

        if (flUploadBanner.HasFile)
        {
            string imagename = flUploadBanner.FileName;
            int npos = imagename.LastIndexOf(".");
            if (npos > 0)
            {
                string extname = imagename.Substring(npos);

                string imagefile = "/uploads/banners/" + Guid.NewGuid().ToString().Replace("-", "") + extname;
                flUploadBanner.SaveAs(Server.MapPath(imagefile));

                sqlComm.Parameters.AddWithValue("@banner", imagefile);
                sqlComm.Parameters.AddWithValue("@bannername", imagename);
            }
            else
            {
                sqlComm.Parameters.AddWithValue("@banner", DBNull.Value);
                sqlComm.Parameters.AddWithValue("@bannername", DBNull.Value);
            }
        }
        else
        {
            sqlComm.Parameters.AddWithValue("@banner", DBNull.Value);
            sqlComm.Parameters.AddWithValue("@bannername", DBNull.Value);
        }

        sqlComm.Parameters.AddWithValue("@bannerpos", ddlBackgroundPosition.SelectedValue);
        sqlComm.Parameters.AddWithValue("@bannerpos_hor", ddlBackgroundPosition2.SelectedValue);



        sqlComm.Connection = sqlConn;
        sqlComm.CommandText = sql;

        sqlComm.Parameters.AddWithValue("@PageID", Session["PageID"].ToString());
		sqlComm.Parameters.AddWithValue("@name", Server.HtmlDecode(QueryStringHelper.AntiXssEncoder_HtmlEncode(txtName.Text, true)));
		sqlComm.Parameters.AddWithValue("@layout", ddlLayout.SelectedValue);
		sqlComm.Parameters.AddWithValue("@title", Server.HtmlDecode(QueryStringHelper.AntiXssEncoder_HtmlEncode(txtTitle.Text, true)));
		sqlComm.Parameters.AddWithValue("@keywords", Server.HtmlDecode(QueryStringHelper.AntiXssEncoder_HtmlEncode(txtKeywords.Text, true)));
		sqlComm.Parameters.AddWithValue("@description", Server.HtmlDecode(QueryStringHelper.AntiXssEncoder_HtmlEncode(txtDescription.Text, true)));
        sqlComm.Parameters.AddWithValue("@group_id", Convert.ToInt32(ddlGroup.SelectedValue));
		sqlComm.Parameters.AddWithValue("@active", cbEnabled.Checked);
		sqlComm.Parameters.AddWithValue("@InsideClass", cbLeftMenu.SelectedValue);

        try
        {
           string t = Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd");
           sqlComm.Parameters.AddWithValue("@StartDatePage", t);
        }
        catch
        {
            sqlComm.Parameters.AddWithValue("@StartDatePage", DBNull.Value);
        }

        try
        {
            string t = Convert.ToDateTime(txtEndDate.Text).ToString("yyyy-MM-dd");
            sqlComm.Parameters.AddWithValue("@EndDatePage", t);
        }
        catch
        {
            sqlComm.Parameters.AddWithValue("@EndDatePage", DBNull.Value);
        }


        if(ddlStartTime.SelectedValue != "")
            sqlComm.Parameters.AddWithValue("@StartTime", ddlStartTime.SelectedValue);
        else
            sqlComm.Parameters.AddWithValue("@StartTime", DBNull.Value);

        if (ddlEndTime.SelectedValue != "")
            sqlComm.Parameters.AddWithValue("@EndTime", ddlEndTime.SelectedValue);
        else
            sqlComm.Parameters.AddWithValue("@EndTime", DBNull.Value);



        if (ddlReviewer.SelectedValue != "")
            sqlComm.Parameters.AddWithValue("@Reviewer", ddlReviewer.SelectedValue);
        else
            sqlComm.Parameters.AddWithValue("@Reviewer", DBNull.Value);

        if (ddlFrequency.SelectedValue != "")
            sqlComm.Parameters.AddWithValue("@ReviewFrequency", ddlFrequency.SelectedValue);
        else
            sqlComm.Parameters.AddWithValue("@ReviewFrequency", DBNull.Value);


        sqlComm.Parameters.AddWithValue("@IsReviewed", cbReviewed.Checked);

        if (!bool.Parse(ViewState["Reviewed"].ToString()) && cbReviewed.Checked)
        {
            sqlComm.CommandText += "update pages set NextReviewDate=dbo.fnGetNewReviewDate(@ReviewFrequency, null) where id=@PageID insert into PagesReviewed (UserId, PageId) values (@PageID, @UserId) update PagesNotificationForRevision set RevisionDate=getdate() where PageId=@PageId and UserId=@UserId and RevisionDate is null";
		    sqlComm.Parameters.AddWithValue("@UserId", Session["LoggedInID"].ToString());

        }

        //sqlComm.Parameters.AddWithValue("@seo2", lang + txtSeo.Text);
        
		sqlConn.Open();
		sqlComm.ExecuteNonQuery();
		sqlConn.Close();

        Response.Redirect(lang + txtSeo.Text);
        //Response.Redirect( txtSeo.Text);
		
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

    protected void btnDeleteImage_Click(object sender, EventArgs e)
    {
        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);

        SqlCommand sqlComm = new SqlCommand("update pages set image='',imagename='' where id=@PageID", sqlConn);
        sqlComm.Parameters.AddWithValue("@PageID", Session["PageID"].ToString());
        sqlConn.Open();
        sqlComm.ExecuteNonQuery();
        sqlConn.Close();

        hylImage.Text = "";
        hylImage.NavigateUrl = "";
        trImage.Visible = false;
    }

    protected void btnDeleteBanner_Click(object sender, EventArgs e)
    {
        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);

        SqlCommand sqlComm = new SqlCommand("update pages set ImageBanner='' where id=@PageID", sqlConn);
        sqlComm.Parameters.AddWithValue("@PageID", Session["PageID"].ToString());
        sqlConn.Open();
        sqlComm.ExecuteNonQuery();
        sqlConn.Close();

        hylBanner.Text = "";
        hylBanner.NavigateUrl = "";
        trBanner.Visible = false;
    }
}
