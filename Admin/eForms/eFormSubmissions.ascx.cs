using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

public partial class Admin_eForms_eFormSubmisions : System.Web.UI.UserControl
{
    DataTable dt;

    //private string submissionID;
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


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            sortExp = "SubmitDate"; // default sorted column
            sortOrder = "desc";    // default sort order

            if (Session["ViewSubmission"] != null)
            {
                //string submissionid = Session["ViewSubmission"].ToString();
                //BindSubmission(submissionid);
                //Session.Remove("ViewSubmission");
            }
        }
    }

    public void export(object o, EventArgs e)
    {
        ////SqlDataAdapter dapt = new SqlDataAdapter("eformexport2", ConfigurationManager.AppSettings.Get("CMServer"));
        SqlDataAdapter dapt = new SqlDataAdapter("eformexport", ConfigurationManager.AppSettings.Get("CMServer"));
        dapt.SelectCommand.CommandType = CommandType.StoredProcedure;
        dapt.SelectCommand.Parameters.AddWithValue("@ID", formID);

        if (txtFilterStartDate.Text.Trim() != "")
            dapt.SelectCommand.Parameters.AddWithValue("@datefrom", Convert.ToDateTime(txtFilterStartDate.Text.Trim()).Date);
        if (txtFilterEndDate.Text.Trim() != "")
            dapt.SelectCommand.Parameters.AddWithValue("@dateto", Convert.ToDateTime(txtFilterEndDate.Text.Trim()).Date);

        DataSet ds = new DataSet();
        dapt.Fill(ds);

        DataTable dt = ds.Tables[0];

        string attachment = "attachment; filename=Export.xls";

        Response.ClearContent();
        Response.Clear();

        Response.AddHeader("content-disposition", attachment);

        //Response.ContentType = "application/excel";
        Response.ContentType = "application/vnd.ms-excel";
        Response.Charset = "utf-8";

        // to show extended characters properly
        //Response.ContentEncoding = System.Text.Encoding.UTF8;
        //Response.BinaryWrite(System.Text.Encoding.UTF8.GetPreamble());
        Response.ContentEncoding = System.Text.Encoding.Default;

        string tab = "";

        //foreach (DataColumn dc in dt.Columns)
        //{
        //    Response.Write(tab + dc.ColumnName);

        //    tab = "\t";
        //}

        //Response.Write("\n");


        int i;
        string regex = @"<#(.*?)#>";
        string fieldType = string.Empty;

        for (i = 0; i < dt.Columns.Count; i++)
        {
            //Response.Write(tab + dt.Columns[i].ColumnName);
            //// Response.Write(tab + Regex.Replace(dt.Columns[i].ColumnName, regex, ""));
            Response.Write(tab + Regex.Replace(dt.Columns[i].ColumnName.Replace("\t", ""), regex, ""));

            tab = "\t";
        }
        Response.Write("\n");

        foreach (DataRow dr in dt.Rows)
        {
            tab = "";

            for (i = 0; i < dt.Columns.Count; i++)
            {
                fieldType = Regex.Match(dt.Columns[i].ColumnName, regex).Groups[1].Value.ToLower();
                if (fieldType == "browse")
                {
                    if (dr[i] != DBNull.Value && dr[i].ToString().Length > 0)
                        Response.Write(tab + string.Format("=HYPERLINK(\"{0}://{1}/data/eFormsUploads/{2}/{3}\", \"{3}\")", Request.Url.Scheme, Request.Url.Host, formID, dr[i].ToString().Replace("\"", "“").Replace("\t", " ")));
                    else
                        Response.Write(tab);
                }
                else if (fieldType == "email")
                {
                    if (dr[i] != DBNull.Value && dr[i].ToString().Length > 0)
                        Response.Write(tab + string.Format("=HYPERLINK(\"mailto:{0}\", \"{0}\")", dr[i].ToString().Replace("\"", "“").Replace("\t", " ")));
                    else
                        Response.Write(tab);
                }
                else if (fieldType == "website")
                {
                    if (dr[i] != DBNull.Value && dr[i].ToString().Length > 0)
                        Response.Write(tab + string.Format("=HYPERLINK(\"{0}\", \"{0}\")", dr[i].ToString().Replace("\"", "“").Replace("\t", " ")));
                    else
                        Response.Write(tab);
                }
                else
                    Response.Write(tab + "\"" + dr[i].ToString().Replace("\"", "“").Replace("\t", " ") + "\"");

                tab = "\t";
            }

            Response.Write("\n");
        }

        Response.End();
    }

    public void InitME(Hashtable ef)
    {
        this.ef = ef;

        lbFormCaption.Text = formName;
        BindData();
        if (gvSubmissions.Rows.Count > 0)
        {
            gvSubmissions.Rows[0].RowState = DataControlRowState.Selected;
            string submissionid = gvSubmissions.DataKeys[0].Value.ToString();
            BindSubmission(submissionid);
            pnlSubmission.Visible = true;
        }
        else if (gvArchive.Rows.Count > 0)
        {
            gvArchive.Rows[0].RowState = DataControlRowState.Selected;
            string submissionid = gvArchive.DataKeys[0].Value.ToString();
            BindSubmission(submissionid);
            pnlSubmission.Visible = true;
        }
        else
            pnlSubmission.Visible = false;
    }

    private void BindData()
    {
        BindData(sortExp, sortOrder);
    }
    protected void BindData(string sortExp, string sortDir)
    {
        Response.Write(formID);

        SqlParameter[] prms = { new SqlParameter("@id", Convert.ToInt32(formID)) };
        dt = eDAL.getTable(sqlSelect, prms);
        DataView dv = new DataView(dt);

        if (sortExp != string.Empty)
        {
            dv.Sort = string.Format("{0} {1}", sortExp, sortDir);
        }

        dv.RowFilter = "archive=0";
        this.gvSubmissions.DataSource = dv;
        this.gvSubmissions.DataBind();

        pager1.ItemCount = dv.Count;
        pager1.Visible = dv.Count > gvSubmissions.PageSize;

        litPagerShowing.Text = CMSHelper.GetPagerInfo(gvSubmissions, dv.Count);

        pnlSubmissions.Visible = dv.Count > 0;
        pnlNoSubmissions.Visible = dv.Count == 0;

        dv.RowFilter = "archive=1";
        this.gvArchive.DataSource = dv;
        this.gvArchive.DataBind();

        pagerArc.ItemCount = dv.Count;
        pagerArc.Visible = dv.Count > gvArchive.PageSize;

        litPagerShowingArc.Text = CMSHelper.GetPagerInfo(gvArchive, dv.Count);

        pnlArchive.Visible = dv.Count > 0;
        pnlNoArchive.Visible = dv.Count == 0;
    }
    protected string test;
    private void BindSubmission(string submissionID)
    {
        SqlParameter[] prms = { new SqlParameter("@id", Convert.ToInt32(submissionID)) };
        DataTable dt = eDAL.getTable(sqlSelectSubmission, prms);
        gvSubmission.DataSource = dt;
        gvSubmission.DataBind();
    }

    private string sqlSelect = "eFormSubmissionsSelect";
    private string sqlSelectSubmission = "eFormSubmissionSelect";
    private string sqlUpdateSubmission = "eFormSubmissionUpdate";
    private string sqlDeleteSubmission = "eFormSubmissionDelete";

    private Hashtable ef
    {
        set { ViewState["ef"] = value; }
        get { return (Hashtable)ViewState["ef"]; }
    }

    protected string formID
    {
        get { return Convert.ToString(ef["formid"]); }
    }

    protected string formName
    {
        get { return Convert.ToString(ef["name"]); }
    }

	protected string truncateCaption(object obj){
		int limit=9;
		string caption =Convert.ToString(obj);
		if(!string.IsNullOrEmpty(caption) && caption.Length>limit)
			caption=caption.Remove(limit)+"...";
		return caption;
	}

    public void PageSizeChange(object o, EventArgs e)
    {
        gvSubmissions.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
        pager1.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
        pager1.CurrentIndex = 1;
        gvSubmissions.PageIndex = 0;

        BindData();
    }
    public void pager_Command(object sender, CommandEventArgs e)
    {
        int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
        pager1.CurrentIndex = currnetPageIndx;
        gvSubmissions.PageIndex = currnetPageIndx - 1;

        BindData();
    }
    public void PageSizeChangeArc(object o, EventArgs e)
    {
        gvArchive.PageSize = Convert.ToInt32(ddlPageSizeArc.SelectedValue);
        pagerArc.PageSize = Convert.ToInt32(ddlPageSizeArc.SelectedValue);
        pagerArc.CurrentIndex = 1;
        gvArchive.PageIndex = 0;

        BindData();
    }
    public void pagerArc_Command(object sender, CommandEventArgs e)
    {
        int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
        pagerArc.CurrentIndex = currnetPageIndx;
        gvArchive.PageIndex = currnetPageIndx - 1;

        BindData();
    }
    protected void gvSubmissions_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        #region Add sorted class to headers
        if (e.Row.RowType == DataControlRowType.Header)
        {
            int colIndex = CMSHelper.GetColumnIndex((sender as GridView), sortExp);
            e.Row.Cells[colIndex].CssClass = sortOrder == "asc" ? "sortasc" : "sortdesc";
        }
        #endregion Add sorted class to headers

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            object obj = e.Row.FindControl("lbDelete");
            if (obj != null)
                ((ImageButton)obj).Attributes.Add("onclick", "return confirm('Are you really want to delete this submission?')");
        }
    }
    protected void gvSubmissions_Sorting(object sender, GridViewSortEventArgs e)
    {
        sortExp = e.SortExpression;

        if (sortOrder == "desc")
            sortOrder = "asc";
        else
            sortOrder = "desc";
        BindData(e.SortExpression, sortOrder);
    }
    protected void gvSubmissions_RowEditing(object sender, GridViewEditEventArgs e)
    {
        //submissionID = gvSubmissions.DataKeys[e.NewEditIndex].Value.ToString();
        //BindData2();
    }

    protected void gvSubmissions_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandArgument != null && e.CommandArgument.ToString().Length > 0)
        {
            string submissionID = e.CommandArgument.ToString();
            switch (e.CommandName)
            {
                case "Select":
                    switch (((GridView)sender).ID)
                    {
                        case "gvArchive":
                            this.gvSubmissions.SelectedIndex = -1;
                            break;
                        case "gvSubmissions":
                            this.gvArchive.SelectedIndex = -1;
                            break;
                    }
                    BindSubmission(submissionID);
                    break;
                case "archive":
                    updateSubmission(submissionID, true);
                    this.BindData();
                    break;
                case "unarchive":
                    updateSubmission(submissionID, false);
                    this.BindData();
                    break;
            }
        }
    }

    protected void gvSubmissions_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string id = ((GridView)sender).DataKeys[e.RowIndex].Value.ToString();
        SqlParameter[] prms = { new SqlParameter("@id", id) };
        eDAL.ProcessRecord(sqlDeleteSubmission, prms);
        BindData();
    }

    private void updateSubmission(string submissionid, bool archive)
    {
        SqlParameter[] prms = { new SqlParameter("@id", submissionid), new SqlParameter("@archive", archive) };
        eDAL.ProcessRecord(sqlUpdateSubmission, prms);
    }

    protected void imgBack_Click(object sender, ImageClickEventArgs e)
    {
        //Session["CurrentView"] = "Submission";
    }

    protected void btnClearFilter_Click(object sender, EventArgs e)
    {
        txtFilterStartDate.Text = "";
        txtFilterEndDate.Text = "";
    }
}
