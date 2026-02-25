//#define ACTIVE_DIRECTORY
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;


public partial class Controls_Search_Search : System.Web.UI.UserControl
{
    protected string searchTerm = "";
    protected DataTable tbPages;
    protected DataTable dtNews;
    protected DataTable dtDirectory;
    protected int nMembers;
    
    protected bool searched = false;
    protected bool advanced = false;

    private string con = ConfigurationManager.AppSettings.Get("CMServer");
    private string cmdSearch = "SearchPagesFTS";      //"SearchPages";
    private string cmdSearchNews = "SearchNewsFTS";
    private string cmdSearchDirectorys = "SearchDirectoryFTS";
 //   private string cmdGroup = "select id,case when len(isnull(description,''))>0 then description else name end description from groups where len(isnull(name,''))>0";

    public int TotalRecords
    {
        get
        {
            int Total = 0;
            if (tbPages != null && tbPages.Rows.Count > 0)
                Total += tbPages.Rows.Count;
            if (dtNews != null && dtNews.Rows.Count > 0)
                Total += dtNews.Rows.Count;

            Total += nMembers;
            
            return Total;
        }
    }

    public Controls_Search_Search()
    {

    }

    public Controls_Search_Search(string s)
    {
        advanced = Convert.ToInt32(s) > 0;
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        ViewState["SearchTerm"] = null;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["MemberId"] != null)
        {
            StringBuilder script = new StringBuilder();
            script.Append(Environment.NewLine + String.Format("<script src=\"/{0}/Scripts/forum/yaf.SearchResults.js\" ></script>", ConfigurationManager.AppSettings["Forum.Page"].Replace("/", "")));
            script.Append(Environment.NewLine + String.Format("<script src=\"/{0}/Scripts/moment-with-locales.js\" ></script>", ConfigurationManager.AppSettings["Forum.Page"].Replace("/", "")));
            ((_Default)this.Page).InjectContent("Scripts", script.ToString(), (int)CMSHelper.counters.search_footer);
        }

        litSuggestion.Text = "";
        litSuggestion2.Text = "";


        if (!Page.IsPostBack)
        {
            //btSearch.Text = Language == "en" ? "Search" : "Recherche";

            if (Request.QueryString["q"] != null)
                ViewState["SearchTerm"] = Sanitize(Request.QueryString["q"]);

            if (Request.QueryString["prm"] != null)
            {
                //Response.Write(Request.QueryString["prm"]);
//                ViewState["SearchTerm"] = EncDec.DESDecrypt(Request.QueryString["prm"]);
                ViewState["SearchTerm"] = Sanitize(Request.QueryString["prm"]);
                //Response.Write(ViewState["SearchTerm"].ToString());
            }

            
            if (ViewState["SearchTerm"] == null)
            {
                ViewState["first"] = true;
            }
            else
            {
                ViewState["first"] = false;

                safeC = tbSearch.Text = ViewState["SearchTerm"].ToString();
                ViewState.Remove("SearchTerm");
                SearchPages();
            }

            //if (Language != "en")
            //{
            //    wordsA.Text = "Votre recherche pour le(s) mot(s) clé(s) '";
            //    wordsB.Text = "' a produit ";
            //    wordsC.Text = " résultat(s)";
            //    wordsD.Text = "Désolé, votre recherche pour '";
            //    wordsE.Text = "' ne retourne aucun résultat.";
            //}
            //if (advanced)
            //{
            //    /////test = getTable(cmdGroup, false).Rows.Count.ToString();
            //    ddlGroups.DataSource = getTable(cmdGroup, false);
            //    ddlGroups.DataTextField = "description";
            //    ddlGroups.DataValueField = "id";
            //    ddlGroups.DataBind();

            //    ddlGroups.Items.Insert(0, Language == "en" ? "Select Group" : "Choisir le Département");
            //}
        }
    }

    private string Sanitize(string input)
    {
        return QueryStringHelper.AntiXssEncoder_HtmlEncode(input, true);
    }

    public void Search(object o, EventArgs e)
    {
        ViewState["first"] = false;
        safeC = tbSearch.Text = Sanitize(tbSearch.Text);
        SearchPages();

    }


    private DataTable getTable(string cmd, SqlParameter[] param)
    {
        SqlDataAdapter da = new SqlDataAdapter(cmd, con);
        da.SelectCommand.CommandType = CommandType.StoredProcedure;
        da.SelectCommand.Parameters.AddRange(param);
        DataTable dt = new DataTable();
        da.Fill(dt);
        return dt;
    }

    private DataTable getTable(string cmd)
    {
        return getTable(cmd, true);
    }

    private DataTable getTable(string cmd, bool SP)
    {
        SqlDataAdapter da = new SqlDataAdapter(cmd, con);
        if (SP)
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
        DataTable dt = new DataTable();
        da.Fill(dt);
        return dt;
    }

    

    //protected void dbSearch_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    //{       

    //    (source as DataGrid).CurrentPageIndex = e.NewPageIndex;
        
    //    SearchPages();

    //}

    protected string Score(string val)
    {

        return "";

    }

    protected string Language
    {
        get
        {
            string[] lang = { "fr", "en", "fr" };
            return lang[Convert.ToInt32(Session["Language"])];
        }
    }
    public string LanguagePrefix
    {
        get
        {
            return CMSHelper.GetLanguagePrefix();
            //return "";
        }
    }

    protected bool First
    {
        set { ViewState["first"] = value; }
        get { return Convert.ToBoolean(ViewState["first"]); }
    }

    FullTextSearch.FullTextSearch myFTS;
    private void Search_FTS(string keywords, string comm, ref DataTable dt, DataGrid dg)
    {
        ViewState["SearchTerm"] = keywords;
        searchTerm = keywords.Replace("&amp;", "&").Replace("-", "");

        if (searchTerm.Length > 0)
        {
            // --------- LCID ------------
           
            searchTerm = RemoveNoiseWords(searchTerm, LCIDs[Convert.ToInt32(Session["Language"]) - 1]);

            myFTS = new FullTextSearch.FullTextSearch(searchTerm);

           
            //if (searchTerm.Length > 0)
            if(!searchTerm.Contains("hospital"))
            {

                //SqlParameter[] param;

                //param = new SqlParameter[] { 
                //            new SqlParameter("@keywords", myFTS.NormalForm),
                //            new SqlParameter("@lang", Convert.ToInt32(Session["Language"])),
                //            new SqlParameter("@LCID", LCIDs[Convert.ToInt32(Session["Language"]) - 1]),
                //};


                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@keywords", myFTS.NormalForm));
                param.Add(new SqlParameter("@lang", Convert.ToInt32(Session["Language"])));
                param.Add(new SqlParameter("@LCID", LCIDs[Convert.ToInt32(Session["Language"]) - 1]));
                if (comm == "SearchPagesFTS" && Session["LoggedInID"] != null)
                    param.Add(new SqlParameter("@userid", Session["LoggedInID"].ToString()));


              //////  if (Convert.ToInt32(Session["Language"]) == 2)
              ////  {
              ////      string[] words = keywords.Split(new char[] { ' ' });
              ////      string kwords = "";

              ////      foreach (string s in words)
              ////      {
              ////          if (s.Length > 0)
              ////              kwords = myFTS.NormalForm.Replace(s, Server.HtmlEncode(s));                        

              ////      }

              ////      if (kwords != myFTS.NormalForm)
              ////      {
              ////          param.Add(new SqlParameter("@keywordsEnc", kwords));
              ////          //Response.Write(kwords + "<br />");
              ////      }

              ////  }


//                Response.Write(myFTS.NormalForm + "<br />");
//                Response.Write(comm + "<br />");

                dt = getTable(comm, param.ToArray());
                if (dt.Rows.Count > 0)
                {
                    dg.DataSource = dt;
                    dg.DataBind();
                }
                //Response.Write(myFTS.NormalForm.Replace("'", "''") + "<br />");

            }
        }

    }

    private void SearchNews_FTS(string keywords)
    {
        bool res = false;

        Search_FTS(keywords, cmdSearchNews, ref dtNews, dbSearchNews);
        res = dtNews != null && dtNews.Rows.Count > 0;
        ntitle.Visible = ngrid.Visible = res;

        litNewsResults.Text = String.Format("<div class='alert alert-warning text-center mt-3' role='alert'>{0} news found.</div>", dtNews.Rows.Count.ToString());

        searched |= res;

    }
    private void SearchPages_FTS(string keywords)
    {
        bool res = false;

      //  Search_FTS(tbSearch.Text, cmdSearch, ref tbPages, dbSearch);
        Search_FTS(keywords, cmdSearch, ref tbPages, dbSearch);
        res = tbPages != null && tbPages.Rows.Count > 0;

        if ( Request.QueryString["seo"] == "error")
        {
            string red = ViewState["SearchTerm"].ToString();
            ViewState["SearchTerm"] = null;
            //Response.Redirect((Session["Language"].ToString() == "1" ? "/en/search?prm=" : "/fr/search?prm=") + Server.UrlEncode(red));
            Response.Redirect((Session["Language"].ToString() == "1" ? LanguagePrefix + "search?prm=" : "/fr/search?prm=") + Server.UrlEncode(red));

        }
        litPageResults.Text = String.Format("<div class='alert alert-warning text-center mt-3' role='alert'>{0} page(s) found.</div>", tbPages.Rows.Count.ToString());

        ptitle.Visible = pgrid.Visible = res;
        searched |= res;

        //Response.Write(searched.ToString() + "<br />");

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

    private string safeC = "";
    private void SearchPages()
    {
        safeC = tbSearch.Text;
        tbSearch.Text = safeC;

        SearchDirectory(safeC);
        DoSearch(safeC.Replace("&amp;", "&"));

    }

    int deepin = 0;
    private int DoSearch(string keywords)
    {
        deepin++;

        //SearchNews();
        SearchNews_FTS(keywords);
        SearchPages_FTS(keywords);
        //SearchDirectory_FTS(keywords);
        //searched = ctitle.Visible | ntitle.Visible | pgrid.Visible;

        using (SqlConnection conn = new SqlConnection(con))
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

        if (!searched && deepin < 2 && searchTerm.Length > 0 && keywords != sDummyText)
        {
            RedoSearch(searchTerm);
            deepin--;
        }

        return TotalRecords;
    }

    enum Languages { English = 1, French };

    private void SearchDirectory(string keywords)
    {
#if ACTIVE_DIRECTORY
        nMembers = StaffDirectory1.SearchExternal(keywords);
#else
        nMembers = 0;
#endif
        bool res = nMembers > 0;
        dtitle.Visible = res;
        dgrid.Visible = res;
        searched |= res;
    }

    

#region MyRegion
    private string sqlGetAllWords = "GetAllWords";

    MyKeywords[] mywords;
    string[] LCIDs = { "1033", "1036" };  //1033 - English USA; 3084 - French France;

    string sDummyText = "";

    private void RedoSearch(string searchTerm)
    {
        string text = "";
        string Terms = "";
        string stemp = "";
        string suggestion = "";
        int nres = 0;
        bool bMisspelled = false;

        int mLanguage = Convert.ToInt32(Session["Language"]);

        mywords = new MyKeywords[myFTS.SearchTerms.Length];


        SqlParameter[] param = new SqlParameter[] { new SqlParameter("@lang", Convert.ToInt32(Session["Language"])) };
        DataTable dt = getTable(sqlGetAllWords, param);

        foreach (DataRow dr in dt.Rows)
        {
            text += dr["Value"].ToString() + Environment.NewLine;
        }
       // Response.Write(text);

        //SpellingCorrector.Faithful.SetDictionary(FTSAux.RemoveAllNoiseWords(text, LCIDs[_mLanguage - 1]));

        if (mLanguage == 1)
            SpellingCorrector.Faithful.SetDictionary(text);
        else
            SpellingCorrector.FaithfulFr.SetDictionary(text);


        for (int i = 0; i < myFTS.SearchTerms.Length; i++)
        {

            mywords[i] = new MyKeywords();
            if (mLanguage == 1)
                mywords[i].terms = SpellingCorrector.Faithful.GetCorrectedWords(myFTS.SearchTerms[i].ToLower());
            else
                mywords[i].terms = SpellingCorrector.FaithfulFr.GetCorrectedWords(myFTS.SearchTerms[i].ToLower());

            Terms += " " + mywords[i].terms[0];

       //     Response.Write(mywords[i].terms[0] + "<br />");

            if (!bMisspelled)
            {
                foreach (string s in mywords[i].terms)
                {
                    if (bMisspelled = s != myFTS.SearchTerms[i])
                        break;
                }
            }
        }

        if (myFTS.SearchTerms.Length == 1)      //if only one word during search then:
        {
            if (bMisspelled)
            {
                safeC = Terms = Sanitize(Terms);
                //Redo search
                nres = DoSearch(Terms);
              //  Response.Write(nres);

                if (nres > 0)
                {
                    if (mLanguage == 1)
                    {
                        litSuggestion.Text = "<br /><div><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\"><tr><td style=\"vertical-align:top; padding:10px;\"><img src=\"/images/attention.png\"></td><td style=\"vertical-align:top; padding:15px 10px 0px 10px;\">Your search for <strong>" + safeC + "</strong> produced no results. Displaying result(s) for <strong>"
                            //+ "<a href='/" + Language + "/search?prm=" + EncDec.DESEncrypt(Terms)
                            + "<a href='" + LanguagePrefix + "search?prm=" + Server.UrlEncode(Terms)
                            + "' style='font-style:italic'>" + Terms + "</a></strong> below.</td></tr></table> ";
                    }
                    else
                    {
                        litSuggestion.Text = "<br /><div><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\"><tr><td style=\"vertical-align:top; padding:10px;\"><img src=\"/images/attention.png\"></td><td style=\"vertical-align:top; padding:15px 10px 0px 10px;\">Votre recherche pour <strong>" + safeC + "</strong> n’a produit aucun résultat. Les résultats de recherche pour <strong>"
                            //+ "<a href='/" + Language + "/search?prm=" + EncDec.DESEncrypt(Terms)
                            + "<a href='" + LanguagePrefix + "search?prm=" + Server.UrlEncode(Terms)
                            + "' style='font-style:italic'>" + Terms + "</a></strong> sont affichés ci-dessous.</td></tr></table> ";
                    }

                    tbSearch.Text = safeC;  // Terms;

                    if (mywords[0].terms.Length > 1)
                    {
                        for (int i = 1; i < mywords[0].terms.Length; i++)
                        {
                            if (RemoveNoiseWords(mywords[0].terms[i], LCIDs[mLanguage - 1]).Length > 0)   //FTSAux.RemoveAllNoiseWords(mywords[0].terms[i], LCIDs[mLanguage - 1]).Length > 0)
                            {
                                if (stemp != "")
                                    stemp += ", ";

                                //stemp += "<a href='/" + Language + "/search?prm=" + EncDec.DESEncrypt(mywords[0].terms[i]) + "' style='font-style:italic'>" + mywords[0].terms[i] + "</a>";
                                stemp += "<a href='" + LanguagePrefix + "search?prm=" + Server.UrlEncode(mywords[0].terms[i]) + "' style='font-style:italic'>" + mywords[0].terms[i] + "</a>";
                            }
                        }

                        if (stemp != "")
                        {
                            if (mLanguage == 1)
                            {
                                suggestion = "Or did you mean... ";
                                litSuggestion.Text = litSuggestion.Text.Replace("below.</td></tr></table>", "below.<br />" + suggestion + stemp + "?<br />" + "</td></tr></table>");
                            }
                            else
                            {
                                suggestion = "Voulez-vous rechercher... ";
                                litSuggestion.Text = litSuggestion.Text.Replace("sont affichés ci-dessous.</td></tr></table>", "sont affichés ci-dessous.<br />" + suggestion + stemp + "?<br />" + "</td></tr></table>");
                            }
                        }
                    }
                }
                else
                {

                    if (mLanguage == 1)
                        litSuggestion2.Text = "Your search for <strong>" + safeC + "</strong> was corrected to <strong>" + Terms + "</strong> but produced no results containing this search term.<br /> ";
                    else
                        litSuggestion2.Text = "Votre recherche de <strong>" + safeC + "</strong> a été corrigée à <strong>" + Terms + "</strong> mais n'a abouti à aucun résultat avec tous les termes de recherche.<br /> ";
                }
            }
            else
            {
                tbNoResults.Visible = true;
            }
        }
        else
        {
            if (bMisspelled) // if there any word misspelled, then suggest
            {
                safeC = Terms = Sanitize(Terms);
                //Redo search
                nres = DoSearch(Terms);

                if (nres > 0)
                {
                    if (mLanguage == 1)
                    {
                        litSuggestion.Text = "<br /><div style=\"background-color:#feffe2;\"><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\"><tr><td style=\"vertical-align:top; padding:10px;\"><img src=\"/images/attention.png\"></td><td style=\"vertical-align:top; padding:15px 10px 0px 10px;\">Your search for <strong>" + safeC + "</strong> produced no results. Displaying result(s) for <strong>" + "<a href='" + LanguagePrefix
                        + "search?prm="
                        + Server.UrlEncode(Terms)
                            //+ EncDec.DESEncrypt(Terms) 
                        + "' style='font-style:italic'>" + Server.UrlEncode(Terms) + "</a></strong> below.</td></tr></table> ";
                    }
                    else
                    {
                        litSuggestion.Text = "<br /><div style=\"background-color:#feffe2;\"><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\"><tr><td style=\"vertical-align:top; padding:10px;\"><img src=\"/images/attention.png\"></td><td style=\"vertical-align:top; padding:15px 10px 0px 10px;\">Votre recherche pour <strong>" + safeC + "</strong> n’a produit aucun résultat. Les résultats de recherche pour <strong>" + "<a href='" + LanguagePrefix
                            + "search?prm="
                            + Server.UrlEncode(Terms)
                            //+ EncDec.DESEncrypt(Terms) 
                            + "' style='font-style:italic'>" + Server.UrlEncode(Terms) + "</a></strong> sont affichés ci-dessous.</td></tr></table> ";
                    }

                    tbSearch.Text = safeC;  // Terms;

                    for (int j = 0; j < mywords.Length; j++)
                    {
                        for (int i = 1; i < mywords[j].terms.Length; i++)
                        {
                            if (RemoveNoiseWords(mywords[j].terms[i], LCIDs[mLanguage - 1]).Length > 0)
                            {
                                if (stemp != "")
                                    stemp += ", ";

                                //stemp += "<a href='/" + Language + "/search?prm=" + EncDec.DESEncrypt(mywords[j].terms[i]) + "' style='font-style:italic'>" + mywords[j].terms[i] + "</a>";
                                stemp += "<a href='" + LanguagePrefix + "search?prm=" + Server.UrlEncode(mywords[j].terms[i]) + "' style='font-style:italic'>" + mywords[j].terms[i] + "</a>";
                                //stemp += mywords[j].terms[i];
                            }
                        }
                    }

                    if (stemp != "")
                    {
                        suggestion = mLanguage == 1 ? "Other suggested word(s): " : "Autres mots suggérés: ";
                        litSuggestion.Text += suggestion + stemp + ".<br /><br />";
                    }
                }
                else
                {
                    if (mLanguage == 1)
                        litSuggestion2.Text = "Your search for <strong>" + safeC + "</strong> was corrected to <strong>" + Terms + "</strong> but produced no results containing all search terms.<br /> ";
                    else
                        litSuggestion2.Text = "Votre recherche de <strong>" + safeC + "</strong> a été corrigée à <strong>" + Terms + "</strong> mais n'a abouti à aucun résultat avec tous les termes de recherche.<br /> ";


                    for (int j = 0; j < mywords.Length; j++)
                    {
                        for (int i = 0; i < mywords[j].terms.Length; i++)
                        {
                            if (RemoveNoiseWords(mywords[j].terms[i], LCIDs[mLanguage - 1]).Length > 0)
                            {
                                if (stemp != "")
                                    stemp += ", ";

                               // stemp += "<a href='/" + Language + "/search?prm=" + EncDec.DESEncrypt(mywords[j].terms[i]) + "' style='font-style:italic'>" + mywords[j].terms[i] + "</a>";
                                stemp += "<a href='" + LanguagePrefix + "search?prm=" + Server.UrlEncode(mywords[j].terms[i]) + "' style='font-style:italic'>" + mywords[j].terms[i] + "</a>";
                            }
                        }
                    }
                    if (stemp != "")
                    {
                        suggestion = mLanguage == 1 ? "Please try with: " : "Prière d'essayer avec";
                        litSuggestion2.Text += suggestion + stemp + ".<br /><br />";
                    }
                }
            }
            else
            {   //Spelled correct
                litSuggestion2.Text = mLanguage == 1 ? "No results found containing all of your search terms. " : "Aucun résultat contenant tous les termes de votre recherche. ";

                string[] searchTerms = searchTerm.Split(new Char[] { ' ' });

                for (int i = 0; i < searchTerms.Length; i++)
                {
                    if (RemoveNoiseWords(searchTerms[i], LCIDs[mLanguage - 1]).Length > 0)
                    {
                        if (stemp != "")
                            stemp += ", ";

                       // stemp += "<a href='/" + Language + "/search?prm=" + EncDec.DESEncrypt(searchTerms[i]) + "' style='font-style:italic'>" + searchTerms[i] + "</a>";
                        stemp += "<a href='" + LanguagePrefix + "search?prm=" + Server.UrlEncode(searchTerms[i]) + "' style='font-style:italic'>" + searchTerms[i] + "</a>";
                    }
                }
                if (stemp != "")
                {
                    suggestion = mLanguage == 1 ? "Please try with: " : "Prière d'essayer avec";
                    litSuggestion2.Text += suggestion + stemp + ".<br /><br />";
                }
            }
        }

        //return nres;
    }

    private string BQString(string s1, string s2)
    {
        return s1.Replace("&keyw=", "&dummy=") + "&keyw=" + s2;
    }

#endregion
}

public class MyKeywords
{
    public string[] terms;

    public MyKeywords()
    {
        terms = new string[5] { "", "", "", "", "" };
    }
}
