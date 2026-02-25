//#define MULTI_LANGUAGE
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Controls_Resources : System.Web.UI.UserControl
{
    private int _mLanguage;
    FullTextSearch.FullTextSearch myFTS;
    string[] LCIDs = { "1033", "1036", "1034" };  //1033 - English USA; 3084 - French France; 1034: Spanish Spain
    private string sqlSearch = "Resources_Search";

    protected void Page_Load(object sender, EventArgs e)
    {
        //litError.Text = "";
      
        this.Page.MaintainScrollPositionOnPostBack = true;

        if (!Page.IsPostBack)
        {
            ViewState["sortOrder"] = "desc";

            PopulateDropDowns();
            //BindGrid();
            Bind();
            SetView();

            lnkDeleteDocu.Attributes.Add("onclick", "return confirm('You are about to delete a file. Are you sure?');");
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        lnkDeleteDocu.Visible = false;
    }
    private void PopulateDropDowns()
    {
        //DataTable dt = MyDAL_Resources.getSTable_Text("select * from status");
        DataSet ds = new DataSet();

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("dbResources")))
        {
            string sqlcmd = " select * from status ";
            sqlcmd += " select * from Languages ";
            sqlcmd += " select * from eko.dbo.Groups where id in (select Group_id from eko.dbo.Users_Groups_Access where User_id=@userid and access_level>1) or @userid=1 order by name ";
            sqlcmd += " select id, title from Resources where status = 1 " +
                "and (@userid = 1 or" +
                "       id in (select ResourceId from Resource_Types_Link where GroupId in " +
                "           (select id from ResourcesGroups where groupid in (select Group_id from eko.dbo.Users_Groups_Access where User_id=@userid and access_level>1)))   " +
                "   )" +
                "order by LTRIM(title)";
            sqlcmd += " select IconGroup from ResourceDocuTypes group by IconGroup order by IconGroup";
            
            SqlDataAdapter dapt = new SqlDataAdapter(sqlcmd, conn);
            dapt.SelectCommand.Parameters.AddWithValue("@userid", Session["LoggedInID"].ToString());
            dapt.Fill(ds);
        }

        ddlFilter.DataSource = ds.Tables[0];
        ddlFilter.DataBind();
        ddlFilter.Items.Insert(0, new ListItem("Any Status", ""));
        ddlFilter.Items.Add(new ListItem("Live Resources", "Show = 1 and Status = 1"));
        ddlFilter.Items.Add(new ListItem("Hidden Resources", "Show = 0"));

#if MULTI_LANGUAGE
        ddlGalleryLanguage.DataSource = ds.Tables[1];
        ddlGalleryLanguage.DataValueField = "id";
        ddlGalleryLanguage.DataTextField = "name";
        ddlGalleryLanguage.DataBind();
        //ddlGalleryLanguage.Items.Insert(0, new ListItem("Select One", ""));

        try
        {
            ddlGalleryLanguage.SelectedValue = "1";
        }
        catch { }
#endif
        ddlGroup.DataSource = ds.Tables[2];
        ddlGroup.DataValueField = "id";
        ddlGroup.DataTextField = "name";
        ddlGroup.DataBind();
        ddlGroup.Items.Insert(0, "");
        //try
        //{
        //    //ddlGroup.Items.FindByText("Common").Selected = true;
        //    ddlGroup.SelectedValue = "1";
        //}
        //catch { }

       // PopulateResourcesGroupCategories("1");

        ddlOtherResources.DataSource = ds.Tables[3];
        ddlOtherResources.DataBind();

        DataView DV = ds.Tables[4].DefaultView;
        DV.RowFilter = "IconGroup <> 'Unknown'";
        ddlIcon.DataSource = DV;
        ddlIcon.DataBind();
        ddlIcon.Items.Insert(0, "");
    }

    private void PopulateResourcesGroupCategories(string language)
    {
        DataTable dt = new DataTable();

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("dbResources")))
        {
            string sqlcmd = " select * from ResourceTypes where Language=@Language and parentid=0 and categ_type=@type order by internal_name ";

            SqlDataAdapter dapt = new SqlDataAdapter(sqlcmd, conn);
            dapt.SelectCommand.Parameters.AddWithValue("@Language", language);
            dapt.SelectCommand.Parameters.AddWithValue("@type", rblLibraryTypes.SelectedValue);
            dapt.Fill(dt);
        }
        pnl_categ.Visible = dt.Rows.Count > 0;

        cblCategories.DataSource = dt;
        cblCategories.DataValueField = "id";
        cblCategories.DataTextField = "internal_name";
        cblCategories.DataBind();

    }
    string SelectedCategories
    {
        get
        {
            string categories = string.Empty;
            foreach (ListItem li in cblCategories.Items)
            {
                if (li.Selected)
                    categories += li.Value + ",";
            }
            categories = categories.TrimEnd(',');
            return categories;
        }
    }

    public string ResourcesGroupId
    {
        set { ViewState["ResourcesGroupId"] = value; }
        get { return ViewState["ResourcesGroupId"] != null ? ViewState["ResourcesGroupId"].ToString() : "1"; }
    }

    //////string ResourcesGroupName
    //////{
    //////    set { ViewState["ResourcesGroupName"] = value; }
    //////    get { return ViewState["ResourcesGroupName"] != null ? ViewState["ResourcesGroupName"].ToString() : ""; }
    //////}
    public string ResourceID
    {
        set { ViewState["resourceid"] = value; }
        get
        {
            string ret = "";
            if(ViewState["resourceid"] != null )
                ret= ViewState["resourceid"].ToString();
            return ret;
        }
    }

#region View
    protected string CurrentView
    {
        set { ViewState["currentview"] = value; }
        get { return ViewState["currentview"] == null ? pnlResourcesGroups.ID : ViewState["currentview"].ToString(); }
    }

    private void SetView(string view)
    {
        btnDone.Visible = false; ;
        btnDone2.Visible = false; ;

        CurrentView = view;
        SetView();
    }

    private void SetView()
    {
        IsViewable(pnlResourcesGroups);
        IsViewable(pnlAddResourcesGroups);
        IsViewable(pnlMain);
        IsViewable(pnlDetails);

    }

    private bool IsViewable(Panel pnl)
    {
        return pnl.Visible = pnl.ID == CurrentView;
    }
#endregion View

    private void Bind()
    {
        if (ViewState["sortExp"] != null)
        {
            Bind(ViewState["sortExp"].ToString(), ViewState["sortOrder"].ToString());
        }
        else
        {
            Bind("", "");
        }
    }

    private void Bind(string sortExp, string sortDir)
    {
        DataTable dt = new DataTable();

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("dbResources")))
        {
            SqlDataAdapter dapt = new SqlDataAdapter("ResourcesGroups_Select_New", conn);
            dapt.SelectCommand.CommandType = CommandType.StoredProcedure;

            string stemp = "";
            foreach (ListItem li in ddlGroup.Items)
            {
                if (stemp != "")
                    stemp += ",";

                stemp += li.Value;
            }

            dapt.SelectCommand.Parameters.AddWithValue("@groups", stemp);

            dapt.Fill(dt);
        }

        if (dt.Rows.Count > 0)
        {
            DataView myDataView = new DataView();
            myDataView = dt.DefaultView;

            if (sortExp != string.Empty)
            {
                myDataView.Sort = string.Format("{0} {1}", sortExp, sortDir);
            }

            gvMainRG.DataSource = myDataView;
            gvMainRG.DataBind();

            //  pager1.ItemCount = myDataView.Count;
            PagerV2_1.ItemCount = myDataView.Count;
            if (gvMainRG.PageSize >= myDataView.Count)
            {
                PagerV2_1.Visible = false;
            }
            else
            {
                PagerV2_1.Visible = true;
            }

            gridarea.Visible = true;
            noMain.Visible = false;

        }
        else
        {
            gridarea.Visible = false;
            noMain.Visible = true;
        }
    }

    public void rowcommand(object o, CommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "deleteresourcesgroup":
                DeleteRow(e.CommandArgument.ToString());
                break;
            case "editresourcesgroup":
                Edit(e.CommandArgument.ToString());
                break;
            case "editresources":
                SetView(pnlMain.ID);
                ViewState["ResourcesGroupId"] = e.CommandArgument.ToString();
                PopulateCategoriesFilter();
                BindGrid();
                break;
            default:
                break;
        }
    }

    private void PopulateCategoriesFilter()
    {
        string sqlcmd = @"  select t.* from ResourceTypes t
                            inner join ResourcesGroups_Types_Link l on t.id = l.ResourceTypeId
                            where t.language = @language and l.ResourcesGroupId = @ResourcesGroupId
                            order by t.name;
                            select * from ResourcesGroups where id = @ResourcesGroupId";

        

        List<SqlParameter> parms = new List<SqlParameter>();
#if MULTI_LANGUAGE
        parms.Add(new SqlParameter("@language", ddlLanguage2.SelectedValue));
#else
        parms.Add(new SqlParameter("@language", 1));
#endif
        parms.Add(new SqlParameter("@ResourcesGroupId", ResourcesGroupId));

        DataSet ds = MyDAL_Resources.getTables(sqlcmd, parms.ToArray(), CommandType.Text);
        DataTable dt = ds.Tables[0];
        ddlCategoryFilter.DataSource = dt;
        ddlCategoryFilter.DataBind();
        ddlCategoryFilter.Items.Insert(0, new ListItem("Any Category", ""));

        if(ds.Tables[1].Rows.Count > 0)
        {
            //////ResourcesGroupName = ds.Tables[1].Rows[0]["name"].ToString();
            litLibraryName.Text = "Library: " + ds.Tables[1].Rows[0]["name"].ToString();
        }
    }
    public void DeleteRow(string id)
    {
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("dbResources")))
        {
            SqlCommand command = new SqlCommand("ResourcesGroups_Delete", conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@id", id);

            conn.Open();
            command.ExecuteNonQuery();
            conn.Close();
        }

        Bind();
    }

    private void Edit(string id)
    {
        //pnlEdit.Visible = true;
        //pnlList.Visible = false;

        SetView(pnlAddResourcesGroups.ID);

        DataSet ds = new DataSet();

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("dbResources")))
        {
            SqlDataAdapter dapt = new SqlDataAdapter("ResourcesGroups_Select", conn);
            dapt.SelectCommand.CommandType = CommandType.StoredProcedure;
            dapt.SelectCommand.Parameters.AddWithValue("@id", id);
            dapt.Fill(ds);
        }

        tbGalleryName.Text = ds.Tables[0].Rows[0]["name"].ToString();
        tbGalleryDesc.Text = ds.Tables[0].Rows[0]["description"].ToString();

        rblLibraryTypes.SelectedValue = ds.Tables[0].Rows[0]["lib_type"].ToString();
        rblLibraryTypes.Enabled = false; 

        PopulateResourcesGroupCategories("1");


#if MULTI_LANGUAGE
        ddlGalleryLanguage.ClearSelection();
        try
        {
            ddlGalleryLanguage.SelectedValue = ds.Tables[0].Rows[0]["language"].ToString();
        }
        catch { }
#endif

        ddlGroup.ClearSelection();
        try
        {
            ddlGroup.SelectedValue = ds.Tables[0].Rows[0]["groupid"].ToString();
        }
        catch { }

        cblCategories.ClearSelection();
        foreach (DataRow dr in ds.Tables[1].Rows)
        {
            try
            {
                ListItem li = cblCategories.Items.FindByValue(dr["ResourceTypeId"].ToString());
                if(li != null)
                    li.Selected = true;
            }
            catch { }
        }

        ibSave.CommandName = "save";
        ibSave.CommandArgument = id;

        ibSave2.CommandName = "save";
        ibSave2.CommandArgument = id;
    }

    public void Add(object o, EventArgs e)
    {
        pnl_categ.Visible = false;

        //pnlEdit.Visible = true;
        //pnlList.Visible = false;
        SetView(pnlAddResourcesGroups.ID);
        ibSave.CommandName = "add";
        ibSave2.CommandName = "add";

        tbGalleryName.Text = "";
        tbGalleryDesc.Text = "";

        rblLibraryTypes.ClearSelection();
        rblLibraryTypes.Enabled = true;


#if MULTI_LANGUAGE
        ddlGalleryLanguage.ClearSelection();
        try
        {
            ddlGalleryLanguage.SelectedValue = "1";
        }
        catch { }
#endif

        ddlGroup.ClearSelection();
        //try
        //{
        //    ddlGroup.SelectedValue = "1";
        //}
        //catch { }

        cblCategories.ClearSelection();

    }

    public void SAVE(object o, CommandEventArgs e)
    {
        //pnlEdit.Visible = false;
        //pnlList.Visible = true;
        SetView(pnlResourcesGroups.ID);

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("dbResources")))
        {
            SqlCommand command = new SqlCommand();

            if (e.CommandName == "save")
            {
                command = new SqlCommand("ResourcesGroups_Update", conn);
                command.Parameters.AddWithValue("@id", e.CommandArgument.ToString());
            }
            else
            {
                command = new SqlCommand("ResourcesGroups_Insert", conn);
            }

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@name", tbGalleryName.Text.Trim());
            command.Parameters.AddWithValue("@description", tbGalleryDesc.Text.Trim());
#if MULTI_LANGUAGE
            command.Parameters.AddWithValue("@language", ddlGalleryLanguage.SelectedValue);
#else
            command.Parameters.AddWithValue("@language", 1);

#endif
            command.Parameters.AddWithValue("@groupid", ddlGroup.SelectedValue);
            command.Parameters.AddWithValue("@categories", SelectedCategories);
            command.Parameters.AddWithValue("@type", rblLibraryTypes.SelectedValue);

            conn.Open();
            command.ExecuteNonQuery();
            conn.Close();
        }

        Bind();
    }

    protected void gvMainRG_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton btnDeleteResourceGroup = (ImageButton)e.Row.FindControl("btnDeleteResourceGroup");
            btnDeleteResourceGroup.Attributes.Add("onclick", "return confirm('Are you sure want to delete this item?')");

        }
    }
    protected void ibCancel_Click(object sender, EventArgs e)
    {
        //pnlEdit.Visible = false;
        //pnlList.Visible = true;
        SetView(pnlResourcesGroups.ID);

        Bind();
    }

    public string sortOrder
    {
        get
        {
            if (ViewState["sortOrder"].ToString() == "desc")
            {
                ViewState["sortOrder"] = "asc";
            }
            else
            {
                ViewState["sortOrder"] = "desc";
            }

            return ViewState["sortOrder"].ToString();
        }
        set
        {
            ViewState["sortOrder"] = value;
        }
    }

    public void dosort(object o, GridViewSortEventArgs e)
    {
        ViewState["sortExp"] = e.SortExpression;
        Bind(e.SortExpression, sortOrder);
    }

    public void PageSizeChange(object o, EventArgs e)
    {
        gvMainRG.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
        PagerV2_1.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
        PagerV2_1.CurrentIndex = 1;
        gvMainRG.PageIndex = 0;

        Bind();
    }

    public void pager_CommandRG(object sender, CommandEventArgs e)
    {
        int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
        PagerV2_1.CurrentIndex = currnetPageIndx;
        gvMainRG.PageIndex = currnetPageIndx - 1;

        Bind();
    }

    protected string sortExpression
    {
        set { ViewState["sortExpression"] = value; }
        get { return Convert.ToString(ViewState["sortExpression"]); }
    }

    private void BindGrid()
    {
        DataTable dt = getResults(sqlSearch);

        if (ddlFilter.SelectedItem != null)
        {
            if (ddlFilter.SelectedItem.Text == "Live Resources" || ddlFilter.SelectedItem.Text == "Hidden Resources")
                dt.DefaultView.RowFilter = ddlFilter.SelectedValue;
        }

        if (!(sortExpression == string.Empty))
        {
            dt.DefaultView.Sort = sortExpression;
        }

        gvMain.DataSource = dt;
        gvMain.DataBind();
        pager1.PageSize = gvMain.PageSize;
        pager1.ItemCount = dt.DefaultView.Count;

        tbl_noresults.Visible = !(gvMain.Rows.Count > 0);
        lbtnDownloadAll.Visible = !tbl_noresults.Visible; // gvMain.Visible = !tbl_noresults.Visible;
    }
    protected void filter(object sender, EventArgs e)
    {
        BindGrid();
    }

    protected void btnClearFilter_Click(object sender, EventArgs e)
    {
        txtFilter.Text = "";
        ddlFilter.ClearSelection();
        ddlCategoryFilter.ClearSelection();
        BindGrid();

    }

#if MULTI_LANGUAGE
    protected void ddlGalleryLanguage_SelectedIndexChanged(object sender, EventArgs e)
    {
        PopulateResourcesGroupCategories(ddlGalleryLanguage.SelectedValue);
    }
#endif
    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtStartDate.Text = "";
        txtEndDate.Text = "";
    }
    private DataTable getResults(string sql)
    {
        _mLanguage = int.Parse(ddlLanguage.SelectedValue);

        string keywords = txtFilter.Text;
        string searchTerm = keywords.Trim();
        List<SqlParameter> parms = new List<SqlParameter>();

        searchTerm = FTSAux.RemoveNoiseWords(searchTerm, LCIDs[_mLanguage - 1]);

        if (searchTerm.Length > 0)
        {

            myFTS = new FullTextSearch.FullTextSearch(searchTerm);
            parms.Add(new SqlParameter("@keywords", myFTS.NormalForm));
        }
        parms.Add(new SqlParameter("@LCID", LCIDs[_mLanguage - 1]));
        parms.Add(new SqlParameter("@LanguageId", _mLanguage));

        try
        {
            int.Parse(ddlFilter.SelectedValue);
            parms.Add(new SqlParameter("@Status", ddlFilter.SelectedValue));
        }
        catch { }

        try
        {
            int.Parse(ddlCategoryFilter.SelectedValue);
            parms.Add(new SqlParameter("@CategID", ddlCategoryFilter.SelectedValue));
        }
        catch { }

        parms.Add(new SqlParameter("@Top", 0));
        parms.Add(new SqlParameter("@ResourcesGroupId", ResourcesGroupId));
        DataTable dt = MyDAL_Resources.getSTable(sql, parms);
        return dt;
    }

    protected void ddlCategoryFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindGrid();
    }

#region Grid Events
    protected void gvMain_Sorting(object sender, GridViewSortEventArgs e)
    {
        string _direction = "ASC";

        if (sortExpression.Contains(e.SortExpression))
        {
            string[] _sorting = sortExpression.Split(' ');
            if (_sorting.Length == 2 && _sorting[1] == _direction)
                _direction = "DESC";
        }
        sortExpression = e.SortExpression + " " + _direction;
        BindGrid();
    }
    protected void gvMain_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ((GridView)sender).PageIndex = e.NewPageIndex;
        BindGrid();
    }
    protected void gvMain_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string command = e.CommandName.ToLower();
        switch (command)
        {
            case "edit":
                ResourceID = e.CommandArgument.ToString();
                if (PopulateFields())
                    SetView(pnlDetails.ID);
                break;
            case "remove":
                //delete resource
                string sql = "Resource_Delete";
                ResourceID = e.CommandArgument.ToString();

                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("@id", ResourceID));
                MyDAL_Resources.ExecuteNonQuery(sql, parms);

                //refresh
                BindGrid();
                break;

            case "down":
                ResourceID = e.CommandArgument.ToString();
                SwapPriority(1);
                BindGrid();
                break;

            case "up":
                ResourceID = e.CommandArgument.ToString();
                SwapPriority(0);
                BindGrid();
                break;
        }
    }

    private void SwapPriority(int _dir)
    {
        if (ddlCategoryFilter.SelectedValue != "")
        {
            SqlCommand cmd = new SqlCommand("Resources_SwapPriority_Extended", new SqlConnection(ConfigurationManager.AppSettings.Get("dbResources")));
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", ResourceID);
            cmd.Parameters.AddWithValue("@dir", _dir);
            cmd.Parameters.AddWithValue("@CategID", ddlCategoryFilter.SelectedValue);
            cmd.Connection.Open();
            int ret = cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }
    }
    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataRowView drw = (DataRowView)e.Row.DataItem;
        GridViewRow gvr = e.Row;

        string ibStatus = "ibStatus";
        string imgStatus = "imStatus";

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ((ImageButton)gvr.FindControl("btDelete")).Attributes.Add("onclick", "return confirm('You are about to delete a resource. Are you sure?');");

            int status = Convert.ToInt32(drw["status"]);

            Panel ib = (Panel)gvr.FindControl(ibStatus);
            Image im = (Image)gvr.FindControl(imgStatus);

            RenderStatus(status, im);
            im.Visible = status != 0;
            ib.Visible = !im.Visible;

            ((Literal)gvr.FindControl("ltShow")).Text = drw["Show"].ToString().ToLower() == "true" ? "Yes" : "No";

            Literal ltTitle = (Literal)gvr.FindControl("ltTitle");
            /*((Literal)gvr.FindControl("ltTitle")).Text = drw["Title"].ToString();*/

            if (drw["Title"].ToString().Length > 50)
            {
                ltTitle.Text = drw["Title"].ToString().Substring(0, 48) + "...";
                e.Row.Cells[4].ToolTip = drw["Title"].ToString();
            }
            else
            {
                ltTitle.Text = drw["Title"].ToString();
            }

            Literal ltType = (Literal)gvr.FindControl("ltType");
            //if (drw["ResourceTypeName"].ToString().Length > 21)
            //{
            //    ltType.Text = drw["ResourceTypeName"].ToString().Substring(0, 18) + "...";
            //    e.Row.Cells[5].ToolTip = drw["ResourceTypeName"].ToString();
            //}
            //else
            //{
            //    ltType.Text = drw["Categories"].ToString();
            //}

            if(ddlCategoryFilter.SelectedValue == "")
            {
                e.Row.FindControl("imgUP").Visible = false;
                e.Row.FindControl("imgDown").Visible = false;
            }

        }
    }
    protected void gvMain_RowEditing(object sender, GridViewEditEventArgs e)
    {

    }

    public void PageSizeChange2(object o, EventArgs e)
    {
        gvMain.PageSize = Convert.ToInt32(ddlPageSize2.SelectedValue);
        pager1.PageSize = Convert.ToInt32(ddlPageSize2.SelectedValue);
        pager1.CurrentIndex = 1;
        gvMain.PageIndex = 0;

        BindGrid();
    }
    public void pager_Command(object sender, CommandEventArgs e)
    {
        int currentPageIndx = Convert.ToInt32(e.CommandArgument);
        pager1.CurrentIndex = currentPageIndx;
        gvMain.PageIndex = currentPageIndx - 1;
        BindGrid();

    }
#endregion

#region Export
    protected void lbtnDownloadAll_Click(object sender, EventArgs e)
    {
        string _divider = ",";
        string _extension = "csv";
        string _suff = DateTime.Today.ToString("_yyyymmdd");

        List<SqlParameter> parms = new List<SqlParameter>();
        parms.Add(new SqlParameter("@ResourcesGroupId", ResourcesGroupId));

        if (txtStartDate.Text != "")
            parms.Add(new SqlParameter("@date1", txtStartDate.Text));
        if (txtEndDate.Text != "")
            parms.Add(new SqlParameter("@date2", txtEndDate.Text));

        DataTable dt = MyDAL_Resources.getSTable("Resources_Export", parms);

        string attachment = string.Format("attachment; filename=Resorces{0}.{1}", _suff, _extension);
        Response.ClearContent();
        Response.AddHeader("content-disposition", attachment);
        Response.ContentType = "application/excel";
        Response.Charset = "utf-8";
        string tab = "";

        foreach (DataColumn dc in dt.Columns)
        {
            Response.Write(tab + dc.ColumnName);

            tab = _divider;
        }

        Response.Write("\n");


        int i;
        foreach (DataRow dr in dt.Rows)
        {
            tab = "";
            for (i = 0; i < dt.Columns.Count; i++)
            {
                Response.Write(tab + dr[i]);
                tab = _divider;
            }
            Response.Write("\n");
        }
        Response.End();
    }
#endregion

#region Change Status
    string imPending = "pending.png";
    string imApproved = "approved.png";
    string imSuspended = "suspended.png";

    private void RenderStatus(int status, Image ib)
    {

        string _fname = System.IO.Path.GetFileName(ib.ImageUrl);
        switch (status)
        {
            case 0:
                ib.ImageUrl = ib.ImageUrl.Replace(_fname, imPending);
                ib.ToolTip = "Pending";
                break;
            case 1:
                ib.ImageUrl = ib.ImageUrl.Replace(_fname, imApproved);
                ib.ToolTip = "Approved";
                break;
            case 2:
                ib.ImageUrl = ib.ImageUrl.Replace(_fname, imSuspended);
                ib.ToolTip = "Suspended";
                break;
        }
    }

    protected void btStatus_Click(object sender, EventArgs e)
    {
        string sql = "Resource_ChangeStatus";
        string id = ((ImageButton)sender).CommandArgument.ToString();

        List<SqlParameter> parms = new List<SqlParameter>();
        parms.Add(new SqlParameter("@id", id));

        ImageButton btStatus = (ImageButton)sender;

        switch (btStatus.CommandName)
        {
            case "decline":
                parms.Add(new SqlParameter("@NewStatus", 2));
                break;
            case "approve":
                parms.Add(new SqlParameter("@NewStatus", 1));
                break;
            case "status":
                parms.Add(new SqlParameter("@NewStatus", 2));
                break;
        }

        DataTable dt = MyDAL_Resources.getTable(sql, parms);
        BindGrid();
        //if (dt.Rows.Count > 0)
        //{
        //    SendNotification(dt.Rows[0]["Status"].ToString());
        //}

    }
#endregion

#region Email
    private string mail_subject;
    private string mail_body;

    private void SendNotification(string message)
    {
        if (message == "1")
            message = "Approved";
        else
            message = "ResourceDeclined";

        if (getMailContent(message))
        {
            //mail_body = mail_body.Replace("<#MEMBER#>", tbFullName.Text) + "<br /><br />";

            //string body = MyTemplates.Registration;
            //body = body.Replace("<#Fullname#>", tbFullName.Text);
            //body = body.Replace("<#Email#>", tbEmail.Text);
            //body = body.Replace("<#Username#>", tbUsername.Text);
            //body = body.Replace("<#Password#>", tbPassword.Text);
            //body = body.Replace("<#PrimaryLocation#>", ddlLocation.SelectedItem.Text);
            //body = body.Replace("<#JoinNewsletter#>", cbNewsletter.Checked ? "Yes" : "No");
            //body = body.Replace("<#IsHealthCareProvider#>", cbHealthCareProvider.Checked ? "Yes" : "No");
            //if (cbHealthCareProvider.Checked)
            //{
            //    body = body.Replace("<#AreaSpeciality#>", tbArea.Text);
            //    body = body.Replace("<#TypePractice#>", ddlType.SelectedItem.Text);
            //}
            //else
            //{
            //    body = body.Remove(body.IndexOf("<tr><td>Area of Speciality:</td>"));
            //    body += "<table>";
            //}

            //mail_body = mail_body + body;
            //MyEmail.SendNotification(mail_subject, mail_body, tbEmail.Text, ConfigurationManager.AppSettings["MAIL_TO"], "", "", true);

        }
    }


    private bool getMailContent(string field)
    {
        DataTable msg = MyDAL_Resources.getTable("MessagesSelect", new SqlParameter("@name", field));
        if (msg.Rows.Count > 0)
        {
            mail_subject = msg.Rows[0]["Subject"].ToString();
            mail_body = msg.Rows[0]["Message"].ToString();
            return true;
        }
        return false;
    }
#endregion

#region Edit
    //protected string sqlSelect = "Resources_Select";
    protected string sqlDelete = "Resources_Delete";
    protected string sqlInsert = "Resource_Insert";
    protected string sqlUpdate = "Resource_Update";
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        ClearFields();
        //trLanguage.Visible = true;
#if MULTI_LANGUAGE
        ddlLanguage2.Enabled = true;
#endif
        ResourceID = "";
        SetView(pnlDetails.ID);
        rblType.SelectedValue = "1";
        tbTitle.Focus();

        LibrariesPartial1.Populate(int.Parse(ResourcesGroupId));

    }

    private void ClearFields()
    {
      //  btAddLibrary.Visible = false;
        cbShow.Checked = false;
        rblType.Enabled = true;
        lnkDeleteDocu.Visible = false;
        imStatus.Visible = false;
        imStatus2.Visible = false;
        tbUrl.Text = "";
        tbTitle.Text = "";
        tbDesc.Text = "";
        litDocu.Text = "";
        tbKeywords.Text = "";

        hfCheckedBoxes.Value = "";
        litErr.Text = "";

#if MULTI_LANGUAGE
        ddlLanguage2.ClearSelection();
#endif

        ddlOtherResources.ClearSelection();
        ddlIcon.ClearSelection();

    }

    protected void btnBackToList_Click(object sender, EventArgs e)
    {
        SetView(pnlMain.ID);
        BindGrid();
    }
    protected void btEdit_Click(object sender, EventArgs e)
    {
        if (UpdateDetails())
        {
            PopulateFields();
            btnDone.Visible = btnDone2.Visible = true;
        }
        else
        {
            if (!string.IsNullOrEmpty(ResourceID))
                LibrariesPartial1.Populate(ResourceID);
            else
                LibrariesPartial1.Populate(int.Parse(ResourcesGroupId));

        }
    }

    protected void LinkButtonDone_Click(object sender, EventArgs e)
    {
        SetView(pnlMain.ID);
        BindGrid();
    }
    private bool UpdateDetails()
    {
     //   if (hfCheckedBoxes.Value != "")
        {
            string[] CheckedBoxes = hfCheckedBoxes.Value.Split(new char[] { ',' });

         //   if (CheckedBoxes.Length > 0)
            {
                string sql = string.IsNullOrEmpty(ResourceID) ? sqlInsert : sqlUpdate;
                List<SqlParameter> prms = new List<SqlParameter>();

                if (!string.IsNullOrEmpty(ResourceID))
                {   //Update
                    prms.Add(new SqlParameter("@id", ResourceID));
                }
                else
                {   //Insert
                    prms.Add(new SqlParameter("@UserId", Session["LoggedInID"].ToString()));
                    prms.Add(new SqlParameter("@IsDocument", rblType.SelectedValue));
                }

                prms.Add(new SqlParameter("@Title", tbTitle.Text));


                prms.Add(new SqlParameter("@Description", tbDesc.Text));
                prms.Add(new SqlParameter("@Keywords", tbKeywords.Text));
                prms.Add(new SqlParameter("@Show", cbShow.Checked ? 0 : 1));
                prms.Add(new SqlParameter("@icon", ddlIcon.SelectedValue));
                
#if MULTI_LANGUAGE
        prms.Add(new SqlParameter("@LanguageId", ddlLanguage2.SelectedValue));
#else
                prms.Add(new SqlParameter("@LanguageId", 1));
#endif

                string items = "";
                foreach (ListItem li in ddlOtherResources.Items)
                {
                    if (li.Selected)
                        items += li.Value + ",";
                }
                if (items.Length > 0)
                {
                    int npos = items.LastIndexOf(",");
                    if (npos > 0)
                        items.Remove(npos);
                }

                prms.Add(new SqlParameter("@ResourceIDs", items));

                bool isVideo = false;
                string filePath = "";
                string filename = "";
                string ext = "";
                string contenttype = "";

                if (rblType.SelectedValue == "1")
                {
                    if (fuDocu.HasFile)
                    {
                        if (fuDocu.PostedFile.ContentLength <= int.Parse(ConfigurationManager.AppSettings["MaxFileSize"]))
                        {
                            filePath = fuDocu.PostedFile.FileName;
                            filename = Path.GetFileName(filePath).ToLower();
                            ext = Path.GetExtension(filename).ToLower();
                            contenttype = String.Empty;

                            contenttype = fuDocu.PostedFile.ContentType;

                            if (contenttype != String.Empty && ext != "js" && ext != "vb" && ext != "cs")
                            {
                                if (!contenttype.ToLower().Contains("video"))
                                {
                                    Stream fs = fuDocu.PostedFile.InputStream;
                                    BinaryReader br = new BinaryReader(fs);
                                    // Set Position to the beginning of the stream.
                                    br.BaseStream.Position = 0;
                                    Byte[] bytes = br.ReadBytes((Int32)fs.Length);

                                    prms.Add(new SqlParameter("@FileName", filename));
                                    prms.Add(new SqlParameter("@MIMEType", contenttype));
                                    prms.Add(new SqlParameter("@content", bytes));
                                    prms.Add(new SqlParameter("@FileExtension", ext));
                                }
                                else
                                {
                                    isVideo = true;
                                    prms.Add(new SqlParameter("@FileExtension", ext));
                                }
                            }
                            else if (litDocu.Text == "")
                            {
                                this.Page.ClientScript.RegisterClientScriptBlock(GetType(), "size", "<script> alert('You must select another type of file');</script>", false);

                                return false;
                            }
                        }
                        else
                        {
                            this.Page.ClientScript.RegisterClientScriptBlock(GetType(), "size", "<script> alert('File size is too big. Try another file smaller than " + ConfigurationManager.AppSettings["MaxFileSize"] + " ');</script>", false);
                            return false; ;
                        }
                    }
                    else if (litDocu.Text == "")
                    {
                        this.Page.ClientScript.RegisterClientScriptBlock(GetType(), "size", "<script> alert('You must select a file');</script>", false);
                        return false;
                    }
                }
                else
                {
                    prms.Add(new SqlParameter("@Url", tbUrl.Text));
                    prms.Add(new SqlParameter("@FileExtension", "link"));
                }


                string resourceid = ResourceID = MyDAL_Resources.ExecuteQuery(sql, prms, 120).ToString();

                SaveCategories(resourceid, CheckedBoxes);

                if (isVideo)
                {
                    DataTable dt = new DataTable();
                    SqlDataAdapter dapt = new SqlDataAdapter(@"SELECT * FROM [Groups] 
                            where id in (
                            select rg.[groupid] from [EKO_OTP_Resources].[dbo].[ResourcesGroups] rg 
                            inner join [EKO_OTP_Resources].[dbo].[Resource_Types_Link] t on rg.id = t.GroupId
                            where t.ResourceId = @id
                            )", ConfigurationManager.AppSettings.Get("CMServer"));
                    dapt.SelectCommand.Parameters.AddWithValue("@id", resourceid);
                    dapt.Fill(dt);

        
                    string path = ConfigurationManager.AppSettings["Resources.Video.Path"] + resourceid;

                    if (dt.Rows.Count > 0)
                    {
                        string docPath = GetFolder(path);

                        if (!Directory.Exists(docPath))
                            Directory.CreateDirectory(docPath);

                        string f = docPath + @"/web.config";
                        if (File.Exists(f))
                            File.Delete(f);

                        //CMSHelper.CreatePrivateConfigFile(dt.Rows[0]["name"].ToString(), path, dt.Rows[0]["id"].ToString());
                        CMSHelper.CreatePrivateConfigFile(dt.Rows[0]["name"].ToString(), docPath, dt.Rows[0]["id"].ToString());
                    }
                    else
                    {
                        string docPath = GetFolder(path);

                        //CMSHelper.CreatePrivateConfigFile("", path, "0");
                        CMSHelper.CreatePrivateConfigFile("", docPath, "0");
                    }


                    string file = GetFolder(path) + "\\" + filename;
                    if (File.Exists(file))
                        File.Delete(file);

                    fuDocu.SaveAs(file);

                    prms = new List<SqlParameter>();
                    prms.Add(new SqlParameter("@ResourceId", resourceid));
                    prms.Add(new SqlParameter("@FileName", filename));
                    prms.Add(new SqlParameter("@MIMEType", contenttype));
                    prms.Add(new SqlParameter("@FileExtension", ext));
                    prms.Add(new SqlParameter("@Length", fuDocu.FileBytes.Length));
                    prms.Add(new SqlParameter("@Path", path + "/"));

                    MyDAL_Resources.ExecuteNonQuery("Pie.Document_File_Insert", prms);
                }

                return true;
            }
        }

        //litErr.Text = "<span style='color:red;'>Select at least one category</span><br>";
        //return false;
    }

    private void SaveCategories(string resourceid, string[] checkedBoxes)
    {
        string sql_ins = "insert into Resource_Types_Link (ResourceId, TypeId, GroupId) values(@ResourceId, @TypeId, @GroupId)";
        string sql_del = "delete from Resource_Types_Link where ResourceId=@ResourceId and TypeId=@TypeId and GroupId=@GroupId";
        string patternCat = "fld_cat_";
        string patternSubCat = "fld_subcat_";
        string subcategs = "";

        foreach (string item in checkedBoxes)
        {
            string pattern = patternCat;
            if (!item.Contains(patternCat))
            {
                pattern = patternSubCat;
                if (!item.Contains(patternSubCat))
                    continue;
            }

            string libid = "", categid = "";

            int n = item.IndexOf(pattern);
            if (n > 0)
            {
                string stemp = item.Substring(n + pattern.Length);
                string[] vals = stemp.Split(new char[] { '_' });
                if (vals.Length != 2)
                    continue;

                libid = vals[0];
                categid = vals[1];

                string sql = sql_ins;
                try
                {
                    string val = Request.Form[item];
                    if (val != "on")
                        sql = sql_del;
                }
                catch { sql = sql_del; }


                List<SqlParameter> prms = new List<SqlParameter>();
                prms.Add(new SqlParameter("@ResourceId", resourceid));
                prms.Add(new SqlParameter("@TypeId", categid));
                prms.Add(new SqlParameter("@GroupId", libid));

                if (pattern == patternSubCat && sql == sql_ins)
                {
                    if (!String.IsNullOrEmpty(subcategs))
                        subcategs += ",";

                    subcategs = categid;
                }

                try { MyDAL_Resources.ExecuteNonQuery(sql, prms, CommandType.Text).ToString(); }
                catch { }
            }
        }

        if (!String.IsNullOrEmpty(subcategs))
        {
            string sql = "delete from Resource_Types_Link where ResourceId=@ResourceId and TypeId in (select parentid from ResourceTypes where id in (@ids))";
            List<SqlParameter> prms = new List<SqlParameter>();
            prms.Add(new SqlParameter("@ResourceId", resourceid));
            prms.Add(new SqlParameter("@ids", subcategs));

            MyDAL_Resources.ExecuteNonQuery(sql, prms, CommandType.Text).ToString();

        }
    }

    private string GetFolder(string _folder)
    {
        string physDir = HttpContext.Current.Request.MapPath(_folder);
        if (!System.IO.Directory.Exists(physDir))
        {
            System.IO.Directory.CreateDirectory(physDir);
        }
        return physDir;
    }

    private bool PopulateFields()
    {
        ClearFields();

        List<SqlParameter> prms = new List<SqlParameter>();
        prms.Add(new SqlParameter("@id", ResourceID));

        // DataTable dt = MyDAL_Resources.getSTable("Resources_Select", "@id", ResourceID);
        DataSet ds = MyDAL_Resources.getTables("Resources_Select", prms);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count > 0)
        {
            rblType.Enabled = false;
            DataRow rw = dt.Rows[0];

#if MULTI_LANGUAGE
            ddlLanguage2.SelectedValue = rw["LanguageId"].ToString();
#endif           

            tbTitle.Text = rw["Title"].ToString();
            tbDesc.Text = rw["Description"].ToString();
            tbKeywords.Text = rw["Keywords"].ToString();
            cbShow.Checked = rw["Show"].ToString().ToLower() == "true" ? false : true;
            ddlIcon.SelectedValue = rw["IconType"].ToString();

            if ((rblType.SelectedValue = (rw["IsDocument"].ToString().ToLower() == "true" ? "1" : "0")) == "1")
            {
                litDocu.Text = "<span class='bodytext'>" + rw["FileName"].ToString() + "</span>";
                lnkDeleteDocu.Visible = rw["FileName"].ToString().Length > 0;
                lnkDeleteDocu.Text = "Delete";
            }
            else
            {
                tbUrl.Text = rw["Url"].ToString();
            }

#if MULTI_LANGUAGE
          //trLanguage.Visible = false;
            ddlLanguage2.Enabled = false;
#endif
            RenderStatus(int.Parse(rw["status"].ToString()), imStatus);
            RenderStatus(int.Parse(rw["status"].ToString()), imStatus2);
            imStatus.Visible = true;
            imStatus2.Visible = true;

            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@id", ResourceID));
            DataTable dt2 = MyDAL_Resources.getTables("select * from Resources_Associated_Link where ResourceId=@id", parms.ToArray(), CommandType.Text).Tables[0];
            foreach (DataRow dr2 in dt2.Rows)
            {
                try
                {
                    ddlOtherResources.Items.FindByValue(dr2["ResourceAssociatedId"].ToString()).Selected = true;
                }
                catch { }
            }

            LibrariesPartial1.Populate(ResourceID);

            return true;
        }

        return false;
    }

#endregion


    protected void ddlFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.BindGrid();
        pager1.CurrentIndex = 1;
    }
    protected void lnkDeleteDocu_Click(object sender, EventArgs e)
    {
        string sql = "ResourceDocument_Delete";

        List<SqlParameter> parms = new List<SqlParameter>();
        parms.Add(new SqlParameter("@id", ResourceID));
        MyDAL_Resources.ExecuteNonQuery(sql, parms);
        litDocu.Text = "";
        lnkDeleteDocu.Visible = false;
    }
    protected void litDocu_Click(object sender, EventArgs e)
    {
        if (ResourceID != null)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@ResourceId", ResourceID));
            DataTable dt = MyDAL_Resources.getSTable("Pie.Document_select", parms);

            if (dt.Rows.Count > 0)
            {
                DataRow rw = dt.Rows[0];
                if (!rw["MIMEType"].ToString().ToLower().Contains("video"))
                {
                    //Response.ClearHeaders();
                    //Response.ClearContent();
                    //Response.Buffer = true;
                    //Response.Charset = "";
                    //Response.Cache.SetCacheability(HttpCacheability.NoCache);

                    //Response.ContentType = "application/octet-stream";
                    //Byte[] bytes = (Byte[])dt.Rows[0]["content"];
                    //Response.BinaryWrite(bytes);
                    //Response.AddHeader("content-disposition", "attachment; filename=" + dt.Rows[0]["FileName"].ToString());
                    //Response.Flush();
                    //Response.End();

                    Page.ClientScript.RegisterClientScriptBlock(
                        this.GetType(),
                        "ViewDocu",
                         String.Format("window.open('/OpenFile.ashx?id={0}');", ResourceID),
                        true);

                }
            }
        }

        LibrariesPartial1.Populate(ResourceID);
    }

    //protected void ddlLanguage2_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    PopulateCategories();

    //}    


    protected void rblLibraryTypes_SelectedIndexChanged(object sender, EventArgs e)
    {
        PopulateResourcesGroupCategories("1");
    }
}