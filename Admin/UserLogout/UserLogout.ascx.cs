using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;

public partial class Admin_UserLogout_UserLogout : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]))
            {
                SqlDataAdapter dapt = new SqlDataAdapter("select case when isnull(name, '') <> '' then name else username end as name, dateloggedin from Users where username=@username", conn);
                dapt.SelectCommand.Parameters.AddWithValue("@username", HttpContext.Current.User.Identity.Name);
                dapt.Fill(dt);
            }
            if (dt.Rows.Count > 0)
            {
                lblUser.Text = dt.Rows[0]["name"].ToString();
                lblUser.ToolTip = "Logged in since: " + Convert.ToDateTime(dt.Rows[0]["dateloggedin"]).ToString("MMM d, yyyy h:mm tt");
            }
            else
            {
                lblUser.Text = HttpContext.Current.User.Identity.Name;
            }
        }
    }
}