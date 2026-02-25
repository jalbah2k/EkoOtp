using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

public partial class Controls_Departments_Departments : System.Web.UI.UserControl
{
	public Controls_Departments_Departments()
	{
		
	}

	public Controls_Departments_Departments(string a)
	{
		
	}

    protected void Page_Load(object sender, EventArgs e)
    {
		if (!IsPostBack)
		{
            //string command = "select * from department";
            string command = "select d.*, z.letter as firstletter, z.[group] as commonletter from department d inner join AZIndex z on z.letter = substring(dept_name_en,1,1) where hide_en = 0";

            if (Session["Language"].ToString() != "1")
                command = "select d.*, z.letter as firstletter, z.[group] as commonletter from department d inner join AZIndex z on z.letter = substring(dept_name_fr,1,1) where hide_fr = 0";

            string header = string.Empty;
			string suffix = "en";

			if(Session["Language"].ToString()!="1")
			{
				suffix = "fr";
			}

            if(Request.QueryString["mode"]!=null)
			{
				if(Request.QueryString["mode"].Trim()=="a")
				{
					if (Session["Language"].ToString() != "1")
					{
						header += " administratifs";
					}
					else
					{
						header += " - Administrative";
					}
					command += " where group_depart=1";
				}
				else if(Request.QueryString["mode"].Trim()=="c")
				{
					if (Session["Language"].ToString() != "1")
					{
						header += " cliniques";
					}
					else
					{
						header += " - Clinical";
					}
					command += " where group_depart=2";
				}
				else if(Request.QueryString["mode"].Trim()=="o")
				{
					header += " - Other";
					command += " where group_depart=0";
				}
			}

            litHeader.Text = header + (header.Length > 0 ? "<br /><br />" : "");

			if(Session["Language"].ToString()!="1")
			{
				command += " order by dept_name_fr";
			}
			else
			{
				command += " order by dept_name_en";
			}

           // Response.Write(command);

			DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]))
            {
                SqlDataAdapter dapt = new SqlDataAdapter(command, conn);
                dapt.Fill(ds);
            }

            #region Letters
            string alphabet = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            string[] letters = alphabet.Split(',');
            
            Dictionary<string, List<string>> AZList = new Dictionary<string, List<string>>();

            foreach (string l in letters)
            {
                AZList.Add(l, new List<string>());
            }

            string FirstLetter = string.Empty;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
              //  FirstLetter = dr["dept_name_" + suffix + ""].ToString().Substring(0, 1).ToUpper();
                FirstLetter = dr["commonletter"].ToString().Trim();
                if (!Convert.ToBoolean(dr["Is_Active"]))
                    AZList[FirstLetter].Add(dr["dept_name_" + suffix + ""].ToString());
                else
                {
                    try
                    {
                        ////AZList[FirstLetter].Add("<a href=\"/" + suffix + "/" + dr["dept_seo_" + suffix + ""].ToString().Replace(" ", "") + "\">" + dr["dept_name_" + suffix + ""].ToString() + "</a>");
                        AZList[FirstLetter].Add("<a href=\"/" + dr["dept_seo_" + suffix + ""].ToString().Replace(" ", "") + "\">" + dr["dept_name_" + suffix + ""].ToString() + "</a>");
                    }
                    catch
                    {
                        Response.Write(FirstLetter + "<br />");
                    }
                }
            }
            #endregion Letters

            //Build Glossary
            StringBuilder glossary = new StringBuilder();
            glossary.Append("<div id=\"servicesGlossary\">");
            foreach (string l in letters)
            {
                if (AZList[l].Count > 0)
                    glossary.Append(string.Format("<a href=\"#az{0}\" ><h2>{0}</h2></a>", l));
                else
                    glossary.Append(string.Format("<h2 class=\"inactive\" aria-disabled=\"true\">{0}</h2>", l));
                glossary.Append("&nbsp;&nbsp;&nbsp;");
            }
            glossary.Append("</div>");

            Literal litGlossary = new Literal();
            litGlossary.Text = glossary.ToString();
            pnlGrids.Controls.Add(litGlossary);

            Literal openDiv = new Literal();
            openDiv.Text = "<div id=\"servicesWrapper\">";
            pnlGrids.Controls.Add(openDiv);

			/*string room = "Room Number";
			if(Session["Language"].ToString()!="1")
			{
				room = "Salle";
			}

			string phone = "Phone Number";
			if(Session["Language"].ToString()!="1")
			{
				phone = "Téléphone";
			}*/

            //string itemWrapperTemplateOpen = "<div class=\"serviceitem\"><h2>{0}</h2><ul>";
            string itemWrapperTemplateOpen = "<div class=\"serviceitem\"><a name=\"az{0}\"></a><h2>{0}</h2><ul>";
            string itemWrapperTemplateClose = "</ul></div>";
            string itemTemplate = "<li>{0}</li>";

            foreach (string l in letters)
            {
                if (AZList[l].Count > 0)
                {
                    Literal lA = new Literal();
                    lA.Text = string.Format(itemWrapperTemplateOpen, l);
                    pnlGrids.Controls.Add(lA);

                    foreach (string name in AZList[l])
                    {
                        Literal temp = new Literal();
                        temp.Text = string.Format(itemTemplate, name);
                        pnlGrids.Controls.Add(temp);
                    }

                    Literal lA2 = new Literal();
                    lA2.Text = itemWrapperTemplateClose;
                    pnlGrids.Controls.Add(lA2);
                }
            }

            // Closes servicesWrapper
            Literal closeDiv = new Literal();
            closeDiv.Text = "</div><br class=\"\" />";
            pnlGrids.Controls.Add(closeDiv);

		}


    }
}
