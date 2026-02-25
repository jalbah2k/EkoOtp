using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CuteWebUI;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Text;
using CuteEditor;

public partial class Controls_ContentAdd_ContentAdd : System.Web.UI.UserControl
{
    #region Properties

    //private string SEO
    //{
    //    get
    //    {
    //        string[] seolist = Request.QueryString["seo"].Split(',');
    //        string seo = string.Empty;
    //        if (seolist.Length > 1)
    //            seo = seolist[seolist.Length - 1];
    //        else
    //            seo = seolist[0];

    //        return seo;
    //    }
    //}

    private int LoggedInID
    {
        get
        {
            int _LoggedInID = 0;
            if (Session["LoggedInID"] != null)
                int.TryParse(Session["LoggedInID"].ToString(), out _LoggedInID);
            //else
            //{
            //    string url = "/Login.aspx?r=" + SEO;
            //    Response.Redirect(url);
            //}

            return _LoggedInID;
        }
    }

    //private string LanguagePrefix
    //{
    //    get
    //    {
    //        //return (Session["Language"] != null && Session["Language"] == "2" ? "/fr" : "/en");
    //        return (CMSHelper.GetCleanQueryString("lang", "1") == "1" ? "/en" : "/fr");
    //    }
    //}

    private string Language
    {
        get
        {
            //return (Session["Language"] != null ? Session["Language"].ToString() : "1");
            //return CMSHelper.GetCleanQueryString("lang", "1");
            return CMSHelper.GetLanguageNumber();
        }
    }

    #endregion Properties

    public Controls_ContentAdd_ContentAdd()
    {
    }

    public Controls_ContentAdd_ContentAdd(string par)
    {
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BuildClickOnceButton(btnSave, divSubmitted);

            //BindEditor(txtContent);
            
            /*if (Session["LoggedInID"] != null)
            {
                if (Permissions.Get(int.Parse(Session["LoggedInID"].ToString()), int.Parse(Session["PageID"].ToString())) > 1)
                //if(Session["LoggedInID"].ToString()=="1")
                {*/
                    btnContentAdd.Visible = true;
                /*}
            }*/
        }
    }

    private void BindEditor(Editor editor)
    {
        //string TemplateItemList = "Paste,PasteText,CssClass,Bold,Italic,Underline,|,JustifyLeft,JustifyCenter,JustifyRight,JustifyFull,|,InsertOrderedList,InsertUnorderedList,|,InsertLink,InsertImage,InsertFlash,InsertMedia,InsertDocument,YouTube,|,RemoveFormat,CleanCode";
        //string TemplateItemList = "FormatBlock,Paste,PasteText,Bold,Italic,Underline,|,JustifyLeft,JustifyCenter,JustifyRight,JustifyFull,|,InsertOrderedList,InsertUnorderedList,|,InsertLink,InsertImage,InsertFlash,InsertMedia,InsertDocument,YouTube,|,RemoveFormat,CleanCode";
        string TemplateItemList = "FormatBlock,Paste,PasteText,|,Undo,Redo,|,Bold,Italic,Underline,|,JustifyLeft,JustifyCenter,JustifyRight,JustifyFull,|,InsertOrderedList,InsertUnorderedList,|,InsertLink,InsertImage,InsertFlash,InsertMedia,YouTube,|,RemoveFormat,CleanCode";
        editor.TemplateItemList = TemplateItemList;
        editor.ResizeMode = CuteEditor.EditorResizeMode.None;
        editor.Height = 300;

        editor.EmptyAlternateText = EmptyAlternateText.ForceAdd;
        editor.EnableStripScriptTags = false;
        editor.EnableStripIframeTags = false;
        editor.BreakElement = BreakElement.P;

        editor.EditorWysiwygModeCss = string.Format("/css/site.css?v={0},{1}", ConfigurationManager.AppSettings.Get("CSSVersion"), ConfigurationManager.AppSettings.Get("FontsUrl"));
        editor.PreviewModeCss = string.Format("/css/site.css?v={0},{1}", ConfigurationManager.AppSettings.Get("CSSVersion"), ConfigurationManager.AppSettings.Get("FontsUrl"));
        editor.EditorBodyStyle = "background-color:#FFFFFF; background-image:none; width:auto; height:auto;";

        /*CuteEditor.ToolControl toolctrl = editor.ToolControls["CssClass"];

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

    protected void btnSave_Click(object sender, EventArgs e)
    {
        using (SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]))
        {
            string strcmd = string.Empty;
            
            string f = "temp";
            if (!string.IsNullOrEmpty(hfTempFolder.Text))
                f = hfTempFolder.Text;

            if (f.Contains("temp")) // Add New Content
            {
                //strcmd = " declare @HTMLId int declare @ContentId int declare @Zone_ID int ";
                strcmd = " declare @HTMLId int declare @ContentId int ";
                strcmd += " insert into html(html,timestamp,editor) values('<h1>Header text</h1><h2>Input your title text here</h2><p>Consectetur adipiscing elit. Fusce rhoncus, lectus eu varius pulvinar, sem sapien viverra dui, ut lacinia urna neque in diam. Aliquam est massa, consequat et facilisis non, euismod tincidunt diam. Aliquam erat volutpat. Morbi suscipit dignissim leo, id commodo nulla posuere id. Sed in velit lorem. Quisque egestas fermentum mi, id placerat lacus sodales quis. Suspendisse potenti. Ut gravida pellentesque leo, et viverra risus iaculis vel. Aliquam erat volutpat. Etiam laoreet nulla eu massa commodo quis sagittis nunc gravida. Duis sagittis nisi vitae risus varius sagittis. In at sem ipsum. Pellentesque id ornare urna. Lorem ipsum.</p>', GETDATE(), @LoggedInID) ";
                strcmd += " select @HTMLId = @@IDENTITY ";
                strcmd += " insert into content(name, control, param, language) values(@name, 'content', @HTMLId, @lang) ";
                strcmd += " select @ContentId = @@IDENTITY ";
                //strcmd += " select @Zone_ID = id from zones where name='content' ";
                strcmd += " if @Priority = 0 ";
                strcmd += "     select @Priority = isnull(max(Priority), 0) + 1 from Pages_Content_Zone where Page_ID=@Page_ID and Zone_ID=@Zone_ID ";
                strcmd += " else ";
                strcmd += " begin ";
                strcmd += "     declare @p int ";
                strcmd += "     select @p = Priority from Pages_Content_Zone where Page_ID = @Page_ID and Zone_ID = @Zone_ID and Priority = @Priority ";
                strcmd += "     if @@ROWCOUNT > 0 ";
                strcmd += "         update Pages_Content_Zone set Priority = Priority + 1 where Page_ID = @Page_ID and Zone_ID = @Zone_ID and Priority >= @Priority ";
                strcmd += " end ";
                strcmd += " insert into Pages_Content_Zone(Page_ID, Content_ID, Zone_ID, Priority) values(@Page_ID, @ContentId, @Zone_ID, @Priority) ";
                strcmd += " select @HTMLId ";
            }
            /*else // Edit Content
            {
                strcmd = " update PopupMessages set name = @name, html = @html where id = @id ";
                strcmd += " update Content set name = case when @name is null then name else 'PopupMessages - ' + @name end where param=@id and control='popupmessages' ";
                strcmd += " select @id ";
            }*/

            SqlCommand sqlComm = new SqlCommand(strcmd, sqlConn);
            sqlComm.Parameters.AddWithValue("@name", txtName.Text.Trim());
            //sqlComm.Parameters.AddWithValue("@html", txtContent.Text.Trim());
            if (f.Contains("temp")) // Add New Content
            {
                sqlComm.Parameters.AddWithValue("@lang", Language);
                sqlComm.Parameters.AddWithValue("@Page_ID", Session["PageID"].ToString());
                sqlComm.Parameters.AddWithValue("@LoggedInID", LoggedInID);
                //sqlComm.Parameters.AddWithValue("@Zone_ID", 1); // Content Zone

                string[] Parameters = hfContentParameters.Text.Split(',');    // Index 0: ZoneId, 1: Priority

                int ZoneId = 0;
                int.TryParse(Parameters[0], out ZoneId);

                sqlComm.Parameters.AddWithValue("@Zone_ID", ZoneId);

                int Priority = 0;
                int.TryParse(Parameters[1], out Priority);

                sqlComm.Parameters.AddWithValue("@Priority", Priority);
            }
            /*else // Edit Content
            {
                int id = 0;
                int.TryParse(f, out id);
                sqlComm.Parameters.AddWithValue("@id", id);
            }*/

            sqlConn.Open();

            sqlComm.ExecuteNonQuery();

            sqlConn.Close();
            
        }

        //Response.Redirect(Request.RawUrl);

        //Response.Redirect(LanguagePrefix + "/" + SEO);
        //Response.Redirect("/" + SEO);
        Response.Redirect(CMSHelper.GetSeoWithLanguagePrefix());
    }

    /*protected void btnEdit_Click(object sender, EventArgs e)
    {
        int id = 0;
        int.TryParse(hfTempFolder.Text, out id);
        
        DataTable dt = new DataTable();
        using (SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]))
        {
            SqlDataAdapter dapt = new SqlDataAdapter("PopupMessagesSelect", sqlConn);
            dapt.SelectCommand.CommandType = CommandType.StoredProcedure;
            dapt.SelectCommand.Parameters.AddWithValue("@id", id);
            dapt.Fill(dt);
        }

        if (dt.Rows.Count > 0)
        {
            txtName.Text = dt.Rows[0]["name"].ToString();
            //txtContent.Text = dt.Rows[0]["html"].ToString();
        }
        else
        {
            txtName.Text = "";
            //txtContent.Text = "";
        }
    }*/

    /*protected void btnCancel_Click(object sender, EventArgs e)
    {
        hfTempFolder.Text = "";
    }*/

    private void BuildClickOnceButton(WebControl ctl, HtmlGenericControl panel)
    {
        System.Text.StringBuilder sbValid = new System.Text.StringBuilder();
        sbValid.Append("if (typeof(Page_ClientValidate) == 'function') { ");
        sbValid.Append("if (Page_ClientValidate('" + (ctl as Button).ValidationGroup + "') == false) { Page_BlockSubmit = false; return false; }} ");
        sbValid.Append("document.getElementById('" + ctl.ClientID + "').style.display = 'none';");
        sbValid.Append("document.getElementById('" + panel.ClientID + "').style.display = 'block';");
        //sbValid.Append("document.getElementById('" + ctl.ClientID + "').disabled = true;");
        //GetPostBackEventReference obtains a reference to a client-side script function that causes the server to post back to the page. 
        //sbValid.Append(Page.ClientScript.GetPostBackEventReference(ctl, ""));
        //sbValid.Append(";");
        ctl.Attributes.Add("onclick", sbValid.ToString());
    }
}