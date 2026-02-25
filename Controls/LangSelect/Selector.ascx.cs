#define MULTI_LANGUAGE
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
#if MULTI_LANGUAGE
		if (!IsPostBack)
		{
			SqlDataAdapter dapt =
				new SqlDataAdapter(
					"select seo, active from pages where language=1 and linkid=(select linkid from pages where id=" +
					Session["PageID"].ToString() +
					") select seo, active from pages where language=2 and linkid=(select linkid from pages where id=" +
					Session["PageID"].ToString() + ")", ConfigurationManager.AppSettings["CMServer"]);
			DataSet ds = new DataSet();
			dapt.Fill(ds);


			if (Session["Language"].ToString() == "1" && ds.Tables[1].Rows.Count == 1)
			{
				if (litLinks.Visible = (Convert.ToBoolean(ds.Tables[1].Rows[0]["active"]) || Session["LoggedInID"] != null))
				{
					//litLinks.Text += "<a class=\"lang_selector\" href=\"/fr/" + ds.Tables[1].Rows[0]["seo"].ToString() + "\">FR</a>";
					
					litLinks.Text = "<a href=\"/fr/" + ds.Tables[1].Rows[0]["seo"].ToString() + "\" class=\"toplinks\" \">français</a>";

					Session["ShowLangSel"] = "true";
				}
			}
			else if (Session["Language"].ToString() == "2" && ds.Tables[0].Rows.Count == 1)
			{
				if (litLinks.Visible = (Convert.ToBoolean(ds.Tables[0].Rows[0]["active"]) || Session["LoggedInID"] != null))
				{
					litLinks.Text = "<a href=\"/" + ds.Tables[0].Rows[0]["seo"].ToString() + "\" class=\"toplinks\" \">english</a>";
					Session["ShowLangSel"] = "true";

				}

			}




			if (Session["Language"].ToString()=="1")
			{
				lang.Text = "Language";
			}
			else
			{
				lang.Text = "Langue";
			}
		}
#endif
	}
}
