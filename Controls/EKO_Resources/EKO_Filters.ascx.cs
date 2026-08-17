using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

public partial class Filters : System.Web.UI.UserControl
{
    public AfterFiltersLoaded doSearch;

    #region Properties 
    public string LibraryId
    {
        get { return ddlLib.SelectedValue; }
    }
    public string CategoryId
    {
        get { return ddlCateg.SelectedValue; }
    }
    public string FormatId
    {
        get { return ddlFormat.SelectedValue; }
    }
    public string AudienceId
    {
        get { return ddlAudience.SelectedValue; }
    }

    public string LibraryName
    {
        get { return ddlLib.SelectedItem.Text; }
    }
    public string CategoryName
    {
        get { return ddlCateg.SelectedItem.Text; }
    }
    public string FormatName
    {
        get { return ddlFormat.SelectedItem.Text; }
    }
    public string AudienceName
    {
        get { return ddlAudience.SelectedItem.Text; }
    }
    public string Library 
    { 
        get 
        {
            string sret = "";
            if (Request.QueryString["library"] != null)
                sret =  Request.QueryString["library"];

            return sret;
        } 
    }
    public string Category
    {
        get
        {
            string sret = "";
            if (Request.QueryString["category"] != null)
                sret = Request.QueryString["category"];

            return sret;
        }
    }
    public string Format
    {
        get
        {
            string sret = "";
            if (Request.QueryString["format"] != null)
                sret = Request.QueryString["format"];

            return sret;
        }
    }
    public string Audience
    {
        get
        {
            string sret = "";
            if (Request.QueryString["audience"] != null)
                sret = Request.QueryString["audience"];

            return sret;
        }
    }
    public string SearchTerm
    {
        get
        {
            string sret = "";
            if (Request.QueryString["search_term"] != null)
                sret = Request.QueryString["search_term"];

            return sret;
        }
    }

    public string Save
    {
        get
        {
            string sret = "";
            if (Request.QueryString["save"] != null)
                sret = Request.QueryString["save"];

            return sret;
        }
    }
    public string MyUrl
    {
        get { return Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, "") + Request.Url.AbsolutePath; }
    }

    public string AllLibrariesWord = "All libraries";
    public string AllCategoriesWord = "All categories";
    public string AllFormatsWord = "All formats";
    public string AllAudiencesWord = "All audiences";
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            PopulateDDLs();
            txtSearch.Text = SearchTerm;
        }

        doSearch();
    }

    private void PopulateDDLs()
    {
        DataSet ds = new DataSet();
        string rowfilterLib = "", rowfilterCat = "", rowfilterFormat = "", rowfilterAudience = "";
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("dbResources")))
        {
            SqlDataAdapter dapt = new SqlDataAdapter("res.Library_ddl", conn);
            dapt.SelectCommand.CommandType = CommandType.StoredProcedure;
            dapt.SelectCommand.Parameters.AddWithValue("@userid", Session["LoggedInId"].ToString());

            #region SQL Parameters
            if (!String.IsNullOrEmpty(Library))
            {
                dapt.SelectCommand.Parameters.AddWithValue("@lib", Library);
                rowfilterLib = BuildRowFilter(Library);

                if (!String.IsNullOrEmpty(Category))
                {
                    dapt.SelectCommand.Parameters.AddWithValue("@cat", Category);
                    rowfilterCat = BuildRowFilter(Category);
                }
            }

            if (!String.IsNullOrEmpty(Format))
            {
                dapt.SelectCommand.Parameters.AddWithValue("@format", Format);
                rowfilterFormat = BuildRowFilter(Format);
            }

            if (!String.IsNullOrEmpty(Audience))
            {
                dapt.SelectCommand.Parameters.AddWithValue("@audience", Audience);
                rowfilterAudience = BuildRowFilter(Audience);
            }
            #endregion

            dapt.Fill(ds);

            BindFilterDropdown(ddlLib, ds.Tables.Count > 0 ? ds.Tables[0] : null, AllLibrariesWord, rowfilterLib, "select library");
            BindFilterDropdown(ddlCateg, ds.Tables.Count > 1 ? ds.Tables[1] : null, AllCategoriesWord, rowfilterCat, "select category");
            BindFilterDropdown(ddlFormat, ds.Tables.Count > 2 ? ds.Tables[2] : null, AllFormatsWord, rowfilterFormat, "select format");
            BindFilterDropdown(ddlAudience, ds.Tables.Count > 3 ? ds.Tables[3] : null, AllAudiencesWord, rowfilterAudience, "select audience");
        }
    }

    private static string BuildRowFilter(string qsValue)
    {
        string escaped = qsValue.Replace("'", "''");
        int id;
        if (int.TryParse(qsValue, out id))
            return "id = " + id;
        return "seo = '" + escaped + "'";
    }

    private void BindFilterDropdown(DropDownList ddl, DataTable dt, string emptyText, string rowFilter, string ariaLabel)
    {
        ddl.Items.Clear();
        if (dt != null && dt.Rows.Count > 0)
        {
            ddl.DataSource = dt;
            ddl.DataBind();
        }
        ddl.Items.Insert(0, new ListItem(emptyText, ""));
        ddl.Attributes.Add("aria-label", ariaLabel);

        if (!String.IsNullOrEmpty(rowFilter) && dt != null)
        {
            DataRow[] drs = dt.Select(rowFilter);
            if (drs.Length == 1)
                ddl.SelectedValue = drs[0]["id"].ToString();
        }
    }
}
