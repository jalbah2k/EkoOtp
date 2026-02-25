using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

public partial class Admin_Content_Content : System.Web.UI.UserControl
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
    public string sortExpV
    {
        get
        {
            return ViewState["sortExpV"] != null ? ViewState["sortExpV"].ToString() : "";
        }
        set
        {
            ViewState["sortExpV"] = value;
        }
    }
    public string sortOrderV
    {
        get
        {
            return ViewState["sortOrderV"] != null ? ViewState["sortOrderV"].ToString() : "desc";
        }
        set
        {
            ViewState["sortOrderV"] = value;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            sortExp = "name"; // default sorted column
            sortOrder = "asc";    // default sort order
            sortExpV = "timestamp"; // default sorted column
            sortOrderV = "desc";    // default sort order
            BindData();
        }
    }

    public void PageSizeChange(object o, EventArgs e)
    {
        this.GV_Main.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
        pager1.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
        pager1.CurrentIndex = 1;
        this.GV_Main.PageIndex = 0;
        BindData();
    }
    public void pager_Command(object sender, CommandEventArgs e)
	{
		int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
		pager1.CurrentIndex = currnetPageIndx;
		GV_Main.PageIndex = currnetPageIndx - 1;
		//BindRepeater();
		BindData();
	}

    protected void ibCancel_Click(object sender, EventArgs e)
    {
        this.GV_Main.EditIndex = -1;
        CurrentView = "Main";
        BindData();
    }

    protected void ibSave_Click(object sender, EventArgs e)
    {
        CurrentView = "Main";

        ExecQuery(sqlAuditSave, new SqlParameter[]{
            new SqlParameter("@htmlid",HtmlID),
            new SqlParameter("@vaultid",VaultID)});
        BindDataV();
        BindHTML();

    }

    #region dal
    protected string _connection = ConfigurationManager.AppSettings.Get("CMServer");
    protected string sqlContentSelect = "HTMLContentSelect";
    protected string sqlVaultSelect = "HTMLVaultSelect";
    protected string sqlAuditSave = "HTMLAuditUpdate";


    private DataTable getTable(SqlCommand cmd, bool sql)
    {
        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        if (!sql)
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
        da.SelectCommand.Connection = new SqlConnection(_connection);
        da.Fill(dt);
        return dt;
    }

    private DataTable getTable(string cmd, bool sql)
    {
        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter(cmd, _connection);
		if (!sql)
		{
			da.SelectCommand.CommandType = CommandType.StoredProcedure;
			da.SelectCommand.Parameters.AddWithValue("@Filter", txtFilter.Text);
		}
        da.Fill(dt);
        return dt;
    }

    private DataTable getTable(string cmd, SqlParameter[] prms, bool sql)
    {
        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter(cmd, _connection);
        if (!sql)
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
        da.SelectCommand.Parameters.AddRange(prms);
        da.Fill(dt);
        return dt;

    }

    private bool ExecQuery(string cmd, SqlParameter[] prms, bool sql)
    {
        SqlCommand sqlCmd = new SqlCommand(cmd, new SqlConnection(_connection));
        sqlCmd.Parameters.AddRange(prms);
        if (!sql)
            sqlCmd.CommandType = CommandType.StoredProcedure;
        sqlCmd.Connection.Open();
        int ret = sqlCmd.ExecuteNonQuery();
        sqlCmd.Connection.Close();
        return ret > 0;
    }

    private bool ExecQuery(string cmd, SqlParameter[] prms)
    {
        return ExecQuery(cmd, prms, false);
    }

    private DataTable getTable(SqlCommand cmd)
    {
        return getTable(cmd, false);
    }

    private DataTable getTable(string cmd)
    {
        return getTable(cmd, false);
    }

    private DataTable getTable(string cmd, SqlParameter[] prms)
    {
        return getTable(cmd, prms, false);
    }

    #endregion dal




    #region GridView
    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        #region Add sorted class to headers
        if (e.Row.RowType == DataControlRowType.Header)
        {
            int colIndex = CMSHelper.GetColumnIndex((sender as GridView), sortExp);
            e.Row.Cells[colIndex].CssClass = sortOrder == "asc" ? "sortasc" : "sortdesc";
        }
        #endregion Add sorted class to headers

        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //}
    }
    /*protected void GV_Main_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.GV_Main.PageIndex = e.NewPageIndex;
        BindData();
    }*/
    protected void GV_Main_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }

    protected void GV_Main_RowEditing(object sender, GridViewEditEventArgs e)
    {
        CurrentView = "Vault";
        HtmlID = Convert.ToInt32(this.GV_Main.DataKeys[e.NewEditIndex].Value);
        BindDataV();
        BindHTML();
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

    /*protected void GV_Vault_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.GV_Vault.PageIndex = e.NewPageIndex;
        BindDataV();
        VaultID = Convert.ToInt32(this.GV_Vault.DataKeys[this.GV_Vault.Rows[0].RowIndex].Value);
        BindHTML();
    }*/
    public void PageSizeChangeVault(object o, EventArgs e)
    {
        this.GV_Vault.PageSize = Convert.ToInt32(ddlPageSizeVault.SelectedValue);
        pagerVault.PageSize = Convert.ToInt32(ddlPageSizeVault.SelectedValue);
        pagerVault.CurrentIndex = 1;
        this.GV_Vault.PageIndex = 0;
        BindDataV();
        VaultID = Convert.ToInt32(this.GV_Vault.DataKeys[this.GV_Vault.Rows[0].RowIndex].Value);
        BindHTML();
    }
    public void pagerVault_Command(object sender, CommandEventArgs e)
    {
        int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
        pagerVault.CurrentIndex = currnetPageIndx;
        GV_Vault.PageIndex = currnetPageIndx - 1;
        BindDataV();
        VaultID = Convert.ToInt32(this.GV_Vault.DataKeys[this.GV_Vault.Rows[0].RowIndex].Value);
        BindHTML();
    }

    protected void GV_Vault_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        #region Add sorted class to headers
        if (e.Row.RowType == DataControlRowType.Header)
        {
            int colIndex = CMSHelper.GetColumnIndex((sender as GridView), sortExpV);
            e.Row.Cells[colIndex].CssClass = sortOrderV == "asc" ? "sortasc" : "sortdesc";
        }
        #endregion Add sorted class to headers

        Image imgMarker = (Image)e.Row.FindControl("imgMarker");
        if (imgMarker != null && (Convert.ToInt32(e.Row.RowIndex)) != 0)
            //if (imgMarker != null)
            imgMarker.Attributes["style"] = "display:none";
        else
        {
            PrevRow = 0;
        }
    }

    protected void GV_Vault_RowEditing(object sender, GridViewEditEventArgs e)
    {
        VaultID = Convert.ToInt32(this.GV_Vault.DataKeys[e.NewEditIndex].Value);
        GridViewRow gvr = GV_Vault.Rows[e.NewEditIndex];
        Image imgMarker = (Image)gvr.FindControl("imgMarker");
        imgMarker.Attributes["style"] = "display:";
        BindHTML();
    }

    protected void GV_Vault_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        switch (e.CommandName.ToUpper())
        {
            case "SELECT":
                BindDataV();
                //int id = Convert.ToInt32(e.CommandArgument);
                int id = Convert.ToInt32(e.CommandArgument) - GV_Vault.PageIndex * GV_Vault.PageSize;
                VaultID = Convert.ToInt32(this.GV_Vault.DataKeys[id].Value);
                GridViewRow gvr = GV_Vault.Rows[id];
                Image imgMarker = (Image)gvr.FindControl("imgMarker");
                imgMarker.Attributes["style"] = "display:";
                imgMarker = (Image)GV_Vault.Rows[PrevRow].FindControl("imgMarker");
                imgMarker.Attributes["style"] = "display:none";
                PrevRow = gvr.RowIndex;
                BindHTML();
                break;
        }
    }
    protected void GV_Vault_Sorting(object sender, GridViewSortEventArgs e)
    {
        sortExpV = e.SortExpression;

        if (sortOrderV == "desc")
            sortOrderV = "asc";
        else
            sortOrderV = "desc";
        BindDataV(e.SortExpression, sortOrderV);
        VaultID = Convert.ToInt32(this.GV_Vault.DataKeys[this.GV_Vault.Rows[0].RowIndex].Value);
        BindHTML();
    }

    public void filter()
	{
		BindData();
	}

	public void filter(object o, EventArgs e)
	{
		BindData();
	}

    private bool BindData()
    {
        return BindData(sortExp, sortOrder);
    }
    private bool BindData(string sortExp, string sortDir)
    {
        DataTable dt = getTable(sqlContentSelect);
        DataView DV = dt.DefaultView;
        if (!(sortExp == string.Empty))
        {
            DV.Sort = string.Format("{0} {1}", sortExp, sortDir);
        }
        this.GV_Main.DataSource = DV;
        this.GV_Main.DataBind();

		pager1.ItemCount = DV.Count;
        pager1.Visible = DV.Count > this.GV_Main.PageSize;

        litPagerShowing.Text = CMSHelper.GetPagerInfo(GV_Main, DV.Count);

        return (dt.Rows.Count > 0);
    }

    private bool BindDataV()
    {
        return BindDataV(sortExpV, sortOrderV);
    }
    private bool BindDataV(string sortExp, string sortDir)
    {
        DataTable dt = getTable(sqlVaultSelect, new SqlParameter[]{
            new SqlParameter("@htmlid",HtmlID)});
        if (dt.Rows.Count > 0)
        {
            DataView DV = dt.DefaultView;
            if (!(sortExp == string.Empty))
            {
                DV.Sort = string.Format("{0} {1}", sortExp, sortDir);
            }
            this.GV_Vault.DataSource = DV;
            this.GV_Vault.DataBind();
            VaultID = Convert.ToInt32(dt.Rows[0]["id"]);

            pagerVault.ItemCount = DV.Count;
            pagerVault.Visible = dt.Rows.Count > GV_Vault.PageSize;

            litPagerShowing.Text = CMSHelper.GetPagerInfo(GV_Vault, DV.Count);
        }
        return (dt.Rows.Count > 0);
    }
    private void BindHTML()
    {
        DataTable dt = getTable(sqlVaultSelect, new SqlParameter[]{
            new SqlParameter("@htmlid",HtmlID),
            new SqlParameter("@vaultid",VaultID)});
        litHTML.Text = dt.Rows[0]["HTML"].ToString();
    }
    #endregion GridView

    #region properties
    protected string CurrentView
    {
        set { ViewState["panel"] = value; }
        get
        {
            return (ViewState["panel"] == null ? "Main" : ViewState["panel"].ToString());
        }
    }

    protected int HtmlID
    {
        set { ViewState["HTML"] = value; }
        get { return (ViewState["HTML"] == null ? -1 : Convert.ToInt32(ViewState["HTML"])); }
    }
    protected int VaultID
    {
        set { ViewState["Vault"] = value; }
        get { return (ViewState["Vault"] == null ? -1 : Convert.ToInt32(ViewState["Vault"])); }
    }

    protected int PrevRow
    {
        set { ViewState["Row"] = value; }
        get { return (ViewState["Row"] == null ? 0 : Convert.ToInt32(ViewState["Row"])); }
    }
    #endregion properties
}
