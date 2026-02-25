using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Configuration;

public partial class Admin_Reports_PagesCore : System.Web.UI.UserControl
{
    public bool IsUpdated
    {
        set { ViewState["IsUpdated"] = value; }
        get { return bool.Parse(ViewState["IsUpdated"].ToString()); }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            sortExp = "name"; // default sorted column
            sortOrder = "asc";    // default sort order

            FillLists();
        }
    }

    private void FillLists()
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = "select * from Zones where name in ('Content', 'Banner', 'Footer')";
        DataTable dt = new DataTable();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(dt);

            ddlZone.DataSource = dt;
            ddlZone.DataBind();

            ddlZone.Items.Insert(0, new ListItem("select one", ""));
        }
    }

    public void pager_Command(object sender, CommandEventArgs e)
    {
        int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
        pager1.CurrentIndex = currnetPageIndx;
        GV_Main.PageIndex = currnetPageIndx - 1;

        mBindData(sortExp, sortOrder);
    }

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
    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
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
            //for delete button
            ////ImageButton lb;
            ////lb = (ImageButton)e.Row.FindControl("LB_Delete");
            ////lb.Attributes.Add("OnClick", "return confirm('Are you sure to delete this page?');");

            DataRowView dr = (DataRowView)e.Row.DataItem;
            HtmlAnchor lkSeo = (HtmlAnchor)e.Row.FindControl("lkSeo");
            //lkSeo.HRef = (dr["language"].ToString() == "1" ? "/en/" : "/fr/") + dr["seo"].ToString();
            lkSeo.HRef = (dr["language"].ToString() == "1" ? CMSHelper.SeoPrefixEN : "/fr/") + dr["seo"].ToString();


            ////CheckBox cbxActive = (CheckBox)e.Row.FindControl("cbxActive");
            ////cbxActive.Checked = Convert.ToBoolean(dr["Active"]);

            if (!String.IsNullOrEmpty(dr["LastUpdated"].ToString()))
            {
                Label lblDate = (Label)e.Row.FindControl("lblDate");
                //lblDate.Text = Convert.ToDateTime(dr["LastUpdated"].ToString()).ToShortDateString();
                lblDate.Text = Convert.ToDateTime(dr["LastUpdated"]).ToString("MMM d, yyyy");
            }
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
        pager1.Visible = DV.Count > GV_Main.PageSize;

        litPagerShowing.Text = CMSHelper.GetPagerInfo(GV_Main, DV.Count);
    }

    DateTime endPeriod, startPeriod;

    public DataTable mGet_All_Page_Grid()
    {        
        DataTable dt = new DataTable();

        GetData(dt);

        if (dt.Rows.Count < 1)
        {
            tbl_noresults.Visible = true;
            pnlList.Visible = false;
        }
        else
        {
            tbl_noresults.Visible = false;
            pnlList.Visible = true;
        }
        return dt;
    }

    private void GetData(DataTable dt)
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = "Report.PagesUpdated";

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@isUpdated", IsUpdated ? 1 : 0);

            if (ddlZone.SelectedValue != "")
                cmd.Parameters.AddWithValue("@zoneid", ddlZone.SelectedValue);


            try
            {
                startPeriod = Convert.ToDateTime(txtStartDate.Text);
                //CalendarExtender1.SelectedDate = startPeriod;
                cmd.Parameters.AddWithValue("@date1", startPeriod.ToString("yyyy-MM-dd"));
            }
            catch { }

            try
            {
                endPeriod = Convert.ToDateTime(txtEndDate.Text);
                //CalendarExtender2.SelectedDate = endPeriod;
                cmd.Parameters.AddWithValue("@date2", endPeriod.ToString("yyyy-MM-dd"));
            }
            catch { }

            if (!(bool)Session["Multilingual"])
                cmd.Parameters.AddWithValue("@Lang", 1);

            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(dt);
        }
    }

    protected void btnClearFilterAllApp_Click(object sender, EventArgs e)
    {
        txtStartDate.Text = "";
        txtEndDate.Text = "";
        ddlZone.ClearSelection();

        mBindData(sortExp, sortOrder);
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();

        GetData(dt);

        //if (dt.Rows.Count > 0)
        //{
        DataView myDataView = new DataView();
        myDataView = dt.DefaultView;

        if (ViewState["sortExp"] != null && !string.IsNullOrEmpty(ViewState["sortExp"].ToString()))
        {
            myDataView.Sort = string.Format("{0} {1}", ViewState["sortExp"].ToString(), ViewState["sortOrder"].ToString());
        }

        //gvMain.DataSource = myDataView;

        //}

        string attachment = "attachment; filename=Pages.xls";

        Response.ClearContent();

        Response.AddHeader("content-disposition", attachment);

        Response.ContentType = "application/vnd.ms-excel";

        string tab = "";

        foreach (DataColumn dc in myDataView.ToTable().Columns)
        {
            if (dc.ColumnName == "id" ||
                dc.ColumnName == "name" ||
                dc.ColumnName == "seo" ||
                dc.ColumnName == "Language_Name" ||
                dc.ColumnName == "LastUpdated" ||
                dc.ColumnName == "UserName")
            {
                Response.Write(tab + dc.ColumnName.Replace("Language_Name", "Language"));

                tab = "\t";
            }
        }

        Response.Write("\n");


        int i;

        foreach (DataRow dr in dt.Rows)
        {
            tab = "";

            for (i = 0; i < dt.Columns.Count; i++)
            {
                if (dt.Columns[i].ColumnName == "id" ||
                    dt.Columns[i].ColumnName == "name" ||
                    dt.Columns[i].ColumnName == "seo" ||
                    dt.Columns[i].ColumnName == "Language_Name" ||
                    dt.Columns[i].ColumnName == "LastUpdated" ||
                    dt.Columns[i].ColumnName == "UserName")
                {
                    //Response.Write(tab + "\"" + dr[i].ToString().Replace(Environment.NewLine, " ") + "\"");
                    Response.Write(tab + "\"" + dr[i].ToString().Replace("\"", "''") + "\"");

                    tab = "\t";
                }
            }

            Response.Write("\n");
        }

        Response.End();
    }
    public void PageSizeChange(object o, EventArgs e)
    {
        this.GV_Main.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
        pager1.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
        pager1.CurrentIndex = 1;
        this.GV_Main.PageIndex = 0;
        mBindData(sortExp, sortOrder);
    }
}