
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Controls_SearchText_SearchText : System.Web.UI.UserControl
{
    public string enSearch = "Search our site";
    public string frSearch = "Recherche sur le site...";

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

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
          
            if (Request.QueryString["q"] != null)
                 tbSearch.Text = Request.QueryString["q"].Replace("(", "").Replace(")", "").Replace("+", "").Trim();
            else
            {
                switch (Convert.ToInt32(Language))
                {
                    case 1:
                        tbSearch.Text = enSearch;
                        break;
                    case 2:
                        tbSearch.Text = frSearch;
                        break;
                }
            }

            if (Request.QueryString["kw"] != null)
            {
                tbSearch.Text = QueryStringHelper.AntiXssEncoder_HtmlEncode(Request.QueryString["kw"].ToString().Trim(), true);
            }
        }

    }

    protected void Search(object sender, EventArgs e)
    {
        if (tbSearch.Text.Trim().Length > 0 && tbSearch.Text != enSearch && tbSearch.Text != frSearch)
        {
			string term = QueryStringHelper.AntiXssEncoder_HtmlEncode(tbSearch.Text.Trim(), true);
			
            if (((_Default)this.Page)._topmenuid == "5" || Session["MemberId"] != null)
                Response.Redirect(LangPrefix + "membersearch?q=" + Server.UrlEncode(term));
            else
                Response.Redirect(LangPrefix + "search?q=" + Server.UrlEncode (term));
        }
       
    }
}
