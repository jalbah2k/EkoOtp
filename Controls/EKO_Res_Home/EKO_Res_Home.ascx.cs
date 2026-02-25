using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class EKO_Res_Home : System.Web.UI.UserControl
{
    public string Parameter;
    public EKO_Res_Home()
    {
    }
    public EKO_Res_Home(string p)
    {
        Parameter = p;
    }

    #region Properties    

    protected string _linktopage = "";
    Res_ItemTemplate _item;

    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        _linktopage = ConfigurationManager.AppSettings.Get("Resources.Page.Details");
        _item = new Res_ItemTemplate(); 
        
        Populate();

        if (IsPostBack)
        {
            string resourceId = hfDownloadId.Value.Replace("btnDownload_", "");
            hfDownloadId.Value = "";
            DownloadFile(resourceId);
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
            SqlDataAdapter dapt = new SqlDataAdapter("Resources_Daily_Select", conn);

            dapt.Fill(ds);
            DataTable dt = ds.Tables[0];

            #region Associated resources
            if (pnlResources.Visible = dt.Rows.Count > 0)
            {
                litLatestRes.Text = "<h2>Today's Featured Resources</h2>";

                repeaterResources.DataSource = dt;
                repeaterResources.DataBind();

            }
            else
                litLatestRes.Text = "";


            #endregion

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