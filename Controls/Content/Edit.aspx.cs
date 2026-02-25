using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;


public partial class Controls_Content_Edit : System.Web.UI.Page
{
    //private bool lookatlive;

    protected void Page_Load(object sender, EventArgs e)
    {
        UrlParameterWhitelistValidator validator = new UrlParameterWhitelistValidator();
        validator.ValidateAndFilter(this);

        // check if user is still authenticated but session variable is expired
        CMSHelper.AutoLogin();

        if (!IsPostBack)
        {
           // Response.Write(Session["PageID"].ToString());

            //if (Request.QueryString["PageID"] == null)
            //    Response.Redirect("/controls/content/edit.aspx?id=" + Request.QueryString["id"] + "&PageID=" + Session["PageID"].ToString());
        }

    }
    protected void btnDone_Click(object sender, EventArgs e)
    {
        //https://developer.mozilla.org/en-US/docs/Web/API/Window/opener
        //From Firefox 79, windows opened because of links with a target of _blank don't get an opener, unless explicitly requested with rel=opener.

        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "closeScript", "if (window.opener.location.href.indexOf(\"admin.aspx\") < 0) { window.opener.location.reload(); } window.open('','_self','');window.close();", true);
    }
}
