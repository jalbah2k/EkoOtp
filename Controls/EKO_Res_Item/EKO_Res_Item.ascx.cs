#define NEW_PDF_API
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class EKO_Res_Item : System.Web.UI.UserControl
{
    public string Parameter;
   //string PDF_clientId = "e9fb2b79971847c58e3e47e95eb20530";           //OLD
    string PDF_clientId = "453c3fd9338642cb90580aeff195a519";

    
    public EKO_Res_Item() 
    {
        IsPdf = IsVideo = false;
    }
    public EKO_Res_Item(string p) 
    { 
        Parameter = p; 
        IsPdf = IsVideo = false;
        IsImage = false;
    }

    #region Properties
    protected string _seo = "";
    private string ResourceID
    {
        set { ViewState["ResourceID"] = value; }
        get
        {
            if (ViewState["ResourceID"] != null)
                return ViewState["ResourceID"].ToString();
            else
                return "0";
        }
    }

    public bool IsVideo
    {
        set { ViewState["IsVideo"] = value; }
        get { return Convert.ToBoolean(ViewState["IsVideo"]); }
    }
    public bool IsPdf
    {
        set { ViewState["IsPdf"] = value; }
        get { return Convert.ToBoolean(ViewState["IsPdf"]); }
    }
    public bool IsImage
    {
        set { ViewState["IsImage"] = value; }
        get { return Convert.ToBoolean(ViewState["IsImage"]); }
    }
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Page.RouteData.Values["seo"] != null)
            _seo = this.Page.RouteData.Values["seo"].ToString().ToLower();

        _linktopage = ConfigurationManager.AppSettings.Get("Resources.Page.Details");

        if (this.Page.RouteData.Values["id"] != null)
        {
            try
            {
                Populate();
            }
            catch
            {
            }
        }

        if (IsPostBack)
        {
            string resourceId = hfDownloadId.Value.Replace("btnDownload_", "");
            hfDownloadId.Value = "";
            if (!String.IsNullOrEmpty(resourceId))
                DownloadFile(resourceId);
        }
    }

    private void DownloadFile(string resourceId)
    {
        ResourceSearch res = new ResourceSearch();
        res.DownloadFile(resourceId, Session["LoggedInId"].ToString());
    }

    private void Populate()
    {
        string seo = "";
        if ((seo = this.Page.RouteData.Values["id"].ToString()) != "")
        {
            Populate(seo);
        }
    }

    private void Populate(string seo)
    {
        DataSet ds = new DataSet();

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("dbResources")))
        {
            SqlDataAdapter dapt = new SqlDataAdapter("res.Resource_Get", conn);
            dapt.SelectCommand.CommandType = CommandType.StoredProcedure;
            dapt.SelectCommand.Parameters.AddWithValue("@id", seo);
            dapt.SelectCommand.Parameters.AddWithValue("@userid", Session["LoggedInId"].ToString());
            dapt.SelectCommand.Parameters.AddWithValue("@Status", DBNull.Value);
            dapt.SelectCommand.Parameters.AddWithValue("@Show", DBNull.Value);

            dapt.Fill(ds);

            if (ds.Tables.Count == 0)
                return;

            #region Resource Details
            DataTable dt = ds.Tables[0];


            if (dt.Rows.Count == 1)
            {
                DataRow dr = dt.Rows[0];
                ResourceID = dr["id"].ToString();

                EKO_Breadcrumbs1.Visible = false;

                string title = dr["Title"].ToString();
                this.Page.Title += " - " + title;

                Literal litHeader = new Literal();
                StringBuilder header = new StringBuilder();
                header.Append("<p class=\"res-eyebrow\">Resource</p>");
                header.AppendFormat("<h1 class=\"res-title\">{0}</h1>", HttpUtility.HtmlEncode(title));
                if (dr["CreatedDate"] != DBNull.Value)
                {
                    header.AppendFormat("<p class=\"res-added\">Added {0}</p>",
                        Convert.ToDateTime(dr["CreatedDate"]).ToString("MMMM d, yyyy"));
                }
                litHeader.Text = header.ToString();
                plHeader.Controls.Add(litHeader);

                ResourceMetadata meta = LoadResourceMetadata(conn, ResourceID, dr);
                BindMetadataPanel(meta);

                string description = dr["Description"].ToString().Trim();
                if (description != "")
                {
                    Literal litDescription = new Literal();
                    litDescription.Text = "<div class=\"res-description\"><p class=\"res-section-label\">Description</p><div class=\"res-description-body\">" + description + "</div></div>";
                    plBody.Controls.Add(litDescription);
                }

                bool isFav = false;
                if (dr.Table.Columns.Contains("Favourite") && dr["Favourite"] != DBNull.Value)
                    isFav = Convert.ToBoolean(dr["Favourite"]);
                btnFavourite.Attributes["class"] = isFav ? "favBtn button res-btn-secondary favourite" : "favBtn button res-btn-secondary";
                btnFavourite.Attributes["values"] = ResourceID + Session["LoggedInId"].ToString() + "&" + ResourceID.Length;
                btnFavourite.Attributes["aria-pressed"] = isFav ? "true" : "false";

                StringBuilder script = new StringBuilder();

                if (dr.Table.Columns.Contains("IsDocument") && dr["IsDocument"] != DBNull.Value && Convert.ToBoolean(dr["IsDocument"]))
                {
                    script.Append(ShowDocument(dr));
                    EnableOpenInNewTab(dr["id"].ToString());
                }
                else if (dr["docType"].ToString() == "link")
                {
                    btnDownload.Visible = false;
                    hlkView.Visible = true;
                    hlkView.Text = "Open in a new tab";
                    hlkView.NavigateUrl = dr["URL"].ToString().Trim();
                    hlkView.Target = "_blank";
                    hlkView.CssClass = "button link res-btn-primary";
                    hlkView.Attributes["rel"] = "noopener noreferrer";
                    hlkView.Attributes["aria-label"] = "Open in a new tab (opens in a new window)";
                    hlkView.Attributes.Remove("onclick");
                }

                script.Append(Environment.NewLine + "<script src=\"/controls/EKO_Res_Item/LikeIt.js\"></script>" + Environment.NewLine);

                ((_Default)this.Page).InjectContent("Scripts", script.ToString(), false);


            }
            else
            {
                btnDownload.Visible = hlkView.Visible = btnFavourite.Visible = false;
            }
            #endregion

            #region All libraries where this Resource is available
            if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
            {
                Literal litLibs = new Literal();
                StringBuilder libs = new StringBuilder();
                libs.Append("<section class=\"res-card res-library-card\">");
                libs.Append("<h2 class=\"res-card-heading\">In this library</h2>");

                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    string libName = HttpUtility.HtmlEncode(dr["name"].ToString());
                    bool locked = false;
                    if (dr.Table.Columns.Contains("islocked") && dr["islocked"] != DBNull.Value)
                        locked = Convert.ToBoolean(dr["islocked"]);
                    string libSeo = dr.Table.Columns.Contains("seo") ? dr["seo"].ToString().ToLower() : "";

                    libs.Append("<div class=\"res-library-block\">");
                    if (locked)
                        libs.AppendFormat("<p class=\"res-library-name lock\">{0}</p>", libName);
                    else
                        libs.AppendFormat("<p class=\"res-library-name\"><a href=\"/reslibrary/{0}\">{1}</a></p>", HttpUtility.HtmlEncode(libSeo), libName);

                    string counts = FormatLibraryCounts(dr);
                    if (counts != "")
                        libs.AppendFormat("<p class=\"res-library-counts\">{0}</p>", counts);

                    if (!locked)
                        libs.AppendFormat("<a class=\"res-back-library\" href=\"/reslibrary/{0}\">&larr; Back to library</a>", HttpUtility.HtmlEncode(libSeo));
                    libs.Append("</div>");
                }

                libs.Append("</section>");
                litLibs.Text = libs.ToString();
                plLibrary.Controls.Add(litLibs);
            }
            #endregion

            #region Associated resources
            if (ds.Tables.Count > 2 && ds.Tables[2].Rows.Count > 0)
            {
                pnlAssociated.Visible = true;
                litAssociated.Text = "<h2 class=\"res-card-heading\">Related resources</h2>";

                repeaterResources.DataSource = ds.Tables[2];
                repeaterResources.DataBind();
            }
            else
            {
                pnlAssociated.Visible = false;
                litAssociated.Text = "";
            }
            #endregion

        }
    }

    protected void btnDownload_Click(object sender, EventArgs e)
    {
        ResourceSearch res = new ResourceSearch();
        res.DownloadFile(ResourceID, Session["LoggedInId"].ToString());
    }

    protected void btnFavourite_Click(object sender, EventArgs e)
    {
        //using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("dbResources")))
        //{
        //    List<SqlParameter> parms = new List<SqlParameter>();
        //    parms.Add(new SqlParameter("@ResourceId", ResourceID));
        //    parms.Add(new SqlParameter("@userid", Session["LoggedInId"].ToString()));

        //    MyDAL.ExecuteNonQuery("update ResourcesUsers_Link set Favourite = case when Favourite = 1 then 0 else 1 end where ResourceId=@ResourceId and UserId=@userid ",
        //            parms.ToArray(),
        //            CommandType.Text,
        //            conn);

        //    Populate();
        //    hfFavourite.Value = "";
        //}
    }

    private string ShowDocument( DataRow dr)
    {
        StringBuilder scrp = new StringBuilder();

        //lnkView.Visible = false;
        if ( (dr["mime"].ToString().ToLower().Contains("video") || dr["mime"].ToString().ToLower().Contains("audio"))  && Convert.ToBoolean(dr["Viewable"].ToString()))
        {
            IsVideo = true;

            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@ResourceId", ResourceID));
            parms.Add(new SqlParameter("@Status", 1));
            parms.Add(new SqlParameter("@Show", 1));
            parms.Add(new SqlParameter("@UserId", Session["LoggedInId"].ToString()));
            DataTable dtvideo = MyDAL_Resources.getSTable("Pie.Document_select", parms);

            if (dtvideo.Rows.Count > 0)
            {
                DataRow rw = dtvideo.Rows[0];
#if CODE64
                        Byte[] bytes = (Byte[])rw["content"];
                        string source = Convert.ToBase64String(bytes);
                        source = String.Format("data:{1};base64,{0}", source, rw["MIMEType"].ToString()) ;
#else
                string source = "";
                string width = "100%";
                string height = "auto";
                if (dr["mime"].ToString().ToLower().Contains("audio"))
                {
                    Byte[] bytes = (Byte[])rw["content"];
                    source = Convert.ToBase64String(bytes);
                    source = String.Format("data:{1};base64,{0}", source, rw["MIMEType"].ToString());
                    width = "575";
                    height = "45";
                }
                else
                {
                    ////string source = "/uploads/Data Management and Privacy Considerations for Private Pay Services (Kathryn Frelick March 10, 2020).mp4";
                    //source = ConfigurationManager.AppSettings["Resources.Video.Path"] + rw["path"].ToString().Replace("/Resources", "") + rw["FileName"].ToString();
                    source = rw["path"].ToString() + rw["FileName"].ToString();
                    source = source.Replace("//", "/");

                   // / Data / Resources / Data / 7652 / GMT20221025 - 140125_Recording_1760x900.mp4
                }
#endif
                litVideo.Text = String.Format("<div class='row row-video'><video poster=\"{0}\" id=\"video_{1}\" width=\"{4}\" height=\"{5}\" controls title=\"File preview\"><source src=\"{2}\" type=\"{3}\"><track default kind=\"subtitles\" srclang=\"en\" src=\"{6}\" /></video></div><br>",
                                        "", "video_" + rw["id"].ToString(), source, rw["MIMEType"].ToString(), width, height, source);
            }
        }
        else if ( dr["docType"].ToString() == ".pdf")
        {
            IsPdf = true;

#if NEW_PDF_API           
            //string scr = @"<script type='text/javascript'>
            //                document.addEventListener('adobe_dc_view_sdk.ready', function()
            //                {
            //                    var adobeDCView = new AdobeDC.View({ clientId: '{2}', divId: 'adobe-dc-view'});
            //                    adobeDCView.previewFile(
            //                    {
            //                        content: { location: { url: '{0}' } },
            //                        metaData: { fileName: '{1}'}
            //                    });
            //                });
            //                </script>";

            //https://acrobatservices.adobe.com/view-sdk-demo/index.html#/customize/FULL_WINDOW/Bodea%20Brochure.pdf

            string scr = @"<script type='text/javascript'>
                            document.addEventListener('adobe_dc_view_sdk.ready', function () {
                                var adobeDCView = new AdobeDC.View({ clientId: '{2}', divId: 'adobe-dc-view' });
                                adobeDCView.previewFile(
                                    {
                                        content: { location: { url: '{0}' } },
                                        metaData: { fileName: '{1}' }
                                    },
                                    { defaultViewMode: 'FIT_WIDTH', showAnnotationTools: false, showDownloadPDF: false,
                                      showPrintPDF: false }
                                );
                            });
                        </script>";


            scr = scr.Replace("'", "\"");
            scr = scr.Replace("{0}", String.Format("{0}://{1}/OpenFile.ashx?id={2}", Request.Url.Scheme, Request.Url.DnsSafeHost, dr["id"].ToString()));
            scr = scr.Replace("{1}", dr["FileName"].ToString());
            scr = scr.Replace("{2}", PDF_clientId);

            scrp.Append(Environment.NewLine + "<script src=\"https://documentservices.adobe.com/view-sdk/viewer.js\"></script>" + Environment.NewLine);
#else
            string scr = @"<script>
                            document.addEventListener('adobe_dc_view_sdk.ready', function(){
                                var adobeDCView = new AdobeDC.View({ clientId: '{2}', divId: 'adobe-dc-view'});
                                adobeDCView.previewFile({
                                    content: { location: { url: '{0}' } },
                                    metaData: { fileName: '{1}'}
                                    }, { });
                            });
                            </script>";   

            scr = scr.Replace("{0}", String.Format("{0}://{1}/OpenFile.ashx?id={2}", Request.Url.Scheme, Request.Url.DnsSafeHost, dr["id"].ToString()));
            scr = scr.Replace("{1}", dr["FileName"].ToString());
            scr = scr.Replace("{2}", PDF_clientId);

            scrp.Append(Environment.NewLine + "<script src=\"https://documentcloud.adobe.com/view-sdk/main.js\"></script>" + Environment.NewLine);

#endif
            scrp.Append(Environment.NewLine + scr + Environment.NewLine);
        }
        else if (dr["mime"].ToString().ToLower().Contains("image"))
        {
            IsImage = true;

            imgPhoto.ImageUrl = "/Controls/EKO_Res_Item/ThumbNail.ashx?PictureID=" + dr["id"].ToString() + "&maxsz=200";
            imgPhoto.AlternateText = dr["Title"].ToString() + " image";

        }
        return scrp.ToString();
    }

    private void EnableOpenInNewTab(string resourceId)
    {
        string url = "/OpenFile.ashx?id=" + resourceId;
        btn_newtab.NavigateUrl = url;
        btn_newtab.Target = "_blank";
        btn_newtab.Attributes["rel"] = "noopener noreferrer";
        btn_newtab.Attributes["aria-label"] = "Open in a new tab (opens in a new window)";
        btn_newtab.Attributes.Remove("onclick");
        btn_newtab.Visible = true;
    }

#region Associated Resources
    protected string _linktopage = "";

    protected void repeaterResources_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
            return;

        if ((DataRowView)e.Item.DataItem != null)
        {
            DataRowView rw = (DataRowView)e.Item.DataItem;
            PlaceHolder ph = (PlaceHolder)e.Item.FindControl("plContent");

            string title = rw["Title"].ToString();
            string format = FirstNonEmpty(rw, "ResourceFormatName", "FormatName");
            if (format == "")
                format = FormatFromDocType(rw.Row);
            string href = "/" + _linktopage + "/" + rw["id"].ToString().ToLower();
            string accessible = title;
            if (format != "")
                accessible += ", " + format;

            Literal litContent = new Literal();
            StringBuilder item = new StringBuilder();
            item.AppendFormat("<a class=\"res-related-title\" href=\"{0}\" aria-label=\"{1}\">{2}</a>",
                HttpUtility.HtmlAttributeEncode(href),
                HttpUtility.HtmlAttributeEncode(accessible),
                HttpUtility.HtmlEncode(title));
            if (format != "")
                item.AppendFormat("<span class=\"res-related-format\">{0}</span>", HttpUtility.HtmlEncode(format));
            litContent.Text = item.ToString();

            ph.Controls.Add(litContent);
        }
    }

    private class ResourceMetadata
    {
        public string Author = "";
        public string Published = "";
        public string Audience = "";
        public string Format = "";
    }

    private ResourceMetadata LoadResourceMetadata(SqlConnection conn, string resourceId, DataRow dr)
    {
        ResourceMetadata meta = new ResourceMetadata();
        meta.Author = FirstNonEmpty(dr, "ResourceAuthor", "Author");
        object publishedRaw = FirstValue(dr, "ResourcePublishedDate", "PublishedDate");
        meta.Published = FormatPublishedDate(publishedRaw);
        meta.Audience = FirstNonEmpty(dr, "ResourceAudience", "Audience", "AudienceNames");
        meta.Format = FirstNonEmpty(dr, "ResourceFormatName", "FormatName", "FormatLabel");

        if (conn.State != ConnectionState.Open)
            conn.Open();

        try
        {
            using (SqlCommand cmd = new SqlCommand(@"
SELECT r.Author, r.PublishedDate, r.Format AS FormatId
FROM dbo.Resources r
WHERE r.id = @id", conn))
            {
                cmd.Parameters.AddWithValue("@id", resourceId);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        if (meta.Author == "")
                            meta.Author = (reader["Author"] == DBNull.Value ? "" : reader["Author"].ToString().Trim());
                        if (meta.Published == "")
                            meta.Published = FormatPublishedDate(reader["PublishedDate"]);
                        string formatId = reader["FormatId"] == DBNull.Value ? "" : reader["FormatId"].ToString();
                        reader.Close();
                        if (meta.Format == "" && formatId != "")
                            meta.Format = LookupFormatName(conn, formatId);
                    }
                }
            }
        }
        catch
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.Resources WHERE id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", resourceId);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable t = new DataTable();
                        da.Fill(t);
                        if (t.Rows.Count == 1)
                        {
                            DataRow row = t.Rows[0];
                            if (meta.Author == "")
                                meta.Author = FirstNonEmpty(row, "Author");
                            if (meta.Published == "")
                                meta.Published = FormatPublishedDate(FirstValue(row, "PublishedDate"));
                            if (meta.Format == "")
                            {
                                string formatId = FirstNonEmpty(row, "Format");
                                int n;
                                if (int.TryParse(formatId, out n))
                                    meta.Format = LookupFormatName(conn, formatId);
                                else
                                    meta.Format = formatId;
                            }
                        }
                    }
                }
            }
            catch { }
        }

        if (meta.Audience == "")
            meta.Audience = LookupAudienceNames(conn, resourceId);

        if (meta.Format == "")
            meta.Format = FormatFromDocType(dr);

        return meta;
    }

    private static string LookupFormatName(SqlConnection conn, string formatId)
    {
        if (String.IsNullOrEmpty(formatId))
            return "";
        string[] sqls = new string[] {
            "SELECT name FROM dbo.ResourceFormats WHERE id = @id",
            "SELECT name FROM dbo.Formats WHERE id = @id"
        };
        foreach (string sql in sqls)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", formatId);
                    object name = cmd.ExecuteScalar();
                    if (name != null && name != DBNull.Value && name.ToString().Trim() != "")
                        return name.ToString().Trim();
                }
            }
            catch { }
        }
        return "";
    }

    private static string LookupAudienceNames(SqlConnection conn, string resourceId)
    {
        string[] sqls = new string[] {
            @"SELECT a.name FROM dbo.Resource_Audience_Link l
              INNER JOIN dbo.ResourceAudiences a ON a.id = l.AudienceId
              WHERE l.ResourceId = @id ORDER BY a.name",
            @"SELECT a.name FROM dbo.Resource_Audience_Link l
              INNER JOIN dbo.Audiences a ON a.id = l.AudienceId
              WHERE l.ResourceId = @id ORDER BY a.name"
        };
        foreach (string sql in sqls)
        {
            try
            {
                List<string> names = new List<string>();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", resourceId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string name = reader[0] == DBNull.Value ? "" : reader[0].ToString().Trim();
                            if (name != "")
                                names.Add(name);
                        }
                    }
                }
                if (names.Count > 0)
                    return String.Join(" · ", names.ToArray());
            }
            catch { }
        }
        return "";
    }

    private static string FormatFromDocType(DataRow dr)
    {
        string docType = FirstNonEmpty(dr, "docType", "FileExtension").ToLower();
        string mime = FirstNonEmpty(dr, "mime", "MIMEType").ToLower();

        if (docType == "link" || mime.Contains("html"))
            return "Web link";
        if (docType == ".pdf" || mime.Contains("pdf"))
            return "PDF Document";
        if (docType == ".doc" || docType == ".docx" || mime.Contains("word") || mime.Contains("msword"))
            return "Word Document";
        if (docType == ".xls" || docType == ".xlsx" || docType == ".csv" || mime.Contains("excel") || mime.Contains("spreadsheet"))
            return "Spreadsheet";
        if (docType == ".ppt" || docType == ".pptx" || mime.Contains("powerpoint") || mime.Contains("presentation"))
            return "Presentation";
        if (mime.Contains("audio") || docType == ".mp3" || docType == ".wav" || docType == ".m4a")
            return "Audio recording";
        if (mime.Contains("video") || docType == ".mp4" || docType == ".m4v" || docType == ".mov" || docType == ".avi")
            return "Video recording";
        if (docType != "")
            return docType.TrimStart('.').ToUpper() + " Document";
        return "";
    }

    private static string FormatPublishedDate(object publishedRaw)
    {
        if (publishedRaw == null || publishedRaw == DBNull.Value || String.IsNullOrWhiteSpace(publishedRaw.ToString()))
            return "";
        DateTime publishedDate;
        if (DateTime.TryParse(publishedRaw.ToString(), out publishedDate))
            return publishedDate.ToString("MMMM yyyy");
        return publishedRaw.ToString().Trim();
    }

    private void BindMetadataPanel(ResourceMetadata meta)
    {
        StringBuilder html = new StringBuilder();
        html.Append("<div class=\"res-meta\"><dl class=\"res-meta-list\">");
        AppendMetaField(html, "Author", meta.Author);
        AppendMetaField(html, "Published Date", meta.Published);
        AppendMetaField(html, "Audience", meta.Audience);
        AppendMetaField(html, "Format", meta.Format);
        html.Append("</dl></div>");

        if (html.ToString().IndexOf("<dt>") < 0)
            return;

        Literal litMeta = new Literal();
        litMeta.Text = html.ToString();
        plMeta.Controls.Add(litMeta);
    }

    private void BindMetadataPanel(DataRow dr)
    {
        ResourceMetadata meta = new ResourceMetadata();
        meta.Author = FirstNonEmpty(dr, "ResourceAuthor", "Author");
        meta.Published = FormatPublishedDate(FirstValue(dr, "ResourcePublishedDate", "PublishedDate"));
        meta.Audience = FirstNonEmpty(dr, "ResourceAudience", "Audience", "AudienceNames");
        meta.Format = FirstNonEmpty(dr, "ResourceFormatName", "FormatName", "FormatLabel");
        if (meta.Format == "")
            meta.Format = FormatFromDocType(dr);
        BindMetadataPanel(meta);
    }

    private static void AppendMetaField(StringBuilder sb, string label, string value)
    {
        if (String.IsNullOrWhiteSpace(value))
            return;
        sb.Append("<div class=\"res-meta-item\">");
        sb.AppendFormat("<dt>{0}</dt>", HttpUtility.HtmlEncode(label));
        sb.AppendFormat("<dd>{0}</dd>", HttpUtility.HtmlEncode(value.Trim()));
        sb.Append("</div>");
    }

    private static string FormatLibraryCounts(DataRow dr)
    {
        int categories = ToInt(dr, "CategoryCount");
        int resources = ToInt(dr, "ResourceCount");
        if (categories <= 0 && resources <= 0)
            return "";

        string catLabel = categories == 1 ? "category" : "categories";
        string resLabel = resources == 1 ? "resource" : "resources";
        return String.Format("{0} {1} · {2} {3}", categories, catLabel, resources, resLabel);
    }

    private static int ToInt(DataRow dr, string column)
    {
        if (!dr.Table.Columns.Contains(column) || dr[column] == DBNull.Value)
            return 0;
        int n;
        int.TryParse(dr[column].ToString(), out n);
        return n;
    }

    private static string FirstNonEmpty(DataRow dr, params string[] columns)
    {
        foreach (string column in columns)
        {
            if (!dr.Table.Columns.Contains(column) || dr[column] == DBNull.Value)
                continue;
            string value = dr[column].ToString().Trim();
            if (value != "")
                return value;
        }
        return "";
    }

    private static string FirstNonEmpty(DataRowView rw, params string[] columns)
    {
        return FirstNonEmpty(rw.Row, columns);
    }

    private static object FirstValue(DataRow dr, params string[] columns)
    {
        foreach (string column in columns)
        {
            if (dr.Table.Columns.Contains(column) && dr[column] != DBNull.Value)
                return dr[column];
        }
        return null;
    }
#endregion

}

