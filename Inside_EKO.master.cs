using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

public partial class Inside : System.Web.UI.MasterPage
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
        set { ViewState["mypage"] = value;}
        get { return ViewState["mypage"].ToString(); }
    }

    public string InsideClass
    {
        set { ViewState["InsideClass"] = value; }
        get { return ViewState["InsideClass"].ToString(); }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
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
            
            {
                
                GetMenuTitle();
                GetPageTitle();
            }

            ////if (Session["SearchTerm"] != null)
            ////    tbSearchMob.Text = Session["SearchTerm"].ToString();
            ////else
            ////{
            ////    switch (Convert.ToInt32(Language))
            ////    {
            ////        case 1:
            ////            tbSearchMob.Text = enSearch;
            ////            break;
            ////        case 2:
            ////            tbSearchMob.Text = frSearch;
            ////            break;
            ////    }

            ////}

            ////if (Request.QueryString["kw"] != null)
            ////    tbSearchMob.Text = Request.QueryString["kw"];
        }

        //lnkShareEmail.HRef += "mailto:?subject=Sharing this page&amp;body=" + Server.HtmlEncode(Request.Url.AbsoluteUri);
        //lnkShareEmail.HRef += String.Format("mailto:?subject=Erieshores Regional Hospital: {0}&amp;body={1}",
        //                       GetPageTitle(),
        //                       String.Format("From Perth & Smith Falls District Hospital: {0}", Server.HtmlEncode(Request.Url.AbsoluteUri)));

    }

    private string GetPageTitle()
    {
        SqlConnection sqlconn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);

        SqlDataAdapter dapt = new SqlDataAdapter("select title from Pages where id=@id", sqlconn);
        dapt.SelectCommand.Parameters.AddWithValue("@id", Session["PageID"].ToString());
        DataTable dt = new DataTable();
        dapt.Fill(dt);

        string s = "";
        if (dt.Rows.Count > 0)
        {
            litPageTitle.Text = "Jump to page in section:";     // dt.Rows[0]["title"].ToString();
            s = dt.Rows[0]["title"].ToString();
        }
        return s;

    }
    private void GetMenuTitle()
    {
        SqlConnection sqlconn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);

        SqlDataAdapter dapt = new SqlDataAdapter("select name from Menus where id = (select param from Content where control='menu' and id = (select Content_ID from pages_content_zone where Page_ID=@id and Zone_ID=24))  select title, seo, InsideClass from Pages where id=@id", sqlconn);
        dapt.SelectCommand.Parameters.AddWithValue("@id", Session["PageID"].ToString());
        DataSet ds = new DataSet();
        dapt.Fill(ds);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count > 0)
            litMenuTitle.Text = "<h2>" + dt.Rows[0]["name"].ToString() + "</h2>";

        if (ds.Tables[1].Rows.Count > 0)
        {
            mypage = ds.Tables[1].Rows[0]["seo"].ToString();
            InsideClass = ds.Tables[1].Rows[0]["InsideClass"].ToString();
        }

    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (mypage.ToLower() == "login" ||
            mypage.ToLower() == "changepassword" ||
            mypage.ToLower() == "forgotpassword" ||
            mypage.ToLower() == "calendar"
            )
        {
            ContentTBL.Visible = false;
            WidgetsToolbar1.Visible = false;
        }

        //if (!InsideMenu)
        {
            StringBuilder script = new StringBuilder();
            script.Append(Environment.NewLine + "$(document).ready(function () {" + Environment.NewLine);
            //script.Append(Environment.NewLine + "$(body).addClass(\"no-inside-menu\").removeClass(\"yes-inside-menu\");" + Environment.NewLine);
            script.Append(Environment.NewLine + String.Format("$(body).addClass(\"{0}\");", InsideClass) + Environment.NewLine);
            if (Session["LoggedInID"] != null)
            {
                script.Append(Environment.NewLine + String.Format("$(body).addClass(\"{0}\");", "logged-in") + Environment.NewLine);
            }
            //if(InsideClass.Contains("no-inside-menu"))
            //{
            //    script.Append(Environment.NewLine + "$('#leftMenu').css(\"display\", \"none;\");" + Environment.NewLine);
            //}
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
    protected void Search(object sender, EventArgs e)
    {
        //if (tbSearchMob.Text.Trim().Length > 0 && tbSearchMob.Text != enSearch && tbSearchMob.Text != frSearch)
        //{
        //    Session["SearchTerm"] = tbSearchMob.Text;            
        //    Response.Redirect(LangPrefix + "search");
        //}
        //else
        //{
        //    Session["SearchTerm"] = null;
        //}
    }

}
