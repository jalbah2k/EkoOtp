using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

public partial class Admin_Users_Users : System.Web.UI.UserControl
{
    #region DAL

    StringBuilder sb;


    #region Template

    public DataTable mGet_All_Users()
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        //string commandString = "select * from Users where (username like '%" + txtFilter.Text + "%' or name like '%" + txtFilter.Text + "%') and id in (select User_id from Users_Groups_Access where Group_id=case when @groupid <> 0 then @groupid else Group_id end) and username!='admin' order by username";
        ////string commandString = "select * from Users ";
        ////commandString += " where username != 'admin' ";
        ////commandString += " and (@filter is null or username like '%' + @filter + '%' or name like '%' + @filter + '%') ";

 		string commandString = @"declare @mytab table (groupid int)
                                
                                insert into @mytab (groupid) 
                                select group_id from Users_Groups_Access where user_id = case when @userid != 1 then @userid else user_id end

                                select distinct u.* from Users u
                                inner join Users_Groups_Access g on u.id=g.User_id
                                inner join @mytab t on g.Group_id=t.groupid
                                where (username like '%' + @filter + '%' or name like '%' + @filter + '%')  ";

        if (ddlGroups.SelectedValue != "0")
            commandString += " and u.id in (select User_id from Users_Groups_Access where Group_id=case when @groupid <> 0 then @groupid else Group_id end) ";

        commandString += " and username!='admin' ";
        commandString += " order by username ";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@userid", Session["LoggedInID"].ToString());

            if (ddlGroups.SelectedValue != "0")
                cmd.Parameters.AddWithValue("@groupid", ddlGroups.SelectedValue);
           // if (txtFilter.Text.Trim().Length > 0)
                cmd.Parameters.AddWithValue("@filter", txtFilter.Text.Trim());
            //else
            //    cmd.Parameters.AddWithValue("@filter", DBNull.Value);

            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds, "table1");
        }

        if (ds.Tables[0].Rows.Count < 1)
        {
            tbl_noresults.Visible = true;
            tbl_Grids.Visible = false;
        }
        else
        {
            tbl_noresults.Visible = false;
            tbl_Grids.Visible = true;
        }

        return ds.Tables[0];
    }



    public DataTable mGet_One_Users(int mID)
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = "select * from Users where id = @mID";
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

    public void mAdd_Users(string username, string password, string status, string name, string address1, string address2, string city, string region, string country, string postalcode, string homephone, string workphone, string workext, string cellphone, string faxphone, string faxext, string email, string sex, string birthdate, string website, string timezone, string language, string cookiekey)
    {

        sb = sb.Remove(0, sb.Length);
        sb.Append("declare @id int insert into Users  (username, password, status, name, address1, address2, city, region, country, postalcode, homephone, workphone, workext, cellphone, faxphone, faxext, email, sex, birthdate, website, timezone, language, cookiekey, datecreated, datemodified, dateloggedin,getsallgroups, reviewer, AD, ManageArea)");
        sb.Append(" values   (@username, @password, @status, @name, @address1, @address2, @city, @region, @country, @postalcode, @homephone, @workphone, @workext, @cellphone, @faxphone, @faxext, @email, @sex, @birthdate, @website, @timezone, @language, @cookiekey, @datecreated, @datemodified, @dateloggedin,@getsallgroups,@reviewer, @AD, @ManageArea) select @id = SCOPE_IDENTITY() ");
        //sb.Append(" SELECT id FROM Template WHERE (id = SCOPE_IDENTITY()) ");
        sb.Append(" insert into Users_Groups_Access(User_id,Group_id,Access_Level) values(@id, @groupid, @access) select @id");
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = sb.ToString();

        string userid;

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password", password);
            cmd.Parameters.AddWithValue("@status", status);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@address1", address1);
            cmd.Parameters.AddWithValue("@address2", address2);
            cmd.Parameters.AddWithValue("@city", city);
            cmd.Parameters.AddWithValue("@region", region);
            cmd.Parameters.AddWithValue("@country", country);
            cmd.Parameters.AddWithValue("@postalcode", postalcode);
            cmd.Parameters.AddWithValue("@homephone", homephone);
            cmd.Parameters.AddWithValue("@workphone", workphone);
            cmd.Parameters.AddWithValue("@workext", workext);
            cmd.Parameters.AddWithValue("@cellphone", cellphone);
            cmd.Parameters.AddWithValue("@faxphone", faxphone);
            cmd.Parameters.AddWithValue("@faxext", faxext);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@sex", sex);
            cmd.Parameters.AddWithValue("@birthdate", birthdate);
            cmd.Parameters.AddWithValue("@website", website);
            cmd.Parameters.AddWithValue("@timezone", timezone);
            cmd.Parameters.AddWithValue("@language", language);
            cmd.Parameters.AddWithValue("@cookiekey", cookiekey);
            cmd.Parameters.AddWithValue("@datecreated", DateTime.Now);
            cmd.Parameters.AddWithValue("@datemodified", DateTime.Now);
            cmd.Parameters.AddWithValue("@dateloggedin", DateTime.Now);
            cmd.Parameters.AddWithValue("@getsallgroups", cbAdmin.Checked);
            cmd.Parameters.AddWithValue("@reviewer", cbReviewer.Checked);
            cmd.Parameters.AddWithValue("@ManageArea", cbManageArea.Checked);

            cmd.Parameters.AddWithValue("@groupid", LB_Group2.SelectedValue);
            cmd.Parameters.AddWithValue("@access", RB_Permission2.SelectedValue);

            cmd.Parameters.AddWithValue("@ad", cbAD.Checked);

            connection.Open();
            userid = cmd.ExecuteScalar().ToString();

            ViewState["newid"] = userid;
            connection.Close();

            if (!cbAD.Checked)
                GeneratePassword(userid);
        }

        if (cbAdmin.Checked)
        {
            SqlConnection sqlConn = new SqlConnection(strConnectionString);
            SqlCommand sqlComm = new SqlCommand("delete from Users_Groups_Access where User_id=@id insert into Users_Groups_Access(User_id,Group_id,Access_Level) select @id,id,4 from groups", sqlConn);
            /////SqlCommand sqlComm = new SqlCommand("delete from Users_Groups_Access where User_id=@id insert into Users_Groups_Access(User_id,Group_id,Access_Level) select @id,id,5 from groups", sqlConn);
            sqlComm.Parameters.AddWithValue("@id", userid);

            sqlConn.Open();
            sqlComm.ExecuteNonQuery();
            sqlConn.Close();
        }

    }

    public void mEdit_Users(string id, string status, string name, string address1, string address2, string city, string region, string country, string postalcode, string homephone, string workphone, string workext, string cellphone, string faxphone, string faxext, string email, string sex, string birthdate, string website, string timezone, string language)
    {
        sb = sb.Remove(0, sb.Length);
        sb.Append(" update Users ");
        sb.Append(" Set status = @status, ");
        sb.Append(" name = @name, ");
        sb.Append(" address1 = @address1, ");
        sb.Append(" address2 = @address2, ");
        sb.Append(" city = @city, ");
        sb.Append(" region = @region, ");
        sb.Append(" country = @country , ");
        sb.Append(" postalcode = @postalcode, ");
        sb.Append(" homephone = @homephone, ");
        sb.Append(" workphone = @workphone, ");
        sb.Append(" workext = @workext, ");
        sb.Append(" cellphone = @cellphone, ");
        sb.Append(" faxphone = @faxphone, ");
        sb.Append(" faxext = @faxext, ");
        sb.Append(" email = @email, ");
        sb.Append(" sex = @sex, ");
        //   sb.Append(" birthdate = @birthdate, ");
        sb.Append(" website = @website, ");
        sb.Append(" timezone = @timezone, ");
        sb.Append(" language = @language, ");
        sb.Append(" datemodified = @datemodified, ");
        sb.Append(" getsallgroups = @getsallgroups, ");
        sb.Append(" reviewer = @reviewer, ");
        sb.Append(" AD = @AD, ");
		sb.Append(" ManageArea = @ManageArea ");

        sb.Append(" where id = @id ");
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);

            cmd.Parameters.AddWithValue("@id", id);

            cmd.Parameters.AddWithValue("@status", status);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@address1", address1);
            cmd.Parameters.AddWithValue("@address2", address2);
            cmd.Parameters.AddWithValue("@city", city);
            cmd.Parameters.AddWithValue("@region", region);
            cmd.Parameters.AddWithValue("@country", country);
            cmd.Parameters.AddWithValue("@postalcode", postalcode);
            cmd.Parameters.AddWithValue("@homephone", homephone);
            cmd.Parameters.AddWithValue("@workphone", workphone);
            cmd.Parameters.AddWithValue("@workext", workext);
            cmd.Parameters.AddWithValue("@cellphone", cellphone);
            cmd.Parameters.AddWithValue("@faxphone", faxphone);
            cmd.Parameters.AddWithValue("@faxext", faxext);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@sex", sex);
            //           cmd.Parameters.AddWithValue("@birthdate", birthdate);
            cmd.Parameters.AddWithValue("@website", website);
            cmd.Parameters.AddWithValue("@timezone", timezone);
            cmd.Parameters.AddWithValue("@language", language);


            cmd.Parameters.AddWithValue("@datemodified", DateTime.Now);

            cmd.Parameters.AddWithValue("@getsallgroups", cbAdmin.Checked);

            cmd.Parameters.AddWithValue("@reviewer", cbReviewer.Checked);
            cmd.Parameters.AddWithValue("@AD", cbAD.Checked);
			cmd.Parameters.AddWithValue("@ManageArea", cbManageArea.Checked);

            connection.Open();
            cmd.ExecuteNonQuery();
        }

    }

    public void mDelete_Users(int mID)
    {
        sb = sb.Remove(0, sb.Length);
        sb.Append(" Delete from Users ");
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
        frow1.Visible = false;          // (bool)Session["Multilingual"];

        if (Session["LoggedInID"].ToString() == "1")
        {
            adminrow.Visible = true;
        }

        sb = new StringBuilder(60);
        this.lbl_msg.Text = "";
		if (!IsPostBack)
		{
			ViewState["action"] = "add";
			//ViewState["sortExpression"] = "";
			//ViewState["sortD"] = "ASC";
            sortExp = "username"; // default sorted column
            sortOrder = "asc";    // default sort order


            this.tbl_add_edit.Visible = false;
            this.tbl_Grid.Visible = true;
            this.tbl_showfield.Visible = false;


            ViewState["record_id"] = "";

            BindEverything();
            mBindData("", "");

            //to be delete
            //Session["user_id"] = "-1";

            string mUser_id = Request["id"];

            if (mUser_id != "")
            {
                DataTable dt = new DataTable();
                dt = mGet_All_Users_Groups_Access_ByUserid(Convert.ToInt32(mUser_id));
                ViewState["dt_permission"] = dt;

                this.GridView1.DataSource = dt;
                this.GridView1.DataBind();

            }
            Load_ListGroup();

        }

    }

    private void BindEverything()
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            //SqlDataAdapter dapt = new SqlDataAdapter("select * from Groups", connection);
            SqlDataAdapter dapt = new SqlDataAdapter(String.Format("select * from Groups where id in (select group_id from Users_Groups_Access where user_id={0})",
                Session["LoggedInID"].ToString() != "1" ? Session["LoggedInID"].ToString() : "user_id")
                , connection);

            DataSet ds = new DataSet();
            dapt.Fill(ds);

            ddlGroups.DataTextField = "name";
            ddlGroups.DataValueField = "id";
            ddlGroups.DataSource = ds.Tables[0];
            ddlGroups.DataBind();
            ddlGroups.Items.Insert(0, new ListItem("Filter by group", "0"));
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        LB_AddGroup.Visible = tbl_permission.Visible;
    }

    void Load_ListGroup()
    {
        DataTable dt = new DataTable();
        dt = mGet_All_Groups();

        this.LB_Group.DataSource = dt;
        this.LB_Group.DataTextField = "name";
        this.LB_Group.DataValueField = "id";
        this.LB_Group.DataBind();
        LB_Group.Items.Insert(0, new ListItem("Select a Group", "", true));
        LB_Group.Items.FindByValue("").Selected = true;

        this.LB_Group2.DataSource = dt;
        this.LB_Group2.DataTextField = "name";
        this.LB_Group2.DataValueField = "id";
        this.LB_Group2.DataBind();
        try { LB_Group2.Items.FindByText("Common").Selected = true; }
        catch { }
    }

	public void filter()
	{
        mBindData("", "");
    }

    public void filter(object o, EventArgs e)
	{
        mBindData("", "");
    }


    #region My_Functions

    public void PageSizeChange(object o, EventArgs e)
    {
        this.GV_Main.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
        pager1.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
        pager1.CurrentIndex = 1;
        this.GV_Main.PageIndex = 0;
        mBindData("", "");
    }
    public void pager_Command(object sender, CommandEventArgs e)
	{
		int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
		pager1.CurrentIndex = currnetPageIndx;
		GV_Main.PageIndex = currnetPageIndx - 1;
		//BindRepeater();
		mBindData("", "");
	}

    //private void mBindData()
    //{
    //    mBindData(sortExp, sortOrder);
    //}
    private void mBindData(string sortExp, string sortDir)
    {
        DataTable dt = new DataTable();
        dt = mGet_All_Users();
        DataView DV = dt.DefaultView;
        if (!(sortExp == string.Empty))
        {
            DV.Sort = string.Format("{0} {1}", sortExp, sortDir);
        }
        this.GV_Main.DataSource = DV;
        this.GV_Main.DataBind();

		pager1.ItemCount = DV.Count;
        pager1.Visible = dt.Rows.Count > GV_Main.PageSize;

        litPagerShowing.Text = CMSHelper.GetPagerInfo(GV_Main, DV.Count);
    }

    void Clearfield()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("id");
        dt.Columns.Add("user_id");
        dt.Columns.Add("group_id");
        dt.Columns.Add("access_level");
        ViewState["dt_permission"] = dt;


        LB_Group.ClearSelection();
        LB_Group2.ClearSelection();

        RB_Permission.ClearSelection();
        RB_Permission2.ClearSelection();

        this.txt_UserName.Text = "";
        this.txt_Name.Text = "";
        this.txt_Website.Text = "";

        cbReviewer.Checked = false;
        cbAD.Checked = false;
		cbManageArea.Checked = false;

        //this.txt_DOB.Text = "";

        //this.RB_Sex.SelectedValue = null;
        this.txt_Address_1.Text = "";
        this.txt_Address_2.Text = "";
        this.txt_City.Text = "";
        this.txt_Region.Text = "";
        this.txt_Country.Text = "";
        this.txt_Postel.Text = "";
        this.txt_Home_Phone.Text = "";
        this.txt_Work_Phone.Text = "";
        this.txt_Work_Ext.Text = "";
        this.txt_Cell_Phone.Text = "";
        this.txt_Fax_Phone.Text = "";
        this.txt_Fax_Ext.Text = "";
        this.txt_Email.Text = "";
        this.txt_Website.Text = "";
        this.txt_TimeZone.Text = "";
        //this.RB_Language.SelectedValue = null;
        //this.RB_Status.SelectedValue = null;



        this.tbl_add_edit.Visible = false;
        this.tbl_Grid.Visible = true;
        this.tbl_showfield.Visible = false;

        ViewState["action"] = "add";
        this.txt_UserName.Enabled = true;
    }

    #endregion


    #region Grid_Events

    protected void GV_Main_RowDataBound(object sender, GridViewRowEventArgs e)
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
            if(drv["username"].ToString().Contains("Member:") || drv["username"].ToString() == "admin")
            {
                lb.Enabled = false;
                lb.Attributes.Add("style", "opacity:0.5");
            }
            else
                lb.Attributes.Add("OnClick", "return confirm('Are you sure you want to delete this user?');");

            //---------------------------------------------------------------------------
            //New code:

            ImageButton LB_Password = (ImageButton)e.Row.FindControl("LB_Password");
            ImageButton LB_Delete = (ImageButton)e.Row.FindControl("LB_Delete");

            if (Session["LoggedInID"].ToString() != "1")
            {
                string sql = @"declare @myqty int
                        select @myqty = count(*) from Users_Groups_Access where user_id = @userid

                        select @myqty as myqty, count(*) as userqty from Users_Groups_Access t1 inner join Users_Groups_Access t2 on t1.group_id = t2.group_id
                        where t1.user_id = @userid and t2.user_id=@otheruser";

                SqlDataAdapter dapt = new SqlDataAdapter(sql, ConfigurationManager.AppSettings.Get("CMServer"));
                dapt.SelectCommand.Parameters.AddWithValue("@userid", Session["LoggedInID"].ToString());
                dapt.SelectCommand.Parameters.AddWithValue("@otheruser", drv["id"].ToString());
                DataSet ds = new DataSet();
                dapt.Fill(ds);
                DataTable dt = ds.Tables[0];

                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToInt32(dt.Rows[0]["myqty"]) < Convert.ToInt32(dt.Rows[0]["userqty"]))
                    {
                        LB_Delete.Enabled = false;
                        LB_Delete.Attributes.Add("disabled", "disabled");
                        LB_Delete.Attributes.Add("style", "opacity:0.2");
                    }
                }
            }

            if (Convert.ToBoolean(drv["AD"]))
            {
                LB_Password.Enabled = false;
                LB_Password.Attributes.Add("disabled", "disabled");
                LB_Password.Attributes.Add("style", "opacity:0.2");
            }
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
        mDelete_Users(id);

       
        //mBindData((string)this.ViewState["sortExpression"].ToString());
        mBindData("", "");


        this.tbl_add_edit.Visible = false;
        this.tbl_Grid.Visible = true;
        this.tbl_showfield.Visible = false;

        ViewState["action"] = "add";
    }

    protected void GV_Main_RowEditing(object sender, GridViewEditEventArgs e)
    {
        this.tbl_Grid.Visible = false;
        this.tbl_add_edit.Visible = true;
        this.tbl_showfield.Visible = false;
        this.grprow.Visible = true;
        this.grprow_add.Visible = false;

        dgPermissionstbl.Visible = true;
        this.tbl_permission.Visible = false;

        Load_ListGroup();

        ViewState["action"] = "edit";

        Int32 id = Convert.ToInt32(this.GV_Main.DataKeys[e.NewEditIndex].Value.ToString());
        ViewState["record_id"] = id;

        //Response.Write(id.ToString());
        //////SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
        //////SqlDataAdapter dapt = new SqlDataAdapter("declare @temp TABLE(id int, name varchar(50)) insert into @temp(id,name) values(0,'Deny') insert into @temp(id,name) values(1,'View') insert into @temp(id,name) values(2,'Editor') insert into @temp(id,name) values(3,'Publisher') insert into @temp(id,name) values(4,'Administrator') select distinct uga.id, g.name as 'Group',t.name as 'Access' from Users_Groups_Access uga, Groups g, @temp t where uga.Access_Level=t.id and g.id=uga.Group_id and uga.User_id=" + id.ToString() + " order by g.name", sqlConn);
        //////DataSet ds = new DataSet();
        //////dapt.Fill(ds);

        DataSet ds = GetPermissionsList(id.ToString());
        ViewState["PermissionList"] = ds.Tables[0];


        dgPermissions.DataSource = ds.Tables[0];
        dgPermissions.DataBind();

        if (ds.Tables[0].Rows.Count < 1)
            dgPermissionstbl.Visible = false;


        DataTable dt = new DataTable();
        dt = mGet_One_Users(id);

        DataTable dt2 = new DataTable();
        dt2 = mGet_One_Users(id);

        litSetPermissions.Text = "<a href=\"/admin/users/userpermission.aspx?id=" + dt2.Rows[0]["id"].ToString() + "\" rel=\"gb_page_center[400, 330]\"><span Style=\"color:orange;text-decoration:none;\">Modify Permissions</span></a>";


        this.txt_UserName.Text = dt.Rows[0]["username"].ToString();
        this.txt_Name.Text = dt.Rows[0]["name"].ToString();
        this.txt_DOB.Text = Convert.ToDateTime(dt.Rows[0]["birthdate"].ToString()).ToShortDateString();
        //string s = dt.Rows[0]["sex"].ToString();
        try { this.RB_Sex.SelectedValue = dt.Rows[0]["sex"].ToString().Trim(); }
        catch { }
        this.txt_Address_1.Text = dt.Rows[0]["address1"].ToString();
        this.txt_Address_2.Text = dt.Rows[0]["address2"].ToString();
        this.txt_City.Text = dt.Rows[0]["city"].ToString();
        this.txt_Region.Text = dt.Rows[0]["region"].ToString();
        this.txt_Country.Text = dt.Rows[0]["country"].ToString();
        this.txt_Postel.Text = dt.Rows[0]["postalcode"].ToString();
        this.txt_Home_Phone.Text = dt.Rows[0]["homephone"].ToString();
        this.txt_Work_Phone.Text = dt.Rows[0]["workphone"].ToString();
        this.txt_Work_Ext.Text = dt.Rows[0]["workext"].ToString();
        this.txt_Cell_Phone.Text = dt.Rows[0]["cellphone"].ToString();
        this.txt_Fax_Phone.Text = dt.Rows[0]["faxphone"].ToString();
        this.txt_Fax_Ext.Text = dt.Rows[0]["faxext"].ToString();
        this.txt_Email.Text = dt.Rows[0]["email"].ToString();
        this.txt_Website.Text = dt.Rows[0]["website"].ToString();
        this.txt_TimeZone.Text = dt.Rows[0]["timezone"].ToString();
        this.RB_Language.SelectedValue = dt.Rows[0]["language"].ToString().Trim();
        this.RB_Status.SelectedValue = dt.Rows[0]["status"].ToString().ToLower().Trim();
        this.cbAdmin.Checked = (bool)dt.Rows[0]["getsallgroups"];
        this.cbReviewer.Checked = (bool)dt.Rows[0]["reviewer"];
        this.cbAD.Checked = (bool)dt.Rows[0]["AD"];
        // this.cbAD.Enabled = false;
		this.cbManageArea.Checked = (bool)dt.Rows[0]["ManageArea"];

        this.txt_UserName.Enabled = false;


        People(id);

        e.Cancel = true;
    }

    #endregion

    protected void bttn_Add_Question_Click(object sender, EventArgs e)
    {
        ViewState["action"] = "add";
        this.tbl_add_edit.Visible = true;
        this.tbl_Grid.Visible = false;
        this.tbl_showfield.Visible = false;

       // this.txt_UserName.Enabled = true;
      ////  this.asvUserName.Enabled = false;       // true;

        this.grprow.Visible = false;            // true;
        this.grprow_add.Visible = true; ;
        this.cbAD.Checked = false;

        LB_Group2.ClearSelection();
        RB_Permission2.ClearSelection();

        try { LB_Group2.Items.FindByText("Common").Selected = true; }
        catch { }
        RB_Permission2.SelectedValue = "2";

        pnlAdmin.Visible = false;
        //FontLabelG.Visible = false;

    }
    protected void btn_Submit_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid)
            return;

       //// this.asvUserName.Enabled = false;

        if (ViewState["action"].ToString() == "add")
        {

            mAdd_Users(this.txt_UserName.Text, "c21f969b5f03d33d43e04f8f136e7682", this.RB_Status.SelectedValue.ToString().Trim(), this.txt_Name.Text, this.txt_Address_1.Text, this.txt_Address_2.Text, this.txt_City.Text, this.txt_Region.Text, this.txt_Country.Text, this.txt_Postel.Text, this.txt_Home_Phone.Text, this.txt_Work_Phone.Text, this.txt_Work_Ext.Text, this.txt_Cell_Phone.Text, this.txt_Fax_Phone.Text, this.txt_Fax_Ext.Text, this.txt_Email.Text, this.RB_Sex.SelectedValue.ToString().Trim(), this.txt_DOB.Text, this.txt_Website.Text, this.txt_TimeZone.Text, this.RB_Language.SelectedValue.ToString().Trim(), "");
            Btn_Save_Click(ViewState["newid"].ToString());


            ShowPermisisons();

            pnlAdmin.Visible = true;
            People(int.Parse(ViewState["newid"].ToString()));

            ViewState["action"] = "edit";
            ViewState["record_id"] = ViewState["newid"].ToString();


            return;
        }
        else if (ViewState["action"].ToString() == "edit")
        {
            string s = ViewState["record_id"].ToString();
            mEdit_Users(s, this.RB_Status.SelectedValue.ToString(), this.txt_Name.Text, this.txt_Address_1.Text, this.txt_Address_2.Text, this.txt_City.Text, this.txt_Region.Text, this.txt_Country.Text, this.txt_Postel.Text, this.txt_Home_Phone.Text, this.txt_Work_Phone.Text, this.txt_Work_Ext.Text, this.txt_Cell_Phone.Text, this.txt_Fax_Phone.Text, this.txt_Fax_Ext.Text, this.txt_Email.Text, this.RB_Sex.SelectedValue.ToString().Trim(), this.txt_DOB.Text, this.txt_Website.Text, this.txt_TimeZone.Text, this.RB_Language.SelectedValue.ToString().Trim()); 

			Btn_Save_Click(sender, e);

            SAVE();

        }




        mBindData("", "");
        ViewState["action"] = "add";

        Clearfield();

		Response.Redirect("/admin.aspx?c=users");
    }

    private void ShowPermisisons()
    {
        dgPermissionstbl.Visible = true;
        this.tbl_permission.Visible = false;


        DataSet ds = GetPermissionsList(ViewState["newid"].ToString());
        ViewState["PermissionList"] = ds.Tables[0];


        dgPermissions.DataSource = ds.Tables[0];
        dgPermissions.DataBind();

        if (ds.Tables[0].Rows.Count < 1)
            dgPermissionstbl.Visible = false;
    }

    protected void btn_Cancel_step1_Click(object sender, EventArgs e)
    {
		dgPermissionstbl.Visible = false;
        Clearfield();


        GV_Main.EditIndex = -1;
        mBindData(sortExp, sortOrder);
    }


    protected void GV_Main_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "view")
        {

            this.tbl_showfield.Visible = true;
            DataTable dt = new DataTable();
            dt = mGet_One_Users(Convert.ToInt32(e.CommandArgument.ToString()));

            litSetPermissions.Text = "<a href=\"/admin/users/userpermission.aspx?id=" + dt.Rows[0]["id"].ToString() + "\" rel=\"gb_page_center[400, 330]\"><span Style=\"color:orange;text-decoration:none;\">Modify Permissions</span></a>";

            this.lbl_Name.Text = dt.Rows[0]["name"].ToString();
            this.lbl_Address1.Text = dt.Rows[0]["address1"].ToString();
            this.lbl_Address2.Text = dt.Rows[0]["address2"].ToString();
            this.lbl_City.Text = dt.Rows[0]["city"].ToString();
            this.lbl_Country.Text = dt.Rows[0]["country"].ToString();
            this.lbl_Postel.Text = dt.Rows[0]["postalcode"].ToString();
            this.lbl_HomePhone.Text = dt.Rows[0]["homephone"].ToString();
            this.lbl_Email.Text = dt.Rows[0]["email"].ToString();
            this.lbl_Status.Text = dt.Rows[0]["status"].ToString();
        }
        else if (e.CommandName == "password")
        {
            GeneratePassword(e.CommandArgument.ToString());

            Page.ClientScript.RegisterClientScriptBlock(GetType(), "alert", "alert('A new password was sent successfully');", true);
        }
        else if (e.CommandName == "People")
        {
            int id = Convert.ToInt32(e.CommandArgument.ToString());
            pnlAdmin.Visible = true;
            //FontLabelG.Visible = true;
            People(id);
        }

    }

    private void People(int id)
    {
        //int id = Convert.ToInt32(e.CommandArgument.ToString());

        lblId.Text = id.ToString();

        SqlDataAdapter dapt = new SqlDataAdapter("declare @temp TABLE(id int, name varchar(500), visible bit) insert into @temp(id,name,visible) select id,[text],'False' from adminmenu where parentid is not NULL order by [text] update @temp set visible='True' where id in (select adminmenuid from users_adminmenu where userid=" + id + ") insert into @temp(id,name,visible) select -1, 'Minisite/Program', (select minisite from users where id=@id) select * from @temp order by ltrim(name)", ConfigurationManager.AppSettings["CMServer"].ToString());
        dapt.SelectCommand.Parameters.Add("@id", id);
        DataSet ds = new DataSet();

        dapt.Fill(ds);

        dgMenu.DataSource = ds.Tables[0];
        dgMenu.DataBind();

        repMenu.DataSource = ds.Tables[0];
        repMenu.DataBind();

        pnlAdmin.Visible = true;
        //FontLabelG.Visible = true;
        tbl_Grid.Visible = false;
    }

    public void GeneratePassword(string userid)
    {
        //string newpwd = GenerateRandomString(6, "qwertyuiopasdfjklzxcvbnm1");

        string alpha = "qwertyuiopasdfjklzxcvbnm";
        string num = "1234567890";
        string nonalphanum = "!@#$%^*";
        string newpwd = GenerateRandomString(1, new string(alpha.Reverse().ToArray()).ToUpper())                                //Reverse to avoid duplicate the first letter as lowercase in second position
                        + GenerateRandomString(5, alpha)
                        + GenerateRandomString(1, nonalphanum)
                        + GenerateRandomString(1, num);
        newpwd = newpwd.Trim();

       
        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
        SqlCommand sqlComm = new SqlCommand("update users set password='" + HashString(newpwd) + "' where id=" + userid, sqlConn);

        sqlConn.Open();
        sqlComm.ExecuteNonQuery();
        sqlConn.Close();

        DataTable dt2 = new DataTable();
        dt2 = mGet_One_Users(Convert.ToInt32(userid));

        string subject = "Website Login Information";
        string body = @"Dear Internet User,
                        <br /><br /> 
                        Please find below your login information to access your content. 
                        <br /><br /> 
                        Please keep your username and password in a secure place. 
                        <br /><br /> 
                        If you have any questions or would like to arrange a training session, please contact the Communications Department.<br /><br />";

        //body += "Username: " + dt2.Rows[0]["username"].ToString() + "<br />Password: " + newpwd + "<br /><br />";
        body += "Username: " + dt2.Rows[0]["username"].ToString() + "<br />Your password will be send in a separate email.<br /><br />";
        body += "Please <a href='https://" + ConfigurationManager.AppSettings["SiteUrl"] + "/ChangePassword.aspx'>click here</a> to change your password.";
        body += @"<br /><br />
                Regards,<br /><br />
                Website Administrator";

        MailMessage message = new MailMessage(Email.FromName + "<" + Email.FromAddress + ">", dt2.Rows[0]["email"].ToString(), subject, body);
        //MailMessage message = new MailMessage(Email.FromName + "<" + Email.FromAddress + ">", dt2.Rows[0]["email"].ToString(), "Website Login Information", "Dear Content Editor,<br /><br />Please find below your login information to manage your content. You will be able to edit existing content as well as add new content.<br /><br />If you have any questions on how to edit content or would like to arrange a training session, please contact the Communications Department.<br /><br />Regards,<br />Website Administrator<br /><br /><br />Username: " + dt2.Rows[0]["username"].ToString() + "<br />Password: " + newpwd);
        message.IsBodyHtml = true;
        SmtpClient emailClient = new SmtpClient(Email.SmtpServer, Email.SmtpPort);
        if (Email.UseSmtpCredentials)
        {
            emailClient.Credentials = new System.Net.NetworkCredential(Email.SmtpUsername, Email.SmtpPassword);
            //emailClient.UseDefaultCredentials = true;
        }
        emailClient.EnableSsl = Email.EnableSsl;
        try
        {
            emailClient.Send(message);
        }
        catch (Exception ex)
        {
            Page.ClientScript.RegisterClientScriptBlock(GetType(), "alert", "alert('" + ex.Message + "');", true);
            return;
        }

        body = @"Dear Internet User,
                        <br /><br /> 
                        Please find below your login information to access your content. 
                        <br /><br />";

        body += "Password: " + newpwd;
        body += @"<br /><br />
                Regards,<br /><br />
                Website Administrator";
        message = new MailMessage(Email.FromName + "<" + Email.FromAddress + ">", dt2.Rows[0]["email"].ToString(), "Website Information", body);
        message.IsBodyHtml = true;
        try { emailClient.Send(message); }
        catch { }
    }

    //public void SAVE(object o, EventArgs e)
    //{
    //    SAVE();
    //}

    private void SAVE()
    {
        string s = lblId.Text;
        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"].ToString());
        SqlCommand sqlClear = new SqlCommand("delete from users_adminmenu where userid=" + s, sqlConn);

        sqlConn.Open();
        sqlClear.ExecuteNonQuery();

        /*
        foreach (DataGridItem di in dgMenu.Items)
        {
            int i = Convert.ToInt32(((Label)di.FindControl("lblId")).Text);
            bool enable = ((CheckBox)di.FindControl("cbVisible")).Checked;

            if (enable)
            {

                SqlCommand sqlComm = new SqlCommand("insert into users_adminmenu(adminmenuid,userid) values(" + i + ", " + s + ")", sqlConn);
                sqlComm.ExecuteNonQuery();
            }
        }*/

        int i = 1;
        foreach (DataListItem li in repMenu.Items)
        {
            CheckBox cb = (CheckBox)li.FindControl("cbOn");

            if (i < repMenu.Items.Count)
            {
                if (cb.Checked)
                {
                    SqlCommand sqlComm = new SqlCommand("insert into users_adminmenu(adminmenuid,userid) values(" + cb.ToolTip + ", " + s + ")", sqlConn);
                    sqlComm.ExecuteNonQuery();
                }
            }
            else
            {
                SqlCommand sqlComm = new SqlCommand("update users set minisite=@minisite where id=@id", sqlConn);
                sqlComm.Parameters.Add("@id", s);
                sqlComm.Parameters.Add("@minisite", cb.Checked ? "1" : "0");
                sqlComm.ExecuteNonQuery();
            }
            i++;
        }

        sqlConn.Close();

        pnlAdmin.Visible = false;
        //FontLabelG.Visible = false;

        tbl_Grid.Visible = true;
        mBindData("", "");
    }

    public string GenerateRandomString(int length, string charsToUse)
    {
        Random randomNumber = new Random();

        StringBuilder randomString = new StringBuilder();
        char appendedChar;

        for (int i = 0; i < length; i++)
        {
            int characterIndex = Convert.ToInt32(randomNumber.Next(i, charsToUse.Length - i));
            appendedChar = charsToUse[characterIndex];
            randomString.Append(appendedChar);
        }
        return randomString.ToString();
    }

    private string HashString(string Value)
    {
        System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] data = System.Text.Encoding.ASCII.GetBytes(Value);
        data = x.ComputeHash(data);
        string ret = "";
        for (int i = 0; i < data.Length; i++)
            ret += data[i].ToString("x2").ToLower();
        return ret;
    }

    protected override void OnPreRender(EventArgs e)
    {
        ScriptManager.RegisterClientScriptInclude(this, this.GetType(), "gAJS", ResolveClientUrl("/js/greybox/AJS.js"));
        ScriptManager.RegisterClientScriptInclude(this, this.GetType(), "gAJSfx", ResolveClientUrl("/js/greybox/AJS_fx.js"));
        ScriptManager.RegisterClientScriptInclude(this, this.GetType(), "gScripts", ResolveClientUrl("/js/greybox/gb_scripts.js"));

        base.OnPreRender(e);
    }


    //________________________________//


    public DataTable mGet_All_Groups()
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = "select * from groups order by name";
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


    public DataTable mGet_All_Users_Groups_Access_ByUserANDGroupid(int mID, int mGroupid)
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = "select ug.*,g.name from Users_Groups_Access ug, Groups g where ug.Group_id = g.id and Group_id = @mGroupid and User_id = @mID";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@mID", mID);
            cmd.Parameters.AddWithValue("@mGroupid", mGroupid);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds, "table1");
        }

        return ds.Tables[0];
    }

    public DataTable mGet_One_Users_Groups_Access(int mID)
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = "select * from Users_Groups_Access where id =  @mID";
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

    public DataTable mGet_All_Users_Groups_Access_ByUserid(int mID)
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = "select ug.*,g.name as [group], '' as access from Users_Groups_Access ug, Groups g where ug.Group_id = g.id  and User_id = @mID";
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

    public void ClearAccess(string userid)
    {
        ////Response.Write(userid);
        sb = sb.Remove(0, sb.Length);
        sb.Append("delete from users_groups_access where user_id=@User_id");

        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@User_id", userid);


            connection.Open();
            cmd.ExecuteScalar();


        }
    }

    public void mAdd_Users_Groups_Access(string User_id, string Group_id, string Access_Level)
    {

        sb = sb.Remove(0, sb.Length);
        sb.Append("delete from users_groups_access where user_id=@User_id and group_id=@Group_id insert into Users_Groups_Access  (User_id, Group_id, Access_Level) ");
        sb.Append(" values   (@User_id, @Group_id, @Access_Level)");

        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@User_id", User_id);
            cmd.Parameters.AddWithValue("@Group_id", Group_id);
            cmd.Parameters.AddWithValue("@Access_Level", Access_Level);


            connection.Open();
            cmd.ExecuteScalar();


        }

    }


    public void mUpdate_Permission(string id, string permission)
    {
        sb = sb.Remove(0, sb.Length);
        sb.Append(" update Users_Groups_Access ");
        sb.Append(" Set Access_Level = @permission ");

        sb.Append(" where id = @id ");
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);

            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@permission", permission);

            connection.Open();
            cmd.ExecuteNonQuery();
        }

    }






    protected void LB_Group_SelectedIndexChanged(object sender, EventArgs e)
    {
        RB_Permission.SelectedValue = "-1";

        LB_Group.Items.FindByValue("").Enabled = false;
        this.tbl_permission.Visible = true;
        string mUser_id = Request["id"];

        int id = Convert.ToInt32(this.LB_Group.SelectedValue.ToString());
        DataTable dt = new DataTable();
        //dt = mGet_All_Users_Groups_Access_ByUserANDGroupid(Convert.ToInt32(mUser_id), id);

        dt = (DataTable)ViewState["dt_permission"];
        DataRow[] dr = dt.Select("group_id = " + id);


        if (dr.Length != 0)
        {
            this.RB_Permission.SelectedValue = dr[0]["Access_Level"].ToString();

        }
        else
        {
            this.RB_Permission.SelectedValue = null;
        }
    }

    protected void Btn_Cancel_Click(object sender, EventArgs e)
    {
        this.tbl_permission.Visible = false;
        this.LB_Group.SelectedValue = null;
        this.LB_Group2.SelectedValue = null;

        string mUser_id = Request["id"];

        if (mUser_id != "")
        {
            DataTable dt = new DataTable();
            dt = mGet_All_Users_Groups_Access_ByUserid(Convert.ToInt32(mUser_id));
            ViewState["dt_permission"] = dt;

            this.GridView1.DataSource = dt;
            this.GridView1.DataBind();
        }
        dgPermissions.Visible = false;
    }

    public void tester(object o, EventArgs e)
    {
        ClearAccess(ViewState["record_id"].ToString());
    }

    public void Btn_Save_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        DataTable dt_Original = new DataTable();

        dt = (DataTable)ViewState["dt_permission"];
        dt_Original = mGet_All_Users_Groups_Access_ByUserid(Convert.ToInt32(ViewState["record_id"].ToString()));

        int i = dt.Rows.Count;
        int ii = dt_Original.Rows.Count;



        //ClearAccess(ViewState["record_id"].ToString());
        if (!cbAdmin.Checked)
            foreach (DataRow mRows in dt.Rows)
            {
                // if (mRows["id"].ToString() == "999999991")
                // {
                //add
                if (mRows["Access_Level"].ToString().Trim() != "0" && mRows["Access_Level"].ToString().Trim() != "-1")
                {
                    mAdd_Users_Groups_Access(mRows["User_id"].ToString(), mRows["Group_id"].ToString(),
                                             mRows["Access_Level"].ToString());
                }
                else if (mRows["Access_Level"].ToString().Trim() == "0")
                {
                    sb = sb.Remove(0, sb.Length);
                    sb.Append("delete from users_groups_access where user_id=@User_id and group_id=@Group_id");

                    string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
                    string commandString = sb.ToString();


                    using (SqlConnection connection = new SqlConnection(strConnectionString))
                    {
                        SqlCommand cmd = new SqlCommand(commandString, connection);
                        cmd.Parameters.AddWithValue("@User_id", mRows["User_id"].ToString());
                        cmd.Parameters.AddWithValue("@Group_id", mRows["Group_id"].ToString());


                        connection.Open();
                        cmd.ExecuteScalar();


                    }
                }
                // }
                // else
                /* {
					 foreach (DataRow mdr_original in dt_Original.Rows)
					 {
						 //if (mRows["id"].ToString() == mdr_original["id"].ToString())
						 //{
						 //	mAdd_Users_Groups_Access(mRows["User_id"].ToString(), mRows["Group_id"].ToString(), mdr_original["Access_Level"].ToString());
						 //}
					
						 //update
						 string s = mRows["id"].ToString();
						 string ss = mdr_original["id"].ToString();
						 if (mRows["id"].ToString() == mdr_original["id"].ToString())
						 {
							 if (mRows["Access_Level"].ToString() != mdr_original["Access_Level"].ToString())
							 {
								 mUpdate_Permission(mdr_original["id"].ToString(), mRows["Access_Level"].ToString());
								 break;
							 }
						 }
					
					 }

				 }*/

            }
        else
        {
            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"].ToString());
            SqlCommand sqlComm = new SqlCommand("delete from Users_Groups_Access where User_id=@id insert into Users_Groups_Access(User_id,Group_id,Access_Level) select @id,id,4 from groups", sqlConn);
            //////SqlCommand sqlComm = new SqlCommand("delete from Users_Groups_Access where User_id=@id insert into Users_Groups_Access(User_id,Group_id,Access_Level) select @id,id,5 from groups", sqlConn);
            sqlComm.Parameters.AddWithValue("@id", ViewState["record_id"].ToString());

            sqlConn.Open();
            sqlComm.ExecuteNonQuery();
            sqlConn.Close();

        }
        dgPermissionstbl.Visible = false;

        this.tbl_Grid.Visible = true;
        this.tbl_add_edit.Visible = false;
        this.tbl_showfield.Visible = false;
        //this.grprow.Visible = true;
        //dgPermissionstbl.Visible = true;


        //Page.ClientScript.RegisterStartupScript(GetType(),"asd","parent.parent.GB_hide();",true);
    }

    public void Btn_Save_Click(string s)
    {
        DataTable dt = new DataTable();
        DataTable dt_Original = new DataTable();

        dt = (DataTable)ViewState["dt_permission"];
        dt_Original = mGet_All_Users_Groups_Access_ByUserid(Convert.ToInt32(s));

        int i = dt.Rows.Count;
        int ii = dt_Original.Rows.Count;

        //ClearAccess(ViewState["record_id"].ToString());

        foreach (DataRow mRows in dt.Rows)
        {
            // if (mRows["id"].ToString() == "999999991")
            // {
            //add
            if (mRows["Access_Level"].ToString().Trim() != "0" && mRows["Access_Level"].ToString().Trim() != "-1")
            {
                mAdd_Users_Groups_Access(s, mRows["Group_id"].ToString(),
                                         mRows["Access_Level"].ToString());
            }
            else if (mRows["Access_Level"].ToString().Trim() == "0")
            {
                sb = sb.Remove(0, sb.Length);
                sb.Append("delete from users_groups_access where user_id=@User_id and group_id=@Group_id");

                string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
                string commandString = sb.ToString();


                using (SqlConnection connection = new SqlConnection(strConnectionString))
                {
                    SqlCommand cmd = new SqlCommand(commandString, connection);
                    cmd.Parameters.AddWithValue("@User_id", s);
                    cmd.Parameters.AddWithValue("@Group_id", mRows["Group_id"].ToString());


                    connection.Open();
                    cmd.ExecuteScalar();


                }
            }
            // }
            // else
            /* {
				 foreach (DataRow mdr_original in dt_Original.Rows)
				 {
					 //if (mRows["id"].ToString() == mdr_original["id"].ToString())
					 //{
					 //	mAdd_Users_Groups_Access(mRows["User_id"].ToString(), mRows["Group_id"].ToString(), mdr_original["Access_Level"].ToString());
					 //}
					
					 //update
					 string s = mRows["id"].ToString();
					 string ss = mdr_original["id"].ToString();
					 if (mRows["id"].ToString() == mdr_original["id"].ToString())
					 {
						 if (mRows["Access_Level"].ToString() != mdr_original["Access_Level"].ToString())
						 {
							 mUpdate_Permission(mdr_original["id"].ToString(), mRows["Access_Level"].ToString());
							 break;
						 }
					 }
					
				 }

			 }*/

        }
        /*dgPermissionstbl.Visible = false;

		this.tbl_Grid.Visible = true;
		this.tbl_add_edit.Visible = false;
		this.tbl_showfield.Visible = false;*/


        this.grprow.Visible = true;
        this.grprow_add.Visible = false;

        //dgPermissionstbl.Visible = true;


        //Page.ClientScript.RegisterStartupScript(GetType(),"asd","parent.parent.GB_hide();",true);
    }

    const int dummy_id = 999999991;

    //protected void RB_Permission_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    mAddAccess();
    //}

    public void mAddAccess(object o, EventArgs e)
    {
        DataTable dt = new DataTable();

        dt = (DataTable)ViewState["dt_permission"];
        string mGroup_id = this.LB_Group.SelectedValue.ToString();
        int i = 0;
        foreach (DataRow dr in dt.Rows)
        {
            if (mGroup_id == dr["Group_id"].ToString())
            {
                dr["Access_Level"] = this.RB_Permission.SelectedValue.ToString();
                i = 1;
                break;
            }
        }

        if (i == 0)
        {
            int id = dummy_id;
            if (dt.Rows.Count > 0)
            {
                id = int.Parse(dt.Rows[dt.Rows.Count - 1]["id"].ToString());
                id++;
            }
            DataRow dr = dt.NewRow();

            dr["id"] = id.ToString();                               // "999999991";

            ////Response.Write(dr["id"].ToString());

            if (ViewState["action"].ToString() == "edit")
                dr["User_id"] = ViewState["record_id"].ToString();//["id"].ToString();
            else
                dr["User_id"] = "0";//["id"].ToString();

            dr["Group_id"] = mGroup_id;
            dr["Access_Level"] = this.RB_Permission.SelectedValue.ToString();

            dt.Rows.Add(dr);

            RefreshPermissionsList(id);

        }

        ViewState["dt_permission"] = dt;
        this.GridView1.DataSource = dt;
        this.GridView1.DataBind();
    }

    private void RefreshPermissionsList(int id)
    {
        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
        DataTable dt = (DataTable)ViewState["PermissionList"];

        if (dt == null)
        {
            //Response.Write("id:" + id.ToString());
            dt = GetPermissionsList(id.ToString()).Tables[0];
            ViewState["PermissionList"] = dt;
            dgPermissions.DataSource = dt;
            dgPermissions.DataBind();
        }

        object[] rowVals = new object[6];
        DataRowCollection rowCollection = dt.Rows;
        rowVals[0] = id;
        rowVals[4] = LB_Group.SelectedItem.Text;
        rowVals[5] = RB_Permission.SelectedItem.Text;

        // Add and return the new row.
        DataRow row = rowCollection.Add(rowVals);

        dgPermissions.DataSource = dt;
        dgPermissions.DataBind();

        dgPermissionstbl.Visible = dgPermissions.Rows.Count > 0;
    }

    protected void dgPermissions_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "delPermission")
        {
            int id = int.Parse(e.CommandArgument.ToString());

            DataTable dt;
            if (id >= dummy_id)
            {
                for (int i = 0; i < dgPermissions.Rows.Count; i++)
                {
                    if (((Label)dgPermissions.Rows[i].FindControl("lblId")).Text == id.ToString())
                    {
                        dt = (DataTable)ViewState["PermissionList"];
                        dt.Rows[i].Delete();
                        ViewState["PermissionList"] = dt;
                        dgPermissions.DataSource = dt;
                        dgPermissions.DataBind();
                        break;
                    }
                }

                dt = (DataTable)ViewState["dt_permission"];
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["id"].ToString().Trim() == id.ToString())
                    {
                        dr.Delete();
                        ViewState["dt_permission"] = dt;
                        GridView1.DataSource = dt;
                        GridView1.DataBind();
                        break;
                    }
                }
            }
            else
            {
                #region Delete from DB
                SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"].ToString());
                SqlCommand sqlComm = new SqlCommand("delete from Users_Groups_Access where id=@id", sqlConn);
                sqlComm.Parameters.AddWithValue("@id", id);

                sqlConn.Open();
                sqlComm.ExecuteNonQuery();
                sqlConn.Close();
                #endregion;


                DataSet ds = GetPermissionsList(ViewState["record_id"].ToString());

                dt = (DataTable)ViewState["dt_permission"];
                foreach (DataRow dr in dt.Rows)
                {
                    object[] rowVals = new object[6];
                    DataRowCollection rowCollection = ds.Tables[0].Rows;
                    rowVals[0] = dr[0];
                    rowVals[1] = dr[1];
                    rowVals[2] = dr[2];
                    rowVals[3] = dr[3];
                    rowVals[4] = LB_Group.Items.FindByValue(dr[2].ToString()).Text;
                    rowVals[5] = RB_Permission.Items.FindByValue(dr[3].ToString()).Text;


                    // Add and return the new row.
                    DataRow row = rowCollection.Add(rowVals);
                }

                ViewState["PermissionList"] = ds.Tables[0];


                dgPermissions.DataSource = ds.Tables[0];
                dgPermissions.DataBind();

            }
        }
    }

    private static DataSet GetPermissionsList(string id)
    {
        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
        SqlDataAdapter dapt = new SqlDataAdapter("declare @temp TABLE(id int, name varchar(50)) insert into @temp(id,name) values(0,'Deny') insert into @temp(id,name) values(1,'View') insert into @temp(id,name) values(2,'Editor') insert into @temp(id,name) values(3,'Publisher') insert into @temp(id,name) values(4,'Administrator') select distinct uga.id, uga.User_id, uga.Group_id, uga.Access_Level, g.name as 'Group',t.name as 'Access' from Users_Groups_Access uga, Groups g, @temp t where uga.Access_Level=t.id and g.id=uga.Group_id and uga.User_id=@id order by g.name", sqlConn);
        dapt.SelectCommand.Parameters.AddWithValue("@id", id);

        DataSet ds = new DataSet();
        dapt.Fill(ds);

        return ds;
    }

    protected void dgPermissions_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView dr = (DataRowView)e.Row.DataItem;

            if (int.Parse(dr["id"].ToString()) < dummy_id)
            {
                //for delete button
                ImageButton lb;
                lb = (ImageButton)e.Row.FindControl("LB_Delete");
                lb.Attributes.Add("OnClick", "return confirm('Are you sure to delete this permission?');");
            }

        }
    }


    protected void dgPermissions_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }

    public void export(object o, EventArgs e)
    {
        SqlDataAdapter dapt = new SqlDataAdapter("select username, Name, email from users where id in (select User_id from Users_Groups_Access where Group_id=case when @groupid <> 0 then @groupid else Group_id end) and username!='admin' order by username ", ConfigurationManager.AppSettings.Get("CMServer"));
        dapt.SelectCommand.Parameters.AddWithValue("@groupid", ddlGroups.SelectedValue);
        DataSet ds = new DataSet();
        dapt.Fill(ds);

        DataTable dt = ds.Tables[0];

        string attachment = "attachment; filename=Export_Users.xls";

        Response.ClearContent();

        Response.AddHeader("content-disposition", attachment);

        Response.ContentType = "application/excel";
        Response.Charset = "utf-8";

        string tab = "";

        //foreach (DataColumn dc in dt.Columns)
        //{
        //    Response.Write(tab + dc.ColumnName);

        //    tab = "\t";
        //}

        //Response.Write("\n");


        int i;

        for (i = 0; i < dt.Columns.Count; i++)
        {
            Response.Write(tab + "\"" + dt.Columns[i].ColumnName + "\"");

            tab = "\t";
        }
        Response.Write("\n");

        foreach (DataRow dr in dt.Rows)
        {
            tab = "";

            for (i = 0; i < dt.Columns.Count; i++)
            {
                Response.Write(tab + "\"" + dr[i].ToString().Replace("\"", "'") + "\"");

                tab = "\t";
            }

            Response.Write("\n");
        }

        Response.End();
    }
    
    ////protected void asvUserName_ServerValidate(object source, ServerValidateEventArgs args)
    ////{
    ////    WSValidator.ValidationWS MyValidatorWS = new WSValidator.ValidationWS();

    ////    args.IsValid = true;        // MyValidatorWS.UserNameExists(args.Value);
    ////}

}
