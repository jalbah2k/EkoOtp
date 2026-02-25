using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

public partial class EKOResources : System.Web.UI.MasterPage
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

    protected void Page_Load(object sender, EventArgs e)
    {
        

    }

    
    protected void Page_PreRender(object sender, EventArgs e)
    {
       
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
