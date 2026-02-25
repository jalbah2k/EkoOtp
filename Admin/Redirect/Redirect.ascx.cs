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

    StringBuilder sb;


    #region Template

    public DataTable mGet_All_Group()
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = "select * from redirect order by oldseo";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds, "table1");
        }

        if (ds.Tables[0].Rows.Count < 1)
        {
            tabletop.Visible = false;
            //ImageButton4.Visible = false;
        }
        else
        {
            tabletop.Visible = true;
            //ImageButton4.Visible = true;
        }

        return ds.Tables[0];
    }



    public DataTable mGet_One_Group(int mID)
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = "select * from redirect where id = @mID order by oldseo";
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

    public void mAdd_Group(string name, string url)
    {

        sb = sb.Remove(0, sb.Length);
        sb.Append(" insert into redirect(oldseo, newseo) ");
        sb.Append(" values   (@name, @url)");
        //sb.Append(" SELECT id FROM Template WHERE (id = SCOPE_IDENTITY()) ");
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@url", url);
           

            connection.Open();
            cmd.ExecuteNonQuery();
         }

    }

    public void mEdit_Group(string id, string name, string description, string color)
    {
        sb = sb.Remove(0, sb.Length);
        sb.Append(" update Groups ");
        sb.Append(" Set name = @name, ");
        sb.Append(" description = @description, ");
        sb.Append(" color = @color ");

        sb.Append(" where id = @id ");
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);

            cmd.Parameters.AddWithValue("@id", id);
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
        sb.Append(" Delete from redirect ");
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

    protected void Page_Load(object sender, EventArgs e)
    {
        sb = new StringBuilder(60);
        this.lbl_msg.Text = "";
        if (!IsPostBack)
        {
            ViewState["action"] = "add";
            sortExp = "oldseo"; // default sorted column
            sortOrder = "asc";    // default sort order


            this.tbl_add_edit.Visible = false;
            this.tbl_Grid.Visible = true;


            ViewState["record_id"] = "";

            mBindData();

            //to be delete
            Session["user_id"] = "-1";
            newseo.Text = "https://";
        }


		


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
        dt = mGet_All_Group();
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
        this.txt_Name.Text = "";
        newseo.Text = "https://";
       // ddlLanguage.SelectedIndex = 0;
       // ddlPages.ClearSelection();

        this.tbl_add_edit.Visible = false;
        this.tbl_Grid.Visible = true;
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
            //for delete button
            ImageButton lb;
            lb = (ImageButton)e.Row.FindControl("LB_Delete");
            lb.Attributes.Add("OnClick", "return confirm('Are you sure to delete this Record?');");
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

        //Delete Dependent records
        // mDelete_Menu_Group(id);
        // mDelete_Pages_Group(id);
        // mDelete_Users_Groups_Access(id);


        mBindData();

        this.tbl_add_edit.Visible = false;
        this.tbl_Grid.Visible = true;

        ViewState["action"] = "add";
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

        this.txt_Name.Text = dt.Rows[0]["oldseo"].ToString();
       // ddlLanguage.ClearSelection();
        newseo.Text = dt.Rows[0]["newseo"].ToString();        

		//SqlDataAdapter dapt = new SqlDataAdapter("declare @temp TABLE(id int, name varchar(500), visible bit) insert into @temp(id,name,visible) select id,[text],'False' from adminmenu where parentid is not NULL update @temp set visible='True' where id in (select adminmenuid from AdminMenu_Groups where groupid=" + id.ToString() + ") select * from @temp",ConfigurationManager.AppSettings["CMServer"].ToString());
		//DataSet ds = new DataSet();

    	//dapt.Fill(ds);

    	//dgMenu.DataSource = ds.Tables[0];
		//dgMenu.DataBind();
    }

    #endregion


    protected void bttn_Add_Question_Click(object sender, EventArgs e)
    {
        this.tbl_add_edit.Visible = true;
        this.tbl_Grid.Visible = false;
    }


    protected void btn_Submit_Click(object sender, EventArgs e)
    {
        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"].ToString());
        sqlConn.Open();
        //Add
        if (ViewState["action"].ToString() == "add")
        {
            //mAdd_Group(this.txt_Name.Text, ddlLanguage.SelectedValue, ddlPages.SelectedValue);

            SqlCommand sqlComm = new SqlCommand("insert into redirect(oldseo,newseo) values(@name,@newseo)", sqlConn);
            sqlComm.Parameters.AddWithValue("@name", txt_Name.Text.Trim());
            string url = newseo.Text.Trim();
            if (!url.StartsWith("http") && !url.StartsWith("/"))
            {
                url = "https://" + url;
            }

            sqlComm.Parameters.AddWithValue("@newseo", url);

            sqlComm.ExecuteNonQuery();

        }
        else if (ViewState["action"].ToString() == "edit")
        {
            string s = ViewState["record_id"].ToString();
			SqlCommand sqlComm = new SqlCommand("update redirect set oldseo=@name,newseo=@newseo where id=@id",sqlConn);
            sqlComm.Parameters.AddWithValue("@name", txt_Name.Text.Trim());
            string url = newseo.Text.Trim();
            if (!url.StartsWith("http") && !url.StartsWith("/"))
            {
                url = "https://" + url;
            }

            sqlComm.Parameters.AddWithValue("@newseo", url);
            sqlComm.Parameters.AddWithValue("@id", s);
                
			sqlComm.ExecuteNonQuery();
        }

        sqlConn.Close();

        this.GV_Main.EditIndex = -1;
        mBindData();
        ViewState["action"] = "add";

        Clearfield();
    }
    protected void btn_Cancel_step1_Click(object sender, EventArgs e)
    {
        Clearfield();
        this.GV_Main.EditIndex = -1;
        mBindData();
    }
}
