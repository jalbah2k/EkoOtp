#define SPLIT_CONTENT
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;

public partial class Accordion : System.Web.UI.UserControl
{
    public string Parameters;
    public Accordion() { }
    public Accordion(string s) { Parameters = s; }

    public string Language
    {
        get
        {
            return CMSHelper.GetLanguageNumber();
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            BindRepeater();
        }

        ((_Default)this.Page).InjectContent("head", "<link rel=\"stylesheet\" href=\"/controls/Accordion/accordion.css\" /><link rel=\"stylesheet\" href=\"/controls/Accordion/sub_accordion.css\" />" + Environment.NewLine, (int)CMSHelper.counters.accordion_header);

        StringBuilder script = new StringBuilder();
        script.Append(Environment.NewLine + "<script src=\"/controls/Accordion/accordion.js?v=1\"></script>");
        script.Append(Environment.NewLine + "<script src=\"/controls/Accordion/sub_accordion.js?v=1\"></script>");
        script.Append(Environment.NewLine + "<script src=\"/controls/Accordion/onload.js\"></script>" + Environment.NewLine);
        ((_Default)this.Page).InjectContent("Scripts", script.ToString(), (int)CMSHelper.counters.accordion_footer);


    }

    protected void BindRepeater()
    {
        DataTable dt = new DataTable();
        string sqlstr = @" select row_number() OVER (ORDER BY i.id) n, i.*, sg.name as subgroupname, g.name as groupname, g.title as grpname, g.title_fr  as grpname_fr,
                    sg.Title as h2, sg.TitleFR as h2_fr
                    from accordion.items i inner join accordion.subgroups sg on i.subgroup_id=sg.id inner join accordion.groups g on g.id=sg.group_id 
                    where subgroup_id = @id order by priority";
        SqlCommand sq = new SqlCommand(sqlstr);
        sq.Parameters.AddWithValue("@id", Parameters);

        dt = getTable(sq);
        if(dt.Rows.Count > 0)
        {
            //// litTitle.Text = String.Format("<h2>{0}</h2>", (Language == "1" ? dt.Rows[0]["grpname"].ToString() : dt.Rows[0]["grpname_fr"].ToString()));
            //litTitle.Text = String.Format("<h2>{0}</h2>", (Language == "1" ? dt.Rows[0]["h2"].ToString() : dt.Rows[0]["h2_fr"].ToString()));

            if (Language == "1" && dt.Rows[0]["h2"].ToString() != "")
                litTitle.Text = String.Format("<h2>{0}</h2>", dt.Rows[0]["h2"].ToString());
            else if (Language == "2" && dt.Rows[0]["h2_fr"].ToString() != "")
                litTitle.Text = String.Format("<h2>{0}</h2>", dt.Rows[0]["h2_fr"].ToString());

            string s = System.Text.RegularExpressions.Regex.Replace(dt.Rows[0]["h2"].ToString(), @"[^A-Za-z0-9]+", "").ToLower();
            if (s.Length == 0)
                s = "accordion-section-" + dt.Rows[0]["id"].ToString();

            string anchor = String.Format("<a name='{0}' id='{1}'></a>", s, s);
            litAnchor.Text = anchor;


            rep1.DataSource = dt;
            rep1.DataBind();
        }
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

    protected void rep1_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if ((DataRowView)e.Item.DataItem != null)
        {
            DataRowView dr = (DataRowView)e.Item.DataItem;

            //HtmlAnchor lnk_title = (HtmlAnchor)e.Item.FindControl("lnk_title");
            //lnk_title.HRef = CMSHelper.GetLanguagePrefix() + CMSHelper.GetSeo() + "#" + "accordion-" + dr["n"].ToString();   // Convert.ToString(e.Item.ItemIndex + 1);

            PlaceHolder plContent = (PlaceHolder)e.Item.FindControl("plContent");

#if SPLIT_CONTENT
            int i = InsertWidget(dr, plContent);

#else
            AddHtmlContent(plContent, dr["content"].ToString());

#endif
            LoadSubItems(dr, plContent);

            if (i == 0 && dr["footer"].ToString() != "")
            {
                AddHtmlContent(plContent, dr["footer"].ToString());
            }
        }
    }

#if SPLIT_CONTENT
    private int InsertWidget(DataRowView dr, PlaceHolder plContent)
    {
        string patern = "&lt;widget id='";
        string patern2 = "' class=";
        string patern3 = "/&gt;";
        string content = dr["content"].ToString();
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
            AddHtmlContent(plContent, dr["content"].ToString());
        }

        return i;
    }
#endif

    private void LoadSubItems(DataRowView dr, PlaceHolder plContent)
    {
        DataTable dtSub = new DataTable();
        string sqlstr_sub = @" select id from Accordion.SubItems where item_id=@id";
        SqlCommand sq_sub = new SqlCommand(sqlstr_sub);
        sq_sub.Parameters.AddWithValue("@id", dr["id"].ToString());

        dtSub = getTable(sq_sub);
        if( dtSub.Rows.Count > 0)
        {
            UserControl userControl = LoadControlExtension.LoadControl(this, "~/Controls/Accordion/SubAccordion.ascx", dr["id"].ToString());
            plContent.Controls.Add(userControl);
            ((SubAccordion)userControl).BindRepeater();
        }
    }

    private void AddHtmlContent(PlaceHolder plContent, string text)
    {
        Literal litContent = new Literal();
        litContent.Text = text;
        plContent.Controls.Add(litContent);
    }
}

