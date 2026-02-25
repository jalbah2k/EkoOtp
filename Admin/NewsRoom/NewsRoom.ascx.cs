using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;


public partial class Admin_NewsRoom_NewsRoom : System.Web.UI.UserControl
{
    protected string id;

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

    //public void iconchanged()
    //{
    //    DataSet dt = new DataSet();
    //    SqlDataAdapter da = new SqlDataAdapter("select * from newsroomtypes where id=@id", _connection);
    //    da.SelectCommand.Parameters.AddWithValue("@id", ddlType.SelectedValue);
    //    da.SelectCommand.CommandType = CommandType.Text;
    //    da.Fill(dt);

    //    imgIcon.ImageUrl = "/images/newsroom/" + dt.Tables[0].Rows[0]["icon"].ToString();
    //}

    //public void iconchanged(object o, EventArgs e)
    //{
    //    iconchanged();
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            sortExp = "NewsDate"; // default sorted column
            sortOrder = "desc";    // default sort order

            ProcessRecord("NewsRoom_InsertFromPending");
            BindData();
        }
    }

    private void ProcessRecord(string sql)
    {
        SqlCommand cmd = new SqlCommand(sql, new SqlConnection(_connection));
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Connection.Open();
        cmd.ExecuteNonQuery();
        cmd.Connection.Close();
    }

    private bool BindData()
    {
        return BindData(sortExp, sortOrder);
    }
    private bool BindData(string sortExp, string sortDir)
    {        

        DataTable dt = new DataTable();
        dt = getTable(sqlSelect);

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

        if (DV.Count > 0)
        {
            tbl_grid.Visible = true;
            noResults.Visible = false;
        }
        else
        {
            tbl_grid.Visible = false;
            noResults.Visible = true;
        }

        return (DV.Count > 0);
    }

    #region dal
    protected string _connection = ConfigurationManager.AppSettings.Get("CMServer");
    protected string sqlSelect = "NewsRoomSelectAdmin";
    protected string sqlDelete = "NewsRoomDelete";
    //protected string sqlInsert = "NewsRoomInsert";
    //protected string sqlUpdate = "NewsRoomUpdate";

    private DataTable getTable(string cmd,SqlParameter[] prms)
    {
        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter(cmd, _connection);
        da.SelectCommand.CommandType = CommandType.StoredProcedure;
        da.SelectCommand.Parameters.AddRange(prms);
        da.Fill(dt);
        return dt;
    }
    private DataTable getTable(string cmd, SqlParameter[] prms, CommandType type)
    {
        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter(cmd, _connection);
        da.SelectCommand.CommandType = type;
        da.SelectCommand.Parameters.AddRange(prms);
        da.Fill(dt);
        return dt;
    }

    private DataTable getTable(string cmd)
    {
        return getTable(cmd, new SqlParameter[] {});
    }

    private DataTable getTable(string cmd, SqlParameter prm, CommandType type)
    {
        return getTable(cmd, new SqlParameter[] { prm }, type);
    }

    private DataTable getTable(string cmd, SqlParameter prm)
    {
        return getTable(cmd, new SqlParameter[] { prm});
    }

    private string ProcessRecord(string sql, SqlParameter[] prms)
    {
        SqlCommand cmd = new SqlCommand(sql, new SqlConnection(_connection));
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddRange(prms);
        cmd.Connection.Open();
        string ret = Convert.ToString( cmd.ExecuteScalar());
        cmd.Connection.Close();
        return ret;
    }

    private string ProcessRecord(string sql, List<SqlParameter> prms)
    {
        return ProcessRecord(sql, prms.ToArray());
    }


    private void DeleteRecord()
    {
        ProcessRecord(sqlDelete,new SqlParameter[]{new SqlParameter( "@id", id)});
    }
    #endregion dal

    #region GriedView

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
            if (e.Row.FindControl("lbDelete") != null)
            {
                ((ImageButton)e.Row.FindControl("lbDelete")).
                    Attributes.Add("OnClick", "return confirm('Are you sure to delete this article?');");
            }

            if (e.Row.FindControl("litImgStatus") != null)
            {
                DataRowView dr = (DataRowView)e.Row.DataItem;
                if (dr["Status"].ToString() != "")
                {
                    Literal litImgStatus = (Literal)e.Row.FindControl("litImgStatus");
                    litImgStatus.Text = "<img src='/images/icons/{0}' alt='{1}' title='{2}' />";
                    switch (dr["Status"].ToString().ToUpper())
                    {
                        case "P":
                            litImgStatus.Text = String.Format(litImgStatus.Text, "pending.png", "P", "Pending");
                            break;
                        case "L":
                            litImgStatus.Text = String.Format(litImgStatus.Text, "live.png", "L", "Live");
                            break;
                    }
                }

            }
        }
    }

    protected void GV_Main_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
    protected void GV_Main_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        id = GV_Main.DataKeys[e.RowIndex].Value.ToString();
        DeleteRecord();
        BindData();
    }
    protected void GV_Main_RowEditing(object sender, GridViewEditEventArgs e)
    {

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

    protected void GV_Main_Sorting(object sender, GridViewSortEventArgs e)
    {
        sortExp = e.SortExpression;

        if (sortOrder == "desc")
            sortOrder = "asc";
        else
            sortOrder = "desc";
        BindData(e.SortExpression, sortOrder);
    }
    #endregion GriedView


    public void clickedit(object s, EventArgs e)
    {
        string sql = @"select  n.*,l.name as language
                        from NewsRoom n,Languages l
                        where linkid=@id
                        and l.id=n.lang 
                        order by n.lang ";

        string id = ((ImageButton)s).CommandArgument;
        DataTable dt = getTable(sql, new SqlParameter("@id", id), CommandType.Text);
        //Response.Write(dt.Rows.Count.ToString() + "<br />");

        if (dt.Rows.Count == 0)
        {
            sql = @"select  n.*,l.name as language
                            from NewsRoom n,Languages l
                            where n.id=@id
                            and l.id=n.lang 
                            order by n.lang";
            dt = getTable(sql, new SqlParameter("@id", id), CommandType.Text);
        }

        if (dt.Rows.Count > 0)
        {
            FillPanels(dt);
            pnlList.Visible = false;
            pnlAdd.Visible = true;
        }
    }

    private void FillPanels(DataTable dt)
    {
        if (dt.Rows.Count == 1)
        {
            DataRow dr = dt.Rows[0];
            Session["NewsLinkID"] = dr["linkid"].ToString();
            if (dr["lang"].ToString() == "1")
            {
               // EditPanel2.clickadd();
                EditPanel1.PopulateFields(dr["id"].ToString());
                //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "tabset", "window.location.href = '#tabs-1';", true);
            }
            else
            {
                EditPanel1.clickadd();
            //    EditPanel2.PopulateFields(dr["id"].ToString());
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "tabset", "window.location.href = '#tabs-2';", true);
            }
        }
        else
        {
            Session["NewsLinkID"] = dt.Rows[0]["linkid"].ToString();

            EditPanel1.PopulateFields(dt.Rows[0]["id"].ToString());
     //       EditPanel2.PopulateFields(dt.Rows[1]["id"].ToString());
        }       
    }

   

    public void clickadd(object s, EventArgs e)
    {
        pnlList.Visible = false;
        pnlAdd.Visible = true;

        EditPanel1.clickadd();
       // EditPanel2.clickadd();
    }

    public void clickcancel(object s, EventArgs e)
    {
        EditPanel1.clickcancel();
     //   EditPanel2.clickcancel();

        pnlList.Visible = true;
        pnlAdd.Visible = false;
        BindData();
    }
}

