using System;



public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        UrlParameterWhitelistValidator validator = new UrlParameterWhitelistValidator();
        var filteredQueryString = validator.ValidateAndFilter(this);

        if (Request.QueryString["c"] != null)
            Response.Redirect("/login?c=" + Request.QueryString["c"]);
        else
            Response.Redirect("/login");


    }

}
