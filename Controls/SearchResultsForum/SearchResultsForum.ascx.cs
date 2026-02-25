using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YAF.Core.Utilities;
using YAF.Core.Helpers;
using System.Text;
using System.Configuration;

public partial class SearchResultsForum : System.Web.UI.UserControl
{
    public string Parameters;
    public int _records = 0;
    public bool _partial = true;

    public SearchResultsForum() { }
    public SearchResultsForum(string p)
    {
        SplitParameters(p);
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
        if (Parameters != "")
            SplitParameters(Parameters);

        if (!Page.IsPostBack)
        {
            if (Request.QueryString["q"] != null)
                SearchTerm = Request.QueryString["q"].Replace("(", "").Replace(")", "").Replace("+", "").Trim();
        }

        this.RegisterJsBlockStartup(
                 "openModalJs",
                 JavaScriptBlocks.DoSearchJs());

        litTitle.Text = "<h2>" + "Forum" + "</h2>";

        StringBuilder script = new StringBuilder();
        script.Append(Environment.NewLine + String.Format("<script src=\"/{0}/Scripts/forum/yaf.SearchResults.js\" ></script>", ConfigurationManager.AppSettings["Forum.Page"].Replace("/", "")));
        script.Append(Environment.NewLine + String.Format("<script src=\"/{0}/Scripts/moment-with-locales.js\" ></script>", ConfigurationManager.AppSettings["Forum.Page"].Replace("/", "")));
        ((_Default)this.Page).InjectContent("Scripts", script.ToString(), (int)CMSHelper.counters.search_footer);

        if (!_partial)
        {
            litSubtitle.Text = String.Format("<p><strong>Your search for keyword(s) '{0}' produced:</strong></p>", SearchTerm);
        }
    }

    protected void SplitParameters(string p)
    {
        try
        {
            string[] s = p.Split(new char[] { ',' });
            _records = Convert.ToInt32(s[0]);

            if (s.Length > 1)
            {
                if (s[1] == "0")
                    _partial = false;
            }
        }
        catch
        {
        }
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

    public List<string> RegisteredElements;
    public bool PageElementExists(string name)
    {
        return this.RegisteredElements.Contains(name.ToLower());
    }
    #endregion
}