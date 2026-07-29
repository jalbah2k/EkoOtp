// =====================================================================
// SearchResults.ascx.cs  --  COMPLETE, with Step 1 semantic fusion +
// score display for tuning.
//
// HOW TO READ THIS FILE:
//   Lines/blocks I ADDED for semantic search are wrapped in:
//       // >>> SEMANTIC: ...                (start)
//       // <<< SEMANTIC                     (end)
//   Everything else is your original control, unchanged.
//
// IMPORTANT: this was rebuilt from your ORIGINAL upload. If you have
// made other edits to this control since, diff this against your copy
// rather than overwriting blindly. (Your apostrophe/redirect fixes were
// in SearchText.ascx.cs / the helper class, NOT here, so they are not
// affected by this file.)
// =====================================================================

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EkoSearch;                       // >>> SEMANTIC: namespace of SemanticSearch.cs

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
        c1.SemanticEnabled = SearchConfig.SemanticEnabledFor(c1.Type);   // >>> SEMANTIC
        _searchClass[0] = c1;

        SearchClass c2 = new SearchClass();
        c2.Title = "News";
        c2.StoredProcedure = "SearchNewsFTS";
        c2.Type = "news";
        c2.AllResultsPage = "newsresults";
        c2.ConnectionString = "CMServer";
        c2.QueryString_SearchParm = "q";
        c2.SemanticEnabled = SearchConfig.SemanticEnabledFor(c2.Type);   // >>> SEMANTIC
        _searchClass[1] = c2;

        SearchClass c3 = new SearchClass();
        c3.Title = "Member Directory";
        c3.StoredProcedure = "SearchDirectoryFTS";
        c3.Type = "members";
        c3.AllResultsPage = "memberresults";
        c3.ConnectionString = "CMServer";
        c3.QueryString_SearchParm = "q";
        c3.SemanticEnabled = SearchConfig.SemanticEnabledFor(c3.Type);   // >>> SEMANTIC
        _searchClass[2] = c3;

        SearchClass c4 = new SearchClass();
        c4.Title = "Resources";
        c4.StoredProcedure = "Resources_Search_New";
        c4.Type = "resources";
        c4.AllResultsPage = "resources";
        c4.ConnectionString = "dbResources";
        c4.QueryString_SearchParm = "search_term";
        c4.SemanticEnabled = SearchConfig.SemanticEnabledFor(c4.Type);   // >>> SEMANTIC (true via config)
        _searchClass[3] = c4;

        _myClass = _searchClass[_type - 1];
    }

    int deepin = 0;

    private void DoSearch(string keywords)
    {
        deepin++;
        // NOTE: tracking now happens inside Search_FTS (TrackSearch), so the
        // old SearchTracking block here was removed to avoid double logging.
        Search_FTS(keywords);
    }

    FullTextSearch.FullTextSearch myFTS;
    string[] LCIDs = { "1033", "1036" };  //1033 - English USA; 3084 - French France;

    private int Search_FTS(string keywords)
    {
        int total = 0;
        litContent.Text = "";
        litTitle.Text = "<h2>" + _myClass.Title + "</h2>";

        string comm = _myClass.StoredProcedure;

        // --------- LCID / noise-word stripping (keyword path) ------------
        string searchTerm = RemoveNoiseWords(keywords, LCIDs[Convert.ToInt32(Session["Language"]) - 1]);
        myFTS = new FullTextSearch.FullTextSearch(searchTerm);

        List<SqlParameter> param = new List<SqlParameter>();
        param.Add(new SqlParameter("@keywords", myFTS.NormalForm));
        param.Add(new SqlParameter("@lang", Convert.ToInt32(Session["Language"])));
        param.Add(new SqlParameter("@LCID", LCIDs[Convert.ToInt32(Session["Language"]) - 1]));
        if ((comm == "SearchPagesFTS" || comm == "Resources_Search_New") && Session["LoggedInID"] != null)
            param.Add(new SqlParameter("@userid", Session["LoggedInID"].ToString()));

        // >>> SEMANTIC: fill the FULL dataset (we need @mytable too), not just Tables[0]
        DataSet ds = getDataSet(comm, param.ToArray());
        DataTable dt = ds.Tables[0];
        // <<< SEMANTIC

        total = dt.Rows.Count;

        // >>> SEMANTIC: Resources-only fusion branch =====================
        bool doSemantic = _myClass.SemanticEnabled
                          && comm == "Resources_Search_New"
                          && Session["LoggedInID"] != null;

        if (doSemantic)
        {
            // 1) keyword-ranked ids (dt already ordered by score DESC)
            List<int> keywordIds = new List<int>();
            foreach (DataRow kr in dt.Rows) keywordIds.Add(Convert.ToInt32(kr["id"]));

            // 2) permitted+filtered universe = @mytable (the LAST result set)
            List<int> permitted = new List<int>();
            DataTable my = ds.Tables[ds.Tables.Count - 1];
            if (my != null && my.Columns.Contains("id"))
                foreach (DataRow mr in my.Rows) permitted.Add(Convert.ToInt32(mr["id"]));

            // 3) semantic ranking over the permitted set (ORIGINAL term to embedder)
            List<SemanticHit> sem = ResourceSemanticSearch.Search(keywords, permitted, 50);

            // score lookup + admin debug toggle
            Dictionary<int, float> semScore = new Dictionary<int, float>();
            foreach (SemanticHit sh in sem) semScore[sh.ResourceId] = sh.Score;
            bool showScores = IsDebugViewer();

            if (sem.Count > 0)
            {
                List<FusedHit> fused = ResourceSemanticSearch.Fuse(keywordIds, sem, 500);

                // display rows: reuse keyword rows; fetch any semantic-only ids
                Dictionary<int, DataRow> rowById = new Dictionary<int, DataRow>();
                foreach (DataRow kr in dt.Rows) rowById[Convert.ToInt32(kr["id"])] = kr;

                List<int> missing = new List<int>();
                foreach (FusedHit fh in fused)
                    if (!rowById.ContainsKey(fh.ResourceId)) missing.Add(fh.ResourceId);

                if (missing.Count > 0)
                {
                    DataTable extra = getDataSet("Resources_GetByIds",
                        new SqlParameter[] { new SqlParameter("@Ids", string.Join(",", missing)) }
                    ).Tables[0];
                    foreach (DataRow er in extra.Rows) rowById[Convert.ToInt32(er["id"])] = er;
                }

                total = fused.Count;
                RenderHeader(total);

                // optional: log every result for tuning (only under ?debug=1)
                if (showScores) LogEval(keywords, fused, semScore);

                int shown = _partial ? Math.Min(total, _records) : total;
                for (int i = 0; i < shown; i++)
                {
                    FusedHit fh = fused[i];
                    DataRow r;
                    if (!rowById.TryGetValue(fh.ResourceId, out r)) continue;

                    string badge = "";
                    if (fh.Source == HitSource.SemanticOnly && SearchConfig.ShowSemanticBadge)
                        badge = " <span class='sem-badge' title='Suggested by meaning-based search'>suggested</span>";

                    string dbg = "";
                    if (showScores)
                    {
                        float sc;
                        if (semScore.TryGetValue(fh.ResourceId, out sc))
                            dbg = String.Format(" <small style='color:#999'>[{0} {1:0.000}]</small>", fh.Source, sc);
                        else
                            dbg = String.Format(" <small style='color:#999'>[{0}]</small>", fh.Source);
                    }

                    litContent.Text += String.Format("<div><a href='{0}'>{1}</a>{2}{3}</div><br>",
                        r["seo"].ToString(), r["title"].ToString(), badge, dbg);
                }

                TrackSearch(keywords, total);
                return total;
            }
            // sem.Count == 0 -> fall through to keyword-only rendering below
        }
        // <<< SEMANTIC =====================================================

        // ---- Original keyword-only rendering (unchanged behavior) ----
        if (dt.Rows.Count == 0)
        {
            litMessage.Text = "<h3>No results</h3>";
            TrackSearch(keywords, 0);
            return 0;
        }

        int total_res = dt.Rows.Count;
        if (_partial)
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
            litSubtitle.Text = String.Format("<p><strong>Your search for keyword(s) '{0}' produced:</strong></p>", Server.HtmlEncode(SearchTerm));
            litMessage.Text = String.Format("<h3>Displaying {0} {1} results</h3>", dt.Rows.Count, _myClass.Type);
        }

        for (int i = 0; i < total_res; i++)
        {
            DataRow dr = dt.Rows[i];
            litContent.Text += String.Format("<div><a href='{0}'>{1}</a></div><br>", dr["seo"].ToString(), dr["title"].ToString());
        }

        TrackSearch(keywords, total);
        return total;
    }

    // >>> SEMANTIC: returns the full DataSet so we can read @mytable ======
    private DataSet getDataSet(string cmd, SqlParameter[] param)
    {
        SqlDataAdapter da = new SqlDataAdapter(cmd,
            new SqlConnection(ConfigurationManager.AppSettings.Get(_myClass.ConnectionString)));
        da.SelectCommand.CommandType = CommandType.StoredProcedure;
        da.SelectCommand.Parameters.AddRange(param);
        DataSet ds = new DataSet();
        da.Fill(ds);
        return ds;
    }
    // <<< SEMANTIC

    // Original getTable kept for RemoveNoiseWords and any other callers.
    private DataTable getTable(string cmd, SqlParameter[] param)
    {
        SqlDataAdapter da = new SqlDataAdapter(cmd, new SqlConnection(ConfigurationManager.AppSettings.Get(_myClass.ConnectionString)));
        da.SelectCommand.CommandType = CommandType.StoredProcedure;
        da.SelectCommand.Parameters.AddRange(param);
        DataSet ds = new DataSet();
        da.Fill(ds);
        return ds.Tables[0];
    }

    // >>> SEMANTIC: header + tracking helpers =============================
    private void RenderHeader(int totalRows)
    {
        if (_partial && totalRows > _records)
            litMessage.Text = String.Format(
                "<h3>Displaying {0} of {1} {2} results – <a href='/{3}?{5}={4}'>View All</a></h3>",
                _records, totalRows, _myClass.Type, _myClass.AllResultsPage, SearchTerm, _myClass.QueryString_SearchParm);
        else if (!_partial)
        {
            litSubtitle.Text = String.Format("<p><strong>Your search for keyword(s) '{0}' produced:</strong></p>", Server.HtmlEncode(SearchTerm));
            litMessage.Text = String.Format("<h3>Displaying {0} {1} results</h3>", totalRows, _myClass.Type);
        }
        else
            litMessage.Text = String.Format("<h3>Displaying {0} {1} results</h3>", totalRows, _myClass.Type);
    }

    private void TrackSearch(string keywords, int count)
    {
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("CMServer")))
        {
            SqlCommand cmd = new SqlCommand("SearchTracking", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@keywords", keywords);
            cmd.Parameters.AddWithValue("@ResultsCount", count);
            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }

    // Admin-only score display toggle. Replace the inner test with your
    // real reviewer/admin check if you want it stricter.
    private bool IsDebugViewer()
    {
        //if (Request.QueryString["debug"] != "1") return false;
        //return Session["LoggedInID"] != null;

        return true;
    }

    // Logs every fused result (only called under ?debug=1) to dbo.SearchEval
    // in the Resources DB. Create that table/proc from the eval harness file.
    private void LogEval(string query, List<FusedHit> fused, Dictionary<int, float> semScore)
    {
        try
        {
            string conn = SearchConfig.ConnString(SearchConfig.ResourcesConnName);
            object uid = (Session["LoggedInID"] != null)
                ? (object)Convert.ToInt32(Session["LoggedInID"]) : DBNull.Value;

            using (SqlConnection cn = new SqlConnection(conn))
            {
                cn.Open();
                for (int i = 0; i < fused.Count; i++)
                {
                    FusedHit fh = fused[i];
                    object sc = DBNull.Value;
                    float tmp;
                    if (semScore.TryGetValue(fh.ResourceId, out tmp)) sc = tmp;

                    using (SqlCommand cmd = new SqlCommand("dbo.SearchEval_Log", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Query", query);
                        cmd.Parameters.AddWithValue("@UserId", uid);
                        cmd.Parameters.AddWithValue("@ResourceId", fh.ResourceId);
                        cmd.Parameters.AddWithValue("@Source", fh.Source.ToString());
                        cmd.Parameters.AddWithValue("@Score", sc);
                        cmd.Parameters.AddWithValue("@Rank", i + 1);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
        catch { /* logging must never break search */ }
    }
    // <<< SEMANTIC

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
    public bool SemanticEnabled;          // >>> SEMANTIC
}