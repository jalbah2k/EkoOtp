using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

using YAF.Core.BaseControls;
using YAF.Core.Services;
using YAF.Core.Utilities;
using YAF.Types.Constants;
using YAF.Types.Interfaces;
using YAF.Core.Helpers;
using System.Collections.Generic;

public partial class Header : UserControl
{
    private string _lang;
    public string _language { set; get; }
    private string _seo { set; get; }

    public List<string> RegisteredElements;

    public Header()
    {
        _lang = "1";
        _language = "en";
        _seo = "";
    }


    #region Auxiliar Functions From YAF (Forum)
    public void RegisterJsBlockStartup(string name, string script)
    {
        this.RegisterJsBlockStartup(this, name, script);
    }
    public void RegisterJsBlockStartup(Control thisControl, string name, string script)
    {
       // if (!this.PageElementExists(name))
        {
            ScriptManager.RegisterStartupScript(
                thisControl,
                thisControl.GetType(),
                name,
                JsAndCssHelper.CompressJavaScript(script),
                true);
        }
    }

    public bool PageElementExists(string name)
    {
        return this.RegisteredElements.Contains(name.ToLower());
    }
    #endregion
    enum Groups { Common = 1, EKOMembers, EKO_PNCA = 82 }
    protected override void OnPreRender(EventArgs e)
    {
        if ( Session["LoggedInID"] != null)
        {

#if BaseUserControl
            this.PageContext.PageElements.RegisterJsBlockStartup(
                "LogOutJs",
                JavaScriptBlocks.LogOutJs(
                    this.GetText("TOOLBAR", "LOGOUT_TITLE"),
                    this.GetText("TOOLBAR", "LOGOUT_QUESTION"),
                    this.GetText("TOOLBAR", "LOGOUT"),
                    this.GetText("COMMON", "CANCEL"),
                    this.Get<LinkBuilder>().GetLink(ForumPages.Account_Logout)));
#else
            this.RegisterJsBlockStartup(
                "LogOutJs",
                JavaScriptBlocks.LogOutJs(
                    "Logout from the Forum.",
                    "Are you sure you want to logout?",
                    "Logout",
                    "Cancel",
                    "/Membership/Account/Logout"));


            DataTable dtm = new DataTable();

            SqlDataAdapter dap = new SqlDataAdapter("select User_id from Users_Groups_Access where Group_id = @groupid and User_id=@id", ConfigurationManager.AppSettings["CMServer"]);
            dap.SelectCommand.CommandType = CommandType.Text;
            dap.SelectCommand.Parameters.AddWithValue("@id", Session["LoggedInID"].ToString());
            dap.SelectCommand.Parameters.AddWithValue("@groupid", (int)Groups.EKO_PNCA);            //   Groups.EKOMembers);
            dap.Fill(dtm);
            if (dtm.Rows.Count > 0)
            {
                //litTopMenu.Text = @"<a href='/EKOMembers' class='toplinks'>My Dashboard</a>
                //                    <a href='/Membership/MyMessages' class='toplinks'>Inbox</a>
                //                    <div id='myAccountLink'><a href='/Membership/MyAccount' class='toplinks'>Account</a></div>"; 

                litTopMenu.Text = @"<a href='/EKOMembers' class='toplinks'>My Dashboard</a>
                                    <a href='/welcomebod' class='toplinks'>Board of Directors</a>
                                    <div id='myAccountLink'><a href='/mleadershipcouncil' class='toplinks'>Leadership Council</a></div>";

            }
#endif
        }

        base.OnPreRender(e);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Page.RouteData.Values["lang"] != null)
        {
            try
            {
                _language = this.Page.RouteData.Values["lang"].ToString().ToLower();
            }
            catch { }
        }

        _seo = this.Page.RouteData.Values["seo"].ToString().ToLower();

        SqlDataAdapter cmserver;
        DataTable dt = new DataTable();

        cmserver = new SqlDataAdapter("BASE_PageFromSeo_OneZone", ConfigurationManager.AppSettings["CMServer"]);
        cmserver.SelectCommand.CommandType = CommandType.StoredProcedure;
        cmserver.SelectCommand.Parameters.AddWithValue("@seo", _seo);
        cmserver.SelectCommand.Parameters.AddWithValue("@language", _language);
        cmserver.SelectCommand.Parameters.AddWithValue("@zone", "MainMenu");

        cmserver.Fill(dt);
        if (dt.Rows.Count > 0)
        {
            string param = dt.Rows[0]["param"].ToString();
            PlaceHolder placeHolder = (PlaceHolder)this.FindControl("MainMenu");

            UserControl userControl = LoadControlExtension.LoadControl(this, "~/Controls/Menu/Menu.ascx", param);
            placeHolder.Controls.Add(userControl);

            ((_Default)this.Page)._topmenuid = param;
        }

        if (_language == "fr")
            _language += "/";
        else
            _language = "";


    }
}