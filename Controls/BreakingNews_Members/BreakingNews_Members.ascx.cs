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
    public int records = 3;
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

    protected int category
    {
        set { ViewState["category"] = value; }
        get
        {
            try { return Convert.ToInt32(ViewState["category"]); }
            catch { return 0; }
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
            string sql = "BreakingNews";
            SqlDataAdapter da = new SqlDataAdapter(sql, conn);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;

            da.SelectCommand.Parameters.AddWithValue("@Lang", 1);       // Session["Language"].ToString());

            if (categoryid != "0")
            {
                da.SelectCommand.Parameters.AddWithValue("@categ", categoryid);
                category = int.Parse(categoryid);
            }
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
            if (publish == "3")
                lnkAllNews.NavigateUrl = "/membernews";

            da.Fill(dt);
        }

        // Response.Write(dt.Tables[0].Rows.Count);
        Repeater1.DataSource = dt.Tables[0];
        Repeater1.DataBind();

        if (dt.Tables[0].Rows.Count == 0)
            this.Visible = false;

        if (bLoadMore = (Convert.ToInt32(dt.Tables[1].Rows[0][0]) >= records))
        {
            ((_Default)this.Page).InjectContent("Scripts", "<script src=\"/controls/Newsroom/LoadNews.js\"></script>");
        }


    }

    public bool bLoadMore = false;
    protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView dr = (DataRowView)e.Item.DataItem;

        System.Web.UI.WebControls.Image imgPhoto = (System.Web.UI.WebControls.Image)e.Item.FindControl("imgPhoto");
        if (dr["Photo"] != DBNull.Value)
        {
            imgPhoto.ImageUrl = "/Controls/Newsroom/ThumbNail.ashx?PictureID=" + dr["id"].ToString() + "&maxsz=80";
        }
        else
        {
            imgPhoto.Visible = false;
        }
       
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

   
        HtmlAnchor theLink = (HtmlAnchor)e.Item.FindControl("theLink");

        theLink.Attributes.Add("title", dr["title"].ToString());

        if (dr["type"].ToString() == "0")
        {
            theLink.HRef = (publish == "3" ? "/membernews?newsid=" : "/newsroom?newsid=") + dr["linkid"].ToString();

        }
        else if (dr["type"].ToString() == "1")
        {
            theLink.Attributes.Add("class", "three jnewssc newsitem read_more open_new_tab");
            //theLink.Attributes.Add("filename", NewsroomFilesPath + dr["filename"].ToString());
            //theLink.HRef = "#";
            
            theLink.HRef = NewsroomFilesPath + dr["filename"].ToString();
            theLink.Target = "_blank";

        }
        else
        {
            if (dr["seo"].ToString() != "")
            {
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