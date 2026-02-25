using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class EKO_Members_Welcome : System.Web.UI.UserControl
{
    public EKO_Members_Welcome() { }
    public EKO_Members_Welcome(string p) { }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["LoggedInId"] != null)
            {
                DataSet ds = new DataSet();

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("CMServer")))
                {
                    SqlDataAdapter dapt = new SqlDataAdapter(@"select FirtsName, LastName from eko.Members where userid=@userid
                                                                select * from Users 
                                                                where id in (select User_id from Users_Groups_Access where User_id=@userid and Group_id=76 ) /*PNCARegisteredUser*/
                                                                and id not in (select User_id from Users_Groups_Access where User_id=@userid and Group_id=33) /*Registered Users*/", conn);
                    dapt.SelectCommand.CommandType = CommandType.Text;
                    dapt.SelectCommand.Parameters.AddWithValue("@userid", Session["LoggedInId"].ToString());

                    dapt.Fill(ds);
                    DataTable dt = ds.Tables[0];

                    //if (ds.Tables[1].Rows.Count > 0)
                    //    Response.Redirect("PNCAMembers");

                    if (dt.Rows.Count > 0)
                    {
                        litTitle.Text = "<h1>Welcome Back " + dt.Rows[0]["FirtsName"].ToString() + "!</h1>";
                    }

                }
            }
        }
    }
}