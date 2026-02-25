using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CuteWebUI;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Text;

public partial class Controls_PhotoGalleryAdd_PhotoGalleryAdd : System.Web.UI.UserControl
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

    private DataTable dtPhotos
    {
        get
        {
            return (DataTable)ViewState["dtPhotos"];
        }
        set
        {
            ViewState["dtPhotos"] = value;
        }
    }

    private DataTable dtPhotosToBeDeleted
    {
        get
        {
            return (DataTable)ViewState["dtPhotosToBeDeleted"];
        }
        set
        {
            ViewState["dtPhotosToBeDeleted"] = value;
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

    public Controls_PhotoGalleryAdd_PhotoGalleryAdd()
    {
    }

    public Controls_PhotoGalleryAdd_PhotoGalleryAdd(string par)
    {
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BuildClickOnceButton(btnSave, divSubmitted);
            
            DataTable dtP = new DataTable();
            dtP.Columns.Add("id", typeof(int));
            dtP.Columns.Add("Name", typeof(string));
            dtP.Columns.Add("Filename", typeof(string));
            dtP.Columns.Add("Mime", typeof(string));
            dtP.Columns.Add("Caption", typeof(string));
            dtP.Columns.Add("AltText", typeof(string));
            dtPhotos = dtP;
            
            hfSort.Value = "";

            DataTable dtPD = new DataTable();
            dtPD.Columns.Add("Filename", typeof(string));
            dtPhotosToBeDeleted = dtPD;

            /*if (Session["LoggedInID"] != null)
            {
                if (Permissions.Get(int.Parse(Session["LoggedInID"].ToString()), int.Parse(Session["PageID"].ToString())) > 1)
                //if(Session["LoggedInID"].ToString()=="1")
                {*/
                    btnPhotoGalleryAdd.Visible = true;
                /*}
            }*/
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

            if (f.Contains("temp")) // Add New Gallery
            {
                //strcmd = " declare @GalleryId int declare @ContentId int declare @Zone_ID int ";
                strcmd = " declare @GalleryId int declare @ContentId int ";
                strcmd += " insert into photogroups(name, title, language, groupid) values(@name, @name, @language, 1) ";
                strcmd += " select @GalleryId = @@IDENTITY ";
               //strcmd += " insert into content(name, control, param, language) values('Photos - ' + @name, 'photos', @GalleryId, @language) ";
                strcmd += " insert into content(name, control, param, language) values('Photos - ' + @name, 'PhotoNanogallery', @GalleryId, @language) ";
                strcmd += " select @ContentId = @@IDENTITY ";
                //strcmd += " select @Zone_ID = id from zones where name='content' ";
                //strcmd += " insert into Pages_Content_Zone(Page_ID, Content_ID, Zone_ID, Priority) values(@Page_ID, @ContentId, @Zone_ID, (select isnull(max(Priority), 0) + 1 from Pages_Content_Zone where Page_ID=@Page_ID and Zone_ID=@Zone_ID)) ";
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
                strcmd += " select @GalleryId ";
            }
            else // Edit Gallery
            {
                strcmd  = " update photogroups set name = @name, title = @name where id = @id ";
                strcmd += " select @id ";
            }

            SqlCommand sqlComm = new SqlCommand(strcmd, sqlConn);
            sqlComm.Parameters.AddWithValue("@name", txtName.Text.Trim());
            if (f.Contains("temp")) // Add New Gallery
            {
                sqlComm.Parameters.AddWithValue("@language", Language);
                sqlComm.Parameters.AddWithValue("@Page_ID", Session["PageID"].ToString());
                //sqlComm.Parameters.AddWithValue("@Zone_ID", 1); // Content Zone

                string[] Parameters = hfPhotoParameters.Text.Split(',');    // Index 0: ZoneId, 1: Priority

                int ZoneId = 0;
                int.TryParse(Parameters[0], out ZoneId);

                sqlComm.Parameters.AddWithValue("@Zone_ID", ZoneId);
                
                int Priority = 0;
                //int.TryParse(hfPhotoPriority.Text, out Priority);
                int.TryParse(Parameters[1], out Priority);

                sqlComm.Parameters.AddWithValue("@Priority", Priority);
            }
            else // Edit Gallery
            {
                int id = 0;
                int.TryParse(f, out id);
                sqlComm.Parameters.AddWithValue("@id", id);
            }

            sqlConn.Open();

            int GalleryId = Convert.ToInt32(sqlComm.ExecuteScalar());

            if (dtPhotos.Rows.Count > 0)
            {
                strcmd = string.Empty;
                int id = 0;

                if (int.TryParse(f, out id)) // Edit Gallery
                {
                    strcmd = " delete from photos where groupid=@GalleryId ";
                }

                string Caption = string.Empty;
                string AltText = string.Empty;

                SortPhotos();
                
                foreach (DataRow dr in dtPhotos.Rows)
                {
                    try
                    {
                        RepeaterItem item = GetRepeaterItemById(dr["id"].ToString());
                        TextBox txtCaption = (TextBox)item.FindControl("txtCaption");
                        if (txtCaption != null)
                            Caption = txtCaption.Text.Trim().Replace("'", "’");
                        else
                            Caption = string.Empty;
                    }
                    catch
                    {
                        Caption = string.Empty;
                    }
                    try
                    {
                        RepeaterItem item = GetRepeaterItemById(dr["id"].ToString());
                        TextBox txtAltText = (TextBox)item.FindControl("txtAltText");
                        if (txtAltText != null)
                            AltText = txtAltText.Text.Trim().Replace("'", "’");
                        else
                            AltText = string.Empty;
                    }
                    catch
                    {
                        AltText = string.Empty;
                    }

                    //strcmd += " insert into photos(name, filename, groupid, caption, mime, priority) values('" + dr["Name"].ToString() + "', '" + dr["Filename"].ToString() + "', @GalleryId, '" + Caption + "', '" + dr["Mime"].ToString() + "', " + dr["id"].ToString() + ") ";
                    strcmd += " insert into photos(name, filename, groupid, captionheader, caption, mime, priority) values('" + dr["Name"].ToString() + "', '" + dr["Filename"].ToString() + "', @GalleryId, '" + AltText + "', '" + Caption + "', '" + dr["Mime"].ToString() + "', (select isnull(max(Priority), 0) + 1 from Photos where groupid=@GalleryId)) ";
                }

                sqlComm = new SqlCommand(strcmd, sqlConn);
                sqlComm.Parameters.AddWithValue("@GalleryId", GalleryId);

                sqlComm.ExecuteNonQuery();
            }
            
            sqlConn.Close();
            
            // Delete files pending to be deleted
            if (dtPhotosToBeDeleted.Rows.Count > 0)
            {
                foreach (DataRow dr in dtPhotosToBeDeleted.Rows)
                {
                    try
                    {
                        if (File.Exists(dr["Filename"].ToString()))
                            File.Delete(dr["Filename"].ToString());
                    }
                    catch { }
                }
            }
            
            string tempDir = HttpContext.Current.Request.MapPath(string.Format("~/Data/Photos/{0}/", f));

            try
            {
                if (Directory.Exists(tempDir))
                {
                    if (f.Contains("temp")) // Add New Gallery
                    {
                        string GalleryDir = HttpContext.Current.Request.MapPath(string.Format("~/Data/Photos/{0}/", GalleryId));

                        if (Directory.Exists(GalleryDir))
                            Directory.Move(GalleryDir, GalleryDir + "_OLD");
                        Directory.Move(tempDir, GalleryDir);
                    }

                    WriteXMLFile(GalleryId.ToString());
                }
            }
            catch { }
        }

        //Response.Redirect(Request.RawUrl);

        //Response.Redirect(LanguagePrefix + "/" + SEO);
        //Response.Redirect("/" + SEO);
        Response.Redirect(CMSHelper.GetSeoWithLanguagePrefix());
    }

    private RepeaterItem GetRepeaterItemById(string id)
    {
        foreach (RepeaterItem item in dlPhotos.Items)
        {
            if ((item.FindControl("btnDeletePhoto") as ImageButton).CommandArgument == id)
                return item;
        }

        return null;
    }
    
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        int id = 0;
        int.TryParse(hfTempFolder.Text, out id);

        dtPhotos.Rows.Clear();
        
        using (SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]))
        {
            SqlDataAdapter dapt = new SqlDataAdapter("select id, name, filename, mime, caption, captionheader as alttext from photos where groupid=@GalleryId order by priority", sqlConn);
            dapt.SelectCommand.Parameters.AddWithValue("@GalleryId", id);
            dapt.Fill(dtPhotos);
        }
        dtPhotosToBeDeleted.Rows.Clear();

        hfSort.Value = GetIdList();
        BindPhotos();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        //string tempDir = HttpContext.Current.Request.MapPath("~/Data/Photos/temp/");
        string f = "temp";
        if (!string.IsNullOrEmpty(hfTempFolder.Text))
            f = hfTempFolder.Text;

        if (f.Contains("temp"))
        {
            string tempDir = HttpContext.Current.Request.MapPath(string.Format("~/Data/Photos/{0}/", f));

            try
            {
                if (Directory.Exists(tempDir))
                    Directory.Delete(tempDir, true);
            }
            catch { }
        }

        hfTempFolder.Text = "";

        dtPhotos.Rows.Clear();
        hfSort.Value = "";
        BindPhotos();
        
        dtPhotosToBeDeleted.Rows.Clear();
    }

    public void fileup(object o, UploaderEventArgs e)
    {
        //string tempDir = HttpContext.Current.Request.MapPath("~/Data/Photos/temp/");
        string f = "temp";
        if (!string.IsNullOrEmpty(hfTempFolder.Text))
            f = hfTempFolder.Text;
        string tempDir = HttpContext.Current.Request.MapPath(string.Format("~/Data/Photos/{0}/", f));

        if (!Directory.Exists(tempDir))
            Directory.CreateDirectory(tempDir);
        if (!Directory.Exists(tempDir + "thumbs\\"))
            Directory.CreateDirectory(tempDir + "thumbs\\");


        string bigfile = tempDir + e.FileName;

        if (File.Exists(bigfile))
            File.Delete(bigfile);

        e.MoveTo(bigfile);

        string newfilename;
        string newfilename2;
        ResizeFromFile(e, bigfile, out newfilename, out newfilename2, 1024, "");
        //ResizeFromFile(e, bigfile, out newfilename, out newfilename2, 75, "thumbs\\");
        ResizeFromFile(e, bigfile, out newfilename, out newfilename2, 150, 100, "thumbs\\");

        File.Delete(bigfile);

        SortPhotos();
        
        DataRow dr = dtPhotos.NewRow();

        int id = 1;

        if (dtPhotos.Rows.Count > 0)
        {
            //id = Convert.ToInt32(dtPhotos.Rows[dtPhotos.Rows.Count - 1]["id"]) + 1;

            object MaxId = dtPhotos.Compute("MAX(ID)", "");
            if (MaxId != null)
                id = Convert.ToInt32(MaxId) + 1;
        }

        dr["id"] = id;
        dr["Name"] = Path.GetFileNameWithoutExtension(newfilename).Replace("%20", " ");
        dr["Filename"] = newfilename2;
        dr["Mime"] = Path.GetExtension(newfilename);
        dtPhotos.Rows.Add(dr);

        hfSort.Value = GetIdList();
    }

    public void refresh(object o, UploaderEventArgs[] e)
    {
        BindPhotos();
    }

    private void BindPhotos()
    {
        SortPhotos();
        dlPhotos.DataSource = dtPhotos;
        dlPhotos.DataBind();
    }

    private string GetIdList()
    {
        string IdList = string.Empty;

        foreach (DataRow dr in dtPhotos.Rows)
        {
            IdList += dr["id"].ToString() + ",";
        }
        IdList = IdList.TrimEnd(',');

        return IdList;
    }

    private void SortPhotos()
    {
        int itemsCount = 0;
        
        string[] IdList = null;
        if (!string.IsNullOrEmpty(hfSort.Value))
        {
            IdList = hfSort.Value.Split(',');
            itemsCount = IdList.Length;
        }

        if (IdList != null && dtPhotos.Rows.Count == itemsCount && hfSort.Value != GetIdList())
        {
            DataTable dt = dtPhotos.Clone();
            
            foreach (string id in IdList)
            {
                DataRow[] dr = dtPhotos.Select("id = " + id);
                dt.ImportRow(dr[0]);
            }

            dtPhotos = dt;
        }
    }

    protected void dlPhotos_ItemDataBound1(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            DataRowView dr = (DataRowView)e.Item.DataItem;

            System.Web.UI.WebControls.Image Photo = (System.Web.UI.WebControls.Image)e.Item.FindControl("Photo");
            Photo.ImageUrl = "~/Data/Photos/" + hfTempFolder.Text + "/" + dr["Filename"].ToString();

            TextBox txtCaption = (TextBox)e.Item.FindControl("txtCaption");
            txtCaption.Text = dr["caption"].ToString();

            TextBox txtAltText = (TextBox)e.Item.FindControl("txtAltText");
            txtAltText.Text = dr["alttext"].ToString();
        }
    }
    protected void dlPhotos_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "delete")
        {
            string f = "temp";
            if (!string.IsNullOrEmpty(hfTempFolder.Text))
                f = hfTempFolder.Text;

            try
            {
                string tempDir = HttpContext.Current.Request.MapPath(string.Format("~/Data/Photos/{0}/", f));
                string Filename = dtPhotos.Rows[e.Item.ItemIndex]["Filename"].ToString();

                if (f.Contains("temp")) // Add New Gallery
                {
                    if (File.Exists(tempDir + Filename))
                        File.Delete(tempDir + Filename);
                    if (File.Exists(tempDir + "thumbs\\" + Filename))
                        File.Delete(tempDir + "thumbs\\" + Filename);
                }

                int id = 0;
                if (int.TryParse(f, out id))    // Edit Gallery
                {
                    DataRow dr = dtPhotosToBeDeleted.NewRow();
                    dr["Filename"] = tempDir + Filename;
                    dtPhotosToBeDeleted.Rows.Add(dr);

                    dr = dtPhotosToBeDeleted.NewRow();
                    dr["Filename"] = tempDir + "thumbs\\" + Filename;
                    dtPhotosToBeDeleted.Rows.Add(dr);
                }

                dtPhotos.Rows.RemoveAt(e.Item.ItemIndex);
                hfSort.Value = GetIdList();

                BindPhotos();
            }
            catch { }
        }
    }
    
    /*protected void dlPhotos_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            DataRowView dr = (DataRowView)e.Item.DataItem;

            System.Web.UI.WebControls.Image Photo = (System.Web.UI.WebControls.Image)e.Item.FindControl("Photo");
            Photo.ImageUrl = "~/Data/Photos/" + hfTempFolder.Text + "/" + dr["Filename"].ToString();

            TextBox txtCaption = (TextBox)e.Item.FindControl("txtCaption");
            txtCaption.Text = dr["caption"].ToString();

            TextBox txtAltText = (TextBox)e.Item.FindControl("txtAltText");
            txtAltText.Text = dr["alttext"].ToString();
        }
    }

    protected void dlPhotos_DeleteCommand(object source, DataListCommandEventArgs e)
    {
        string f = "temp";
        if (!string.IsNullOrEmpty(hfTempFolder.Text))
            f = hfTempFolder.Text;

        try
        {
            string tempDir = HttpContext.Current.Request.MapPath(string.Format("~/Data/Photos/{0}/", f));
            string Filename = dtPhotos.Rows[e.Item.ItemIndex]["Filename"].ToString();
            
            if (f.Contains("temp")) // Add New Gallery
            {
                if (File.Exists(tempDir + Filename))
                    File.Delete(tempDir + Filename);
                if (File.Exists(tempDir + "thumbs\\" + Filename))
                    File.Delete(tempDir + "thumbs\\" + Filename);
            }
            
            int id = 0;
            if (int.TryParse(f, out id))    // Edit Gallery
            {
                DataRow dr = dtPhotosToBeDeleted.NewRow();
                dr["Filename"] = tempDir + Filename;
                dtPhotosToBeDeleted.Rows.Add(dr);
                
                dr = dtPhotosToBeDeleted.NewRow();
                dr["Filename"] = tempDir + "thumbs\\" + Filename;
                dtPhotosToBeDeleted.Rows.Add(dr);
            }

            dtPhotos.Rows.RemoveAt(e.Item.ItemIndex);
            BindPhotos();
        }
        catch { }
    }*/
    
    private void ResizeFromFile(UploaderEventArgs e, string bigfile, out string newfilename, out string newfilename2, int MaxSideSize, string thumbsFolder)
    {
        System.Drawing.Image imgInput = System.Drawing.Image.FromFile(bigfile);
        ImageFormat fmtImageFormat = imgInput.RawFormat;

        #region Calculate new size
        int intNewWidth;
        int intNewHeight;

        //get image original width and height 
        int intOldWidth = imgInput.Width;
        int intOldHeight = imgInput.Height;

        //determine if landscape or portrait 
        int intMaxSide;

        if (thumbsFolder == "")
        {
            if (intOldWidth >= intOldHeight)
            {
                intMaxSide = intOldWidth;
            }
            else
            {
                intMaxSide = intOldHeight;
            }
        }
        else
        {
            if (intOldWidth >= intOldHeight)
            {
                intMaxSide = intOldHeight;
            }
            else
            {
                intMaxSide = intOldWidth;
            }
        }


        if (intMaxSide > MaxSideSize)
        {
            //set new width and height 
            double dblCoef = MaxSideSize / (double)intMaxSide;
            intNewWidth = Convert.ToInt32(dblCoef * intOldWidth);
            intNewHeight = Convert.ToInt32(dblCoef * intOldHeight);
        }
        else
        {
            intNewWidth = intOldWidth;
            intNewHeight = intOldHeight;
        }
        #endregion

        newfilename = "resized_" + e.FileName;
        newfilename2 = newfilename.Replace("%20", "-");

        //string newfile = Server.MapPath("~/Data/Photos/") + Session["docgroup"].ToString() + "\\" + thumbsFolder + newfilename2;
        //string newfile = Server.MapPath("~/Data/Photos/temp") + "\\" + thumbsFolder + newfilename2;
        string f = "temp";
        if (!string.IsNullOrEmpty(hfTempFolder.Text))
            f = hfTempFolder.Text;
        string newfile = Server.MapPath("~/Data/Photos/" + f) + "\\" + thumbsFolder + newfilename2;
        Bitmap bmpResized = new Bitmap(imgInput, intNewWidth, intNewHeight);
        bmpResized.Save(newfile, fmtImageFormat);
        imgInput.Dispose();
        bmpResized.Dispose();
    }

    private void ResizeFromFile(UploaderEventArgs e, string bigfile, out string newfilename, out string newfilename2, int maxWidth, int maxHeight, string thumbsFolder)
    {
        int intNewWidth;
        int intNewHeight;
        System.Drawing.Image imgInput = System.Drawing.Image.FromFile(bigfile);
        ImageFormat fmtImageFormat = imgInput.RawFormat;
        int intOldWidth = imgInput.Width;
        int intOldHeight = imgInput.Height;

        //      int intMaxSide;
        double dblCoef;

        intNewHeight = intOldHeight;
        intNewWidth = intOldWidth;

        if (intNewWidth > maxWidth)     //485 original value for maxWidth
        {
            dblCoef = intNewWidth / (double)maxWidth;
            intNewWidth = Convert.ToInt32(intNewWidth / dblCoef);
            intNewHeight = Convert.ToInt32(intNewHeight / dblCoef);
        }

        if (intNewHeight > maxHeight)   //364 original value for maxHeight
        {
            dblCoef = intNewHeight / (double)maxHeight;
            intNewWidth = Convert.ToInt32(intNewWidth / dblCoef);
            intNewHeight = Convert.ToInt32(intNewHeight / dblCoef);
        }

        if (intNewWidth > maxWidth)
        {
            dblCoef = intNewWidth / (double)maxWidth;
            intNewWidth = Convert.ToInt32(intNewWidth / dblCoef);
            intNewHeight = Convert.ToInt32(intNewHeight / dblCoef);
        }


        newfilename = "resized_" + e.FileName;
        newfilename2 = newfilename.Replace("%20", "-");

        //string newfile = Server.MapPath("~/Data/Photos/") + Session["docgroup"].ToString() + "\\" + thumbsFolder + newfilename2;
        string f = "temp";
        if (!string.IsNullOrEmpty(hfTempFolder.Text))
            f = hfTempFolder.Text;
        string newfile = Server.MapPath("~/Data/Photos/" + f) + "\\" + thumbsFolder + newfilename2;
        Bitmap bmpResized = new Bitmap(imgInput, intNewWidth, intNewHeight);
        bmpResized.Save(newfile, fmtImageFormat);
        imgInput.Dispose();
        bmpResized.Dispose();
    }

    private void WriteXMLFile(string GalleryId)
    {
        // Create a new XmlTextWriter instance
        XmlTextWriter writer = new XmlTextWriter(Server.MapPath("~/Data/Photos/" + GalleryId + "/gallery.xml"), Encoding.UTF8);

        // start writing!
        writer.WriteStartDocument();
        writer.WriteStartElement("simpleviewergallery");
        writer.WriteAttributeString("maxImageWidth", "700");
        writer.WriteAttributeString("maxImageHeight", "500");
        writer.WriteAttributeString("imageQuality", "80");
        writer.WriteAttributeString("thumbWidth", "75");
        writer.WriteAttributeString("thumbHeight", "75");
        writer.WriteAttributeString("thumbQuality", "90");
        writer.WriteAttributeString("useFlickr", "false");
        writer.WriteAttributeString("resizeOnImport", "true");
        writer.WriteAttributeString("cropToFit", "false");
        //writer.WriteAttributeString("galleryStyle", ddlMode.SelectedValue);
        writer.WriteAttributeString("galleryStyle", "MODERN");
        writer.WriteAttributeString("backgroundTransparent", "true");
        writer.WriteAttributeString("galleryWidth", "690");
        writer.WriteAttributeString("galleryHeight", "600");

        /*if (ddlMode.SelectedValue == "COMPACT")
        {
            writer.WriteAttributeString("thumbPosition", "BOTTOM");
            writer.WriteAttributeString("thumbColumns", "7");
            writer.WriteAttributeString("thumbRows", "1");
            writer.WriteAttributeString("frameWidth", "0");
            writer.WriteAttributeString("backgroundColor", "FFFFFF");
            writer.WriteAttributeString("imageScaleMode", "SCALE");

        }
        else*/
        {
            writer.WriteAttributeString("useFlash", "true");
            writer.WriteAttributeString("thumbNavStyle", "CLASSIC");
            writer.WriteAttributeString("thumbDropShadow", "true");
            //writer.WriteAttributeString("changeImageOnHover", cbHover.Checked.ToString().ToLower());
            writer.WriteAttributeString("changeImageOnHover", "false");
            writer.WriteAttributeString("changeCaptionOnHover", "true");
            writer.WriteAttributeString("mobileShowNav", "true");
            writer.WriteAttributeString("thumbNavPosition", "BOTTOM");
            writer.WriteAttributeString("imageScaleMode", "SCALE");
            writer.WriteAttributeString("imageTransitionType", "FADE");
            writer.WriteAttributeString("showImageNav", "HOVER");
            writer.WriteAttributeString("showBigPlayButton", "false");
            writer.WriteAttributeString("thumbFrameStyle", "SQUARE");
            writer.WriteAttributeString("useFixedLayout", "false");

        }

        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
        SqlDataAdapter dapt = new SqlDataAdapter("select * from photos where groupid=@ID order by priority", sqlConn);
        dapt.SelectCommand.Parameters.AddWithValue("@ID", GalleryId);
        DataSet ds = new DataSet();
        dapt.Fill(ds);

        DataTable dt = ds.Tables[0];

        foreach (DataRow dr in dt.Rows)
        {
            // Creating the <imageURL> elements
            writer.WriteStartElement("image");
            writer.WriteAttributeString("imageURL", "/Data/Photos/" + GalleryId + "/" + dr["filename"].ToString());
            writer.WriteAttributeString("thumbURL", "/Data/Photos/" + GalleryId + "/thumbs/" + dr["filename"].ToString());
            writer.WriteAttributeString("linkURL", "/Data/Photos/" + GalleryId + "/" + dr["filename"].ToString());
            writer.WriteAttributeString("linkTarget", "_blank");
            writer.WriteStartElement("caption");
            writer.WriteCData(dr["caption"].ToString());
            writer.WriteEndElement();
            writer.WriteEndElement();
        }

        writer.WriteEndElement();
        writer.WriteEndDocument();
        writer.Close();


    }

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