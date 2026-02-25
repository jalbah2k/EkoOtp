using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Text.RegularExpressions;

public partial class Menu : System.Web.UI.UserControl
{
    public string Parameters;

    private bool IsMobile
    {
        get 
        {
            bool ret = false;
            try { ret = Request.UserAgent.ToLower().Contains("mobile"); }
            catch { }
            return ret;

        }
    }
    public bool isUtility
    {
        get; set;
    }

    public string CssClass
    {
        set { ViewState["CssClass"] = value; }

        get
        {
            if(ViewState["CssClass"] != null)
                return ViewState["CssClass"].ToString();
            else
                return "";
        }
    }
    public Menu()
    {
        Parameters = "1";
        ItemIds = new List<string>();
    }

    public Menu(string s)
    {
        Parameters = s;
        ItemIds = new List<string>();
    }

    private bool isMultiLanguage;
    private string Language;
    private string Seo;

    List<string> ItemIds;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string script = string.Empty;

            SetSeo();
            litMenuItems.Text = BuildMenu(Parameters, CMSHelper.GetLanguageNumber(), "1");
           
        }
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        try{((_Default)this.Page).InjectContent("Scripts", "<script>var mymenuid = '#Menu_" + Parameters + "'</script>" + Environment.NewLine + "<script src = \"/Controls/Menu/smartmenu_activate.js\" ></script >" + Environment.NewLine);}
        catch { }
    }


    private string BuildMenu(string Param, string lang, string userid, int indent = 0, bool linkedmenuid = false)
    {
        StringBuilder sb = new StringBuilder("");

        DataTable dt = new DataTable();
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]))
        {
            SqlDataAdapter dapt = new SqlDataAdapter(
                String.Format("select distinct * from [menu].[GetMenuItems{0}] (@id, @parentid, @lang, @seo) order by sort", IsMobile ? "_Mobile" : ""),
                conn);
            dapt.SelectCommand.Parameters.AddWithValue("@id", Param);
            dapt.SelectCommand.Parameters.AddWithValue("@parentid", 0);
            dapt.SelectCommand.Parameters.AddWithValue("@lang", lang);
            dapt.SelectCommand.Parameters.AddWithValue("@seo", Seo);

            if (Session["LoggedInID"] != null)
            {
                dapt.SelectCommand.CommandText = String.Format("select distinct * from [menu].[GetMenuItemsForUser{0}] (@id, @parentid, @lang, @seo, @userid) order by sort", IsMobile ? "_Mobile" : "");
                dapt.SelectCommand.Parameters.AddWithValue("@userid", Session["LoggedInID"].ToString());
            }

            dapt.Fill(dt);
            
        }
        if (dt.Rows.Count > 0)
        {
            //int nrows = dt.Rows.Count;
            //for (int i = 0; i < nrows; i++)
            //{
            //    DataRow dr = dt.Rows[i];

            //    if (dr["Orientation"].ToString() == "Horizontal"
            //        && Request.UserAgent.ToLower().Contains("mobile")
            //        && Convert.ToInt32(dr["fakeid"]) > 0
            //        )                    
            //    {
            //        string linkedmenuid = dr["linkedmenuid"].ToString();
            //        dr["linkedmenuid"] = DBNull.Value;

            //        DataRow clonedRow = dt.NewRow();
            //        clonedRow.ItemArray = dr.ItemArray.Clone() as object[];

            //        clonedRow["text"] = dr["faketext"].ToString();
            //        clonedRow["Sort"] = dr["Sort"].ToString() + "|A";
            //        clonedRow["Steps"] = Convert.ToInt32(dr["Steps"]) + 1;
            //        clonedRow["linkedmenuid"] = linkedmenuid;
            //        clonedRow["fakeid"] = 0;
            //        dt.Rows.InsertAt(clonedRow, i + 1);
            //        i++;
            //    }
            //}

            if (indent == 0)
                CssClass = dt.Rows[0]["CssClass"].ToString() + ((Parameters != "5" && dt.Rows[0]["Orientation"].ToString() == "Horizontal") ? " hide-third-level" : "");

            int step = 0;
            bool ulopen = false;
            foreach (DataRow dr in dt.Rows)
            {

                string parentid = dr["parentid"].ToString();
                if (Convert.ToInt32(parentid) > 0)
                {
                    if (!ItemIds.Exists(x => x.Contains(parentid)))
                        continue;
                }
                ItemIds.Add(dr["id"].ToString()); 

                int Steps = Convert.ToInt32(dr["Steps"]) + indent;
                if (step < Steps )
                {
                    //if (!Item.IsHtml)
                        sb.Append("<ul><li>");
                    //else
                        //sb.Append("<ul class=\"mega-menu\"><li>");

                    ulopen = true;
                    BuildItem(sb, dr, indent, linkedmenuid);

                }
                else if (step == Steps)
                {
                    if (sb.Length > 0)
                    {
                        sb.Append("</li>");
                    }

                    sb.Append("<li>");
                    if (!BuildItem(sb, dr, indent, linkedmenuid))
                    {
                        int n = sb.ToString().LastIndexOf("<li>");
                        sb.Remove(n, "<li>".Length);
                        sb.Append("<li class='hide'>");
                    }
                    if (dr["linkedmenuid"].ToString() != "")
                    {
                        string s = BuildMenu(dr["linkedmenuid"].ToString(), CMSHelper.GetLanguageNumber(), "1", 1, true);
                        if(s != "")
                        {
                            int n1 = Regex.Matches(s, "<ul>").Count;
                            int n2 = Regex.Matches(s, "</ul>").Count;

                            if(n1 > n2)
                                s += "</ul>";
                            sb.Append(s);
                        }
                    }

                }
                else
                {
                    for (int i = 0; i < step - Steps; i++)
                    {
                        sb.Append("</li></ul></li>");
                    }
                    sb.Append("<li>");
                    if(!BuildItem(sb, dr, indent, false))
                    {
                        int n = sb.ToString().LastIndexOf("<li>");
                        sb.Remove(n, "<li>".Length);
                        sb.Append("<li class='hide'>");
                    }
                    ulopen = false;
                }

                step = Steps;

            }
            if (ulopen)
            {
                sb.Append("</li></ul>");
            }

            sb.Append("</li>");
        }

        string html = sb.ToString();

        string pattern = @"(<li class='hide'><ul>)([A-Za-z0-9\<\>\s\'\=\/]{0,1000})(</ul></li>)";
        html = FindAndReplace(html, pattern);

        return html;
    }
    private bool BuildItem(StringBuilder sbglobal, DataRow Item, int indent, bool linkedmenuid)
    {
        bool bret = false;
        StringBuilder sb = new StringBuilder();
        //if (!Item.IsHtml)
        {
            sb.Append("<a href=\"");

            if (Item["NavigateUrl"].ToString() != "" && !IsPrivate(Item))
            {
                if (Item["Orientation"].ToString() == "Horizontal" && indent == 0
                    && IsMobile
                    && Convert.ToInt32(Item["fakeid"]) > 0
                    )
                {
                    sb.Append("javascript:void(" + Item["id"].ToString() + ")");
                }
                else
                {
                    string url = Item["NavigateUrl"].ToString();

                    if(linkedmenuid)
                    {
                        if (url.Contains("?"))
                            url += "&t=1";
                        else
                            url += "?t=1";

                    }

                    if (
                        (CMSHelper.isMultilingual || CMSHelper.AlwaysWithEN)
                        && !url.Contains("http:")
                        && !url.Contains("https:")
                        && !url.Contains("ftp:")
                        && !url.Contains("ftps:")
                        && !url.Contains("mailto:")
                        )
                    {
                        //sb.Append(CMSHelper.GetLanguagePrefix() + url.Replace("/", ""));
                        sb.Append(Request.Url.Scheme + "://" + Request.Url.Authority + url);

                    }
                    else
                        sb.Append(url);
                }
            }
            else
                sb.Append("javascript:void(" + Item["id"].ToString() + ")");

            sb.Append("\"");

            if (!String.IsNullOrEmpty(Item["Target"].ToString()))
            {
                sb.Append(" target=\"");
                sb.Append(Item["Target"].ToString());
                sb.Append("\"");
            }

            //if (!String.IsNullOrEmpty(Item["Tooltip"].ToString()))
            //{
            //    sb.Append(" title=\"");
            //    sb.Append(Item["Tooltip"].ToString());
            //    sb.Append("\"");
            //}

            if ((
                //Item.MultiHighlighted && 
                Convert.ToInt32(Item["Highlighted"]) > 0) || Item["NavigateUrl"].ToString() == Seo)
            {
                sb.Append(" class=\"current");

                if (IsPrivate(Item))
                {
                    sb.Append(" class=\" lock");
                }
                sb.Append("\"");
            }
            else if (IsPrivate(Item))
            {
                sb.Append(" class=\"lock\"");
            }


            sb.Append(">");
            sb.Append(Item["Text"].ToString());
            sb.Append("</a>");

        }
        //else
        //{
        //    sb.Append(Item.Content);
        //}


        if (!IsPrivate(Item) || Convert.ToBoolean(Item["ShowLock"]))
        {
            sbglobal.Append(sb.ToString());
            bret = true;
        }

        return bret;
    }

    private bool IsPrivate(DataRow Item)
    {
        return String.IsNullOrEmpty(Item["User_id"].ToString()) && Convert.ToBoolean(Item["private"]);
    }

    private void SetSeo()
    {
        //if (RouteData.Values["url"] != null)
        //{
        //    string url = RouteData.Values["url"].ToString();
        //    string[] parms = url.Split(new char[] { '/' });
        //    Seo = parms[0];
        //}

        try { Seo = CMSHelper.GetSeo(); }
        catch { Seo = "home"; }
    }

    
    private string FindAndReplace(string html, string pattern, string replace = "")
    {
        MatchCollection mc = Regex.Matches(html, pattern);

        foreach (Match m in mc)
        {
            //if (m.Success)
            {
                string value = m.Value;
                html = html.Replace(value, replace);
            }
        }

        return html;
    }
}