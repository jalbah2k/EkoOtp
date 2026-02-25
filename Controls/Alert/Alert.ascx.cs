using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Controls_Alert_Alert : System.Web.UI.UserControl
{
    private string parameter;
    public Controls_Alert_Alert() { parameter = "1";  }
    public Controls_Alert_Alert(string s) { parameter = s; }

    public string Language
    {
        get { return CMSHelper.GetLanguageNumber(); }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindData();
        }
    }

    private void BindData()
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("CMServer"));

        SqlDataAdapter dapt = new SqlDataAdapter("select * from Alerts where id=@id and status=1", conn);
        dapt.SelectCommand.CommandType = CommandType.Text;
        dapt.SelectCommand.Parameters.AddWithValue("@id", parameter);
        DataTable dt = new DataTable();
        dapt.Fill(dt);

        if(header_alert.Visible = (dt.Rows.Count > 0))
        {
           // litTile.Text = "<img src='/images/icons/icon-alert@2x.png' alert'alert icon' /><span class='alert-title'>" +  dt.Rows[0]["title"].ToString().ToUpper() + "</span>";
            litTile.Text = "<span class='alert-title'>" +  dt.Rows[0]["title"].ToString().ToUpper() + "</span>";
			litBody.Text = dt.Rows[0]["body"].ToString();
        }
        else
        {
            litTile.Text = "";
            litBody.Text = "";
        }
    }
}