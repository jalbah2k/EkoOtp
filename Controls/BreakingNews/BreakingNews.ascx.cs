#define STORED_PROC
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
using System.Globalization;

public partial class BreakingNews : System.Web.UI.UserControl
{
    string NewsroomFilesPath = "/data/NewsroomFiles/";
    public int records = 2;
    public string categoryid;
    public string publish =  "1";
    public bool ShowHeader = true;
    public BreakingNews() { }
    public BreakingNews(string p) 
    {
        string[] s = p.Split(new char[] { ',' });
        categoryid = s[0];

        if (s.Length > 1)
            publish = s[1];
    }

    #region properties

    public string Language
    {
        get
        {
            //return CMSHelper.GetCleanQueryString("lang", "1");
            return CMSHelper.GetLanguageNumber();
        }
    }

    public string LangPrefix
    {
        get
        {
            return CMSHelper.GetLanguagePrefix();
        }
    }
    private bool _auto = false;
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

    private bool _autoControls = false;
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

    private int _displaySlideQty = 4;
    public int displaySlideQty
    {
        get
        {
            return _displaySlideQty;
        }
        set
        {
            _displaySlideQty = value;
        }
    }

    private int _moveSlideQty = 1;
    public int moveSlideQty
    {
        get
        {
            return _moveSlideQty;
        }
        set
        {
            _moveSlideQty = value;
        }
    }

    private string _prevImage = "/images/amgh/prev-news.png";
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

    private string _nextImage = "/images/amgh/next-news.png";
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

    private string _startImage = "/Images/Icons/start.png";
    public string startImage
    {
        get
        {
            return _startImage;
        }
        set
        {
            _startImage = value;
        }
    }

    private string _stopImage = "/Images/Icons/stop.png";
    public string stopImage
    {
        get
        {
            return _stopImage;
        }
        set
        {
            _stopImage = value;
        }
    }

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

    private int _tickerSpeed = 5000;   //miliseconds
    public int tickerSpeed
    {
        get
        {
            return _tickerSpeed;
        }
        set
        {
            _tickerSpeed = value;
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

    public enum TickerDirection { next, prev };
    private TickerDirection _tickerDirection = TickerDirection.next;
    public TickerDirection tickerDirection
    {
        get
        {
            return _tickerDirection;
        }
        set
        {
            _tickerDirection = value;
        }
    }

    #endregion properties

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack && Session["BreakingNewsLoaded"] == null)
        {
            BindData();
        }

        Session["BreakingNewsLoaded"] = null;
    }

    private void BindData()
    {
        DataSet dt = new DataSet();
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("CMServer")))
        {

#if !STORED_PROC
            string sql = @"select top (@top) n.id, n.title, convert(varchar,newsdate,109) as newsdatev, DetailsShort, Photo, MIMEType, n.PhotoAltText, n.type, n.filename, ExternalURL, seo, NewsDate, linkid 
            from newsroom n left outer join NewsRoomImages i on n.id = i.NewsroomId 
		                                            inner join NewsCategories_Link c on n.id = c.NewsId
		                                            where c.CategoryId =isnull(@categ, c.CategoryId)
                                                    and PublishOn in (@publish, 2)
                                                    and lang=@lang 
                                                    and upper(isnull(n.status, '')) = 'L'
	                                                and isnull(GoLiveDate, getdate()) <= getdate()
	                                                and isnull(ExpiryDate, DATEADD(day, 1, getdate())) > getdate()
                                                    order by feature desc, newsdate desc";
#else
            string sql = "BreakingNews";
#endif

            SqlDataAdapter da = new SqlDataAdapter( sql, conn);
#if !STORED_PROC
            da.SelectCommand.CommandType = CommandType.Text;
#else
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
#endif
            da.SelectCommand.Parameters.AddWithValue("@Lang", 1);       // Session["Language"].ToString());

            if (categoryid != "0")
                da.SelectCommand.Parameters.AddWithValue("@categ", categoryid);
            else
                da.SelectCommand.Parameters.AddWithValue("@categ", DBNull.Value);

            da.SelectCommand.Parameters.AddWithValue("@publish", publish);

            //Response.Write(Session["Language"].ToString());
            if (records > 0)
            {
                da.SelectCommand.Parameters.AddWithValue("@top", records);
                //Response.Write("A");
            }

            lnkAllNews.NavigateUrl = "/newsroom";
            if(publish == "3")
                lnkAllNews.NavigateUrl = "/membernews";

            da.Fill(dt);
        }

       // Response.Write(dt.Tables[0].Rows.Count);
        Repeater1.DataSource = dt.Tables[0];
        Repeater1.DataBind();

        if (dt.Tables[0].Rows.Count == 0)
            this.Visible = false;

    }

    //public void BindData(string id, int qty, bool iscateg = false)
    //{
    //    DataSet dt = new DataSet();
    //    using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("CMServer")))
    //    {
    //        SqlDataAdapter da = new SqlDataAdapter(@"select top (@top) n.id, n.title, convert(varchar,newsdate,109) as newsdatev, DetailsShort, Photo, MIMEType, n.PhotoAltText, n.type, n.filename, ExternalURL, seo, NewsDate, linkid 
    //                                                from newsroom n left outer join NewsRoomImages i on n.id = i.NewsroomId 
    //                                                where lang=@lang 
    //                                                and upper(isnull(n.status, '')) = 'L'
    //                                             and isnull(GoLiveDate, getdate()) <= getdate()
    //                                             and isnull(ExpiryDate, DATEADD(day, 1, getdate())) > getdate()
    //                                                and n.id != @id
    //                                                order by feature desc, newsdate desc", conn);
    //        if (iscateg)
    //        {
    //            da.SelectCommand.CommandText = @"declare @categ int;
    //                                                select @categ = CategoryId from NewsCategories_Link where NewsId= @id;
    //                                                select top (@top) n.id, n.title, convert(varchar,newsdate,109) as newsdatev, DetailsShort, Photo, MIMEType, n.PhotoAltText, n.type, n.filename, ExternalURL, seo, NewsDate, linkid 
    //                                                from newsroom n left outer join NewsRoomImages i on n.id = i.NewsroomId 
    //                                          inner join NewsCategories_Link c on n.id = c.NewsId
    //                                          where c.CategoryId = @categ
    //                                                and lang=@lang 
    //                                                and upper(isnull(n.status, '')) = 'L'
    //                                             and isnull(GoLiveDate, getdate()) <= getdate()
    //                                             and isnull(ExpiryDate, DATEADD(day, 1, getdate())) > getdate()
    //                                                and n.id != @id
    //                                                order by feature desc, newsdate desc";
    //        }

    //        da.SelectCommand.CommandType = CommandType.Text;

    //        da.SelectCommand.Parameters.AddWithValue("@id", id);       
    //        da.SelectCommand.Parameters.AddWithValue("@Lang", 1);       // Session["Language"].ToString());
    //        da.SelectCommand.Parameters.AddWithValue("@top", qty);

    //        da.Fill(dt);
    //    }

    //    Repeater1.DataSource = dt.Tables[0];
    //    Repeater1.DataBind();

    //    Session["BreakingNewsLoaded"] = true;
    //    ShowHeader = true;      // false;
    //}

    protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView dr = (DataRowView)e.Item.DataItem;

        System.Web.UI.WebControls.Image imgPhoto = (System.Web.UI.WebControls.Image)e.Item.FindControl("imgPhoto");
        /*if (imgPhoto.Visible = dr["Photo"].ToString() != "")
        {
            imgPhoto.ImageUrl = "/Controls/Newsroom/ThumbNail.ashx?PictureID=" + dr["id"].ToString() + "&maxsz=222";
        }*/
        if (dr["Photo"] != DBNull.Value)
        {
            imgPhoto.ImageUrl = "/Controls/Newsroom/ThumbNail.ashx?PictureID=" + dr["id"].ToString() + "&maxsz=80";

            HtmlGenericControl div_image_news = (HtmlGenericControl)e.Item.FindControl("div_image_news");
            div_image_news.Attributes["style"] = "background:none!important";
        }
        else
        {
            //////imgPhoto.ImageUrl = "/images/inactive-bg.gif";
            //////imgPhoto.Width = new Unit("222");
            //////imgPhoto.Height = new Unit("163");
            imgPhoto.Visible = false;
        }
       // Literal litDate = (Literal)e.Item.FindControl("litDate");
        
        ////DateTime dt = DateTime.ParseExact(dr["NewsDate"].ToString(), "yyyy-MM-d", CultureInfo.InvariantCulture);
        ////string result = dt.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
        //litDate.Text = DateTime.Parse(Convert.ToDateTime(dr["NewsDate"].ToString(), CultureInfo.InvariantCulture).ToString(), CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
       
        string ltdate = DateTime.Parse(Convert.ToDateTime(dr["NewsDate"].ToString(), CultureInfo.InvariantCulture).ToString(), CultureInfo.InvariantCulture).ToString("MMMM dd, yyyy", CultureInfo.InvariantCulture);
        Literal litDate = (Literal)e.Item.FindControl("litDate");
        litDate.Text = ltdate;

        Literal litTitle = (Literal)e.Item.FindControl("litTitle");
        
        string Title = dr["title"].ToString();
        if (Title.Length > 90)
        {
            Title = Title.Substring(0, 90);

            int npos = Title.LastIndexOf(" ");
            if (npos > 0)
                Title = Title.Remove(npos);
        }

        litTitle.Text = Title;

        //Literal litShortDesc = (Literal)e.Item.FindControl("litShortDesc");

        //int len = 100;
        //string ShortDesc = dr["DetailsShort"].ToString();
        //if (ShortDesc.Length > len)
        //    ShortDesc = ShortDesc.Substring(0, len - 3) + "...";

        //if(ShortDesc.Length > 0)
        //    litShortDesc.Text = "<p>" + ShortDesc + "</p>";

        // HtmlGenericControl theLink = (HtmlGenericControl)e.Item.FindControl("theLink");
        HtmlAnchor theLink = (HtmlAnchor)e.Item.FindControl("theLink");

       // theLink.Attributes.Add("title", "Read full story: " + dr["title"].ToString());
        theLink.Attributes.Add("title", dr["title"].ToString());

        if (dr["type"].ToString() == "0")
        {
            //theLink.Attributes.Add("onclick", "window.location='/newsroom?newsid=" + dr["id"].ToString() + "'; return false;");
            theLink.HRef = (publish == "3" ? "/membernews?newsid=" : "/newsroom?newsid=") + dr["linkid"].ToString();

        }
        else if (dr["type"].ToString() == "1")
        {
            // theLink.Attributes.Add("onclick", "window.open('" + NewsroomFilesPath + dr["filename"].ToString() + "', null, 'status=no, toolbar=no, menubar=no, location=no, scrollbars=yes, resizable'); return false;");
            theLink.Attributes.Add("class", "newsitem read_more open_new_tab");
            theLink.Attributes.Add("filename", NewsroomFilesPath + dr["filename"].ToString());
            theLink.HRef = "#";
        }
        else
        {
            if (dr["seo"].ToString() != "")
            {
                //   theLink.Attributes.Add("onclick", "window.location='/" + dr["seo"].ToString() + "'; return false;");
                theLink.HRef = "/" + dr["seo"].ToString();

            }
            else
            {
                // theLink.Attributes.Add("onclick", "window.open('" + dr["ExternalURL"].ToString() + "', null, 'status=no, toolbar=no, menubar=no, location=no, scrollbars=yes, resizable'); return false;");
                theLink.HRef = dr["ExternalURL"].ToString();
                theLink.Target = "_blank";
            }
        }
    }
}