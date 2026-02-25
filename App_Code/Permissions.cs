using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Permissions
/// </summary>
public class Permissions
{
	public Permissions()
	{
		//
		// TODO: Add constructor logic here
		//
	}

	public enum SpecialGroups { EKOMembers = 2, PrivateGroupTest = 1163 };

	static public int Get(int userid, int pageid)
	{

		if (userid == 1)
			return 4;

		string sql = "select access_level from users_groups_access where user_id=" + userid + " and group_id=(select group_id from pages_group where page_id=" + pageid + ")";

		if(pageid == 11225) //ekoss2025-ondemand
		{
            sql += " select access_level from users_groups_access where user_id=" + userid + " and group_id in (" + 
				Convert.ToString((int)SpecialGroups.EKOMembers) + "," + Convert.ToString((int)SpecialGroups.PrivateGroupTest) + ")";	 // PrivateGroupTest, EKOMembers		
        }

        SqlDataAdapter dapt = new SqlDataAdapter(sql,ConfigurationManager.AppSettings["CMServer"]);
		DataSet ds = new DataSet();
		dapt.Fill(ds);

		if(ds.Tables[0].Rows.Count > 0)
		{
			return Convert.ToInt32(ds.Tables[0].Rows[0][0]);
		}
		else if(pageid == 11225 && ds.Tables.Count > 1) //ekoss2025-ondemand
        {
            return Convert.ToInt32(ds.Tables[1].Rows[0][0]);
        }

        return 0;
	}

	static public int Get(int userid)
	{
		SqlDataAdapter dapt = new SqlDataAdapter("select max(access_level) from users_groups_access where user_id=" + userid + " group by user_id", ConfigurationManager.AppSettings["CMServer"]);
		DataSet ds = new DataSet();
		dapt.Fill(ds);

		if (ds.Tables[0].Rows.Count > 0)
		{
			return Convert.ToInt32(ds.Tables[0].Rows[0][0]);
		}

		return 0;
	}

	static public bool ManageArea(int userid)
    {
        SqlDataAdapter dapt = new SqlDataAdapter("select ManageArea from users where id=" + userid, ConfigurationManager.AppSettings["CMServer"]);
        DataSet ds = new DataSet();
        dapt.Fill(ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            return Convert.ToBoolean(ds.Tables[0].Rows[0][0]);
        }

        return false;
    }
    static public bool SeeMenu(int userid, int menuitem)
    {
        SqlDataAdapter dapt = new SqlDataAdapter("select * from menu_group where menuitem_id=" + menuitem + " and group_id in (select group_id from users_groups_access where access_level>0 and user_id=" + userid + ")", ConfigurationManager.AppSettings["CMServer"]);
        DataSet ds = new DataSet();
        dapt.Fill(ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            return true;
        }

        return false;
    }
}
