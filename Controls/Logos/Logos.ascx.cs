using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.IO;
using System.Drawing;
using System.Web.UI.HtmlControls;
using System.Text;

public partial class Controls_Logos_Logos : System.Web.UI.UserControl
{
    #region properties

    public string GalleryId = "1";
    protected string _logoPath = "/uploads/logos/{0}/";

    public enum Mode { horizontal, vertical, fade };
    private Mode _mode = Mode.horizontal;
    public Mode mode
    {
        get
        {
            return _mode;
        }
        set
        {
            _mode = value;
        }
    }

    private int _MaxWidth = 187;
    public int MaxWidth
    {
        get
        {
            return _MaxWidth;
        }
        set
        {
            _MaxWidth = value;
        }
    }

    private int _MaxHeight = 60;
    public int MaxHeight
    {
        get
        {
            return _MaxHeight;
        }
        set
        {
            _MaxHeight = value;
        }
    }

    private bool _ShowText = false;
    public bool ShowText
    {
        get
        {
            return _ShowText;
        }
        set
        {
            _ShowText = value;
        }
    }

    private bool _controls = true;
    public bool controls
    {
        get
        {
            return _controls;
        }
        set
        {
            _controls = value;
        }
    }

    private bool _customControls = false;
    public bool customControls
    {
        get
        {
            return _customControls;
        }
        set
        {
            _customControls = value;
        }
    }

    private bool _auto = true;
    public bool auto
    {
        get
        {
            return _auto;
        }
        set
        {
            _auto = value;
        }
    }

    private bool _autoStart = true;
    public bool autoStart
    {
        get
        {
            return _autoStart;
        }
        set
        {
            _autoStart = value;
        }
    }

    private bool _autoControls = true;
    public bool autoControls
    {
        get
        {
            return _autoControls;
        }
        set
        {
            _autoControls = value;
        }
    }

    private bool _autoHover = true;
    public bool autoHover
    {
        get
        {
            return _autoHover;
        }
        set
        {
            _autoHover = value;
        }
    }

    public enum AutoDirection { next, prev };
    private AutoDirection _autoDirection = AutoDirection.next;
    public AutoDirection autoDirection
    {
        get
        {
            return _autoDirection;
        }
        set
        {
            _autoDirection = value;
        }
    }

    private int _speed = 2000;   //miliseconds
    public int speed
    {
        get
        {
            return _speed;
        }
        set
        {
            _speed = value;
        }
    }

    private int _pause = 1000;   //miliseconds
    public int pause
    {
        get
        {
            return _pause;
        }
        set
        {
            _pause = value;
        }
    }

    private int _minSlides = 1;
    public int minSlides
    {
        get
        {
            return _minSlides;
        }
        set
        {
            _minSlides = value;
        }
    }

    private int _maxSlides = 2;
    public int maxSlides
    {
        get
        {
            return _maxSlides;
        }
        set
        {
            _maxSlides = value;
        }
    }

    private int _moveSlides = 1;
    public int moveSlides
    {
        get
        {
            return _moveSlides;
        }
        set
        {
            _moveSlides = value;
        }
    }

    private string _prevImage = "/images/slider-prev.png";
    public string prevImage
    {
        get
        {
            return _prevImage;
        }
        set
        {
            _prevImage = value;
        }
    }

    private string _nextImage = "/images/slider-next.png";
    public string nextImage
    {
        get
        {
            return _nextImage;
        }
        set
        {
            _nextImage = value;
        }
    }

    //private string _startImage = "/images/icons/start.png";
    //public string startImage
    //{
    //    get
    //    {
    //        return _startImage;
    //    }
    //    set
    //    {
    //        _startImage = value;
    //    }
    //}

    //private string _stopImage = "/images/icons/stop.png";
    //public string stopImage
    //{
    //    get
    //    {
    //        return _stopImage;
    //    }
    //    set
    //    {
    //        _stopImage = value;
    //    }
    //}

    private bool _ticker = false;
    public bool ticker
    {
        get
        {
            return _ticker;
        }
        set
        {
            _ticker = value;
        }
    }

    private bool _tickerHover = true;
    public bool tickerHover
    {
        get
        {
            return _tickerHover;
        }
        set
        {
            _tickerHover = value;
        }
    }

    private bool _shuffle = true;
    public bool shuffle
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

    #endregion properties

    public Controls_Logos_Logos()
    {
        GalleryId = "1";
    }
    public Controls_Logos_Logos(string par)
    {
        GalleryId = par;
    }
    protected void Page_Init(object sender, EventArgs e)
    {
        //if (!Page.ClientScript.IsClientScriptIncludeRegistered("bxSliderVideoPlugin"))
        //    this.Page.ClientScript.RegisterClientScriptInclude("bxSliderVideoPlugin", "/js/jquery.fitvids.js");
        if (!Page.ClientScript.IsClientScriptIncludeRegistered("bxSliderScript"))
            this.Page.ClientScript.RegisterClientScriptInclude("bxSliderScript", "/js/jquery.bxSlider.min.js");

        if (!IsPostBack)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]))
            {
                string sqlstr = "select * from LogoGallery where id=@GalleryId";

                SqlDataAdapter dapt = new SqlDataAdapter(sqlstr, conn);
                dapt.SelectCommand.Parameters.AddWithValue("@GalleryId", GalleryId);
                dapt.Fill(dt);
            }
            if (dt.Rows.Count > 0)
            {
                _mode = (Mode)Convert.ToInt32(dt.Rows[0]["mode"]);
                _MaxWidth = Convert.ToInt32(dt.Rows[0]["width"]);
                _MaxHeight = Convert.ToInt32(dt.Rows[0]["height"]);
                _ShowText = (bool)dt.Rows[0]["ShowText"];
                _auto = (bool)dt.Rows[0]["auto"];
                _autoStart = (bool)dt.Rows[0]["autoStart"];
                _autoControls = (bool)dt.Rows[0]["autoControls"];
                _autoHover = (bool)dt.Rows[0]["autoHover"];
                _autoDirection = (AutoDirection)Convert.ToInt32(dt.Rows[0]["autoDirection"]);
                _speed = Convert.ToInt32(dt.Rows[0]["speed"]);
                _pause = Convert.ToInt32(dt.Rows[0]["pause"]);
                _minSlides = Convert.ToInt32(dt.Rows[0]["minSlides"]);
                _maxSlides = Convert.ToInt32(dt.Rows[0]["maxSlides"]);
                _moveSlides = Convert.ToInt32(dt.Rows[0]["moveSlides"]);
                _controls = (bool)dt.Rows[0]["controls"];
                _customControls = (bool)dt.Rows[0]["customControls"];
                _prevImage = dt.Rows[0]["prevImage"].ToString();
                _nextImage = dt.Rows[0]["nextImage"].ToString();
                //_startImage = dt.Rows[0]["startImage"].ToString();
                //_stopImage = dt.Rows[0]["stopImage"].ToString();
                _ticker = (bool)dt.Rows[0]["ticker"];
                _tickerHover = (bool)dt.Rows[0]["tickerHover"];
                _shuffle = (bool)dt.Rows[0]["shuffle"];
            }
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            BindData();
        }
    }

    private void BindData()
    {
        DataTable dt = new DataTable();
        using (SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["CMServer"].ConnectionString))
        {
            string sqlstr = "select * from logos where gallery=@GalleryId order by priority";
            SqlDataAdapter da = new SqlDataAdapter(sqlstr, sqlConn);
            da.SelectCommand.Parameters.AddWithValue("@GalleryId", GalleryId);
            da.Fill(dt);
        }

        if (dt.Rows.Count > 0)
        {
            dt.Columns.Add(new DataColumn("RandomNum", Type.GetType("System.Int32")));
            DataView dv = new DataView(dt);

            if (_shuffle)
            {
                Random random = new Random();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["RandomNum"] = random.Next(1000);
                }

                dv.Sort = "RandomNum";
            }

            this.Repeater1.DataSource = dv;
            this.Repeater1.DataBind();


            //HiddenField hf = (HiddenField)FindControl("HFImgCount");
            //hf.Value = dt.Rows.Count.ToString();

        }
    }

    //private static Size GetSize(string url, string MIMEType)
    //{
    //    string tmp = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + HttpContext.Current.Request.ApplicationPath;
    //    tmp = tmp + url.Remove(0, 1); // Remove ~
    //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(tmp);
    //    request.Method = "GET";
    //    //        request.Accept = "image/jpeg";
    //    request.Accept = MIMEType;
    //    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
    //    Stream s = response.GetResponseStream();
    //    Bitmap bmp = new Bitmap(s);
    //    Size sz = new Size(bmp.Width, bmp.Height);
    //    return sz;
    //}

    protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;

        Literal litImage = (Literal)e.Item.FindControl("litImage");

        ////Size z = GetSize(thelink.ImageUrl, drv["MIMEType"].ToString());
        //Size z = GetSize("/ShowPicture.aspx?PictureID=" + drv["id"].ToString(), drv["MIMEType"].ToString());
        //double ImageRatio = (double)z.Width / (double)z.Height;
        //double Ratio = (double)_MaxWidth / (double)_MaxHeight;

        //int width;
        //int height;

        //if (ImageRatio >= Ratio)
        // {
        //     width = _MaxWidth;
        //    height = (int)(_MaxWidth / ImageRatio);
        //}
        // else
        // {
        //     height = _MaxHeight;
        //     width = (int)(_MaxHeight * ImageRatio);
        // }

        bool hasImageOnHover = drv["LogoHoverName"] != DBNull.Value && drv["LogoHoverName"].ToString().Length > 0;

        StringBuilder sb = new StringBuilder();

        if (drv["URL"].ToString() != "")
        {
            sb.Append("<a href='");
            sb.Append(drv["URL"].ToString());
            sb.Append("' target='");
            sb.Append(drv["Target"].ToString());
            sb.Append("'>");
        }

        sb.Append("<img ");
        if (hasImageOnHover)
            sb.Append("class='hoverable' ");
        sb.Append("src='");
        sb.Append(string.Format(_logoPath, GalleryId));
        sb.Append(drv["LogoName"].ToString());
        sb.Append("' width='");
        sb.Append(_MaxWidth);
        sb.Append("px' height='");
        sb.Append(_MaxHeight);
        sb.Append("px' alt='");
        sb.Append(drv["AltText"].ToString());
        if (hasImageOnHover)
        {
            sb.Append("' hoverImage='");
            sb.Append(string.Format(_logoPath, GalleryId));
            sb.Append(drv["LogoHoverName"].ToString());
        }
        sb.Append("' />");

        if (drv["URL"].ToString() != "")
        {
            sb.Append("</a>");
        }

        litImage.Text = sb.ToString();

        //e.Item.FindControl("Desc").Visible = _ShowText;
        HtmlContainerControl Description = (HtmlContainerControl)e.Item.FindControl("Desc");
        Description.Visible = _ShowText;
        Description.Style.Add("width", _MaxWidth.ToString() + "px");
        Description.Style.Add("height", _MaxHeight.ToString() + "px");
    }
    
    /*protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;

        //HyperLink thelink = (HyperLink)e.Item.FindControl("thelink");
        System.Web.UI.WebControls.Image thelink = (System.Web.UI.WebControls.Image)e.Item.FindControl("thelink");
        thelink.ToolTip = drv["URL"].ToString();

        Size z = GetSize(thelink.ImageUrl, drv["MIMEType"].ToString());
        double ImageRatio = (double)z.Width / (double)z.Height;
        double Ratio = (double)_MaxWidth / (double)_MaxHeight;
        if (ImageRatio >= Ratio)
        {
            thelink.Width = _MaxWidth;
            thelink.Height = (int)(_MaxWidth / ImageRatio);
        }
        else
        {
            thelink.Height = _MaxHeight;
            thelink.Width = (int)(_MaxHeight * ImageRatio);
        }

        if (drv["URL"].ToString() != "")
            thelink.Attributes.Add("onclick", "window.location='" + drv["URL"].ToString() + "'");

        //e.Item.FindControl("Desc").Visible = _ShowText;
        HtmlContainerControl Description = (HtmlContainerControl)e.Item.FindControl("Desc");
        Description.Visible = _ShowText;
        Description.Style.Add("width", _MaxWidth.ToString() + "px");
        Description.Style.Add("height", _MaxHeight.ToString() + "px");
    }*/
}