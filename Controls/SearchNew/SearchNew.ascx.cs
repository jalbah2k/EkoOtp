using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SearchNew : System.Web.UI.UserControl
{
    public string Parameters;
    public SearchNew () { }
    public SearchNew (string p) 
    {
       
    }
    private string SearchTerm
    {
        set { ViewState["SearchTerm"] = value; }
        get
        {
            if (ViewState["SearchTerm"] != null)
            {
                return ViewState["SearchTerm"].ToString();
            }
            else
                return "";
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!Page.IsPostBack)
        {
            if (Request.QueryString["q"] != null)
                SearchTerm = Request.QueryString["q"];
        }

        litSubtitle.Text = "";
        if (!String.IsNullOrEmpty(SearchTerm))
            litSubtitle.Text = String.Format("<p><strong>Your search for keyword(s) '{0}' produced:</strong></p>", SearchTerm);
    }
}