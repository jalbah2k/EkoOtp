//#define SPLIT_CONTENT
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

public partial class SubAccordion : System.Web.UI.UserControl
{
    public string Parameters;

    public SubAccordion() { }
    public SubAccordion(string s) { Parameters = s; }

    public string Language
    {
        get
        {
            return CMSHelper.GetLanguageNumber();
        }
    }

  
    protected void Page_Load(object sender, EventArgs e)
    {
      
    }

    public void BindRepeater()
    {
        DataTable dt = new DataTable();
        string sqlstr = @"SELECT         row_number() OVER (ORDER BY id) n, id, item_id, Title, [Content], Priority, TitleFR, ContentFR, TitleEN, ContentEN
                            FROM  Accordion.SubItems
                        where item_id = @id order by priority";
        SqlCommand sq = new SqlCommand(sqlstr);
        sq.Parameters.AddWithValue("@id", Parameters);

        dt = getTable(sq);
        if(dt.Rows.Count > 0)
        {

            //if (Language == "1" && dt.Rows[0]["h2"].ToString() != "")
            //    litTitle.Text = String.Format("<h2>{0}</h2>", dt.Rows[0]["h2"].ToString());
            //else if (Language == "2" && dt.Rows[0]["h2_fr"].ToString() != "")
            //    litTitle.Text = String.Format("<h2>{0}</h2>", dt.Rows[0]["h2_fr"].ToString());

            string s = System.Text.RegularExpressions.Regex.Replace(dt.Rows[0]["Title"].ToString(), @"[^A-Za-z0-9]+", "").ToLower();
            if (s.Length == 0)
                s = "sub-accordion-section-" + dt.Rows[0]["id"].ToString();

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
            string patern = "&lt;widget id='";
            string patern2 = "' class=";
            string patern3 = "&lt;/widget&gt;";
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

#else
            AddHtmlContent(plContent, dr["content"].ToString());

#endif

        }
    }

    private void AddHtmlContent(PlaceHolder plContent, string text)
    {
        Literal litContent = new Literal();
        litContent.Text = text;
        plContent.Controls.Add(litContent);
    }
}

