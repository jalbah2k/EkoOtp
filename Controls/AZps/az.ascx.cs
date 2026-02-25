using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class controls_AZ_az : System.Web.UI.UserControl
{
    public controls_AZ_az()
    {
    }

    public controls_AZ_az(string a)
    {
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SqlDataAdapter dapt = new SqlDataAdapter("AZPSgetall", ConfigurationManager.AppSettings["CMServer"]);
        DataSet ds = new DataSet();
        dapt.Fill(ds);

        List<string> cats = new List<string>();

        foreach (DataRow dr in ds.Tables[1].Rows)
        {
            cats.Add(dr["name"].ToString());
        }

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Literal temp = new Literal();

            temp.Text = "<div class=\"header\" style=\"text-transform:uppercase;padding-top:20px;\"><a id=\"az" + dr["letter"].ToString().ToLower() + "\">";
            pnlAZ.Controls.Add(temp);
            temp = new Literal();
            temp.Text = dr["letter"].ToString();
            pnlAZ.Controls.Add(temp);
            temp = new Literal();
            temp.Text = "</a></div>";
            pnlAZ.Controls.Add(temp);

            foreach (string s in cats)
            {
                if (s.ToLower().StartsWith(dr["letter"].ToString().ToLower()))
                {
                    temp = new Literal();
                    temp.Text = "<div class=\"title\" style=\"text-transform:capitalize;padding-top:10px;padding-bottom:5px;padding-left:15px;\">";
                    pnlAZ.Controls.Add(temp);
                    temp = new Literal();
                    temp.Text = s;
                    pnlAZ.Controls.Add(temp);
                    temp = new Literal();
                    temp.Text = "</div>";
                    pnlAZ.Controls.Add(temp);

                    foreach (DataRow dr2 in ds.Tables[2].Rows)
                    {
                        if (dr2["catname"].ToString() == s)
                        {
                            temp = new Literal();
                            temp.Text = "<div style=\"padding-left:30px;\"><a href=\"" + dr2["url"].ToString() + "\"><span style=\"font-family:arial;font-size:12px;color:#2790d4;text-decoration:none;\">" + dr2["name"].ToString() + "</span></a></div>";
                            pnlAZ.Controls.Add(temp);
                        }
                    }

                }
            }
        }
    }
}
