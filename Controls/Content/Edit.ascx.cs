using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using CuteEditor;

public partial class Editor : System.Web.UI.UserControl
{

    #region Properties
    public string LoggedInID
    {
        get
        {
            if (Session["LoggedInID"] == null)
            {
                gotologin();
                return ViewState["initialLoggedInID"] != null ? ViewState["initialLoggedInID"].ToString() : "1";
            }
            else
                return Session["LoggedInID"].ToString();
        }
    }
    public string Language
    {
        set { ViewState["Language"] = value; }
        get { return ViewState["Language"].ToString(); }
    }

    public string PageId
    {
        set { ViewState["PageId"] = value; }
        get { return ViewState["PageId"].ToString(); }
    }

    public string HtmlId
    {
        set { ViewState["HtmlId"] = value; }
        get { return ViewState["HtmlId"].ToString(); }
    }

    public bool IsPending
    {
        get { return btnPending.Visible; }
    } 
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["initialLoggedInID"] = LoggedInID;

        if (!IsPostBack)
        {
            if (!SetProperties())
            {
                pnlEdit.Visible = false;
                return;
            }

             // this.LoadEditor();
            showLive();
        }
        this.SetEditor();
    }

    private bool SetProperties()
    {
        if (Request.QueryString["PageID"] == null || Request.QueryString["id"] == null)
            return false;

        SqlDataAdapter dapt = new SqlDataAdapter("HtmlPageInfoByLanguageGet", ConfigurationManager.AppSettings["CMServer"]);
        dapt.SelectCommand.CommandType = CommandType.StoredProcedure;
        dapt.SelectCommand.Parameters.AddWithValue("@PageId", Request.QueryString["PageID"]);
        dapt.SelectCommand.Parameters.AddWithValue("@HtmlId", Request.QueryString["id"]);
        dapt.SelectCommand.Parameters.AddWithValue("@lang", Language);


        DataSet ds = new DataSet();
        dapt.Fill(ds);

        if (ds.Tables[0].Rows.Count == 0 || ds.Tables[1].Rows.Count == 0)
            return false;

        PageId = ds.Tables[0].Rows[0]["PageId"].ToString();
        HtmlId = ds.Tables[1].Rows[0]["HtmlId"].ToString();

        //Response.Write(PageId + "<br />");
        //Response.Write(HtmlId + "<br />");

        if (String.IsNullOrEmpty(PageId) || String.IsNullOrEmpty(HtmlId))
            return false;

        //Response.Write("<br />---------------------------");
        //Response.Write("Language: " + Language + "<br />");
        //Response.Write("PageId: " + PageId + "<br />");
        //Response.Write("HtmlId: " + HtmlId + "<br />");
        //Response.Write("IsPending: " + IsPending.ToString() + "<br />");
        return true;
    }

    public void LoadEditor()
    {
       // if (!IsPostBack)

        {
            Editor1.EnableStripScriptTags = false;

            Editor1.BreakElement = CuteEditor.BreakElement.P;

            //// Create the custom button using the Editor.CreateCommandButton method   
            ////Themes/%ThemeName%/Images/text.gif   
            //WebControl ctrl = Editor1.CreateCommandButton("btnInsertResponsiveRow", "insrow.gif", "Insert Responsive Row");
            //ctrl.Attributes["onclick"] = "CuteEditor_GetEditor(this).ExecCommand('PasteHTML',false,'<div class=\"row\"><div class=\"twelve columns\"><p>Lorem ipsum...</p></div></div>')";
            ////register the custom button   
            //Editor1.RegisterCustomButton("btnInsertResponsiveRow", ctrl);

            //ctrl = Editor1.CreateCommandButton("btnInsertResponsiveColumn", "inscol.gif", "Insert Responsive Column");
            //ctrl.Attributes["onclick"] = "CuteEditor_GetEditor(this).ExecCommand('PasteHTML',false,'<div class=\"four columns\"><p>Lorem ipsum...</p></div>')";
            ////register the custom button   
            //Editor1.RegisterCustomButton("btnInsertResponsiveColumn", ctrl);

            Editor1.TemplateItemList = "Find,CleanCode,netspell,|,Cut,Copy,PasteText,PasteWord,|,Undo,Redo,|,Bold,Italic,Underline,|,JustifyLeft,JustifyCenter,JustifyRight,JustifyFull,|,InsertOrderedList,InsertUnorderedList,Indent,Outdent,|,Subscript,Superscript,Ucase,Lcase|,InsertLayer,/,InsertDate,InsertTime,|,InsertChars,UniversalKeyboard,|,InsertImage,InsertMedia,YouTube,|,Table,|,InsertRowTop,InsertRowBottom,DeleteRow,|,InsertColumnLeft,InsertColumnRight,DeleteColumn,|,InsertCell,DeleteCell,|,MergeRight,HorSplitCell,MergeBottom,VerSplitCell,|,InsertHorizontalRule,InsertLink,Unlink,InsertAnchor,ImageMap,/,FormatBlock,CssClass,Images";

            Editor1.ThemeType = CuteEditor.ThemeType.Office2007;

            Editor1.Height = 460;
            Editor1.ResizeMode = CuteEditor.EditorResizeMode.ResizeCorner;
            Editor1.EmptyAlternateText = EmptyAlternateText.ForceAdd;


            Editor1.RenderRichDropDown = true;
            Editor1.StyleWithCSS = true;
            Editor1.EditorWysiwygModeCss = string.Format(ConfigurationManager.AppSettings.Get("EditorCss") + "?v={0},{1}", ConfigurationManager.AppSettings.Get("CSSVersion"), ConfigurationManager.AppSettings.Get("FontsUrl"));
            Editor1.PreviewModeCss = string.Format(ConfigurationManager.AppSettings.Get("EditorCss") + "?v={0},{1}", ConfigurationManager.AppSettings.Get("CSSVersion"), ConfigurationManager.AppSettings.Get("FontsUrl"));

            Editor1.EditorBodyStyle = "background-color:#FFFFFF; background-image:none; width:auto; height:auto;";

            //Editor1.SetSecurityAllowDelete(false);
            Editor1.SetSecurityAllowDeleteFolder(false);
            Editor1.SetSecurityAllowRename(false);

            Editor1.URLType = CuteEditor.URLType.SiteRelative;
            //Editor1.AutoParseClasses = false;

            //Editor1.AllowPasteHtml = false;
            //Editor1.
            Editor1.EditorOnPaste = CuteEditor.PasteBehavior.PasteText;

            //Editor1.SetSecurityFilesGalleryPath("/Content/Files/");
            //Editor1.SetSecurityFlashGalleryPath("/Content/Flash/");
            //Editor1.SetSecurityGalleryPath("/Content/Gallery/");
            //Editor1.SetSecurityImageBrowserPath("/Uploads/");



            Editor1.SetSecurityMaxDocumentSize(256000);
            Editor1.SetSecurityMaxDocumentFolderSize(90000000);
            Editor1.SetSecurityMaxMediaSize(256000);
            Editor1.SetSecurityMaxMediaFolderSize(90000000);
            Editor1.SetSecurityMaxImageSize(1024);
            Editor1.SetSecurityMaxImageFolderSize(90000000);
            Editor1.SetSecurityMaxFlashFolderSize(90000000);
            Editor1.SetSecurityMaxFlashSize(102400);

            Editor1.SetSecurityMaxTemplateFolderSize(90000000);

            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"].ToString());
            // SqlDataAdapter dapt = new SqlDataAdapter("select G.name from Pages_Group,Groups G where Page_id=" + Request.QueryString["PageID"].ToString() + " and G.id=Pages_Group.Group_id", sqlConn);
            //SqlDataAdapter dapt = new SqlDataAdapter("select G.name from Pages_Group,Groups G where Page_id=" + this.PageId + " and G.id=Pages_Group.Group_id", sqlConn);
            SqlDataAdapter dapt = new SqlDataAdapter("select G.name, isnull(G.private, 0) as private, G.id from Pages_Group pg inner join Groups G on pg.Group_id = G.id where pg.Page_id = @PageId", sqlConn);
            dapt.SelectCommand.Parameters.AddWithValue("@PageId", this.PageId);
            DataSet ds = new DataSet();
            dapt.Fill(ds);

            //string group = (Session["LoggedInID"].ToString() == "1" ? "" : ds.Tables[0].Rows[0][0].ToString());

            string group = "";  // (LoggedInID == "1" ? "" : ds.Tables[0].Rows[0]["name"].ToString());
            if( LoggedInID != "1" 
               
                )
            {
                group = ds.Tables[0].Rows[0]["name"].ToString();
            }


            string physDir = HttpContext.Current.Request.MapPath("/uploads/" + group.Trim());

            if (!Directory.Exists(physDir))
            {
                Directory.CreateDirectory(physDir);
            }

            string uploadsPath = HttpContext.Current.Request.MapPath("/uploads/" + ds.Tables[0].Rows[0]["name"].ToString());

            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            if ((bool)ds.Tables[0].Rows[0]["private"])
            {
                if (!File.Exists(uploadsPath + @"/web.config"))
                    CMSHelper.CreatePrivateConfigFile(ds.Tables[0].Rows[0]["name"].ToString(), uploadsPath, ds.Tables[0].Rows[0]["id"].ToString());
            }
            else
            {
                if (File.Exists(uploadsPath + @"/web.config"))
                    File.Delete(uploadsPath + @"/web.config");
            }

            Editor1.SetSecurityImageGalleryPath("~/uploads/" + group.Trim());
            Editor1.SetSecurityImageBrowserPath("~/uploads/" + group.Trim());
            Editor1.SetSecurityMediaGalleryPath("~/uploads/" + group.Trim());
            Editor1.SetSecurityFilesGalleryPath("~/uploads/" + group.Trim());
            Editor1.SetSecurityFlashGalleryPath("~/uploads/" + group.Trim());
            Editor1.SetSecurityGalleryPath("~/uploads/" + group.Trim());
            Editor1.SetSecurityTemplateGalleryPath("~/templates/ResponsiveTemplates");
            //Editor1.SetSecurityTemplateGalleryPath("~/templates");

            //if (Session["Language"] != null && Session["Language"].ToString() == "2")
            if (Language != null && Language == "2")
                Editor1.CustomCulture = "fr-FR";

            //Editor1.SetSecurityMaxFlashSize();

            CuteEditor.ToolControl ronster = Editor1.ToolControls["Images"];

            if (ronster != null)
            {

                DataTable dt = new DataTable();
                string sqlstr = "ZoneControlList";
                SqlCommand sq = new SqlCommand(sqlstr);
                //sq.Parameters.AddWithValue("@control", "TableWidget,photos,video");
                sq.Parameters.AddWithValue("@control", "video");
                sq.Parameters.AddWithValue("@MYUSERID", Session["LoggedInId"].ToString());

                dt = getTable(sq);

                if (dt.Rows.Count > 0)
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

                    foreach (DataRow dr in dt.Rows)
                    {
                        string title = dr["name"].ToString();
                        int npos = title.IndexOf("-");
                        if (npos > 0)
                            title = title.Substring(npos + 1).Trim();

                        dd.Items.Add("<img src=\"/images/icons/types/txt.png\"/><span>" + dr["name"].ToString() + "</span>", "",
                                        String.Format("&lt;widget id='{0}' class='{1}' title='{2}' &sol;&gt;",
                                                        dr["id"].ToString(),
                                                        dr["control"].ToString(),
                                                        title
                                                        )
                                    );

                        //other variant for the format could be : <widget id='10159' class='control-quicklinks'>Quicklinks: Test</widget>
                    }
                }
            }



            CuteEditor.ToolControl csstoolctrl = Editor1.ToolControls["CssClass"];

            if (csstoolctrl != null)
            {
                CuteEditor.RichDropDownList dropdown = (CuteEditor.RichDropDownList)csstoolctrl.Control;
                //the first item is the caption
                CuteEditor.RichListItem richitem = dropdown.Items[0];
                //clear the items from configuration files
                dropdown.Items.Clear();
                //add the caption
                dropdown.Items.Add(richitem);
                //add value only
                dropdown.Items.Add("<span>button</span>", "button", "button");
                dropdown.Items.Add("<span>enhanced button</span>", "enhanced button", "button button-primary");
                dropdown.Items.Add("<span>table heading</span>", "table heading", "table-heading");
                dropdown.Items.Add("<span>Toggle Header</span>", "Toggle Header", "Toggle-Header");
                dropdown.Items.Add("<span>Toggle More</span>", "Toggle More", "Toggle-More");
                dropdown.Items.Add("<span>tel-link-bodytext</span>", "tel-link-bodytext", "tel-link-bodytext");
               //dropdown.Items.Add("<span>two-columns</span>", "two-columns", "two-columns");

                //add html and text and value
                //dropdown.Items.Add("<span class='BoldGreen'>Bold      Green Text</span>","Bold      Green Text","BoldGreen");
            }

            CuteEditor.ToolControl toolctrl = Editor1.ToolControls["FormatBlock"];
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
               // dropdown.Items.Add(String.Format("<span style=\"{0}\">Header</span>", ConfigurationManager.AppSettings.Get("EditorDropdownH1")), "Header", "<h1>");
                dropdown.Items.Add(String.Format("<span style=\"{0}\">Title</span>", ConfigurationManager.AppSettings.Get("EditorDropdownH2")), "Title", "<h2>");
                dropdown.Items.Add(String.Format("<span style=\"{0}\">Subtitle</span>", ConfigurationManager.AppSettings.Get("EditorDropdownH3")), "Subtitle", "<h3>");
              //  dropdown.Items.Add(String.Format("<span style=\"{0}\">SubSubtitle</span>", ConfigurationManager.AppSettings.Get("EditorDropdownH4")), "SubSubtitle", "<h4>");

            //dropdown.Items.Add("DIV", "<div>");
        }

            Editor1.BackColor = System.Drawing.Color.FromArgb(103, 172, 198);
        }

        //CuteEditor.ToolControl tc = Editor1.ToolControls["insertcustombutonhere"];
        //if (tc != null)
        //{
        //    System.Web.UI.WebControls.Image Image1 = new System.Web.UI.WebControls.Image();
        //    Image1.ToolTip = "Insert responsive row";
        //    Image1.ImageUrl = "tools.gif";
        //    Image1.CssClass = "CuteEditorButton";
        //    SetMouseEvents(Image1);
        //    Image1.Attributes["onclick"] = "CuteEditor_GetEditor(this).ExecCommand('PasteHTML',false,'<div class=\"row\"></div>')";
        //    tc.Control.Controls.Add(Image1);
        //}
        
    }

    public void SetEditor()
    {
        CuteEditor.ToolControl tc = Editor1.ToolControls["InsertMedia"];
        if (tc != null)
        {

            System.Web.UI.WebControls.Image Image2 = new System.Web.UI.WebControls.Image();
            Image2.ToolTip = "Insert Video";
            Image2.ImageUrl = "~/CuteSoft_Client/CuteEditor/Images/flash.gif";
            Image2.CssClass = "CuteEditorButton";
            SetMouseEvents(Image2);
            Image2.Attributes["Command"] = "MyCmd";
            tc.Control.Controls.Add(Image2);

            //Editor1.AddToolControl("CustomPostBack",postbutton);
        }
    }

    void SetMouseEvents(WebControl control)
    {
        control.Attributes["onmouseover"] = "CuteEditor_ButtonCommandOver(this)";
        control.Attributes["onmouseout"] = "CuteEditor_ButtonCommandOut(this)";
        control.Attributes["onmousedown"] = "CuteEditor_ButtonCommandDown(this)";
        control.Attributes["onmouseup"] = "CuteEditor_ButtonCommandUp(this)";
        control.Attributes["ondragstart"] = "CuteEditor_CancelEvent()";
        control.Attributes["onclick"] = "ShowMyDialog(this)";
    }

  public void showLive()
    {
        this.LoadEditor();

        btnLive.CssClass = "editor_button_top_on";
        btnPending.CssClass = "editor_button_top_off";
        btnMessages.CssClass = "editor_button_top_off";

        Editor1.Visible = true;
        messagearea.Visible = false;

        btnApprove.Visible = false;
        btnNotice.Visible = false;

        btnSave.Visible = true;
        btnPreview.Visible = false;

        SqlDataAdapter dapt = new SqlDataAdapter("CONTROL_Content_Get", ConfigurationManager.AppSettings["CMServer"]);
        dapt.SelectCommand.CommandType = CommandType.StoredProcedure;
        dapt.SelectCommand.Parameters.AddWithValue("@id", this.HtmlId);              //Request.QueryString["id"]);
        DataSet ds = new DataSet();
        dapt.Fill(ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            Editor1.Text = ds.Tables[0].Rows[0][0].ToString();

            //Response.Write("Language= " + Language + "<br />");
            //Response.Write(ds.Tables[0].Rows.Count + "<br />");

            if (ds.Tables[1].Rows.Count > 0)
            {
                btnPending.Visible = true;
            }
            else
            {
                btnPending.Visible = false;
            }
        }
    }

    public void clickSave(object o, EventArgs e)
    {
        Session["Language"] = Language;
        //if (Session["LoggedInID"] == null)
        //{
        //    gotologin();
        //    return;
        //}

        dosave();
    }

    public void dosave()
    {
        string content = Editor1.Text;

        //if (content.Contains("alt=\"\""))
        //{
        //    //Page.ClientScript.RegisterStartupScript(GetType(), "NoAlt", "alert('There are some images with empty alternate text.')", true);
        //    UpdatePanel UpdatePanel1 = (UpdatePanel)Page.FindControl("UpdatePanel1");
        //    if (UpdatePanel1 != null)
        //        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "NoAlt", "alert('There are some images with empty alternate text.');", true);
        //    else
        //        Page.ClientScript.RegisterStartupScript(GetType(), "NoAlt", "alert('There are some images with empty alternate text.')", true);
        //    return;
        //}

        int npos = content.IndexOf("/controls/content/edit.aspx?id=");
        while (npos > -1)
        {
            /*int npos1 = content.LastIndexOf("http://", npos);
            if (npos1 < 0)
                npos1 = npos;*/
            int npos2 = content.IndexOf("#", npos);
            /*content = content.Remove(npos1, npos2 - npos1);*/
            try { content = content.Remove(npos, npos2 - npos); }
            catch { break; }

            npos = content.IndexOf("/controls/content/edit.aspx?id=");
        }

        int lpos = content.IndexOf("<a href=", 0);
        //Response.Write("Pos: " + lpos.ToString());
        while (lpos > -1)
        {
            int lpos2 = content.IndexOf(">", lpos);
            int lpos3 = content.IndexOf("</a>", lpos2);
            if (lpos3 - lpos2 == 1)
            {
                content = content.Remove(lpos, (lpos3 + 4) - lpos);
            }
            else
            {
                lpos = lpos3;
            }

            lpos = content.IndexOf("<a href=", lpos);
        }

        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
        SqlCommand sqlComm = new SqlCommand("CONTROL_Content_Set", sqlConn);
        sqlComm.CommandType = CommandType.StoredProcedure;
        sqlComm.Parameters.AddWithValue("@id", this.HtmlId);                                     //  Request.QueryString["id"]);
        //sqlComm.Parameters.AddWithValue("@user", Session["LoggedInID"].ToString());
        sqlComm.Parameters.AddWithValue("@user", LoggedInID);
        sqlComm.Parameters.AddWithValue("@content", content);

        sqlConn.Open();
        sqlComm.ExecuteNonQuery();
        sqlConn.Close();

        showPending();

        forcesubscribe();
    }

    public void clickedLive(object o, EventArgs e)
    {
        Session["Language"] = Language;
        showLive();
    }

    public void clickedPending(object o, EventArgs e)
    {
        Session["Language"] = Language;
        showPending();
    }

    public string myseo = "";
    public void showPending()
    {
        this.LoadEditor();

        btnLive.CssClass = "editor_button_top_off";
        btnPending.CssClass = "editor_button_top_on";
        btnMessages.CssClass = "editor_button_top_off";

        Editor1.Visible = true;
        messagearea.Visible = false;
        btnSave.Visible = true;


        if (IsPublisher())
        {
            btnApprove.Visible = true;
        }
        else
        {
            btnNotice.Visible = true;
        }

        SqlDataAdapter dapt = new SqlDataAdapter("CONTROL_Content_Get", ConfigurationManager.AppSettings["CMServer"]);
        dapt.SelectCommand.CommandType = CommandType.StoredProcedure;
        dapt.SelectCommand.Parameters.AddWithValue("@id", this.HtmlId);             // Request.QueryString["id"]);
        dapt.SelectCommand.Parameters.AddWithValue("@pageid", PageId);
        DataSet ds = new DataSet();
        dapt.Fill(ds);



        if (ds.Tables[1].Rows.Count > 0)
        {
            Editor1.Text = ds.Tables[1].Rows[0][0].ToString();
            btnPending.Visible = true;
        }
        else
        {
            btnPending.Visible = false;
        }

        if (ds.Tables[2].Rows.Count > 0)
        {
            myseo = ds.Tables[2].Rows[0][0].ToString();
            btnPreview.Visible = myseo != "";
        }
    }

    public void clickedMessages(object o, EventArgs e)
    {
        showMessages();
    }

    public void clickedSendMessage(object o, EventArgs e)
    {
        sendMessage();
    }

    public void clickedSub(object o, EventArgs e)
    {
        subscribe();
    }

    private void subscribe()
    {
        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"].ToString());
        //SqlDataAdapter dapt = new SqlDataAdapter("select * from HtmlMessageSub where htmlid=" + Request.QueryString["id"].ToString() + " and userid=" + Session["LoggedInID"].ToString(), sqlConn);
        //SqlDataAdapter dapt = new SqlDataAdapter("select * from HtmlMessageSub where htmlid=" + this.HtmlId + " and userid=" + Session["LoggedInID"].ToString(), sqlConn);
        SqlDataAdapter dapt = new SqlDataAdapter("select * from HtmlMessageSub where htmlid=" + this.HtmlId + " and userid=" + LoggedInID, sqlConn);
        DataSet ds = new DataSet();
        dapt.Fill(ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            //SqlCommand sqlComm = new SqlCommand("delete from HtmlMessageSub where htmlid=" + Request.QueryString["id"].ToString() + " and userid=" + Session["LoggedInID"].ToString(), sqlConn);
            //SqlCommand sqlComm = new SqlCommand("delete from HtmlMessageSub where htmlid=" + this.HtmlId + " and userid=" + Session["LoggedInID"].ToString(), sqlConn);
            SqlCommand sqlComm = new SqlCommand("delete from HtmlMessageSub where htmlid=" + this.HtmlId + " and userid=" + LoggedInID, sqlConn);
            sqlConn.Open();
            sqlComm.ExecuteNonQuery();
            sqlConn.Close();
        }
        else
        {
          //  SqlCommand sqlComm = new SqlCommand("insert into HtmlMessageSub(htmlid,userid) values(" + Request.QueryString["id"].ToString() + ", " + Session["LoggedInID"].ToString() + ")", sqlConn);
            //SqlCommand sqlComm = new SqlCommand("insert into HtmlMessageSub(htmlid,userid) values(" + this.HtmlId + ", " + Session["LoggedInID"].ToString() + ")", sqlConn);
            SqlCommand sqlComm = new SqlCommand("insert into HtmlMessageSub(htmlid,userid) values(" + this.HtmlId + ", " + LoggedInID + ")", sqlConn);
            sqlConn.Open();
            sqlComm.ExecuteNonQuery();
            sqlConn.Close();
        }

        bindlabel();
    }

    public void forcesubscribe()
    {
        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"].ToString());
    //    SqlDataAdapter dapt = new SqlDataAdapter("select * from HtmlMessageSub where htmlid=" + Request.QueryString["id"].ToString() + " and userid=" + Session["LoggedInID"].ToString(), sqlConn);
        //SqlDataAdapter dapt = new SqlDataAdapter("select * from HtmlMessageSub where htmlid=" + this.HtmlId + " and userid=" + Session["LoggedInID"].ToString(), sqlConn);
        SqlDataAdapter dapt = new SqlDataAdapter("select * from HtmlMessageSub where htmlid=" + this.HtmlId + " and userid=" + LoggedInID, sqlConn);
        DataSet ds = new DataSet();
        dapt.Fill(ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            //SqlCommand sqlComm = new SqlCommand("delete from HtmlMessageSub where htmlid=" + Request.QueryString["id"].ToString() + " and userid=" + Session["LoggedInID"].ToString(), sqlConn);
            //sqlConn.Open();
            //sqlComm.ExecuteNonQuery();
            //sqlConn.Close();
        }
        else
        {
            //SqlCommand sqlComm = new SqlCommand("insert into HtmlMessageSub(htmlid,userid) values(" + Request.QueryString["id"].ToString() + ", " + Session["LoggedInID"].ToString() + ")", sqlConn);
            //SqlCommand sqlComm = new SqlCommand("insert into HtmlMessageSub(htmlid,userid) values(" + this.HtmlId + ", " + Session["LoggedInID"].ToString() + ")", sqlConn);
            SqlCommand sqlComm = new SqlCommand("insert into HtmlMessageSub(htmlid,userid) values(" + this.HtmlId + ", " + LoggedInID + ")", sqlConn);
            sqlConn.Open();
            sqlComm.ExecuteNonQuery();
            sqlConn.Close();
        }

        bindlabel();
    }

    public void forceunsubscribe()
    {
        if (Session["LoggedInID"] != null)
        {
            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"].ToString());
            //SqlDataAdapter dapt = new SqlDataAdapter("select * from HtmlMessageSub where htmlid=" + Request.QueryString["id"].ToString() + " and userid=" + Session["LoggedInID"].ToString(), sqlConn);
            // DataSet ds = new DataSet();
            // dapt.Fill(ds);
            //  if (ds.Tables[0].Rows.Count > 0)
            //  {
            ////SqlCommand sqlComm = new SqlCommand("delete from HtmlMessageSub where htmlid=" + Request.QueryString["id"].ToString() + " and userid=" + Session["LoggedInID"].ToString(), sqlConn);
            //SqlCommand sqlComm = new SqlCommand("delete from HtmlMessageSub where htmlid=" + this.HtmlId + " and userid=" + Session["LoggedInID"].ToString(), sqlConn);
            SqlCommand sqlComm = new SqlCommand("delete from HtmlMessageSub where htmlid=" + this.HtmlId + " and userid=" + LoggedInID, sqlConn);
            sqlConn.Open();
            sqlComm.ExecuteNonQuery();
            sqlConn.Close();
            //  }
            //  else
            //  {
            //      SqlCommand sqlComm = new SqlCommand("insert into HtmlMessageSub(htmlid,userid) values(" + Request.QueryString["id"].ToString() + ", " + Session["LoggedInID"].ToString() + ")", sqlConn);
            //      sqlConn.Open();
            //      sqlComm.ExecuteNonQuery();
            //      sqlConn.Close();
            //  }

            bindlabel();
        }
    }

    private void sendMessage()
    {
        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"].ToString());
        SqlCommand sqlComm = new SqlCommand("insert into htmlmessages(message,htmlid,editor) values(@msg,@html,@editor)", sqlConn);

        sqlComm.Parameters.AddWithValue("@msg", txtMessage.Text);
        sqlComm.Parameters.AddWithValue("@html", this.HtmlId);                          // Request.QueryString["id"].ToString());
        //sqlComm.Parameters.AddWithValue("@editor", Session["LoggedInID"].ToString());
        sqlComm.Parameters.AddWithValue("@editor", LoggedInID);

        sqlConn.Open();
        sqlComm.ExecuteNonQuery();
        sqlConn.Close();

        BindMessages();

        txtMessage.Text = "";

        //SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
        //SqlDataAdapter dapt = new SqlDataAdapter("select email from users where id in (select userid from htmlmessagesub where htmlid=" + Request.QueryString["id"] + ") select language,seo from pages where id=" + Request.QueryString["PageId"], sqlConn);
        SqlDataAdapter dapt = new SqlDataAdapter("select email from users where id in (select userid from htmlmessagesub where htmlid=" + this.HtmlId + ") select language,seo from pages where id=" + this.PageId, sqlConn);
        DataSet ds = new DataSet();
        dapt.Fill(ds);

        //string url = "/" + ((ds.Tables[1].Rows[0]["language"].ToString() == "1") ? "en/" : "fr/") + ds.Tables[1].Rows[0]["seo"].ToString();
        string url = ((ds.Tables[1].Rows[0]["language"].ToString() == "1") ? CMSHelper.SeoPrefixEN : "/fr/") + ds.Tables[1].Rows[0]["seo"].ToString();

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            
            MailMessage message = new MailMessage(Email.FromName + "<" + Email.FromAddress + ">", dr["email"].ToString(), "Content Message", "A new message has been posted about the content on this page: <a href=\"http://" + ConfigurationManager.AppSettings["SiteUrl"] + "/login.aspx?r=" + url + "\">" + "http://" + ConfigurationManager.AppSettings["SiteUrl"] + url + "</a>");

            message.IsBodyHtml = true;
            SmtpClient emailClient = new SmtpClient(Email.SmtpServer, Email.SmtpPort);
            if (Email.UseSmtpCredentials)
            {
                emailClient.Credentials = new System.Net.NetworkCredential(Email.SmtpUsername, Email.SmtpPassword);
                //emailClient.UseDefaultCredentials = true;
            }
            emailClient.EnableSsl = Email.EnableSsl;
            emailClient.Send(message);
        }
    }

    public void showMessages()
    {
        btnLive.CssClass = "editor_button_top_off";
        btnPending.CssClass = "editor_button_top_off";
        btnMessages.CssClass = "editor_button_top_on";

        btnApprove.Visible = false;
        btnNotice.Visible = false;

        btnSave.Visible = false;
        btnPreview.Visible = false;

        Editor1.Visible = false;
        messagearea.Visible = true;

        BindMessages();
        bindlabel();
    }

    private void BindMessages()
    {
        txtMessages.Text = "";

        SqlDataAdapter dapt = new SqlDataAdapter("MessagesGet", ConfigurationManager.AppSettings["CMServer"]);
        dapt.SelectCommand.CommandType = CommandType.StoredProcedure;
        dapt.SelectCommand.Parameters.AddWithValue("@id", this.HtmlId);                         //Request.QueryString["id"]);
        DataSet ds = new DataSet();
        dapt.Fill(ds);

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            txtMessages.Text += "\n" + dr["username"].ToString() + " (" + dr["timestamp"].ToString() + "): " + dr["message"].ToString();
        }

    }

    private void bindlabel()
    {
        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"].ToString());
        //SqlDataAdapter dapt = new SqlDataAdapter("select * from HtmlMessageSub where htmlid=" + Request.QueryString["id"].ToString() + " and userid=" + Session["LoggedInID"].ToString(), sqlConn);
        //SqlDataAdapter dapt = new SqlDataAdapter("select * from HtmlMessageSub where htmlid=" + this.HtmlId + " and userid=" + Session["LoggedInID"].ToString(), sqlConn);
        SqlDataAdapter dapt = new SqlDataAdapter("select * from HtmlMessageSub where htmlid=" + this.HtmlId + " and userid=" + LoggedInID, sqlConn);
        DataSet ds = new DataSet();
        dapt.Fill(ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            lblSub.Text = "You are subscribed to updates for this content";
            btnSubscribe.Visible = true;
        }
        else
        {
            //lblSub.Text = "You are NOT subscribed to updates for this content";
            lblSub.Text = "";
            btnSubscribe.Visible = false;
        }
    }

    private bool IsPublisher()
    {
        int page = Convert.ToInt32(this.PageId);                                 // Convert.ToInt32(Request.QueryString["PageID"]);
        //int user = Convert.ToInt32(Session["LoggedInID"]);
        int user = Convert.ToInt32(LoggedInID);

        if (Permissions.Get(user, page) > 2)
        {
            return true;
        }

        return false;

    }

    public void clickedNotice(object o, EventArgs e)
    {

        Session["Language"] = Language;

        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);

        //SqlDataAdapter dapt = new SqlDataAdapter("select color from groups where id = (select group_id from Pages_Group where Page_id=" + this.PageId + ") select language,seo from pages where id=" + this.PageId + " select username from users where id=" + Session["LoggedInID"].ToString(), sqlConn);
        SqlDataAdapter dapt = new SqlDataAdapter("select color from groups where id = (select group_id from Pages_Group where Page_id=" + this.PageId + ") select language,seo from pages where id=" + this.PageId + " select username from users where id=" + LoggedInID, sqlConn);
        DataSet ds = new DataSet();
        dapt.Fill(ds);

        //string url = "/" + ((ds.Tables[1].Rows[0]["language"].ToString() == "1") ? "en/" : "fr/") + ds.Tables[1].Rows[0]["seo"].ToString();
        string url = ((ds.Tables[1].Rows[0]["language"].ToString() == "1") ? CMSHelper.SeoPrefixEN : "/fr/") + ds.Tables[1].Rows[0]["seo"].ToString();

        MailMessage message = new MailMessage(Email.FromName + "<" + Email.FromAddress + ">", ds.Tables[0].Rows[0][0].ToString(), Email.FromName + " - Approval Request", "An approval has been requested, by the user [" + ds.Tables[2].Rows[0][0].ToString() + "] for content on this page: <a href=\"http://" + ConfigurationManager.AppSettings["SiteUrl"] + "/login.aspx?r=" + url + "\">" + "http://" + ConfigurationManager.AppSettings["SiteUrl"] + url + "</a>");


        message.IsBodyHtml = true;
        SmtpClient emailClient = new SmtpClient(Email.SmtpServer, Email.SmtpPort);
        if (Email.UseSmtpCredentials)
        {
            emailClient.Credentials = new System.Net.NetworkCredential(Email.SmtpUsername, Email.SmtpPassword);
            //emailClient.UseDefaultCredentials = true;
        }
        emailClient.EnableSsl = Email.EnableSsl;
        emailClient.Send(message);

        showLive();
        
        this.forcesubscribe();


        //ClientScript.RegisterStartupScript(GetType(),"asd","parent.parent.GB_hide();",true);
        //ClientScript.RegisterClientScriptBlock(GetType(), "closeScript", "if (window.opener.location.href.indexOf(\"admin.aspx\") < 0) { window.opener.location.reload(); } window.close();", true);
    }

    public void clickedApprove(object o, EventArgs e)
    {
        Session["Language"] = Language;

        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
        SqlCommand sqlComm = new SqlCommand("CONTROL_Content_Activate", sqlConn);
        sqlComm.CommandType = CommandType.StoredProcedure;
        sqlComm.Parameters.AddWithValue("@html_id", this.HtmlId);
        sqlComm.CommandTimeout = 120;

        sqlConn.Open();
        sqlComm.ExecuteNonQuery();
        sqlConn.Close();

        //ViewState["approve"] = true;

        //ClientScript.RegisterStartupScript(GetType(),"asd","parent.parent.window.location.reload();",true); // REAL ONE


        //ClientScript.RegisterClientScriptBlock(GetType(), "closeScript", "window.opener.location.reload();window.close();", true);

        //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "closeScript", "if (window.opener.location.href.indexOf(\"admin.aspx\") < 0) { window.opener.location.reload(); } window.open('','_self','');window.close();", true);


        //Response.Redirect(Request.Path);

        showLive();

        this.forceunsubscribe();


    }


    #region Login
    public void testlogout(object o, EventArgs e)
    {
        gotologin();
    }


    public void login(object o, EventArgs e)
    {
        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
        SqlDataAdapter dapt = new SqlDataAdapter("BASE_Login", sqlConn);
        dapt.SelectCommand.CommandType = CommandType.StoredProcedure;
        dapt.SelectCommand.Parameters.AddWithValue("@username", txtUsername.Text);
        dapt.SelectCommand.Parameters.AddWithValue("@password", HashString(txtPassword.Text));

        DataSet ds = new DataSet();
        dapt.Fill(ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["LoggedInID"] = ds.Tables[0].Rows[0]["id"].ToString();
            //this.dosave();
            //this.dosave();
            pnlEdit.Visible = true;
            pnlLogin.Visible = false;
        }
        else
        {
            //literr.Text = "<span class=\"bodytext\"><strong>Invalid Username or Password. Please try again.</strong></span><br />";
            txtUsername.Text = "";
            txtPassword.Text = "";
        }
    }

    private string HashString(string Value)
    {
        System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] data = System.Text.Encoding.ASCII.GetBytes(Value);
        data = x.ComputeHash(data);
        string ret = "";
        for (int i = 0; i < data.Length; i++)
            ret += data[i].ToString("x2").ToLower();
        return ret;
    }

    public void gotologin()
    {
        pnlEdit.Visible = false;
        pnlLogin.Visible = true;
    } 
    #endregion

    protected void btnPreview_Click(object sender, EventArgs e)
    {
        SqlDataAdapter dapt = new SqlDataAdapter("CONTROL_Content_Get", ConfigurationManager.AppSettings["CMServer"]);
        dapt.SelectCommand.CommandType = CommandType.StoredProcedure;
        dapt.SelectCommand.Parameters.AddWithValue("@id", this.HtmlId);             // Request.QueryString["id"]);
        dapt.SelectCommand.Parameters.AddWithValue("@pageid", PageId);
        DataSet ds = new DataSet();
        dapt.Fill(ds);

        this.LoadEditor();

        if (ds.Tables[2].Rows.Count > 0)
        {
            myseo = ds.Tables[2].Rows[0][0].ToString();

            if (Language == "1")
            {
                //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "openPreviewt", "window.open('/en/" + myseo + "?pv=1');", true);
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "openPreviewt", "window.open('" + CMSHelper.SeoPrefixEN + myseo + "?pv=1');", true);
            }
            else
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "openPreviewt", "window.open('/fr/" + myseo + "?pv=1');", true);

        }
    }

    #region dal
    protected string _connection = ConfigurationManager.AppSettings.Get("CMServer");

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

    private DataSet getDataset(SqlCommand cmd, bool a)
    {
        DataSet dt = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.SelectCommand.CommandType = CommandType.Text;
        da.SelectCommand.Connection = new SqlConnection(_connection);
        da.Fill(dt);
        return dt;
    }

    private bool ProcessRecord(string sql, string fld, string _value, CommandType type)
    {
        SqlCommand cmd = new SqlCommand(sql, new SqlConnection(_connection));
        cmd.CommandType = type;
        cmd.Parameters.AddWithValue(fld, _value);
        cmd.Connection.Open();
        int ret = cmd.ExecuteNonQuery();
        cmd.Connection.Close();
        return (ret > 0);
    }

    private bool ProcessRecord(string sql, List<SqlParameter> parms, CommandType type)
    {
        SqlCommand cmd = new SqlCommand(sql, new SqlConnection(_connection));
        cmd.CommandType = type;

        foreach (SqlParameter p in parms)
            cmd.Parameters.Add(p);

        cmd.Connection.Open();
        int ret = cmd.ExecuteNonQuery();
        cmd.Connection.Close();
        return (ret > 0);
    }

    private bool RemoveRecord(string sql, int rcrd)
    {
        return ProcessRecord(sql, "@id", rcrd.ToString(), CommandType.Text);
    }
    #endregion

}