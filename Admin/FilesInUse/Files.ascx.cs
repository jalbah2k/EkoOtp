using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_FilesInUse_Files : System.Web.UI.UserControl
{
    //StringBuilder sb = new StringBuilder();
    SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("CMServer"));
    DataTable dt = new DataTable();

    string rootfolder = "uploads";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            sortExp = "FileName"; // default sorted column
            sortOrder = "asc";    // default sort order

            if (ibBack.Visible = Request.QueryString["root"] != null)
                rootfolder = Request.QueryString["root"];

            ViewState["rootfolder"] = rootfolder;
            Bind();
        }
    }


    private void Bind()
    {
        Bind(sortExp, sortOrder);
    }

    private void Bind(string sortExp, string sortDir)
    {
        SqlDataAdapter dapt = new SqlDataAdapter("select '' as FileName, '' as Path, 0 as Used, 0 as Folder", conn);
        dapt.SelectCommand.CommandType = CommandType.Text;
        dapt.Fill(dt);

        string root = Server.MapPath("~/" + ViewState["rootfolder"].ToString() + "/");
        GetFiles(root);
        GetFolders(root);

        DataRow dr = dt.Rows[0];
        dt.Rows.Remove(dr);

        DataView DV = dt.DefaultView;
        if (!(sortExp == string.Empty))
        {
            DV.Sort = string.Format("{0} {1}", sortExp, sortDir);
        }

        if (ddlFilter.SelectedValue == "0")
            DV.RowFilter = "Used = 0";

        gvMain.DataSource = DV;
        gvMain.DataBind();

        pager1.ItemCount = DV.Count;
        pager1.Visible = DV.Count > gvMain.PageSize;

        litPagerShowing.Text = CMSHelper.GetPagerInfo(gvMain, DV.Count);
    }

    private void GetFolders(string dir)
    {
       // sb.Append("<ul><li>");

        string[] directories = Directory.GetDirectories(dir);

        foreach (string s in directories)
        {
            //string stemp = "\\" + rootfolder + "\\";
            //sb.Append("<br /><strong>" + s.Substring(s.IndexOf(stemp) + stemp.Length) + "</strong></li>");

           // GetFolders(s);
            //GetFiles(s);


            DataRow dr = dt.NewRow();

            string[] f = Directory.GetFiles(s);

            string parm = s.ToString().Replace(Request.MapPath("~/"), "").Replace("\\", "/");
            dr[0] = "<a href='" + Request.ServerVariables["URL"] + "?c=files&root=" + parm +"'>" + parm + "</a>";
            dr[1] = f.Length.ToString() + " file(s)";
            dr[2] = false;
            dr[3] = true;

            // Add and return the new row.
            dt.Rows.Add(dr);
        }

        //sb.Append("</ul>");
    }

    private void GetFiles(string dir)
    {
       // sb.Append("<ul>");
        string[] files = Directory.GetFiles(dir);

        foreach (string s in files)
        {
            string filename = s.Substring(s.LastIndexOf("\\") + 1); ;

            string localDir = Request.MapPath("~/");
            int len = localDir.Length;

            DataTable dtHtml = new DataTable();
            SqlDataAdapter dapt = new SqlDataAdapter("select id from html where html like '%' + @filename + '%'  union select BannerID from Banners where BannerName = @filename ", conn);
            dapt.SelectCommand.CommandType = CommandType.Text;
            dapt.SelectCommand.Parameters.AddWithValue("@filename", filename);
            dapt.Fill(dtHtml);

            DataRow dr = dt.NewRow();

            dr[0] = filename;
            dr[1] = dir.Substring(len - 1).Replace("\\", "/");;
            dr[2] = dtHtml.Rows.Count > 0;
            dr[3] = false;

            // Add and return the new row.
            dt.Rows.Add(dr);

          //  sb.Append("<li>" + s.Substring(s.LastIndexOf("\\") + 1) + "</li>");
        }
     //   sb.Append("</ul>");
    }

    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
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
            DataRowView dr = (DataRowView)e.Row.DataItem;
           
            CheckBox cbxActive = (CheckBox)e.Row.FindControl("cbxActive");
            cbxActive.Checked = Convert.ToBoolean(dr["Used"]);

            cbxActive.Visible = !Convert.ToBoolean(dr["folder"]);


            Literal litName = (Literal)e.Row.FindControl("litName");
            litName.Text = dr["FileName"].ToString();    
        }
    }
    protected void GV_Main_Sorting(object sender, GridViewSortEventArgs e)
    {
        sortExp = e.SortExpression;

        if (sortOrder == "desc")
            sortOrder = "asc";
        else
            sortOrder = "desc";
        Bind(e.SortExpression, sortOrder);
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

    public void PageSizeChange(object o, EventArgs e)
    {
        gvMain.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
        pager1.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
        pager1.CurrentIndex = 1;
        gvMain.PageIndex = 0;

        Bind();
    }

    public void pager_Command(object sender, CommandEventArgs e)
    {
        int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
        pager1.CurrentIndex = currnetPageIndx;
        gvMain.PageIndex = currnetPageIndx - 1;

        Bind();
    }

    protected void btnFilter_Click(object sender, ImageClickEventArgs e)
    {
        Bind();
    }
    protected void btnClearFilter_Click(object sender, ImageClickEventArgs e)
    {        
        Response.Redirect("/admin.aspx?c=files");
    }
    protected void ddlFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        Bind();
    }

    protected void ibDownload_Click(object sender, EventArgs e)
    {
        Bind();
        DataRow[] drs = dt.Select("");

        if (ddlFilter.SelectedValue == "0")
        {
            drs = dt.Select("Used = 0");
        }
       
        string attachment = "attachment; filename=Export.xls";

        Response.ClearContent();

        Response.AddHeader("content-disposition", attachment);

        Response.ContentType = "application/excel";
        Response.Charset = "utf-8";

        string tab = "";

     

        int i;

        for (i = 0; i < dt.Columns.Count; i++)
        {
            Response.Write(tab + "\"" + dt.Columns[i].ColumnName + "\"");

            tab = "\t";
        }
        Response.Write("\n");

        foreach (DataRow dr in drs)
        {
            tab = "";

            for (i = 0; i < dt.Columns.Count; i++)
            {
                Response.Write(tab + "\"" + dr[i].ToString().Replace("\"", "'") + "\"");

                tab = "\t";
            }

            Response.Write("\n");
        }

        Response.End();
    }

    protected void ibBack_Click(object sender, EventArgs e)
    {
        string s = Request.QueryString["root"];

        int npos = s.LastIndexOf("/");
        if (npos > -1)
        {
            s = s.Remove(npos);
            Response.Redirect(Request.ServerVariables["URL"] + "?c=files&root=" + s);
        }

    }
}