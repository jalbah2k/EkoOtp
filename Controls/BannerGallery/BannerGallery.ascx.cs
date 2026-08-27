using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

/// <summary>
/// Banner gallery user control.
///
/// Reads gallery settings from dbo.BannerGallery and the banners from dbo.Banners,
/// and renders them as a rotating gallery.
///
/// Declarative use (normal case in a Web Site project):
///     &lt;%@ Register Src="~/controls/BannerGallery.ascx" TagPrefix="blm" TagName="BannerGallery" %&gt;
///     &lt;blm:BannerGallery ID="ucBanners" runat="server" GalleryId="17" /&gt;
///
/// Dynamic use, passing the id through the constructor:
///     BannerGalleryControl ctl =
///         (BannerGalleryControl)LoadControl(typeof(BannerGalleryControl), new object[] { 17 });
///     phBanners.Controls.Add(ctl);
///
/// Either way GalleryId stays public, so the containing page can read or set it:
///     ucBanners.GalleryId = someId;
/// </summary>
public partial class BannerGallery : UserControl
{
    // =====================================================================
    // Settings the page can override
    // =====================================================================

    /// <summary>
    /// Name of the connection string in web.config for the database holding
    /// dbo.Banners and dbo.BannerGallery. Change this default to match the site.
    /// </summary>
    public string ConnectionStringName
    {
        get
        {
            object o = ViewState["ConnectionStringName"];
            return o == null ? "EkoOtpConnectionString" : (string)o;
        }
        set { ViewState["ConnectionStringName"] = value; }
    }

    /// <summary>
    /// Base folder for banner images, used when a row has no BannerFileLocation.
    /// The full path is {base}/{Gallery}/{BannerName} - /data/Banners/17/star.png
    /// </summary>
    public string ImageBasePath
    {
        get
        {
            object o = ViewState["ImageBasePath"];
            return o == null ? "/data/Banners" : (string)o;
        }
        set { ViewState["ImageBasePath"] = value; }
    }

    /// <summary>
    /// Milliseconds between slides when autoplay is on. There is no column for
    /// this, so it is a control-level setting.
    /// </summary>
    public int AutoplayInterval
    {
        get
        {
            object o = ViewState["AutoplayInterval"];
            return o == null ? 6000 : (int)o;
        }
        set { ViewState["AutoplayInterval"] = value; }
    }

    /// <summary>
    /// When false (default) the Body column is HTML-encoded and line breaks become
    /// &lt;br /&gt;. Set true only if Body is known to hold trusted HTML.
    /// </summary>
    public bool AllowHtmlInBody
    {
        get
        {
            object o = ViewState["AllowHtmlInBody"];
            return o == null ? false : (bool)o;
        }
        set { ViewState["AllowHtmlInBody"] = value; }
    }

    // =====================================================================
    // Gallery identity and resulting state
    // =====================================================================

    /// <summary>Id of the row in dbo.BannerGallery to render.</summary>
    public int GalleryId
    {
        get
        {
            object o = ViewState["GalleryId"];
            return o == null ? 0 : (int)o;
        }
        set { ViewState["GalleryId"] = value; }
    }

    /// <summary>True when this gallery is set to rotate on its own.</summary>
    public bool Autoplay
    {
        get
        {
            object o = ViewState["Autoplay"];
            return o == null ? false : (bool)o;
        }
        private set { ViewState["Autoplay"] = value; }
    }

    /// <summary>True when this gallery is set to randomise banner order.</summary>
    public bool Shuffle
    {
        get
        {
            object o = ViewState["Shuffle"];
            return o == null ? false : (bool)o;
        }
        private set { ViewState["Shuffle"] = value; }
    }

    /// <summary>Banners actually rendered. 0 means the control drew nothing.</summary>
    public int BannerCount
    {
        get
        {
            object o = ViewState["BannerCount"];
            return o == null ? 0 : (int)o;
        }
        private set { ViewState["BannerCount"] = value; }
    }

    // =====================================================================
    // Construction
    // =====================================================================

    public BannerGallery()
    {
    }

    public BannerGallery(string galleryId)
    {
        int id = 0;
        if(int.TryParse(galleryId, out id)) 
            GalleryId = id;
    }

    // =====================================================================
    // Lifecycle
    // =====================================================================

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindGallery();
        }
    }

    /// <summary>
    /// Reads the gallery from the database and rebinds. Call this directly if the
    /// page sets GalleryId after Page_Load has already run.
    /// </summary>
    public void BindGallery()
    {
        pnlGallery.Visible = false;
        BannerCount = 0;

        if (GalleryId <= 0)
        {
            return;
        }

        LoadGallerySettings();

        List<BannerItem> banners = LoadBanners();

        // Nothing active for this gallery: render no markup at all rather than an
        // empty shell, so whatever sits below moves up.
        if (banners.Count == 0)
        {
            return;
        }

        if (Shuffle && banners.Count > 1)
        {
            ShuffleList(banners);
        }

        BannerCount = banners.Count;

        rptBanners.DataSource = banners;
        rptBanners.DataBind();

        // Arrows and dots are pointless for a single banner.
        //
        // Test a local bool here, not phControls.Visible. The Visible getter walks
        // up the parent chain and returns false while any ancestor is invisible -
        // and pnlGallery is still false at this point, so reading it back would
        // never be true and the dots would never bind.
        bool showControls = banners.Count > 1;

        phControls.Visible = showControls;

        if (showControls)
        {
            rptDots.DataSource = banners;
            rptDots.DataBind();
        }

        pnlGallery.Visible = true;
    }

    // =====================================================================
    // Data access
    // =====================================================================

    private string GetConnectionString()
    {
        ConnectionStringSettings cs = ConfigurationManager.ConnectionStrings["CMServer"];

        if (cs == null)
        {
            throw new ConfigurationErrorsException(
                "BannerGallery: connection string '" + ConnectionStringName +
                "' was not found in web.config.");
        }

        return cs.ConnectionString;
    }

    private void LoadGallerySettings()
    {
        const string sql =
            "SELECT [autoplay], [shuffle] " +
            "FROM dbo.BannerGallery " +
            "WHERE [ID] = @GalleryId";

        // Default to a static, unshuffled gallery if the gallery row is missing.
        Autoplay = false;
        Shuffle = false;

        using (SqlConnection cn = new SqlConnection(GetConnectionString()))
        using (SqlCommand cmd = new SqlCommand(sql, cn))
        {
            cmd.Parameters.Add("@GalleryId", SqlDbType.Int).Value = GalleryId;

            cn.Open();

            using (SqlDataReader rd = cmd.ExecuteReader(CommandBehavior.SingleRow))
            {
                if (rd.Read())
                {
                    Autoplay = !rd.IsDBNull(0) && rd.GetBoolean(0);
                    Shuffle = !rd.IsDBNull(1) && rd.GetBoolean(1);
                }
            }
        }
    }

    private List<BannerItem> LoadBanners()
    {
        List<BannerItem> list = new List<BannerItem>();

        // BannerStatus must be 'A' (active), and the banner must be inside its
        // display window where StartDate / EndDate are set.
        const string sql =
            "SELECT b.[BannerID], b.[BannerName], b.[BannerFileLocation], b.[Gallery], " +
            "       b.[BannerLink], b.[target], b.[AltText], b.[caption], " +
            "       b.[Title], b.[Body], " +
            "       b.[ButtonText], b.[ButtonTitle], b.[ButtonLink], " +
            "       b.[ButtonText1], b.[ButtonTitle1], b.[ButtonLink1], " +
            "       b.[PresentationClass] " +
            "FROM dbo.Banners b " +
            "WHERE b.[Gallery] = @GalleryId " +
            "  AND b.[BannerStatus] = 'A' " +
            "  AND (b.[StartDate] IS NULL OR b.[StartDate] <= @Now) " +
            "  AND (b.[EndDate]   IS NULL OR b.[EndDate]   >= @Now) " +
            "ORDER BY ISNULL(b.[BannerPriority], 2147483647) ASC, b.[BannerID] ASC";

        using (SqlConnection cn = new SqlConnection(GetConnectionString()))
        using (SqlCommand cmd = new SqlCommand(sql, cn))
        {
            cmd.Parameters.Add("@GalleryId", SqlDbType.Int).Value = GalleryId;
            cmd.Parameters.Add("@Now", SqlDbType.DateTime).Value = DateTime.Now;

            cn.Open();

            using (SqlDataReader rd = cmd.ExecuteReader())
            {
                int oBannerId = rd.GetOrdinal("BannerID");
                int oBannerName = rd.GetOrdinal("BannerName");
                int oFileLocation = rd.GetOrdinal("BannerFileLocation");
                int oGallery = rd.GetOrdinal("Gallery");
                int oBannerLink = rd.GetOrdinal("BannerLink");
                int oTarget = rd.GetOrdinal("target");
                int oAltText = rd.GetOrdinal("AltText");
                int oCaption = rd.GetOrdinal("caption");
                int oTitle = rd.GetOrdinal("Title");
                int oBody = rd.GetOrdinal("Body");
                int oButtonText = rd.GetOrdinal("ButtonText");
                int oButtonTitle = rd.GetOrdinal("ButtonTitle");
                int oButtonLink = rd.GetOrdinal("ButtonLink");
                int oButtonText1 = rd.GetOrdinal("ButtonText1");
                int oButtonTitle1 = rd.GetOrdinal("ButtonTitle1");
                int oButtonLink1 = rd.GetOrdinal("ButtonLink1");
                int oPresentation = rd.GetOrdinal("PresentationClass");

                while (rd.Read())
                {
                    BannerItem b = new BannerItem();

                    b.BannerId = rd.GetInt32(oBannerId);
                    b.BannerName = ReadString(rd, oBannerName);
                    b.FileLocation = ReadString(rd, oFileLocation);
                    b.Gallery = rd.IsDBNull(oGallery) ? GalleryId : rd.GetInt32(oGallery);
                    b.BannerLink = ReadString(rd, oBannerLink);
                    b.OpenInNewWindow = !rd.IsDBNull(oTarget) && rd.GetBoolean(oTarget);
                    b.AltText = ReadString(rd, oAltText);
                    b.Caption = ReadString(rd, oCaption);
                    b.Title = ReadString(rd, oTitle);
                    b.Body = ReadString(rd, oBody);
                    b.ButtonText = ReadString(rd, oButtonText);
                    b.ButtonTitle = ReadString(rd, oButtonTitle);
                    b.ButtonLink = ReadString(rd, oButtonLink);
                    b.ButtonText1 = ReadString(rd, oButtonText1);
                    b.ButtonTitle1 = ReadString(rd, oButtonTitle1);
                    b.ButtonLink1 = ReadString(rd, oButtonLink1);
                    b.PresentationClass = ReadString(rd, oPresentation);

                    b.ImageUrl = BuildImageUrl(b);

                    list.Add(b);
                }
            }
        }

        return list;
    }

    private static string ReadString(SqlDataReader rd, int ordinal)
    {
        return rd.IsDBNull(ordinal) ? string.Empty : rd.GetString(ordinal).Trim();
    }

    /// <summary>
    /// Builds {base}/{Gallery}/{BannerName}. BannerFileLocation is used as the base
    /// folder when the row has one, otherwise ImageBasePath. If the stored path
    /// already ends with the gallery id, the id is not appended twice.
    /// </summary>
    private string BuildImageUrl(BannerItem b)
    {
        if (b.BannerName.Length == 0)
        {
            return string.Empty;
        }

        string basePath = b.FileLocation.Length > 0 ? b.FileLocation : ImageBasePath;
        basePath = basePath.Replace('\\', '/').TrimEnd('/');

        string galleryFolder = b.Gallery.ToString();

        if (!basePath.EndsWith("/" + galleryFolder, StringComparison.OrdinalIgnoreCase))
        {
            basePath = basePath + "/" + galleryFolder;
        }

        return basePath + "/" + b.BannerName.TrimStart('/');
    }

    private static void ShuffleList(List<BannerItem> list)
    {
        Random rnd = new Random();

        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = rnd.Next(i + 1);
            BannerItem tmp = list[i];
            list[i] = list[j];
            list[j] = tmp;
        }
    }

    // =====================================================================
    // Rendering
    // =====================================================================

    protected void rptBanners_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Item &&
            e.Item.ItemType != ListItemType.AlternatingItem)
        {
            return;
        }

        BannerItem b = (BannerItem)e.Item.DataItem;

        // ---------------- slide wrapper ----------------
        HtmlGenericControl slide = (HtmlGenericControl)e.Item.FindControl("divSlide");

        // First slide is the visible one; the rest are hidden until the script runs.
        if (e.Item.ItemIndex == 0)
        {
            slide.Attributes["class"] = "bg-slide is-active";
        }
        else
        {
            slide.Attributes["aria-hidden"] = "true";
        }

        // A row can opt into a different layout without a code change.
        if (b.PresentationClass.Length > 0)
        {
            slide.Attributes["class"] = slide.Attributes["class"] + " bg-" + b.PresentationClass;
        }

        // ---------------- image ----------------
        Image img = (Image)e.Item.FindControl("imgBanner");
        HyperLink lnkImage = (HyperLink)e.Item.FindControl("lnkImage");

        if (b.ImageUrl.Length > 0)
        {
            img.ImageUrl = ResolveUrl(b.ImageUrl);

            // Where AltText is blank the image is decorative: the title and body
            // beside it already carry the meaning, so an empty alt is correct.
            img.AlternateText = b.AltText;
        }
        else
        {
            img.Visible = false;
        }

        // With no NavigateUrl a HyperLink renders as a plain span, which is what
        // we want when the banner image is not clickable.
        if (b.BannerLink.Length > 0)
        {
            lnkImage.NavigateUrl = b.BannerLink;

            if (b.OpenInNewWindow || IsExternal(b.BannerLink))
            {
                lnkImage.Target = "_blank";
                lnkImage.Attributes["rel"] = "noopener noreferrer";
            }
        }

        // ---------------- eyebrow / caption ----------------
        Literal litEyebrow = (Literal)e.Item.FindControl("litEyebrow");

        if (b.Caption.Length > 0)
        {
            litEyebrow.Text = "<p class=\"bg-eyebrow\">" + HttpUtility.HtmlEncode(b.Caption) + "</p>";
        }
        else
        {
            litEyebrow.Visible = false;
        }

        // ---------------- title ----------------
        Literal litTitle = (Literal)e.Item.FindControl("litTitle");

        if (b.Title.Length > 0)
        {
            litTitle.Text = "<h2 class=\"bg-title\">" + HttpUtility.HtmlEncode(b.Title) + "</h2>";
        }
        else
        {
            litTitle.Visible = false;
        }

        // ---------------- body ----------------
        Literal litBody = (Literal)e.Item.FindControl("litBody");

        if (b.Body.Length > 0)
        {
            string body = AllowHtmlInBody
                ? b.Body
                : HttpUtility.HtmlEncode(b.Body).Replace("\r\n", "<br />").Replace("\n", "<br />");

            litBody.Text = "<div class=\"bg-text\">" + body + "</div>";
        }
        else
        {
            litBody.Visible = false;
        }

        // ---------------- buttons ----------------
        HyperLink primary = (HyperLink)e.Item.FindControl("lnkPrimary");
        HyperLink secondary = (HyperLink)e.Item.FindControl("lnkSecondary");

        bool hasPrimary = ApplyButton(primary, b.ButtonText, b.ButtonLink, b.ButtonTitle, b.OpenInNewWindow);
        bool hasSecondary = ApplyButton(secondary, b.ButtonText1, b.ButtonLink1, b.ButtonTitle1, b.OpenInNewWindow);

        // No buttons at all: drop the action row rather than leaving empty space.
        PlaceHolder phActions = (PlaceHolder)e.Item.FindControl("phActions");
        phActions.Visible = hasPrimary || hasSecondary;
    }

    /// <summary>
    /// Shows a button only when it has both a label and a link.
    /// Returns true when the button was rendered.
    /// </summary>
    private static bool ApplyButton(HyperLink link, string text, string url, string title, bool newWindow)
    {
        if (text.Length == 0 || url.Length == 0)
        {
            link.Visible = false;
            return false;
        }

        link.Text = HttpUtility.HtmlEncode(text);
        link.NavigateUrl = url;

        if (title.Length > 0)
        {
            link.ToolTip = title;
        }

        if (newWindow || IsExternal(url))
        {
            link.Target = "_blank";
            link.Attributes["rel"] = "noopener noreferrer";
        }

        return true;
    }

    private static bool IsExternal(string url)
    {
        return url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
               url.StartsWith("https://", StringComparison.OrdinalIgnoreCase);
    }

    // =====================================================================
    // Row model
    // =====================================================================

    private class BannerItem
    {
        public int BannerId;
        public string BannerName = string.Empty;
        public string FileLocation = string.Empty;
        public int Gallery;
        public string BannerLink = string.Empty;
        public bool OpenInNewWindow;
        public string AltText = string.Empty;
        public string Caption = string.Empty;
        public string Title = string.Empty;
        public string Body = string.Empty;
        public string ButtonText = string.Empty;
        public string ButtonTitle = string.Empty;
        public string ButtonLink = string.Empty;
        public string ButtonText1 = string.Empty;
        public string ButtonTitle1 = string.Empty;
        public string ButtonLink1 = string.Empty;
        public string PresentationClass = string.Empty;
        public string ImageUrl = string.Empty;
    }
}
