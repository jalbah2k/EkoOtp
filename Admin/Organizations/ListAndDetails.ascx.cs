using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CuteEditor;

public partial class Admin_MemberDirectory_ListAndDetails : System.Web.UI.UserControl
{
    #region Properties
    protected string OrganizationId
    {
        get
        {
            return ViewState["OrganizationId"] != null ? ViewState["OrganizationId"].ToString() : "0";
        }
        set
        {
            ViewState["OrganizationId"] = value;
        }
    }

    protected string sortExp
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
    protected string sortOrder
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
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindData();
        }
    }

    #region Grid Panel
    private void BindData(string SortExpresion, string SortOrder)
    {
        sortExp = SortExpresion;
        sortOrder = SortOrder;
        BindData();

    }
    private void BindData()
    {
        DataTable dt = new DataTable();

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("CMServer")))
        {
            SqlDataAdapter dapt = new SqlDataAdapter("select * from Organizations", conn);
            dapt.SelectCommand.CommandType = CommandType.Text;
            dapt.Fill(dt);
        }

        DataView DV = dt.DefaultView;
        if (!(sortExp == string.Empty))
        {
            DV.Sort = string.Format("{0} {1}", sortExp, sortOrder);
        }

        if(txtFilter.Text != "")
        {
            DV.RowFilter = "name like '%" + txtFilter.Text + "%'";
        }

        this.GV_Main.DataSource = DV;
        this.GV_Main.DataBind();

        pager1.ItemCount = DV.Count;
        pager1.Visible = dt.Rows.Count > GV_Main.PageSize;

    }

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        BindData();
    }
    protected void cbxFeatureFilter_CheckedChanged(object sender, EventArgs e)
    {
        BindData();
    }

    protected void btnClearFilter_Click(object sender, EventArgs e)
    {
        txtFilter.Text = "";
        BindData();
    }
    protected void bttn_Add_Click(object sender, EventArgs e)
    {
        ClearFields();
        SetView("pnlDetails");
        btnDone.Visible = btnDone2.Visible = false;

    }

    protected void GV_Main_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton lb;
            lb = (ImageButton)e.Row.FindControl("LB_Delete");
            lb.Attributes.Add("OnClick", "return confirm('Are you sure to delete this Organization?');");

            DataRowView dr = (DataRowView)e.Row.DataItem;

            CheckBox cbxActive = (CheckBox)e.Row.FindControl("cbxActive");
            cbxActive.Checked = Convert.ToBoolean(dr["active"]);

            CheckBox cbAffiliate = (CheckBox)e.Row.FindControl("cbxAffiliate");
            cbAffiliate.Checked = Convert.ToBoolean(dr["AffiliateMember"]);

        }
    }

    protected void GV_Main_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    protected void GV_Main_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        switch (e.CommandName.ToLower())
        {
            case "delete":
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("CMServer")))
                {
                    SqlCommand command = new SqlCommand("delete from Organizations where id=@id", conn);
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id", e.CommandArgument.ToString());

                    conn.Open();
                    try { command.ExecuteScalar(); }
                    catch (Exception ex)
                    {
                        Page.ClientScript.RegisterClientScriptBlock(GetType(), "warning", "alert('" + ex.Message + "');", true);
                    }
                    conn.Close();
                }
                BindData();
                break;

            case "editing":
                ClearFields();
                OrganizationId = e.CommandArgument.ToString();
                PopulateDetails(OrganizationId);
                break;

            default:
                break;
        }
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

    protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.GV_Main.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
        pager1.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
        pager1.CurrentIndex = 1;
        this.GV_Main.PageIndex = 0;
        BindData();
    }

    protected void pager1_Command(object sender, CommandEventArgs e)
    {
        int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
        pager1.CurrentIndex = currnetPageIndx;
        GV_Main.PageIndex = currnetPageIndx - 1;

        BindData();
    }
    #endregion

    #region Details Panel
    private void PopulateDetails(string id)
    {
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("CMServer")))
        {
            SqlDataAdapter dapt = new SqlDataAdapter("select * from Organizations where id=@id", conn);
            dapt.SelectCommand.CommandType = CommandType.Text;
            dapt.SelectCommand.Parameters.Add(new SqlParameter("@id", id));

            DataSet ds = new DataSet();
            dapt.Fill(ds);
            DataTable dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];

                tbTitle.Text = dr["Name"].ToString();
                cbActive.Checked = Convert.ToBoolean(dr["active"]);
                cbAffiliate.Checked = Convert.ToBoolean(dr["AffiliateMember"]);

                btnDone.Visible = btnDone2.Visible = true;

                SetView("pnlDetails");
            }
        }

    }

    private void ClearFields()
    {
        OrganizationId = "0";

        tbTitle.Text = "";
        cbActive.Checked = false;
        cbAffiliate.Checked = false;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string sql = "";

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("CMServer")))
        {
            List<SqlParameter> prms = new List<SqlParameter>();
            if (OrganizationId == "0")
            {
                //Insert
                sql = "insert into Organizations (name, active, AffiliateMember) values(@name, @active, @affiliate) select SCOPE_IDENTITY()";
            }
            else
            {
                //Edit
                sql = "update Organizations set name=@name, active=@active, AffiliateMember=@affiliate where id=@id select @id";
                prms.Add(new SqlParameter("@id", OrganizationId));
            }

            //Set SQL Parameters:
            #region SQL Parameters
            prms.Add(new SqlParameter("@name", tbTitle.Text));
            prms.Add(new SqlParameter("@active", cbActive.Checked ? 1 : 0));
            prms.Add(new SqlParameter("@affiliate", cbAffiliate.Checked ? 1 : 0));

            #endregion


            //Execute SP
            try
            {
                object ret = ExecuteQuery(sql, prms.ToArray(), conn, CommandType.Text);
                OrganizationId = ret.ToString();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Cannot insert duplicate key row in object") && ex.Message.Contains("IX_Organizations_2"))
                {
                    Page.ClientScript.RegisterClientScriptBlock(GetType(), "warning_seo", "alert(\"This Organization is already in use. Please enter a different name.\");", true);
                }
                else
                    throw new Exception(ex.Message);
            }
          
            if (sql.Contains("insert"))
            {
                ClearFields();
                SetView("pnlList");
                BindData();
            }
            else
            {
                //Reload the form:
                PopulateDetails(OrganizationId);
            }
        }
    }


    protected void btnBackToList_Click(object sender, EventArgs e)
    {
        ClearFields();
        SetView("pnlList");
    }

    protected void btnDone_Click(object sender, EventArgs e)
    {

        ClearFields();
        SetView("pnlList");
        BindData();
    }
    #endregion

    #region Set View
    protected string CurrentView
    {
        set { ViewState["currentview"] = value; }
        get { return ViewState["currentview"] == null ? pnlList.ID : ViewState["currentview"].ToString(); }
    }

    private void SetView(string view)
    {
        CurrentView = view;
        SetView();
    }

    private void SetView()
    {
        IsViewable(pnlList);
        IsViewable(pnlDetails);

    }

    private bool IsViewable(Panel pnl)
    {
        return pnl.Visible = pnl.ID == CurrentView;
    }

    #endregion View

    #region Auxiliar
    private string con = ConfigurationManager.AppSettings.Get("CMServer");
    private DataTable getTable(string cmd, SqlParameter[] param)
    {
        SqlDataAdapter da = new SqlDataAdapter(cmd, con);
        da.SelectCommand.CommandType = CommandType.StoredProcedure;
        da.SelectCommand.Parameters.AddRange(param);
        DataTable dt = new DataTable();
        da.Fill(dt);
        return dt;
    }

    public object ExecuteQuery(string sql, SqlParameter[] prms, SqlConnection conn, CommandType cmdType)
    {
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.CommandType = cmdType;
        cmd.Parameters.AddRange(prms);
        cmd.Connection.Open();
        object ret = cmd.ExecuteScalar();
        cmd.Connection.Close();
        return ret;
    }
    public void ExecuteNoQuery(string sql, SqlParameter[] prms, SqlConnection conn, CommandType cmdType)
    {
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.CommandType = cmdType;
        cmd.Parameters.AddRange(prms);
        cmd.Connection.Open();
        cmd.ExecuteNonQuery();
        cmd.Connection.Close();
    }
    #endregion

}