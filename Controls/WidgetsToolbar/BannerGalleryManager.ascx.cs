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

public partial class Controls_BannerGalleryManager_BannerGalleryManager : System.Web.UI.UserControl
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

    private DataTable dtBanners
    {
        get
        {
            return (DataTable)ViewState["dtBanners"];
        }
        set
        {
            ViewState["dtBanners"] = value;
        }
    }

    private DataTable dtBannersToBeDeleted
    {
        get
        {
            return (DataTable)ViewState["dtBannersToBeDeleted"];
        }
        set
        {
            ViewState["dtBannersToBeDeleted"] = value;
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

    public Controls_BannerGalleryManager_BannerGalleryManager()
    {
    }

    public Controls_BannerGalleryManager_BannerGalleryManager(string par)
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
            dtP.Columns.Add("Caption", typeof(string));
            dtP.Columns.Add("URL", typeof(string));
            dtBanners = dtP;
            
            hfSort.Value = "";

            DataTable dtPD = new DataTable();
            dtPD.Columns.Add("Filename", typeof(string));
            dtBannersToBeDeleted = dtPD;

            /*if (Session["LoggedInID"] != null)
            {
                if (Permissions.Get(int.Parse(Session["LoggedInID"].ToString()), int.Parse(Session["PageID"].ToString())) > 1)
                //if(Session["LoggedInID"].ToString()=="1")
                {*/
                    btnBannerGalleryAdd.Visible = true;
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
                strcmd += " insert into BannerGallery(name, language, group_id) values(@name, @language, 1) ";
                strcmd += " select @GalleryId = @@IDENTITY ";
                strcmd += " insert into content(name, control, param, language) values('BannerGallery - ' + @name, 'banners', @GalleryId, @language) ";
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
                strcmd = " update BannerGallery set name = @name where id = @id ";
                strcmd += " select @id ";
            }

            SqlCommand sqlComm = new SqlCommand(strcmd, sqlConn);
            sqlComm.Parameters.AddWithValue("@name", txtName.Text.Trim());
            if (f.Contains("temp")) // Add New Gallery
            {
                sqlComm.Parameters.AddWithValue("@language", Language);
                sqlComm.Parameters.AddWithValue("@Page_ID", Session["PageID"].ToString());
                //sqlComm.Parameters.AddWithValue("@Zone_ID", 1); // Content Zone

                string[] Parameters = hfBannerParameters.Text.Split(',');    // Index 0: ZoneId, 1: Priority

                int ZoneId = 0;
                int.TryParse(Parameters[0], out ZoneId);

                sqlComm.Parameters.AddWithValue("@Zone_ID", ZoneId);
                
                int Priority = 0;
                //int.TryParse(hfBannerPriority.Text, out Priority);
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

            if (dtBanners.Rows.Count > 0)
            {
                strcmd = string.Empty;
                int id = 0;

                if (int.TryParse(f, out id)) // Edit Gallery
                {
                    strcmd = " delete from Banners where Gallery=@GalleryId ";
                }

                string BannerName = string.Empty;
                string Caption = string.Empty;
                string URL = string.Empty;

                SortBanners();
                
                foreach (DataRow dr in dtBanners.Rows)
                {
                    try
                    {
                        RepeaterItem item = GetRepeaterItemById(dr["id"].ToString());
                        TextBox txtBannerName = (TextBox)item.FindControl("txtBannerName");
                        if (txtBannerName != null)
                            BannerName = txtBannerName.Text.Trim().Replace("'", "’");
                        else
                            BannerName = string.Empty;
                    }
                    catch
                    {
                        BannerName = string.Empty;
                    }
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
                        TextBox txtURL = (TextBox)item.FindControl("txtURL");
                        if (txtURL != null)
                            URL = txtURL.Text.Trim().Replace("'", "’");
                        else
                            URL = string.Empty;
                    }
                    catch
                    {
                        URL = string.Empty;
                    }

                    //strcmd += " insert into Banners(name, filename, groupid, caption, mime, priority) values('" + dr["Name"].ToString() + "', '" + dr["Filename"].ToString() + "', @GalleryId, '" + Caption + "', '" + dr["Mime"].ToString() + "', " + dr["id"].ToString() + ") ";
                    strcmd += " insert into Banners(BannerName, BannerLink, Gallery, BannerStatus, BannerFileLocation, [target], name, caption, BannerPriority) values('" + dr["Filename"].ToString() + "', '" + URL + "', @GalleryId, 'A', '/Uploads/Banners/', 0, '" + BannerName + "', '" + Caption + "', (select isnull(max(BannerPriority), 0) + 1 from Banners where Gallery=@GalleryId)) ";
                }
                strcmd += "  exec Report.ContentLogModifications 'banners', @GalleryId, @userid ";

                sqlComm = new SqlCommand(strcmd, sqlConn);
                sqlComm.Parameters.AddWithValue("@GalleryId", GalleryId);
                sqlComm.Parameters.AddWithValue("@userid", Session["LoggedInID"].ToString());

                sqlComm.ExecuteNonQuery();
            }
            
            sqlConn.Close();
            
            // Delete files pending to be deleted
            if (dtBannersToBeDeleted.Rows.Count > 0)
            {
                foreach (DataRow dr in dtBannersToBeDeleted.Rows)
                {
                    try
                    {
                        if (File.Exists(dr["Filename"].ToString()))
                            File.Delete(dr["Filename"].ToString());
                    }
                    catch { }
                }
            }

            string tempDir = HttpContext.Current.Request.MapPath(string.Format("~/Uploads/Banners/{0}/", f));

            try
            {
                if (Directory.Exists(tempDir))
                {
                    if (f.Contains("temp")) // Add New Gallery
                    {
                        string GalleryDir = HttpContext.Current.Request.MapPath(string.Format("~/Uploads/Banners/{0}/", GalleryId));

                        if (Directory.Exists(GalleryDir))
                            Directory.Move(GalleryDir, GalleryDir + "_OLD");
                        Directory.Move(tempDir, GalleryDir);
                    }
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
        foreach (RepeaterItem item in dlBanners.Items)
        {
            if ((item.FindControl("btnDeleteBanner") as ImageButton).CommandArgument == id)
                return item;
        }

        return null;
    }
    
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        int id = 0;
        int.TryParse(hfTempFolder.Text, out id);

        dtBanners.Rows.Clear();
        
        using (SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]))
        {
            SqlDataAdapter dapt = new SqlDataAdapter("select BannerID as id, name, BannerName as filename, caption, BannerLink as url from Banners where Gallery=@GalleryId order by BannerPriority", sqlConn);
            dapt.SelectCommand.Parameters.AddWithValue("@GalleryId", id);
            dapt.Fill(dtBanners);
        }
        dtBannersToBeDeleted.Rows.Clear();

        hfSort.Value = GetIdList();
        BindBanners();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        //string tempDir = HttpContext.Current.Request.MapPath("~/Banners/temp/");
        string f = "temp";
        if (!string.IsNullOrEmpty(hfTempFolder.Text))
            f = hfTempFolder.Text;

        if (f.Contains("temp"))
        {
            string tempDir = HttpContext.Current.Request.MapPath(string.Format("~/Uploads/Banners/{0}/", f));

            try
            {
                if (Directory.Exists(tempDir))
                    Directory.Delete(tempDir, true);
            }
            catch { }
        }

        hfTempFolder.Text = "";

        dtBanners.Rows.Clear();
        hfSort.Value = "";
        BindBanners();
        
        dtBannersToBeDeleted.Rows.Clear();
    }

    public void fileup(object o, UploaderEventArgs e)
    {
        //string tempDir = HttpContext.Current.Request.MapPath("~/Banners/temp/");
        string f = "temp";
        if (!string.IsNullOrEmpty(hfTempFolder.Text))
            f = hfTempFolder.Text;
        string tempDir = HttpContext.Current.Request.MapPath(string.Format("~/uploads/banners/{0}/", f));

        if (!Directory.Exists(tempDir))
            Directory.CreateDirectory(tempDir);

        string bigfile = tempDir + e.FileName;

        if (File.Exists(bigfile))
            File.Delete(bigfile);

        e.MoveTo(bigfile);
        //e.CopyTo(bigfile);

        SortBanners();
        
        DataRow dr = dtBanners.NewRow();

        int id = 1;

        if (dtBanners.Rows.Count > 0)
        {
            //id = Convert.ToInt32(dtBanners.Rows[dtBanners.Rows.Count - 1]["id"]) + 1;

            object MaxId = dtBanners.Compute("MAX(ID)", "");
            if (MaxId != null)
                id = Convert.ToInt32(MaxId) + 1;
        }

        dr["id"] = id;
        dr["Name"] = Path.GetFileNameWithoutExtension(e.FileName);
        dr["Filename"] = e.FileName;
        dtBanners.Rows.Add(dr);

        hfSort.Value = GetIdList();
    }

    public void refresh(object o, UploaderEventArgs[] e)
    {
        BindBanners();
    }

    private void BindBanners()
    {
        SortBanners();
        dlBanners.DataSource = dtBanners;
        dlBanners.DataBind();
    }

    private string GetIdList()
    {
        string IdList = string.Empty;

        foreach (DataRow dr in dtBanners.Rows)
        {
            IdList += dr["id"].ToString() + ",";
        }
        IdList = IdList.TrimEnd(',');

        return IdList;
    }

    private void SortBanners()
    {
        int itemsCount = 0;
        
        string[] IdList = null;
        if (!string.IsNullOrEmpty(hfSort.Value))
        {
            IdList = hfSort.Value.Split(',');
            itemsCount = IdList.Length;
        }

        if (IdList != null && dtBanners.Rows.Count == itemsCount && hfSort.Value != GetIdList())
        {
            DataTable dt = dtBanners.Clone();
            
            foreach (string id in IdList)
            {
                DataRow[] dr = dtBanners.Select("id = " + id);
                dt.ImportRow(dr[0]);
            }

            dtBanners = dt;
        }
    }

    protected void dlBanners_ItemDataBound1(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            DataRowView dr = (DataRowView)e.Item.DataItem;

            System.Web.UI.WebControls.Image Banner = (System.Web.UI.WebControls.Image)e.Item.FindControl("Banner");
            Banner.ImageUrl = "~/Uploads/Banners/" + hfTempFolder.Text + "/" + dr["Filename"].ToString();

            TextBox txtBannerName = (TextBox)e.Item.FindControl("txtBannerName");
            txtBannerName.Text = dr["name"].ToString();

            TextBox txtCaption = (TextBox)e.Item.FindControl("txtCaption");
            txtCaption.Text = dr["caption"].ToString();

            TextBox txtURL = (TextBox)e.Item.FindControl("txtURL");
            txtURL.Text = dr["URL"].ToString();
        }
    }
    protected void dlBanners_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "delete")
        {
            string f = "temp";
            if (!string.IsNullOrEmpty(hfTempFolder.Text))
                f = hfTempFolder.Text;

            try
            {
                string tempDir = HttpContext.Current.Request.MapPath(string.Format("~/Uploads/Banners/{0}/", f));
                string Filename = dtBanners.Rows[e.Item.ItemIndex]["Filename"].ToString();

                if (f.Contains("temp")) // Add New Gallery
                {
                    if (File.Exists(tempDir + Filename))
                        File.Delete(tempDir + Filename);
                }

                int id = 0;
                if (int.TryParse(f, out id))    // Edit Gallery
                {
                    DataRow dr = dtBannersToBeDeleted.NewRow();
                    dr["Filename"] = tempDir + Filename;
                    dtBannersToBeDeleted.Rows.Add(dr);
                }

                dtBanners.Rows.RemoveAt(e.Item.ItemIndex);
                hfSort.Value = GetIdList();

                BindBanners();
            }
            catch { }
        }
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