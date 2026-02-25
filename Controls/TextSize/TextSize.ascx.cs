using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Controls_TextSize_TextSize : System.Web.UI.UserControl
{
    public string Language
    {
        get
        {
            return Session["Language"] != null ? Session["Language"].ToString() : "1";
        }
    }
    
    protected void Page_Load(object sender, EventArgs e)
    {

    }
}