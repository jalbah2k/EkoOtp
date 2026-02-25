using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class Content : System.Web.UI.UserControl
{
    public string Parameters;

    public Content()
    {
    }

    public Content(string Params)
    {
        Parameters = Params;
    }
	protected override void OnPreRender(EventArgs e)
	{
        //ScriptManager.RegisterClientScriptInclude(this, this.GetType(), "gAJS", ResolveClientUrl("/js/greybox/AJS.js"));
        //ScriptManager.RegisterClientScriptInclude(this, this.GetType(), "gAJSfx", ResolveClientUrl("/js/greybox/AJS_fx.js"));
        //ScriptManager.RegisterClientScriptInclude(this, this.GetType(), "gScripts", ResolveClientUrl("/js/greybox/gb_scripts.js"));

		base.OnPreRender(e);
	}

    protected void Page_Load(object sender, EventArgs e)
    {
		//if (!IsPostBack)
			DisplayContent();
    }

	private void DisplayContent()
	{
		bool admin = IsEditor();

		if (admin)
		{
			//trAdmin.Visible = true;
			//tblContent.Style.Clear();
		}


		SqlDataAdapter dapt = new SqlDataAdapter("CONTROL_Content_Get", ConfigurationManager.AppSettings["CMServer_External"]); //TODO: Redo as a Command, not Dapt
        dapt.SelectCommand.CommandType = CommandType.StoredProcedure;

        dapt.SelectCommand.Parameters.AddWithValue("@id", Parameters);

        DataSet ds = new DataSet();

        dapt.Fill(ds);

        int ntab = 0;
        if (Session["LoggedInID"] != null)
        {
            if (!(Request.QueryString["pv"] == null) && Request.QueryString["pv"] == "1")
            {
                if (ds.Tables[1].Rows.Count > 0)
                    ntab = 1;
            }
        }

        if (ds.Tables[0].Rows.Count == 0)
            return;

        litContent.Text = ds.Tables[ntab].Rows[0][0].ToString();

   //     Response.Write("Id:" + Session["PageID"].ToString());

        if (Session["LoggedInID"]!=null)
        {

            if (ds.Tables[1].Rows.Count > 0 && admin)
            {
                litEdit.Text = String.Format("<a onmouseover=\"MM_swapImage('Image3','','/images/lemonaid/buttons/editadmin.png',0)\" onmouseout=\"MM_swapImgRestore()\" href='/controls/content/edit.aspx?id={0}&PageId={1}' title=\"Content Editor\" class=\"Tibby_Bottom_Button_Enabled_NotHighlighted\" style=\"color:#ffffff\" target=\"_blank\"><img id=\"Image3\" name=\"Image3\" alt=\"\" src=\"/images/lemonaid/buttons/editadmin_over.png\" border=\"0\" style=\"height:30px; width:75px;\"></a>", Parameters,  Session["PageID"].ToString());
            }
            else if (admin)
            {
                litEdit.Text = String.Format("<a onmouseover=\"MM_swapImage('Image3','','/images/lemonaid/buttons/editadmin_over.png',0)\" onmouseout=\"MM_swapImgRestore()\" href='/controls/content/edit.aspx?id={0}&PageId={1}' title=\"Content Editor\" class=\"Tibby_Bottom_Button_Enabled_NotHighlighted\" style=\"color:#ffffff\" target=\"_blank\"><img id=\"Image3\" name=\"Image3\" alt=\"\" src=\"/images/lemonaid/buttons/editadmin.png\" border=\"0\" style=\"height:30px; width:75px;\"></a>", Parameters, Session["PageID"].ToString());
            }
        }

	}


	public void clickedApprove(object o, EventArgs e)
	{
		

	}

	private bool IsEditor()
	{
		int page = Convert.ToInt32(Session["PageID"]);
		int user = Convert.ToInt32(Session["LoggedInID"]);

		if (Permissions.Get(user, page) > 1)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

}
