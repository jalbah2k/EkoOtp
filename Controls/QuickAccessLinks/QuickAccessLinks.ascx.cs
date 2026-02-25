using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Controls_QuickAccessLinks_QuickAccessLinks : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    public bool IsEditor()
    {
        if (Session["LoggedInID"] != null)
        {
            int page = Convert.ToInt32(Session["PageID"]);
            int user = Convert.ToInt32(Session["LoggedInID"]);

            if (user == 1 || Permissions.Get(user, page) > 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}