#define SPLIT_CONTENT
#define PRE_VIEW
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.HtmlControls;
using System.Text;

public partial class Controls_NewsRoom_NewsRoom : System.Web.UI.UserControl
{
    public int records;
    protected ITemplate temp;
    string NewsroomFilesPath = "/data/NewsroomFiles/";
    public string publish = "1";
    
    public string newspage
    {
        get { return publish == "3" ? "membernews" : "newsroom"; }
    }

    public string Language
    {
        get
        {
            //return CMSHelper.GetCleanQueryString("lang", "1");
            return CMSHelper.GetLanguageNumber();
        }
    }

    public Controls_NewsRoom_NewsRoom()
    {
        records = 0;
    }

    public Controls_NewsRoom_NewsRoom(string a)
    {
        try
        {
            string[] s = a.Split(new char[] { ',' });
            records = Convert.ToInt32(s[0]);

            if (s.Length > 1)
                publish = s[1];
        }
        catch
        {
            records = 0;
        }
    }

    protected int total
    {
        set { ViewState["total"] = value; }
        get { 
                try { return Convert.ToInt32(ViewState["total"]); }
                catch { return records; } 
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

    protected void Page_Load(object sender, EventArgs e)
    {
        //this.Page.MaintainScrollPositionOnPostBack = true;
#if LIKE_IT
        litjsLikeIt.Text = "";
#endif
        if (!Page.IsPostBack)
        {
            total = records;

            if (Request.QueryString["newsid"] != null)
            {
                singleitem.Visible = true;
                DataList1.Visible = false;
                BindItem();
            }
            else
            {
                singleitem.Visible = false;
                //if (records == 0)
                {
                    DataList1.Visible = true;
                }
                //else
                //{
                //    DataList1.Visible = false;
                //}
                this.PopulateDates();
                this.PopulateCategories();

                ////if(Request.QueryString["ctg"] != null)
                ////{
                ////    int c = 0;
                ////    if (int.TryParse(Request.QueryString["ctg"], out c))
                ////    {
                ////        try { ddlCategories.SelectedValue = c.ToString(); }
                ////        catch { }
                ////    }
                ////}

                BindData();
            }

        }

    }

    private void PopulateCategories()
    {
        //DataTable dt = new DataTable();
        //SqlDataAdapter da = new SqlDataAdapter("select * from NewsCategories where deleted=0", _connection);
        //da.SelectCommand.CommandType = CommandType.Text;
        //da.Fill(dt);

        //ddlCategories.DataSource = dt;
        //ddlCategories.DataBind();
        //ddlCategories.Items.Insert(0, new ListItem("All Items", ""));
    }

    public void BindItem()
    {
        //DataTable dt = getTablei("NewsRoomSelect");
        DataTable dt = getTablei("NewsRoomSelect_New");
        DataRow rw;
        if (dt.Rows.Count > 0)
        {
            rw = dt.Rows[0];
            litDate.Text = ((DateTime)rw["NewsDate"]).ToString("MMMM dd, yyyy");
            litTitle.Text = rw["Title"].ToString();
            //if(imgPhoto1.Visible = rw["MIMEType"].ToString() != "")
            //    imgPhoto1.ImageUrl = "/ShowPicture.aspx?PictureID=" + rw["id"].ToString();

#if LIKE_IT
            // if (trLikeit.Visible = (rw["layout"].ToString() != "3"))
            {
                litjsLikeIt.Text = "<script src=\"/Controls/Newsroom/LikeNews.js\" type=\"text/javascript\"></script>";
            }
#endif
#if SPLIT_CONTENT

            InsertWidget(rw, plContent);
#else
            litDetails.Text = rw["Details"].ToString();

#endif
        }
        else
        {
            Response.Redirect(CMSHelper.GetLanguagePrefix() + newspage);
        }
        trDates.Visible = false;
    }

#if SPLIT_CONTENT
    private int InsertWidget(DataRow dr, PlaceHolder plContent)
    {
        string patern = "&lt;widget id='";
        string patern2 = "' class=";
        string patern3 = "/&gt;";
        string content = dr["Details"].ToString();
        string remaning = "";
        int npos = content.IndexOf(patern);
        int i = 0, limit = 10;
        while (npos >= 0)
        {
            string s = content.Substring(0, npos);

            AddHtmlContent(plContent, s);

            remaning = content.Substring(npos + patern.Length);
            int npos2 = remaning.IndexOf(patern2);

            if (npos2 >= 0)
            {
    #region Insert Widget
                string id = remaning.Substring(0, npos2);
                //AddHtmlContent(plContent, "<br><strong>" + id.ToString() + "</strong>");

                DataTable dt = new DataTable();
                string sqlstr = @" select * from Content where id=@id";
                SqlCommand sq = new SqlCommand(sqlstr);
                sq.Parameters.AddWithValue("@id", id);

                dt = getTable(sq);
                if (dt.Rows.Count > 0)
                {
                    string control = dt.Rows[0]["control"].ToString();
                    UserControl userControl = LoadControlExtension.LoadControl(this, "~/Controls/" + control + "/" + control + ".ascx", dt.Rows[0]["param"].ToString());
                    plContent.Controls.Add(userControl);
                }

    #endregion

                int npos3 = remaning.IndexOf(patern3);
                if (npos3 < 0)
                {
                    patern3 = "&lt;/widget&gt;";
                    npos3 = remaning.IndexOf(patern3);
                }
                remaning = remaning.Substring(npos3 + patern3.Length);
                npos = remaning.IndexOf(patern);
                if (npos < 0)
                {
                    i++;
                    AddHtmlContent(plContent, remaning);
                    break;
                }

            }
            else
            {
                i++;
                AddHtmlContent(plContent, remaning);
                break;
            }

            content = remaning;

            i++;
            if (i > limit)
                break;
        }

        if (i == 0)
        {
            AddHtmlContent(plContent, dr["Details"].ToString());
        }

        return i;
    }

    private void AddHtmlContent(PlaceHolder plContent, string text)
    {
        Literal litContent = new Literal();
        litContent.Text = text;
        plContent.Controls.Add(litContent);
    }

    private DataTable getTable(SqlCommand cmd)
    {
        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.SelectCommand.CommandType = CommandType.Text;
        da.SelectCommand.Connection = new SqlConnection(_connection);
        da.Fill(dt);
        return dt;
    }
#endif
    private DataTable getTablei(string cmd)
    {
        //string Remote_Addr = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString().Trim();

//string myIP = Remote_Addr;

//if (myIP.Contains("72.141.12.194")             // Juan
//    || myIP.Contains("76.66.143.113")          //Vivian
//    )
//{
//    cmd = "NewsRoomSelect_New_Preview";
//}

#if PRE_VIEW
        if (Request.QueryString["pv"] != null && Request.QueryString["pv"] == "1" && Session["LoggedInID"] != null)
        {
            DataTable dtu = new DataTable();
            SqlDataAdapter dat = new SqlDataAdapter(@"SELECT * FROM Users_Groups_Access
                                                      where User_id=@userid 
                                                      and Group_id in (select Group_id from Pages_Group where Page_id in (select id from pages where seo = @seo))
                                                      and Access_Level = 4", _connection);
            dat.SelectCommand.CommandType = CommandType.Text;
            dat.SelectCommand.Parameters.AddWithValue("@userid", Session["LoggedInID"].ToString());
            dat.SelectCommand.Parameters.AddWithValue("@seo", this.Page.RouteData.Values["seo"]);
            dat.Fill(dtu);
            if (dtu.Rows.Count == 1 
                //|| Session["LoggedInID"].ToString() == "1"
                )
                cmd = "NewsRoomSelect_New_Preview";

        }
#endif

        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter(cmd, _connection);
        da.SelectCommand.CommandType = CommandType.StoredProcedure;
        da.SelectCommand.Parameters.AddWithValue("@Lang", Language);
		da.SelectCommand.Parameters.AddWithValue("@FeatureType", 0);
		da.SelectCommand.Parameters.AddWithValue("@publish", publish);
        
        if (Request.QueryString["newsid"] != null)
        {
            int id = 0;
            if (int.TryParse(Request.QueryString["newsid"], out id))
            {
                da.SelectCommand.Parameters.AddWithValue("@id", id);
            }

            //Response.Write("[" + Session["Language"].ToString() + "][" + Request.QueryString["newsid"] + "]");
        }
        da.Fill(dt);
        return dt;
    }

    public bool bLoadMore = false; 
    private void BindData()
    {
        DataTable dt = new DataTable();
        dt = getTable(sql);
        //if (dt.Rows.Count > 0)
        {
            DataView DV = dt.DefaultView;

            this.DataList1.DataSource = DV;
            this.DataList1.DataBind();
        }

        if (bLoadMore = (dt.Rows.Count >= 6))
        {
            ((_Default)this.Page).InjectContent("Scripts", "<script src=\"/controls/Newsroom/LoadNews.js\"></script>");
        }

    }
#region dal

    protected string _connection = ConfigurationManager.AppSettings.Get("CMServer");
    protected string sql = "NewsRoomSelect_New";      //"NewsRoomSelect";

    private DataTable getTable(string cmd)
    {
        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter(cmd, _connection);
        da.SelectCommand.CommandType = CommandType.StoredProcedure;
    	da.SelectCommand.Parameters.AddWithValue("@Lang", Language);
		da.SelectCommand.Parameters.AddWithValue("@FeatureType", 0);
        da.SelectCommand.Parameters.AddWithValue("@publish", publish);

        //Response.Write(Session["Language"].ToString());
  //      if (records > 0)
		//{
		//	da.SelectCommand.Parameters.AddWithValue("@top", records);
		//	//Response.Write("A");
		//}

        if (ddlDateSelect.Items.Count > 0 && ddlDateSelect.SelectedValue != "")
        {
            string stardendyear = this.ddlDateSelect.SelectedItem == null ? this.ddlDateSelect.Items[0].Value : this.ddlDateSelect.SelectedItem.Value;
            string startyear = stardendyear.Split('A')[0];
            string endyear = stardendyear.Split('A')[0];
            DateTime startdate = new DateTime(int.Parse(startyear), 1, 1);
            DateTime enddate = new DateTime(int.Parse(endyear), 12, 31);
            da.SelectCommand.Parameters.AddWithValue("@startDate", startdate);
            da.SelectCommand.Parameters.AddWithValue("@endDate", enddate);
        }

        ////if(ddlCategories.Items.Count > 0  )
        ////{
        ////    if (ddlCategories.SelectedValue != "")
        ////        da.SelectCommand.Parameters.AddWithValue("@Category", ddlCategories.SelectedValue);

        ////}

        da.Fill(dt);
        return dt;
    }
#endregion dal

#region properties
    /*protected string Language
    {
        get
        {
            string[] lang = { "fr", "en", "fr" };
            return lang[Convert.ToInt32(Session["Language"])];
        }
    }*/

    protected string More
    {
        get
        {
            string[] more = { "suite", "more", "suite" };
            return more[Convert.ToInt32(Language)];
        }
    }
#endregion properties


    private void PopulateDates()
    {

        List<DateTime> dates = new List<DateTime>();
        List<string> dateStrings = new List<string>();
        for (int year = DateTime.Now.Year; year >= DateTime.Now.AddYears(-3).Year; --year)
        {
            dateStrings.Add(year.ToString());//+"/"+(year+1).ToString());
        }

        foreach (string s in dateStrings)
        {
            this.ddlDateSelect.Items.Add(new ListItem() { Text = s, Value = s.Replace('/', 'A') });
        }
        this.ddlDateSelect.Items.Insert(0,(new ListItem() { Text = "All", Value = "" }));

        this.ddlDateSelect.Items[0].Selected = true;
    }
    protected void ddlDateSelect_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.BindData();

    }
    protected void DataList1_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.DataItem != null)
        {
            DataRowView dr = (DataRowView)e.Item.DataItem;

           
            HtmlAnchor theLink = (HtmlAnchor)e.Item.FindControl("theLink");


            theLink.Attributes.Add("title", dr["title"].ToString());

            //string prefix = "/en/";
            //if (Session["Language"].ToString() == "2")
            //    prefix = "/fr/";
            string prefix = CMSHelper.GetLanguagePrefix();

            if (dr["type"].ToString() == "0")
            {
                //theLink.HRef = prefix + (publish == "3" ? "membernews?newsid=" : "newsroom?newsid=") + dr["linkid"].ToString();
                theLink.HRef = "/" + newspage + "?newsid=" + dr["linkid"].ToString();
            }
            else if (dr["type"].ToString() == "1")
            {
               // theLink.Attributes.Add("onclick", "window.open('" + NewsroomFilesPath + dr["filename"].ToString() + "', null, 'status=no, toolbar=no, menubar=no, location=no, scrollbars=yes, resizable'); return false;");
                theLink.Attributes.Add("class", "newsitem read_more open_new_tab");
                theLink.Attributes.Add("filename", NewsroomFilesPath + dr["filename"].ToString());
                theLink.HRef = "#?" + dr["id"].ToString();
            }
            else 
            {
                if (dr["seo"].ToString() != "")
                {
                    //theLink.Attributes.Add("onclick", "window.location='/" + dr["seo"].ToString() + "'; return false;");
                    theLink.HRef = "/" + dr["seo"].ToString();
                }
                else
                {
                    // theLink.Attributes.Add("onclick", "window.open('" + dr["ExternalURL"].ToString() + "', null, 'status=no, toolbar=no, menubar=no, location=no, scrollbars=yes, resizable'); return false;");
                    theLink.HRef = dr["ExternalURL"].ToString();
                    theLink.Target = "_blank";

                }
            }            

            trDates.Visible = true;

#region Photo
            System.Web.UI.WebControls.Image imgPhoto = (System.Web.UI.WebControls.Image)e.Item.FindControl("imgPhoto");
            if (imgPhoto != null)
            {
                if (imgPhoto.Visible = dr["MIMEType"].ToString() != "")
                {
                    imgPhoto.ImageUrl = "/Controls/Newsroom/ThumbNail.ashx?PictureID=" + dr["id"].ToString() + "&maxsz=125";

                }
                else
                {
                    imgPhoto.Visible = true;
                    imgPhoto.ImageUrl = "/images/base/newsplaceholder.jpg";
                    imgPhoto.AlternateText = "";

                }
            }
            ////imgPhoto = (System.Web.UI.WebControls.Image)e.Item.FindControl("imgPhoto2");
            ////if (imgPhoto != null)
            ////{
            ////    if (imgPhoto.Visible = dr["MIMEType"].ToString() != "")
            ////    {
            ////        imgPhoto.ImageUrl = "/ThumbNail.ashx?PictureID=" + dr["id"].ToString() + "&maxsz=125";

////    }
////    else
////    {
////        imgPhoto.Visible = true;
////        //imgPhoto.ImageUrl = "/images/wrh/untitled.png";
////        imgPhoto.ImageUrl = "/images/perth/newsplaceholder.jpg";
////        imgPhoto.AlternateText = "";

////    }
////}

#endregion

#if LIKE_IT

            int likes = 0;
            int.TryParse(dr["Likes"].ToString(), out likes);
            if (likes > 0)
            {
                Literal litLikeIt = (Literal)e.Item.FindControl("litLikeIt");
                litLikeIt.Text = String.Format("<img src='/Images/Icons/thumb-up-16_blue.png' alt='Like it' class='likeit' />&nbsp;<label>{0}</label>", likes.ToString()); ;
            }
#endif

            ////string[] categories = dr["Categories"].ToString().Split(';');
            ////string categoryLinks = string.Empty;
            ////string[] category;
            ////foreach (string cat in categories)
            ////{
            ////    category = cat.Split(',');
            ////    if (category.Length > 1)
            ////        categoryLinks += string.Format("<a href=\"/newsroom?ctg={1}\">{0}</a>", category[0], category[1]) + ", ";
            ////}
            ////categoryLinks = categoryLinks.Trim().TrimEnd(',');

            ////Label lblCategories = (Label)e.Item.FindControl("lblCategories");
            ////lblCategories.Text = categoryLinks != "" ? " in " + categoryLinks : "";

            ////lblCategories = (Label)e.Item.FindControl("lblCategories2");
            ////lblCategories.Text = categoryLinks != "" ? " in " + categoryLinks : "";
        }
    }

    ////protected void ddlCategories_SelectedIndexChanged(object sender, EventArgs e)
    ////{
    ////    this.BindData();
    ////}
}
