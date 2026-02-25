using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using CuteEditor;

public partial class Admin_PopupMessages_PopupMessages : System.Web.UI.UserControl
{
    SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("CMServer"));

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
        if (!IsPostBack)
        {
            sortExp = "name"; // default sorted column
            sortOrder = "asc";    // default sort order

            Bind();
        }
	    BindEditor(txtContent);

    }

    private void BindEditor(Editor MyEditor)
    {
        //string TemplateItemList = "Paste,PasteText,CssClass,Bold,Italic,Underline,|,JustifyLeft,JustifyCenter,JustifyRight,JustifyFull,|,InsertOrderedList,InsertUnorderedList,|,InsertLink,InsertImage,InsertFlash,InsertMedia,InsertDocument,YouTube,|,RemoveFormat,CleanCode";
        string TemplateItemList = "FormatBlock,Images,Paste,PasteText,|,Undo,Redo,|,Bold,Italic,Underline,|,JustifyLeft,JustifyCenter,JustifyRight,JustifyFull,|,InsertOrderedList,InsertUnorderedList,|,InsertLink,InsertImage,InsertFlash,InsertMedia,YouTube,|,RemoveFormat,CleanCode";
        MyEditor.TemplateItemList = TemplateItemList;
        MyEditor.ResizeMode = CuteEditor.EditorResizeMode.None;
        MyEditor.Height = 500;

        MyEditor.EmptyAlternateText = EmptyAlternateText.ForceAdd;
        MyEditor.EnableStripScriptTags = false;
        MyEditor.EnableStripIframeTags = false;
        MyEditor.BreakElement = BreakElement.P;

        MyEditor.EditorWysiwygModeCss = string.Format(ConfigurationManager.AppSettings.Get("EditorCss") + "?v={0},{1}", ConfigurationManager.AppSettings.Get("CSSVersion"), ConfigurationManager.AppSettings.Get("FontsUrl"));
        MyEditor.PreviewModeCss = string.Format(ConfigurationManager.AppSettings.Get("EditorCss") + "?v={0},{1}", ConfigurationManager.AppSettings.Get("CSSVersion"), ConfigurationManager.AppSettings.Get("FontsUrl"));
        MyEditor.EditorBodyStyle = "background-color:#FFFFFF; background-image:none; width:auto; height:auto;";

        /*CuteEditor.ToolControl toolctrl = MyEditor.ToolControls["CssClass"];

        if (toolctrl != null)
        {
            CuteEditor.RichDropDownList dropdown = (CuteEditor.RichDropDownList)toolctrl.Control;
            //the first item is the caption
            CuteEditor.RichListItem richitem = dropdown.Items[0];
            //clear the items from configuration files
            dropdown.Items.Clear();
            //add the caption
            dropdown.Items.Add(richitem);
            //add value only
			dropdown.Items.Add("<span style='font-family : Arial, Tahoma, Sans Serif;font-size : 25px;color : #000000;	font-weight:bold;'>Header</span>", "Header", "header");
			dropdown.Items.Add("<span style='font-family : Arial, Tahoma, Sans Serif;font-size : 16px;color : #026ab3;	font-weight:bold;'>Title</span>", "Title", "title");
			dropdown.Items.Add("<span style='font-family:Arial, Tahoma, Sans Serif;	font-size:13px;	color:#292929;	line-height:20px;'>Main Body</span>", "Main Body", "bodytext");
			dropdown.Items.Add("<span style='color: #adadad; font-family: Arial,Helvetica,sans-serif; font-size: 26px; text-decoration: none; text-transform: uppercase; font-weight: bold;'>Banner</span>", "Banner", "banner");

            //add html and text and value
            //dropdown.Items.Add("<span class='BoldGreen'>Bold      Green Text</span>","Bold      Green Text","BoldGreen");
        }*/

        CuteEditor.ToolControl ronster = MyEditor.ToolControls["Images"];
        if (ronster != null)
        {
            CuteEditor.RichDropDownList dd = (CuteEditor.RichDropDownList)ronster.Control;
            CuteEditor.RichListItem rich = dd.Items[0];
            dd.Items.Clear();
            rich.Text = "Template";
            rich.Html = "Template";
            dd.Items.Add(rich);
            dd.Items.Add("Callout Box", "", "<div class=\"callout-box\"><h3>This is the callout headline</h3><p>Quisque egestas fermentum mi, id placerat lacus sodales quis. Suspendisse potenti. Ut gravida pellentesque leo, et viverra risus iaculis vel. Aliquam erat volutpat. </p></div>");
            dd.Items.Add("Callout Box - Check", "", "<div class=\"callout-box check\"><h3>This is the callout headline</h3><p>Quisque egestas fermentum mi, id placerat lacus sodales quis. Suspendisse potenti. Ut gravida pellentesque leo, et viverra risus iaculis vel. Aliquam erat volutpat. </p></div>");
            dd.Items.Add("Callout Box - Bell", "", "<div class=\"callout-box bell\"><h3>This is the callout headline</h3><p>Quisque egestas fermentum mi, id placerat lacus sodales quis. Suspendisse potenti. Ut gravida pellentesque leo, et viverra risus iaculis vel. Aliquam erat volutpat. </p></div>");
            dd.Items.Add("Unordered List in Two Columns", "", "<ul class='two-columns'><li>one</li><li>two</li><li>three</li><li>four</li></ul>");
            dd.Items.Add("Button", "", "<div><span class=\"button\">This is a button!</span></div>");
        }

        CuteEditor.ToolControl toolctrl = MyEditor.ToolControls["FormatBlock"];
        if (toolctrl != null)
        {
            CuteEditor.RichDropDownList dropdown = (CuteEditor.RichDropDownList)toolctrl.Control;
            //the first item is the caption     
            CuteEditor.RichListItem richitem = dropdown.Items[0];
            //clear the items from configuration files     
            dropdown.Items.Clear();
            //add the caption    
            dropdown.Items.Add(richitem);
            //add text and value     
            dropdown.Items.Add(String.Format("<span style=\"{0}\">Bodytext</span>", ConfigurationManager.AppSettings.Get("EditorDropdownBody")), "Bodytext", "<p>");
            dropdown.Items.Add(String.Format("<span style=\"{0}\">Header</span>", ConfigurationManager.AppSettings.Get("EditorDropdownH1")), "Header", "<h1>");
            dropdown.Items.Add(String.Format("<span style=\"{0}\">Title</span>", ConfigurationManager.AppSettings.Get("EditorDropdownH2")), "Title", "<h2>");
            dropdown.Items.Add(String.Format("<span style=\"{0}\">Subtitle</span>", ConfigurationManager.AppSettings.Get("EditorDropdownH3")), "Subtitle", "<h3>");

        }

    }

    private void Bind()
    {
        Bind(sortExp, sortOrder);
    }

    private void Bind(string sortExp, string sortDir)
    {
        SqlDataAdapter dapt = new SqlDataAdapter("PopupMessagesSelect", conn);
        dapt.SelectCommand.CommandType = CommandType.StoredProcedure;

        if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() != "")
            dapt.SelectCommand.Parameters.AddWithValue("@id", Request.QueryString["id"].ToString());

        if (txtFilterText.Text != "")
            dapt.SelectCommand.Parameters.AddWithValue("@Filter", txtFilterText.Text.Trim());

        DataTable dt = new DataTable();
        dapt.Fill(dt);

        DataView myDataView = new DataView();
        myDataView = dt.DefaultView;

        if (sortExp != string.Empty)
        {
            myDataView.Sort = string.Format("{0} {1}", sortExp, sortDir);
        }

        gvMain.DataSource = myDataView;
        gvMain.DataBind();

        pager1.ItemCount = myDataView.Count;
        pager1.Visible = myDataView.Count > gvMain.PageSize;

        litPagerShowing.Text = CMSHelper.GetPagerInfo(gvMain, myDataView.Count);

        if (myDataView.Count > 0)
        {
            gridarea.Visible = true;
            noMain.Visible = false;
        }
        else
        {
            gridarea.Visible = false;
            noMain.Visible = true;
        }
    }

    /*private void BindEverything()
    {
        SqlDataAdapter dapt = new SqlDataAdapter("CAMP_MemberSelect", conn);
        dapt.SelectCommand.CommandType = CommandType.StoredProcedure;
        DataTable dt = new DataTable();
        dapt.Fill(dt);

    }*/

    /*public void export(object s, EventArgs e)
    {
        SqlDataAdapter dapt = new SqlDataAdapter("PopupMessagesSelect", conn);
        dapt.SelectCommand.CommandType = CommandType.StoredProcedure;

        if (txtFilterText.Text != "")
            dapt.SelectCommand.Parameters.AddWithValue("@Filter", txtFilterText.Text.Trim());

        DataTable dt = new DataTable();
        dapt.Fill(dt);

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

        string attachment = "attachment; filename=PopupMessagesExport.xls";

        Response.ClearContent();

        Response.AddHeader("content-disposition", attachment);

        Response.ContentType = "application/vnd.ms-excel";

        string tab = "";

        foreach (DataColumn dc in myDataView.ToTable().Columns)
        {
            Response.Write(tab + dc.ColumnName);

            tab = "\t";
        }

        Response.Write("\n");


        int i;

        foreach (DataRow dr in dt.Rows)
        {
            tab = "";

            for (i = 0; i < dt.Columns.Count; i++)
            {
                //Response.Write(tab + "\"" + dr[i].ToString().Replace(Environment.NewLine, " ") + "\"");
                Response.Write(tab + "\"" + dr[i].ToString().Replace("\"", "''") + "\"");

                tab = "\t";
            }

            Response.Write("\n");
        }

        Response.End();

    }*/

    public void rowcommand(object o, CommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "deletepopup":
                DeleteRow(e.CommandArgument.ToString());
                break;
            case "editpopup":
                Edit(e.CommandArgument.ToString());
                break;
            default:
                break;
        }
    }

    public void DeleteRow(string id)
    {
        SqlCommand command = new SqlCommand("PopupMessagesDelete", conn);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.AddWithValue("@id", id);

        conn.Open();
        command.ExecuteNonQuery();
        conn.Close();

        Bind();
    }

    private void Edit(string id)
    {
        pnlEdit.Visible = true;
        pnlList.Visible = false;

        SqlDataAdapter dapt = new SqlDataAdapter("PopupMessagesSelect", conn);
        dapt.SelectCommand.CommandType = CommandType.StoredProcedure;
        dapt.SelectCommand.Parameters.AddWithValue("@id", id);
        DataTable dt = new DataTable();
        dapt.Fill(dt);

        txtName.Text = dt.Rows[0]["name"].ToString();
        txtContent.Text = dt.Rows[0]["html"].ToString();
        cbFTO.Checked = Convert.ToBoolean(dt.Rows[0]["firsttimeonly"]);

        ibSave.CommandName = "save";
        ibSave.CommandArgument = id;
    }

    public void Add(object o, EventArgs e)
    {
        pnlEdit.Visible = true;
        pnlList.Visible = false;
        ibSave.CommandName = "add";

        txtName.Text = "";
        txtContent.Text = "";
        cbFTO.Checked = false;
    }

    public void SAVE(object o, CommandEventArgs e)
    {
        pnlEdit.Visible = false;
        pnlList.Visible = true;

        SqlCommand command = new SqlCommand();

        if (e.CommandName == "save")
        {
            command = new SqlCommand("PopupMessagesUpdate", conn);
            command.Parameters.AddWithValue("@id", e.CommandArgument.ToString());
        }
        else
        {
            command = new SqlCommand("PopupMessagesInsert", conn);
        }

        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.AddWithValue("@name", txtName.Text.Trim());
        command.Parameters.AddWithValue("@html", txtContent.Text.Trim());
        command.Parameters.AddWithValue("@firsttimeonly", cbFTO.Checked ? 1 :0);

        conn.Open();
        command.ExecuteNonQuery();
        conn.Close();


        Bind();
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
            //DataRowView dr = e.Row.DataItem as DataRowView;

            ImageButton ImageButton2 = (ImageButton)e.Row.FindControl("ImageButton2");
            ImageButton2.Attributes.Add("onclick", "return confirm('Are you sure want to delete this item?')");
        }
    }
    
    protected void ibCancel_Click(object sender, EventArgs e)
    {
        pnlEdit.Visible = false;
        pnlList.Visible = true;

        Bind();
    }

    public void dosort(object o, GridViewSortEventArgs e)
    {
        sortExp = e.SortExpression;

        if (sortOrder == "desc")
            sortOrder = "asc";
        else
            sortOrder = "desc";
        Bind(e.SortExpression, sortOrder);
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

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        Bind();
    }
    protected void btnClearFilter_Click(object sender, EventArgs e)
    {
        /*txtFilterStartDate.Text = "";
        txtFilterEndDate.Text = "";
        txtFilterText.Text = "";
        ddlFilterVacancyType.ClearSelection();

        Bind();*/
        Response.Redirect("/admin.aspx?c=popup");
    }
}