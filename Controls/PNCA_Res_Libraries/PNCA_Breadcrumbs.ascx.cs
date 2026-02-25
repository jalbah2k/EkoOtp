using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Breadcrumbs : System.Web.UI.UserControl
{
    public string Content
    {
        set { litContent.Text = value; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {

    }


    protected void btnSearchRes_Click(object sender, EventArgs e)
    {
        Response.Redirect("/" + ConfigurationManager.AppSettings["Resources.Page"] + "?search_term=" + txtSearch.Text.Trim() + "&save=1");
    }
}