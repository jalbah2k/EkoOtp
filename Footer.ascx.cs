#define SHOW_EkoWalk
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Footer : System.Web.UI.UserControl
{
    enum Groups { Common = 1, EKOMembers }
    public bool ShowEkoWalk = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["LoggedInID"] != null)
            {
                string seo = "";
                if (Request.Url.AbsolutePath.Contains("error.aspx"))
                    seo = "error";
                else
                    seo = this.Page.RouteData.Values["seo"].ToString().ToLower();

                DataSet ds = new DataSet();

                SqlDataAdapter dap = new SqlDataAdapter(@"eko.GetWalkInfo", ConfigurationManager.AppSettings["CMServer"]);

                dap.SelectCommand.CommandType = CommandType.StoredProcedure;
                dap.SelectCommand.Parameters.AddWithValue("@uid", Session["LoggedInID"].ToString());
                dap.SelectCommand.Parameters.AddWithValue("@groupid", (int)Groups.EKOMembers);
                dap.SelectCommand.Parameters.AddWithValue("@seo", seo);

                if (Request.QueryString["step"] != null)
                {
                    //int step = 0;
                    //if(int.TryParse(Request.QueryString["step"], out step))
                        dap.SelectCommand.Parameters.AddWithValue("@step", Request.QueryString["step"]);
                }

                dap.Fill(ds);

                DataTable dtm = ds.Tables[0];
                DataTable dtg = ds.Tables[1];
                DataTable dtu = ds.Tables[2];

                litBottomMenu.Text = "<ul id='miniMemberMenu'>";
                if (dtm.Rows.Count > 0)
                {
                    litBottomMenu.Text += @"<li><a href='/EKOMembers'>Go to Dashboard</a></li>
                                            <li><a href='/tips-and-tricks'>Tips &amp; Tricks</a></li>";

#if EkoWalk
                    if(dtg.Rows.Count > 0 && dtu.Rows.Count > 0)
                    {
                        if( Convert.ToBoolean(dtg.Rows[0]["EkoWalk"]) &&
                            dtu.Rows[0]["EkoWalkStatus"].ToString() == "1")
                        {
                            ShowEkoWalk = true; 

                            string script = Environment.NewLine +
                                                @"$(document).ready(function () {
                                                    $(body).addClass('show-walk');      
                                                    $(body).attr('ekowalkstep', '{0}');    
                                                });" + Environment.NewLine;

                            script = script.Replace("{0}", dtu.Rows[0]["EkoWalkStep"].ToString());

                            ((_Default)this.Page).InjectContent("Scripts", script, true);

                        }
                       // else if (dtu.Rows[0]["EkoWalkStatus"].ToString() == "3")
                        {
                           // litBottomMenu.Text += " <li><a href='#' class='open-walk'>Interactive Walkthrough</a></li>"; 
                        }
                    }
#endif
                }

                litBottomMenu.Text += "<li><a href='javascript: LogOutClick()'>Logout of Members’ Portal</a></li></ul>";

            }
        }
    }
}