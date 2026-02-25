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
      //  set { try { ddlLib.SelectedValue = value; } catch { } }
    }
    public string CategoryId
    {
        get { return ddlCateg.SelectedValue; }
       // set { try { ddlCateg.SelectedValue = value; } catch { } }
    }
    public string SubCategoryId
    {
        get { return ddlSubcateg.SelectedValue; }
        //set { try { ddlSubcateg.SelectedValue = value; } catch { } }
    }

    public string LibraryName
    {
        get { return ddlLib.SelectedItem.Text; }
    }
    public string CategoryName
    {
        get { return ddlCateg.SelectedItem.Text; }
    }
    public string SubCategoryName
    {
        get { return ddlSubcateg.SelectedItem.Text; }
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
    public string SubCategory
    {
        get
        {
            string sret = "";
            if (Request.QueryString["subcategory"] != null)
                sret = Request.QueryString["subcategory"];

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

    public string SelectWord = "select";
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
        string rowfilterLib = "", rowfilterCat = "", rowfilterSubcat = "";
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("dbResources")))
        {
            SqlDataAdapter dapt = new SqlDataAdapter("res.Library_ddl", conn);
            dapt.SelectCommand.CommandType = CommandType.StoredProcedure;
            dapt.SelectCommand.Parameters.AddWithValue("@userid", Session["LoggedInId"].ToString());

            #region SQL Parameters
            if (!String.IsNullOrEmpty(Library))
            {
                dapt.SelectCommand.Parameters.AddWithValue("@lib", Library);
                rowfilterLib = "seo = '" + Library + "'";
            
                if (!String.IsNullOrEmpty(Category))
                {
                    dapt.SelectCommand.Parameters.AddWithValue("@cat", Category);
                    rowfilterCat = "seo = '" + Category + "'";

                    if (!String.IsNullOrEmpty(SubCategory))
                        rowfilterSubcat = "seo = '" + SubCategory + "'";
                }
            }

            #endregion

            dapt.Fill(ds);
            DataTable dt = ds.Tables[0];

            #region Library
            ddlLib.DataSource = dt;
            ddlLib.DataBind();
            ddlLib.Items.Insert(0, new ListItem(SelectWord, ""));
            ddlLib.Attributes.Add("aria-label", "select library");

            DataRow[] drs = dt.Select(rowfilterLib);
            if (drs.Length == 1)
            {
                ddlLib.SelectedValue = drs[0]["id"].ToString();
            }
            #endregion

            #region Categories
            DataTable dtcateg = ds.Tables[1];
            if (dtcateg.Rows.Count > 0)
            {

                #region Category
                ddlCateg.DataSource = dtcateg;
                ddlCateg.DataBind();
                ddlCateg.Items.Insert(0, new ListItem(SelectWord, ""));
                ddlCateg.Attributes.Add("aria-label", "select category");

                drs = dtcateg.Select("id = 0");
                if (rowfilterCat != "")
                {
                    drs = dtcateg.Select(rowfilterCat);
                    if (drs.Length == 1)
                    {
                        ddlCateg.SelectedValue = drs[0]["id"].ToString();
                    }
                }
                #endregion

                #region Subcategory
                DataTable dtSubcateg = ds.Tables[2];
                if (dtSubcateg.Rows.Count > 0)
                {
                    ddlSubcateg.DataSource = dtSubcateg;
                    ddlSubcateg.DataBind();
                    ddlSubcateg.Items.Insert(0, new ListItem(SelectWord, ""));

                    drs = dtSubcateg.Select("id = 0");
                    if (rowfilterSubcat != "")
                    {
                        drs = dtSubcateg.Select(rowfilterSubcat);
                        if (drs.Length == 1)
                        {
                            ddlSubcateg.SelectedValue = drs[0]["id"].ToString();
                        }
                    }
                }
                else
                {
                    //this.Page.ClientScript.RegisterClientScriptBlock(GetType(), "hide_Categ", "$(document).ready(function () {$('#sub-filter div').hide();});", true);
                    ddlSubcateg.Items.Insert(0, new ListItem(SelectWord, ""));

                }
                ddlSubcateg.Attributes.Add("aria-label", "select subcategory");

                #endregion
            }
            else
            {
                //this.Page.ClientScript.RegisterClientScriptBlock(GetType(), "hide_Categ", "$(document).ready(function () {$('#cat-filter div').hide();$('#sub-filter div').hide();});", true);
                ddlCateg.Items.Insert(0, new ListItem(SelectWord, ""));
                ddlSubcateg.Items.Insert(0, new ListItem(SelectWord, ""));

            }
            #endregion

        }
    }
}