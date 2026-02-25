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

public partial class Admin_PageShare_PageShareAdmin : System.Web.UI.UserControl
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

    public DataTable mGet_All_PageShare()
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = "select ps.*, p.name, g.name as gname from pageshare ps, Pages p, groups g where ps.Page_id = p.id and g.id in (select group_id from pages_group where page_id = p.id) order by ps.counter desc";
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

    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {

        sb = new StringBuilder(60);
        if (!IsPostBack)
        {
            ViewState["action"] = "add";
            //ViewState["sortExpression"] = "";
            //ViewState["sortD"] = "ASC";
            sortExp = "Counter"; // default sorted column
            sortOrder = "desc";    // default sort order

            mBindData();

            //to be delete
            Session["user_id"] = "-1";
        }

    }

    #region My_Functions

    private void mBindData()
    {
        mBindData(sortExp, sortOrder);
    }
    private void mBindData(string sortExp, string sortDir)
    {
        DataTable dt = new DataTable();
        dt = mGet_All_PageShare();
        DataView DV = dt.DefaultView;
        if (!(sortExp == string.Empty))
        {
            DV.Sort = string.Format("{0} {1}", sortExp, sortDir);
        }
        this.GV_Main.DataSource = DV;
        this.GV_Main.DataBind();

        if (DV.Count < 1)
        {
            tbl_noresults.Visible = true;
            tbl_Grid.Visible = false;
        }
        else
        {
            tbl_noresults.Visible = false;
            tbl_Grid.Visible = true;
        }

        pager1.ItemCount = DV.Count;
        pager1.Visible = dt.Rows.Count > GV_Main.PageSize;

        litPagerShowing.Text = CMSHelper.GetPagerInfo(GV_Main, DV.Count);
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

        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    DataRowView drv = (DataRowView)e.Row.DataItem;

        //    //for delete button
        //    ImageButton lb;
        //    lb = (ImageButton)e.Row.FindControl("LB_Delete");
        //    lb.Attributes.Add("OnClick", "return confirm('Are you sure to delete this Record?');");
        //    if (Convert.ToInt32(drv["id"]) == 1)
        //        lb.Visible = false;
        //}
    }

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




    #endregion
}
