using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

public partial class EKO_Search : System.Web.UI.UserControl
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            Populate();
        }
    }

    private void Populate()
    {
        string sql = @"  select top 3 id, querystring, keywords, [parameters] from
                          (select max(id) id, querystring, keywords, [parameters] from Searches where userid=@userid and resources=1 
                          --and  keywords=[parameters]
                          group by querystring, keywords, [parameters]) t
                          order by id desc";
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("dbResources")))
        {
            DataSet ds = new DataSet();
            SqlDataAdapter dapt = new SqlDataAdapter(sql, conn);
            dapt.SelectCommand.CommandType = CommandType.Text;

            dapt.SelectCommand.Parameters.AddWithValue("@userid", Session["LoggedInID"].ToString());

            dapt.Fill(ds);
            DataTable dt = ds.Tables[0];

            repSearches.DataSource = dt;
            repSearches.DataBind();
        }
    }

    protected void btSearch_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        if (txtSearch.Text.Trim().Length > 0 
            //&& tbSearch.Text != enSearch 
            )
        {
            Response.Redirect("resources?search_term=" + txtSearch.Text.Trim() + "&save=1");
        }
    }

    protected void repSearches_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if ((DataRowView)e.Item.DataItem != null)
        {
            DataRowView rw = (DataRowView)e.Item.DataItem;
            Literal litItem = (Literal)e.Item.FindControl("litItem");

            litItem.Text = String.Format("<div><a href='/resources?{0}'>{1}</a></div>", 
                rw["querystring"].ToString(), 
                rw["keywords"].ToString() + ((rw["parameters"].ToString() != rw["keywords"].ToString()) ? " *" : "")
                );
        }
    }
}