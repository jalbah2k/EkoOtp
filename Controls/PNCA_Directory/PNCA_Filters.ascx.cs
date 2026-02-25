using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

public partial class Filters : System.Web.UI.UserControl
{
    public AfterPNCAFiltersLoaded doSearch;

    #region Properties 
    public string ServiceId
    {
        get { return ddlService.SelectedValue; }
    }
    public string RegionId
    {
        get { return ddlRegions.SelectedValue; }
       // set { try { ddlCateg.SelectedValue = value; } catch { } }
    }
    public string CityId
    {
        get { return ddlCities.SelectedValue; }
        //set { try { ddlSubcateg.SelectedValue = value; } catch { } }
    }

    public string ServiceName
    {
        get { return ddlService.SelectedItem.Text; }
    }
    public string RegionName
    {
        get { return ddlRegions.SelectedItem.Text; }
    }
    public string CityName
    {
        get { return ddlCities.SelectedItem.Text; }
    }
    public string Service 
    { 
        get 
        {
            string sret = "";
            if (Request.QueryString["service"] != null)
                sret =  Request.QueryString["service"];

            return sret;
        } 
    }
    public string Region
    {
        get
        {
            string sret = "";
            if (Request.QueryString["region"] != null)
                sret = Request.QueryString["region"];

            return sret;
        }
    }
    public string City
    {
        get
        {
            string sret = "";
            if (Request.QueryString["city"] != null)
                sret = Request.QueryString["city"];

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
    public string MyUrl
    {
        get { return Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, "") + Request.Url.AbsolutePath; }
    }

    public string SelectWordServ = "All Services";
    public string SelectWordReg = "All Regions";
    public string SelectWordCity = "All Areas";
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["LoggedInId"] != null)
        {
            if (!IsPostBack)
            {
                PopulateDDLs();
                txtSearch.Text = SearchTerm;
            }

            doSearch();
        }
    }

    private void PopulateDDLs()
    {
        DataSet ds = new DataSet();
        string rowfilterSer = "", rowfilterReg = "", rowfilterCity = "";
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("CMServer")))
        {
            SqlDataAdapter dapt = new SqlDataAdapter("pnca.Filters_Sel", conn);
            dapt.SelectCommand.CommandType = CommandType.StoredProcedure;

            #region SQL Parameters
            if (!String.IsNullOrEmpty(Service))
            {
                dapt.SelectCommand.Parameters.AddWithValue("@service", Service);
                rowfilterSer = "ServiceSeo = '" + Service + "'";
            }

            if (!String.IsNullOrEmpty(Region))
            {
                dapt.SelectCommand.Parameters.AddWithValue("@region", Region);
                rowfilterReg = "RegionSeo = '" + Region + "'";
            }

            if (!String.IsNullOrEmpty(City))
            {
                rowfilterCity = "CitySeo = '" + City + "'";
            }

            #endregion


            dapt.Fill(ds);
            DataTable dt = ds.Tables[0];

            #region Services
            ddlService.DataSource = dt;
            ddlService.DataBind();
            ddlService.Items.Insert(0, new ListItem(SelectWordServ, ""));

            if (rowfilterSer != "")
            {
                DataRow[] drs = dt.Select(rowfilterSer);
                if (drs.Length == 1)
                {
                    ddlService.SelectedValue = drs[0]["id"].ToString();
                }
            }
            #endregion

            #region Regions
            DataTable dtRegions = ds.Tables[1];
            if (dtRegions.Rows.Count > 0)
            {

                ddlRegions.DataSource = dtRegions;
                ddlRegions.DataBind();
                ddlRegions.Items.Insert(0, new ListItem(SelectWordReg, ""));

                if (rowfilterReg != "")
                {
                    DataRow[] drs = dtRegions.Select(rowfilterReg);
                    if (drs.Length == 1)
                    {
                        ddlRegions.SelectedValue = drs[0]["id"].ToString();
                    }
                }

            }
            #endregion

            #region Cities
            DataTable dtCities = ds.Tables[2];
            if (dtCities.Rows.Count > 0)
            {
                ddlCities.DataSource = dtCities;
                ddlCities.DataBind();
                ddlCities.Items.Insert(0, new ListItem(SelectWordCity, ""));

                if (rowfilterCity != "")
                {
                    DataRow[] drs = dtCities.Select(rowfilterCity);
                    if (drs.Length == 1)
                    {
                        ddlCities.SelectedValue = drs[0]["id"].ToString();
                    }
                }
            }
            #endregion

        }
    }
}