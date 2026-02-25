using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class EKO_Favorites : System.Web.UI.UserControl
{
    public EKO_Favorites() { }
    public EKO_Favorites(string p) { }
    protected string _seo = "";
    protected string _linktopage = "";

    enum Languages { English = 1, French };

    Res_ItemTemplate _item;

    protected void Page_Load(object sender, EventArgs e)
    {
        _seo = this.Page.RouteData.Values["seo"].ToString().ToLower();
        _linktopage = ConfigurationManager.AppSettings.Get("Resources.Page.Details");

        _item = new Res_ItemTemplate();

        Populate();

        if (IsPostBack)
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

        DataSet ds = new DataSet();

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("dbResources")))
        {
            ResourceSearch res = new ResourceSearch("Resources_Search_New", CommandType.StoredProcedure, Session["LoggedInId"].ToString(), (int)Languages.English);
            //res.LibraryId = EKO_Filters.LibraryId;
            //res.CategoryId = EKO_Filters.CategoryId;
            //res.SubCategoryId = EKO_Filters.SubCategoryId;

            res.Keywords = String.Empty;           

            SqlDataAdapter dapt = res.BuildFavourite();
            dapt.SelectCommand.Connection = conn;

            dapt.Fill(ds);
            DataTable dt = ds.Tables[0];
            DataView dv = dt.DefaultView;

            repeaterResources.DataSource = dv;
            repeaterResources.DataBind();

            //Literal litHeader = new Literal();

            //litHeader.Text = String.Format("<h1>{0}</h1>", "My Favourites");
            //plHeader.Controls.Add(litHeader);

            string stemp = "Welcome {0} <h1>Your Favourites:</h1>";
            string myname = "";
            if (ds.Tables[2].Rows.Count > 0)
                myname = ds.Tables[2].Rows[0]["name"].ToString();

            EKO_Breadcrumbs1.Content = String.Format(stemp, myname);

        }
    }

    protected void repeaterResources_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if ((DataRowView)e.Item.DataItem != null)
        {
            DataRowView rw = (DataRowView)e.Item.DataItem;
            PlaceHolder ph = (PlaceHolder)e.Item.FindControl("plContent");

            Literal litContent = new Literal();

            litContent.Text = _item.GetFullContent(rw, _linktopage);

            ph.Controls.Add(litContent);

        }
    }
}