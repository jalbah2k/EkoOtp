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
using System.Web.UI.HtmlControls;

public partial class Admin_Pages_Pages : System.Web.UI.UserControl
{
    #region DAL

    StringBuilder sb;


    #region Pages

	public void pager_Command(object sender, CommandEventArgs e)
	{
		int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
		pager1.CurrentIndex = currnetPageIndx;
		GV_Main.PageIndex = currnetPageIndx-1;
		//BindRepeater();
		mBindData("");
	}

    public DataTable mGet_All_Page()
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = "select * from Pages where id in (select page_id from pages_group where group_id in (select group_id where user_id=" + Session["LoggedInId"].ToString() + " and access_level>1))";
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

    public int mCheck_SeoName(string seo, string lang_id)
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = "select * from Pages where language = @lang_id and seo = @seo";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@seo", seo);
            cmd.Parameters.AddWithValue("@lang_id", lang_id);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds, "table1");
        }

        return ds.Tables[0].Rows.Count;
    }

    public int mCheck_SeoName_Edit(string seo, string lang_id, string id)
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = "select * from Pages where language = @lang_id and seo = @seo and id <> @id";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@seo", seo);
            cmd.Parameters.AddWithValue("@lang_id", lang_id);
            cmd.Parameters.AddWithValue("@id", id);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds, "table1");
        }

        return ds.Tables[0].Rows.Count;
    }



    public DataTable mGet_All_Page_Grid()
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = "select p.*,lay.name as 'layout_name',l.name as 'Language_Name',g.name as gname from pages p, Languages l,layouts lay, groups as g where g.id in (select top 1 group_id from pages_group where page_id = p.id) and p.layout = lay.id and p.language = l.id and p.id in (select page_id from pages_group where group_id in (select group_id from users_groups_access where user_id=" + Session["LoggedInId"].ToString() + " and access_level>1)) and p.name like '%" + txtFilter.Text + "%' order by p.name";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            //cmd.Parameters.AddWithValue("@FIL", txtFilter.Text.Trim());
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


    public DataTable mGet_One_Page_Linkid(int mLinkid)
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = "select * from Pages where linkid = @mLinkid and id <> @mLinkid";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@mLinkid", mLinkid);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds, "table1");
        }

        return ds.Tables[0];



    }



    public DataTable mGet_One_Page(int mID)
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = "select * from Pages where id = @mID";
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

    public int mAdd_Page(string name, string layout, string language, string title, string keywords, string description, string seo, string linkid)
    {

        sb = sb.Remove(0, sb.Length);
        sb.Append(" insert into Pages  (name, layout, language, title, keywords, description, seo, linkid) ");
        sb.Append(" values   (@name, @layout, @language, @title, @keywords, @description, @seo, @linkid)");
        sb.Append(" SELECT id FROM Pages WHERE (id = SCOPE_IDENTITY()) ");
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@layout", layout);
            cmd.Parameters.AddWithValue("@language", language);
            cmd.Parameters.AddWithValue("@title", title);
            cmd.Parameters.AddWithValue("@keywords", keywords);
            cmd.Parameters.AddWithValue("@description", description);
            cmd.Parameters.AddWithValue("@seo", seo);
            cmd.Parameters.AddWithValue("@linkid", linkid);




            connection.Open();
            int i = Convert.ToInt32(cmd.ExecuteScalar());
            return i;

        }

    }

    public void mEdit_Page(string id, string name, string layout, string language, string title, string keywords, string description, string seo)
    {
        sb = sb.Remove(0, sb.Length);
        sb.Append(" update Pages ");
        sb.Append(" Set name = @name, ");
        sb.Append(" layout = @layout, ");
        sb.Append(" language = @language, ");
        sb.Append(" title = @title, ");
        sb.Append(" keywords = @keywords, ");
        sb.Append(" description = @description, ");
        sb.Append(" seo = @seo ");



        sb.Append(" where id = @id ");
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);

            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@layout", layout);
            cmd.Parameters.AddWithValue("@language", language);
            cmd.Parameters.AddWithValue("@title", title);
            cmd.Parameters.AddWithValue("@keywords", keywords);
            cmd.Parameters.AddWithValue("@description", description);
            cmd.Parameters.AddWithValue("@seo", seo);



            connection.Open();
            cmd.ExecuteNonQuery();
        }

    }

    public void mUpdateLinkid(string id)
    {
        sb = sb.Remove(0, sb.Length);
        sb.Append(" update Pages ");
        sb.Append(" Set linkid = @id ");


        sb.Append(" where id = @id ");
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);

            cmd.Parameters.AddWithValue("@id", id);

            connection.Open();
            cmd.ExecuteNonQuery();
        }

    }


    public void mDelete_Page(int mID)
    {
        sb = sb.Remove(0, sb.Length);
        sb.Append(" Delete from Pages ");
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

    public void mDelete_Pages_Group(int mID)
    {
        sb = sb.Remove(0, sb.Length);
        sb.Append(" Delete from Pages_Group ");
        sb.Append(" where Page_id = @id ");

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

    public void mDelete_Page_ByLinkid(int mID)
    {
        sb = sb.Remove(0, sb.Length);
        sb.Append(" Delete from Pages ");
        sb.Append(" where linkid = @id ");

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


    public void mDelete_MenuGroups(int mID)
    {
        sb = sb.Remove(0, sb.Length);
        sb.Append(" Delete from Menu_Group ");
        sb.Append(" where id  = (select mg.id from MenuItems mi, Menu_Group mg where mg.MenuItem_id = mi.id and mi.pageid = @id) ");

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






    #region General_Functions

    public DataTable mGet_All_Template()
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = "select * from Template";
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

    public DataTable mGet_All_Template(string id)
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = "select * from Template where id = @id";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@id", id);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds, "table1");
        }

        return ds.Tables[0];
    }


    public DataTable mGet_Template_Contents_ByTemplateID(string id, string lang_id)
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = "select * from TemplateContents where Template_id = @id and Language_id = @lang_id";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@lang_id", lang_id);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds, "table1");
        }

        return ds.Tables[0];
    }


    public void mAdd_Menu(string menuid, string parentid, string pageid, string priority, string text, string tooltip, string navigateurl, string target, string visible, string enabled)
    {

        sb = sb.Remove(0, sb.Length);
        sb.Append(" insert into MenuItems  (menuid, parentid, pageid, priority, text, tooltip, navigateurl, target, visible, enabled) ");
        sb.Append(" values   (@menuid, @parentid, @pageid, @priority, @text, @tooltip, @navigateurl, @target, @visible, @enabled)");
        //sb.Append(" SELECT id FROM Pages WHERE (id = SCOPE_IDENTITY()) ");
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@menuid", menuid);

            if (parentid == "")
            {
                cmd.Parameters.AddWithValue("@parentid", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@parentid", parentid);
            }

            cmd.Parameters.AddWithValue("@pageid", pageid);
            cmd.Parameters.AddWithValue("@priority", priority);
            cmd.Parameters.AddWithValue("@text", text);
            cmd.Parameters.AddWithValue("@tooltip", tooltip);
            cmd.Parameters.AddWithValue("@navigateurl", navigateurl);
            cmd.Parameters.AddWithValue("@target", target);
            cmd.Parameters.AddWithValue("@visible", visible);
            cmd.Parameters.AddWithValue("@enabled", enabled);

            connection.Open();
            cmd.ExecuteScalar();


        }

    }

    public void mAdd_Pages_Content_Zone(string Page_ID, string Content_ID, string Zone_ID, string Priority)
    {

        sb = sb.Remove(0, sb.Length);
        sb.Append(" insert into Pages_Content_Zone  (Page_ID,Content_ID,Zone_ID,Priority) ");
        sb.Append(" values   (@Page_ID,@Content_ID,@Zone_ID,@Priority)");
        //sb.Append(" SELECT id FROM Pages WHERE (id = SCOPE_IDENTITY()) ");
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@Page_ID", Page_ID);
            cmd.Parameters.AddWithValue("@Content_ID", Content_ID);
            cmd.Parameters.AddWithValue("@Zone_ID", Zone_ID);
            cmd.Parameters.AddWithValue("@Priority", Priority);




            connection.Open();
            cmd.ExecuteScalar();


        }

    }

    public void mUpdate_Priority(string id, string priority)
    {
        sb = sb.Remove(0, sb.Length);
        sb.Append(" update MenuItems ");
        sb.Append(" Set priority = @priority ");

        sb.Append(" where id = @id ");
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);

            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@priority", priority);

            connection.Open();
            cmd.ExecuteNonQuery();
        }

    }

    public void mDelete_MenuItems(int mPageid)
    {
        sb = sb.Remove(0, sb.Length);
        sb.Append(" Delete from MenuItems ");
        sb.Append(" where pageid = @mPageid ");

        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@mPageid", mPageid);

            connection.Open();
            cmd.ExecuteNonQuery();
        }
    }


    public void mDelete_PagesContentZones(int mPageid)
    {
        sb = sb.Remove(0, sb.Length);
        sb.Append(" Delete from Pages_Content_Zone ");
        sb.Append(" where Page_ID = @mPageid ");

        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@mPageid", mPageid);

            connection.Open();
            cmd.ExecuteNonQuery();
        }
    }


    public DataTable mGet_One_Menu(int mID)
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = "select * from MenuItems where pageid = @mID";
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

    #endregion







    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        sb = new StringBuilder(60);
        this.lbl_msg.Text = "";
      
        if (!IsPostBack)
        {
            
            ViewState["sortExpression"] = "";
            ViewState["sortD"] = "ASC";

           
            mBindData("");
           

            //to be delete
            Session["user_id"] = "-1";
        }
    }



    #region My_Functions

	public void filternow(object o, EventArgs e)
	{
		mBindData("");
	}

    public void filter(object o, EventArgs e)
    {
        mBindData("");
    }

    private void mBindData(string sortExp)
    {
        DataTable dt = new DataTable();
        dt = mGet_All_Page_Grid();
        DataView DV = dt.DefaultView;
        if (!(sortExp == string.Empty))
        {
            DV.Sort = sortExp;
        }
        this.GV_Main.DataSource = DV;
        this.GV_Main.DataBind();

		repPages.DataSource = DV;
		repPages.DataBind();

		pager1.ItemCount = DV.Count;
    }

    #endregion



    #region Grid_Events

    protected void GV_Main_DataBound(object sender, EventArgs e)
    {
        for (int i = 0; i < this.GV_Main.Rows.Count; i++)
        {
			TableCell cell = (TableCell)this.GV_Main.Rows[i].Cells[2];
			if (cell.Text.Length > 28)
				cell.Text = cell.Text.Substring(0, 26) + "...";

            //for delete button
            ImageButton lb;
            lb = (ImageButton)this.GV_Main.Rows[i].Cells[5].FindControl("LB_Delete");
            lb.Attributes.Add("OnClick", "return confirm('Are you sure to delete this page?');");


        }
    }

    protected void GV_Main_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.GV_Main.PageIndex = e.NewPageIndex;
        mBindData((string)ViewState["sortExpression"]);
    }

    protected void GV_Main_Sorting(object sender, GridViewSortEventArgs e)
    {
        if ((string)ViewState["sortD"] == "ASC")
        {
            ViewState["sortD"] = "DESC";
            ViewState["sortExpression"] = e.SortExpression + " DESC";
            mBindData(e.SortExpression + " DESC");
        }

        else
        {
            if ((string)ViewState["sortD"] == "DESC")
            {
                ViewState["sortD"] = "ASC";
                ViewState["sortExpression"] = e.SortExpression + " ASC";
                mBindData(e.SortExpression + " ASC");
            }
        }

    }


    protected void GV_Main_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        Int32 id = Convert.ToInt32(this.GV_Main.DataKeys[e.RowIndex].Value.ToString());

        DataTable dt = new DataTable();
        dt = mGet_One_Page(id);

        int mLink_id;

        if (dt.Rows[0]["id"].ToString() == dt.Rows[0]["linkid"].ToString())
        {
            //english
            mLink_id = id;
        }
        else
        {
            //french
            mLink_id = Convert.ToInt32(dt.Rows[0]["linkid"].ToString());
        }


        int mNewLinkid = mLink_id;

        if (id == mLink_id)
        {
            dt = mGet_One_Page_Linkid(id);
			if(dt.Rows.Count > 0)
				mNewLinkid = Convert.ToInt32(dt.Rows[0]["id"].ToString());
        }



        //Validation - Check child record
        DataTable dt1 = new DataTable();
        dt1 = mGet_Menuitems_ByParentid(Convert.ToInt32(id));

        if (dt1.Rows.Count != 0)
        {
            ViewState["MyId"] = id;
            ViewState["MyLink_id"] = mLink_id;
            ViewState["MyNewLinkid"] = mNewLinkid;
            divpopup.Visible = true;
            return;
        }
        //end validation

        DeleteProcedure(id, mLink_id, mNewLinkid);

    }

    private void DeleteProcedure(Int32 id, int mLink_id, int mNewLinkid)
    {
        ////Delete Pages////
        mDelete_Page(id);
        //Related pages

        if ((bool)Session["Multilingual"])
            mDelete_Page_ByLinkid(mLink_id);


        ////Delete Menu_Group////
        mDelete_MenuGroups(id);
        //Related pages
        if ((bool)Session["Multilingual"])
            mDelete_MenuGroups(mNewLinkid);


        ////Delete Pages_group////
        mDelete_Pages_Group(id);
        //Related pages
        if ((bool)Session["Multilingual"])
            mDelete_Pages_Group(mNewLinkid);


        ////Delete MenusItems////  
        mDelete_MenuItems(id);
        //Related pages
        if ((bool)Session["Multilingual"])
            mDelete_MenuItems(mNewLinkid);


        ////Delete pages_Control_zone////
        mDelete_PagesContentZones(id);
        //Related pages
        if ((bool)Session["Multilingual"])
            mDelete_PagesContentZones(mNewLinkid);




        //this.lbl_msg.Text = "Record has been deleted successfully";
        mBindData((string)this.ViewState["sortExpression"].ToString());
    }

    #region To check for children items
    private DataTable mGet_Menuitems_ByParentid(int mPageid)
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = " declare @id int select @id=id from MenuItems where pageid = @mPageid select * from MenuItems where parentid = @id";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@mPageid", mPageid);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds);
        }

        return ds.Tables[0];
    }
    #endregion

    #endregion
    protected void bttn_Add_Question_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/admin.aspx?c=wizard");

    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        int id = int.Parse(ViewState["MyId"].ToString());
        int Link_id = int.Parse(ViewState["MyLink_id"].ToString());
        int NewLinkid = int.Parse(ViewState["MyNewLinkid"].ToString());
        DeleteProcedure(id, Link_id, NewLinkid);
        divpopup.Visible = false;

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        divpopup.Visible = false;
    }
    protected void GV_Main_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.DataItem != null)
        {
            DataRowView dr = (DataRowView)e.Row.DataItem;
            HtmlAnchor lkSeo = (HtmlAnchor)e.Row.FindControl("lkSeo");
            lkSeo.HRef = (dr["language"].ToString() == "1" ? "/en/" : "/fr/") + dr["seo"].ToString() ;
        }
    }
}
