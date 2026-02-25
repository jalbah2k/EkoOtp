using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SearchResults : System.Web.UI.UserControl
{
    public string Parameters;

    int _records = 0;
    int _type = 1;
    public bool _partial = true;

    private SearchClass[] _searchClass = new SearchClass[4];
    private SearchClass _myClass;
    public SearchResults() 
    { 
        Parameters = ""; 
    }

    public SearchResults(string p) 
    {
        SplitParameters(p);
    }

    protected void SplitParameters(string p)
    {
        try
        {
            string[] s = p.Split(new char[] { ',' });
            _records = Convert.ToInt32(s[0]);

            if (s.Length > 1)
            {
                _type = Convert.ToInt32(s[1]);
                if (_type == 0)
                    _type++;
            }

            if (s.Length > 2)
            {
                if (s[2] == "0")
                    _partial = false;
            }
        }
        catch
        {
        }
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

        InitClasses();

        if (!Page.IsPostBack)
        {
            if (Request.QueryString["q"] != null)
                SearchTerm = Request.QueryString["q"];
        }

        if (!String.IsNullOrEmpty(SearchTerm))
            DoSearch(SearchTerm.Replace("&amp;", "&"));

    }

    private void InitClasses()
    {
        SearchClass c1 = new SearchClass();
        c1.Title = "Pages";
        c1.StoredProcedure = "SearchPagesFTS";
        c1.Type = "page";
        c1.AllResultsPage = "pageresults";
        c1.ConnectionString = "CMServer";
        c1.QueryString_SearchParm = "q";
        _searchClass[0] = c1;

        SearchClass c2 = new SearchClass();
        c2.Title = "News";
        c2.StoredProcedure = "SearchNewsFTS";
        c2.Type = "news";
        c2.AllResultsPage = "newsresults";
        c2.ConnectionString = "CMServer";
        c2.QueryString_SearchParm = "q";
        _searchClass[1] = c2;

        SearchClass c3 = new SearchClass();
        c3.Title = "Member Directory";
        c3.StoredProcedure = "SearchDirectoryFTS";
        c3.Type = "members";
        c3.AllResultsPage = "memberresults";
        c3.ConnectionString = "CMServer";
        c3.QueryString_SearchParm = "q";
        _searchClass[2] = c3;

        SearchClass c4 = new SearchClass();
        c4.Title = "Resources";
        c4.StoredProcedure = "Resources_Search_New";
        c4.Type = "resources";
        c4.AllResultsPage = "resources";
        c4.ConnectionString = "dbResources";
        c4.QueryString_SearchParm = "search_term";
        _searchClass[3] = c4;

        _myClass = _searchClass[_type - 1];
    }

    int deepin = 0;

    private void DoSearch(string keywords)
    {
        deepin++;
        int TotalRecords = Search_FTS(keywords);

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("CMServer")))
        {
            SqlCommand cmd = new SqlCommand("SearchTracking", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            // cmd.Parameters.AddWithValue("@keywords", tbSearch.Text.Replace("&amp;", "&"));
            cmd.Parameters.AddWithValue("@keywords", keywords);
            cmd.Parameters.AddWithValue("@ResultsCount", TotalRecords);

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }

    }

    FullTextSearch.FullTextSearch myFTS;
    string[] LCIDs = { "1033", "1036" };  //1033 - English USA; 3084 - French France;

    private int Search_FTS(string keywords)
    {
        int total = 0;
        litContent.Text = "";
        litTitle.Text = "<h2>" + _myClass.Title + "</h2>";

        string comm = _myClass.StoredProcedure;
        // --------- LCID ------------
        string searchTerm = RemoveNoiseWords(keywords, LCIDs[Convert.ToInt32(Session["Language"]) - 1]);
        myFTS = new FullTextSearch.FullTextSearch(searchTerm);


        //if (!searchTerm.Contains("hospital"))
        {

            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@keywords", myFTS.NormalForm));
            param.Add(new SqlParameter("@lang", Convert.ToInt32(Session["Language"])));
            param.Add(new SqlParameter("@LCID", LCIDs[Convert.ToInt32(Session["Language"]) - 1]));
            if ((comm == "SearchPagesFTS" || comm == "Resources_Search_New") && Session["LoggedInID"] != null)
                param.Add(new SqlParameter("@userid", Session["LoggedInID"].ToString()));

            DataTable dt = getTable(comm, param.ToArray());

            total = dt.Rows.Count;

            if (dt.Rows.Count == 0)
            {
                litMessage.Text = "<h3>No results</h3>";
                return total;
            }

            int total_res = dt.Rows.Count;
            if (_partial )
            {
                if (total_res > _records)
                {
                    total_res = _records;
                    litMessage.Text = String.Format("<h3>Displaying {0} of {1} {2} results – <a href='/{3}?{5}={4}'>View All</a></h3>",
                       total_res, dt.Rows.Count, _myClass.Type, _myClass.AllResultsPage, SearchTerm, _myClass.QueryString_SearchParm);
                }
                else
                {
                    litMessage.Text = String.Format("<h3>Displaying {0} {1} results</h3>", dt.Rows.Count, _myClass.Type);
                }
            }
            else
            {
                litSubtitle.Text = String.Format("<p><strong>Your search for keyword(s) '{0}' produced:</strong></p>", SearchTerm);
                litMessage.Text = String.Format("<h3>Displaying {0} {1} results</h3>", dt.Rows.Count, _myClass.Type);
            }

            for (int i = 0; i < total_res; i++ )
            {
                DataRow dr = dt.Rows[i];
                litContent.Text += String.Format("<div><a href='{0}'>{1}</a></div><br>", dr["seo"].ToString(), dr["title"].ToString());

            }

        }

        return total;

    }

    private DataTable getTable(string cmd, SqlParameter[] param)
    {
        SqlDataAdapter da = new SqlDataAdapter(cmd, new SqlConnection(ConfigurationManager.AppSettings.Get(_myClass.ConnectionString)));
        da.SelectCommand.CommandType = CommandType.StoredProcedure;
        da.SelectCommand.Parameters.AddRange(param);
        DataSet ds = new DataSet();
        da.Fill(ds);
        return ds.Tables[0];
    }

    private string RemoveNoiseWords(string searchTerm, string LCID)
    {
        //Do nothing if there is any quotations " or '
        string strReturn = "";

        if (searchTerm.IndexOf("\"") == -1 && searchTerm.IndexOf("'") == -1)
        {

            SqlParameter[] param;
            param = new SqlParameter[] {
                    new SqlParameter("@keywords", searchTerm),
                    new SqlParameter("@lang", LCID)
                };

            DataTable tbKeywords = getTable("RemoveNoiseWords", param);
            foreach (DataRow dr in tbKeywords.Rows)
            {
                strReturn += dr["item"] + " ";
            }
        }
        else
        {
            strReturn = searchTerm;
        }
        return strReturn.Trim();
    }
}

public class SearchClass
{
    public string Type;
    public string Title;
    public string StoredProcedure;
    public string AllResultsPage;
    public string ConnectionString;
    public string QueryString_SearchParm;
}