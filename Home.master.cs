using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Home : System.Web.UI.MasterPage
{
    //public string enSearch = "SEARCH INTRANET...";
    //public string frSearch = "Recherche sur le site...";

    public string LangPrefix
    {
        get
        {
            return CMSHelper.GetLanguagePrefix();
        }
    }
    public string Language
    {
        get
        {
            //return CMSHelper.GetCleanQueryString("lang", "1");
            return CMSHelper.GetLanguageNumber();
        }
    }

    public string mypage
    {
        set { ViewState["mypage"] = value; }
        get { return ViewState["mypage"].ToString(); }
    }
    public string InsideClass
    {
        set { ViewState["InsideClass"] = value; }
        get { return ViewState["InsideClass"].ToString(); }
    }
    public string ImageBanner
    {
        set { ViewState["ImageBanner"] = value; }
        get { return ViewState["ImageBanner"].ToString(); }
    }

    public string BackgroundBannerPosition
    {
        set { ViewState["BackgroundBannerPosition"] = value; }
        get { return ViewState["BackgroundBannerPosition"].ToString(); }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            if (Session["LoggedInID"] != null)
            {
                sessioncontrol.Visible = true;

                if (Session["LoggedInID"] != null)
                {
                    sessioncontrol.Visible = true;

                    int userid = int.Parse(Session["LoggedInID"].ToString());
                    if (((Permissions.Get(userid, int.Parse(Session["PageID"].ToString())) > 1) && Permissions.ManageArea(userid))
                        || userid == 1)
                    {
                        ContentTBL.Visible = true;
                         WidgetsToolbar1.Visible = true;
                    }
                }
                else
                {
                    ContentTBL.Visible = false;
                    WidgetsToolbar1.Visible = false;
                }
               


            }

            GetImageBanner();

        }

    }

    private void GetImageBanner()
    {
        SqlConnection sqlconn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);

        SqlDataAdapter dapt = new SqlDataAdapter(@"select title, seo, InsideClass, ImageBanner, BackgroundBannerPosition, BackgroundBannerPosition_Horizontal from Pages where id=@id", sqlconn);
        dapt.SelectCommand.Parameters.AddWithValue("@id", Session["PageID"].ToString());
        DataSet ds = new DataSet();
        dapt.Fill(ds);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            mypage = dr["seo"].ToString().ToLower();

            InsideClass = dr["InsideClass"].ToString();
            if (String.IsNullOrEmpty(dr["ImageBanner"].ToString()))
                ImageBanner = "/Images/EKO/inside-banner.jpg";
            else
                ImageBanner = dr["ImageBanner"].ToString();

            BackgroundBannerPosition = "background-position:" + dr["BackgroundBannerPosition"].ToString() + " " +
                                            dr["BackgroundBannerPosition_Horizontal"].ToString() + ";";
        }


    }
    protected void Search(object sender, EventArgs e)
    {
        ////////if (tbSearchMob.Text.Trim().Length > 0 && tbSearchMob.Text != enSearch && tbSearchMob.Text != frSearch)
        ////////{
        ////////    Session["SearchTerm"] = tbSearchMob.Text;
        ////////    //string _language = "en";

        ////////    //switch (Convert.ToInt32(Session["Language"]))
        ////////    //{
        ////////    //    case 1:
        ////////    //        _language = "en";
        ////////    //        break;
        ////////    //    case 2:
        ////////    //        _language = "fr";
        ////////    //        break;
        ////////    //    default:
        ////////    //        _language = "en";
        ////////    //        break;
        ////////    //}

        ////////    //string _redirect = string.Format("/{0}/search", _language);

        ////////    //Response.Redirect(_redirect);
        ////////    Response.Redirect(LangPrefix + "search");
        ////////}
        ////////else
        ////////{
        ////////    Session["SearchTerm"] = null;
        ////////}
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
       
        if (Session["LoggedInID"] != null)
        {
            StringBuilder script = new StringBuilder();
            script.Append(Environment.NewLine + "$(document).ready(function () {" + Environment.NewLine);
            script.Append(Environment.NewLine + String.Format("$(body).addClass(\"{0}\");", "logged-in") + Environment.NewLine);
            script.Append(Environment.NewLine + "});" + Environment.NewLine);
            InjectContent(Scripts, script.ToString(), true);
        }
    }

    public void InjectContent(ContentPlaceHolder placeholder, string content, bool addScriptTags)
    {
        Literal lit = new Literal();
        if (addScriptTags)
            lit.Text = "<script>" + content + "</script>";
        else
            lit.Text = content;

        placeholder.Controls.Add(lit);
    }
}
