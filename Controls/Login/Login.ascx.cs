//#define LOGIN_IN_ACTIVE_DIRECTORY
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;
public partial class Controls_Login_Login : System.Web.UI.UserControl
{
    public Controls_Login_Login() { }
    public Controls_Login_Login(string param) { }
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Redirect("/Membership/Account/Login");
    }

    public void ClickLogin(object s, EventArgs e)
    {

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


            //https://msdn.microsoft.com/en-us/library/ms998347.aspx
            // bool isValidUser = Membership.ValidateUser("user432431", "sdfdsfdsfsd!1");

            //     Response.Write(isValidUser);
            //   return;

            Session["LoggedInID"] = ds.Tables[0].Rows[0]["id"].ToString();


            if (Request.QueryString["ReturnUrl"] != null)
                Response.Redirect(Request.QueryString["ReturnUrl"], true);

            if (Request.QueryString["c"] == null || Request.QueryString["c"] == "")
            {
                if (Request.QueryString["r"] == null || Request.QueryString["r"] == "")
                {
                    if (RouteConfig.isMultilingual)
                    {
                        try { Response.Redirect(CMSHelper.GetLanguagePrefix() + "home"); }
                        catch { Response.Redirect("/"); }
                    }
                    else
                        Response.Redirect("/");

                }
                else
                {
                    string lang = "";
                    if (!Request.QueryString["r"].Contains("/admin/"))
                    {
                        lang = Request.QueryString["l"] == "2" ? "/fr/" : CMSHelper.SeoPrefixEN;
                    }
                    Response.Redirect(lang + Request.QueryString["r"]);
                }
            }
            else
            {
                string stemp = "/admin.aspx?c=" + Request.QueryString["c"];
                if (Request.QueryString["id"] != null)
                    stemp += "&id=" + Request.QueryString["id"];
                Response.Redirect(stemp);
            }
        }
        else
        {
            literr.Text = "<span style=\"text-decoration:none;font-size:11px;font-family:Arial; color:#ff0000;\">Invalid Username or Password. Please try again.</span><a style=\"text-decoration:none;font-size:11px;font-family:Arial; color:#ff0000;\" href=\"forgotpassword\" target=\"_self\">&nbsp;&nbsp;&nbsp;Click here if you forgot your password.</a>";
        }
    }

}