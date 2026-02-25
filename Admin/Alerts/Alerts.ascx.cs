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

public partial class Alerts : System.Web.UI.UserControl
{
    SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("CMServer"));

    protected string ItemID
    {
        set { ViewState["nid"] = value; }
        get { return Convert.ToString(ViewState["nid"]); }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        BindEditor(txtContent);
        if (!IsPostBack)
        {
            ViewState["sortOrder"] = "desc";

            Bind();
        }
    }

    private void BindEditor(Editor editor)
    {
        //string TemplateItemList = "Paste,PasteText,CssClass,Bold,Italic,Underline,|,JustifyLeft,JustifyCenter,JustifyRight,JustifyFull,|,InsertOrderedList,InsertUnorderedList,|,InsertLink,InsertImage,InsertFlash,InsertMedia,InsertDocument,YouTube,|,RemoveFormat,CleanCode";
////        string TemplateItemList = "FormatBlock,Paste,PasteText,Bold,Italic,Underline,|,JustifyLeft,JustifyCenter,JustifyRight,JustifyFull,|,InsertOrderedList,InsertUnorderedList,|,InsertLink,InsertImage,InsertFlash,InsertMedia,InsertDocument,YouTube,|,RemoveFormat,CleanCode";
        string TemplateItemList = "FormatBlock,Paste,PasteText,Bold,Italic,Underline,|,JustifyLeft,JustifyCenter,JustifyRight,JustifyFull,|,InsertOrderedList,InsertUnorderedList,|,InsertLink,CleanCode";
        editor.TemplateItemList = TemplateItemList;
        editor.ResizeMode = CuteEditor.EditorResizeMode.None;
        editor.Height = 250;

        editor.EditorWysiwygModeCss = string.Format("/css/site.css?v={0},{1}", ConfigurationManager.AppSettings.Get("CSSVersion"), ConfigurationManager.AppSettings.Get("FontsUrl"));
        editor.PreviewModeCss = string.Format("/css/site.css?v={0},{1}", ConfigurationManager.AppSettings.Get("CSSVersion"), ConfigurationManager.AppSettings.Get("FontsUrl"));


        /*CuteEditor.ToolControl toolctrl = Editor.ToolControls["CssClass"];

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

        CuteEditor.ToolControl toolctrl = editor.ToolControls["FormatBlock"];
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
        SqlDataAdapter dapt = new SqlDataAdapter("select id, title, case when Language = 2 then 'French' else 'English' end as Language  from Alerts where status in (0, 1)", conn);
        dapt.SelectCommand.CommandType = CommandType.Text;

        if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() != "")
            dapt.SelectCommand.Parameters.AddWithValue("@id", Request.QueryString["id"].ToString());

        ////if (txtFilterText.Text != "")
        ////    dapt.SelectCommand.Parameters.AddWithValue("@Filter", txtFilterText.Text.Trim());

        DataTable dt = new DataTable();
        dapt.Fill(dt);

        if (dt.Rows.Count > 0)
        {
            DataView myDataView = new DataView();
            myDataView = dt.DefaultView;

            if (sortExp != string.Empty)
            {
                myDataView.Sort = string.Format("{0} {1}", sortExp, sortDir);
            }

            gvMain.DataSource = myDataView;
            gvMain.DataBind();

            pager1.ItemCount = myDataView.Count;
            if (gvMain.PageSize >= myDataView.Count)
            {
                pager1.Visible = false;
            }
            else
            {
                pager1.Visible = true;
            }

            tbl_Grids.Visible = true;
            tbl_noresults.Visible = false;
        }
        else
        {
            tbl_Grids.Visible = false;
            tbl_noresults.Visible = true;
        }
    }
 

    public void rowcommand(object o, CommandEventArgs e)
    {
        switch (e.CommandName)
        {
            ////case "deletepopup":
            ////    DeleteRow(e.CommandArgument.ToString());
            ////    break;
            case "editpopup":
                Edit(e.CommandArgument.ToString());
                break;
            default:
                break;
        }
    }

    ////public void DeleteRow(string id)
    ////{
    ////    SqlCommand command = new SqlCommand("PopupMessagesDelete", conn);
    ////    command.CommandType = CommandType.StoredProcedure;
    ////    command.Parameters.AddWithValue("@id", id);

    ////    conn.Open();
    ////    command.ExecuteNonQuery();
    ////    conn.Close();

    ////    Bind();
    ////}

    private void Edit(string id)
    {
        pnlEdit.Visible = true;
        pnlList.Visible = false;

        SqlDataAdapter dapt = new SqlDataAdapter("select * from Alerts where id=@id", conn);
        dapt.SelectCommand.CommandType = CommandType.Text;
        dapt.SelectCommand.Parameters.AddWithValue("@id", id);
        DataTable dt = new DataTable();
        dapt.Fill(dt);

        txtName.Text = dt.Rows[0]["title"].ToString();
        txtContent.Text = dt.Rows[0]["body"].ToString();
        ddlLanguage.SelectedValue = dt.Rows[0]["language"].ToString();
        ddlLanguage.Enabled = false;
        rblStatus.SelectedValue = Convert.ToInt32(dt.Rows[0]["status"]).ToString();

        ////ibSave.CommandName = "save";
        ////ibSave.CommandArgument = id;
        ///
        ItemID = id;
    }

    //////public void Add(object o, EventArgs e)
    //////{
    //////    pnlEdit.Visible = true;
    //////    pnlList.Visible = false;
    //////    ibSave.CommandName = "add";

    //////    txtName.Text = "";
    //////    txtContent.Text = "";
    //////    ddlLanguage.ClearSelection();
    //////    ddlLanguage.Enabled = true;
    //////}

  
    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.DataItem != null)
        {
            //DataRowView dr = e.Row.DataItem as DataRowView;

            // Model column
            /*if (e.Row.Cells[2].Text.Length > 45)
            {
                e.Row.Cells[2].ToolTip = e.Row.Cells[2].Text;
                e.Row.Cells[2].Text = e.Row.Cells[2].Text.Substring(0, 42) + "...";
            }*/

            // Campground column
            if (e.Row.Cells[2].Text.Length > 15)
            {
                e.Row.Cells[2].ToolTip = e.Row.Cells[2].Text;
                e.Row.Cells[2].Text = e.Row.Cells[2].Text.Substring(0, 12) + "...";
            }

            ImageButton ImageButton2 = (ImageButton)e.Row.FindControl("ImageButton2");
            ImageButton2.Attributes.Add("onclick", "return confirm('Are you sure want to delete this item?')");
        }
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
        this.gvMain.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
        pager1.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
        pager1.CurrentIndex = 1;
        this.gvMain.PageIndex = 0;
        Bind();
    }
    public void pager_Command(object sender, CommandEventArgs e)
    {
        int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
        pager1.CurrentIndex = currnetPageIndx;
        gvMain.PageIndex = currnetPageIndx - 1;
        Bind();
    }

    //////protected void btnFilter_Click(object sender, ImageClickEventArgs e)
    //////{
    //////    Bind();
    //////}
    //////protected void btnClearFilter_Click(object sender, ImageClickEventArgs e)
    //////{
    //////    /*txtFilterStartDate.Text = "";
    //////    txtFilterEndDate.Text = "";
    //////    txtFilterText.Text = "";
    //////    ddlFilterVacancyType.ClearSelection();

    //////    Bind();*/
    //////    Response.Redirect("/admin.aspx?c=popup");
    //////}

    protected void ibCancel_Click1(object sender, EventArgs e)
    {
        pnlEdit.Visible = false;
        pnlList.Visible = true;

        Bind();
    }

    protected void ibSave_Click(object sender, EventArgs e)
    {
        pnlEdit.Visible = false;
        pnlList.Visible = true;

        SqlCommand command = new SqlCommand();

       ///// if (e.CommandName == "save")
        {
            command = new SqlCommand("update Alerts set Title=@name, body=@html where id=@id update Alerts set status=@status ", conn);
            command.Parameters.AddWithValue("@id", ItemID);
        }
        ////else
        ////{
        ////    command = new SqlCommand("PopupMessagesInsert", conn);
        ////    command.Parameters.AddWithValue("@lang", ddlLanguage.SelectedValue);
        ////}

        command.CommandType = CommandType.Text;
        command.Parameters.AddWithValue("@name", txtName.Text.Trim());
        command.Parameters.AddWithValue("@html", txtContent.Text.Trim());
        command.Parameters.AddWithValue("@status", rblStatus.SelectedValue);

        conn.Open();
        command.ExecuteNonQuery();
        conn.Close();


        Bind();
    }
}