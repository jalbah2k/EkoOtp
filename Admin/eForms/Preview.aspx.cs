using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_eForms_Preview : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["LoggedInID"] == null)
        {
            if (Request.QueryString["previewid"] != null)
                Response.Redirect("/login.aspx?r=/admin/eforms/preview.aspx?previewid=" + CMSHelper.GetCleanQueryString("previewid"));
            else
                Response.Redirect("/");
        }
        
        Session["Language"] = "1";
        Session["PageID"] = "1";
        Session["PageId"] = "1";

        if (!IsPostBack)
        {
            if (Request.QueryString["previewid"] != null)
                eForm1.param = CMSHelper.GetCleanQueryString("previewid");
            else
                eForm1.Visible = false;
        }
    }
}