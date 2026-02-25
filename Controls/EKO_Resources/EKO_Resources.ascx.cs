using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class EKO_Resources : System.Web.UI.UserControl
{

    public string Parameter;
    public EKO_Resources() { }
    public EKO_Resources(string p) { Parameter = p; }

    protected string _seo = "";
    protected string _linktopage = "";

    enum Languages { English = 1, French};

    Res_ItemTemplate _item;

    protected void Page_Load(object sender, EventArgs e)
    {       
        _seo = this.Page.RouteData.Values["seo"].ToString().ToLower();
        _linktopage = ConfigurationManager.AppSettings.Get("Resources.Page.Details");

        _item = new Res_ItemTemplate();

        EKO_Filters.doSearch += new AfterFiltersLoaded(Populate);

        if(IsPostBack)
        {
            string ResourceId = hfDownloadId.Value.Replace("btnDownload_", "");
            hfDownloadId.Value = "";
            DownloadFile(ResourceId);
        }        
    }

    private void DownloadFile(string resourceId)
    {
        ResourceSearch res = new ResourceSearch();
        res.DownloadFile(resourceId, Session["LoggedInId"].ToString());
    }

    private void Populate()
    {
        if (EKO_Filters.LibraryId == "" && EKO_Filters.SearchTerm == "")
            return;

        DataSet ds = new DataSet();

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("dbResources")))
        {
            ResourceSearch res = new ResourceSearch("Resources_Search_New", CommandType.StoredProcedure, Session["LoggedInId"].ToString(), (int)Languages.English);
            res.LibraryId = EKO_Filters.LibraryId;
            res.CategoryId = EKO_Filters.CategoryId;
            res.SubCategoryId = EKO_Filters.SubCategoryId;
            res.Keywords = EKO_Filters.SearchTerm;
            res.Save = EKO_Filters.Save;

            SqlDataAdapter dapt = res.Build();
            dapt.SelectCommand.Connection = conn;

            dapt.Fill(ds);
            DataTable dt = ds.Tables[0];

            repeaterResources.DataSource = dt;
            repeaterResources.DataBind();

            Literal litHeader = new Literal();
            if (EKO_Filters.SearchTerm != "" )
            {
                int records = dt.Rows.Count;

                litHeader.Text = res.GetHeaderResult(records, EKO_Filters.SearchTerm, 
                    (EKO_Filters.LibraryId != "" ? EKO_Filters.LibraryName : ""),
                    ConfigurationManager.AppSettings["Resources.Page"] + "?search_term=" + EKO_Filters.SearchTerm

                    //,(EKO_Filters.CategoryId != "" ? EKO_Filters.CategoryName : "")
                    //,(EKO_Filters.SubCategoryId != "" ? EKO_Filters.SubCategoryName : "")
                    );

            }
            else if (ds.Tables[1].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[1].Rows[0];
                litHeader.Text = String.Format("<h1>{0}</h1><p>{1}</p>", dr["name"].ToString(), dr["description"].ToString());

                this.Page.Title += " - " + dr["name"].ToString();

            }
            plHeader.Controls.Add(litHeader);

        }
    }
       
    protected void repeaterResources_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if ((DataRowView)e.Item.DataItem != null)
        {
            DataRowView rw = (DataRowView)e.Item.DataItem;
            PlaceHolder ph = (PlaceHolder)e.Item.FindControl("plContent");

            Literal litContent = new Literal();

            litContent.Text = _item.GetContent(rw, _linktopage);

            ph.Controls.Add(litContent);

        }
    }

}