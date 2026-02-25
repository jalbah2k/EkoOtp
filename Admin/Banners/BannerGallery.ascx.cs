using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.IO;
using CuteWebUI;
using System.Drawing.Imaging;

public partial class Admin_Banners_BannerGallery : System.Web.UI.UserControl
{
    protected string _logoPath = "/data/banners/{0}";

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
    public string sortExpBanners
    {
        get
        {
            return ViewState["sortExpBanners"] != null ? ViewState["sortExpBanners"].ToString() : "";
        }
        set
        {
            ViewState["sortExpBanners"] = value;
        }
    }
    public string sortOrderBanners
    {
        get
        {
            return ViewState["sortOrderBanners"] != null ? ViewState["sortOrderBanners"].ToString() : "desc";
        }
        set
        {
            ViewState["sortOrderBanners"] = value;
        }
    }
    public void PageSizeChange(object o, EventArgs e)
    {
        GV_Main.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
        pager1.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
        pager1.CurrentIndex = 1;
        GV_Main.PageIndex = 0;

        BindData();
    }
    public void pager_Command(object sender, CommandEventArgs e)
	{
		int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
		pager1.CurrentIndex = currnetPageIndx;
		GV_Main.PageIndex = currnetPageIndx - 1;

		BindData();
	}

    protected void Page_Load(object sender, EventArgs e)
    {
        tbCaption2.Attributes.Add("maxlength", "1000");

        if (!IsPostBack)
        {
            sortExp = "GalleryName"; // default sorted column
            sortOrder = "asc";    // default sort order
            sortExpBanners = "BannerPriority"; // default sorted column
            sortOrderBanners = "asc";    // default sort order

            if ((bool)Session["Multilingual"])
            {
                trLanguage.Visible = true;
            }

            BindData();
            Load_Times();
        }
        SwitchView();

		if (Session["GalleryID"] != null)
		{
			string physDir = HttpContext.Current.Request.MapPath("/data/banners/" + Session["GalleryID"].ToString());

			if (!Directory.Exists(physDir))
			{
				Directory.CreateDirectory(physDir);
			}

		}
    }

    protected string viewUpload
    {
        set { ViewState["vUpload"] = value; }
        get { return ViewState["vUpload"] == null ? "none" : ViewState["vUpload"].ToString(); }
    }

    protected string viewUploadEdit
    {
        set { ViewState["vUploadEdit"] = value; }
        get { return ViewState["vUploadEdit"] == null ? "none" : ViewState["vUploadEdit"].ToString(); }
    }

    #region dal
    protected string _connection = ConfigurationManager.AppSettings.Get("CMServer");
    protected string sqlGallerySelecty = "BannerGallerySelect";
    protected string sqlGalleryInsert = "BannerGalleryInsert";
    protected string sqlGalleryUpdate = "BannerGalleryUpdate";
    protected string sqlGalleryDelete = "BannerGalleryDelete";
    protected string sqlBannersSelect = "BannersSelect";
    protected string sqlBannersInsert = "BannersInsert";
    protected string sqlBannersUpdate = "BannersUpdate";
    protected string sqlBannersDelete = "BannersDelete";
    protected string sqlBannersSwapPriority = "BannersSwapPriority";

    //   protected string sqlUpdate = "";

    private DataTable getTable(string cmd)
    {
        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter(cmd, _connection);
        da.SelectCommand.CommandType = CommandType.StoredProcedure;
        da.Fill(dt);
        return dt;
    }
    private DataTable getTable(SqlCommand cmd)
    {
        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.SelectCommand.CommandType = CommandType.StoredProcedure;
        da.SelectCommand.Connection = new SqlConnection(_connection);
        da.Fill(dt);
        return dt;
    }

    private DataTable getTable(SqlCommand cmd, bool a)
    {
        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.SelectCommand.CommandType = CommandType.Text;
        da.SelectCommand.Connection = new SqlConnection(_connection);
        da.Fill(dt);
        return dt;
    }

    private DataSet getTables(SqlCommand cmd, bool a)
    {
        DataSet ds = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.SelectCommand.CommandType = CommandType.Text;
        da.SelectCommand.Connection = new SqlConnection(_connection);
        da.Fill(ds);
        return ds;
    }

    private bool ProcessRecord(string sql, string fld, string _value)
    {
        SqlCommand cmd = new SqlCommand(sql, new SqlConnection(_connection));
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue(fld, _value);
        cmd.Connection.Open();
        int ret = cmd.ExecuteNonQuery();
        cmd.Connection.Close();
        return (ret > 0);
    }

    private bool RemoveRecord(string sql, int rcrd)
    {
        return ProcessRecord(sql, "@id", rcrd.ToString());
    }


    private string AddGallery()
    {
        SqlCommand cmd = new SqlCommand(sqlGalleryInsert, new SqlConnection(_connection));
        if (imgSubmit.CommandName == "save")
        {
            cmd.CommandText = sqlGalleryUpdate;
            cmd.Parameters.AddWithValue("@id", imgSubmit.CommandArgument.ToString());
        }
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Name", tbGalleryName.Text.Trim());
        cmd.Parameters.AddWithValue("@language", ddlLanguage.SelectedValue);
        cmd.Parameters.AddWithValue("@group", ddlGroup.SelectedValue);
        if (txtWidth.Text.Trim().Length > 0)
            cmd.Parameters.AddWithValue("@width", txtWidth.Text.Trim());
        if (txtHeight.Text.Trim().Length > 0)
            cmd.Parameters.AddWithValue("@height", txtHeight.Text.Trim());
        cmd.Parameters.AddWithValue("@autoplay", cbAutoplay.Checked);
        cmd.Parameters.AddWithValue("@shuffle", cbShuffle.Checked);
        cmd.Parameters.AddWithValue("@dir", ddlDirection.SelectedValue);
        cmd.Parameters.AddWithValue("@view", ddlTransitions.SelectedValue);
        cmd.Connection.Open();
        string ret = (string)cmd.ExecuteScalar();
        cmd.Connection.Close();
        return ret;
    }

    private bool DeleteGallery()
    {
        return RemoveRecord(sqlGalleryDelete, GalleryID);
    }

    private string AddBanner()
    {
        SqlCommand cmd = new SqlCommand(sqlBannersInsert, new SqlConnection(_connection));
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@FileName", FileName);
        cmd.Parameters.AddWithValue("@URL", URL);
        cmd.Parameters.AddWithValue("@target", Convert.ToInt32(rbOpenNew2.Checked));
        cmd.Connection.Open();
        string ret = cmd.ExecuteScalar().ToString();
        //       cmd.Connection.Close();
        return ret;
    }

    private string AddBanner2()
    {

        SqlCommand cmd = new SqlCommand(@"update banners set BannerName=@filename, bannerlink=@url, PresentationClass=@class, target=@target, 
                                        name=@name, title=@title,body=@body,AltText=@AltText,StartDate=@StartDate,EndDate=@EndDate,
                                        ButtonText=@ButtonText,ButtonTitle=@ButtonTitle,ButtonLink=@ButtonLink,ButtonText1=@ButtonText1,
                                        ButtonTitle1=@ButtonTitle1,ButtonLink1=@ButtonLink1 
                                        where bannerid=@id 
                                        exec Report.ContentLogModifications 'banners', @id, @userid", new SqlConnection(_connection));
        if (currentAction == Actions.AddBanner)
        {
            cmd.CommandText = @"insert banners (BannerName, bannerlink, PresentationClass, target, name, title, body, AltText, StartDate, EndDate, 
                                                ButtonText, ButtonTitle,  ButtonLink,   ButtonText1, ButtonTitle1, ButtonLink1, gallery,
                                                BannerStatus, BannerFileLocation)
                                        values(@filename, @url, @class, @target, @name, @title, @body, @AltText, @StartDate, @EndDate, 
                                                @ButtonText, @ButtonTitle,  @ButtonLink,   @ButtonText1, @ButtonTitle1, @ButtonLink1, @gallery,
                                                'A', '/data/Banners/')";

            cmd.Parameters.AddWithValue("@gallery", HttpContext.Current.Session["GalleryID"].ToString());

        }
        else
            cmd.Parameters.AddWithValue("@id", BannerID);

        cmd.CommandType = CommandType.Text;
        cmd.Parameters.AddWithValue("@filename", ddlFilename.SelectedValue);
        cmd.Parameters.AddWithValue("@url", URL);
        cmd.Parameters.AddWithValue("@name", tbName2.Text);
        cmd.Parameters.AddWithValue("@title", txtTitle.Text );
        cmd.Parameters.AddWithValue("@body", tbCaption2.Text );
        cmd.Parameters.AddWithValue("@target", Convert.ToInt32(rbOpenNew2.Checked));
        cmd.Parameters.AddWithValue("@AltText", txtAltText.Text.Trim());
        cmd.Parameters.AddWithValue("@userid", Session["LoggedInID"].ToString());

        cmd.Parameters.AddWithValue("@ButtonText", txtButtonText.Text);
        cmd.Parameters.AddWithValue("@ButtonTitle", txtButtonTitle.Text);
        cmd.Parameters.AddWithValue("@ButtonLink", txtButtonLink.Text);
        cmd.Parameters.AddWithValue("@class", ddlClass.SelectedValue);

        cmd.Parameters.AddWithValue("@ButtonText1", txtButtonText1.Text);
        cmd.Parameters.AddWithValue("@ButtonTitle1", txtButtonTitle1.Text);
        cmd.Parameters.AddWithValue("@ButtonLink1", txtButtonLink1.Text);


        if (txtStartDate.Text != "")
            cmd.Parameters.AddWithValue("@StartDate", txtStartDate.Text + ' ' + ddlStartTime.SelectedValue);
        else
            cmd.Parameters.AddWithValue("@StartDate", DBNull.Value);

        if (txtEndDate.Text != "")
            cmd.Parameters.AddWithValue("@EndDate", txtEndDate.Text + ' ' + ddlEndTime.SelectedValue);
        else
            cmd.Parameters.AddWithValue("@EndDate", DBNull.Value);

        cmd.Connection.Open();
        cmd.ExecuteNonQuery();
        string ret = "";
        cmd.Connection.Close();
        return ret;
    }

    private bool DeleteBanner()
    {
        return RemoveRecord(sqlBannersDelete, BannerID);
    }

    private bool SwapBannerPriority(int _dir)
    {
        SqlCommand cmd = new SqlCommand(sqlBannersSwapPriority, new SqlConnection(_connection));
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@id", BannerID);
        cmd.Parameters.AddWithValue("@direction", _dir);
        cmd.Connection.Open();
        int ret = cmd.ExecuteNonQuery();
        cmd.Connection.Close();
        return ret > 0;
    }

    #endregion dal

    #region SwitchView

    protected Actions currentAction
    {
        set { ViewState["Action"] = value; }
        get
        {
            if (ViewState["Action"] == null)
                return Actions.View;
            else
                return (Actions)ViewState["Action"];
        }

    }

    protected enum Actions
    { View = 1, Add, Edit, AddBanner, EditBanner }

    private void SwitchView(Actions act)
    {
        currentAction = act;
        pnlGalleryList.Visible = (currentAction == Actions.View);
        btGallery.Visible = (currentAction == Actions.View);
        pnlAddGallery.Visible = (currentAction == Actions.Add);
        pnlBanners.Visible = (currentAction == Actions.Edit);
        pnlEditBanner.Visible = (currentAction == Actions.AddBanner) || (currentAction == Actions.EditBanner);
/*
        if (currentAction == Actions.Edit)
        {
            ((FileSaverAdapter)FileUploader1.Adapters[0]).FolderName = string.Format(_logoPath,GalleryID);
            ((DocProc.Banners)FileUploader1.Adapters[1]).gallery = GalleryID.ToString();
        }
*/		
    }

    private void SwitchView()
    {
        SwitchView(currentAction);
    }
    #endregion SwitchView

    #region Gallery
    protected void btGallery_Click(object sender, EventArgs e)
    {
        ClearFields();
        imgSubmit.CommandName = "add";
        SwitchView(Actions.Add);
    }

    private void Edit(string id)
    {
        ClearFields();

        DataTable dt = new DataTable();
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]))
        {
            SqlDataAdapter dapt = new SqlDataAdapter("select * from BannerGallery where id=@id", conn);
            dapt.SelectCommand.Parameters.AddWithValue("@id", id);
            dapt.Fill(dt);
        }

        if (dt.Rows.Count > 0)
        {
            tbGalleryName.Text = dt.Rows[0]["Name"].ToString();
            ddlLanguage.ClearSelection();
            try
            {
                ddlLanguage.SelectedValue = dt.Rows[0]["Language"].ToString();
            }
            catch { }
            ddlGroup.ClearSelection();
            try
            {
                ddlGroup.SelectedValue = dt.Rows[0]["group_id"].ToString();
            }
            catch { }
            txtWidth.Text = dt.Rows[0]["width"].ToString();
            txtHeight.Text = dt.Rows[0]["height"].ToString();
            cbAutoplay.Checked = (bool)dt.Rows[0]["autoplay"];
            cbShuffle.Checked = (bool)dt.Rows[0]["shuffle"];
            ddlDirection.ClearSelection();
            try
            {
                ddlDirection.SelectedValue = dt.Rows[0]["dir"].ToString();
            }
            catch { }
            ddlTransitions.ClearSelection();
            try
            {
                ddlTransitions.SelectedValue = dt.Rows[0]["view"].ToString();
            }
            catch { }
        }

        imgSubmit.CommandName = "save";
        imgSubmit.CommandArgument = id;
        
        SwitchView(Actions.Add);
    }

    private void ClearFields()
    {
        tbGalleryName.Text = "";
        ddlLanguage.ClearSelection();
        ddlGroup.ClearSelection();
        ddlGroup.SelectedValue = "1";
        txtWidth.Text = "960";
        txtHeight.Text = "300";
        cbAutoplay.Checked = true;
        cbShuffle.Checked = false;
        ddlDirection.ClearSelection();
        ddlTransitions.ClearSelection();
        txtTitle.Text = "";
        tbCaption2.Text = "";

        txtButtonText.Text = "";
        txtButtonTitle.Text = "";
        txtButtonLink.Text = "";

        txtButtonText1.Text = "";
        txtButtonTitle1.Text = "";
        txtButtonLink1.Text = "";

        ddlClass.ClearSelection();

    }

    private void ClearBannerFields()
    {
        BannerID = -1;
        ddlFilename.ClearSelection();

        tbUrl2.Text = String.Empty;
        tbName2.Text = String.Empty;
        tbCaption2.Text = String.Empty;
        txtTitle.Text = String.Empty;
        rbOpenNew2.Checked = false;
        rbOpen2.Checked = false;
        txtAltText.Text = String.Empty;

        txtButtonText.Text = String.Empty;
        txtButtonTitle.Text = String.Empty;
        txtButtonLink.Text = String.Empty;

        ddlClass.ClearSelection();

        txtButtonText1.Text = String.Empty;
        txtButtonTitle1.Text = String.Empty;
        txtButtonLink1.Text = String.Empty;


        ddlStartTime.ClearSelection();
        ddlEndTime.ClearSelection();
        txtStartDate.Text = "";
        txtEndDate.Text = "";

    }

    //protected void imgSubmit_Click(object sender, ImageClickEventArgs e)
    public void imgSubmit_Click(object sender, CommandEventArgs e)
    {
        AddGallery();
        SwitchView(Actions.View);
        BindData();
    }
    protected void imgBack_Click(object sender, EventArgs e)
    {
        SwitchView(Actions.View);
    }

    private bool BindData()
    {
        return BindData(sortExp, sortOrder);
    }
    private bool BindData(string sortExp, string sortDir)
    {
        DataSet dsMain = new DataSet();
        //SqlCommand sq = new SqlCommand("declare @id int declare @name nvarchar(100) select ID id,Name GalleryName,LanguageID,LangName Language from (select ID,Name,Language LanguageID, case Language when 1 then 'English' when 2 then 'French' else 'unknown' end LangName from bannergallery where group_id in (select group_id from users_groups_access where user_id=" + Session["LoggedInID"].ToString() + ") and (@id is null or @id=ID) and (@name is null or @name=Name)) a");
        string sqlstr = " declare @id int declare @name nvarchar(100) ";
        sqlstr += " select bg.ID, bg.Name as GalleryName, bg.Language as LanguageID, l.name as Language from bannergallery bg inner join Languages l on bg.Language = l.id where bg.group_id in (select group_id from users_groups_access where user_id=@user_id) and (@lang is null or bg.Language=@lang) select * from BannerIcons order by name";
        SqlCommand sq = new SqlCommand(sqlstr);
        sq.Parameters.AddWithValue("@user_id", Session["LoggedInID"]);
        if (!(bool)Session["Multilingual"])
            sq.Parameters.AddWithValue("@lang", 1);
        else
            sq.Parameters.AddWithValue("@lang", DBNull.Value);
        dsMain = getTables(sq, true);

        DataTable dt = dsMain.Tables[0];

        DataView DV = dt.DefaultView;
        if (sortExp != string.Empty)
        {
            DV.Sort = string.Format("{0} {1}", sortExp, sortDir);
        }
        this.GV_Main.DataSource = DV;
        this.GV_Main.DataBind();

		pager1.ItemCount = DV.Count;
        pager1.Visible = DV.Count > GV_Main.PageSize;

        litPagerShowing.Text = CMSHelper.GetPagerInfo(GV_Main, DV.Count);

        ddlFilename.DataSource = dsMain.Tables[1];
        ddlFilename.DataBind();
        ddlFilename.Items.Insert(0, "");


        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings.Get("CMServer"));
        SqlDataAdapter dapt = new SqlDataAdapter("select * from groups order by name select * from languages", sqlConn);
        DataSet ds = new DataSet();
        dapt.Fill(ds);
        ddlGroup.DataSource = ds.Tables[0];
        ddlGroup.DataBind();

    	ddlLanguage.DataSource = ds.Tables[1];
    	ddlLanguage.DataTextField = "name";
    	ddlLanguage.DataValueField = "id";
		ddlLanguage.DataBind();

        try
        {
            ddlGroup.Items.FindByValue("Common").Selected = true;
        }
        catch (Exception){ }

        return (dt.Rows.Count > 0);
    }
    protected int GalleryID
    {
        set { ViewState["GalleryID"] = value; }
        get { return (ViewState["GalleryID"]==null?-1:(int)ViewState["GalleryID"]); }
    }
	
	
    #endregion gallery

    #region Grid_Gallery

    protected void GV_Main_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        // Language column
        e.Row.Cells[1].Visible = (bool)Session["Multilingual"];

        #region Add sorted class to headers
        if (e.Row.RowType == DataControlRowType.Header)
        {
            int colIndex = CMSHelper.GetColumnIndex((sender as GridView), sortExp);
            e.Row.Cells[colIndex].CssClass = sortOrder == "asc" ? "sortasc" : "sortdesc";
        }
        #endregion Add sorted class to headers

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton LB_Delete = (ImageButton)e.Row.FindControl("LB_Delete");
            LB_Delete.Attributes.Add("onclick", "return confirm('Are you sure want to delete this Banner Gallery?')");
        }
    }
    protected void GV_Main_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "EditGallery")
        {
            Edit(e.CommandArgument.ToString());
        }
    }
    protected void GV_Main_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        GalleryID = (int)GV_Main.DataKeys[e.RowIndex].Value;
        DeleteGallery();
        this.BindData();
    }
    protected void GV_Main_RowEditing(object sender, GridViewEditEventArgs e)
    {
        Session["GalleryID"]=Convert.ToInt32(GV_Main.DataKeys[e.NewEditIndex].Values[0]);
		
		GalleryID = Convert.ToInt32(GV_Main.DataKeys[e.NewEditIndex].Values[0]);
        GalleryName = GV_Main.DataKeys[e.NewEditIndex].Values[1].ToString();
        GalleryLanguage = GV_Main.DataKeys[e.NewEditIndex].Values[2].ToString();
        SwitchView(Actions.Edit);
        this.BannersBindData();
    }

    protected void GV_Main_Sorting(object sender, GridViewSortEventArgs e)
    {
        sortExp = e.SortExpression;

        if (sortOrder == "desc")
            sortOrder = "asc";
        else
            sortOrder = "desc";
        BindData(e.SortExpression, sortOrder);
    }

    #endregion Grid_Gallery


    #region BannersGrid
    protected void GV_Banners_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        #region Add sorted class to headers
        //if (e.Row.RowType == DataControlRowType.Header)
        //{
        //    int colIndex = CMSHelper.GetColumnIndex((sender as GridView), sortExpBanners);
        //    e.Row.Cells[colIndex].CssClass = sortOrderBanners == "asc" ? "sortasc" : "sortdesc";
        //}
        #endregion Add sorted class to headers

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton LB_Delete = (ImageButton)e.Row.FindControl("LB_Delete");
            LB_Delete.Attributes.Add("onclick", "return confirm('Are you sure want to delete this Banner?')");

            DataRowView dr = (DataRowView)e.Row.DataItem;
            Literal litHfId = (Literal)e.Row.FindControl("litHfId");
            litHfId.Text = String.Format("<input type='hidden' class='HfId' value='{0}' />", dr["BannerID"].ToString());
        }
    }
    protected void GV_Banners_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Up")
        {
            BannerID = Convert.ToInt32(e.CommandArgument);
            SwapBannerPriority(1);
            BannersBindData();
        }
        else if (e.CommandName == "Down")
        {
            BannerID = Convert.ToInt32(e.CommandArgument);
            SwapBannerPriority(0);
            BannersBindData();
        }
    }

    protected void GV_Banners_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        BannerID = (int)GV_Banners.DataKeys[e.RowIndex].Value;
        DeleteBanner();
        this.BannersBindData();
    }
    protected void GV_Banners_RowEditing(object sender, GridViewEditEventArgs e)
    {
        SwitchView(Actions.EditBanner);

        BannerID = (int)GV_Banners.DataKeys[e.NewEditIndex].Value;
        //txtfilename.Text = GV_Banners.DataKeys[e.NewEditIndex].Values[1].ToString();
        string filename = GV_Banners.DataKeys[e.NewEditIndex].Values[1].ToString();
        try { ddlFilename.SelectedValue = filename; }
        catch { ddlFilename.Items.Add(new ListItem(filename)); }

        tbUrl2.Text = GV_Banners.DataKeys[e.NewEditIndex].Values[2].ToString();
        tbName2.Text = GV_Banners.DataKeys[e.NewEditIndex].Values[4].ToString();
        tbCaption2.Text = GV_Banners.DataKeys[e.NewEditIndex].Values[10].ToString();
        txtTitle.Text = GV_Banners.DataKeys[e.NewEditIndex].Values[9].ToString();
        rbOpenNew2.Checked = Convert.ToBoolean(((GridView)sender).DataKeys[e.NewEditIndex].Values[3]);
        rbOpen2.Checked = !rbOpenNew2.Checked;
        txtAltText.Text = GV_Banners.DataKeys[e.NewEditIndex].Values[6].ToString();

        txtButtonText.Text = GV_Banners.DataKeys[e.NewEditIndex].Values[11].ToString();
        txtButtonTitle.Text = GV_Banners.DataKeys[e.NewEditIndex].Values[12].ToString();
        txtButtonLink.Text = GV_Banners.DataKeys[e.NewEditIndex].Values[13].ToString();

        ddlClass.SelectedValue = GV_Banners.DataKeys[e.NewEditIndex].Values[14].ToString();

        txtButtonText1.Text = GV_Banners.DataKeys[e.NewEditIndex].Values[15].ToString();
        txtButtonTitle1.Text = GV_Banners.DataKeys[e.NewEditIndex].Values[16].ToString();
        txtButtonLink1.Text = GV_Banners.DataKeys[e.NewEditIndex].Values[17].ToString();


        ddlStartTime.ClearSelection();
        ddlEndTime.ClearSelection();
        txtStartDate.Text = "";
        txtEndDate.Text = "";

        if (!String.IsNullOrEmpty(GV_Banners.DataKeys[e.NewEditIndex].Values[7].ToString()))
        {
            DateTime dtime = Convert.ToDateTime(GV_Banners.DataKeys[e.NewEditIndex].Values[7].ToString());
            try { ddlStartTime.Items.FindByText (dtime.ToShortTimeString()).Selected = true; }
            catch(Exception ex) { }
            try { txtStartDate.Text = dtime.ToString("yyyy/MM/dd"); }
            catch { }
        }
        if (!String.IsNullOrEmpty(GV_Banners.DataKeys[e.NewEditIndex].Values[8].ToString()))
        {
            DateTime dtime = Convert.ToDateTime(GV_Banners.DataKeys[e.NewEditIndex].Values[8].ToString());
            try { ddlEndTime.Items.FindByText(dtime.ToShortTimeString()).Selected = true; }
            catch (Exception ex) { }
            try { txtEndDate.Text = dtime.ToString("yyyy/MM/dd"); }
            catch { }
        }

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

        time = time.AddMinutes(15);
        while (time < end && Counter < 96)
        {
            ddlStartTime.Items.Add(new ListItem(time.ToString("h:mm tt"), time.ToString("HH:mm")));
            ddlEndTime.Items.Add(new ListItem(time.ToString("h:mm tt"), time.ToString("HH:mm")));

            time = time.AddMinutes(15);

            Counter++;
        }

        ddlStartTime.Items.Add(new ListItem("11:59 PM", "23:59"));
        ddlEndTime.Items.Add(new ListItem("11:59 PM", "23:59"));
    }

    protected void GV_Banners_Sorting(object sender, GridViewSortEventArgs e)
    {
        sortExpBanners = e.SortExpression;

        if (sortOrderBanners == "desc")
            sortOrderBanners = "asc";
        else
            sortOrderBanners = "desc";
        BannersBindData(e.SortExpression, sortOrderBanners);
    }

    protected void imgEditSave_Click(object sender, ImageClickEventArgs e)
    {
        SwitchView(Actions.View);
    }
    protected void imbEditCancel_Click(object sender, ImageClickEventArgs e)
    {
        SwitchView(Actions.View);
    }
    #endregion BannersGrid

    #region Banners

    protected int BannerID
    {
        set { ViewState["BannerID"] = value; }
        get { return (int)ViewState["BannerID"]; }
    }

    protected string GalleryName
    {
        set { ViewState["GalleryName"] = value; }
        get { return ViewState["GalleryName"].ToString(); }
    }
    protected string GalleryLanguage
    {
        set { ViewState["GalleryLanguage"] = value; }
        get { return ViewState["GalleryLanguage"].ToString(); }
    }



    protected string FileName
    {
        set { ViewState["FileName"] = value; }
        get { return (ViewState["FileName"] == null ? "" : ViewState["FileName"].ToString()); }
    }
    protected string URL
    {
        set { ViewState["URL"] = value; }
        get { return ViewState["URL"].ToString(); }
    }

    private bool BannersBindData()
    {
        return BannersBindData(sortExpBanners, sortOrderBanners);
    }
    private bool BannersBindData(string sortExp, string sortDir)
    {
        DataTable dt = new DataTable();
        SqlCommand cmd = new SqlCommand(sqlBannersSelect);
        cmd.Parameters.AddWithValue("@gallery", GalleryID);

        dt = getTable(cmd);
        DataView DV = dt.DefaultView;
        if (sortExp != string.Empty)
        {
            DV.Sort = string.Format("{0} {1}", sortExp, sortDir);
        }
        this.GV_Banners.DataSource = DV;
        this.GV_Banners.DataBind();

        if (dt.Rows.Count > 0)
        {
            this.pnlBannersVisible.Visible = true;
            this.pnlNoBanners.Visible = false;
        }
        else
        {
            this.pnlBannersVisible.Visible = false;
            this.pnlNoBanners.Visible = true;
        }
        return (dt.Rows.Count > 0);
    }

    protected void imgBannerEditSave_Click2(object sender, EventArgs e)
    {
        FixUrl(tbUrl2.Text.Trim());

        AddBanner2();
        SwitchView(Actions.Edit);
        this.BannersBindData();
        //UploadStatusLabel.Text = "";
        tbUrl2.Text = "";
        txtAltText.Text = "";

        ddlStartTime.ClearSelection();
        ddlEndTime.ClearSelection();
        txtStartDate.Text = "";
        txtEndDate.Text = "";

    }

    private void FixUrl(string _url)
    {
        if (_url.Length > 0)
            if (_url.Length > 0 && !(_url.Contains("http://") || _url.Contains("https://")) && _url.IndexOf('/') != 0)
                _url = string.Concat("https://", _url);

        URL = _url;
    }


    private string GetFolder()
    {
        string _subfolder = this.GalleryID.ToString();
        string _folder = _logoPath + _subfolder + "/";
        string physDir = HttpContext.Current.Request.MapPath(_folder);

        if (!Directory.Exists(physDir))
        {
            Directory.CreateDirectory(physDir);
        }
        return physDir;
    }

    protected void imbBannerEditCancel_Click(object sender, EventArgs e)
    {
        SwitchView(Actions.Edit);
    }

	/*
	 *   <span id="spBannerEdit" runat="server">
                <div style="margin-top: 10px; margin-bottom: 10px">
                    <asp:ImageButton ID="imgBannerAdd" runat="server" CssClass="button" ImageUrl="/images/buttons/add.gif"
                        OnClick="imgBannerAdd_Click"  Visible="false" />
                </div>
            </span>
	 * */
    protected void imgBannerAdd_Click(object sender, EventArgs e)
    {
        SwitchView(Actions.AddBanner);
    }
    protected void imgBannerBack_Click(object sender, EventArgs e)
    {
        this.GV_Main.EditIndex = -1;
        BindData();
        SwitchView(Actions.View);
    }

    #endregion BannersGrid

    public void refresh(object o, EventArgs e)
    {
        BannersBindData();
    }

	public void refresh(object o, UploaderEventArgs[] e)
	{
		BannersBindData();
	}

    protected string Test
    {
        set { ViewState["Test"] = value; }
        get
        {
            if (ViewState["Test"] == null)
                return "";
            else
                return ViewState["Test"].ToString();
        }
    }

	public void fileup(object o, UploaderEventArgs e)
	{
        string baseLocation = "~/data/banners/" + Session["GalleryID"].ToString() + "/";

        try { e.CopyTo(Request.MapPath(baseLocation + e.FileName)); }
        catch { }
        SaveThumbnail(baseLocation, e.FileName);

        SqlConnection sqlConn = new SqlConnection(_connection);
		SqlCommand sqlComm = new SqlCommand("BannersInsert", sqlConn);
		sqlComm.CommandType = CommandType.StoredProcedure;
		sqlComm.Parameters.AddWithValue("@filename", e.FileName);
		sqlComm.Parameters.AddWithValue("@gallery", HttpContext.Current.Session["GalleryID"].ToString());
		sqlConn.Open();
		sqlComm.ExecuteNonQuery();
		sqlConn.Close();
	}

    private void SaveThumbnail(string location, string filename)
    {
        int MaxSideSize = 233;
        int MaxHeight = 121;

        // create an image object, using the filename we just retrieved
        System.Drawing.Image imgInput = System.Drawing.Image.FromFile(Server.MapPath(location + filename));


        #region Calculate new size
        int intNewWidth;
        int intNewHeight;

        //get image original width and height 
        int intOldWidth = imgInput.Width;
        int intOldHeight = imgInput.Height;

        //determine if landscape or portrait 
        int intMaxSide;

        if (intOldWidth >= intOldHeight)
        {
            intMaxSide = intOldWidth;
        }
        else
        {
            intMaxSide = intOldHeight;
        }


        if (intMaxSide > MaxSideSize)
        {
            //set new width and height 
            double dblCoef = MaxSideSize / (double)intMaxSide;
            intNewWidth = Convert.ToInt32(dblCoef * intOldWidth);
            intNewHeight = Convert.ToInt32(dblCoef * intOldHeight);
        }
        else
        {
            intNewWidth = intOldWidth;
            intNewHeight = intOldHeight;
        }
        #endregion

        try { intNewHeight = MaxHeight; }
        catch { }

        string thumbfolder = Server.MapPath(location + "/thumbnails/");
        if (!Directory.Exists(thumbfolder))
            Directory.CreateDirectory(thumbfolder);

        // create the actual thumbnail image
        System.Drawing.Image thumbnailImage = imgInput.GetThumbnailImage(intNewWidth, intNewHeight, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
        //Determine image format 
        ImageFormat fmtImageFormat = imgInput.RawFormat;

        thumbnailImage.Save(thumbfolder + filename, fmtImageFormat);

    }

    public bool ThumbnailCallback()
    {
        return true;
    }


    protected void btnAddItem_Click(object sender, EventArgs e)
    {
        ClearBannerFields();
        SwitchView(Actions.AddBanner);

    }
}
