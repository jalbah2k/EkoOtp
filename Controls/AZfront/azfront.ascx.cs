using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class controls_AZfront_azfront : System.Web.UI.UserControl
{
    public controls_AZfront_azfront()
    {
    }

    public controls_AZfront_azfront(string a)
    {
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        string javahead = "<script type=\"text/javascript\">var lang = \"" + (Session["Language"].ToString() == "1" ? "en" : "fr") + "\";</script>";
        litJavahead.Text = javahead;


        string java = "<script type=\"text/javascript\">var dletter;";


        SqlDataAdapter dapt = new SqlDataAdapter("AZHBgetall", ConfigurationManager.AppSettings["CMServer"]);
        dapt.SelectCommand.CommandType = CommandType.StoredProcedure;
        dapt.SelectCommand.Parameters.AddWithValue("@lang", Convert.ToInt32(Session["Language"].ToString()));
        DataSet ds = new DataSet();
        dapt.Fill(ds);

        string alpha = "abcdefghijklmnopqrstuvwxyz";
        string temp = "";

        foreach(DataRow dr in ds.Tables[5].Rows)
        {
            //temp += dr["letter"].ToString();
            java += "dletter = document.getElementById(\"" + dr["alphabet"].ToString().ToLower() + "\");dletter.src = \"/images/cheo2/letters/" + dr["alphabet"].ToString().ToLower() + "disabled.png\";dletter.className = \"\";";
        }

       // foreach(Char c in alpha)
       // {
       //     if (!temp.Contains(c.ToString()))
       //     {
               
         //   }
       // }
            

        java += "</script>";

        litJava.Text = java;

    }
}

