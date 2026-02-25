using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Controls_Zone_Zone : System.Web.UI.Page
{
    #region page_events
    protected void Page_Load(object sender, EventArgs e)
    {
        UrlParameterWhitelistValidator validator = new UrlParameterWhitelistValidator();
        var filteredQueryString = validator.ValidateAndFilter(this);

        if (!IsLoggedIn)
            Login();
        else
        {
            if (!IsPostBack)
            {
                Pending = false;
                ddBind();
                gvBind();
            }
        }
    }
    protected string test;
    protected void AddToZone(object sender, ImageClickEventArgs e)
    {
        DropDownList ddl=((DropDownList)Page.FindControl(((ImageButton)sender).CommandArgument));
        AddZoneContent(ddl);
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        SaveUpdates();
    }

    protected void clickedApprove(object sender, EventArgs e)
    {
        ApprovePending();
        CloseMe();

    }
    protected void clickedLive(object sender, EventArgs e)
    {
        Pending = false;
        gvBind();

    }
    protected void clickedPending(object sender, EventArgs e)
    {
        Pending = true;
        gvBind();
    }

    protected void AddNewHTML(object sender, ImageClickEventArgs e)
    {
        if (tbHTML.Text.Trim().Length > 0)
        {
            DataTable dt = CreateNewHTML();
            DataRow[] rws = dt.Select("lang=%l".Replace("%l", Language));
            string cont = rws[0]["content"].ToString();

            ddBind(true);
            ddlContentHTML.SelectedIndex = ddlContentHTML.Items.IndexOf(ddlContentHTML.Items.FindByValue(cont));
        }
    }

    private void CloseMe()
    {
        //ClientScript.RegisterStartupScript(GetType(), "asd", "parent.parent.window.location.reload();", true);
        ClientScript.RegisterClientScriptBlock(GetType(), "closeScript", "window.opener.location.reload();window.close();", true);
    }

    private void Login()
    {
        ClientScript.RegisterStartupScript(GetType(), "asd", "parent.parent.window.location.href='/login';", true);
    }

    #endregion page_events

    #region dal

    private DataTable CreateNewHTML()
    {
        DataTable dt=new DataTable();
        SqlDataAdapter da = new SqlDataAdapter(sqlHTMLCreate, connect);
        da.SelectCommand.CommandType = CommandType.StoredProcedure;
        da.SelectCommand.Parameters.AddWithValue("@name", tbHTML.Text.Trim());
        da.Fill(dt);
        return dt;
    }

    private void AddZoneContent(DropDownList ddCurrent)
    {
        DataTable dt = CurrentTable;
        int _priority = (dt.Rows.Count > 0 ? Convert.ToInt32(dt.Compute("max(priority)", "")) : 0) + 1;
        DataRow dr = dt.NewRow();
        dr["Page_ID"] = PageID;
        dr["Zone_ID"] = Zone;
        dr["Content_ID"] = ddCurrent.SelectedValue;
        dr["Content_Name"] = ddCurrent.SelectedItem.Text;
        dr["Priority"] = _priority;

        dt.Rows.Add(dr);
        CurrentTable = dt;
        gvBind();
    }

    private void DeleteZoneContent(string id)
    {
        DataTable dt = CurrentTable;
        DataRow[] rws = dt.Select("priority=%pr".Replace("%pr", id));
        dt.Rows.Remove(rws[0]);
        CurrentTable = dt;
        gvBind();
    }

    private void SwapBannerPriority(int _dir, string id)
    {
        DataTable _dt = CurrentTable;
        string nextid =
        (_dir == 0) ?
            _dt.Compute("min(priority)", "priority>%pr".Replace("%pr", id)).ToString()
        :
            _dt.Compute("max(priority)", "priority<%pr".Replace("%pr", id)).ToString();

        if (!string.IsNullOrEmpty(nextid))
        {
            int _currentRecord = FindRecord(id);
            int _nextRecord = FindRecord(nextid);
            _dt.Rows[_currentRecord]["Priority"] = nextid;
            _dt.Rows[_nextRecord]["Priority"] = id;

            DataView dv = _dt.DefaultView;
            dv.Sort = "priority asc";

            CurrentTable = dv.ToTable();
            gvBind();
        }
    }

    private void SwapBannerPriority(string id)
    {
        SwapBannerPriority(0, id);
    }

    private void SaveUpdates()
    {
        DataTable dt = CurrentTable;
        SqlConnection con = new SqlConnection(connect);
        con.Open();
        //delete all page-zone record from pending table
        SqlCommand cmdDel = new SqlCommand(sqlPendingClear, con);
        cmdDel.CommandType = CommandType.StoredProcedure;
        cmdDel.Parameters.AddWithValue("@page", PageID);
        cmdDel.Parameters.AddWithValue("@zone", Zone);
        cmdDel.ExecuteNonQuery();

        //insert records into page-zone pending table
        SqlCommand cmdIns = new SqlCommand(sqlPendingInsert, con);
        cmdIns.CommandType = CommandType.StoredProcedure;
        cmdIns.Parameters.AddWithValue("@page", PageID);
        cmdIns.Parameters.AddWithValue("@zone", Zone);
        cmdIns.Parameters.AddWithValue("@content", 0);
        cmdIns.Parameters.AddWithValue("@priority", 0);

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            DataRow rw = dt.Rows[i];
            cmdIns.Parameters["@content"].Value = rw["Content_ID"].ToString();
            cmdIns.Parameters["@priority"].Value = i + 1;
            cmdIns.ExecuteNonQuery();
        }

        con.Close();
        Pending = true;
        CurrentTable = dt;
        gvBind();
    }

    private void ApprovePending()
    {
        DataTable dt = TableZonePending;
        SqlConnection con = new SqlConnection(connect);
        con.Open();

        //delete all page-zone record from Pages_Zone_Content table
        SqlCommand cmdDel = new SqlCommand(sqlMainClear, con);
        cmdDel.CommandType = CommandType.StoredProcedure;
        cmdDel.Parameters.AddWithValue("@page", PageID);
        cmdDel.Parameters.AddWithValue("@zone", Zone);
        cmdDel.ExecuteNonQuery();

        //insert records into page-zone Pages_Zone_Content table
        SqlCommand cmdIns = new SqlCommand(sqlMainInsert, con);
        cmdIns.CommandType = CommandType.StoredProcedure;
        cmdIns.Parameters.AddWithValue("@page", PageID);
        cmdIns.Parameters.AddWithValue("@zone", Zone);
        cmdIns.Parameters.AddWithValue("@content", 0);
        cmdIns.Parameters.AddWithValue("@priority", 0);

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            DataRow rw = dt.Rows[i];
            cmdIns.Parameters["@content"].Value = rw["Content_ID"].ToString();
            cmdIns.Parameters["@priority"].Value = i + 1;
            cmdIns.ExecuteNonQuery();
        }
        //delete all page-zone record from pending table
        SqlCommand cmdDelPending = new SqlCommand(sqlPendingClear, con);
        cmdDelPending.CommandType = CommandType.StoredProcedure;
        cmdDelPending.Parameters.AddWithValue("@page", PageID);
        cmdDelPending.Parameters.AddWithValue("@zone", Zone);
        cmdDelPending.ExecuteNonQuery();

        con.Close();
        ClearTable();
        gvBind();
    }

    private void ddBind()
    {
        ddBind(false);
    }


    private void ddBind(bool OnlyHTML)
    {
        SqlDataAdapter dapt = new SqlDataAdapter(sqlContent, connect);
		dapt.SelectCommand.CommandType = CommandType.StoredProcedure;
        dapt.SelectCommand.Parameters.AddWithValue("@MYLANG", Language);
    	dapt.SelectCommand.Parameters.AddWithValue("@MYUSERID", Session["LoggedInID"].ToString());
        DataSet ds = new DataSet();
        dapt.Fill(ds);

        DataView dv1 = ds.Tables[0].DefaultView;
        dv1.RowFilter = "control='content'";

        ddlContentHTML.DataSource =dv1;
        ddlContentHTML.DataTextField = "name";
        ddlContentHTML.DataValueField = "id";
        ddlContentHTML.DataBind();

        if (!OnlyHTML)
        {
            dv1.RowFilter = "control not in('content','menu', 'ContentRow')";

            ddlContentControl.DataSource = dv1;
            ddlContentControl.DataTextField = "name";
            ddlContentControl.DataValueField = "id";
            ddlContentControl.DataBind();
        }

		

    	foreach (ListItem li in ddlContentHTML.Items)
    	{
			if (li.Text.EndsWith("9"))
				li.Text = li.Text.TrimEnd('9');
    	}

		foreach (ListItem li in ddlContentControl.Items)
    	{
			if (li.Text.EndsWith("9"))
				li.Text = li.Text.TrimEnd('9');
    	}
    }

    private void gvBind()
    {
        if (TableZone == null)
            GetTables();

        btnPending.Visible = TableZonePending.Rows.Count > 0;
        btnApprove.Visible = IsPublisher && (TableZonePending.Rows.Count > 0);

        Pending = Pending && (TableZonePending.Rows.Count > 0);
        if (Pending)
        {
            btnLive.CssClass = "editor_button_top_off";
            btnPending.CssClass = "editor_button_top_on";
        }
        else
        {
            btnLive.CssClass = "editor_button_top_on";
            btnPending.CssClass = "editor_button_top_off";
        }

        DataTable dt = CurrentTable;

        if (dt.Rows.Count > 0)
        {
            DataView dv = dt.DefaultView;
            if (!(sortExpression == string.Empty))
            {
                dv.Sort = sortExpression;
            }
            this.gvContent.DataSource = dv;
            this.gvContent.DataBind();
        }
    }

    protected string connect = ConfigurationManager.AppSettings["CMServer"];

	protected string sqlContent = "ZoneContentList";
    protected string sqlZoneContent = "PageContentZone_Get";
    protected string sqlPendingClear = "Pages_Content_ZonePendingClear";
    protected string sqlPendingInsert = "Pages_Content_ZonePendingInsert";
    protected string sqlMainClear = "Pages_Content_ZoneClear";
    protected string sqlMainInsert = "Pages_Content_ZoneInsert";
    protected string sqlHTMLCreate = "HTML_AutoFill";

    #endregion dal

    #region GriedView

    protected void gvContent_DataBound(object sender, EventArgs e)
    {

    }

    protected void gvContent_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string id = e.CommandArgument.ToString();

        if (e.CommandName == "Up")
            SwapBannerPriority(1, id);
        else if (e.CommandName == "Down")
            SwapBannerPriority(id);
    }

    protected void gvContent_RowDataBound(object sender, GridViewRowEventArgs e)
    {


        if (e.Row.FindControl("ibDelete") != null)
        {
            ((ImageButton)e.Row.FindControl("ibDelete")).
                Attributes.Add("OnClick", "return confirm('Are you sure to delete this Record?');");
        }
    }
    protected void gvContent_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string id = gvContent.DataKeys[e.RowIndex].Value.ToString();
        DeleteZoneContent(id);
    }

    protected void gvContent_Sorting(object sender, GridViewSortEventArgs e)
    {
        string _direction = "ASC";

        if (sortExpression.Contains(e.SortExpression))
        {
            string[] _sorting = sortExpression.Split(' ');
            if (_sorting.Length == 2 && _sorting[1] == _direction)
                _direction = "DESC";
        }
        sortExpression = e.SortExpression + " " + _direction;
        gvBind();
    }

    private int FindRecord(string priority)
    {
        int _record = -1;
        DataTable dt = CurrentTable;

        for (int i = 0; i < dt.Rows.Count; i++)
            if (dt.Rows[i]["Priority"].ToString() == priority)
                _record = i;

        return _record;
    }

    private void GetTables()
    {
        SqlDataAdapter dapt = new SqlDataAdapter(sqlZoneContent, connect);
        dapt.SelectCommand.Parameters.AddWithValue("@zone", Zone);
        dapt.SelectCommand.Parameters.AddWithValue("@page", PageID);
        dapt.SelectCommand.CommandType = CommandType.StoredProcedure;

        DataSet ds = new DataSet();
        dapt.Fill(ds);

        TableZone = ds.Tables[0];
        TableZonePending = ds.Tables[1];
    }


    #endregion GriedView

    #region properties
    protected string sortExpression
    {
        set
        {
            ViewState[Pending ? "sortExpressionPending" : "sortExpression"] = value;
        }
        get
        {
            return Convert.ToString(ViewState[Pending ? "sortExpressionPending" : "sortExpression"]);
        }
    }
    protected string Language
    {
        get { return Convert.ToString(Session["Language"]); }
    }
    protected string PageID
    {
//        get { return Convert.ToString(Session["PageID"]); }
        get { return Request.QueryString["PageID"]; }
    }
    protected string Zone
    {
        get { return Request.QueryString["zone"].ToString(); }
    }
    private bool IsPublisher
    {
        get
        {
            int page = Convert.ToInt32(Session["PageID"] == null ? -1 : Session["PageID"]);
            int user = Convert.ToInt32(Session["LoggedInID"] == null ? -1 : Session["LoggedInID"]);

            return (Permissions.Get(user, page) > 1);
        }
    }

    private bool IsLoggedIn
    {
        get { return Session["LoggedInID"]!=null; }
    }

    protected bool Pending
    {
        set { ViewState["pending"] = value; }
        get { return Convert.ToBoolean(ViewState["pending"]); }
    }
    #endregion properties

    #region tables
    private string mainTable = "maintable";
    private string pendingTable = "pendingtable";

    protected DataTable TableZone
    {
        set { ViewState[mainTable] = value; }
        get { return (DataTable)ViewState[mainTable]; }
    }
    protected DataTable TableZonePending
    {
        set { ViewState[pendingTable] = value; }
        get { return (DataTable)ViewState[pendingTable]; }
    }
    protected DataTable CurrentTable
    {
        set
        {
            ViewState[Pending ? pendingTable : mainTable] = value;
        }
        get
        {
            return (DataTable)ViewState[Pending ? pendingTable : mainTable];
        }
    }

    protected void ClearTable()
    {
        ViewState.Remove(pendingTable);
        ViewState.Remove(mainTable);
    }
    #endregion tables
}
