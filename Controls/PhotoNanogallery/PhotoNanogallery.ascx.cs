//#define ALBUM
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

public partial class PhotoNanogallery : System.Web.UI.UserControl
{
    SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
    
    StringBuilder sb;

    private string _par = string.Empty;

    public string ContentId
    {
        set; get;
    }

    public string ImagePath
    {
        set; get;
    }
    public string GalleryId
    {
        get
        {
            return _par;
        }
        set
        {
            _par = value;
        }
    }

    public PhotoNanogallery()
	{

    }

    public PhotoNanogallery(string p)
	{
        this._par = p;
	}

    protected void Page_Load(object sender, EventArgs e)
    {

        ImagePath = "/Data/Photos/" + GalleryId;        // + "/thumbs/";
        InitControl();

        if (!IsPostBack)
        {
            Bind();
        }

    }

    private void InitControl()
    {

        StringBuilder scriptCSS = new StringBuilder();
        scriptCSS.Append(Environment.NewLine + "<link rel=\"stylesheet\" href=\"/Libraries/nanogallery2-master/dist/css/nanogallery2.min.css\" />");
        scriptCSS.Append(Environment.NewLine + "<link rel=\"stylesheet\" href=\"/Libraries/nanogallery2-master/dist/css/nanogallery2.woff.min.css\" />");
        scriptCSS.Append(Environment.NewLine + "<link rel=\"stylesheet\" href=\"/Controls/PhotoNanogallery/photonano.css?v=2\" />");

        ((_Default)this.Page).InjectContent("head", scriptCSS + Environment.NewLine, (int)CMSHelper.counters.photolight_header);

        StringBuilder scriptCode = new StringBuilder();
        scriptCode.Append(Environment.NewLine + "<script src=\"/Libraries/nanogallery2-master/dist/jquery.nanogallery2.js\"></script>" + Environment.NewLine);

        ((_Default)this.Page).InjectContent("Scripts", scriptCode.ToString(), (int)CMSHelper.counters.photolight_footer);

        ContentId = Guid.NewGuid().ToString().Replace("-", "");

    }

    private void Bind()
    {
        string sqlcmd = " SELECT p.*, g.name as title, g.flickr, g.FlickrUserName, g.FlickrSetId from PhotoGroups g left join Photos p on g.id=p.groupid  where g.id = @groupid order by p.priority";
        
        SqlDataAdapter dapt = new SqlDataAdapter(sqlcmd, conn);
        dapt.SelectCommand.Parameters.AddWithValue("@groupid", this._par);
        DataTable dt = new DataTable();
        dapt.Fill(dt);

        FillData(dt);

        DataRow dr = dt.Rows[0];
        lblTitle.Text = "<p>" + dr["title"].ToString() + "</p>";

        if (Session["LoggedInID"] != null)
        {
            if (Permissions.Get(int.Parse(Session["LoggedInID"].ToString()), int.Parse(Session["PageID"].ToString())) > 1)
            {
                btnPhotoAdd.Visible = true;
                litBtnAddPhoto.Text = "<a href='javascript:void(0)' class='BtnAddPhoto' title='Add Photo' GalleryId='"
                    + dr["groupid"] + "' GalleryName='"
                    + dr["title"] + "'><img src='/images/lemonaid/buttons/plus.png' alt='add photo' width='20px' /></a>";

                string script = "$(document).ready(function () {" + Environment.NewLine;
                script += "     $('.BtnAddPhoto').click(function (e) {" + Environment.NewLine;
                script += "         if (!$('#pnlPhotoGalleryAdd').is(':visible')) {" + Environment.NewLine;
                script += "             $('.hfTempFolder').val($(this).attr('GalleryId'));" + Environment.NewLine;
                script += "             $find('wmePhotoGalleryName').set_Text($(this).attr('GalleryName'));" + Environment.NewLine;
                script += "             $('.btnFrontPhotoGalleryEdit').click();" + Environment.NewLine;
                script += "             $('#pnlPhotoGalleryAdd').slideDown(600);" + Environment.NewLine;
                script += "         }" + Environment.NewLine;
                script += "         return false;" + Environment.NewLine;
                script += "     });" + Environment.NewLine;
                script += "});" + Environment.NewLine;

                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "PhotoGallery", script, true);
            }
        }
    }

    private void FillData(DataTable dt)
    {
        //https://codepen.io/Kris-B/pen/GrNmxv
        //https://nanogallery2.nanostudio.org/
        //https://nanogallery2.nanostudio.org/documentation.html#ngy2_gallery_thumbnails_label
        //https://codepen.io/Kris-B/pen/EoYLpN

        DataRow dw = dt.Rows[0];
        string filename = dw["filename"].ToString().Replace("&amp;", "and").Replace("'", "’").Replace("\"", "“");
        string title = dw["title"].ToString().Replace("&amp;", "and").Replace("'", "’").Replace("\"", "“");
        string AlbumId = GalleryId;

        string s = "$(document).ready(function () {" + Environment.NewLine +
            "  $(\"#my_nanogallery2_" + ContentId + "\").nanogallery2({" + Environment.NewLine +
            "    items:[" + Environment.NewLine
#if ALBUM
            + "    { src: '" + filename +  "', srct: '" + filename + "', title: '" + title + "', ID: " + AlbumId + ", kind: 'album'}," + Environment.NewLine
#endif
            ;

        StringBuilder sb = new StringBuilder();

        foreach (DataRow dr in dt.Rows)
        {
            filename = dr["filename"].ToString().Replace("&amp;", "and").Replace("'", "’").Replace("\"", "“");

            string captionheader = dr["captionheader"].ToString().Replace("'", "’").Replace("\"", "“");
            int maxlen = 99;
            if (captionheader.Length > maxlen)
                captionheader = captionheader.Remove(maxlen);

            string caption = dr["caption"].ToString().Replace("'", "’").Replace("\"", "“");

            string temp = "{ src: \"" + filename + 
                "\", srct: \"" + filename + 
                "\", title: \"" + captionheader + 
                "\", description: \"" + caption +
#if ALBUM
                "\", albumID: " + AlbumId +
#else
                "\"" + 
#endif                
                "},";

            sb.Append( temp + Environment.NewLine);
        }

        s += sb.ToString() + 
             "    ]," + Environment.NewLine +
             "    thumbnailWidth: '700', " + Environment.NewLine +      //auto
             "    thumbnailHeight: 700, " + Environment.NewLine +       //170
             "    itemsBaseURL: '" + ImagePath + "/', " + Environment.NewLine +
             "    locationHash: false, " + Environment.NewLine +
             "    viewerToolbar: { " + Environment.NewLine +
             "        display: true, " + Environment.NewLine +
             "        standard: 'minimizeButton, label'," + Environment.NewLine +
             "        minimized: 'minimizeButton, label, fullscreenButton, downloadButton, infoButton'" + Environment.NewLine +
             "    }," + Environment.NewLine +
             "    viewerTools: { " + Environment.NewLine +
             "        topLeft: 'pageCounter', " + Environment.NewLine +
             "        topRight: 'playPauseButton, zoomButton, fullscreenButton, closeButton'" + Environment.NewLine +
             "    }," + Environment.NewLine +
             "    thumbnailLabel: {" + Environment.NewLine +
             "        display: false" + Environment.NewLine +  //,valign: "bottom", position: 'overImage', hideIcons: true, displayDescription: false
             "    }" + Environment.NewLine +
             "  });" + Environment.NewLine + 
             "});" + Environment.NewLine;


        ((_Default)this.Page).InjectContent("Scripts", Environment.NewLine + s + Environment.NewLine, true);

    }

}