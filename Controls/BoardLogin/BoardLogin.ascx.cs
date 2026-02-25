using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Controls_BoardLogin_BoardLogin : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            //txtUsername.Text = "USERNAME";
            //txtPassword.Text = "PASSWORD";
        }

        btnSubmit.Attributes.Add("aria-hidden", "true");
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (txtUsername.Text == "" || txtPassword.Text == "")
            return;

        if (txtUsername.Text == "USERNAME" || txtPassword.Text == "PASSWORD")
            return;


        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
        SqlDataAdapter dapt = new SqlDataAdapter("BASE_Login", sqlConn);
        dapt.SelectCommand.CommandType = CommandType.StoredProcedure;
        dapt.SelectCommand.Parameters.AddWithValue("@username", txtUsername.Text);
        dapt.SelectCommand.Parameters.AddWithValue("@password", CMSHelper.HashString(txtPassword.Text));

        //Response.Write(CMSHelper.HashString(txtPassword.Text));
        //return;

        DataSet ds = new DataSet();
        dapt.Fill(ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            //FormsAuthentication.SetAuthCookie(
            //     this.txtUsername.Text.Trim(), false);

            string roles = string.Join(",", Roles.GetRolesForUser(this.txtUsername.Text.Trim()));

            FormsAuthenticationTicket tkt =
               new FormsAuthenticationTicket(
                    2,                                  // version
                    this.txtUsername.Text.Trim(),       // get username from the form
                    DateTime.Now,                       // issue time is now
                    DateTime.Now.AddMinutes(30),        // expires in 30 minutes
                    false,          //cbRem.Checked,                      // is cookie persistent?
                    roles                               // role assignment is stored in userData
                    );
            string cookiestr = FormsAuthentication.Encrypt(tkt);
            HttpCookie ck = new HttpCookie(FormsAuthentication.FormsCookieName, cookiestr);
            //////if (cbRem.Checked)
            //////    ck.Expires = tkt.Expiration;
            ck.Path = FormsAuthentication.FormsCookiePath;
            Response.Cookies.Add(ck);

            Response.Redirect("/boardportal");
        }

        //Response.Write("submitted");
        txtUsername.Text = "";
        txtPassword.Text = "";
    }
}