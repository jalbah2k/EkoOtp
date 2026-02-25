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

    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            
            
            {
                
                //GetMenuTitle();
                //GetPageTitle();
            }

            
        }

    }

    //private string GetPageTitle()
    //{
    //    SqlConnection sqlconn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);

    //    SqlDataAdapter dapt = new SqlDataAdapter("select title from Pages where id=@id", sqlconn);
    //    dapt.SelectCommand.Parameters.AddWithValue("@id", Session["PageID"].ToString());
    //    DataTable dt = new DataTable();
    //    dapt.Fill(dt);

    //    string s = "";
    //    if (dt.Rows.Count > 0)
    //    {
    //        litPageTitle.Text = dt.Rows[0]["title"].ToString();
    //        s = dt.Rows[0]["title"].ToString();
    //    }
    //    return s;

    //}
    private void GetMenuTitle()
    {
        //SqlConnection sqlconn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);

        //SqlDataAdapter dapt = new SqlDataAdapter("select name from Menus where id = (select param from Content where control='menu' and id = (select Content_ID from pages_content_zone where Page_ID=@id and Zone_ID=24))  select title, seo, InsideClass from Pages where id=@id", sqlconn);
        //dapt.SelectCommand.Parameters.AddWithValue("@id", Session["PageID"].ToString());
        //DataSet ds = new DataSet();
        //dapt.Fill(ds);
        //DataTable dt = ds.Tables[0];

        //if (dt.Rows.Count > 0)
        //    litMenuTitle.Text = "<h2>" + dt.Rows[0]["name"].ToString() + "</h2>";

       
    }

  

   
}
