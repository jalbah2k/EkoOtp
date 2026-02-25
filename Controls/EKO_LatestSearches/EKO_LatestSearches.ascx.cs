using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

public partial class EKO_LatestSearches : System.Web.UI.UserControl
{
    public int records;

    public EKO_LatestSearches() 
    { 
        records = 0;
    }
    public EKO_LatestSearches(string a) 
    {

        try
        {
            string[] s = a.Split(new char[] { ',' });
            records = Convert.ToInt32(s[0]);
        }
        catch
        {
            records = 0;
        }
    }
    protected int total
    {
        set { ViewState["total"] = value; }
        get
        {
            try { return Convert.ToInt32(ViewState["total"]); }
            catch { return records; }
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            total = records;
            BindData();
        }
    }

    public bool bLoadMore = false;
    private void BindData()
    {
        string sql = "MyLastSearches";
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("dbResources")))
        {
            DataSet ds = new DataSet();
            SqlDataAdapter dapt = new SqlDataAdapter(sql, conn);
            dapt.SelectCommand.CommandType = CommandType.StoredProcedure;

            dapt.SelectCommand.Parameters.AddWithValue("@userid", Session["LoggedInID"].ToString());
            dapt.SelectCommand.Parameters.AddWithValue("@top", records);
            //dapt.SelectCommand.Parameters.AddWithValue("@resorces_only", resources);

            dapt.Fill(ds);
            DataTable dt = ds.Tables[0];

            repSearches.DataSource = dt;
            repSearches.DataBind();

            if (bLoadMore = (dt.Rows.Count >= 3))
            {
                ((_Default)this.Page).InjectContent("Scripts", "<script src=\"/controls/EKO_LatestSearches/LoadMore.js\"></script>" + Environment.NewLine + Environment.NewLine);
            }
        }
    }

    protected void repSearches_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if ((DataRowView)e.Item.DataItem != null)
        {
            DataRowView rw = (DataRowView)e.Item.DataItem;
            Literal litItem = (Literal)e.Item.FindControl("litItem");

            litItem.Text = String.Format("<div>{3} - <a href='/resources?{0}&m={4}' target='{2}'>{1}</a></div>", 
                rw["querystring"].ToString(), 
                rw["parameters"].ToString(),
                rw["target"].ToString(),
                Convert.ToDateTime(rw["timestamp"]).ToString("MMMM dd, yyyy")
                ,rw["id"].ToString()
                );
        }
    }
}