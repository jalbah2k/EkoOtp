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


    protected void Page_Load(object sender, EventArgs e)
    {
       // if (!IsPostBack)
        {
            if (Session["LoggedInID"] == null)
                this.Visible = false;
         
        }
  
    }

}
