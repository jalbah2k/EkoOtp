using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.IO;

public partial class Admin_Groups_Groups : System.Web.UI.UserControl
{
    public string sortExp
    {
        get
        {
            return ViewState["sortExp"] != null ? ViewState["sortExp"].ToString() : "";
        }
        set
        {
            ViewState["sortExp"] = value;
        }
    }
    public string sortOrder
    {
        get
        {
            return ViewState["sortOrder"] != null ? ViewState["sortOrder"].ToString() : "desc";
        }
        set
        {
            ViewState["sortOrder"] = value;
        }
    }

    public int Group
    {
        set { ViewState["Group"] = value; }
        get
        {
            if (ViewState["Group"] == null)
                return 0;
            else
                return Convert.ToInt32(ViewState["Group"]);
        }
    }

    public void PageSizeChange(object o, EventArgs e)
    {
        this.GV_Main.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
        pager1.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
        pager1.CurrentIndex = 1;
        this.GV_Main.PageIndex = 0;
        mBindData();
    }
    public void pager_Command(object sender, CommandEventArgs e)
	{
		int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
		pager1.CurrentIndex = currnetPageIndx;
		GV_Main.PageIndex = currnetPageIndx - 1;
        //BindRepeater();
        mBindData();
    }
    #region DAL

    private string QLImagesPath = "/data/QuickLinks/";
    StringBuilder sb;


    #region Template

    public DataTable mGet_Links()
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        //string commandString = "select *,l.name as language from quicklinks,languages l where quicklinks.lang_id=l.id and groupid=@id order by quicklinks.priority";
        string commandString = "select * from quicklinks where groupid=@id order by quicklinks.priority";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@id", Group);

            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds, "table1");
        }

        if (ds.Tables[0].Rows.Count < 1)
        {
            tbl_noresults.Visible = true;
            tbl_Grid.Visible = false;
        }
        else
        {
            tbl_noresults.Visible = false;
            tbl_Grid.Visible = true;
        }
        return ds.Tables[0];
    }



    public DataTable mGet_One_Group(int mID)
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = "select * from quicklinks where id = @mID order by name";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@mID", mID);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds, "table1");
        }

        return ds.Tables[0];



    }

    public void mAdd_Group(string name, string description, string color)
    {

        sb = sb.Remove(0, sb.Length);
        sb.Append(" insert into quicklinks(name, description, color) ");
        sb.Append(" values   (@name, @description, @color)");
        //sb.Append(" SELECT id FROM Template WHERE (id = SCOPE_IDENTITY()) ");
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@description", description);
            cmd.Parameters.AddWithValue("@color", color);
           

            connection.Open();
            cmd.ExecuteNonQuery();
         }

    }

    public void mDelete_Group(int mID)
    {
        sb = sb.Remove(0, sb.Length);
        sb.Append(" Delete from quicklinks ");
        sb.Append(" where id = @id ");

        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@id", mID);

            connection.Open();
            cmd.ExecuteNonQuery();
        }


    }

   
    #endregion

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        sb = new StringBuilder(60);
        this.lbl_msg.Text = "";
        if (!IsPostBack)
        {
            ViewState["action"] = "add";
            //sortExp = "name"; // default sorted column
            //sortOrder = "asc";    // default sort order

            SwitchView(pnlGroups);

            ViewState["record_id"] = "";

            mBindGrp();

            //to be delete
            Session["user_id"] = "-1";


			SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"].ToString());
			SqlDataAdapter dapt = new SqlDataAdapter("select * from languages", sqlConn);
			DataSet ds = new DataSet();
			dapt.Fill(ds);

			ddlLanguage.DataSource = ds.Tables[0];
			ddlLanguage.DataTextField = "name";
			ddlLanguage.DataValueField = "id";
			ddlLanguage.DataBind();

			getpages();

        }


		


    }

    private void mBindGrp()
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = "select *, c.id as contentid from QuickLinksGroups q inner join content c on q.id=c.param and control in ('QuickLinks', 'QuickLinkPhotos') order by q.name";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds, "table1");
        }

        GV_Group.DataSource = ds.Tables[0];
        GV_Group.DataBind();
    }

    public void onitembound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
	{
		CheckBox cb = (CheckBox) e.Item.FindControl("cbVisible");

	}


    #region My_Functions

    private void mBindData()
    {
        mBindData(sortExp, sortOrder);
    }
    private void mBindData(string sortExp, string sortDir)
    {
        DataTable dt = new DataTable();
        dt = mGet_Links();
        DataView DV = dt.DefaultView;
        //if (!(sortExp == string.Empty))
        //{
        //    DV.Sort = string.Format("{0} {1}", sortExp, sortDir);
        //}
        this.GV_Main.DataSource = DV;
        this.GV_Main.DataBind();

		pager1.ItemCount = DV.Count;
        pager1.Visible = dt.Rows.Count > GV_Main.PageSize;

        litPagerShowing.Text = CMSHelper.GetPagerInfo(GV_Main, DV.Count);

        SwitchView(pnlList);
    }

    void Clearfield()
    {

        txtNameGrp.Text = "";
        this.txt_Name.Text = "";
		txtUrl.Text = "";
        ddlLanguage.SelectedIndex = 0;
        ddlPages.ClearSelection();

        //this.tbl_add_edit.Visible = false;2
        //this.pnlList.Visible = true;
        ViewState["action"] = "add";

        imgCurrentImage.Visible = false;
        lnkDlete.Visible = true;

        imgCurrentImage2.Visible = false;
        lnkDlete2.Visible = true;

       
    }

    #endregion



    #region Grid_Events

    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //#region Add sorted class to headers
        //if (e.Row.RowType == DataControlRowType.Header)
        //{
        //    int colIndex = CMSHelper.GetColumnIndex((sender as GridView), sortExp);
        //    e.Row.Cells[colIndex].CssClass = sortOrder == "asc" ? "sortasc" : "sortdesc";
        //}
        //#endregion Add sorted class to headers

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //for delete button
            ImageButton lb;
            lb = (ImageButton)e.Row.FindControl("LB_Delete");
            lb.Attributes.Add("OnClick", "return confirm('Are you sure to delete this quicklink?');");

            DataRowView dr = (DataRowView)e.Row.DataItem;

            Literal litHfId = (Literal)e.Row.FindControl("litHfId");
            litHfId.Text = String.Format("<input type='hidden' class='HfId' value='{0}' />", dr["id"].ToString());
        }


    }

    protected void GV_Main_Sorting(object sender, GridViewSortEventArgs e)
    {
        sortExp = e.SortExpression;

        if (sortOrder == "desc")
            sortOrder = "asc";
        else
            sortOrder = "desc";
        mBindData(e.SortExpression, sortOrder);
    }


    protected void GV_Main_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        Int32 id = Convert.ToInt32(this.GV_Main.DataKeys[e.RowIndex].Value.ToString());

        //Delete Group
        mDelete_Group(id);


        mBindData();

        this.tbl_add_edit.Visible = false;
        this.pnlList.Visible = true;

        ViewState["action"] = "add";
    }

    protected void GV_Main_RowEditing(object sender, GridViewEditEventArgs e)
    {
        this.pnlList.Visible = false;
        this.tbl_add_edit.Visible = true;
      

        ViewState["action"] = "edit";

        Int32 id = Convert.ToInt32(this.GV_Main.DataKeys[e.NewEditIndex].Value.ToString());
        ViewState["record_id"] = id;

        DataTable dt = new DataTable();
        dt = mGet_One_Group(id);

        this.txt_Name.Text = dt.Rows[0]["name"].ToString();
		this.txtUrl.Text = dt.Rows[0]["url"].ToString();

        {
            ddlLanguage.ClearSelection();

            ListItem li = ddlLanguage.Items.FindByValue(dt.Rows[0]["lang_id"].ToString());
            if (li != null)
                li.Selected = true;
        }

        getpages();
        ddlPages.ClearSelection();

        //if (dt.Rows[0]["url"].ToString().Trim() == "")
        {
            try
            {
                ListItem li =  ddlPages.Items.FindByValue(dt.Rows[0]["pageid"].ToString());
                if (li != null)
                    li.Selected = true;
            }
            catch
            {
            }
        }

        rbNewWindow.SelectedValue = Convert.ToBoolean(dt.Rows[0]["newwindow"]) ? "1" : "0";

        lnkDlete.Visible = false;
        if (dt.Rows[0]["image"] != DBNull.Value && dt.Rows[0]["image"].ToString().Length > 0)
        {
            imgCurrentImage.ImageUrl = QLImagesPath + Group + "/" + dt.Rows[0]["image"].ToString();
            imgCurrentImage.Visible = true;
            lnkDlete.Visible = true;
        }

        lnkDlete2.Visible = false;
        if (dt.Rows[0]["image2"] != DBNull.Value && dt.Rows[0]["image2"].ToString().Length > 0)
        {
            imgCurrentImage2.ImageUrl = QLImagesPath + Group + "/" + dt.Rows[0]["image2"].ToString();
            imgCurrentImage2.Visible = true;
            lnkDlete2.Visible = true;
        }

        //SqlDataAdapter dapt = new SqlDataAdapter("declare @temp TABLE(id int, name varchar(500), visible bit) insert into @temp(id,name,visible) select id,[text],'False' from adminmenu where parentid is not NULL update @temp set visible='True' where id in (select adminmenuid from AdminMenu_Groups where groupid=" + id.ToString() + ") select * from @temp",ConfigurationManager.AppSettings["CMServer"].ToString());
        //DataSet ds = new DataSet();

        //dapt.Fill(ds);

        //dgMenu.DataSource = ds.Tables[0];
        //dgMenu.DataBind();
    }

    #endregion


    protected void bttn_Add_Question_Click(object sender, EventArgs e)
    {
        lnkDlete.Visible = false;
        imgCurrentImage.Visible = false;
        imgCurrentImage.ImageUrl = "";

        lnkDlete2.Visible = false;
        imgCurrentImage2.Visible = false;
        imgCurrentImage2.ImageUrl = "";

        rbNewWindow.SelectedValue = "0";

        this.tbl_add_edit.Visible = true;
        this.pnlList.Visible = false;
    }

    public void getpages(object o, EventArgs e)
    {
        getpages();
    }

    public void getpages()
    {
        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"].ToString());
        SqlDataAdapter dapt = new SqlDataAdapter("select id,name from pages where language=" + ddlLanguage.SelectedValue + " order by name",sqlConn);
        DataSet ds = new DataSet();
        dapt.Fill(ds);
        ddlPages.Items.Clear();
        ddlPages.DataSource = ds.Tables[0];
        ddlPages.DataTextField = "name";
        ddlPages.DataValueField = "id";
        ddlPages.DataBind();

        ddlPages.Items.Insert(0, new ListItem("",""));
    }

    
    protected void btn_Submit_Click(object sender, EventArgs e)
    {
        string id = "0";
        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"].ToString());
        sqlConn.Open();
        //Add
        if (ViewState["action"].ToString() == "add")
        {
            //mAdd_Group(this.txt_Name.Text, ddlLanguage.SelectedValue, ddlPages.SelectedValue);
			SqlCommand sqlComm = new SqlCommand();
			if (txtUrl.Text.Trim() == "")
			{
				sqlComm = new SqlCommand("declare @temp varchar(50) set @temp=(select seo from pages where id=@pageid) insert into quicklinks(name,url,pageid,linkid,lang_id,groupid,newwindow,target) values(@name,@url+@temp,@pageid,1,@lang_id,@groupid,@newwindow,@target) select SCOPE_IDENTITY()", sqlConn);
				sqlComm.Parameters.AddWithValue("@name", txt_Name.Text);
				if (ddlLanguage.SelectedValue == "1")
					sqlComm.Parameters.AddWithValue("@url", CMSHelper.SeoPrefixEN);
				else
                    sqlComm.Parameters.AddWithValue("@url", "/fr/");
                //sqlComm.Parameters.AddWithValue("@url", "/");
				sqlComm.Parameters.AddWithValue("@pageid", ddlPages.SelectedValue);
				sqlComm.Parameters.AddWithValue("@lang_id", ddlLanguage.SelectedValue);
				sqlComm.Parameters.AddWithValue("@newwindow", rbNewWindow.SelectedValue);
 				sqlComm.Parameters.AddWithValue("@target", rbNewWindow.SelectedValue == "1" ? "_blank" :"_self");
                sqlComm.Parameters.AddWithValue("@groupid", Group);

                id = Convert.ToString(sqlComm.ExecuteScalar());
            }
            else
			{
				sqlComm = new SqlCommand("insert into quicklinks(name,url,pageid,linkid,lang_id,groupid,newwindow,target) values(@name,@url,@pageid,1,@lang_id,@groupid,@newwindow,@target) select SCOPE_IDENTITY()", sqlConn);
				sqlComm.Parameters.AddWithValue("@name", txt_Name.Text);
                //sqlComm.Parameters.AddWithValue("@url", txtUrl.Text.Trim());
                string url = txtUrl.Text;
                if (!url.StartsWith("http") && !url.StartsWith("/"))
                {
                    url = (ddlLanguage.SelectedValue == "1" ? CMSHelper.SeoPrefixEN : "/fr/") + url;
                }
                sqlComm.Parameters.AddWithValue("@url", url);
                sqlComm.Parameters.AddWithValue("@pageid", ddlPages.SelectedValue);
				sqlComm.Parameters.AddWithValue("@lang_id", ddlLanguage.SelectedValue);
				sqlComm.Parameters.AddWithValue("@newwindow", rbNewWindow.SelectedValue);
 				sqlComm.Parameters.AddWithValue("@target", rbNewWindow.SelectedValue == "1" ? "_blank" :"_self");
                sqlComm.Parameters.AddWithValue("@groupid", Group);

				//sqlComm.ExecuteNonQuery();
				id = Convert.ToString(sqlComm.ExecuteScalar());
            }


        }
        else if (ViewState["action"].ToString() == "edit")
        {
            string s = ViewState["record_id"].ToString();

			if (txtUrl.Text.Trim() == "")
			{
                SqlCommand sqlComm = new SqlCommand("declare @temp varchar(50) set @temp=(select seo from pages where id=@pageid) update quicklinks set name=@name,url=@url+@temp,pageid=@pageid,linkid=id,lang_id=@lang_id,newwindow=@newwindow,target=@target where id=@id select @id", sqlConn);
                //SqlCommand sqlComm = new SqlCommand("declare @temp varchar(50) set @temp=(select seo from pages where id=@pageid) update quicklinks set name=@name,url=@temp,pageid=@pageid,linkid=1,lang_id=@lang_id where id=@id", sqlConn);
				sqlComm.Parameters.AddWithValue("@name", txt_Name.Text);
				/*if (ddlLanguage.SelectedValue == "1")
					sqlComm.Parameters.AddWithValue("@url", "/en/");
				else
                    sqlComm.Parameters.AddWithValue("@url", "/fr/");*/
                sqlComm.Parameters.AddWithValue("@url", "/");
				sqlComm.Parameters.AddWithValue("@pageid", ddlPages.SelectedValue);
				sqlComm.Parameters.AddWithValue("@lang_id", ddlLanguage.SelectedValue);
				sqlComm.Parameters.AddWithValue("@newwindow", rbNewWindow.SelectedValue);
  				sqlComm.Parameters.AddWithValue("@target", rbNewWindow.SelectedValue == "1" ? "_blank" :"_self");
                sqlComm.Parameters.AddWithValue("@id", s);

                id = Convert.ToString(sqlComm.ExecuteScalar());
            }
            else
			{
				SqlCommand sqlComm = new SqlCommand("update quicklinks set name=@name,url=@url,pageid=@pageid,linkid=id,lang_id=@lang_id,newwindow=@newwindow,target=@target where id=@id select @id", sqlConn);
				sqlComm.Parameters.AddWithValue("@name", txt_Name.Text);
                //sqlComm.Parameters.AddWithValue("@url", txtUrl.Text);
                string url = txtUrl.Text;
                //if (!url.StartsWith("/"))
                //    url = "/" + url;
                if (!url.StartsWith("http") && !url.StartsWith("/"))
                {
                    url = (ddlLanguage.SelectedValue == "1" ? CMSHelper.SeoPrefixEN : "/fr/") + url;
                }
                sqlComm.Parameters.AddWithValue("@url", url);
				sqlComm.Parameters.AddWithValue("@pageid", ddlPages.SelectedValue);
				sqlComm.Parameters.AddWithValue("@lang_id", ddlLanguage.SelectedValue);
                sqlComm.Parameters.AddWithValue("@newwindow", rbNewWindow.SelectedValue);
   				sqlComm.Parameters.AddWithValue("@target", rbNewWindow.SelectedValue == "1" ? "_blank" :"_self");
                sqlComm.Parameters.AddWithValue("@id", s);

                id = Convert.ToString(sqlComm.ExecuteScalar());
            }
            //}
            //}



        }

        if (fuImage.HasFile)
        {
            SaveImage(fuImage, QLImagesPath, id);
        }
        if (fuImage2.HasFile)
        {
            SaveImage(fuImage2, QLImagesPath, id);
        }

        sqlConn.Close();

        this.GV_Main.EditIndex = -1;
        mBindData();
        ViewState["action"] = "add";

        Clearfield();
    }

    private void SaveImage(FileUpload fu, string folderpath, string id)
    {
        try
        {
            HttpPostedFile myFile = fu.PostedFile;
            if (myFile != null)
            {
                string fExten = Path.GetExtension(myFile.FileName).ToLower();

                if (fExten == ".jpg" || fExten == ".png" || fExten == ".bmp" || fExten == ".gif" || fExten == ".jpeg" || fExten == ".jfif" || fExten == ".svg")
                {
                    string filename = Path.GetFileName(fu.FileName);
                    string folder = Server.MapPath(folderpath + Group);
                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder);

                    fu.SaveAs(folder + "\\" + filename);

                    List<SqlParameter> prms2 = new List<SqlParameter>();
                    prms2.Add(new SqlParameter("@id", id));
                    prms2.Add(new SqlParameter("@image", filename));
                    if (fu.ID == "fuImage")
                    {
                        ProcessRecord("update QuickLinks set image=@image where id=@id", prms2.ToArray(), CommandType.Text);
                        imgCurrentImage.ImageUrl = folderpath + Group + "/" + filename;
                        lnkDlete.Visible = true;
                    }
                    else if (fu.ID == "fuImage2")
                    {
                        ProcessRecord("update QuickLinks set image2=@image where id=@id", prms2.ToArray(), CommandType.Text);
                        imgCurrentImage2.ImageUrl = folderpath + Group + "/" + filename;
                        lnkDlete2.Visible = true;
                    }

                }
            }
        }
        catch (Exception ex) { }
    }

    protected void lnkDlete_Click(object sender, EventArgs e)
    {
        string cmd = "update QuickLinks set Image=null where id=@id";

        List<SqlParameter> prms = new List<SqlParameter>();
        prms.Add(new SqlParameter("@id", ViewState["record_id"]));
        ProcessRecord(cmd, prms.ToArray(), CommandType.Text);

        imgCurrentImage.ImageUrl = "";
        lnkDlete.Visible = false;
    }
    protected void btn_Cancel_step1_Click(object sender, EventArgs e)
    {
        this.GV_Main.EditIndex = -1;
        mBindData();
        Clearfield();
    }

    protected void btnAddGallery_Click(object sender, EventArgs e)
    {
        SwitchView(pnlEditGrp);
        txtNameGrp.Text = "";
    }

    protected void GV_Group_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "EditGroup")
        {
            txtNameGrp.Text = "";

            SwitchView(pnlEditGrp);
            Group = Convert.ToInt32(e.CommandArgument.ToString());

            DataTable dt = new DataTable();
            string sqlstr = @" select * from QuickLinksGroups where id=@id";
            SqlCommand sq = new SqlCommand(sqlstr);
            sq.Parameters.AddWithValue("@id", Group);

            dt = getTable(sq, true);

            if (dt.Rows.Count > 0)
            {
                txtNameGrp.Text = dt.Rows[0]["name"].ToString();
            }


        }
        else if (e.CommandName == "EditRows")
        {
            Group = Convert.ToInt32(e.CommandArgument.ToString());

            mBindData();
        }
        else if (e.CommandName == "DeleteGroup")
        {
            string sqlstr = @" delete from QuickLinksGroups where id=@id delete from content where control='QuickLinks' and param=@id ";
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@id", e.CommandArgument.ToString()));

            ProcessRecord(sqlstr, parms, CommandType.Text);
            mBindGrp();
        }
    }

    protected void GV_Group_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if ((DataRowView)e.Row.DataItem != null)
        {
            ImageButton LB_Delete = (ImageButton)e.Row.FindControl("btnDelete");
            LB_Delete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");

            DataRowView dr = (DataRowView)e.Row.DataItem;

            //https://www.w3schools.com/howto/howto_js_copy_clipboard.asp
            Literal litCopy = (Literal)e.Row.FindControl("litCopy");
            litCopy.Text = String.Format("<input type='button' value='Copy' onclick=\"javascript: myCopyFunction({0}, '{1}');\" />", 
                                            dr["contentid"].ToString(), dr["name"].ToString());
        }
    }

    protected void btn_Cancel_Grp_Click(object sender, EventArgs e)
    {
        Group = 0;

        SwitchView(pnlGroups);

    }

    protected void btn_Submit_Grp_Click(object sender, EventArgs e)
    {
        List<SqlParameter> parms = new List<SqlParameter>();
        string sql = "";

        if (Group == 0)
        {
            sql = @"declare @id int  
                    insert into QuickLinksGroups (name) values(@name); 
                    select @id = SCOPE_IDENTITY(); 
                    insert into content ([name],[control],[param],[language]) values ('QuickLink -' + @name, 'QuickLinks', @id, 1) 
                    select @id";

        }
        else
        {
            //Update
            sql = "update QuickLinksGroups set name=@name where id=@id; update content set name = 'QuickLink -' + @name where control='QuickLinks' and param=@id; select @id";
            parms.Add(new SqlParameter("@id", Group));
        }
        parms.Add(new SqlParameter("@name", txtNameGrp.Text));

        string id = ExecuteScalar(sql, parms, CommandType.Text).ToString();


        Clearfield();
        Group = 0;

        SwitchView(pnlGroups);
        mBindGrp();

    }

    private void SwitchView(Panel pnl)
    {
        pnlGroups.Visible = false;
        pnlEditGrp.Visible = false;
        pnlList.Visible = false;
        tbl_add_edit.Visible = false;

        pnl.Visible = true;

    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        SwitchView(pnlGroups);
    }

    private bool ProcessRecord(string sql, List<SqlParameter> parms, CommandType type)
    {
        SqlCommand cmd = new SqlCommand(sql, new SqlConnection(ConfigurationManager.AppSettings["CMServer"]));
        cmd.CommandType = type;

        foreach (SqlParameter p in parms)
            cmd.Parameters.Add(p);

        cmd.Connection.Open();
        int ret = cmd.ExecuteNonQuery();
        cmd.Connection.Close();
        return (ret > 0);
    }

    private string ProcessRecord(string sql, SqlParameter[] prms, CommandType type)
    {
        SqlCommand cmd = new SqlCommand(sql, new SqlConnection(ConfigurationManager.AppSettings["CMServer"]));
        cmd.CommandType = type;
        cmd.Parameters.AddRange(prms);
        cmd.Connection.Open();
        string ret = Convert.ToString(cmd.ExecuteScalar());
        cmd.Connection.Close();
        return ret;
    }
    private object ExecuteScalar(string sql, List<SqlParameter> parms, CommandType type)
    {
        SqlCommand cmd = new SqlCommand(sql, new SqlConnection(ConfigurationManager.AppSettings["CMServer"]));
        cmd.CommandType = type;

        foreach (SqlParameter p in parms)
            cmd.Parameters.Add(p);

        cmd.Connection.Open();
        object ret = cmd.ExecuteScalar();
        cmd.Connection.Close();
        return ret;
    }
    private DataTable getTable(SqlCommand cmd, bool a)
    {
        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.SelectCommand.CommandType = CommandType.Text;
        da.SelectCommand.Connection = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
        da.Fill(dt);
        return dt;
    }

    protected void lnkDlete2_Click(object sender, EventArgs e)
    {
        string cmd = "update QuickLinks set Image2=null where id=@id";

        List<SqlParameter> prms = new List<SqlParameter>();
        prms.Add(new SqlParameter("@id", ViewState["record_id"]));
        ProcessRecord(cmd, prms.ToArray(), CommandType.Text);

        imgCurrentImage2.ImageUrl = "";
        lnkDlete2.Visible = false;
    }
}
