using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class oTreeMenuCollection : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadMenus();
        }

    }

    public void LoadMenus()
    {
        string selected = "";
        if (Request.QueryString["m"] != null)
        {
            selected = Request.QueryString["m"];
        }

        DataTable dt = mGet_RootMenuItems();
        foreach (DataRow dr in dt.Rows)
        {
            Control control = LoadControl("oTreeMenu.ascx");
            plcTreeViews.Controls.Add(control);

            ((oTreeMenu)control).selectedNode = selected;
            ((oTreeMenu)control).PopulateFields(dr["id"].ToString(), false);
        }
    }

    #region DAL
 
    public DataTable mGet_RootMenuItems()
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = "select id,name from Menus where language ='1'";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds, "table1");
        }

        return ds.Tables[0];

    }
    #endregion   

}