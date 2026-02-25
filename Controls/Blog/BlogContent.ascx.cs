using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Controls_Blog_BlogContent : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindData(0);
        }
    }

    public void BindData(int id)
    {
        if (Request.QueryString["id"] != null)
        {
            try
            {
                id = int.Parse(Request.QueryString["id"]);
            }
            catch { }
        }

        DataTable dt = new DataTable();
        dt = getTable(sql, id);
        if (dt.Rows.Count > 0)
        {
            DataView DV = dt.DefaultView;
            DV.Sort = "NewsDate desc";
            litTitle.Text = dt.Rows[0]["Title"].ToString();
            litDate.Text = Convert.ToDateTime(dt.Rows[0]["NewsDate"].ToString()).ToShortDateString();
            litContent.Text = dt.Rows[0]["Details"].ToString();
            
        }
    }

    #region dal

    protected string _connection = ConfigurationManager.AppSettings.Get("CMServer");
    protected string sql = "AskLindsayGet";
   
    private DataTable getTable(string cmd, int id)
    {
        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter(cmd, _connection);
        da.SelectCommand.CommandType = CommandType.StoredProcedure;
        if( id > 0)
            da.SelectCommand.Parameters.AddWithValue("@id", id.ToString());
        da.Fill(dt);
        return dt;
    }
    #endregion dal
}