using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public delegate void LoadContent(int id);

public partial class Controls_Blog_BlogList : System.Web.UI.UserControl
{
    public LoadContent delegateLoadContent;

    public int group = 1;
    private int ItemIndex1;
    private string sid;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindData();
        }
    }

    private void BindData()
    {
        DataTable dt = new DataTable();
        dt = getTable(sqlYear);
        if (dt.Rows.Count > 0)
        {
            DataView DV = dt.DefaultView;
            this.Repeater1.DataSource = DV;
            this.Repeater1.DataBind();
        }
    }

    #region dal

    protected string _connection = ConfigurationManager.AppSettings.Get("CMServer");
    protected string sql = "AskLindsaySelect";
    protected string sqlYear = "AskLindsayYearSelect";

    private DataTable getTable(string cmd)
    {
        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter(cmd, _connection);
        da.SelectCommand.CommandType = CommandType.StoredProcedure;
        da.Fill(dt);
        return dt;
    }
    private DataTable getTable(string cmd, string year)
    {
        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter(cmd, _connection);
        da.SelectCommand.CommandType = CommandType.StoredProcedure;
        da.SelectCommand.Parameters.AddWithValue("@Year", year);
        da.Fill(dt);
        return dt;
    }
    #endregion dal
    protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.DataItem != null)
        {
            ItemIndex1 = e.Item.ItemIndex;
            Repeater Repeater2 = (Repeater)e.Item.FindControl("Repeater2");

            DataTable dt = new DataTable();
            dt = getTable(sql, ((DataRowView)e.Item.DataItem)["MyYear"].ToString());
            if (dt.Rows.Count > 0)
            {
                DataView DV = dt.DefaultView;
                DV.Sort = "NewsDate desc";
                Repeater2.DataSource = DV;
                Repeater2.DataBind();
            }
        }
    }
    protected void Repeater2_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.DataItem != null )
        {            
            int id;
            id = int.Parse(((DataRowView)e.Item.DataItem)["id"].ToString());

            if (Request.QueryString["id"] != null)
            {
                try
                {
                    sid = int.Parse(Request.QueryString["id"]).ToString();
                }
                catch { }
            }

            if (sid == id.ToString())
                group = ItemIndex1 + 1;
        }
    }


    protected void Repeater2_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "link")
        {            
            sid = e.CommandArgument.ToString();
            string str = "?id=";
            string url = "/BarbsBlog";
            int pos = url.IndexOfAny(str.ToCharArray());

            if (pos > 0)
            {
                url = url.Remove(pos);
            }
            Response.Redirect(url + str + sid);

            //BindData();
            //delegateLoadContent( int.Parse(sid));
        }
    }
}