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

    SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("CMServer"));
    StringBuilder sb;


    #region Pages

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

        mBindData();
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
////        string commandString = "select p.*,lay.name as 'layout_name',l.name as 'Language_Name',g.name as gname from pages p, Languages l,layouts lay, groups as g where g.id in (select top 1 group_id from pages_group where page_id = p.id) and p.layout = lay.id and p.language = l.id and p.id in (select page_id from pages_group where group_id in (select group_id from users_groups_access where user_id=" + Session["LoggedInId"].ToString() + " and access_level>1)) and p.name like '%" + txtFilter.Text + "%' order by p.name";
        //string commandString = "select p.*,lay.name as 'layout_name',l.name as 'Language_Name',g.name as gname, (select name from Pages where id = p.linkid) as EnglishName from pages p, Languages l,layouts lay, groups as g where g.id in (select top 1 group_id from pages_group where page_id = p.id) and p.layout = lay.id and p.language = l.id and p.id in (select page_id from pages_group where group_id in (select group_id from users_groups_access where user_id=" + Session["LoggedInId"].ToString() + " and access_level>1)) and p.name like '%" + txtFilter.Text + "%' and p.id in (select Page_id from Pages_Group where Group_id in (select id from groups where id = case when @groupid <> 0 then @groupid else id end)) order by EnglishName, language";
        string commandString = "select p.*,lay.name as 'layout_name',l.name as 'Language_Name',g.name as gname, (select name from Pages where id = p.linkid) as EnglishName from pages p, Languages l,layouts lay, groups as g where g.id in (select top 1 group_id from pages_group where page_id = p.id) and p.layout = lay.id and p.language = l.id and p.id in (select page_id from pages_group where group_id in (select group_id from users_groups_access where user_id=" + 
            Session["LoggedInId"].ToString() + " and access_level>1) or " + Session["LoggedInId"].ToString() + "=1) and (@filter is null or p.name like '%' + @filter + '%' or p.seo like '%' + @filter + '%') and p.id in (select Page_id from Pages_Group where Group_id in (select id from groups where id = case when @groupid <> 0 then @groupid else id end) and (@lang is null or p.language=@lang)) order by EnglishName, language";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            //cmd.Parameters.AddWithValue("@FIL", txtFilter.Text.Trim());
            cmd.Parameters.AddWithValue("@groupid", ddlGroups.SelectedValue);
            if (txtFilter.Text.Trim().Length > 0)
                cmd.Parameters.AddWithValue("@filter", txtFilter.Text.Trim());
            else
                cmd.Parameters.AddWithValue("@filter", DBNull.Value);
            if ((bool)Session["Multilingual"])
                cmd.Parameters.AddWithValue("@lang", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@lang", 1);
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
        sb.Append(" update PagesRemoved set [User] = @userid where id = @id");

        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@id", mID);
            cmd.Parameters.AddWithValue("@userid", Session["LoggedInId"].ToString());

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
    public string sortExpTC
    {
        get
        {
            return ViewState["sortExpTC"] != null ? ViewState["sortExpTC"].ToString() : "";
        }
        set
        {
            ViewState["sortExpTC"] = value;
        }
    }
    public string sortOrderTC
    {
        get
        {
            return ViewState["sortOrderTC"] != null ? ViewState["sortOrderTC"].ToString() : "desc";
        }
        set
        {
            ViewState["sortOrderTC"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        sb = new StringBuilder(60);
        this.lbl_msg.Text = "";
      
        if (!IsPostBack)
        {

            //ViewState["sortExpression"] = "";
            //ViewState["sortD"] = "ASC";
            sortExp = "EnglishName"; // default sorted column
            sortOrder = "asc";    // default sort order
            //ViewState["sortExpressionTC"] = "";
            //ViewState["sortDTC"] = "ASC";
            sortExpTC = "ZoneName"; // default sorted column
            sortOrderTC = "asc";    // default sort order

            BindEverything();
            mBindData();
           

            //to be delete
            Session["user_id"] = "-1";
        }
    }



    #region My_Functions

	public void filternow(object o, EventArgs e)
	{
        mBindData();
    }

    public void filter(object o, EventArgs e)
    {
        mBindData();
    }

    private void mBindData()
    {
        mBindData(sortExp, sortOrder);
    }
    private void mBindData(string sortExp, string sortDir)
    {
        DataTable dt = new DataTable();
        dt = mGet_All_Page_Grid();
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

    private void BindTimedContent()
    {
        BindTimedContent(sortExpTC, sortOrderTC);
    }
    private void BindTimedContent(string sortExp, string sortDir)
    {
        string sqlcmd = "select pcz.*, p.seo, c.name as ContentName, c.param as HtmlId, z.name as ZoneName, CAST(case when StartDate is null and EndDate is null and StartTime is null and EndTime is null then 1 else 0 end as bit) as isDefaultContent from Pages_Content_Zone pcz inner join Content c on pcz.Content_ID=c.id inner join Zones z on pcz.Zone_ID=z.id inner join Pages p on pcz.Page_ID=p.id where pcz.Page_ID=@Page_ID and c.control='content' order by pcz.Zone_ID, pcz.Priority";

        SqlDataAdapter dapt = new SqlDataAdapter(sqlcmd, conn);
        dapt.SelectCommand.Parameters.AddWithValue("@Page_ID", ViewState["Page_ID"].ToString());
        DataTable dt = new DataTable();
        dapt.Fill(dt);

        ViewState["Page_SEO"] = dt.Rows[0]["seo"].ToString();

        DataView DV = dt.DefaultView;
        if (!(sortExp == string.Empty))
        {
            DV.Sort = string.Format("{0} {1}", sortExp, sortDir);
        }
        this.gvTimedContent.DataSource = DV;
        this.gvTimedContent.DataBind();

        pager2.ItemCount = DV.Count;
        pager2.Visible = dt.Rows.Count > gvTimedContent.PageSize;

        litPagerShowingTC.Text = CMSHelper.GetPagerInfo(gvTimedContent, DV.Count);
    }

    private void BindEverything()
    {
        SqlDataAdapter dapt = new SqlDataAdapter("select * from Zones select * from Groups", conn);
        DataSet ds = new DataSet();
        dapt.Fill(ds);

        ddlZones.DataSource = ds.Tables[0];
        ddlZones.DataTextField = "name";
        ddlZones.DataValueField = "id";
        ddlZones.DataBind();
        ddlZones.Items.Insert(0, new ListItem("Select One", ""));

        DateTime time = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
        DateTime end = time.AddDays(1);;

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


        DataView DV = ds.Tables[1].DefaultView;
        DV.Sort = "name";
        ddlGroups.DataTextField = "name";
        ddlGroups.DataValueField = "id";
        ddlGroups.DataSource = DV;
        ddlGroups.DataBind();
        ddlGroups.Items.Insert(0, new ListItem("Filter by group", "0"));
    }
    #endregion



    #region Grid_Events

    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        // Language column
        e.Row.Cells[2].Visible = (bool)Session["Multilingual"];

        #region Add sorted class to headers
        if (e.Row.RowType == DataControlRowType.Header)
        {
            int colIndex = CMSHelper.GetColumnIndex((sender as GridView), sortExp);
            e.Row.Cells[colIndex].CssClass = sortOrder == "asc" ? "sortasc" : "sortdesc";
        }
        #endregion Add sorted class to headers

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //TableCell cell = (TableCell)e.Row.Cells[2];  // Name column
            //if (cell.Text.Length > 28)
            //{
            //    cell.ToolTip = cell.Text;
            //    cell.Text = cell.Text.Substring(0, 26) + "...";
            //}

            //cell = (TableCell)e.Row.Cells[4];    // Group column
            //if (cell.Text.Length > 18)
            //{
            //    cell.ToolTip = cell.Text;
            //    cell.Text = cell.Text.Substring(0, 16) + "...";
            //}

            //cell = (TableCell)e.Row.Cells[8];    // Seo column
            //if (cell.Text.Length > 18)
            //{
            //    cell.ToolTip = cell.Text;
            //    cell.Text = cell.Text.Substring(0, 16) + "...";
            //}
            
            
            //for delete button
            ImageButton lb;
            lb = (ImageButton)e.Row.FindControl("LB_Delete");
            lb.Attributes.Add("OnClick", "return confirm('Are you sure to delete this page?');");

            DataRowView dr = (DataRowView)e.Row.DataItem;
            HtmlAnchor lkSeo = (HtmlAnchor)e.Row.FindControl("lkSeo");
            //lkSeo.HRef = (dr["language"].ToString() == "1" ? "/en/" : "/fr/") + dr["seo"].ToString();
            lkSeo.HRef = (dr["language"].ToString() == "1" ? CMSHelper.SeoPrefixEN : "/fr/") + dr["seo"].ToString();


            CheckBox cbxActive = (CheckBox)e.Row.FindControl("cbxActive");
            cbxActive.Checked = Convert.ToBoolean(dr["Active"]);

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



        ////Delete Pages////
        mDelete_Page(id);
        //Related pages

		if((bool)Session["Multilingual"])
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
        //mBindData((string)this.ViewState["sortExpression"].ToString());
        mBindData();
    }

    public void rowcommand(object o, CommandEventArgs e)
    {
        switch (e.CommandName.ToLower())
        {
            case "managetimedcontent":
                pnlList.Visible = false;
                pnlManageTimedContent.Visible = true;

                ViewState["Page_ID"] = e.CommandArgument.ToString();    // Page ID

                BindTimedContent();

                break;
            default:
                break;
        }
    }

    protected void gvTimedContent_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        #region Add sorted class to headers
        if (e.Row.RowType == DataControlRowType.Header)
        {
            int colIndex = CMSHelper.GetColumnIndex((sender as GridView), sortExpTC);
            e.Row.Cells[colIndex].CssClass = sortOrderTC == "asc" ? "sortasc" : "sortdesc";
        }
        #endregion Add sorted class to headers

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView dr = (DataRowView)e.Row.DataItem;
            
            //TableCell cell = (TableCell)e.Row.Cells[2];  // Content column
            //if (cell.Text.Length > 28)
            //{
            //    cell.ToolTip = cell.Text;
            //    cell.Text = cell.Text.Substring(0, 26) + "...";
            //}

            //cell = (TableCell)e.Row.Cells[3];    // Zone column
            //if (cell.Text.Length > 18)
            //{
            //    cell.ToolTip = cell.Text;
            //    cell.Text = cell.Text.Substring(0, 16) + "...";
            //}

            //cell = (TableCell)e.Row.Cells[7];    // StartTime column
            //if (dr["StartTime"] != DBNull.Value)
            //    cell.Text = DateTime.Parse(dr["StartTime"].ToString()).ToString("h:mm tt");

            //cell = (TableCell)e.Row.Cells[8];    // EndTime column
            //if (dr["EndTime"] != DBNull.Value)
            //    cell.Text = DateTime.Parse(dr["EndTime"].ToString()).ToString("h:mm tt");

            //for edit button
            ImageButton ibEdit = (ImageButton)e.Row.FindControl("ibEdit");
            //ibEdit.Enabled = !(bool)dr["isDefaultContent"];
            //ibEdit.Visible = !(bool)dr["isDefaultContent"];
            if ((bool)dr["isDefaultContent"])
            {
                ibEdit.ImageUrl = "/images/lemonaid/buttons/chronometer_32x32.png";
                ibEdit.CommandName = "addtc";
                ibEdit.ToolTip = "Add Timed Content";
            }

            //for edit content button
            HyperLink hlEditContent = (HyperLink)e.Row.FindControl("hlEditContent");
            hlEditContent.NavigateUrl = "/controls/content/edit.aspx?id=" + dr["HtmlId"].ToString() + "&PageID=" + ViewState["Page_ID"].ToString();

            //for delete button
            ImageButton ibDelete = (ImageButton)e.Row.FindControl("ibDelete");
            //ibDelete.Enabled = !(bool)dr["isDefaultContent"];
            ibDelete.Visible = !(bool)dr["isDefaultContent"];
            ibDelete.Attributes.Add("OnClick", "return confirm('Are you sure want to delete this item?');");


        }
    }

    public void PageSizeChangeTC(object o, EventArgs e)
    {
        gvTimedContent.PageSize = Convert.ToInt32(ddlPageSizeTC.SelectedValue);
        pager2.PageSize = Convert.ToInt32(ddlPageSizeTC.SelectedValue);
        pager2.CurrentIndex = 1;
        gvTimedContent.PageIndex = 0;

        BindTimedContent();
    }

    public void pager2_Command(object sender, CommandEventArgs e)
    {
        int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
        pager2.CurrentIndex = currnetPageIndx;
        gvTimedContent.PageIndex = currnetPageIndx - 1;

        BindTimedContent();
    }
    protected void gvTimedContent_Sorting(object sender, GridViewSortEventArgs e)
    {
        sortExpTC = e.SortExpression;

        if (sortOrderTC == "desc")
            sortOrderTC = "asc";
        else
            sortOrderTC = "desc";
        BindTimedContent(e.SortExpression, sortOrderTC);
        
        //if ((string)ViewState["sortDTC"] == "ASC")
        //{
        //    ViewState["sortDTC"] = "DESC";
        //    ViewState["sortExpressionTC"] = e.SortExpression + " DESC";
        //    BindTimedContent(e.SortExpression + " DESC");
        //}

        //else
        //{
        //    if ((string)ViewState["sortDTC"] == "DESC")
        //    {
        //        ViewState["sortDTC"] = "ASC";
        //        ViewState["sortExpressionTC"] = e.SortExpression + " ASC";
        //        BindTimedContent(e.SortExpression + " ASC");
        //    }
        //}
    }

    public void gvTimedContent_RowCommand(object sender, CommandEventArgs e)
    {
        switch (e.CommandName.ToLower())
        {
            case "deletetc":
                string sqlcmd = " delete Pages_Content_Zone where id=@id ";
                SqlCommand cmd = new SqlCommand(sqlcmd, conn);
                cmd.Parameters.AddWithValue("@id", e.CommandArgument.ToString());

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                BindTimedContent();
                break;
            case "edittc":

                Edit(e.CommandArgument.ToString());

                break;
            case "addtc":

                btnAdd_Command(sender, e);

                break;
            default:
                break;
        }
    }

   

    #endregion
    
    protected void bttn_Add_Question_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/admin.aspx?c=wizard");
    }
    
    protected void ibCancel_Click(object sender, EventArgs e)
    {
        pnlManageTimedContent.Visible = false;
        pnlList.Visible = true;

        ViewState["Page_ID"] = null;    // Page ID
        ViewState["Page_SEO"] = null;
        
        //mBindData((string)ViewState["sortExpression"]);
        mBindData();
    }

    protected void ibCancelTC_Click(object sender, EventArgs e)
    {
        pnlEdit.Visible = false;
        pnlTCList.Visible = true;

        BindTimedContent();
    }

    private void Edit(string id)
    {
        pnlEdit.Visible = true;
        pnlTCList.Visible = false;

        SqlDataAdapter dapt = new SqlDataAdapter("select * from Pages_Content_Zone where id=@id", conn);
        dapt.SelectCommand.Parameters.AddWithValue("@id", id);
        DataTable dt = new DataTable();
        dapt.Fill(dt);

        ddlZones.ClearSelection();
        try
        {
            ddlZones.SelectedValue = dt.Rows[0]["Zone_ID"].ToString();
        }
        catch { }

        txtPriority.Text = dt.Rows[0]["Priority"].ToString().Trim();

        if (dt.Rows[0]["StartDate"] != DBNull.Value)
            txtStartDate.Text = Convert.ToDateTime(dt.Rows[0]["StartDate"]).ToString("yyyy-MM-dd");
        else
            txtStartDate.Text = "";
        if (dt.Rows[0]["EndDate"] != DBNull.Value)
            txtEndDate.Text = Convert.ToDateTime(dt.Rows[0]["EndDate"]).ToString("yyyy-MM-dd");
        else
            txtEndDate.Text = "";

        ddlStartTime.ClearSelection();
        try
        {
            //ddlStartTime.SelectedValue = Convert.ToDateTime(dt.Rows[0]["StartTime"]).ToString("H:mm");
            ddlStartTime.Items.FindByText(DateTime.Parse(dt.Rows[0]["StartTime"].ToString()).ToString("h:mm tt").ToUpper()).Selected = true;
        }
        catch { }

        ddlEndTime.ClearSelection();
        try
        {
            //ddlEndTime.SelectedValue = Convert.ToDateTime(dt.Rows[0]["EndTime"]).ToString("H:mm");
            ddlEndTime.Items.FindByText(DateTime.Parse(dt.Rows[0]["EndTime"].ToString()).ToString("h:mm tt").ToUpper()).Selected = true;
        }
        catch { }

        ibSaveTC.CommandName = "save";
        ibSaveTC.CommandArgument = id;
    }

    protected void btnAdd_Command(object sender, CommandEventArgs e)
    {
        pnlEdit.Visible = true;
        pnlTCList.Visible = false;
        ibSaveTC.CommandName = "add";

        int id = 0;
        int.TryParse(e.CommandArgument.ToString(), out id);

        DataTable dt = new DataTable();
        if (id > 0)
        {
            using ( SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]))
            {
                SqlDataAdapter dapt = new SqlDataAdapter("select * from Pages_Content_Zone where id=@id", conn);
                dapt.SelectCommand.Parameters.AddWithValue("@id", id);
                dapt.Fill(dt);
            }
            ibSaveTC.CommandArgument = id.ToString();
        }
        
        ddlZones.ClearSelection();
        if (dt.Rows.Count > 0)
        {
            try
            {
                ddlZones.SelectedValue = dt.Rows[0]["Zone_ID"].ToString();
            }
            catch { }
            txtPriority.Text = dt.Rows[0]["Priority"].ToString();
        }
        else
        {
            txtPriority.Text = "1";
        }
        txtStartDate.Text = "";
        txtEndDate.Text = "";
        ddlStartTime.ClearSelection();
        ddlEndTime.ClearSelection();

    }

    public void SaveTimedContent(object o, CommandEventArgs e)
    {
        pnlEdit.Visible = false;
        pnlTCList.Visible = true;

        SqlCommand command;

        if (e.CommandName == "save")
        {
            string sqlcmd = " update Pages_Content_Zone set Zone_ID=@zone, Priority=@Priority, StartDate=@StartDate, EndDate=@EndDate, StartTime=@StartTime, EndTime=@EndTime where id=@id ";
            sqlcmd += " update Content set name=@name where id = (select Content_ID from Pages_Content_Zone where id=@id) ";

            command = new SqlCommand(sqlcmd, conn);
            command.Parameters.AddWithValue("@id", e.CommandArgument.ToString());
        }
        else
        {
            command = new SqlCommand("BASE_Wizard_AutoFill", conn);
            command.CommandType = CommandType.StoredProcedure;
            
            command.Parameters.AddWithValue("@page", ViewState["Page_ID"].ToString());
            command.Parameters.AddWithValue("@lang", 1);
            if (e.CommandArgument.ToString().Length > 0)
                command.Parameters.AddWithValue("@LinkId", e.CommandArgument.ToString());
        }

        string name = ViewState["Page_SEO"].ToString() + " (" + ddlZones.SelectedItem.Text + ")";
        if (!string.IsNullOrEmpty(txtStartDate.Text.Trim()) && !string.IsNullOrEmpty(txtEndDate.Text.Trim()))
            name += " [" + txtStartDate.Text.Trim() + " - " + txtEndDate.Text.Trim() + "]";
        name += " [" + ddlStartTime.SelectedValue + " - " + ddlEndTime.SelectedValue + "]";

        command.Parameters.AddWithValue("@name", name);

        command.Parameters.AddWithValue("@zone", ddlZones.SelectedValue);
        command.Parameters.AddWithValue("@priority", txtPriority.Text.Trim());

        if (!string.IsNullOrEmpty(txtStartDate.Text.Trim()))
            command.Parameters.AddWithValue("@StartDate", txtStartDate.Text.Trim());
        else
            command.Parameters.AddWithValue("@StartDate", DBNull.Value);
        
        if (!string.IsNullOrEmpty(txtEndDate.Text.Trim()))
            command.Parameters.AddWithValue("@EndDate", txtEndDate.Text.Trim());
        else
            command.Parameters.AddWithValue("@EndDate", DBNull.Value);
        
        command.Parameters.AddWithValue("@StartTime", ddlStartTime.SelectedValue);
        command.Parameters.AddWithValue("@EndTime", ddlEndTime.SelectedValue);

        conn.Open();
        command.ExecuteNonQuery();
        conn.Close();

        BindTimedContent();
    }

}
