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
    #region DAL

    StringBuilder sb;


    #region Template

    public DataTable mGet_All_Reviewers()
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = "select * from Users where lower(status) = 'active' and reviewer =1 order by username";
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

    public DataSet mGet_All_Group()
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        //string commandString = "select * from Groups where name like '%" + txtFilter.Text + "%' order by name";
        string commandString = "select * from Groups where (@filter is null or name like '%' + @filter + '%' or color like '%' + @filter + '%') order by name";

        if(cbRoleFilter.Checked )
            commandString = "select * from Groups where (@filter is null or name like '%' + @filter + '%' or color like '%' + @filter + '%') and ForumRoleId is not null order by name";

        commandString += " select * from EkoSuperGroups";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            if (txtFilter.Text.Trim().Length > 0)
                cmd.Parameters.AddWithValue("@filter", txtFilter.Text.Trim());
            else
                cmd.Parameters.AddWithValue("@filter", DBNull.Value);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds);
        }

        return ds;
    }



    public DataTable mGet_One_Group(int mID)
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = "select *, isnull(private, 0) _private from Groups where id = @mID order by name";
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

    public string mAdd_Group(string name, string description, string color, string reviewer, bool bprivate)
    {
        string groupId = "0";

        sb = sb.Remove(0, sb.Length);
        sb.Append(" insert into Groups  (name, description, color, reviewer, private) ");
        sb.Append(" values   (@name, @description, @color, @reviewer, @private)");
       //// sb.Append(" declare @gid int set @gid=SCOPE_IDENTITY() insert into users_groups_access(user_id,group_id,access_level) values(1,@gid,4)");   //Admin
        sb.Append(" declare @gid int set @gid=SCOPE_IDENTITY() insert into users_groups_access(user_id,group_id,access_level) values(1,@gid,5)");       //Admin    

        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();

        SqlDataAdapter dapt = new SqlDataAdapter("select id from users where getsallgroups=1", strConnectionString);
        DataTable dt = new DataTable();
        dapt.Fill(dt);

        foreach (DataRow dr in dt.Rows)
        {
            ////sb.Append(" insert into users_groups_access(user_id,group_id,access_level) values(" + dr["id"].ToString() + ",@gid,3)");    //Publisher
            sb.Append(" insert into users_groups_access(user_id,group_id,access_level) values(" + dr["id"].ToString() + ",@gid,4)");        //Publisher
        }

        sb.Append(" select @gid ");
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@description", description);
            cmd.Parameters.AddWithValue("@color", color.Replace(",", ";"));
            cmd.Parameters.AddWithValue("@reviewer", reviewer);
            cmd.Parameters.AddWithValue("@private", bprivate ? "1" : "0");

            connection.Open();

            groupId = cmd.ExecuteScalar().ToString();

          //  SqlCommand cmd2 = new SqlCommand("", connection);
          //  cmd2.ExecuteNonQuery();

            connection.Close();
        }

        return groupId;
    }

    public void mEdit_Group(string id, string name, string description, string color, string reviewer, bool bprivate)
    {
        sb = sb.Remove(0, sb.Length);
        sb.Append(" update Groups ");
        sb.Append(" Set name = @name, ");
        sb.Append(" description = @description, ");
        sb.Append(" color = @color, ");
        sb.Append(" reviewer = @reviewer, ");
        sb.Append(" private = @private, ");
        sb.Append(" supergroup = @supergroup,");
        sb.Append(" ShowOnRegistration = @ShowOnRegistration");

        sb.Append(" where id = @id ");


        sb.Append(" update pages");
        sb.Append(" set reviewer  = @reviewer ");
        sb.Append(" where id in (select Page_id from Pages_Group where Group_id = @id) ");
        
        
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);

            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@description", description);
            cmd.Parameters.AddWithValue("@color", color);
            cmd.Parameters.AddWithValue("@reviewer", reviewer);
            cmd.Parameters.AddWithValue("@private", bprivate ? "1" : "0");

            if (rblSupergroup.SelectedValue != "")
                cmd.Parameters.AddWithValue("@supergroup", rblSupergroup.SelectedValue);
            else
                cmd.Parameters.AddWithValue("@supergroup", DBNull.Value);

            cmd.Parameters.AddWithValue("@ShowOnRegistration", cbShowOnRegForm.Checked ? "1" : "0");


            connection.Open();
            cmd.ExecuteNonQuery();
        }

    }

    public bool mDelete_Group(int mID)
    {
        sb = sb.Remove(0, sb.Length);
        sb.Append(" declare @pages int = 0, @menus int = 0  ");
        sb.Append(" select @pages = count(page_id) from Groups g inner join Pages_Group pg on g.id = pg.Group_id inner join Pages p on p.id = pg.Page_id where g.id = @id ");
        sb.Append(" select @menus = count(MenuItem_id) from Groups g inner join Menu_Group mg on g.id = mg.Group_id inner join MenuItems m on m.id = mg.MenuItem_id where g.id = @id  ");
        sb.Append(" select @pages, @menus ");
        sb.Append(" if @pages = 0 and @menus = 0  ");
        sb.Append(" Delete from Groups where id = @id  ");

        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = sb.ToString();
            
        DataSet ds = new DataSet();
        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            //SqlCommand cmd = new SqlCommand(commandString, connection);
            //cmd.Parameters.AddWithValue("@id", mID);
            //connection.Open();
            //cmd.ExecuteNonQuery();


            SqlDataAdapter da = new SqlDataAdapter(commandString, connection);
            da.SelectCommand.Parameters.AddWithValue("@id", mID);

            da.Fill(ds);

        }

        string script = "";
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (Convert.ToInt32(ds.Tables[0].Rows[0][0]) > 0)
                script = "This group has page(s) associated with it.";

            if (Convert.ToInt32(ds.Tables[0].Rows[0][1]) > 0)
                script += " This group has menu item(s) associated with it.";

            if (script != "")
            {
                script += " Please remove the association(s) before deleting the group.";
                this.Page.ClientScript.RegisterClientScriptBlock(GetType(), "errorgroup", String.Format("<script>alert('{0}');</script>", script));
                return false;
            }
        }

        return true;

    }

    public void mDelete_Menu_Group(int mID)
    {
        sb = sb.Remove(0, sb.Length);
        sb.Append(" Delete from Menu_Group ");
        sb.Append(" where Group_id = @id ");

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

    public void mDelete_Pages_Group(int mID)
    {
        sb = sb.Remove(0, sb.Length);
        sb.Append(" Delete from Pages_Group ");
        sb.Append(" where Group_id = @id ");

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

    public void mDelete_Users_Groups_Access(int mID)
    {
        sb = sb.Remove(0, sb.Length);
        sb.Append(" Delete from Users_Groups_Access ");
        sb.Append(" where Group_id = @id ");

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
    protected void Page_Load(object sender, EventArgs e)
    {
        sb = new StringBuilder(60);
        this.lbl_msg.Text = "";
        if (!IsPostBack)
        {
            ViewState["action"] = "add";
            //ViewState["sortExpression"] = "";
            //ViewState["sortD"] = "ASC";
            sortExp = "name"; // default sorted column
            sortOrder = "asc";    // default sort order


            this.tbl_add_edit.Visible = false;
            this.tbl_Grid.Visible = true;


            ViewState["record_id"] = "";

            mBindData();

            //to be delete
            Session["user_id"] = "-1";
        }


		


    }

    public void filter(object o, EventArgs e)
    {
        mBindData();
    }

    public void onitembound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
	{
		CheckBox cb = (CheckBox) e.Item.FindControl("cbVisible");
	}


    #region My_Functions

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

    private void mBindData()
    {
        mBindData(sortExp, sortOrder);
    }
    private void mBindData(string sortExp, string sortDir)
    {
        DataSet ds = new DataSet();
        ds = mGet_All_Group();

        DataTable dt = ds.Tables[0];
        DataView DV = dt.DefaultView;
        if (!(sortExp == string.Empty))
        {
            DV.Sort = string.Format("{0} {1}", sortExp, sortDir);
        }
        this.GV_Main.DataSource = DV;
        this.GV_Main.DataBind();

        rblSupergroup.DataSource = ds.Tables[1];
        rblSupergroup.DataBind();

        dt = mGet_All_Reviewers();
        ddlReviewer.DataSource = dt;
        ddlReviewer.DataBind();
        ddlReviewer.Items.Insert(0, new ListItem("select one", ""));

		pager1.ItemCount = DV.Count;
        pager1.Visible = dt.Rows.Count > GV_Main.PageSize;

        litPagerShowing.Text = CMSHelper.GetPagerInfo(GV_Main, DV.Count);

    }

    void Clearfield()
    {
        this.txt_Name.Text = "";
        this.txt_yafName.Text = "";
        this.txt_Description.Text = "";
        this.txt_Color.Text = "";
        ddlReviewer.ClearSelection();
        this.cbPrivate.Checked = false;
        rblSupergroup.ClearSelection();
        trSupergroup.Visible = false;
        trShowOnReg.Visible = false;
        cbShowOnRegForm.Checked = false;

        this.tbl_add_edit.Visible = false;
        this.tbl_Grid.Visible = true;
        this.GV_Main.EditIndex = -1;
        ViewState["action"] = "add";

    }

    #endregion



    #region Grid_Events

    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        #region Add sorted class to headers
        if (e.Row.RowType == DataControlRowType.Header)
        {
            int colIndex = CMSHelper.GetColumnIndex((sender as GridView), sortExp);
            e.Row.Cells[colIndex].CssClass = sortOrder == "asc" ? "sortasc" : "sortdesc";
        }
        #endregion Add sorted class to headers

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView drv = (DataRowView)e.Row.DataItem;

            //for delete button
            ImageButton lb;
            lb = (ImageButton)e.Row.FindControl("LB_Delete");

            if (drv["ForumRoleID"].ToString() != "")
            {
                //lb.Enabled = false;
                //lb.Attributes.Add("style", "opacity:0.5");
                lb.Visible = false;
            }
            else
                lb.Attributes.Add("OnClick", "return confirm('Are you sure to delete this Record?');");
            
            if (Convert.ToInt32(drv["id"]) == 1)
                lb.Visible = false;
        }
    }

    /*protected void GV_Main_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.GV_Main.PageIndex = e.NewPageIndex;
        mBindData((string)ViewState["sortExpression"]);
    }*/

    protected void GV_Main_Sorting(object sender, GridViewSortEventArgs e)
    {
        sortExp = e.SortExpression;

        if (sortOrder == "desc")
            sortOrder = "asc";
        else
            sortOrder = "desc";
        mBindData(e.SortExpression, sortOrder);

        //if ((string)ViewState["sortD"] == "ASC")
        //{
        //    ViewState["sortD"] = "DESC";
        //    ViewState["sortExpression"] = e.SortExpression + " DESC";
        //    mBindData(e.SortExpression + " DESC");
        //}

        //else
        //{
        //    if ((string)ViewState["sortD"] == "DESC")
        //    {
        //        ViewState["sortD"] = "ASC";
        //        ViewState["sortExpression"] = e.SortExpression + " ASC";
        //        mBindData(e.SortExpression + " ASC");
        //    }
        //}
    }


    protected void GV_Main_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        Int32 id = Convert.ToInt32(this.GV_Main.DataKeys[e.RowIndex].Value.ToString());

        //Delete Group
        if (mDelete_Group(id))
        {
            ////Delete Dependent records
            //mDelete_Menu_Group(id);
            //mDelete_Pages_Group(id);
            mDelete_Users_Groups_Access(id);

            mBindData();

            this.tbl_add_edit.Visible = false;
            this.tbl_Grid.Visible = true;

            ViewState["action"] = "add";
        }
    }

    protected void GV_Main_RowEditing(object sender, GridViewEditEventArgs e)
    {
        this.tbl_Grid.Visible = false;
        this.tbl_add_edit.Visible = true;
      

        ViewState["action"] = "edit";

        Int32 id = Convert.ToInt32(this.GV_Main.DataKeys[e.NewEditIndex].Value.ToString());
        ViewState["record_id"] = id;

        DataTable dt = new DataTable();
        dt = mGet_One_Group(id);

        this.txt_Name.Text = dt.Rows[0]["name"].ToString();
        this.txt_yafName.Text = dt.Rows[0]["yaf_GroupName"].ToString();
        
        this.txt_Description.Text = dt.Rows[0]["description"].ToString();
        this.txt_Color.Text = dt.Rows[0]["color"].ToString();
        this.cbPrivate.Checked = bool.Parse(dt.Rows[0]["_private"].ToString());

        try { rblSupergroup.SelectedValue = dt.Rows[0]["supergroup"].ToString(); }
        catch { }
       
        try { cbShowOnRegForm.Checked = Convert.ToBoolean(dt.Rows[0]["ShowOnRegistration"]); }
        catch { }

        if (!String.IsNullOrEmpty(dt.Rows[0]["reviewer"].ToString()) && dt.Rows[0]["reviewer"].ToString() != "0")
        {
            try { this.ddlReviewer.SelectedValue = dt.Rows[0]["reviewer"].ToString(); }
            catch {  }
        }

		SqlDataAdapter dapt = new SqlDataAdapter("declare @temp TABLE(id int, name varchar(500), visible bit) insert into @temp(id,name,visible) select id,[text],'False' from adminmenu where parentid is not NULL update @temp set visible='True' where id in (select adminmenuid from AdminMenu_Groups where groupid=" + id.ToString() + ") select * from @temp",ConfigurationManager.AppSettings["CMServer"].ToString());
		DataSet ds = new DataSet();

    	dapt.Fill(ds);

    	dgMenu.DataSource = ds.Tables[0];
		dgMenu.DataBind();

        trSupergroup.Visible = txt_yafName.Text != "";

        trShowOnReg.Visible = txt_yafName.Text != "";
        this.cbShowOnRegForm.Checked = bool.Parse(dt.Rows[0]["ShowOnRegistration"].ToString());

    }

    #endregion


    protected void bttn_Add_Question_Click(object sender, EventArgs e)
    {
        this.tbl_add_edit.Visible = true;
        this.tbl_Grid.Visible = false;
        trSupergroup.Visible = false;
        this.cbShowOnRegForm.Checked = false;
        trShowOnReg.Visible = false;

    }
    protected void btn_Submit_Click(object sender, EventArgs e)
    {
        string groupId = "0";

		txt_Name.Text = txt_Name.Text.Replace(" ", "").Replace("&", "").Replace(",", "").Replace("'", "").Replace("#", "").Replace("*", "").Replace("$", "");
        //Add
        if (ViewState["action"].ToString() == "add")
        {
            groupId = mAdd_Group(this.txt_Name.Text, this.txt_Description.Text, this.txt_Color.Text, this.ddlReviewer.SelectedValue, this.cbPrivate.Checked);
        }
        else if (ViewState["action"].ToString() == "edit")
        {
            groupId = ViewState["record_id"].ToString();
            mEdit_Group(groupId, this.txt_Name.Text, this.txt_Description.Text, this.txt_Color.Text, this.ddlReviewer.SelectedValue, this.cbPrivate.Checked);

			SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"].ToString());

			SqlCommand sqlClear = new SqlCommand("delete from adminmenu_groups where groupid=" + groupId,sqlConn);
			
			sqlConn.Open();
        	sqlClear.ExecuteNonQuery();

        	foreach (DataGridItem di in dgMenu.Items)
        	{
        		int i = Convert.ToInt32(((Label)di.FindControl("lblId")).Text);
        		bool enable = ((CheckBox) di.FindControl("cbVisible")).Checked;

				if (enable)
				{
					
					SqlCommand sqlComm = new SqlCommand("insert into adminmenu_groups(adminmenuid,groupid) values(" + i + ", " + groupId + ")",sqlConn);
					sqlComm.ExecuteNonQuery();
				}
        	}

			sqlConn.Close();

        }

        string uploadsPath = HttpContext.Current.Request.MapPath("/uploads/" + txt_Name.Text);
        string docPathBase = HttpContext.Current.Request.MapPath("/Data/Documents/");
        string docPath = string.Empty;

        DataTable dt = new DataTable();
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]))
        {
            SqlDataAdapter dapt = new SqlDataAdapter("select id from DocumentGroups where groupid=@groupid", conn);
            dapt.SelectCommand.Parameters.AddWithValue("@groupid", groupId);
            dapt.Fill(dt);
        }

        if (this.cbPrivate.Checked)
        {
            // creates web.config in group's folder inside uploads
            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            if (!File.Exists(uploadsPath + @"/web.config"))
                CMSHelper.CreatePrivateConfigFile(txt_Name.Text, uploadsPath, groupId);

            // creates web.config in Documents's folder
            foreach (DataRow dr in dt.Rows)
            {
                docPath = docPathBase + dr["id"].ToString();

                if (!Directory.Exists(docPath))
                    Directory.CreateDirectory(docPath);

                if (!File.Exists(docPath + @"/web.config"))
                    CMSHelper.CreatePrivateConfigFile(txt_Name.Text, docPath, groupId);
            }
        }
        else
        {
            // delete web.config in group's folder inside uploads
            if (File.Exists(uploadsPath + @"/web.config"))
                File.Delete(uploadsPath + @"/web.config");

            // delete web.config in Documents's folder
            foreach (DataRow dr in dt.Rows)
            {
                docPath = docPathBase + dr["id"].ToString();

                if (File.Exists(docPath + @"/web.config"))
                    File.Delete(docPath + @"/web.config");
            }
        }

        Clearfield();

        mBindData();
        ViewState["action"] = "add";
    }
    protected void btn_Cancel_step1_Click(object sender, EventArgs e)
    {
        Clearfield();

        mBindData();
    }
}
