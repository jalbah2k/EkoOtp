using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ASB;

public partial class asb_includes_GetAutoSuggestData : System.Web.UI.Page
{
    protected void Page_Load(object sender, System.EventArgs e)
	{
		if(!IsPostBack)
			bind();
	}

	private void bind()
	{
		string sKeyword		=Request.QueryString["Keyword"];
		string sTextBoxID	=Request.QueryString["TextBoxID"];
		string sDivID		=Request.QueryString["DivID"];

		ArrayList aMenuItems=LoadSearches(sKeyword);
						
		ASBMenuItem oMenuItem;
		string sHtml;

		for (int nCount=0; nCount < aMenuItems.Count; nCount++)
		{
			oMenuItem=(ASBMenuItem)aMenuItems[nCount];
			sHtml=oMenuItem.GenHtml(nCount+1, sTextBoxID, sDivID);

			Response.Write(sHtml + "\n\r");
		}
	}

	private ArrayList LoadSearches(string sKeyword)
	{
		ArrayList aOut=new ArrayList();

		SqlConnection oCn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
		//string sSql = "select top 10 name from Searches where name like '" + sKeyword.Replace("'", "''") + "%' and count>2 order by name";
        string sSql = "select top 10 name from Searches where name like @keyword + '%' and count>2 order by name";
        SqlDataAdapter dapt = new SqlDataAdapter(sSql,oCn);
        dapt.SelectCommand.Parameters.AddWithValue("@keyword", sKeyword);

        DataSet ds = new DataSet();
		dapt.Fill(ds);

		ASBMenuItem oMenuItem;

		foreach (DataRow dr in ds.Tables[0].Rows)
		{
			oMenuItem = new ASBMenuItem();
			oMenuItem.Label = dr[0].ToString();
			aOut.Add(oMenuItem);
		}
		
		return aOut;
	}
}
