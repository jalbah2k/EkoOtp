using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;




public partial class Controls_LangSelect_Selector : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
		if (!IsPostBack)
		{
			if (Session["Language"] == null)
				Session["Language"] = "1";
			if(Session["Language"].ToString()=="1")
			{
				btnEn.Visible = false;
				btnFr.Visible = true;
			}
			else
			{
				btnEn.Visible = true;
				btnFr.Visible = false;
				
			}
		}
    }

	public void GoFrench(object s, EventArgs e)
	{
		Session["Language"] = "2";
		btnEn.Visible = true;
		btnFr.Visible = false;
		this.Page.Culture = "fr-CA";
	}

	public void GoEnglish(object s, EventArgs e)
	{
		Session["Language"] = "1";
		btnEn.Visible = false;
		btnFr.Visible = true;
		this.Page.Culture = "en-CA";
	}
}
