#define SPLIT_CONTENT
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


		SqlDataAdapter dapt = new SqlDataAdapter("CONTROL_Content_Get", ConfigurationManager.AppSettings["CMServer"]); //TODO: Redo as a Command, not Dapt
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

#if SPLIT_CONTENT
        InsertWidget(ds.Tables[ntab].Rows[0][0].ToString(), plContent);
#else
        litContent.Text = ds.Tables[ntab].Rows[0][0].ToString();
#endif

        //     Response.Write("Id:" + Session["PageID"].ToString());

        if (Session["LoggedInID"]!=null)
        {
            //https://developer.mozilla.org/en-US/docs/Web/API/Window/opener
            //From Firefox 79, windows opened because of links with a target of _blank don't get an opener, unless explicitly requested with rel=opener.

            if (ds.Tables[1].Rows.Count > 0 && admin)
            {
                litEdit.Text = String.Format("<a onmouseover=\"MM_swapImage('Image3','','/images/lemonaid/buttons/editadmin.png',0)\" onmouseout=\"MM_swapImgRestore()\" href='/controls/content/edit.aspx?id={0}&PageId={1}' rel='opener' title=\"Content Editor\" class=\"Tibby_Bottom_Button_Enabled_NotHighlighted\" style=\"color:#ffffff\" target=\"_blank\"><img id=\"Image3\" name=\"Image3\" alt=\"\" src=\"/images/lemonaid/buttons/editadmin_over.png\" border=\"0\" style=\"height:30px; width:75px;\"></a>", Parameters,  Session["PageID"].ToString());
            }
            else if (admin)
            {
                litEdit.Text = String.Format("<a onmouseover=\"MM_swapImage('Image3','','/images/lemonaid/buttons/editadmin_over.png',0)\" onmouseout=\"MM_swapImgRestore()\" href='/controls/content/edit.aspx?id={0}&PageId={1}' rel='opener' title=\"Content Editor\" class=\"Tibby_Bottom_Button_Enabled_NotHighlighted\" style=\"color:#ffffff\" target=\"_blank\"><img id=\"Image3\" name=\"Image3\" alt=\"\" src=\"/images/lemonaid/buttons/editadmin.png\" border=\"0\" style=\"height:30px; width:75px;\"></a>", Parameters, Session["PageID"].ToString());
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
#if SPLIT_CONTENT

    private int InsertWidget(string content, PlaceHolder plContent)
    {
        string patern = "&lt;widget id='";
        string patern2 = "' class=";
        string patern3 = "/&gt;";
        string remaning = "";
        int npos = content.IndexOf(patern);
        int i = 0, limit = 10;
        while (npos >= 0)
        {
            string s = content.Substring(0, npos);

            AddHtmlContent(plContent, s);

            remaning = content.Substring(npos + patern.Length);
            int npos2 = remaning.IndexOf(patern2);

            if (npos2 >= 0)
            {
#region Insert Widget
                string id = remaning.Substring(0, npos2);
                //AddHtmlContent(plContent, "<br><strong>" + id.ToString() + "</strong>");

                DataTable dt = new DataTable();
                string sqlstr = @" select * from Content where id=@id";
                SqlCommand sq = new SqlCommand(sqlstr);
                sq.Parameters.AddWithValue("@id", id);

                dt = getTable(sq);
                if (dt.Rows.Count > 0)
                {
                    string control = dt.Rows[0]["control"].ToString();
                    UserControl userControl = LoadControlExtension.LoadControl(this, "~/Controls/" + control + "/" + control + ".ascx", dt.Rows[0]["param"].ToString());
                    plContent.Controls.Add(userControl);
                }

#endregion

                int npos3 = remaning.IndexOf(patern3);
                if (npos3 < 0)
                {
                    patern3 = "&lt;/widget&gt;";
                    npos3 = remaning.IndexOf(patern3);
                }
                remaning = remaning.Substring(npos3 + patern3.Length);
                npos = remaning.IndexOf(patern);
                if (npos < 0)
                {
                    i++;
                    AddHtmlContent(plContent, remaning);
                    break;
                }

            }
            else
            {
                i++;
                AddHtmlContent(plContent, remaning);
                break;
            }

            content = remaning;

            i++;
            if (i > limit)
                break;
        }

        if (i == 0)
        {
            AddHtmlContent(plContent, content);
        }

        return i;
    }

    protected string _connection = ConfigurationManager.AppSettings.Get("CMServer");
    private DataTable getTable(SqlCommand cmd)
    {
        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.SelectCommand.CommandType = CommandType.Text;
        da.SelectCommand.Connection = new SqlConnection(_connection);
        da.Fill(dt);
        return dt;
    }
    private void AddHtmlContent(PlaceHolder plContent, string text)
    {
        Literal litContent = new Literal();
        litContent.Text = text;
        plContent.Controls.Add(litContent);
    }
#endif

}
