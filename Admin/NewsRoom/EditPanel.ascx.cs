#define WIDGET
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Text;
using System.Drawing.Imaging;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;
using CuteEditor;


public partial class Admin_NewsRoom_EditPanel : System.Web.UI.UserControl
{

    protected void Page_Load(object sender, EventArgs e)
    {
        ShowErrorMessage("", false);
        if (!IsPostBack)
        {
            ////DataSet dt = new DataSet();
            ////SqlDataAdapter da = new SqlDataAdapter("select * from languages select * from newsroomtypes", _connection);
            ////da.SelectCommand.CommandType = CommandType.Text;
            ////da.Fill(dt);

            ////ddlLang.DataSource = dt.Tables[0];
            ////ddlLang.DataTextField = "name";
            ////ddlLang.DataValueField = "id";
            ////ddlLang.DataBind();

            ddlLang.Items.Add(Language);

            BindEditor(Editor1, 400);
            //BindEditor(Editor2, 200);

            //BindLayouts();
            BindCategories();
            BindStatus();
            BindPublsih();
            Load_Times();

            tbDate.Attributes.Add("readonly", "readonly");

            rbInternal.Attributes.Add("onclick", "checkExtInt_" + Language + "()");
            rbExternal.Attributes.Add("onclick", "checkExtInt_" + Language + "()");
            lnkDlete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this image?');");

            RequiredFieldValidator1.ValidationGroup = "publication_" + Language;
            RequiredFieldValidator2.ValidationGroup = "publication_" + Language;
            CustomValidator1.ValidationGroup = "publication_" + Language;
            imgSave.ValidationGroup = "publication_" + Language;
            ValidationSummary1.ValidationGroup = "publication_" + Language;
            rfvCategories.ValidationGroup = "publication_" + Language;

        }
    }

    //private void BindLayouts()
    //{
    //    DataTable dt = getTable("GetLayoutsByGroup", new SqlParameter("@userid", Session["LoggedInID"].ToString()));
    //    if (dt.Rows.Count > 0)
    //    {
    //        foreach (DataRow dr in dt.Rows)
    //        {
    //            if (dr["name"].ToString() == "Communications" || Session["LoggedInID"].ToString() == "1")
    //            {
    //                ddlLayout.Items.Clear();
    //                ddlLayout.Items.Add(new ListItem("Home", "4"));
    //                ddlLayout.Items.Add(new ListItem("Foundation ", "24"));
    //                ddlLayout.Items.Add(new ListItem("Research", "25"));
    //                break;
    //            }
    //            if (dr["name"].ToString() == "Foundation")
    //            {
    //                ddlLayout.Items.Add(new ListItem("Foundation ", "24"));
    //            }
    //            if (dr["name"].ToString() == "Research")
    //            {
    //                ddlLayout.Items.Add(new ListItem("Research ", "25"));
    //            }      
    //        }
    //    }
    //}

    private string SelectedCategories
    {
        get
        {
            string _categories = string.Empty;
            foreach (ListItem li in cblCategories.Items)
            {
                if (li.Selected)
                    _categories += li.Value + ",";
            }
            _categories = _categories.TrimEnd(',');

            return _categories;
        }
    }

    private void BindCategories()
    {
        DataTable dt = getTable("NewsCategoriesSelect");

        cblCategories.DataSource = dt;
        cblCategories.DataValueField = "id";
        cblCategories.DataTextField = "name";
        cblCategories.DataBind();
        cblCategories.SelectedValue = "1";
    }

    private void BindStatus()
    {
        ddlStatus.Items.Add(new ListItem("Pending", "P"));
        ddlStatus.Items.Add(new ListItem("Live", "L"));
    }
    private void BindPublsih()
    {
        ddlPublish.Items.Add(new ListItem("Public", "1"));
        ddlPublish.Items.Add(new ListItem("Members only", "3"));
        ddlPublish.Items.Add(new ListItem("Both", "2"));
    }

    public string Language
    {
        set { ViewState["Language"] = value; }
        get { return Convert.ToString(ViewState["Language"]); }
    }

    private void BindEditor(Editor editor, int height)
    {
        //string TemplateItemList = "Paste,PasteText,CssClass,Bold,Italic,Underline,|,JustifyLeft,JustifyCenter,JustifyRight,JustifyFull,|,InsertOrderedList,InsertUnorderedList,|,InsertLink,InsertImage,InsertFlash,InsertMedia,InsertDocument,YouTube,|,RemoveFormat,CleanCode";
        //string TemplateItemList = "FormatBlock,Paste,PasteText,Bold,Italic,Underline,|,JustifyLeft,JustifyCenter,JustifyRight,JustifyFull,|,InsertOrderedList,InsertUnorderedList,|,InsertLink,InsertImage,InsertFlash,InsertMedia,InsertDocument,YouTube,|,RemoveFormat,CleanCode";
        string TemplateItemList = "FormatBlock,Images,Paste,PasteText,|,Undo,Redo,|,Bold,Italic,Underline,|,JustifyLeft,JustifyCenter,JustifyRight,JustifyFull,|,InsertOrderedList,InsertUnorderedList,Indent,Outdent|,InsertLink,InsertImage,InsertFlash,InsertMedia,YouTube,|,Table,|,RemoveFormat,CleanCode";
        editor.TemplateItemList = TemplateItemList;
        editor.ResizeMode = CuteEditor.EditorResizeMode.None;
        editor.Height = height; // 200;

        editor.EmptyAlternateText = EmptyAlternateText.ForceAdd;
        editor.EnableStripScriptTags = false;
        editor.EnableStripIframeTags = false;
        editor.BreakElement = BreakElement.P;

        editor.EditorWysiwygModeCss = string.Format(ConfigurationManager.AppSettings.Get("EditorCss") + "?v={0},{1}", ConfigurationManager.AppSettings.Get("CSSVersion"), ConfigurationManager.AppSettings.Get("FontsUrl"));
        editor.PreviewModeCss = string.Format(ConfigurationManager.AppSettings.Get("EditorCss") + "?v={0},{1}", ConfigurationManager.AppSettings.Get("CSSVersion"), ConfigurationManager.AppSettings.Get("FontsUrl"));
        editor.EditorBodyStyle = "background-color:#FFFFFF; background-image:none; width:auto; height:auto;";

#if !WIDGET
        CuteEditor.ToolControl ronster = Editor1.ToolControls["Images"];
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
#else
        CuteEditor.ToolControl ronster = editor.ToolControls["Images"];
        if (ronster != null)
        {

            DataTable dt = new DataTable();
            string sqlstr = "ZoneControlList";
            SqlCommand sq = new SqlCommand(sqlstr);
            sq.Parameters.AddWithValue("@control", "AwardGallery");
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

#endif
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

            //dropdown.Items.Add("DIV", "<div>");
        }

    }

    public void clickadd()
    {
        NewsID = "";

        clear();
    }

    private void clear()
    {
        Session["NewsLinkID"] = null;

        tbTitle.Text = "";
        Editor1.Text = "";
        //Editor2.Text = "";
        txtShortDesc.Text = "";
        // EditorImage.Text = "";
        tbDate.Text = "";
        tbImgAltText.Text = "";

       // cblCategories.ClearSelection();
        cblCategories.SelectedValue = "1";


        FillDDLPages();
        ddlType.SelectedValue = "0";
        hlkFile.Text = "";


        imageName = string.Empty;
        //pnlUploadedPhoto.Visible = false;
        trCurrentImage.Visible = false;

        ddlStatus.ClearSelection();
        ddlPublish.ClearSelection();

        txtAuthor.Text = "";
        txtComments.Text = "";
        txtUrl.Text = "";

        cbWeekly.Checked = false;

        ddlStartTime.ClearSelection();
        tbDateLive.Text = "";
    }

    public string NewsID
    {
        set { ViewState["nid"] = value; }
        get { return Convert.ToString(ViewState["nid"]); }
    }

    string NewsroomFilesPath = "/data/NewsroomFiles/";

    protected string imageName
    {
        set { ViewState["imageName"] = value; }
        get { return ViewState["imageName"] == null ? string.Empty : ViewState["imageName"].ToString(); }
    }
    protected void UploadImage_Click(object sender, EventArgs e)
    {
        HttpPostedFile myFile = this.fuImage.PostedFile;
        if (myFile != null)
        {
            string fExten = Path.GetExtension(myFile.FileName).ToLower();

            if (fExten == ".jpg" || fExten == ".png" || fExten == ".bmp" || fExten == ".gif" || fExten == ".jpeg")
            {
                //----- Reformat image to .JPEG and save it onto the server under ImageCropper folder ------
                //  try
                {
                    //txtFile.Text = myFile.FileName;

                    Bitmap bitmap = new Bitmap(this.fuImage.PostedFile.InputStream);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        System.Drawing.Image newImage = System.Drawing.Image.FromStream(this.fuImage.PostedFile.InputStream);

                        //-- Create new image with resolution 150 X 150 
                        var cropBitmap = new Bitmap(bitmap.Width, bitmap.Height);

                        var cropGraph = Graphics.FromImage(cropBitmap);
                        cropGraph.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                        cropGraph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        cropGraph.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                        var imageRectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                        cropGraph.DrawImage(newImage, imageRectangle);

                        imageName = Guid.NewGuid().ToString(); // Path.GetFileNameWithoutExtension();
                        cropBitmap.Save(Server.MapPath("~/data/ImageCropper/") + imageName + ".jpg", ImageFormat.Jpeg);
                        cropGraph.Dispose();
                        cropBitmap.Dispose();
                        newImage.Dispose();

                        //-- Popup a subwindow and display the cropped image ---
                        string AppFolder = Server.MapPath("~/").TrimEnd('\\');
                        AppFolder = AppFolder.Substring(AppFolder.LastIndexOf('\\') + 1);

                        StringBuilder sb = new StringBuilder();
                        sb.Append("<script language ='javascript'>");
                        sb.Append("GB_showCenter('ImageCropper', '");
                        sb.Append("//cropperiweb.bluelemonmedia.com/Default.aspx");
                        sb.Append("?p=" + AppFolder + "/data");
                        sb.Append("&id=" + imageName);
                        sb.Append("&width=125&height=125");
                        sb.Append("', '420', '800'");
                        sb.Append(", cropperCallback_"); sb.Append(Language);
                        sb.Append(");");
                        sb.Append("</script>");


                        Type t = this.GetType();
                        if (!Page.ClientScript.IsClientScriptBlockRegistered(t, "PopupScript"))
                        {
                            Page.ClientScript.RegisterStartupScript(t, "PopupScript", sb.ToString());
                        }
                    }
                }
                //catch (Exception err)
                //{
                //    //txtFile.Text = "";
                //    string error = err.Message;
                //    // Response.Write(err.Message);
                //}
            }
        }
    }

    private void ShowErrorMessage(string msg, bool visible)
    {
        ErrorMessage.Text = msg;
        ErrorMessage.Visible = visible;
    }
    protected void CheckWordLimit(object sender, ServerValidateEventArgs args)
    {
        try
        {
            int number = 0;
            int minWords = 0;
            int maxWords = 0;

            string TextToValidate = string.Empty;
            string MyClass = string.Empty;

            CustomValidator cv = ((CustomValidator)sender);

            if (FindControl(cv.ControlToValidate).GetType().ToString() == "CuteEditor.Editor")
            {
                Editor ed = (Editor)FindControl(cv.ControlToValidate);
                TextToValidate = ed.PlainText;
                MyClass = Regex.Match(ed.CssClass, @"Count\[\d+,?\d*\]", System.Text.RegularExpressions.RegexOptions.IgnoreCase).Value;
            }

            if (FindControl(cv.ControlToValidate).GetType().ToString() == "System.Web.UI.WebControls.TextBox")
            {
                TextBox tb = (TextBox)FindControl(cv.ControlToValidate);
                TextToValidate = args.Value;
                MyClass = Regex.Match(tb.CssClass, @"Count\[\d+,?\d*\]", System.Text.RegularExpressions.RegexOptions.IgnoreCase).Value;
            }


            string[] countControl = Regex.Match(MyClass, @"\d+,?\d*", System.Text.RegularExpressions.RegexOptions.IgnoreCase).Value.Split(',');

            if (countControl.Length > 1)
            {
                minWords = Convert.ToInt32(countControl[0]);
                maxWords = Convert.ToInt32(countControl[1]);
            }
            else if (countControl.Length > 0)
            {
                maxWords = Convert.ToInt32(countControl[0]);
            }

            // Words validation
            /*string sPattern = @"(\s*[\w+\S]+\s*)";
            MatchCollection numWords = Regex.Matches(TextToValidate, sPattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            if (numWords.Count > 0)
            {
                number = numWords.Count;
            }*/

            // Characters validation
            number = TextToValidate.Length;

            if (number < minWords || (number > maxWords && maxWords != 0))
                args.IsValid = false;
            else
                args.IsValid = true;
        }
        catch { }
    }

    protected void imgSave_Click(object sender, EventArgs e)
    {
        Save();
        // Page.ClientScript.RegisterClientScriptBlock(GetType(), "close", "parent.parent.window.location.reload();", true);
    }

    public void Save()
    {
        if (!Page.IsValid)
        {
            ShowErrorMessage("Validation Error", true);
            return;
        }

        try
        {
            if (Session["NewsLinkID"] == null && ddlLang.SelectedValue == "2")
            {
                Page.ClientScript.RegisterClientScriptBlock(GetType(), "warning", "alert('Please submit first the news in English and then do it in French.');", true);
                return;
            }

            string shortDesc = txtShortDesc.Text;       // Editor2.Text;
            string description = Editor1.Text;

            if (shortDesc.Contains("alt=\"\"") || description.Contains("alt=\"\""))
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "NoAlt", "alert('There are some images with empty alternate text.')", true);
                //UpdatePanel UpdatePanel1 = (UpdatePanel)Page.FindControl("UpdatePanel1");
                //if (UpdatePanel1 != null)
                //    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "NoAlt", "alert('There are some images with empty alternate text.');", true);
                //else
                //    Page.ClientScript.RegisterStartupScript(GetType(), "NoAlt", "alert('There are some images with empty alternate text.')", true);
                return;
            }


            byte[] imageBytes = null;
            string MIMEType = string.Empty;

            /*UploadPicture(ref imageBytes, ref MIMEType);*/

            string sql = string.IsNullOrEmpty(NewsID) ? sqlInsert : sqlUpdate;
            List<SqlParameter> prms = new List<SqlParameter>();

            if (!string.IsNullOrEmpty(NewsID))
                prms.Add(new SqlParameter("@id", NewsID));

            prms.Add(new SqlParameter("@title", tbTitle.Text));
            prms.Add(new SqlParameter("@DetailsShort", txtShortDesc.Text));     // Editor2.Text));
            // prms.Add(new SqlParameter("@image", EditorImage.Text));
            prms.Add(new SqlParameter("@Details", Editor1.Text));
            prms.Add(new SqlParameter("@NewsDate", tbDate.Text));
            prms.Add(new SqlParameter("@Lang", ddlLang.SelectedValue));
            prms.Add(new SqlParameter("@Type", ddlType.SelectedValue));
            prms.Add(new SqlParameter("@Feature", cbFeature.Checked));
            prms.Add(new SqlParameter("@Redevelopment", cbRedevelopment.Checked));
            prms.Add(new SqlParameter("@Ignore", cbIgnore.Checked));
            prms.Add(new SqlParameter("@FeatureType", Convert.ToInt32(ddlFeatureType.SelectedValue.ToString())));
            prms.Add(new SqlParameter("@PhotoAltText", tbImgAltText.Text.Trim()));
            prms.Add(new SqlParameter("@Categories", SelectedCategories));

            prms.Add(new SqlParameter("@layout", cblCategories.SelectedValue));

            prms.Add(new SqlParameter("@Comments", txtComments.Text));
            prms.Add(new SqlParameter("@Author", txtAuthor.Text));
            prms.Add(new SqlParameter("@Status", ddlStatus.SelectedValue));
            prms.Add(new SqlParameter("@publish", ddlPublish.SelectedValue));
            prms.Add(new SqlParameter("@ExcludeWeeklyRoundup", cbWeekly.Checked));
            

            if (Session["NewsLinkID"] != null)
                prms.Add(new SqlParameter("@linkid", Session["NewsLinkID"].ToString()));


            if (!String.IsNullOrEmpty(tbDateLive.Text))
                prms.Add(new SqlParameter("@GoLiveDate", tbDateLive.Text + " " + ddlStartTime.SelectedValue));
            else
                prms.Add(new SqlParameter("@GoLiveDate", DBNull.Value));


            /*if (imageBytes != null && !string.IsNullOrEmpty(MIMEType))
            {
                prms.Add(new SqlParameter("@Photo", imageBytes));
                prms.Add(new SqlParameter("@MIMEType", MIMEType));
            }*/

            if (!string.IsNullOrEmpty(imageName))
            {
                string FileName = Server.HtmlEncode(imageName + ".jpg");
                string imagePath = Server.MapPath("~/data/ImageCropper/") + FileName;

                //File.Copy(imagePath, _baseLocation + FileName);
                FileStream fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);

                imageBytes = br.ReadBytes((Int32)fs.Length);

                br.Close();
                fs.Close();

                MIMEType = "image/jpeg";

                //---- Remove uploaded image files ---
                if (File.Exists(imagePath))
                    File.Delete(imagePath);

                string croppedImagePath = "C:\\WEBSITES\\Cropper\\Save\\" + FileName;
                if (File.Exists(croppedImagePath))
                    File.Delete(croppedImagePath);

                prms.Add(new SqlParameter("@Photo", imageBytes));
                prms.Add(new SqlParameter("@MIMEType", MIMEType));

                imageName = string.Empty;
            }

            if (fuFile.HasFile && ddlType.SelectedValue == "1")
            {
                try
                {
                    string filename = Path.GetFileName(fuFile.FileName);
                    fuFile.SaveAs(Server.MapPath(NewsroomFilesPath) + filename);

                    prms.Add(new SqlParameter("@filename", filename));
                }
                catch { }
            }

            if (ddlType.SelectedValue == "2")
            {
                if (rbExternal.Checked)
                {
                    prms.Add(new SqlParameter("@ExternalURL", txtUrl.Text));
                }
                else
                {
                    prms.Add(new SqlParameter("@seo", ddlPages.SelectedValue));
                }
            }


            Session["NewsLinkID"] = this.ProcessRecord(sql, prms);
            if (string.IsNullOrEmpty(NewsID))
                NewsID = Session["NewsLinkID"].ToString();

            PopulateFields(Session["NewsLinkID"].ToString());

        }
        catch (Exception ex){ ShowErrorMessage(ex.Message, true); }

        //Response.Write(Session["NewsLinkID"].ToString());
    }

    public void PopulateFields(string id)
    {
        NewsID = id;
        hlkFile.Text = "";
        //Response.Write("id: " + NewsID + "<br />");

        clear();
        //Response.Write(Session["NewsLinkID"].ToString());

        if (!string.IsNullOrEmpty(NewsID))
        {
            DataSet ds = getTables(sqlSelect, new SqlParameter("@id", NewsID));
            DataTable dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                DataRow rw = dt.Rows[0];
                tbTitle.Text = rw["title"].ToString();
                // EditorImage.Text = rw["Image"].ToString();
                //Editor2.Text = rw["DetailsShort"].ToString();
                txtShortDesc.Text = rw["DetailsShort"].ToString();
                Editor1.Text = rw["Details"].ToString();
                ////ddlLang.ClearSelection();
                ////ddlLang.Items.FindByValue(rw["lang"].ToString()).Selected = true;

                try { tbDateLive.Text = ((DateTime)rw["GoLiveDate"]).ToString("yyyy/MM/dd"); }
                catch { tbDateLive.Text = ""; }

                if (!String.IsNullOrEmpty(rw["starttime"].ToString()))
                {
                    try { ddlStartTime.SelectedValue = rw["starttime"].ToString(); }
                    catch { }
                }

                tbImgAltText.Text = rw["PhotoAltText"].ToString();

                ddlType.ClearSelection();
                ddlType.Items.FindByValue(rw["type"].ToString()).Selected = true;

                //ddlLayout.ClearSelection();
                //ddlLayout.Items.FindByValue(rw["layout"].ToString()).Selected = true;

                ddlFeatureType.ClearSelection();
                ddlFeatureType.Items.FindByValue(rw["featuretype"].ToString()).Selected = true;

                //iconchanged();
                if (!String.IsNullOrEmpty(rw["NewsDate"].ToString()))
                    tbDate.Text = ((DateTime)rw["NewsDate"]).ToString("yyyy-MM-dd");

                cbFeature.Checked = Convert.ToBoolean(rw["feature"]);

                if (rw["filename"] != DBNull.Value && rw["filename"].ToString() != "")
                {
                    hlkFile.Text = rw["filename"].ToString();
                    hlkFile.NavigateUrl = NewsroomFilesPath + rw["filename"].ToString();
                }

                Session["NewsLinkID"] = rw["linkid"].ToString();

                FillDDLPages();

                ddlPages.ClearSelection();
                txtUrl.Text = "";
                if (rw["ExternalURL"].ToString().Length > 0)
                {
                    txtUrl.Text = rw["ExternalURL"].ToString();
                    rbExternal.Checked = true;
                }
                else if (rw["seo"].ToString().Length > 0)
                {
                    ddlPages.SelectedValue = rw["seo"].ToString();
                    rbInternal.Checked = true;
                }

                txtAuthor.Text = rw["Author"].ToString();
                txtComments.Text = rw["Comments"].ToString();
                ddlStatus.SelectedValue = rw["Status"].ToString();

                ddlPublish.SelectedValue = rw["PublishOn"].ToString();

                cbWeekly.Checked = Convert.ToBoolean(rw["ExcludeWeeklyRoundup"]);

                //pnlUploadedPhoto.Visible = false;
                trCurrentImage.Visible = false;
                if (rw["photo"] != DBNull.Value && rw["photo"].ToString().Length > 0)
                {
                    imgCurrentImage.ImageUrl = "/Controls/Newsroom/ThumbNail.ashx?PictureID=" + rw["id"].ToString() + "&maxsz=300";
                    trCurrentImage.Visible = true;
                }

               // cblCategories.ClearSelection();
                cblCategories.SelectedValue = "1";

                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    try { cblCategories.Items.FindByValue(dr["CategoryId"].ToString()).Selected = true; }
                    catch { }
                }
            }
        }
    }

    private void FillDDLPages()
    {
        DataSet dt = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter("SELECT p.* FROM dbo.Pages AS p INNER JOIN dbo.Pages_Group AS pg ON p.id = pg.Page_id", _connection);
        da.SelectCommand.CommandType = CommandType.Text;
        da.Fill(dt);

        ddlPages.DataSource = dt.Tables[0];
        ddlPages.DataBind();
        ddlPages.Items.Insert(0, "");
    }

    public void clickcancel()
    {
        Session["NewsLinkID"] = null;

        if (!string.IsNullOrEmpty(imageName))
        {
            string imagePath = Server.MapPath("~/data/ImageCropper/") + Server.HtmlEncode(imageName + ".jpg");

            //---- Remove uploaded image files ---
            if (File.Exists(imagePath))
                File.Delete(imagePath);

            imageName = string.Empty;
        }
    }

#region dal
    protected string _connection = ConfigurationManager.AppSettings.Get("CMServer");
    protected string sqlSelect = "NewsRoomGetOneAdmin";
    protected string sqlInsert = "NewsRoomInsert";
    protected string sqlUpdate = "NewsRoomUpdate";

    private DataSet getTables(string cmd, SqlParameter prm)
    {
        return getTables(cmd, new SqlParameter[] { prm });
    }

    private DataSet getTables(string cmd, SqlParameter[] prms)
    {
        DataSet dt = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter(cmd, _connection);
        da.SelectCommand.CommandType = CommandType.StoredProcedure;
        da.SelectCommand.Parameters.AddRange(prms);
        da.Fill(dt);
        return dt;
    }

    private DataSet getTables(string cmd)
    {
        return getTables(cmd, new SqlParameter[] { });
    }

    private DataTable getTable(string cmd, SqlParameter[] prms)
    {
        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter(cmd, _connection);
        da.SelectCommand.CommandType = CommandType.StoredProcedure;
        da.SelectCommand.Parameters.AddRange(prms);
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

    private DataTable getTable(string cmd)
    {
        return getTable(cmd, new SqlParameter[] { });
    }

    private DataTable getTable(string cmd, SqlParameter prm)
    {
        return getTable(cmd, new SqlParameter[] { prm });
    }

    private string ProcessRecord(string sql, SqlParameter[] prms)
    {
        SqlCommand cmd = new SqlCommand(sql, new SqlConnection(_connection));
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddRange(prms);
        cmd.Connection.Open();
        string ret = Convert.ToString(cmd.ExecuteScalar());
        cmd.Connection.Close();
        return ret;
    }

    private string ProcessRecord(string sql, List<SqlParameter> prms)
    {
        return ProcessRecord(sql, prms.ToArray());
    }

    private string ProcessRecord(string sql, SqlParameter[] prms, CommandType type)
    {
        SqlCommand cmd = new SqlCommand(sql, new SqlConnection(_connection));
        cmd.CommandType = type;
        cmd.Parameters.AddRange(prms);
        cmd.Connection.Open();
        string ret = Convert.ToString(cmd.ExecuteScalar());
        cmd.Connection.Close();
        return ret;
    }

    private string ProcessRecord(string sql, List<SqlParameter> prms, CommandType type)
    {
        return ProcessRecord(sql, prms.ToArray(), type);
    }

#endregion dal

    protected void lnkDlete_Click(object sender, EventArgs e)
    {
        List<SqlParameter> prms = new List<SqlParameter>();
        prms.Add(new SqlParameter("@id", NewsID));
       // ProcessRecord("update newsroom set Photo=null, MIMEType=null, PhotoAltText=null where id=@id", prms, CommandType.Text);
        ProcessRecord("delete NewsRoomImages where NewsroomId=@id", prms, CommandType.Text);
        PopulateFields(NewsID);
    }

    private void Load_Times()
    {
        DateTime time = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
        DateTime end = time.AddDays(1); ;

        ddlStartTime.Items.Clear();

        int Counter = 0;
        ddlStartTime.Items.Add(new ListItem("Select One", ""));


        time = time.AddMinutes(15);
        while (time < end && Counter < 96)
        {
            ddlStartTime.Items.Add(new ListItem(time.ToString("h:mm tt"), time.ToString("HH:mm")));
            time = time.AddMinutes(15);
            Counter++;
        }

        ddlStartTime.Items.Add(new ListItem("11:59 PM", "23:59"));
    }
}