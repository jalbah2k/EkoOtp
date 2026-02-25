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
    public string param;

    public controls_AZ_az()
    {
    }

    public controls_AZ_az(string a)
    {
        param = a;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SqlDataAdapter dapt = new SqlDataAdapter("R2getall", ConfigurationManager.AppSettings["CMServer"]);
        dapt.SelectCommand.CommandType = CommandType.StoredProcedure;
        dapt.SelectCommand.Parameters.AddWithValue("@id", 3);
        DataSet ds = new DataSet();
        dapt.Fill(ds);



		string[] az = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

		DataTable dtAZ = new DataTable();
		dtAZ.Columns.Add("letter");
		dtAZ.Columns.Add("enabled");
		foreach (string l in az)
		{
			DataRow r = dtAZ.NewRow();
			r["letter"] = l;

			bool enable = false;
			foreach (DataRow r2 in ds.Tables[0].Rows)
			{
				if (r2["letter"].ToString() == l)
					enable = true;
			}

			if (enable)
				r["enabled"] = "1";
			else
				r["enabled"] = "0";


			dtAZ.Rows.Add(r);
		}

		repAZlinks.DataSource = dtAZ;
		repAZlinks.DataBind();






        List<string> cats = new List<string>();

        foreach (DataRow dr in ds.Tables[1].Rows)
        {
            cats.Add(dr["name"].ToString());
        }

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Literal temp = new Literal();

            temp.Text = "<div class=\"header\" style=\"text-transform:uppercase;padding-top:20px;width:600px;\"><a name=\"" + dr["letter"].ToString().ToUpper() + "\" id=\"az" + dr["letter"].ToString().ToLower() + "\"><div style=\"float:right;font-size:15px;\"><a href=\"#TOP\">Back to Top</a></div>";
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
							temp.Text = "<div style=\"padding-left:30px;\"><a href=\"" + dr2["url"].ToString() + "\" target=\"" + dr2["target"].ToString() + "\"><span style=\"font-family:arial;font-size:12px;color:#2790d4;text-decoration:none;\">" + dr2["name"].ToString() + "</span></a>";//</div>";
                            if (dr2["description"].ToString().Trim() != "")
                            {
                                temp.Text += "<div style=\"position:relative;left:15px;color:#555555;font-size:11px;font-family:arial;\">" + dr2["description"].ToString().Trim() + "<br /><br /></div>";
                            }
                            temp.Text += "</div>";
                            pnlAZ.Controls.Add(temp);
                        }
                    }

                }


                
            }
        }

        /*
        SqlDataAdapter dapt = new SqlDataAdapter("ResourcesList", ConfigurationManager.AppSettings["CMServer"]);
        dapt.SelectCommand.CommandType = CommandType.StoredProcedure;
        dapt.SelectCommand.Parameters.AddWithValue("@id", param);
        DataSet ds = new DataSet();
        dapt.Fill(ds);

        List<string> cats = new List<string>();

       // foreach (DataRow dr in ds.Tables[1].Rows)
       // {
       //     cats.Add(dr["name"].ToString());
        //}

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

            foreach (DataRow dr2 in ds.Tables[1].Rows)
            {
                if (dr2["name"].ToString().StartsWith(dr["letter"].ToString()))
                {
                    temp = new Literal();
                    temp.Text = "<div style=\"padding-left:30px;\"><a href=\"" + dr2["url"].ToString() + "\"><span style=\"font-family:arial;font-size:12px;color:#2790d4;text-decoration:none;\">" + dr2["name"].ToString() + "</span></a></div>";
                    pnlAZ.Controls.Add(temp);
                }
                   
            }
        }
         * */
    }
}
