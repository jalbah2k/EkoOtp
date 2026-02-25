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

public partial class Controls_DocumentsManager_DocumentsManager : System.Web.UI.UserControl
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
            //    string url = "/login?r=" + SEO;
            //    Response.Redirect(url);
            //}

            return _LoggedInID;
        }
    }

    private DataTable dtDocuments
    {
        get
        {
            return (DataTable)ViewState["dtDocuments"];
        }
        set
        {
            ViewState["dtDocuments"] = value;
        }
    }

    private DataTable dtDocumentsToBeDeleted
    {
        get
        {
            return (DataTable)ViewState["dtDocumentsToBeDeleted"];
        }
        set
        {
            ViewState["dtDocumentsToBeDeleted"] = value;
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

    public Controls_DocumentsManager_DocumentsManager()
    {
    }

    public Controls_DocumentsManager_DocumentsManager(string par)
    {
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //BuildClickOnceButton(btnSave, divSubmitted);

            BindEverything();
            
            DataTable dtP = new DataTable();
            dtP.Columns.Add("id", typeof(int));
            dtP.Columns.Add("Name", typeof(string));
            dtP.Columns.Add("Filename", typeof(string));
            dtP.Columns.Add("Mime", typeof(string));
            dtDocuments = dtP;
            
            hfSort.Value = "";

            DataTable dtPD = new DataTable();
            dtPD.Columns.Add("Filename", typeof(string));
            dtDocumentsToBeDeleted = dtPD;

            /*if (Session["LoggedInID"] != null)
            {
                if (Permissions.Get(int.Parse(Session["LoggedInID"].ToString()), int.Parse(Session["PageID"].ToString())) > 1)
                //if(Session["LoggedInID"].ToString()=="1")
                {*/
                    btnDocumentsAdd.Visible = true;
                /*}
            }*/
        }
    }

    private void BindEverything()
    {
        DataTable dt = new DataTable();
        
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]))
        {
            string sqlstr = "select id,name from groups where (id in (select group_id from users_groups_access where user_id=" + LoggedInID + " and access_level>1)) or " + LoggedInID + "=1 order by name";
            SqlDataAdapter dapt = new SqlDataAdapter(sqlstr, conn);
            dapt.Fill(dt);
        }
        
        ddlGroup.DataSource = dt;
        ddlGroup.DataValueField = "id";
        ddlGroup.DataTextField = "name";
        ddlGroup.DataBind();
        ddlGroup.Items.Insert(0, new ListItem("Select Group", ""));
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
                strcmd += " insert into DocumentGroups(name, listname, language, groupid, sortbyname) values(@name, @title, @language, @groupid, 0) ";
                strcmd += " select @GalleryId = @@IDENTITY ";
                strcmd += " insert into content(name, control, param, language) values('Documents - ' + @name, 'documents', @GalleryId, @language) ";
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
                strcmd = " update DocumentGroups set name = @name, listname = @title, groupid = @groupid where id = @id ";
                strcmd += " select @id ";
            }

            SqlCommand sqlComm = new SqlCommand(strcmd, sqlConn);
            sqlComm.Parameters.AddWithValue("@name", txtName.Text.Trim());
            sqlComm.Parameters.AddWithValue("@title", txtTitle.Text.Trim());
            sqlComm.Parameters.AddWithValue("@groupid", ddlGroup.SelectedValue);
            if (f.Contains("temp")) // Add New Gallery
            {
                sqlComm.Parameters.AddWithValue("@language", Language);
                sqlComm.Parameters.AddWithValue("@Page_ID", Session["PageID"].ToString());
                //sqlComm.Parameters.AddWithValue("@Zone_ID", 1); // Content Zone

                string[] Parameters = hfDocParameters.Text.Split(',');    // Index 0: ZoneId, 1: Priority

                int ZoneId = 0;
                int.TryParse(Parameters[0], out ZoneId);

                sqlComm.Parameters.AddWithValue("@Zone_ID", ZoneId);

                int Priority = 0;
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

            if (dtDocuments.Rows.Count > 0 
                || dtDocumentsToBeDeleted.Rows.Count > 0            //this line it's becuase was failing the delete when only one itme left
                )
            {
                strcmd = string.Empty;
                int id = 0;

                if (int.TryParse(f, out id)) // Edit Gallery
                {
                    strcmd = " delete from Documents where groupid=@GalleryId ";
                }

                string DocName = string.Empty;

                SortDocuments();
                
                foreach (DataRow dr in dtDocuments.Rows)
                {
                    try
                    {
                        RepeaterItem item = GetRepeaterItemById(dr["id"].ToString());
                        TextBox txtDocName = (TextBox)item.FindControl("txtDocName");
                        if (txtDocName != null)
                            DocName = txtDocName.Text.Trim().Replace("'", "’");
                        else
                            DocName = string.Empty;
                    }
                    catch
                    {
                        DocName = string.Empty;
                    }


                    strcmd += " insert into Documents(name, filename, groupid, mime, priority) values('" + DocName + "', '" + dr["Filename"].ToString().Replace("'", "''")
                        + "', @GalleryId, '" + dr["Mime"].ToString() + "', (select isnull(max(Priority), 0) + 1 from Documents where groupid=@GalleryId)) ";
                }
                strcmd += "  exec Report.ContentLogModifications 'documents', @GalleryId, @userid ";

                sqlComm = new SqlCommand(strcmd, sqlConn);
                sqlComm.Parameters.AddWithValue("@GalleryId", GalleryId);
                sqlComm.Parameters.AddWithValue("@userid", Session["LoggedInID"].ToString());

                sqlComm.ExecuteNonQuery();
            }
            
            sqlConn.Close();
            
            // Delete files pending to be deleted
            if (dtDocumentsToBeDeleted.Rows.Count > 0)
            {
                foreach (DataRow dr in dtDocumentsToBeDeleted.Rows)
                {
                    try
                    {
                        if (File.Exists(dr["Filename"].ToString()))
                            File.Delete(dr["Filename"].ToString());
                    }
                    catch { }
                }
            }
            
            string tempDir = HttpContext.Current.Request.MapPath(string.Format("~/Data/Documents/{0}/", f));

            try
            {
                if (Directory.Exists(tempDir))
                {
                    if (f.Contains("temp")) // Add New Gallery
                    {
                        string GalleryDir = HttpContext.Current.Request.MapPath(string.Format("~/Data/Documents/{0}/", GalleryId));

                        if (Directory.Exists(GalleryDir))
                            Directory.Move(GalleryDir, GalleryDir + "_OLD");
                        Directory.Move(tempDir, GalleryDir);

                        SetProtection( GalleryId);
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

    private void SetProtection(int GalleryId)
    {
        using (SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]))
        {
            DataTable dt = new DataTable();
            SqlDataAdapter dapt = new SqlDataAdapter("select id, name, private from Groups where id=@id", sqlConn);
            dapt.SelectCommand.Parameters.AddWithValue("@id", ddlGroup.SelectedValue);
            dapt.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                string docPath = HttpContext.Current.Request.MapPath("/Data/Documents/" + GalleryId);

                if ((bool)dt.Rows[0]["private"])
                {
                    if (!Directory.Exists(docPath))
                        Directory.CreateDirectory(docPath);

                    if (!File.Exists(docPath + @"/web.config"))
                        CMSHelper.CreatePrivateConfigFile(dt.Rows[0]["name"].ToString(), docPath, dt.Rows[0]["id"].ToString());
                }
                else
                {
                    if (File.Exists(docPath + @"/web.config"))
                        File.Delete(docPath + @"/web.config");
                }
            }
        }
    }

    private RepeaterItem GetRepeaterItemById(string id)
    {
        foreach (RepeaterItem item in repDocuments.Items)
        {
            if ((item.FindControl("btnDeleteDocument") as ImageButton).CommandArgument == id)
                return item;
        }

        return null;
    }
    
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        int id = 0;
        int.TryParse(hfTempFolder.Text, out id);

        dtDocuments.Rows.Clear();
        using (SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]))
        {
            SqlDataAdapter dapt = new SqlDataAdapter("select id, name, filename, mime from Documents where groupid=@GalleryId order by priority", sqlConn);
            dapt.SelectCommand.Parameters.AddWithValue("@GalleryId", id);
            dapt.Fill(dtDocuments);
        }
        dtDocumentsToBeDeleted.Rows.Clear();

        hfSort.Value = GetIdList();
        BindDocuments();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        string f = "temp";
        if (!string.IsNullOrEmpty(hfTempFolder.Text))
            f = hfTempFolder.Text;

        if (f.Contains("temp"))
        {
            string tempDir = HttpContext.Current.Request.MapPath(string.Format("~/Data/Documents/{0}/", f));

            try
            {
                if (Directory.Exists(tempDir))
                    Directory.Delete(tempDir, true);
            }
            catch { }
        }

        hfTempFolder.Text = "";

        dtDocuments.Rows.Clear();
        hfSort.Value = "";
        BindDocuments();
        
        dtDocumentsToBeDeleted.Rows.Clear();
    }

    public void fileup(object o, UploaderEventArgs e)
    {
        string f = "temp";
        if (!string.IsNullOrEmpty(hfTempFolder.Text))
            f = hfTempFolder.Text;
        string tempDir = HttpContext.Current.Request.MapPath(string.Format("~/Data/Documents/{0}/", f));

        if (!Directory.Exists(tempDir))
            Directory.CreateDirectory(tempDir);

        string _filename = e.FileName.Replace("&amp;", "and").Replace("&", "and").Replace("%", "").Replace("*", "").Replace(":", "").Replace(",", "").Replace("#", "No.");
        //string bigfile = tempDir + e.FileName;
        string bigfile = tempDir + _filename;

        if (File.Exists(bigfile))
            File.Delete(bigfile);

        //e.MoveTo(bigfile);
        e.CopyTo(bigfile);

        DataRow[] delRow;
        if (dtDocumentsToBeDeleted.Rows.Count > 0)
        {
            delRow = dtDocumentsToBeDeleted.Select("Filename='" + bigfile + "'");
            if (delRow.Length > 0)
            {
                dtDocumentsToBeDeleted.Rows.Remove(delRow[0]);
            }
        }

        SortDocuments();
        
        DataRow dr = dtDocuments.NewRow();

        int id = 1;

        if (dtDocuments.Rows.Count > 0)
        {
            object MaxId = dtDocuments.Compute("MAX(ID)", "");
            if (MaxId != null)
                id = Convert.ToInt32(MaxId) + 1;
        }

        dr["id"] = id;
        dr["Name"] = Path.GetFileNameWithoutExtension(e.FileName);
        dr["Filename"] = _filename;
        dr["Mime"] = Path.GetExtension(e.FileName);
        dtDocuments.Rows.Add(dr);

        hfSort.Value = GetIdList();
    }

    public void refresh(object o, UploaderEventArgs[] e)
    {
        BindDocuments();
    }

    private void BindDocuments()
    {
        SortDocuments();
        repDocuments.DataSource = dtDocuments;
        repDocuments.DataBind();
    }

    private string GetIdList()
    {
        string IdList = string.Empty;

        foreach (DataRow dr in dtDocuments.Rows)
        {
            IdList += dr["id"].ToString() + ",";
        }
        IdList = IdList.TrimEnd(',');

        return IdList;
    }

    private void SortDocuments()
    {
        int itemsCount = 0;
        
        string[] IdList = null;
        if (!string.IsNullOrEmpty(hfSort.Value))
        {
            IdList = hfSort.Value.Split(',');
            itemsCount = IdList.Length;
        }

        if (IdList != null && dtDocuments.Rows.Count == itemsCount && hfSort.Value != GetIdList())
        {
            DataTable dt = dtDocuments.Clone();
            
            foreach (string id in IdList)
            {
                DataRow[] dr = dtDocuments.Select("id = " + id);
                dt.ImportRow(dr[0]);
            }

            dtDocuments = dt;
        }
    }

    protected void repDocuments_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            DataRowView dr = (DataRowView)e.Item.DataItem;

            /*TextBox txtDocName = (TextBox)e.Item.FindControl("txtDocName");
            txtDocName.Text = dr["name"].ToString();*/

            Image ib = (Image)e.Item.FindControl("imgStatus");
            if (ib.ToolTip.ToLower() == ".doc" || ib.ToolTip.ToLower() == ".docx")
                ib.ImageUrl = "/images/icons/types/doc.png";
            else if (ib.ToolTip.ToLower() == ".pdf")
                ib.ImageUrl = "/images/icons/types/pdf.png";
            else if (ib.ToolTip.ToLower() == ".ppt" || ib.ToolTip.ToLower() == ".pptx")
                ib.ImageUrl = "/images/icons/types/ppt.png";
            else if (ib.ToolTip.ToLower() == ".txt")
                ib.ImageUrl = "/images/icons/types/txt.png";
            else if (ib.ToolTip.ToLower() == ".xls" || ib.ToolTip.ToLower() == ".xlsx")
                ib.ImageUrl = "/images/icons/types/xls.png";
            else if (ib.ToolTip.ToLower() == ".zip" || ib.ToolTip.ToLower() == ".rar")
                ib.ImageUrl = "/images/icons/types/zip.png";
            else if (ib.ToolTip.ToLower() == ".gif")
                ib.ImageUrl = "/images/icons/types/gif.png";
            else if (ib.ToolTip.ToLower() == ".jpg" || ib.ToolTip.ToLower() == ".jpeg")
                ib.ImageUrl = "/images/icons/types/jpg.png";
            else if (ib.ToolTip.ToLower() == ".png")
                ib.ImageUrl = "/images/icons/types/png.png";
        }
    }
    protected void repDocuments_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "delete")
        {
            string f = "temp";
            if (!string.IsNullOrEmpty(hfTempFolder.Text))
                f = hfTempFolder.Text;

            try
            {
                string tempDir = HttpContext.Current.Request.MapPath(string.Format("~/Data/Documents/{0}/", f));
                string Filename = dtDocuments.Rows[e.Item.ItemIndex]["Filename"].ToString();

                if (f.Contains("temp")) // Add New Gallery
                {
                    if (File.Exists(tempDir + Filename))
                        File.Delete(tempDir + Filename);
                }

                int id = 0;
                if (int.TryParse(f, out id))    // Edit Gallery
                {
                    DataRow dr = dtDocumentsToBeDeleted.NewRow();
                    dr["Filename"] = tempDir + Filename;
                    dtDocumentsToBeDeleted.Rows.Add(dr);
                }

                dtDocuments.Rows.RemoveAt(e.Item.ItemIndex);
                hfSort.Value = GetIdList();

                BindDocuments();
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