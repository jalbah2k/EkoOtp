using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class EKO_Directory_Featured : System.Web.UI.UserControl
{
    protected string imgPath { set; get; }

    public string BackgroundPosition { set; get; }
    public EKO_Directory_Featured() { }
    public EKO_Directory_Featured(string parm) { }
    protected void Page_Load(object sender, EventArgs e)
    {
        BackgroundPosition = ";";

        imgPath = ConfigurationManager.AppSettings["Organizations.Logo.Path"];

        LoadItem();
    }

    public string BackgroundImage = "/Images/kidsability.png";
    private void LoadItem()
    {
        DataTable dt = new DataTable();

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]))
        {
            string sqlstr = @"select top 1 id, Name, SEO, AboutUs, URL, Logo, AltTextLogo, Image, AltTextImg, BackgroundPosition, BackgroundPosition_Horizontal 
                                from eko.Organizations 
                                where featured=1 and active=1 and deleted=0 and [Type]=1 ";

            SqlDataAdapter dapt = new SqlDataAdapter(sqlstr, ConfigurationManager.AppSettings["CMServer"]);
            dapt.Fill(dt);
        }

        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];

            string extraclass = "no-img";
            if (dr["Image"].ToString() != "")
            {
                // litImage.Text = String.Format("<img src=\"{0}\" alt=\"{1}\" />", imgPath + dr["id"].ToString() + "/" + dr["Image"].ToString(), dr["AltTextImg"].ToString());

                if (dr["Image"].ToString() != "")
                    BackgroundImage = imgPath + dr["id"].ToString() + "/" + dr["Image"].ToString();

                if (dr["BackgroundPosition"].ToString() != "" || dr["BackgroundPosition_Horizontal"].ToString() != "")
                {
                    BackgroundPosition = "background-position:" + dr["BackgroundPosition"].ToString() + " " +
                                              dr["BackgroundPosition_Horizontal"].ToString() + ";";
                }

                extraclass = "with-img";
            }

            string script = "$(document).ready(function () { $('#homeFeatMember').addClass('" + extraclass + "');}); ";
            ((_Default)this.Page).InjectContent("Scripts", script, true);

            litContent.Text = "<h2>FEATURED MEMBER</h2>";
            if (dr["Logo"].ToString() != "")
            {
                litContent.Text += String.Format("<img src=\"{0}\" alt=\"{1}\" />", imgPath + dr["id"].ToString() + "/" + dr["Logo"].ToString(), dr["AltTextLogo"].ToString());
            }

            if (dr["AboutUs"].ToString() != "")
            {
                litContent.Text += "<p>" + dr["AboutUs"].ToString() + "</p><br>";
            }

            litContent.Text += String.Format("<a href=\"{0}/{1}\" class=\"button1\">View Member Profile</a>", ConfigurationManager.AppSettings["Organizations.Page"], dr["seo"].ToString());

            // if (dr["URL"].ToString() != "")
            // {
            //     litContent.Text += String.Format("&nbsp;&nbsp;<a href=\"{0}\" class=\"button2\" target=\"_blank\">View Website</a>", dr["URL"].ToString()); ;
            // }


        }
        else
            this.Visible = false;
    }
}