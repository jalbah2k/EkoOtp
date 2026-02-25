using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web.Optimization;
using System.Web.UI.HtmlControls;

public partial class Controls_Banners_Banners : System.Web.UI.UserControl
{
    #region Properties

    public string GalleryId = "1";

    public int BannersQty = 0;

    private int _width;// = 1200;
    public int Width
    {
        get
        {
            return _width;
        }
        set
        {
            _width = value;
        }
    }

    private int _height;// = 457;
    public int Height
    {
        get
        {
            return _height;
        }
        set
        {
            _height = value;
        }
    }

    private bool _autoplay = true;
    public bool Autoplay
    {
        get
        {
            return _autoplay;
        }
        set
        {
            _autoplay = value;
        }
    }

    private bool _shuffle = false;
    public bool Shuffle
    {
        get
        {
            return _shuffle;
        }
        set
        {
            _shuffle = value;
        }
    }

    private string _direction = "h";
    public string Direction
    {
        get
        {
            return _direction;
        }
        set
        {
            _direction = value;
        }
    }

    private string _transitions = "basic";
    public string Transitions
    {
        get
        {
            return _transitions;
        }
        set
        {
            _transitions = value;
        }
    }

    #endregion Properties

    public Controls_Banners_Banners()
    {
        GalleryId = "17";
    }

    public Controls_Banners_Banners(string parameters)
    {
        GalleryId = parameters;
    }

   
    protected void Page_Load(object sender, EventArgs e)
    {
        LoadGallery();

    }

    private void LoadGallery()
    {
        DataTable dt = new DataTable();

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]))
        {
            //string sqlstr = "select b.*, width, height, autoplay from banners b inner join BannerGallery g on b.Gallery = g.id where bannerstatus='A' and gallery=@GalleryId and getdate() between isnull(StartDate, getdate()) and isnull(EndDate, getdate()) order by bannerpriority";

            string sqlstr = @"declare @shuffle int 
                select @shuffle=shuffle from BannerGallery where id=@GalleryId

                if @shuffle = 0
	                select b.*, width, height, autoplay from banners b inner join BannerGallery g on b.Gallery = g.id where bannerstatus='A' 
	                and gallery=@GalleryId and getdate() between isnull(StartDate, getdate()) and isnull(EndDate, getdate()) 
	                order by bannerpriority
                else
	                select b.*, width, height, autoplay from banners b inner join BannerGallery g on b.Gallery = g.id where bannerstatus='A' 
	                and gallery=@GalleryId and getdate() between isnull(StartDate, getdate()) and isnull(EndDate, getdate()) 
	                ORDER BY NEWID();
                ";

            SqlDataAdapter dapt = new SqlDataAdapter(sqlstr, ConfigurationManager.AppSettings["CMServer"]);
            dapt.SelectCommand.Parameters.AddWithValue("@GalleryId", GalleryId);
            dapt.Fill(dt);
        }

        BannersQty = dt.Rows.Count;

        if(BannersQty > 0)
        {
            DataRow dr0 = dt.Rows[0];

            HtmlGenericControl divRow = new HtmlGenericControl("div");
            divRow.Attributes.Add("class", "myslick single-item");
            divRow.ID = "Gallery_" + GalleryId;
                 
            Literal litContent = new Literal();

            foreach(DataRow dr in dt.Rows)
            {
                string stemp = "<div{5}><div{0}>{1}{2}{3}{4}</div></div>";

                string header = !String.IsNullOrEmpty(dr["Title"].ToString()) ? "<h1>" + dr["Title"].ToString() + "</h1>" : "";
                string body = !String.IsNullOrEmpty(dr["Body"].ToString()) ? "<p>" + dr["Body"].ToString() + "<p>" : "";
                string button = "", button1 = "";
                if (!String.IsNullOrEmpty(dr["ButtonText"].ToString()) && !String.IsNullOrEmpty(dr["ButtonLink"].ToString()))
                    button = String.Format("<a href='{0}' class='button1'>{1}</a>", dr["ButtonLink"].ToString(),  dr["ButtonText"].ToString());

                if (!String.IsNullOrEmpty(dr["ButtonText1"].ToString()) && !String.IsNullOrEmpty(dr["ButtonLink1"].ToString()))
                    button1 = String.Format("<a href='{0}' class='button2'>{1}</a>", dr["ButtonLink1"].ToString(), dr["ButtonText1"].ToString());

                stemp = String.Format(stemp, " class='innerHomeBanner'",  header, body, button, button1, " class='contained-width banner-contained-width'");
                
                litContent.Text += String.Format("<div{1}{2}>{0}</div>",
                    stemp,
                    // String.Format(" style=\"background:url('{0}{1}/{2}') no-repeat; background-position:top left 3vw; min-height:{3};text-align:center;\"", 
                    // dr["BannerFileLocation"].ToString(), GalleryId, dr["BannerName"].ToString(), "calc(75vh - 100px)!important"));
                    String.Format(" style=\"background:url('{0}{1}/{2}') no-repeat; background-position:top left 3vw; min-height:{3};text-align:center;\"", 
                    dr["BannerFileLocation"].ToString(), GalleryId, dr["BannerName"].ToString(), "540px!important"),
                    dr["PresentationClass"].ToString() != "" ? String.Format(" class='{0}'", dr["PresentationClass"].ToString()) : ""
                    );
            }

            divRow.Controls.Add(litContent);
            plhGallery.Controls.Add(divRow);

            //https://kenwheeler.github.io/slick/

            // srcript = "$(document).ready(function () {$('#Gallery_" + GalleryId + ".single-item').slick();});";
            string script = @"$(document).ready(function () {
                            $('{0}.myslick.single-item').slick({
                                infinite: true, 
                                speed: 600,
                                slidesToShow: 1, 
                                slidesToScroll: 1, 
                                autoplay: {1},
                                autoplaySpeed: {2},
                                arrows: {3},
                                adaptiveHeight: true
                            });
                        });" + Environment.NewLine; 

            script = script.Replace("{0}", "#" + divRow.ClientID);
            script = script.Replace("{1}", Convert.ToBoolean(dr0["autoplay"]) ? "true" : "false");
            script = script.Replace("{2}", "4400");
            script = script.Replace("{3}", BannersQty > 1 ? "true" : "false");
            

            ((_Default)this.Page).InjectContent("Scripts", script, true);

        }

    }

    
   
}