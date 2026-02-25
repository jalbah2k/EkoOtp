using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
    using System.Text;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;

public partial class Admin_Template_PageWizard : System.Web.UI.UserControl
{

    #region Properties
    private bool isMultilingual
    {
        get
        {
            return ViewState["Multilingual"] != null ? (bool)ViewState["Multilingual"] : false;
        }
        set
        {
            ViewState["Multilingual"] = value;
        }
    }
/*
    private string SeoPrefixEN
    {
        get
        {
            return ViewState["SeoPrefixEN"].ToString();
        }
        set
        {
            ViewState["SeoPrefixEN"] = value;
        }
    }*/

    private string SeoPrefixFR
    {
        get
        {
            return ViewState["SeoPrefixFR"].ToString();
        }
        set
        {
            ViewState["SeoPrefixFR"] = value;
        }
    }
    #endregion

    #region DAL

    StringBuilder sb;


    #region Pages

    public DataTable mGet_All_Page()
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = "select * from Pages order by name";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds, "table1");
        }

        return ds.Tables[0];
    }

    public DataTable mGet_All_Department()
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = "select * from department order by dept_name_en";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds, "table1");
        }

        return ds.Tables[0];
    }


    public DataTable mGet_All_Facility()
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = "select * from Facility order by facility_name_en";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds, "table1");
        }

        return ds.Tables[0];
    }


    public DataTable mGet_All_JGH_Program()
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = "select * from JGH_Program order by jgh_name_en";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds, "table1");
        }

        return ds.Tables[0];
    }
    public int mCheck_SeoName(string seo, string lang_id)
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = "select * from Pages where language = @lang_id and seo = @seo";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@seo", seo);
            cmd.Parameters.AddWithValue("@lang_id", lang_id);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds, "table1");
        }

        return ds.Tables[0].Rows.Count;
    }

    public int mCheck_SeoName_Edit(string seo, string lang_id,string id)
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = "select * from Pages where language = @lang_id and seo = @seo and id <> @id";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@seo", seo);
            cmd.Parameters.AddWithValue("@lang_id", lang_id);
            cmd.Parameters.AddWithValue("@id", id);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds, "table1");
        }

        return ds.Tables[0].Rows.Count;
    }



    public DataTable mGet_All_Page_Grid()
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = "select p.*,lay.name as 'layout_name',l.name as 'Language_Name' from pages p, Languages l,layouts lay where p.layout = lay.id and p.language = l.id order by p.name";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds, "table1");
        }

        return ds.Tables[0];
    }


    public DataTable mGet_One_Page_Linkid(int mLinkid)
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = "select * from Pages where linkid = @mLinkid and id <> @mLinkid";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@mLinkid", mLinkid);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds, "table1");
        }

        return ds.Tables[0];



    }

    public DataTable mGet_One_Depart(int mID)
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = "select * from Department where id = @mID";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@mID", mID);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds, "table1");
        }

        return ds.Tables[0];



    }

    public DataTable mGet_One_Facility(int mID)
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = "select * from Facility where id = @mID";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@mID", mID);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds, "table1");
        }

        return ds.Tables[0];



    }
    public DataTable mGet_One_JGH_Program(int mID)
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = "select * from JGH_Program where id = @mID";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@mID", mID);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds, "table1");
        }

        return ds.Tables[0];



    }



    public DataTable mGet_One_Page(int mID)
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = "select * from Pages where id = @mID";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@mID", mID);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds, "table1");
        }

        return ds.Tables[0];



    }

    public int mAdd_Page(string name, string layout, string language, string title, string keywords, string description, string seo, string linkid)
    {

        sb = sb.Remove(0, sb.Length);
        sb.Append(" insert into Pages  (name, layout, language, title, keywords, description, seo, linkid, Active) ");
        sb.Append(" values   (@name, @layout, @language, @title, @keywords, @description, @seo, @linkid, @Active)");
        sb.Append(" SELECT id FROM Pages WHERE (id = SCOPE_IDENTITY()) ");
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@layout", layout);
            cmd.Parameters.AddWithValue("@language", language);
            cmd.Parameters.AddWithValue("@title", title);
            cmd.Parameters.AddWithValue("@keywords", keywords);
            cmd.Parameters.AddWithValue("@description", description);
            cmd.Parameters.AddWithValue("@seo", seo.Replace("&",""));
            cmd.Parameters.AddWithValue("@linkid", linkid);
            cmd.Parameters.AddWithValue("@Active", "0");
            



            connection.Open();
            int i = Convert.ToInt32(cmd.ExecuteScalar());
			connection.Close();
            return i;

        }

    }

    public int mAdd_Page(string name, string layout, string language, string title, string keywords, string description, string seo, string linkid, string startdate, string enddate, string starttime, string endtime, string reviewer, string frequency)
    {

        sb = sb.Remove(0, sb.Length);
        sb.Append(" insert into Pages  (name, layout, language, title, keywords, description, seo, linkid, Active, StartDatePage, EndDatePage, StartTimePage, EndTimePage, Reviewer, ReviewFrequency, NextReviewDate) ");
        sb.Append(" values   (@name, @layout, @language, @title, @keywords, @description, @seo, @linkid, @Active, @StartDate, @EndDate, @StartTime, @EndTime, @Reviewer, @ReviewFrequency, dbo.fnGetNewReviewDate(@ReviewFrequency, null))");
        sb.Append(" SELECT id FROM Pages WHERE (id = SCOPE_IDENTITY()) ");
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@layout", layout);
            cmd.Parameters.AddWithValue("@language", language);
            cmd.Parameters.AddWithValue("@title", title);
            cmd.Parameters.AddWithValue("@keywords", keywords);
            cmd.Parameters.AddWithValue("@description", description);
            cmd.Parameters.AddWithValue("@seo", seo.Replace("&", ""));
            cmd.Parameters.AddWithValue("@linkid", linkid);
            cmd.Parameters.AddWithValue("@Active", "0");


            if (!string.IsNullOrEmpty(startdate.Trim()))
                cmd.Parameters.AddWithValue("@StartDate", startdate.Trim());
            else
                cmd.Parameters.AddWithValue("@StartDate", DBNull.Value);

            if (!string.IsNullOrEmpty(enddate.Trim()))
                cmd.Parameters.AddWithValue("@EndDate", enddate.Trim());
            else
                cmd.Parameters.AddWithValue("@EndDate", DBNull.Value);


            if (starttime.Trim() != "")
                cmd.Parameters.AddWithValue("@StartTime", starttime.Trim());
            else
                cmd.Parameters.AddWithValue("@StartTime", DBNull.Value);

            if (endtime.Trim() != "")
                cmd.Parameters.AddWithValue("@EndTime", endtime.Trim());
            else
                cmd.Parameters.AddWithValue("@EndTime", DBNull.Value);


            if (reviewer != "")
                cmd.Parameters.AddWithValue("@Reviewer", reviewer);
            else
                cmd.Parameters.AddWithValue("@Reviewer", DBNull.Value);

            if (frequency != "")
                cmd.Parameters.AddWithValue("@ReviewFrequency", frequency);
            else
                cmd.Parameters.AddWithValue("@ReviewFrequency", DBNull.Value);


            connection.Open();
            int i = Convert.ToInt32(cmd.ExecuteScalar());
            connection.Close();
            return i;

        }

    }

    public int mAdd_Group(string name, string description, string color)
    {

        sb = sb.Remove(0, sb.Length);
        sb.Append(" insert into Groups  (name, description, color) ");
        sb.Append(" values   (@name, @description, @color)");
        sb.Append(" SELECT id FROM Groups WHERE (id = SCOPE_IDENTITY()) ");
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@description", description);
            cmd.Parameters.AddWithValue("@color", color);



            connection.Open();

            int i = Convert.ToInt32(cmd.ExecuteScalar());
            string com = "insert into users_groups_access(user_id,group_id,access_level) values(1," + i + ",3)";
            //////string com = "insert into users_groups_access(user_id,group_id,access_level) values(1," + i + ",4)";

            SqlDataAdapter dapt = new SqlDataAdapter("select id from users where getsallgroups=1", strConnectionString);
            DataTable dt = new DataTable();
            dapt.Fill(dt);

            foreach (DataRow dr in dt.Rows)
            {
                com += " insert into users_groups_access(user_id,group_id,access_level) values(" + dr["id"].ToString() + "," + i + ",3)";
                //////com += " insert into users_groups_access(user_id,group_id,access_level) values(" + dr["id"].ToString() + "," + i + ",4)";
            }

            SqlCommand cmd2 = new SqlCommand(com, connection);
            cmd2.ExecuteNonQuery();

            connection.Close();

            return i;

        }

    }

	public int mAdd_Group_Mini(string name, string description, string color)
	{

		sb = sb.Remove(0, sb.Length);
		sb.Append(" insert into Groups  (name, description, color, private) ");
		sb.Append(" values   (@name, @description, @color, @private)");
		sb.Append(" SELECT id FROM Groups WHERE (id = SCOPE_IDENTITY()) ");
		string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
		string commandString = sb.ToString();


		using (SqlConnection connection = new SqlConnection(strConnectionString))
		{
			SqlCommand cmd = new SqlCommand(commandString, connection);
			cmd.Parameters.AddWithValue("@name", name);
			cmd.Parameters.AddWithValue("@description", description);
			cmd.Parameters.AddWithValue("@color", color);
			cmd.Parameters.AddWithValue("@private", cbPrivate.Checked);



			connection.Open();

			int i = Convert.ToInt32(cmd.ExecuteScalar());

            string com = "insert into users_groups_access(user_id,group_id,access_level) values(1," + i + ",4)";
           ////// string com = "insert into users_groups_access(user_id,group_id,access_level) values(1," + i + ",5)";

			SqlDataAdapter dapt = new SqlDataAdapter("select id from users where getsallgroups=1 select id from users where id in (select user_id from users_groups_access where group_id=174)", strConnectionString);                                                                  //??????????????    174 ?????????????????
			DataSet dt = new DataSet();
			dapt.Fill(dt);

			foreach (DataRow dr in dt.Tables[0].Rows)
			{
                com += " insert into users_groups_access(user_id,group_id,access_level) values(" + dr["id"].ToString() + "," + i + ",3)";
                //////com += " insert into users_groups_access(user_id,group_id,access_level) values(" + dr["id"].ToString() + "," + i + ",4)";
			}

			foreach (DataRow dr in dt.Tables[1].Rows)
			{
				com += " insert into users_groups_access(user_id,group_id,access_level) values(" + dr["id"].ToString() + "," + i + ",2)";
			}

			//Session["azlayout"]

			SqlCommand cmd2 = new SqlCommand(com, connection);
			cmd2.ExecuteNonQuery();

			connection.Close();

			return i;
		}

	}


    public void mAdd_Department(string dept_name_en, string dept_name_fr, string dept_seo_en, string dept_seo_fr, string group, string Phone_Number, string Room_Number, string Is_Active, string Is_Featured)
    {

        sb = sb.Remove(0, sb.Length);
        sb.Append(" insert into Department  (dept_name_en, dept_name_fr, dept_seo_en, dept_seo_fr, group_depart, Phone_Number, Room_Number, Is_Active, Is_Featured) ");
        sb.Append(" values   (@dept_name_en, @dept_name_fr, @dept_seo_en, @dept_seo_fr, @group, @Phone_Number, @Room_Number, @Is_Active, @Is_Featured)");
        //sb.Append(" SELECT id FROM Pages WHERE (id = SCOPE_IDENTITY()) ");
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@dept_name_en", dept_name_en);
            cmd.Parameters.AddWithValue("@dept_name_fr", dept_name_fr);
            cmd.Parameters.AddWithValue("@dept_seo_en", dept_seo_en.Replace(" ",""));
            cmd.Parameters.AddWithValue("@dept_seo_fr", dept_seo_fr.Replace(" ",""));
			cmd.Parameters.AddWithValue("@group", group);
            cmd.Parameters.AddWithValue("@Phone_Number", Phone_Number);
            cmd.Parameters.AddWithValue("@Room_Number", Room_Number);
            cmd.Parameters.AddWithValue("@Is_Active", Is_Active);
            cmd.Parameters.AddWithValue("@Is_Featured", Is_Featured);

			/*

            //if (ddlSite.Items[0].Selected)
            cmd.Parameters.AddWithValue("@sitea", cblSites.Items[0].Selected);
            //if (ddlSite.Items[1].Selected)
            cmd.Parameters.AddWithValue("@sitec", cblSites.Items[1].Selected);
            //if (ddlSite.Items[2].Selected)
            cmd.Parameters.AddWithValue("@sitef", cblSites.Items[2].Selected);
           // if (ddlSite.Items[3].Selected)
            cmd.Parameters.AddWithValue("@siteg", cblSites.Items[3].Selected);
            //if (ddlSite.Items[4].Selected)
            //cmd.Parameters.AddWithValue("@sitep", cblSites.Items[4].Selected);
            //if (ddlSite.Items[5].Selected)
            cmd.Parameters.AddWithValue("@sitew", cblSites.Items[4].Selected);

            cmd.Parameters.AddWithValue("@age", ddlAge.SelectedValue);
			*/
            connection.Open();
            //int i = Convert.ToInt32(cmd.ExecuteScalar());
            //return i;

            cmd.ExecuteNonQuery();

        }

    }


    public void mAdd_Facility(string dept_name_en, string dept_name_fr, string dept_seo_en, string dept_seo_fr, string Is_Active)
    {

        sb = sb.Remove(0, sb.Length);
        sb.Append(" insert into Facility  (Facility_name_en, Facility_name_fr, Facility_seo_en, Facility_seo_fr, Is_Active) ");
        sb.Append(" values   (@dept_name_en, @dept_name_fr, @dept_seo_en, @dept_seo_fr,@Is_Active)");
        //sb.Append(" SELECT id FROM Pages WHERE (id = SCOPE_IDENTITY()) ");
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@dept_name_en", dept_name_en);
            cmd.Parameters.AddWithValue("@dept_name_fr", dept_name_fr);
            cmd.Parameters.AddWithValue("@dept_seo_en", dept_seo_en.Replace(" ", ""));
            cmd.Parameters.AddWithValue("@dept_seo_fr", dept_seo_fr.Replace(" ", ""));
            cmd.Parameters.AddWithValue("@Is_Active", Is_Active);


            connection.Open();
            //int i = Convert.ToInt32(cmd.ExecuteScalar());
            //return i;

            cmd.ExecuteNonQuery();

        }

    }



    public void mAdd_JGH_Program(string dept_name_en, string dept_name_fr, string dept_seo_en, string dept_seo_fr, string Is_Active)
    {

        sb = sb.Remove(0, sb.Length);
        sb.Append(" insert into JGH_Program  (JGH_name_en, JGH_name_fr, JGH_seo_en, JGH_seo_fr, Is_Active) ");
        sb.Append(" values   (@dept_name_en, @dept_name_fr, @dept_seo_en, @dept_seo_fr,@Is_Active)");
        //sb.Append(" SELECT id FROM Pages WHERE (id = SCOPE_IDENTITY()) ");
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@dept_name_en", dept_name_en);
            cmd.Parameters.AddWithValue("@dept_name_fr", dept_name_fr);
            cmd.Parameters.AddWithValue("@dept_seo_en", dept_seo_en.Replace(" ", ""));
            cmd.Parameters.AddWithValue("@dept_seo_fr", dept_seo_fr.Replace(" ", ""));
            cmd.Parameters.AddWithValue("@Is_Active", Is_Active);


            connection.Open();
            //int i = Convert.ToInt32(cmd.ExecuteScalar());
            //return i;

            cmd.ExecuteNonQuery();

        }

    }


    public void mEdit_Page(string id, string name, string layout, string language, string title, string keywords, string description, string seo)
    {
        sb = sb.Remove(0, sb.Length);
        sb.Append(" update Pages ");
        sb.Append(" Set name = @name, ");
        sb.Append(" layout = @layout, ");
        sb.Append(" language = @language, ");
        sb.Append(" title = @title, ");
        sb.Append(" keywords = @keywords, ");
        sb.Append(" description = @description, ");
        sb.Append(" seo = @seo ");
        
     

        sb.Append(" where id = @id ");
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);

            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@layout", layout);
            cmd.Parameters.AddWithValue("@language", language);
            cmd.Parameters.AddWithValue("@title", title);
            cmd.Parameters.AddWithValue("@keywords", keywords);
            cmd.Parameters.AddWithValue("@description", description);
            cmd.Parameters.AddWithValue("@seo", seo);
       


            connection.Open();
            cmd.ExecuteNonQuery();
        }

    }

    public void mUpdateLinkid(string id)
    {
        sb = sb.Remove(0, sb.Length);
        sb.Append(" update Pages ");
        sb.Append(" Set linkid = @id ");


        sb.Append(" where id = @id ");
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);

            cmd.Parameters.AddWithValue("@id", id);
            
            connection.Open();
            cmd.ExecuteNonQuery();
        }

    }


    public void mDelete_Page(int mID)
    {
        sb = sb.Remove(0, sb.Length);
        sb.Append(" Delete from Pages ");
        sb.Append(" where id = @id ");

        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@id", mID);

            connection.Open();
            cmd.ExecuteNonQuery();
        }


    }

    public void mDelete_Page_ByLinkid(int mID)
    {
        sb = sb.Remove(0, sb.Length);
        sb.Append(" Delete from Pages ");
        sb.Append(" where linkid = @id ");

        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@id", mID);

            connection.Open();
            cmd.ExecuteNonQuery();
        }


    }

    #endregion



    


    #region General_Functions

    public DataTable mGet_All_Template()
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = "select * from Template order by template_name";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds, "table1");
        }

        return ds.Tables[0];
    }

    public DataTable mGet_All_Groups()
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        //        string commandString = "select * from Groups where id in (select group_id from Users_Groups_Access where User_id=" + Session["LoggedInId"].ToString() + " and access_level>1) order by name";
        string commandString = "select * from Groups where id in (select group_id from users_groups_access where user_id=" + Session["LoggedInId"].ToString() + " and access_level>1) or 1="
            + Session["LoggedInId"].ToString() + " order by name";


        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();  
            da.SelectCommand = cmd;

            da.Fill(ds, "table1");
        }

        return ds.Tables[0];
    }

    public DataTable mGet_All_Reviewers()
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        //string commandString = "select *, case when name <> '' and name is not null then username + ' (' + name + ')' else username end as fullname from users where reviewer=1 and status='active' order by name";
        string commandString = "select * from users where reviewer=1 and status='active' order by name";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds, "table1");
        }

        return ds.Tables[0];
    }

    public DataTable mGet_All_Frequencies()
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = "select * from PageReviewFrequencies order by Priority";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds, "table1");
        }

        return ds.Tables[0];
    }

    public DataTable mGet_All_Template(string id)
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = "select * from Template where id = @id";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@id", id);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds, "table1");
        }

        return ds.Tables[0];
    }

    public DataTable mGet_All_Menu_ByLangid(string lang_id)
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = "select * from Menus where language = @lang_id and id in (select menuid from MenuItems where id in (select MenuItem_id from Menu_Group where Group_id in (select group_id from Users_groups_access where access_level>1 and user_id=" + Session["LoggedInId"].ToString() + " or " + Session["LoggedInId"].ToString() + "=1 ))) order by ltrim(name)" ;
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@lang_id", lang_id);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds, "table1");
        }

        return ds.Tables[0];
    }

     public DataTable mGet_One_Menu_Linkid(int mLinkid)
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = "select * from Menus where linkid = @mLinkid and id <> @mLinkid";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@mLinkid", mLinkid);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds, "table1");
        }

        return ds.Tables[0];



    }

     public DataTable mGet_One_Menu_forFrenchid_NULL(string mPriority, string lang_id, string menuitemid)
     {
         string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
         string commandString = "select * from menuitems where parentid=0 and priority=@mPriority and menuid= (select id from menus where language=@lang_id and linkid=(select menuid from menuitems where id=@menuitemid))";
         DataSet ds = new DataSet();

         using (SqlConnection connection = new SqlConnection(strConnectionString))
         {
             SqlCommand cmd = new SqlCommand(commandString, connection);
            // cmd.Parameters.AddWithValue("@mParentid", mParentid);
             cmd.Parameters.AddWithValue("@mPriority", mPriority);
             cmd.Parameters.AddWithValue("@lang_id", lang_id);
             cmd.Parameters.AddWithValue("@menuitemid", menuitemid);
             connection.Open();
             SqlDataAdapter da = new SqlDataAdapter();
             da.SelectCommand = cmd;

             da.Fill(ds, "table1");
         }

         return ds.Tables[0];
     }


     public DataSet mGet_One_Menu_forFrenchid(string mParentid, string mPriority, string lang_id, string menuitemid)
     {
         string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
         string commandString = "select * from menuitems where parentid = @mParentid and priority=@mPriority and menuid= (select id from menus where language=@lang_id and linkid=(select menuid from menuitems where id=@menuitemid)) select id from menuitems where linkid = @mParentid and id <> linkid";
         DataSet ds = new DataSet();

         using (SqlConnection connection = new SqlConnection(strConnectionString))
         {
             SqlCommand cmd = new SqlCommand(commandString, connection);
              cmd.Parameters.AddWithValue("@mParentid", mParentid);
             cmd.Parameters.AddWithValue("@mPriority", mPriority);
             cmd.Parameters.AddWithValue("@lang_id", lang_id);
             cmd.Parameters.AddWithValue("@menuitemid", menuitemid);
             connection.Open();
             SqlDataAdapter da = new SqlDataAdapter();
             da.SelectCommand = cmd;

             da.Fill(ds, "table1");
             da.Fill(ds);
         }

         //return ds.Tables[0];
         return ds;
     }

     public DataTable mGet_One_Menuitems_Linkid(int mLinkid)
     {
         string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
         string commandString = "select * from MenuItems where linkid = @mLinkid and id <> @mLinkid";
         DataSet ds = new DataSet();

         using (SqlConnection connection = new SqlConnection(strConnectionString))
         {
             SqlCommand cmd = new SqlCommand(commandString, connection);
             cmd.Parameters.AddWithValue("@mLinkid", mLinkid);
             connection.Open();
             SqlDataAdapter da = new SqlDataAdapter();
             da.SelectCommand = cmd;

             da.Fill(ds, "table1");
         }

         return ds.Tables[0];



     }


    public DataTable mGet_Template_Contents_ByTemplateID(string id,string lang_id)
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = "select * from TemplateContents where Template_id = @id and Language_id = @lang_id";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@lang_id", lang_id);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds, "table1");
        }

        return ds.Tables[0];
    }


    public int mAdd_Menu(string name, string orientation, string language, string linkid)
    {

        sb = sb.Remove(0, sb.Length);
        sb.Append(" insert into Menus  (name, orientation, language, linkid, menucss, spacerheight) ");
        sb.Append(" values   (@name, @orientation, @language, @linkid, @menucss, @spacerheight)");
        sb.Append(" SELECT id FROM Menus WHERE (id = SCOPE_IDENTITY()) ");
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@name", name);

            cmd.Parameters.AddWithValue("@language", language);
            cmd.Parameters.AddWithValue("@linkid", linkid);
            cmd.Parameters.AddWithValue("@orientation", "Vertical");
        	cmd.Parameters.AddWithValue("@menucss", "minimenu");
        	cmd.Parameters.AddWithValue("@spacerheight", "0");
            //cmd.Parameters.AddWithValue("@width", "150");
            //cmd.Parameters.AddWithValue("@expanddelay", "0");
            //cmd.Parameters.AddWithValue("@expandduration", "150");
            //cmd.Parameters.AddWithValue("@collapsedelay", "0");
            //cmd.Parameters.AddWithValue("@collapseduration", "60");
            //cmd.Parameters.AddWithValue("@shadowenabled", "1");
            //cmd.Parameters.AddWithValue("@shadowcolor", "000000");
            //cmd.Parameters.AddWithValue("@shadowoffset", "3");
            //cmd.Parameters.AddWithValue("@language", language);
            //cmd.Parameters.AddWithValue("@linkid", linkid);

            connection.Open();
            int i =  Convert.ToInt32(cmd.ExecuteScalar());
            return i;

        }

    }


    public int mAdd_MenuItems (string menuid, string parentid, string pageid, string priority, string text, string tooltip, string navigateurl, string target, string visible, string enabled, string linkid, string language)
    {
       
        sb = sb.Remove(0, sb.Length);
        sb.Append(" insert into MenuItems  (menuid, parentid, pageid, priority, text, tooltip, navigateurl, target, visible, enabled, linkid,language) ");
        sb.Append(" values   (@menuid, @parentid, @pageid, @priority, @text, @tooltip, @navigateurl, @target, @visible, @enabled, @linkid,@language)");
        sb.Append(" SELECT id FROM MenuItems WHERE (id = SCOPE_IDENTITY()) ");
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@menuid", menuid);

            if (parentid == "" || parentid == null || parentid == "NULL")
            {
                cmd.Parameters.AddWithValue("@parentid", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@parentid", parentid);
            }

            cmd.Parameters.AddWithValue("@pageid", pageid);
            cmd.Parameters.AddWithValue("@priority", priority);
            cmd.Parameters.AddWithValue("@text", text);
            cmd.Parameters.AddWithValue("@tooltip", tooltip);
            cmd.Parameters.AddWithValue("@navigateurl", navigateurl.Replace("&",""));
            cmd.Parameters.AddWithValue("@target", target);
        	cmd.Parameters.AddWithValue("@visible", visible);
        	cmd.Parameters.AddWithValue("@enabled", enabled);
            cmd.Parameters.AddWithValue("@linkid", linkid);
            cmd.Parameters.AddWithValue("@language", language);

            connection.Open();
            int i = Convert.ToInt32(cmd.ExecuteScalar());
            return i;

        }

    }

    public int mAdd_MenuItems(string menuid, string parentid, string pageid, string priority, string text, string tooltip, string navigateurl, string target, string visible, string enabled, string linkid)
    {

        sb = sb.Remove(0, sb.Length);
        sb.Append(" insert into MenuItems  (menuid, parentid, pageid, priority, text, tooltip, navigateurl, target, visible, enabled, linkid) ");
        sb.Append(" values   (@menuid, @parentid, @pageid, @priority, @text, @tooltip, @navigateurl, @target, @visible, @enabled, @linkid)");
        sb.Append(" SELECT id FROM MenuItems WHERE (id = SCOPE_IDENTITY()) ");
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@menuid", menuid);

            if (parentid == "" || parentid == null || parentid == "NULL")
            {
                cmd.Parameters.AddWithValue("@parentid", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@parentid", parentid);
            }

            cmd.Parameters.AddWithValue("@pageid", pageid);
            cmd.Parameters.AddWithValue("@priority", priority);
            cmd.Parameters.AddWithValue("@text", text);
            cmd.Parameters.AddWithValue("@tooltip", tooltip);
            cmd.Parameters.AddWithValue("@navigateurl", navigateurl);
            cmd.Parameters.AddWithValue("@target", target);
            cmd.Parameters.AddWithValue("@visible", visible);
            cmd.Parameters.AddWithValue("@enabled", enabled);
            cmd.Parameters.AddWithValue("@linkid", linkid);

            connection.Open();
            int i = Convert.ToInt32(cmd.ExecuteScalar());
            return i;

        }

    }
    public int mAdd_Contents(string name, string control, string param, string language)
    {

        sb = sb.Remove(0, sb.Length);
        sb.Append(" insert into Content  (name, control, param, language) ");
        sb.Append(" values   (@name, @control, @param, @language)");
        sb.Append(" SELECT id FROM Content WHERE (id = SCOPE_IDENTITY()) ");
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);

            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@control", control);
            cmd.Parameters.AddWithValue("@param", param);
            cmd.Parameters.AddWithValue("@language", language);

            connection.Open();
            int i = Convert.ToInt32(cmd.ExecuteScalar());
            return i;

        }

    }


    public void mAdd_Pages_Content_Zone(string Page_ID, string Content_ID, string Zone_ID, string Priority)
    {

        sb = sb.Remove(0, sb.Length);
        sb.Append(" insert into Pages_Content_Zone  (Page_ID,Content_ID,Zone_ID,Priority) ");
        sb.Append(" values   (@Page_ID,@Content_ID,@Zone_ID,@Priority)");
        //sb.Append(" SELECT id FROM Pages WHERE (id = SCOPE_IDENTITY()) ");
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@Page_ID", Page_ID);
            cmd.Parameters.AddWithValue("@Content_ID", Content_ID);
            cmd.Parameters.AddWithValue("@Zone_ID", Zone_ID);
            cmd.Parameters.AddWithValue("@Priority", Priority);


           

            connection.Open();
            cmd.ExecuteScalar();


        }

    }

	public void mAdd_Pages_Content_Zone_Copy(string Page_ID, string Content_ID, string Zone_ID, string Priority, string name)
	{

		sb = sb.Remove(0, sb.Length);
		sb.Append(" insert into html(html,timestamp,editor)  select html,timestamp,'1' from html where id=(select param from content where id=@Content_ID) ");
		//sb.Append(" insert into content(name,control,param,language) values( (select name from content where id=@Content_ID), 'content', @@IDENTITY, (select language from content where id=@Content_ID))");
		sb.Append(" insert into content(name,control,param,language) values(@name, 'content', @@IDENTITY, (select language from content where id=@Content_ID))");
		sb.Append(" insert into Pages_Content_Zone  (Page_ID,Content_ID,Zone_ID,Priority) values(@Page_ID,@@IDENTITY,@Zone_ID,@Priority)");
		//sb.Append(" SELECT id FROM Pages WHERE (id = SCOPE_IDENTITY()) ");
		string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
		string commandString = sb.ToString();


		using (SqlConnection connection = new SqlConnection(strConnectionString))
		{
			SqlCommand cmd = new SqlCommand(commandString, connection);
			cmd.Parameters.AddWithValue("@Page_ID", Page_ID);
			cmd.Parameters.AddWithValue("@Content_ID", Content_ID);
			cmd.Parameters.AddWithValue("@Zone_ID", Zone_ID);
			cmd.Parameters.AddWithValue("@Priority", Priority);

			cmd.Parameters.AddWithValue("@name", name);




			connection.Open();
			cmd.ExecuteScalar();


		}

	}

    public void mUpdate_Priority(string id, string priority)
    {
        sb = sb.Remove(0, sb.Length);
        sb.Append(" update MenuItems ");
        sb.Append(" Set priority = @priority ");
   
        sb.Append(" where id = @id ");
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);

            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@priority", priority);
            
            connection.Open();
            cmd.ExecuteNonQuery();
        }

    }

    public void mUpdate_Pages_Content_Zone(string Page_id, string Content_id, string Zone_id)
    {
        sb = sb.Remove(0, sb.Length);
        sb.Append(" update Pages_Content_Zone ");
        sb.Append(" Set Content_ID = @Content_ID ");

        sb.Append(" where Page_id = @Page_id and Zone_id = @Zone_id ");
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);

            cmd.Parameters.AddWithValue("@Page_id", Page_id);
            cmd.Parameters.AddWithValue("@Content_id", Content_id);
            cmd.Parameters.AddWithValue("@Zone_id", Zone_id);

            connection.Open();
            cmd.ExecuteNonQuery();
        }

    }

    public void mUpdate_Menu_Linkid(string id, string linkid)
    {
        sb = sb.Remove(0, sb.Length);
        sb.Append(" update Menus ");
        sb.Append(" Set linkid = @linkid ");

        sb.Append(" where id = @id ");
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);

            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@linkid", linkid);

            connection.Open();
            cmd.ExecuteNonQuery();
        }

    }

    public void mUpdate_MenuItems_Linkid(string id, string linkid)
    {
        sb = sb.Remove(0, sb.Length);
        sb.Append(" update MenuItems ");
        sb.Append(" Set linkid = @linkid ");

        sb.Append(" where id = @id ");
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);

            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@linkid", linkid);

            connection.Open();
            cmd.ExecuteNonQuery();
        }

    }


    public void mDelete_MenuItems(int mPageid)
    {
        sb = sb.Remove(0, sb.Length);
        sb.Append(" Delete from MenuItems ");
        sb.Append(" where pageid = @mPageid ");

        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@mPageid", mPageid);

            connection.Open();
            cmd.ExecuteNonQuery();
        }
    }


    public void mDelete_PagesContentZones(int mPageid)
    {
        sb = sb.Remove(0, sb.Length);
        sb.Append(" Delete from Pages_Content_Zone ");
        sb.Append(" where Page_ID = @mPageid ");

        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@mPageid", mPageid);

            connection.Open();
            cmd.ExecuteNonQuery();
        }
    }


    public void mAdd_Menu_Groups(string Menu_id, string Group_id)
    {

        sb = sb.Remove(0, sb.Length);
        sb.Append(" insert into Menu_Group  (MenuItem_id,Group_id) ");
        sb.Append(" values   (@Menu_id,@Group_id)");
        //sb.Append(" SELECT id FROM Pages WHERE (id = SCOPE_IDENTITY()) ");
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@Menu_id", Menu_id);
            cmd.Parameters.AddWithValue("@Group_id", Group_id);
    
            connection.Open();
            cmd.ExecuteScalar();


        }

    }




    public void mAdd_Page_Groups(string Page_id, string Group_id)
    {

        sb = sb.Remove(0, sb.Length);
        sb.Append(" insert into Pages_Group  (Page_id,Group_id) ");
        sb.Append(" values   (@Page_id, @Group_id)");
        //sb.Append(" SELECT id FROM Pages WHERE (id = SCOPE_IDENTITY()) ");
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@Page_id", Page_id);
            cmd.Parameters.AddWithValue("@Group_id", Group_id);

            connection.Open();
            cmd.ExecuteScalar();


        }

    }


    #endregion







    #endregion



    protected void Page_Load(object sender, EventArgs e)
    {
        sb = new StringBuilder(60);
        this.lbl_msg.Text = "";
        this.lbl_msg_step3A.Text = "";
       // this.lbl_msg_step3B.Text = "";

        if (!IsPostBack)
        {
            /*if (!CMSHelper.isMultilingual)
            {
                SeoPrefixEN = "/";
                SeoPrefixFR = "/";
            }
            else
            {
                SeoPrefixEN = "/en/";
            }*/
            SeoPrefixFR = "/fr/";

			//Response.Write(Session["Multilingual"].ToString());

			if ((bool)Session["Multilingual"])
			{
				RB_Language.Items.FindByValue("-2").Selected = true;
			}
            else
            {
                RB_Language.Items.FindByValue("1").Selected = true;

                // MAKE ALL FRENCH FIELDS DISSAPPEAR ------------------------------------------------------

                frow1.Visible = false;
                frow2.Visible = false;
                frow3.Visible = false;
                frow4.Visible = false;
                frow5.Visible = false;
                frow6.Visible = false;
                frow7.Visible = false;
                frow8.Visible = false;
                frow9.Visible = false;
                frow10.Visible = false;
                frow11.Visible = false;
                frow12.Visible = false;

                //frow13.Visible = false;
                //frow14.Visible = false;
                frow15.Visible = false;
                frow16.Visible = false;
                frow17.Visible = false;
                frow18.Visible = false;
                Tr1.Visible = false;

                frow25.Visible = false;
                frow26.Visible = false;
                frow27.Visible = false;
                //----------------------------------------------------------------------------------------------------------




            }

            //to be delete - Global variable
            //Session["user_id"] = "-1";
            //Session["lang"] = "1";


            ViewState["action"] = "add";
           

            //this.tbl_Step1.Visible = false;
            this.tbl_Step2.Visible = false;
            this.tbl_Step3.Visible = false;
            //this.tbl_Step3B.Visible = false;

            this.tbl_Step5.Visible = false;

            //this.tbl_Grid.Visible = true;


            ViewState["record_id"] = "";

            ViewState["submit"] = "0";
            Load_DDLDepart();
            Load_DDLFacility();

           // mBindData("");


            bool bMinisite = false;
            string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
            string commandString = "select * from users where id=" + Session["LoggedInId"].ToString() + " and minisite=1";
            DataSet ds = new DataSet();

            using (SqlConnection connection = new SqlConnection(strConnectionString))
            {
                SqlCommand cmd = new SqlCommand(commandString, connection);
                connection.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;

                da.Fill(ds, "table1");
                bMinisite = ds.Tables[0].Rows.Count > 0;
            }

            RB_Step0.Items[1].Enabled = RB_Step0.Items[2].Enabled = bMinisite;

            //if(Session["LoggedInId"].ToString() == "1")
            //{
            //    RB_Step0.Items[0].Enabled = true;
            //    //RB_Step0.Items[1].Enabled = true;
            //    RB_Step0.Items[1].Enabled = true;
            //    RB_Step0.Items[2].Enabled = true;
            //    //RB_Step0.Items[4].Enabled = true;
            //}

            //if (Session["LoggedInId"].ToString() == "18" || Session["LoggedInId"].ToString() == "19" || Session["LoggedInId"].ToString() == "20" || Session["LoggedInId"].ToString() == "243")
            //{
            //    RB_Step0.Items[0].Enabled = true;
            //    //RB_Step0.Items[1].Enabled = true;
            //    RB_Step0.Items[1].Enabled = true;
            //    RB_Step0.Items[2].Enabled = true;
            //    //RB_Step0.Items[4].Enabled = true;
				
            //}

		Load_DDLTemplate();  
		Load_DDLMenu();
		Load_DDLGroups();

        Load_DDLReviewers();
        Load_DDLFrequencies();
        Load_Times();

		//load intial data
			firstimeload();
		}
    }

    private void Load_DDLFrequencies()
    {
        DataTable dt = new DataTable();
        dt = mGet_All_Frequencies();
        this.ddlFrequency.DataSource = dt;
        this.ddlFrequency.DataTextField = "name";
        this.ddlFrequency.DataValueField = "id";
        this.ddlFrequency.DataBind();
        this.ddlFrequency.Items.Insert(0, "");        

    }

    private void Load_DDLReviewers()
    {
        DataTable dt = new DataTable();
        dt = mGet_All_Reviewers();
        this.ddlReviewer.DataSource = dt;
        this.ddlReviewer.DataTextField = "name";
        this.ddlReviewer.DataValueField = "id";
        this.ddlReviewer.DataBind();
        this.ddlReviewer.Items.Insert(0, "");
    }

    private void Load_Times()
    {
        DateTime time = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
        DateTime end = time.AddDays(1); ;

        ddlStartTime.Items.Clear();
        ddlEndTime.Items.Clear();

        int Counter = 0;

        ddlStartTime.Items.Add(new ListItem("Select One", ""));
        ddlEndTime.Items.Add(new ListItem("Select One", ""));

        while (time < end && Counter < 97)
        {
            ddlStartTime.Items.Add(new ListItem(time.ToString("h:mm tt"), time.ToString("HH:mm")));
            ddlEndTime.Items.Add(new ListItem(time.ToString("h:mm tt"), time.ToString("HH:mm")));

            time = time.AddMinutes(15);

            Counter++;
        }
    }

    #region My_Functions

    //private void mBindData(string sortExp)
    //{
    //    DataTable dt = new DataTable();
    //    dt = mGet_All_Page_Grid();
    //    DataView DV = dt.DefaultView;
    //    if (!(sortExp == string.Empty))
    //    {
    //        DV.Sort = sortExp;
    //    }
    //    this.GV_Main.DataSource = DV;
    //    this.GV_Main.DataBind();
    //}

    void Load_DDLDepart()
    {

        DataTable dt = new DataTable();
        
		//dt = mGet_All_Department();
    	dt = mGet_All_Page();

        this.DDL_Depart.DataSource = dt;
        
		//this.DDL_Depart.DataTextField = "dept_name_en";
    	this.DDL_Depart.DataTextField = "name";

		this.DDL_Depart.DataValueField = "id";
        this.DDL_Depart.DataBind();

        this.DDL_Depart.Items.Insert(0, new ListItem("Create New Page", "none"));
    }

    void Load_DDLFacility()
    {

        DataTable dt = new DataTable();

        //dt = mGet_All_Facility();
		dt = mGet_All_Page();

        this.DDL_Facility.DataSource = dt;
        
		
		//this.DDL_Facility.DataTextField = "Facility_name_en";
    	this.DDL_Facility.DataTextField = "name";

        this.DDL_Facility.DataValueField = "id";
        this.DDL_Facility.DataBind();

        this.DDL_Facility.Items.Insert(0, new ListItem("Create New Page", "none"));
    }


    void firstimeload()
    {
        ViewState["action"] = "add";
        ShowSaveButton();
        Reset_Step2();
        Reset_Step3();
        Reset_Step4();

        this.tbl_step_0.Visible = true;
        this.tbl_depart.Visible = false;
        this.tbl_Facility.Visible = false;

        this.tbl_Step1.Visible = false;
        this.tbl_Step2.Visible = false;
        this.tbl_Step3.Visible = false;
        this.tbl_step_before3.Visible = false;
        //this.tbl_Step3B.Visible = false;
        this.tbl_Step5.Visible = false;
        //this.tbl_Grid.Visible = false;

       
    }


    void ShowSaveButton()
    {
        //0= dont Show Save Button
        //1= Show Save Button
        if (ViewState["submit"].ToString() == "1")
        {

            //this.btn_Save_Temp1.Visible = true;
            //this.btn_Save_Temp2.Visible = true;
            //this.btn_Save_Temp3.Visible = true;
            //this.btn_Save_Temp4.Visible = true;

            //this.btn_Save_Temp5.Visible = true;
            //this.btn_Save_Temp6.Visible = true;


        }
        else
        {
            this.btn_Save_Temp1.Visible = false;
            this.btn_Save_Temp2.Visible = false;
            this.btn_Save_Temp3.Visible = false;
            ////this.btn_Save_Temp4.Visible = false;
            //this.btn_Save_Temp5.Visible = false;
            //this.btn_Save_Temp6.Visible = false;
        }

    }

    void Load_DDLTemplate()
    {
        DataTable dt = new DataTable();
        dt = mGet_All_Template();
        this.DDL_Template_Select.DataSource = dt;
        this.DDL_Template_Select.DataTextField = "Template_Name";
        this.DDL_Template_Select.DataValueField = "id";
        this.DDL_Template_Select.DataBind();

        this.DDL_Template_Select.Items.Insert(0, new ListItem("Select Template", "select"));
    }

     void Load_DDLMenu()
    {
		
        DataTable dt = new DataTable();
        dt = mGet_All_Menu_ByLangid("1");
        this.DDL_Menu.DataSource = dt;
        this.DDL_Menu.DataTextField = "name";
        this.DDL_Menu.DataValueField = "id";
        this.DDL_Menu.DataBind();

        this.DDL_Menu.Items.Insert(0, new ListItem("Select a Menu", ""));
    }

     void Load_DDLGroups()
     {

         DataTable dt = new DataTable();
         dt = mGet_All_Groups();
         this.DDL_Groups.DataSource = dt;
         this.DDL_Groups.DataTextField = "name";
         this.DDL_Groups.DataValueField = "id";
         this.DDL_Groups.DataBind();

         ListItem li = this.DDL_Groups.Items.FindByText("Common");

         if (li == null)
         {
             this.DDL_Groups.SelectedIndex = 0; 
         }
         else
         {
             this.DDL_Groups.SelectedValue = li.Value;
         }

        
        // this.DDL_Groups.Items.Insert(0, new ListItem("None", "none"));
     }

     

    void Populate_Step5()
    {
        //step1
        this.lbl_Language.Text = this.RB_Language.SelectedItem.Text;

        //step2
        this.lbl_Page_EN.Text = this.txt_Page_Name_EN.Text;
        this.lbl_Page_FR.Text = this.txt_Page_Name_FR.Text;
        this.lbl_Title_EN.Text = this.txt_Title_EN.Text;
        this.lbl_Title_FR.Text = this.txt_Title_FR.Text;
        this.lbl_Keywords_EN.Text = this.txt_Keywords_EN.Text;
        this.lbl_Keywords_FR.Text = this.txt_Keywords_FR.Text;
        this.lbl_Desc_EN.Text = this.txt_Desc_EN.Text;
        this.lbl_Desc_FR.Text = this.txt_Desc_FR.Text;
		this.lbl_SEO_EN.Text = this.txt_SEO_EN.Text.Replace("en/", "").Replace("/en/", "").Replace("/","").Replace("'","").Replace(",","");
		this.lbl_SEO_FR.Text = this.txt_SEO_FR.Text.Replace("fr/", "").Replace("/fr/", "").Replace("/", "").Replace("'", "").Replace(",", "").Replace("&","");

        //step3
        this.lbl_Menu_EN.Text = this.txt_Menu_EN.Text;
        this.lbl_Menu_FR.Text = this.txt_Menu_FR.Text;

        //step4
        this.lbl_Template.Text = this.DDL_Template_Select.SelectedItem.ToString();
        int i = this.DDL_Template_Select.SelectedIndex;

        lblStartDate.Text = txtStartDate.Text.Trim() + " " + (ddlStartTime.SelectedValue != "" ? ddlStartTime.SelectedItem.Text : "");
        lblEndDate.Text = txtEndDate.Text.Trim() + " " + (ddlEndTime.SelectedValue != "" ? ddlEndTime.SelectedItem.Text : "");

        lblReviewer.Text = ddlReviewer.SelectedItem.Text;
        lblFreq.Text = ddlFrequency.SelectedItem.Text;
    }

    void Reset_Step2()
    {
        this.txt_Page_Name_EN.Text = "";
        this.txt_Page_Name_FR.Text = "";
        this.txt_Title_EN.Text = "";
        this.txt_Title_FR.Text = "";
        this.txt_Keywords_EN.Text = "";
        this.txt_Keywords_FR.Text = "";
        this.txt_Desc_EN.Text = "";
        this.txt_Desc_FR.Text = "";
        this.txt_SEO_EN.Text = "";
        this.txt_SEO_FR.Text = "";
    }

    void Reset_Step3()

    {
        this.DDL_Menu.SelectedIndex = 0;

        //Step 3 - A
        this.txt_Menu_EN.Text = "";
        

        //this.Btn_Add_To_Menu_EN.Visible = true;
        this.tbl_Step3_EN.Visible = false;

        this.TV_Menu_EN.Nodes.Clear();


        //Step 3 - B
        //this.txt_Menu_FR.Text = "";
        //this.txt_ToolTip_FR.Text = "";
        //this.txt_Priority_FR.Text = "";
        //this.txt_URL_FR.Text = "";
      

        //this.RB_Visible_FR.SelectedIndex = 0;
        //this.RB_Visible_FR.SelectedIndex = 0;

        //this.Btn_Add_To_Menu_FR.Visible = true;
        //this.tbl_Step3_FR.Visible = false;

        //this.TV_Menu_FR.Nodes.Clear();


         ViewState["selectednode_fr"] = "";
         ViewState["selectednode_en"] = "";
         ViewState["menu_en"] = "0";
         ViewState["menu_fr"] = "0";

    }

    void Reset_Step4()
    {
        this.DDL_Template_Select.SelectedIndex = 0;
    }

    void Clear_Fields()
    {
        Reset_Step2();
        Reset_Step3();
        Reset_Step4();

        ViewState["action"] = "add";
        ViewState["sortExpression"] = "";
        ViewState["sortD"] = "ASC";
        ViewState["dt_menu_en"] = "";
        ViewState["dt_menu_fr"] = "";

        this.tbl_Step1.Visible = false;
        this.tbl_Step2.Visible = false;
        this.tbl_step_before3.Visible = false;
        this.tbl_Step3.Visible = false;
        //this.tbl_Step3B.Visible = false;
        this.tbl_Step5.Visible = false;

        //this.tbl_Grid.Visible = true;
        ViewState["record_id"] = "";
        ViewState["submit"] = "0";
        //mBindData("");

    }

    void Load_data(int id) //I DONT THINK THIS DOES ANYTHING
    {
        DataTable dt1 = new DataTable();
        

        DataTable dt_en = new DataTable();
        DataTable dt_fr = new DataTable();

        dt1 = mGet_One_Page(id);



        if (dt1.Rows[0]["id"].ToString() == dt1.Rows[0]["linkid"].ToString())
        {
            //dt1 is english

            //for english
            dt_en = dt1;

            //for french
            dt_fr = mGet_One_Page_Linkid(id);
            
        }
        else
        {
            //dt1 is french

            //french
            dt_fr = dt1;

            //english
            dt_en = mGet_One_Page(Convert.ToInt32(dt_fr.Rows[0]["linkid"].ToString()));

            
        }

        //populate step - 2

        //english
        this.txt_Page_Name_EN.Text = dt_en.Rows[0]["name"].ToString();
        this.txt_Title_EN.Text = dt_en.Rows[0]["title"].ToString();
        this.txt_Keywords_EN.Text = dt_en.Rows[0]["keywords"].ToString();
        this.txt_Desc_EN.Text = dt_en.Rows[0]["description"].ToString();
        this.txt_SEO_EN.Text = dt_en.Rows[0]["seo"].ToString();       

        //french
        this.txt_Page_Name_FR.Text = dt_fr.Rows[0]["name"].ToString();
        this.txt_Title_FR.Text = dt_fr.Rows[0]["title"].ToString();
        this.txt_Keywords_FR.Text = dt_fr.Rows[0]["keywords"].ToString();
        this.txt_Desc_FR.Text = dt_fr.Rows[0]["description"].ToString();
        this.txt_SEO_FR.Text = dt_fr.Rows[0]["seo"].ToString();

        //populate step - 3




        //populate step - 4
        this.DDL_Template_Select.SelectedValue = dt1.Rows[0]["Template_id"].ToString();
    }


    #endregion



   


    #region NavigateButtons
   
    
    protected void btn_Step1_Next_Click(object sender, EventArgs e)
    {
         ShowSaveButton();

        this.tbl_Step1.Visible = false;
        this.tbl_Step2.Visible = true;
        this.tbl_step_before3.Visible = false;
        this.tbl_Step3.Visible = false;
        //this.tbl_Step3B.Visible = false;

        this.tbl_Step5.Visible = false;

        //this.tbl_Grid.Visible = false;


		//TODO: Ronnie
    	this.tbl_Step2.Visible = false;
    	this.tbl_step_before3.Visible = true;
		//

		//if(RB_Language.SelectedValue ==
    }
    protected void btn_Step2_Next_Click(object sender, EventArgs e)
    {

        //Validation

        if (ViewState["action"].ToString() == "add")
        {
            int i;
            //for english
			i = mCheck_SeoName(this.txt_SEO_EN.Text.Trim().Replace("en/", "").Replace("/en/", "").Replace("/", "").Replace("'", "").Replace(",", "").Replace("&",""), "1");
            if (i != 0)
            {
				this.lbl_msg.Text = "ALERT: English URL has already been used.";
                return;
            }

            //for french
			i = mCheck_SeoName(this.txt_SEO_FR.Text.Trim().Replace("fr/", "").Replace("/fr/", "").Replace("/", "").Replace("'", "").Replace(",", "").Replace("&",""), "2");
            if (i != 0)
            {
				this.lbl_msg.Text = "ALERT: French Seo has already been used.";
                return;
            }

            //for url
            //this.lbl_URL_en.Text = "/en/" + this.txt_SEO_EN.Text;
            //this.lbl_URL_fr.Text = "/fr/" + this.txt_SEO_FR.Text;

        }
        else if (ViewState["action"].ToString() == "edit")
        {
            //get idz
            int mID = Convert.ToInt32(ViewState["record_id"].ToString());
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();

            dt = mGet_One_Page(mID);

            int mEnglish_id;
            int mFrench_id;


            if (dt.Rows[0]["id"].ToString() == dt.Rows[0]["linkid"].ToString())
            {
                //english
                mEnglish_id = mID;

                //to get french record id
                dt2 = mGet_One_Page_Linkid(mEnglish_id);
                mFrench_id = Convert.ToInt32(dt2.Rows[0]["id"].ToString());
            }
            else
            {
                //french
                mFrench_id = mID;

                //to get english record id
                mEnglish_id = Convert.ToInt32(dt.Rows[0]["linkid"].ToString());
            }


            int i;
            //for english
			i = mCheck_SeoName_Edit(this.txt_SEO_EN.Text.Trim().Replace("en/", "").Replace("/en/", "").Replace("/", "").Replace("'", "").Replace(",", "").Replace("&",""), "1", mEnglish_id.ToString());
            if (i != 0)
            {
                this.lbl_msg.Text = "ALERT: English URL should be unique";
                return;
            }


            //for french
			i = mCheck_SeoName_Edit(this.txt_SEO_FR.Text.Trim().Replace("fr/", "").Replace("/fr/", "").Replace("/", "").Replace("'", "").Replace(",", "").Replace("&",""), "2", mFrench_id.ToString());
            if (i != 0)
            {
                this.lbl_msg.Text = "ALERT: French Seo should be unique";
                return;
            }


        }

        
        
        ShowSaveButton();

        this.tbl_Step1.Visible = false;
        this.tbl_Step2.Visible = false;
        this.tbl_step_before3.Visible = true;
        this.tbl_Step3.Visible = false;
       // this.tbl_Step3B.Visible = false;
        this.tbl_Step5.Visible = false;

        //this.tbl_Grid.Visible = false;


		//TODO: Ronald
    	this.tbl_step_before3.Visible = false;
		//
    }
    protected void Btn_Back_Step2_Click(object sender, EventArgs e)
    {
        ShowSaveButton();

        this.tbl_Step1.Visible = false;
        this.tbl_Step2.Visible = false;
        this.tbl_step_before3.Visible = true;
        this.tbl_Step3.Visible = false;
        //this.tbl_Step3B.Visible = false;
        this.tbl_Step5.Visible = false;

       // this.tbl_Grid.Visible = false;
    }
   
    protected void Btn_Back_Step3_Click(object sender, EventArgs e)
    {
        ShowSaveButton();

        this.tbl_Step1.Visible = false;
        this.tbl_Step2.Visible = false;
        this.tbl_step_before3.Visible = true;
        this.tbl_Step3.Visible = false;
        //this.tbl_Step3B.Visible = false;
        this.tbl_Step5.Visible = false;

        //this.tbl_Grid.Visible = false;
    }
    protected void btn_Step3_Next_Click(object sender, EventArgs e)
    {
        //Validation
        //string s = ViewState["menu_en"].ToString();
       // if (s == "0")
       // {
       //     this.lbl_msg_step3A.Text = "Please insert in Menu";
       //     return;
       // }
        
        ShowSaveButton();

            this.tbl_Step1.Visible = false;
            this.tbl_Step2.Visible = false;
            this.tbl_step_before3.Visible = false;
            this.tbl_Step3.Visible = false;
            //this.tbl_Step3B.Visible = true;
            this.tbl_Step5.Visible = false;

            //this.tbl_Grid.Visible = false;


			//TODO:Ronald
    	this.tbl_Step2.Visible = true;
			//
    }
    protected void btn_Step4_Next_Click(object sender, EventArgs e)
    {
        Populate_Step5();
        ShowSaveButton();

        this.tbl_Step1.Visible = false;
        this.tbl_Step2.Visible = false;
        this.tbl_step_before3.Visible = false;
        this.tbl_Step3.Visible = false;
        //this.tbl_Step3B.Visible = false;
        this.tbl_Step5.Visible = true;

        //this.tbl_Grid.Visible = false;

        //if (this.chk_Step3.Checked)
        if (this.DDL_Menu.SelectedValue != "none")
        {
            this.tbl_Step5_menu.Visible = true;
        }
        else
        {
            this.tbl_Step5_menu.Visible = false;
        }
    }
    protected void Btn_Back_Step5_Click(object sender, EventArgs e)
    {
        ShowSaveButton();

        this.tbl_Step1.Visible = false;
        this.tbl_Step2.Visible = false;
        this.tbl_step_before3.Visible = false;
        this.tbl_Step3.Visible = false;
        //this.tbl_Step3B.Visible = false;
        this.tbl_Step5.Visible = false;

       // this.tbl_Grid.Visible = false;
    }

   
    protected void Btn_Back_Step3B_Click(object sender, EventArgs e)
    {
        ShowSaveButton();

        this.tbl_Step1.Visible = false;
        this.tbl_Step2.Visible = false;
        this.tbl_step_before3.Visible = false;
        this.tbl_Step3.Visible = true;
        //this.tbl_Step3B.Visible = false;
        this.tbl_Step5.Visible = false;

       // this.tbl_Grid.Visible = false;
    }

    protected void btn_Save_Temp_Click(object sender, EventArgs e)
    {
        //Validation
       //step 2
        if (ViewState["action"].ToString() == "add")
        {
            if (this.tbl_Step2.Visible)
            {
                int i;
                //for english
				i = mCheck_SeoName(this.txt_SEO_EN.Text.Trim().Replace("en/", "").Replace("/en/", "").Replace("/", "").Replace("'", "").Replace(",", "").Replace("&",""), "1");
                if (i != 0)
                {
					this.lbl_msg.Text = "ALERT: English URL has already been used.";
                    return;
                }

                //for french
				i = mCheck_SeoName(this.txt_SEO_FR.Text.Trim().Replace("fr/", "").Replace("/fr/", "").Replace("/", "").Replace("'", "").Replace(",", "").Replace("&",""), "2");
                if (i != 0)
                {
					this.lbl_msg.Text = "ALERT: French Seo has already been used.";
                    return;
                }

                //for url
                //this.lbl_URL_en.Text = "/en/" + this.txt_SEO_EN.Text;
                //this.lbl_URL_fr.Text = "/en/" + this.txt_SEO_FR.Text;
            }


            //step 3
            //Validation
            if (this.tbl_Step3.Visible )
            {
                string s = ViewState["menu_en"].ToString();
                if (s == "0")
                {
                    this.lbl_msg_step3A.Text = "Please insert in Menu";
                    return;
                }
            }

         

            //if (this.chk_Step3.Checked)
            if (this.DDL_Menu.SelectedValue != "none")
            {
                this.tbl_Step5_menu.Visible = true;
            }
            else
            {
                this.tbl_Step5_menu.Visible = false;
            }


        }
        else if (ViewState["action"].ToString() == "edit")
        {
            //get idz
            int mID = Convert.ToInt32(ViewState["record_id"].ToString());
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();

            dt = mGet_One_Page(mID);

            int mEnglish_id;
            int mFrench_id;


            if (dt.Rows[0]["id"].ToString() == dt.Rows[0]["linkid"].ToString())
            {
                //english
                mEnglish_id = mID;

                //to get french record id
                dt2 = mGet_One_Page_Linkid(mEnglish_id);
                mFrench_id = Convert.ToInt32(dt2.Rows[0]["id"].ToString());
            }
            else
            {
                //french
                mFrench_id = mID;

                //to get english record id
                mEnglish_id = Convert.ToInt32(dt.Rows[0]["linkid"].ToString());
            }


            int i;
            //for english
			i = mCheck_SeoName_Edit(this.txt_SEO_EN.Text.Trim().Replace("en/", "").Replace("/en/", "").Replace("/", "").Replace("'", "").Replace(",", "").Replace("&",""), "1", mEnglish_id.ToString());
            if (i != 0)
            {
				this.lbl_msg.Text = "ALERT: English URL has already been used.";
                return;
            }


            //for french
			i = mCheck_SeoName_Edit(this.txt_SEO_FR.Text.Trim().Replace("fr/", "").Replace("/fr/", "").Replace("/", "").Replace("'", "").Replace(",", "").Replace("&",""), "2", mFrench_id.ToString());
            if (i != 0)
            {
				this.lbl_msg.Text = "ALERT: French Seo has already been used.";
                return;
            }


            //step 3
            //Validation
            string s = ViewState["menu_en"].ToString();
            if (s == "0")
            {
                this.lbl_msg.Text = "Please insert in Menu";
                return;
            }

        }


        
        ViewState["submit"] = "0";
        ShowSaveButton();

        this.tbl_Step1.Visible = false;
        this.tbl_Step2.Visible = false;
        this.tbl_step_before3.Visible = false;
        this.tbl_Step3.Visible = false;
       // this.tbl_Step3B.Visible = false;
        this.tbl_Step5.Visible = true;
        Populate_Step5();

    }

    
   

    protected void LB_Step1_Click(object sender, EventArgs e)
    {
        
        ViewState["submit"] = "1";
        this.tbl_Step5.Visible = false;

        this.tbl_Step1.Visible = true;
        ShowSaveButton();
    }
    protected void LB_Step2_Click(object sender, EventArgs e)
    {
        ViewState["submit"] = "1";
        this.tbl_Step5.Visible = false;

        this.tbl_Step2.Visible = true;
        ShowSaveButton();

    }
    protected void LB_Step3_Click(object sender, EventArgs e)
    {
        ViewState["submit"] = "1";
        this.tbl_Step5.Visible = false;

        this.tbl_step_before3.Visible = true;
        ShowSaveButton();
    }
    protected void LB_Step4_Click(object sender, EventArgs e)
    {
        ViewState["submit"] = "1";
        this.tbl_Step5.Visible = false;

        ShowSaveButton();
    }

    protected void btn_Cancel_step1_Click(object sender, EventArgs e)
    {
        Clear_Fields();
        Response.Redirect("~/admin.aspx?c=wizard");
    }

    #endregion

    protected void btn_Submit_Click(object sender, EventArgs e)
    {
        if (ViewState["action"].ToString() == "add")
        {

            SqlConnection sqlConn2 = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);


            ////SqlDataAdapter dapt1 = new SqlDataAdapter("select id from pages where seo=@en or seo=@fr", sqlConn2);
            ////dapt1.SelectCommand.Parameters.AddWithValue("@en", this.txt_SEO_EN.Text.Trim().Replace(" ", "").Replace("/en/", "").Replace("en/", "").Replace("/", "").Replace("'", "").Replace(",", "").Replace("&", ""));
            ////dapt1.SelectCommand.Parameters.AddWithValue("@fr", this.txt_SEO_FR.Text.Trim().Replace(" ", "").Replace("/fr/", "").Replace("fr/", "").Replace("/", "").Replace("'", "").Replace(",", "").Replace("&", ""));

            ////DataSet ds1 = new DataSet();
            ////dapt1.Fill(ds1);
            ////if (ds1.Tables[0].Rows.Count > 0)
            ////{
            ////    this.Page.ClientScript.RegisterStartupScript(GetType(), "asd", "alert('A page with this URL already exists');", true);
            ////    return;
            ////}

            string sqlcmd = @"select id from pages where title=@title 
                              select id from pages where seo=@en  
                              select id from pages where isnull(description, '') <> '' and description=@description 
                            ";
            if ((bool)isMultilingual)
            {
                sqlcmd += @"select id from pages where title=@titlefr 
                            select id from pages where seo=@fr
                            select id from pages where isnull(description, '') <> '' and description=@descriptionfr
                            ";
            }

            SqlDataAdapter dapt1 = new SqlDataAdapter(sqlcmd, sqlConn2);
            dapt1.SelectCommand.Parameters.AddWithValue("@en", this.txt_SEO_EN.Text.Trim().Replace(" ", "").Replace("/en/", "").Replace("en/", "").Replace("/", "").Replace("'", "").Replace(",", "").Replace("&", ""));
            dapt1.SelectCommand.Parameters.AddWithValue("@title", txt_Title_EN.Text);
            dapt1.SelectCommand.Parameters.AddWithValue("@description", txt_Desc_EN.Text);
            if ((bool)isMultilingual)
            {
                dapt1.SelectCommand.Parameters.AddWithValue("@fr", this.txt_SEO_FR.Text.Trim().Replace(" ", "").Replace("/fr/", "").Replace("fr/", "").Replace("/", "").Replace("'", "").Replace(",", "").Replace("&", ""));
                dapt1.SelectCommand.Parameters.AddWithValue("@titlefr", txt_Title_FR.Text);
                dapt1.SelectCommand.Parameters.AddWithValue("@descriptionfr", txt_Desc_FR.Text);
            }

            DataSet ds1 = new DataSet();
            dapt1.Fill(ds1);

            bool errMsg = false;
            if ((bool)isMultilingual)
                errMsg = ds1.Tables[0].Rows.Count > 0 || ds1.Tables[1].Rows.Count > 0 || ds1.Tables[2].Rows.Count > 0 || ds1.Tables[3].Rows.Count > 0 || ds1.Tables[4].Rows.Count > 0 || ds1.Tables[5].Rows.Count > 0;
            else
                errMsg = ds1.Tables[0].Rows.Count > 0 || ds1.Tables[1].Rows.Count > 0 || ds1.Tables[2].Rows.Count > 0;

            if (errMsg)
            {
                string mssg = "";
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    mssg += "A page with the field Title already exists. " + "\\r\\n";
                }
                if ((bool)isMultilingual)
                {
                    if (ds1.Tables[3].Rows.Count > 0)
                        mssg += "A page with the French Title already exists. " + "\\r\\n";
                }
                if (ds1.Tables[1].Rows.Count > 0)
                    mssg += "A page with the English URL already exists. " + "\\r\\n";
                if ((bool)isMultilingual)
                {
                    if (ds1.Tables[4].Rows.Count > 0)
                        mssg += "A page with the French URL already exists. " + "\\r\\n";
                }
                if (ds1.Tables[2].Rows.Count > 0)
                    mssg += "A page with the English description already exists. " + "\\r\\n";
                if ((bool)isMultilingual)
                {
                    if (ds1.Tables[5].Rows.Count > 0)
                        mssg += "A page with the French description already exists. " + "\\r\\n";
                }

                this.Page.ClientScript.RegisterStartupScript(GetType(), "asd", "alert('" + mssg + "');", true);
                return;
            }



            //for getting layout
            DataTable mdt = new DataTable();
            mdt = mGet_All_Template(this.DDL_Template_Select.SelectedValue.ToString());


            /////////////////////Insert in pages///////////////////////
            //for english
            int i_en = mAdd_Page(this.txt_Page_Name_EN.Text, mdt.Rows[0]["layout_id"].ToString(), "1", this.txt_Title_EN.Text, this.txt_Keywords_EN.Text, this.txt_Desc_EN.Text, this.txt_SEO_EN.Text.Replace("en/", "").Replace("/en/", "").Replace("/", "").Replace("'", "").Replace(",", "").Replace("&", ""), "-1", txtStartDate.Text.Trim(), txtEndDate.Text.Trim(), ddlStartTime.SelectedValue, ddlEndTime.SelectedValue,  ddlReviewer.SelectedValue, ddlFrequency.SelectedValue);

            //Update link id
            mUpdateLinkid(i_en.ToString());

            //for French
			 int i_fr = 0;

			if((bool)isMultilingual)
                i_fr = mAdd_Page(this.txt_Page_Name_FR.Text, mdt.Rows[0]["layout_id"].ToString(), "2", this.txt_Title_FR.Text, this.txt_Keywords_FR.Text, this.txt_Desc_FR.Text, this.txt_SEO_FR.Text.Replace("fr/", "").Replace("/fr/", "").Replace("/", "").Replace("'", "").Replace(",", "").Replace("&", ""), i_en.ToString(), txtStartDate.Text.Trim(), txtEndDate.Text.Trim(), ddlStartTime.SelectedValue, ddlEndTime.SelectedValue, ddlReviewer.SelectedValue, ddlFrequency.SelectedValue);

            /////////////////////Insert in Menus///////////////////////
            //for Menu

            //if (this.chk_Step3.Checked)
            //{
			if (this.DDL_Menu.SelectedValue != "none" && ViewState["menu_en"].ToString() != "0")
            {
                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();

                dt1 = Get_Datatable_NotEmpty("1", this.DDL_Menu.SelectedValue.ToString());
                dt2 = Get_Datatable_NotEmpty("2", this.DDL_Menu.SelectedValue.ToString());


                //for English          

                DataTable dt_en = new DataTable();
                dt_en = (DataTable)ViewState["dt_menu_en"];

                foreach (DataRow mRows in dt_en.Rows)
                {
                    if (mRows["id"].ToString() == "99999991")
                    {
                        //insert

                        //for english
                        //int id_Menu_en = mAdd_MenuItems(mRows["menuid"].ToString(), mRows["parentid"].ToString(), i_en.ToString(), mRows["priority"].ToString(), mRows["text"].ToString(), mRows["tooltip"].ToString(), "/en/" + this.txt_SEO_EN.Text.Replace("en/", "").Replace("/en/", "").Replace("/", "").Replace("'", "").Replace(",", "").Replace("&","").Replace("&",""), mRows["target"].ToString(), mRows["visible"].ToString(), mRows["enabled"].ToString(), "");
                        //int id_Menu_en = mAdd_MenuItems(mRows["menuid"].ToString(), mRows["parentid"].ToString(), i_en.ToString(), mRows["priority"].ToString(), mRows["text"].ToString(), mRows["tooltip"].ToString(), "/" + this.txt_SEO_EN.Text.Replace("en/", "").Replace("/en/", "").Replace("/", "").Replace("'", "").Replace(",", "").Replace("&", "").Replace("&", ""), mRows["target"].ToString(), mRows["visible"].ToString(), mRows["enabled"].ToString(), "");
                        int id_Menu_en = mAdd_MenuItems(mRows["menuid"].ToString(), mRows["parentid"].ToString(), i_en.ToString(), mRows["priority"].ToString(), mRows["text"].ToString(), mRows["tooltip"].ToString(), CMSHelper.SeoPrefixEN + this.txt_SEO_EN.Text.Replace("en/", "").Replace("/en/", "").Replace("/", "").Replace("'", "").Replace(",", "").Replace("&", "").Replace("&", ""), mRows["target"].ToString(), mRows["visible"].ToString(), mRows["enabled"].ToString(), "","1");
                        //Update linkid
                        mUpdate_MenuItems_Linkid(id_Menu_en.ToString(), id_Menu_en.ToString());

                        //for french

						

							//get french parentid & Menuid
							int mParentid_en;
							string s = mRows["parentid"].ToString();
							string mParentid_fr = "";
							string mMenuid;

							DataTable dt4 = new DataTable();

							if (s == "" || s == null || s == "NULL")
							{
								mParentid_fr = "NULL";
							}
							else
							{

								mParentid_en = Convert.ToInt32(mRows["parentid"].ToString());

								//dt4 = mGet_One_Menuitems_Linkid(mParentid_en);
                                DataSet ds4 = mGet_One_Menu_forFrenchid(mParentid_en.ToString(), mRows["priority"].ToString(), "1", id_Menu_en.ToString());
                                dt4 = ds4.Tables[0];
                                if ((bool)isMultilingual)
                                {
                                    if (ds4.Tables[1].Rows.Count > 0)
                                        mParentid_fr = ds4.Tables[1].Rows[0]["id"].ToString();
                                    else
                                        mParentid_fr = ds4.Tables[0].Rows[0]["parentid"].ToString();

                                    ////if (dt4.Rows.Count > 0)
                                       //// mParentid_fr = dt4.Rows[0]["id"].ToString();
                                    ////else
                                    ////    mParentid_fr = "0";
                                }
							}




						
							//for menuid
							
							int id_Menu_fr = 0;
							if ((bool)isMultilingual)
							{
								dt4 = mGet_One_Menu_Linkid(Convert.ToInt32(mRows["menuid"].ToString()));
								mMenuid = dt4.Rows[0]["id"].ToString();
								//id_Menu_fr = mAdd_MenuItems(mMenuid, mParentid_fr.ToString(), i_fr.ToString(), mRows["priority"].ToString(), this.txt_Menu_FR.Text, mRows["tooltip"].ToString(), "/fr/" + this.txt_SEO_FR.Text.Replace("fr/", "").Replace("/fr/", "").Replace("/", "").Replace("'", "").Replace(",", "").Replace("&",""), mRows["target"].ToString(), mRows["visible"].ToString(), mRows["enabled"].ToString(), id_Menu_en.ToString());
                                id_Menu_fr = mAdd_MenuItems(mMenuid, mParentid_fr.ToString(), i_fr.ToString(), mRows["priority"].ToString(), this.txt_Menu_FR.Text, mRows["tooltip"].ToString(), SeoPrefixFR + this.txt_SEO_FR.Text.Replace("fr/", "").Replace("/fr/", "").Replace("/", "").Replace("'", "").Replace(",", "").Replace("&", ""), mRows["target"].ToString(), mRows["visible"].ToString(), mRows["enabled"].ToString(), id_Menu_en.ToString(),"2");
							}
                        /////////////////////Insert in MenuGroups///////////////////////
                        
                        //for engish
                        if (this.DDL_Groups.SelectedValue.ToString() != "none")
                        {
                            mAdd_Menu_Groups(id_Menu_en.ToString(), this.DDL_Groups.SelectedValue.ToString());

                            //French
							if ((bool)isMultilingual)
								mAdd_Menu_Groups(id_Menu_fr.ToString(), this.DDL_Groups.SelectedValue.ToString());
                        }

                    }
                    else
                    {
                        foreach (DataRow mdr_original in dt1.Rows)
                        {
                            if (mRows["id"].ToString() == mdr_original["id"].ToString())
                            {
                                if (mRows["priority"].ToString() != mdr_original["priority"].ToString())
                                {
                                    //Update
                                    DataTable dt4 = new DataTable();
                                    string mParent_id = mRows["parentid"].ToString();
                                    string mFrench_id = "";
                                 


                                    dt4 = mGet_One_Menuitems_Linkid(Convert.ToInt32(mRows["id"].ToString()));
									if ((bool)isMultilingual)
                                    mFrench_id = dt4.Rows[0]["id"].ToString();

                                   

                                    //for english
                                    mUpdate_Priority(mRows["id"].ToString(), mRows["priority"].ToString());

                                    //for french

									if ((bool)isMultilingual)
										mUpdate_Priority(mFrench_id, mRows["priority"].ToString());
                                }
                                break;
                            }
                        }
                    }
                }

            }
               
         

            /////////////////////Insert in Pages_Content_Zone///////////////////////
           
            DataTable dt_temp_en = new DataTable();
            DataTable dt_temp_fr = new DataTable();
            
            dt_temp_en = mGet_Template_Contents_ByTemplateID(this.DDL_Template_Select.SelectedValue.ToString(), "1");
            dt_temp_fr = mGet_Template_Contents_ByTemplateID(this.DDL_Template_Select.SelectedValue.ToString(), "2");


            //english
            foreach (DataRow dr in dt_temp_en.Rows)
            {
				SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
				SqlCommand sqlComm = new SqlCommand("BASE_Wizard_AutoFill", sqlConn);
				SqlCommand getzonename = new SqlCommand("select name from zones where id=" + dr["Zone_id"].ToString(), sqlConn);
					sqlConn.Open();

                string name = "";
                try { name = getzonename.ExecuteScalar().ToString(); }
                catch { continue; }

                string s = dr["Content_id"].ToString();
                if (Convert.ToInt32(dr["Content_id"].ToString()) > 0)
                {
					if((bool)dr["copy"])
						mAdd_Pages_Content_Zone_Copy(i_en.ToString(), dr["Content_id"].ToString(), dr["Zone_id"].ToString(), "1", this.txt_SEO_EN.Text.Replace("en/", "").Replace("/en/", "").Replace("/", "").Replace("'", "").Replace(",", "").Replace("&","") + " (" + name + ")");
                    else
						mAdd_Pages_Content_Zone(i_en.ToString(), dr["Content_id"].ToString(), dr["Zone_id"].ToString(), "1");
                }
                else if (Convert.ToInt32(dr["Content_id"].ToString()) == -2)
                {
                    
                    sqlComm.CommandType = CommandType.StoredProcedure;
                    sqlComm.Parameters.AddWithValue("@page", i_en.ToString());
                    sqlComm.Parameters.AddWithValue("@zone", dr["Zone_id"].ToString());
                    sqlComm.Parameters.AddWithValue("@lang", 1);
                   // sqlComm.Parameters.AddWithValue("@name", this.txt_SEO_EN.Text);// + " (" + dr["Zone_id"].ToString() + ")");

					;
					sqlComm.Parameters.AddWithValue("@name", this.txt_SEO_EN.Text.Replace("en/", "").Replace("/en/", "").Replace("/", "").Replace("'", "").Replace(",", "").Replace("&","") + " (" + name + ")");
                    sqlComm.ExecuteNonQuery();
                    sqlConn.Close();
                }
            }
            

            //French
			if ((bool)isMultilingual)
            foreach (DataRow dr in dt_temp_fr.Rows)
            {
				SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
                    SqlCommand sqlComm = new SqlCommand("BASE_Wizard_AutoFill", sqlConn);
				SqlCommand getzonename = new SqlCommand("select name from zones where id=" + dr["Zone_id"].ToString(), sqlConn);
					sqlConn.Open();

					string name = getzonename.ExecuteScalar().ToString();

                string s = dr["Content_id"].ToString();
                if (Convert.ToInt32(dr["Content_id"].ToString()) > 0)
                {
					if ((bool)dr["copy"])
						mAdd_Pages_Content_Zone_Copy(i_fr.ToString(), dr["Content_id"].ToString(), dr["Zone_id"].ToString(), "1", this.txt_SEO_FR.Text.Replace("fr/", "").Replace("/fr/", "").Replace("/", "").Replace("'", "").Replace(",", "").Replace("&","") + " (" + name + ")");
					else
						mAdd_Pages_Content_Zone(i_fr.ToString(), dr["Content_id"].ToString(), dr["Zone_id"].ToString(), "1");
                }
                else if (Convert.ToInt32(dr["Content_id"].ToString()) == -2)
                {
                    
                    sqlComm.CommandType = CommandType.StoredProcedure;
                    sqlComm.Parameters.AddWithValue("@page", i_fr.ToString());
                    sqlComm.Parameters.AddWithValue("@zone", dr["Zone_id"].ToString());
                    sqlComm.Parameters.AddWithValue("@lang", 2);


					sqlComm.Parameters.AddWithValue("@name", this.txt_SEO_FR.Text.Replace("fr/", "").Replace("/fr/", "").Replace("/", "").Replace("'", "").Replace(",", "").Replace("&","") + " (" + name + ")");

                    
                    sqlComm.ExecuteNonQuery();
                    sqlConn.Close();
                }
            }



            /////////////////////Insert in Page_Group///////////////////////

            if (this.DDL_Groups.SelectedValue.ToString() != "none")
            {
                //for engish
                mAdd_Page_Groups(i_en.ToString(), this.DDL_Groups.SelectedValue.ToString());

                //French
				if ((bool)isMultilingual)
					mAdd_Page_Groups(i_fr.ToString(), this.DDL_Groups.SelectedValue.ToString());
            }


            //for Main menu level-2 child////////////////////////////////////
            //hard coded "1" - for main menu

            /*
            if (this.DDL_Menu.SelectedValue.ToString() == "1")
            {
                //Add in Menus

                //english menu
                int oo_en = mAdd_Menu(this.txt_Menu_EN.Text, "Vertical", "1", "");
                mUpdate_Menu_Linkid(oo_en.ToString(), oo_en.ToString());

                //french menu
                int oo_fr = mAdd_Menu(this.txt_Menu_FR.Text, "Vertical", "2", oo_en.ToString());


                //Add in Menuitems
                int jj_en = mAdd_MenuItems(oo_en.ToString(), "NULL", i_en.ToString(), "1", "<span>" + this.txt_Menu_EN.Text + "</span>", "", this.txt_SEO_EN.Text, "_self", "0", "1", "");
                mUpdate_MenuItems_Linkid(jj_en.ToString(), jj_en.ToString());

                int jj_fr = mAdd_MenuItems(oo_fr.ToString(), "NULL", i_fr.ToString(), "1", "<span>" + this.txt_Menu_FR.Text + "</span>", "", this.txt_SEO_FR.Text, "_self", "0", "1", jj_en.ToString()); 


                //update or add new left menu in left zone...........
                
                //insert in content
                //for en
                int con_en = mAdd_Contents(this.txt_Menu_EN.Text, "menu", oo_en.ToString(), "1");
                //for fr
                int con_fr = mAdd_Contents(this.txt_Menu_FR.Text, "menu", oo_fr.ToString(), "2");

                //update in page_content_zone
                //Hard coded zone_id = "8" - for left menu in Zones table
                
                //for en                
                mUpdate_Pages_Content_Zone(i_en.ToString(), con_en.ToString(), "8");

                //for fr                
                mUpdate_Pages_Content_Zone(i_fr.ToString(), con_fr.ToString(), "8");


            }

            */

            if (DDL_Menu.SelectedIndex != 0)
            {

                if (int.Parse(DDL_Menu.SelectedValue) > 1)
                {
                    SqlDataAdapter dapt = new SqlDataAdapter("select id from Content where control='menu' and param in (select id from Menus where linkid=" + DDL_Menu.SelectedValue + ") order by language", ConfigurationManager.AppSettings["CMServer"]);
                    DataSet ds = new DataSet();
                    dapt.Fill(ds);
                    //Zone of menu to replace.
                    mUpdate_Pages_Content_Zone(i_en.ToString(), ds.Tables[0].Rows[0][0].ToString(), "24");  //Zone: Left menu
					if((bool)isMultilingual)
                        mUpdate_Pages_Content_Zone(i_fr.ToString(), ds.Tables[0].Rows[1][0].ToString(), "24"); //Zone: Left menu
                }
            }
            //Response.Redirect("~/admin.aspx?c=pages");
            ////Response.Redirect("/en/" + this.txt_SEO_EN.Text.Replace("en/", "").Replace("/en/", "").Replace("/", "").Replace("'", "").Replace(",", "").Replace("&", ""));
            Response.Redirect(CMSHelper.SeoPrefixEN + this.txt_SEO_EN.Text.Replace("en/", "").Replace("/en/", "").Replace("/", "").Replace("'", "").Replace(",", "").Replace("&", ""));

            //Response.Redirect("/" + this.txt_SEO_EN.Text.Replace("en/", "").Replace("/en/", "").Replace("/", "").Replace("'", "").Replace(",", "").Replace("&", ""));



        }
        else if (ViewState["action"].ToString() == "edit")
        {
            int mID = Convert.ToInt32(ViewState["record_id"].ToString());
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
           
            dt = mGet_One_Page(mID);

            int mEnglish_id;
            int mFrench_id;

         
            if (dt.Rows[0]["id"].ToString() == dt.Rows[0]["linkid"].ToString())
            {
                //english
                mEnglish_id = mID;

                //to get french record id
                dt2 = mGet_One_Page_Linkid(mEnglish_id);
                mFrench_id = Convert.ToInt32(dt2.Rows[0]["id"].ToString());
            }
            else
            {
                //french
                mFrench_id = mID;

                //to get english record id
                mEnglish_id = Convert.ToInt32(dt.Rows[0]["linkid"].ToString());
            }

            //update
            //for english
			mEdit_Page(mEnglish_id.ToString(), this.txt_Page_Name_EN.Text, this.DDL_Template_Select.SelectedValue.ToString(), "1", this.txt_Title_EN.Text, this.txt_Keywords_EN.Text, this.txt_Desc_EN.Text, this.txt_SEO_EN.Text.Replace("en/", "").Replace("/en/", "").Replace("/", "").Replace("'", "").Replace(",", "").Replace("&",""));
           
            //for french
			mEdit_Page(mFrench_id.ToString(), this.txt_Page_Name_FR.Text, this.DDL_Template_Select.SelectedValue.ToString(), "2", this.txt_Title_FR.Text, this.txt_Keywords_FR.Text, this.txt_Desc_FR.Text, this.txt_SEO_FR.Text.Replace("fr/", "").Replace("/fr/", "").Replace("/", "").Replace("'", "").Replace(",", "").Replace("&",""));
            
        }
            
            Clear_Fields();
    }


    #region Functions_MENU_STEP3


    DataTable Get_Datatable_Empty(string lang_id, string id)
    {
        string connString = ConfigurationManager.AppSettings["CMServer"].ToString();
        SqlConnection connection = new SqlConnection(connString);
        SqlCommand command = new SqlCommand("select *,id as menuid,'' as priority, '' as parentid from Menus where language = " + lang_id + " and id = " + id, connection);
        SqlDataAdapter da = new SqlDataAdapter(command);
        DataSet ds = new DataSet();
        da.Fill(ds);

        DataTable dt = new DataTable();


        dt = ds.Tables[0];
        return dt;
    }

    DataTable Get_Datatable_NotEmpty(string lang_id, string id)
    {
        string connString = ConfigurationManager.AppSettings["CMServer"].ToString();
        SqlConnection connection = new SqlConnection(connString);
        SqlCommand command = new SqlCommand("declare @temp TABLE(id int, menuid int, parentid int, priority int, text varchar(1000), tooltip varchar(1000), navigateurl varchar(1000), pageid int, target varchar(100),visible bit, enabled bit, linkid int, groupid int) insert into @temp(id,menuid,parentid,priority,text,tooltip,navigateurl,pageid,target,visible,enabled,linkid,groupid) (select MenuItems.id, MenuItems.menuid, isnull(MenuItems.parentid, 0) as parentid, MenuItems.priority, MenuItems.text, MenuItems.tooltip, MenuItems.navigateurl, MenuItems.pageid, MenuItems.target, MenuItems.visible, MenuItems.enabled, MenuItems.linkid,Menu_Group.Group_id from MenuItems left join Menu_Group on MenuItems.id=Menu_Group.MenuItem_id, Menus m where MenuItems.menuid = m.id and m.language = " + lang_id + " and MenuItems.menuid = " + id + ") update @temp set groupid=1 where groupid is NULL update @temp set enabled=0 update @temp set enabled=1 where groupid in (select group_id from users_groups_access where Access_Level>1 and User_id=" + Session["LoggedInID"].ToString() + ") or 1=" + Session["LoggedInID"].ToString() + " select * from @temp", connection);
        SqlDataAdapter da = new SqlDataAdapter(command);
        DataSet ds = new DataSet();
        da.Fill(ds);

        DataTable dt = new DataTable();


        dt = ds.Tables[0];
        return dt;
    }



    void Get_Datatable(string lang_id)
    {
       

        DataTable dt = new DataTable();


        dt = Get_Datatable_NotEmpty(lang_id,this.DDL_Menu.SelectedValue.ToString());

        if (dt.Rows.Count == 0)
        {
            //dt = Get_Datatable_Empty(lang_id, this.DDL_Menu.SelectedValue.ToString());
        }

        if (lang_id == "1")
        {
            //English
            ViewState["dt_menu_en"] = dt;
        }
        else if (lang_id == "2")
        {
            //French
             ViewState["dt_menu_fr"] = dt;
        }


    }


    void mPopulateFields(string lang_id)
    {

        if (lang_id == "1")
        {
            //English
            this.TV_Menu_EN.Nodes.Clear();
        }
        else if (lang_id == "2")
        {
            //French
           // this.TV_Menu_FR.Nodes.Clear();
        }

        TreeNode mRootNode = new TreeNode(DDL_Menu.SelectedItem.Text, "NULL");
        mRootNode.PopulateOnDemand = false;

        mRootNode.SelectAction = TreeNodeSelectAction.Expand;
    	this.Btn_AddRoot_To_Menu_EN.Visible = true;
		//mRootNode.SelectAction = TreeNodeSelectAction.Expand;

        DataTable dt = new DataTable();


        if (lang_id == "1")
        {
            //English
            dt = (DataTable)ViewState["dt_menu_en"];
        }
        else if (lang_id == "2")
        {
            //French
            dt = (DataTable)ViewState["dt_menu_fr"];
        }

        if (dt.Rows.Count == 0)
        {
            if (lang_id == "1")
            {
                //English
                this.TV_Menu_EN.Nodes.Add(mRootNode);
                this.TV_Menu_EN.ExpandAll();
                //this.TV_Menu_EN.Nodes[0].Select();
            }
            else if (lang_id == "2")
            {
                //French
                //this.TV_Menu_FR.Nodes.Add(mRootNode);
                //this.TV_Menu_FR.ExpandAll();
            }
            return;
        }
        else
        {
            if (dt.Rows[0]["priority"].ToString() != "")
            {
                DataRow[] dr = dt.Select("parentid=0", "priority asc");


                foreach (DataRow mRow in dr)
                {
                    string mID, mText;
                    mID = mRow["id"].ToString();
                    mText = mRow["text"].ToString();

                    TreeNode mParentNode = new TreeNode(mText, mID);
                    mParentNode.PopulateOnDemand = false;

					if (mRow["enabled"].ToString() == "True")
					{
						
						mParentNode.SelectAction = TreeNodeSelectAction.SelectExpand;
						//
					}
					else
					{
						mParentNode.SelectAction = TreeNodeSelectAction.None;
						this.Btn_AddRoot_To_Menu_EN.Visible = false;
						if(TV_Menu_EN.Nodes.Count > 0)
							this.TV_Menu_EN.Nodes[0].SelectAction = TreeNodeSelectAction.Expand;
					}

					//mParentNode.Text += "e:" + mRow["enabled"].ToString();

                    mParentNode = mAddNode(mParentNode, lang_id);

                    //add in tree menu
					

                    mRootNode.ChildNodes.Add(mParentNode);

                }
            }
        }

        if (lang_id == "1")
        {
            //English
            this.TV_Menu_EN.Nodes.Add(mRootNode);
            this.TV_Menu_EN.ExpandAll();
        }
        else if (lang_id == "2")
        {
            //French
            //this.TV_Menu_FR.Nodes.Add(mRootNode);
            //this.TV_Menu_FR.ExpandAll();
        }
        string s = "";
        if (lang_id == "1")
        {
            //English
            s = (string)ViewState["selectednode_en"];
        }
        else if (lang_id == "2")
        {
            //French
            s = (string)ViewState["selectednode_fr"];
        }
       
        
        if (s != null && s != "")
        {
            if (lang_id == "1")
            {
                //English
                this.TV_Menu_EN.FindNode(s).Select();
            }
            else if (lang_id == "2")
            {
                //French
                //this.TV_Menu_FR.FindNode(s).Select();
            }
        }
        else
        {
            if (lang_id == "1")
            {
                //English
                //this.TV_Menu_EN.Nodes[0].Select();
                
            }
            else if (lang_id == "2")
            {
                //French
                //this.TV_Menu_FR.Nodes[0].Select();
            }
        }
    }


    TreeNode mAddNode(TreeNode mChildNode, string lang_id)
    {
        string id = mChildNode.Value;


        DataTable dt = new DataTable();

        if (lang_id == "1")
        {
            //English
            dt = (DataTable)ViewState["dt_menu_en"];
        }
        else if (lang_id == "2")
        {
            //French
            dt = (DataTable)ViewState["dt_menu_fr"];
        }

     

        DataRow[] dr = dt.Select("parentid = " + id, "priority asc");



        foreach (DataRow mRow in dr)
        {
            string mID, mText;
            mID = mRow["id"].ToString();
            mText = mRow["text"].ToString();

            TreeNode mNewNode = new TreeNode(mText, mID);
            mNewNode.PopulateOnDemand = false;

			if(mRow["enabled"].ToString() == "True")
				mNewNode.SelectAction = TreeNodeSelectAction.SelectExpand;
			else
			{
				mNewNode.SelectAction = TreeNodeSelectAction.None;
				this.Btn_AddRoot_To_Menu_EN.Visible = false;
				if(TV_Menu_EN.Nodes.Count > 0)
					this.TV_Menu_EN.Nodes[0].SelectAction = TreeNodeSelectAction.Expand;
			}

			//mNewNode.Text += "e2:" + mRow["enabled"].ToString();

            mNewNode = mAddNode(mNewNode,lang_id);

            mChildNode.ChildNodes.Add(mNewNode);
        }


        return mChildNode;
    }

    #endregion

	protected void Btn_AddRoot_To_Menu_EN_Click(object sender, EventArgs e)
    {
		menudelete.Visible = true;
		menudown.Visible = true;
		menuup.Visible = true;
		ImageButton3.Visible = true;
		ImageButton4.Visible = true;
		ImageButton5.Visible = true;
        //validation

        //string s = this.TV_;


        //hard coded "1" - for main menu
        //if (this.DDL_Menu.SelectedValue.ToString() == "1" && s == "NULL")
        //{
        //    this.lbl_msg.Text = "You can't add item at Level 1";
        //    return;
        //}

        //if (this.TV_Menu_EN.SelectedNode == null)
       // {
       //     this.lbl_msg.Text = "Please select menu";
       //     return;
       // }
        
        

        


		string id = "0";// this.TV_Menu_EN.SelectedNode.Value.ToString();

        DataTable dt = new DataTable();
        dt = (DataTable)ViewState["dt_menu_en"];

        string mParent_id;
        string mMenu_id;

        DataRow[] dr;
        if (id == "0")
        {
            mParent_id = "0";
            

            //for menuid
            //mMenu_id = dt.Rows[0]["menuid"].ToString();

            mMenu_id = this.DDL_Menu.SelectedValue.ToString();
             dr = dt.Select("parentid=" + id, "priority desc");
        }
        else
        {

            DataRow[] dr_id = dt.Select("id = " + id);

            mParent_id = dr_id[0]["parentid"].ToString();
            
            //mMenu_id = dr_id[0]["menuid"].ToString();
            mMenu_id = this.DDL_Menu.SelectedValue.ToString();

            dr = dt.Select("parentid = " + id, "priority desc");

        }


        
        int mPriority;

        if (dr.Length == 0)
        {
            mPriority = 0;
        }
        else
        {
            mPriority = Convert.ToInt32(dr[0]["priority"].ToString());
        }
      
        //add in dt

        //for english
        DataRow newdr = dt.NewRow();

        newdr["id"] = "99999991";
        newdr["menuid"] = mMenu_id;
       
        if (id == "NULL")
        {
            newdr["parentid"] = DBNull.Value;
        }
        else
        {
            newdr["parentid"] = id;
        }
        
        newdr["priority"] = mPriority + 1;
        newdr["text"] = this.txt_Menu_EN.Text;
        newdr["tooltip"] = "";
		newdr["navigateurl"] =  this.txt_SEO_EN.Text.Replace("en/", "").Replace("/en/", "").Replace("/", "").Replace("'", "").Replace(",", "").Replace("&","");
        newdr["target"] = "_self";                
        newdr["visible"] = false;
        newdr["enabled"] = true;
        

        dt.Rows.Add(newdr);

      
        ViewState["dt_menu_en"] = dt;

		TV_Menu_EN.Nodes[0].Selected = true;
		string mValue_path = this.TV_Menu_EN.SelectedNode.ValuePath;
        mValue_path = mValue_path + "/" + "99999991";
        ViewState["selectednode_en"] = mValue_path;
        ViewState["menu_en"] = "1"; // Record check - Record Exist   


        

        mPopulateFields("1");

		this.Btn_Add_To_Menu_EN.Visible = false;
		this.Btn_AddRoot_To_Menu_EN.Visible = false;
        this.tbl_Step3_EN.Visible = true;
    }

    protected void Btn_Add_To_Menu_EN_Click(object sender, EventArgs e)
    {
		menudelete.Visible = true;
		menudown.Visible = true;
		menuup.Visible = true;
		ImageButton3.Visible = true;
		ImageButton4.Visible = true;
		ImageButton5.Visible = true;
        //validation

        string s = this.TV_Menu_EN.SelectedNode.Value;


        //hard coded "1" - for main menu
        if (this.DDL_Menu.SelectedValue.ToString() == "1" && s == "NULL")
        {
            this.lbl_msg.Text = "You can't add item at Level 1";
            return;
        }

        if (this.TV_Menu_EN.SelectedNode == null)
        {
            this.lbl_msg.Text = "Please select menu";
            return;
        }
        
        

        


        string id = this.TV_Menu_EN.SelectedNode.Value.ToString();

        DataTable dt = new DataTable();
        dt = (DataTable)ViewState["dt_menu_en"];

        string mParent_id;
        string mMenu_id;

        DataRow[] dr;
        if (id == "NULL")
        {
            mParent_id = "NULL";
            

            //for menuid
            //mMenu_id = dt.Rows[0]["menuid"].ToString();

            mMenu_id = this.DDL_Menu.SelectedValue.ToString();
             dr = dt.Select("parentid is " + id, "priority desc");
        }
        else
        {

            DataRow[] dr_id = dt.Select("id = " + id);

            mParent_id = dr_id[0]["parentid"].ToString();
            
            //mMenu_id = dr_id[0]["menuid"].ToString();
            mMenu_id = this.DDL_Menu.SelectedValue.ToString();

            dr = dt.Select("parentid = " + id, "priority desc");

        }


        
        int mPriority;

        if (dr.Length == 0)
        {
            mPriority = 0;
        }
        else
        {
            mPriority = Convert.ToInt32(dr[0]["priority"].ToString());
        }
      
        //add in dt

        //for english
        DataRow newdr = dt.NewRow();

        newdr["id"] = "99999991";
        newdr["menuid"] = mMenu_id;
       
        if (id == "NULL")
        {
            newdr["parentid"] = DBNull.Value;
        }
        else
        {
            newdr["parentid"] = id;
        }
        
        newdr["priority"] = mPriority + 1;
        newdr["text"] = this.txt_Menu_EN.Text;
        newdr["tooltip"] = "";
        //newdr["navigateurl"] = "/en/" + this.txt_SEO_EN.Text.Replace("en/", "").Replace("/en/", "").Replace("/", "").Replace("'", "").Replace(",", "").Replace("&", "");
        newdr["navigateurl"] = CMSHelper.SeoPrefixEN + this.txt_SEO_EN.Text.Replace("en/", "").Replace("/en/", "").Replace("/", "").Replace("'", "").Replace(",", "").Replace("&", "");
        newdr["target"] = "_self";                
        newdr["visible"] = false;
        newdr["enabled"] = true;
        

        dt.Rows.Add(newdr);

      
        ViewState["dt_menu_en"] = dt;

        string mValue_path = this.TV_Menu_EN.SelectedNode.ValuePath;
        mValue_path = mValue_path + "/" + "99999991";
        ViewState["selectednode_en"] = mValue_path;
        ViewState["menu_en"] = "1"; // Record check - Record Exist   


        

        mPopulateFields("1");

		this.Btn_Add_To_Menu_EN.Visible = false;
    	this.Btn_AddRoot_To_Menu_EN.Visible = false;
        this.tbl_Step3_EN.Visible = true;


    }
    protected void btn_Up_EN_Click(object sender, EventArgs e)
    {
        //UP
        //validation
        if (this.TV_Menu_EN.SelectedNode == null)
        {
            return;
        }

        string id = this.TV_Menu_EN.SelectedNode.Value.ToString();

        if (id != "99999991")
        {
            return;
        }

        DataTable dt = new DataTable();
        dt = (DataTable)ViewState["dt_menu_en"];

        //get Parentid
        
        DataRow[] dr;

        string mParent_id;
        string mPriority;

        DataRow[] dr_id = dt.Select("id = " + id);

        string s = dr_id[0]["parentid"].ToString();

        if (s == "" || s == "NULL")
        {
           
            mParent_id = "NULL";

            mPriority = dr_id[0]["priority"].ToString();
          
             dr = dt.Select("parentid is " + mParent_id + " and priority < " + mPriority, "priority desc");
        }
        else
        {
           
            mParent_id = dr_id[0]["parentid"].ToString();

            mPriority = dr_id[0]["priority"].ToString();

             dr = dt.Select("parentid = " + mParent_id + " and priority < " + mPriority, "priority desc");

           
        }


       

        

        if (dr.Length == 0)
        {
            //first record
            return;
        }

        int swap_id = Convert.ToInt32(dr[0]["id"].ToString());
        int new_priority = Convert.ToInt32(dr[0]["priority"].ToString());

        foreach (DataRow mRows in dt.Rows)
        {
            if (mRows["id"].ToString() == swap_id.ToString())
            {
                mRows["priority"] = mPriority.ToString();

            }

            if (mRows["id"].ToString() == id.ToString())
            {
                mRows["priority"] = new_priority;


            }


        }

  
        ViewState["dt_menu_en"] = dt;
        ViewState["selectednode_en"] = this.TV_Menu_EN.SelectedNode.ValuePath;
        mPopulateFields("1");
    }

	protected void selectedamenu(object s, EventArgs e)
	{
		Btn_Add_To_Menu_EN.Visible = true;

		string id = this.TV_Menu_EN.SelectedNode.Value.ToString();

		if (id == "99999991")
		{
			menuup.Visible = true;
			menudown.Visible = true;
			menudelete.Visible = true;
		}
		else
		{
			menuup.Visible = false;
			menudown.Visible = false;
			menudelete.Visible = false;
		}
	}

    protected void Btn_Down_EN_Click(object sender, EventArgs e)
    {
        //Down
        //validation
        if (this.TV_Menu_EN.SelectedNode == null)
        {
            return;
        }

        string id = this.TV_Menu_EN.SelectedNode.Value.ToString();

        if (id != "99999991")
        {
            return;
        }

        DataTable dt = new DataTable();
        dt = (DataTable)ViewState["dt_menu_en"];

        ////////////////

        DataRow[] dr;

        string mParent_id;
        string mPriority;

        DataRow[] dr_id = dt.Select("id = " + id);

        string s = dr_id[0]["parentid"].ToString();

        if (s == "" || s == "NULL")
        {
            mParent_id = "NULL";

            mPriority = dr_id[0]["priority"].ToString();

            dr = dt.Select("parentid is " + mParent_id + " and priority > " + mPriority, "priority asc");
        }
        else
        {

            mParent_id = dr_id[0]["parentid"].ToString();

            mPriority = dr_id[0]["priority"].ToString();

            dr = dt.Select("parentid = " + mParent_id + " and priority > " + mPriority, "priority asc");


        }


        ////////////////
        
        if (dr.Length == 0)
        {
            //first record
            return;
        }

        int swap_id = Convert.ToInt32(dr[0]["id"].ToString());
        int new_priority = Convert.ToInt32(dr[0]["priority"].ToString());

        foreach (DataRow mRows in dt.Rows)
        {
            if (mRows["id"].ToString() == swap_id.ToString())
            {
                mRows["priority"] = mPriority.ToString();

            }

            if (mRows["id"].ToString() == id.ToString())
            {
                mRows["priority"] = new_priority;


            }


        }


        ViewState["dt_menu_en"] = dt;
        ViewState["selectednode_en"] = this.TV_Menu_EN.SelectedNode.ValuePath;
        mPopulateFields("1");
    }
    protected void Btn_Delete_EN_Click(object sender, EventArgs e)
    {
        this.Btn_Add_To_Menu_EN.Visible = false;
        this.tbl_Step3_EN.Visible = false;

        DataTable dt = new DataTable();
        dt = (DataTable)ViewState["dt_menu_en"];

        foreach (DataRow mRows in dt.Rows)
        {
            string s = mRows["id"].ToString();
            if (mRows["id"].ToString() =="99999991")
            {
                mRows.Delete();
                break;

            }

            

        }

        ViewState["dt_menu_en"] = dt;
        ViewState["selectednode_en"] = "";
        ViewState["menu_en"] = "0";  // Record check - Record not Exist
        mPopulateFields("1");

    }
   
   
    protected void Btn_Back_Step3_Before_Click(object sender, EventArgs e)
    {
        ShowSaveButton();
		this.tbl_step_0.Visible = true;
        this.tbl_Step1.Visible = false;
        this.tbl_Step2.Visible = false;
        this.tbl_step_before3.Visible = false;
        this.tbl_Step3.Visible = false;
        //this.tbl_Step3B.Visible = false;
        this.tbl_Step5.Visible = false;

       // this.tbl_Grid.Visible = false;
    }
    protected void btn_Step3_Next_Before_Click(object sender, EventArgs e)
    {
        ShowSaveButton();

        this.tbl_Step1.Visible = false;
        this.tbl_Step2.Visible = false;
        this.tbl_step_before3.Visible = false;

        //if (this.chk_Step3.Checked)
        //if (this.DDL_Menu.SelectedValue != "none")
        if (this.DDL_Menu.SelectedValue != "")
        {
        	this.tbl_Step2.Visible = false;
            this.tbl_Step3.Visible = true;
            this.tbl_step_before3.Visible = false;
            this.tbl_Step5.Visible = false;

          
        }
        else
        {
			this.tbl_Step2.Visible = true;
            this.tbl_Step3.Visible = false;
            //this.tbl_Step3B.Visible = false;
            this.tbl_Step5.Visible = false;
        }

        

        //this.tbl_Grid.Visible = false;
    }
    protected void LB_Step3_Command(object sender, CommandEventArgs e)
    {

    }
    protected void DDL_Menu_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.DDL_Menu.SelectedValue != "none")
        {
            //this.Btn_Add_To_Menu_EN.Visible = true;
            this.tbl_Step3_EN.Visible = false;

            this.txt_Menu_EN.Text = "";
            this.txt_Menu_FR.Text = "";

            ViewState["selectednode_en"] = "";
            //for menu
            Get_Datatable("1");
            //Get_Datatable("2");

            mPopulateFields("1");
            //mPopulateFields("2");
        }
    }


    protected void Button4_Click(object sender, EventArgs e)
    {
        if (this.RB_Step0.SelectedIndex == 0)
        {
             this.tbl_step_0.Visible = false;
            //this.tbl_Step1.Visible = true;            //this.tbl_Step1.Visible = true;

            ShowSaveButton();

            this.tbl_Step1.Visible = false;             //remove this line
            this.tbl_Step2.Visible = true;              //this.tbl_Step2.Visible = false;

            this.tbl_step_before3.Visible = false;
            this.tbl_Step3.Visible = false;
            //this.tbl_Step3B.Visible = false;

            this.tbl_Step5.Visible = false;

            //this.tbl_Grid.Visible = false;


            //TODO: Ronnie
            this.tbl_Step2.Visible = false;
            this.tbl_step_before3.Visible = true;       //this.tbl_step_before3.Visible = false;
        }
        else if (this.RB_Step0.SelectedValue == "6")
        {
			/*
            SqlDataAdapter dapt = new SqlDataAdapter("select * from DepartmentSites order by name select * from DepartmentAges order by age", ConfigurationManager.AppSettings["CMServer"]);
            DataSet ds = new DataSet();
            dapt.Fill(ds);
            ddlAge.DataSource = ds.Tables[1];
            ddlAge.DataTextField = "age";
            ddlAge.DataValueField = "id";
            ddlAge.DataBind();
            ddlSite.DataSource = ds.Tables[0];
            ddlSite.DataTextField = "name";
            ddlSite.DataValueField = "id";
            ddlSite.DataBind();
            cblSites.DataSource = ds.Tables[0];
            cblSites.DataTextField = "name";
            cblSites.DataValueField = "id";
            cblSites.DataBind();*/
            this.tbl_step_0.Visible = false;
            this.tbl_depart.Visible = true;
        }
		else if (this.RB_Step0.SelectedValue == "5")
        {
            this.tbl_step_0.Visible = false;
            this.tbl_Mini.Visible = true;
        }


        if (rblMultiLang.SelectedValue == "1")
        {
            isMultilingual = true;

            RB_Language.Items.FindByValue("-2").Selected = true;

            frow1.Visible = true;
            frow2.Visible = true;
            frow3.Visible = true;
            frow4.Visible = true;
            frow5.Visible = true;
            frow6.Visible = true;
            frow7.Visible = true;
            frow8.Visible = true;
            frow9.Visible = true;
            frow10.Visible = true;
            frow11.Visible = true;
            frow12.Visible = true;

            //frow13.Visible = true;
            //frow14.Visible = true;
            frow15.Visible = true;
            frow16.Visible = true;
            frow17.Visible = true;
            frow18.Visible = true;
            Tr1.Visible = true;

            frow25.Visible = true;
            frow26.Visible = true;
            frow27.Visible = true;

        }
        else 
        { 
            isMultilingual = false;

            RB_Language.Items.FindByValue("1").Selected = true;

            // MAKE ALL FRENCH FIELDS DISSAPPEAR ------------------------------------------------------

            frow1.Visible = false;
            frow2.Visible = false;
            frow3.Visible = false;
            frow4.Visible = false;
            frow5.Visible = false;
            frow6.Visible = false;
            frow7.Visible = false;
            frow8.Visible = false;
            frow9.Visible = false;
            frow10.Visible = false;
            frow11.Visible = false;
            frow12.Visible = false;

            //frow13.Visible = false;
            //frow14.Visible = false;
            frow15.Visible = false;
            frow16.Visible = false;
            frow17.Visible = false;
            frow18.Visible = false;
            Tr1.Visible = false;

            frow25.Visible = false;
            frow26.Visible = false;
            frow27.Visible = false;

            //----------------------------------------------------------------------------------------------------------

        }
    }
    protected void btn_dept_back_Click(object sender, EventArgs e)
    {
        this.tbl_step_0.Visible = true;
        this.tbl_Facility.Visible = false;
        this.tbl_depart.Visible = false;
		this.tbl_Mini.Visible = false;
    }


    protected void btn_Dept_Submit_Click(object sender, EventArgs e)
    {
		SqlConnection sqlConn2 = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
        string sqlcmd = @"select id from pages where title=@title 
                              select id from pages where title=@titlefr 
                              select id from pages where seo=@en  
                              select id from pages where seo=@fr 
                              select id from groups where name=@group ";

        SqlDataAdapter dapt1 = new SqlDataAdapter(sqlcmd, sqlConn2);
        dapt1.SelectCommand.Parameters.AddWithValue("@en", this.txt_Depart_seo_en.Text.Trim().Replace(" ", "").Replace("/en/", "").Replace("en/", "").Replace("/", "").Replace("'", "").Replace(",", "").Replace("&", ""));
        dapt1.SelectCommand.Parameters.AddWithValue("@fr", this.txt_Depart_seo_fr.Text.Trim().Replace(" ", "").Replace("/fr/", "").Replace("fr/", "").Replace("/", "").Replace("'", "").Replace(",", "").Replace("&", ""));
        dapt1.SelectCommand.Parameters.AddWithValue("@title", txt_TitleDepart_EN.Text);
        dapt1.SelectCommand.Parameters.AddWithValue("@titlefr", txt_TitleDepart_FR.Text);
        dapt1.SelectCommand.Parameters.AddWithValue("@group", txtGroup2.Text);

        DataSet ds1 = new DataSet();
        dapt1.Fill(ds1);

        if (ds1.Tables[0].Rows.Count > 0 || ds1.Tables[1].Rows.Count > 0 || ds1.Tables[2].Rows.Count > 0 || ds1.Tables[3].Rows.Count > 0 || ds1.Tables[4].Rows.Count > 0)
        {
            string mssg = "";
            if (ds1.Tables[0].Rows.Count > 0)
                mssg += "A site with the field Title already exists. " + "\\r\\n";
            if (ds1.Tables[1].Rows.Count > 0)
                mssg += "A site with the URL already exists. " + "\\r\\n";
            if (ds1.Tables[2].Rows.Count > 0)
                mssg += "A site with the English URL already exists. " + "\\r\\n";
            if (ds1.Tables[3].Rows.Count > 0)
                mssg += "A site with the French URL already exists. " + "\\r\\n";
            if (ds1.Tables[4].Rows.Count > 0)
                mssg += "A group with this name already exists. ";

            this.Page.ClientScript.RegisterStartupScript(GetType(), "asd", "alert('" + mssg + "');", true);
            return;
        }


        SqlDataAdapter dapt = new SqlDataAdapter("select id from pages where seo=@en or seo=@fr",sqlConn2);
		dapt.SelectCommand.Parameters.AddWithValue("@en", this.txt_Depart_seo_en.Text.Trim().Replace(" ", "").Replace("/en/", "").Replace("en/", "").Replace("/", "").Replace("'", "").Replace(",", "").Replace("&",""));
		dapt.SelectCommand.Parameters.AddWithValue("@fr", this.txt_Depart_seo_fr.Text.Trim().Replace(" ", "").Replace("/fr/", "").Replace("fr/", "").Replace("/", "").Replace("'", "").Replace(",", "").Replace("&",""));
		
		DataSet ds = new DataSet();
    	dapt.Fill(ds);
		if(ds.Tables[0].Rows.Count > 0)
		{
			this.Page.ClientScript.RegisterStartupScript(GetType(),"asd","alert('A page with this URL already exists');",true);
			return;
		}

        if (this.DDL_Depart.SelectedValue.ToString() != "none")
        {
            DataTable dt9 = new DataTable();
			DataTable dtFr = new DataTable();
            //dt9 = mGet_One_Depart(Convert.ToInt32(this.DDL_Depart.SelectedValue.ToString()));
        	dt9 = mGet_One_Page(Convert.ToInt32(this.DDL_Depart.SelectedValue.ToString()));
        	dtFr = mGet_One_Page_Linkid(Convert.ToInt32(this.DDL_Depart.SelectedValue.ToString()));

            // mAdd_Department(this.txt_Depart_en.Text, this.txt_Depart_fr.Text, dt9.Rows[0]["dept_seo_en"].ToString(), dt9.Rows[0]["dept_seo_fr"].ToString(), this.ddlDepGrp.SelectedValue, this.txt_Phone_number.Text, this.txt_Room_Number.Text, this.RB_Active.SelectedValue.ToString());
            ////			mAdd_Department(this.txt_Depart_en.Text, this.txt_Depart_fr.Text, dt9.Rows[0]["seo"].ToString(), "", this.ddlDepGrp.SelectedValue, this.txt_Phone_number.Text, this.txt_Room_Number.Text, this.RB_Active.SelectedValue.ToString(),

            string seoen = "", seofr = "";
            if (dt9.Rows.Count > 0)
                seoen = dt9.Rows[0]["seo"].ToString();
            if (dt9.Rows.Count > 0)
                seofr = dtFr.Rows[0]["seo"].ToString();

            mAdd_Department(this.txt_Depart_en.Text,
                this.txt_Depart_fr.Text,
                seoen,
                seofr, 
                this.ddlDepGrp.SelectedValue, this.txt_Phone_number.Text, this.txt_Room_Number.Text, this.RB_Active.SelectedValue.ToString(),
                chbFeatured.Checked.ToString());
            string murl = dt9.Rows[0]["seo"].ToString();
            //Response.Redirect("/en/" + murl.Replace(" ", ""));
            Response.Redirect(CMSHelper.SeoPrefixEN + murl.Replace(" ", ""));
        }
        else
        {
			if (txt_Depart_seo_en.Text.Trim() == "")
				return;

            //add in department///
          ////  mAdd_Department(this.txt_Depart_en.Text.Trim(), this.txt_Depart_fr.Text.Trim(), this.txt_Depart_seo_en.Text.Trim(), this.txt_Depart_seo_fr.Text.Trim(), this.ddlDepGrp.SelectedValue, this.txt_Phone_number.Text, this.txt_Room_Number.Text, this.RB_Active.SelectedValue.ToString(), chbFeatured.Checked.ToString());
            mAdd_Department(this.txt_Depart_en.Text.Trim(), 
                this.txt_Depart_fr.Text.Trim(), 
                this.txt_Depart_seo_en.Text.Trim(), txt_Depart_seo_fr.Text.Trim(), this.ddlDepGrp.SelectedValue, this.txt_Phone_number.Text, this.txt_Room_Number.Text, this.RB_Active.SelectedValue.ToString(), chbFeatured.Checked.ToString());



            ///// add in pages /////
            //hard coded layout_id = "23" Services
            //en
            int i_en = mAdd_Page(this.txt_Depart_en.Text, "2", "1", this.txt_TitleDepart_EN.Text, this.txt_Depart_en.Text, this.txt_Depart_en.Text, this.txt_Depart_seo_en.Text.Trim().Replace(" ", ""), "");
            mUpdateLinkid(i_en.ToString());

            //fr
            int i_fr = mAdd_Page(this.txt_Depart_fr.Text, "2", "2", this.txt_TitleDepart_FR.Text, this.txt_Depart_fr.Text, this.txt_Depart_fr.Text, this.txt_Depart_seo_fr.Text.Trim().Replace(" ", ""), i_en.ToString());


            ////// add group//////////////

           // int groupid = mAdd_Group(this.txt_Depart_en.Text, this.txt_Depart_en.Text, "");
            int groupid = mAdd_Group(this.txtGroup2.Text, this.txtGroup2.Text, "");

            /////// add page group ////////
            //for engish
            mAdd_Page_Groups(i_en.ToString(), groupid.ToString());

            //French
            mAdd_Page_Groups(i_fr.ToString(), groupid.ToString());


            /////////////////Add in Menus////////////////////////

            //english menu
            int oo_en = mAdd_Menu(this.txt_Depart_en.Text, "Vertical", "1", "");
            mUpdate_Menu_Linkid(oo_en.ToString(), oo_en.ToString());

            //french menu
            int oo_fr = mAdd_Menu(this.txt_Depart_fr.Text, "Vertical", "2", oo_en.ToString());


            /////////////////////////Add in Menuitems/////////////////////////
            //int jj_en = mAdd_MenuItems(oo_en.ToString(), "NULL", i_en.ToString(), "1", "<span>" + this.txt_Depart_en.Text + "</span>", "", "/en/" + this.txt_Depart_seo_en.Text.Trim().Replace(" ", ""), "_self", "1", "1", "");
           // int jj_en = mAdd_MenuItems(oo_en.ToString(), "NULL", i_en.ToString(), "1", "<span>" + this.txt_Depart_en.Text + "</span>", "", CMSHelper.SeoPrefixEN + this.txt_Depart_seo_en.Text.Trim().Replace(" ", ""), "_self", "1", "1", "");
           //// int jj_en = mAdd_MenuItems(oo_en.ToString(), "NULL", i_en.ToString(), "1", this.txt_Depart_en.Text, "", CMSHelper.SeoPrefixEN + this.txt_Depart_seo_en.Text.Trim().Replace(" ", ""), "_self", "1", "1", "");
            int jj_en = mAdd_MenuItems(oo_en.ToString(), "0", i_en.ToString(), "1", this.txt_Depart_en.Text, "", CMSHelper.SeoPrefixEN + this.txt_Depart_seo_en.Text.Trim().Replace(" ", ""), "_self", "1", "1", "");
            mUpdate_MenuItems_Linkid(jj_en.ToString(), jj_en.ToString());

            //int jj_fr = mAdd_MenuItems(oo_fr.ToString(), "NULL", i_fr.ToString(), "1", "<span>" + this.txt_Depart_fr.Text + "</span>", "", "/fr/" + this.txt_Depart_seo_fr.Text.Trim().Replace(" ", ""), "_self", "1", "1", jj_en.ToString());
            int jj_fr = mAdd_MenuItems(oo_fr.ToString(), "0", i_fr.ToString(), "1", this.txt_Depart_fr.Text, "", SeoPrefixFR + this.txt_Depart_seo_fr.Text.Trim().Replace(" ", ""), "_self", "1", "1", jj_en.ToString(), "2");


            ////////////////////////////////insert in content////////////////////////
            //for en
            int con_en = mAdd_Contents(this.txt_Depart_en.Text, "menu", oo_en.ToString(), "1");
            //for fr
            int con_fr = mAdd_Contents(this.txt_Depart_fr.Text, "menu", oo_fr.ToString(), "2");


            ///////////// add in Menu group ////////////////
            mAdd_Menu_Groups(jj_en.ToString(), groupid.ToString());

            //French
            mAdd_Menu_Groups(jj_fr.ToString(), groupid.ToString());



            ////////////////////update in page_content_zone//////////////////////
            //hard coded template id = "12" (for inside)

            DataTable dt_temp_en = new DataTable();
            DataTable dt_temp_fr = new DataTable();

            // Hardcoded template 18 Services
            
            dt_temp_en = mGet_Template_Contents_ByTemplateID("16", "1");
            dt_temp_fr = mGet_Template_Contents_ByTemplateID("16", "2");


            //english
            using (SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]))
            {
                sqlConn.Open();
                foreach (DataRow dr in dt_temp_en.Rows)
                {
                    SqlCommand getzonename = new SqlCommand("select name from zones where id=" + dr["Zone_id"].ToString(), sqlConn);

                    string name = getzonename.ExecuteScalar().ToString();

                    string s = dr["Content_id"].ToString();
                    if (Convert.ToInt32(dr["Content_id"].ToString()) > 0)
                    {
                        if ((bool)dr["copy"])
                            mAdd_Pages_Content_Zone_Copy(i_en.ToString(), dr["Content_id"].ToString(), dr["Zone_id"].ToString(), "1", this.txt_Depart_seo_en.Text.Replace("en/", "").Replace("/en/", "").Replace("/", "").Replace("'", "").Replace(",", "").Replace("&", "") + " (" + name + ")");
                        else
                            mAdd_Pages_Content_Zone(i_en.ToString(), dr["Content_id"].ToString(), dr["Zone_id"].ToString(), "1");
                        /*mAdd_Pages_Content_Zone(i_en.ToString(), dr["Content_id"].ToString(), dr["Zone_id"].ToString(), "1");*/
                    }
                    else if (Convert.ToInt32(dr["Content_id"].ToString()) == -2)
                    {
                        /*SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);*/
                        SqlCommand sqlComm = new SqlCommand("BASE_Wizard_AutoFill", sqlConn);
                        sqlComm.CommandType = CommandType.StoredProcedure;
                        sqlComm.Parameters.AddWithValue("@page", i_en.ToString());
                        sqlComm.Parameters.AddWithValue("@zone", dr["Zone_id"].ToString());
                        sqlComm.Parameters.AddWithValue("@lang", 1);
                        sqlComm.Parameters.AddWithValue("@name", this.txt_Depart_en.Text.Trim() + dr["Zone_id"].ToString());

                        /*sqlConn.Open();*/
                        sqlComm.ExecuteNonQuery();
                        /*sqlConn.Close();*/
                    }
                }
            }


            //French
            
            foreach (DataRow dr in dt_temp_fr.Rows)
            {
                string s = dr["Content_id"].ToString();
                if (Convert.ToInt32(dr["Content_id"].ToString()) > 0)
                {
                    mAdd_Pages_Content_Zone(i_fr.ToString(), dr["Content_id"].ToString(), dr["Zone_id"].ToString(), "1");
                }
                else if (Convert.ToInt32(dr["Content_id"].ToString()) == -2)
                {
                    SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
                    SqlCommand sqlComm = new SqlCommand("BASE_Wizard_AutoFill", sqlConn);
                    sqlComm.CommandType = CommandType.StoredProcedure;
                    sqlComm.Parameters.AddWithValue("@page", i_fr.ToString());
                    sqlComm.Parameters.AddWithValue("@zone", dr["Zone_id"].ToString());
                    sqlComm.Parameters.AddWithValue("@lang", 2);
                    sqlComm.Parameters.AddWithValue("@name", this.txt_Depart_fr.Text.Trim() + dr["Zone_id"].ToString());

                    sqlConn.Open();
                    sqlComm.ExecuteNonQuery();
                    sqlConn.Close();
                }
            }
            
            /////update in page_content_zone/////////    

            //for en                
            mUpdate_Pages_Content_Zone(i_en.ToString(), con_en.ToString(), "24");

            //for fr                
            mUpdate_Pages_Content_Zone(i_fr.ToString(), con_fr.ToString(), "24");
            string murl = this.txt_Depart_seo_en.Text.Trim();
            //Response.Redirect("/en/" + murl.Replace(" ", ""));
            Response.Redirect(CMSHelper.SeoPrefixEN + murl.Replace(" ", ""));
        }




    }

    protected void btn_Facility_Submit_Click(object sender, EventArgs e)
    {
		if(this.txt_Facility_en.Text.Trim() == "" || this.txt_Facility_fr.Text.Trim() == "")
			return;

		SqlConnection sqlConn2 = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
		SqlDataAdapter dapt = new SqlDataAdapter("select id from facility where facility_seo_en=@en or facility_seo_fr=@fr",sqlConn2);
    	dapt.SelectCommand.Parameters.AddWithValue("@en", this.txt_Facility_seo_en.Text.Trim().Replace(" ", ""));
		dapt.SelectCommand.Parameters.AddWithValue("@fr", this.txt_Facility_seo_fr.Text.Trim().Replace(" ", ""));
		DataSet ds = new DataSet();
    	dapt.Fill(ds);
		if(ds.Tables[0].Rows.Count > 0)
		{
			this.Page.ClientScript.RegisterStartupScript(GetType(),"asd","alert('Facility already exists');",true);
			return;
		}

        if (this.DDL_Facility.SelectedValue.ToString() != "none")
        {
            DataTable dt9 = new DataTable();
			DataTable dtFr = new DataTable();
            dt9 = mGet_One_Page(Convert.ToInt32(this.DDL_Facility.SelectedValue.ToString()));
        	dtFr = mGet_One_Page(Convert.ToInt32(this.DDL_Facility.SelectedValue.ToString()));

            mAdd_Facility(this.txt_Facility_en.Text, this.txt_Facility_fr.Text, dt9.Rows[0]["seo"].ToString(), dtFr.Rows[0]["seo"].ToString(), this.RB_Active_Facility.SelectedValue.ToString());
            string murl = dt9.Rows[0]["seo"].ToString();
            //Response.Redirect("/en/" + murl.Replace(" ", ""));
            Response.Redirect(CMSHelper.SeoPrefixEN + murl.Replace(" ", ""));
        }
        else
        {
            //add in department///

            mAdd_Facility(this.txt_Facility_en.Text, this.txt_Facility_fr.Text, this.txt_Facility_seo_en.Text.Trim().Replace(" ",""), this.txt_Facility_seo_fr.Text.Trim().Replace(" ",""), this.RB_Active_Facility.SelectedValue.ToString());


            ///// add in pages /////
            //hard coded layout_id = "3"
            //en
            int i_en = mAdd_Page(this.txt_Facility_en.Text, "3", "1", this.txt_Facility_en.Text, this.txt_Facility_en.Text, this.txt_Facility_en.Text, this.txt_Facility_seo_en.Text.Trim().Replace(" ", ""), "");
            mUpdateLinkid(i_en.ToString());

            //fr
            int i_fr = mAdd_Page(this.txt_Facility_fr.Text, "3", "2", this.txt_Facility_fr.Text, this.txt_Facility_fr.Text, this.txt_Facility_fr.Text, this.txt_Facility_seo_fr.Text.Trim().Replace(" ", ""), i_en.ToString());


            ////// add group//////////////

            int groupid = mAdd_Group(this.txt_Facility_en.Text, this.txt_Facility_en.Text, "");

            /////// add page group ////////
            //for engish
            mAdd_Page_Groups(i_en.ToString(), groupid.ToString());

            //French
            mAdd_Page_Groups(i_fr.ToString(), groupid.ToString());


            /////////////////Add in Menus////////////////////////

            //english menu
            int oo_en = mAdd_Menu(this.txt_Facility_en.Text, "Vertical", "1", "");
            mUpdate_Menu_Linkid(oo_en.ToString(), oo_en.ToString());

            //french menu
            int oo_fr = mAdd_Menu(this.txt_Facility_fr.Text, "Vertical", "2", oo_en.ToString());


            /////////////////////////Add in Menuitems/////////////////////////
            ////int jj_en = mAdd_MenuItems(oo_en.ToString(), "NULL", i_en.ToString(), "1","<span>" +  this.txt_Facility_en.Text + "</span>", "",  this.txt_Facility_seo_en.Text.Trim().Replace(" ",""), "_self", "0", "1", "","1");
            int jj_en = mAdd_MenuItems(oo_en.ToString(), "NULL", i_en.ToString(), "1", this.txt_Facility_en.Text, "",  this.txt_Facility_seo_en.Text.Trim().Replace(" ",""), "_self", "0", "1", "","1");
            mUpdate_MenuItems_Linkid(jj_en.ToString(), jj_en.ToString());

            int jj_fr = mAdd_MenuItems(oo_fr.ToString(), "NULL", i_fr.ToString(), "1", this.txt_Facility_fr.Text, "",  this.txt_Facility_seo_fr.Text.Trim().Replace(" ",""), "_self", "0", "1", jj_en.ToString(),"2");


            ////////////////////////////////insert in content////////////////////////
            //for en
            int con_en = mAdd_Contents(this.txt_Facility_en.Text, "menu", oo_en.ToString(), "1");
            //for fr
            int con_fr = mAdd_Contents(this.txt_Facility_fr.Text, "menu", oo_fr.ToString(), "2");


            ///////////// add in Menu group ////////////////
            mAdd_Menu_Groups(jj_en.ToString(), groupid.ToString());

            //French
            mAdd_Menu_Groups(jj_fr.ToString(), groupid.ToString());



            ////////////////////update in page_content_zone//////////////////////
            //hard coded template id = "12" (for inside)

            DataTable dt_temp_en = new DataTable();
            DataTable dt_temp_fr = new DataTable();

//            dt_temp_en = mGet_Template_Contents_ByTemplateID("13", "1");
//            dt_temp_fr = mGet_Template_Contents_ByTemplateID("13", "2");
            dt_temp_en = mGet_Template_Contents_ByTemplateID("16", "1");
            dt_temp_fr = mGet_Template_Contents_ByTemplateID("16", "2");


            //english
            foreach (DataRow dr in dt_temp_en.Rows)
            {
                string s = dr["Content_id"].ToString();
                if (Convert.ToInt32(dr["Content_id"].ToString()) > 0)
                {
                    mAdd_Pages_Content_Zone(i_en.ToString(), dr["Content_id"].ToString(), dr["Zone_id"].ToString(), "1");
                }
                else if (Convert.ToInt32(dr["Content_id"].ToString()) == -2)
                {
                    SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
                    SqlCommand sqlComm = new SqlCommand("BASE_Wizard_AutoFill", sqlConn);
                    sqlComm.CommandType = CommandType.StoredProcedure;
                    sqlComm.Parameters.AddWithValue("@page", i_en.ToString());
                    sqlComm.Parameters.AddWithValue("@zone", dr["Zone_id"].ToString());
                    sqlComm.Parameters.AddWithValue("@lang", 1);
                    sqlComm.Parameters.AddWithValue("@name", this.txt_Facility_en.Text.Trim() + dr["Zone_id"].ToString());

                    sqlConn.Open();
                    sqlComm.ExecuteNonQuery();
                    sqlConn.Close();
                }
            }


            //French
            foreach (DataRow dr in dt_temp_fr.Rows)
            {
                string s = dr["Content_id"].ToString();
                if (Convert.ToInt32(dr["Content_id"].ToString()) > 0)
                {
                    mAdd_Pages_Content_Zone(i_fr.ToString(), dr["Content_id"].ToString(), dr["Zone_id"].ToString(), "1");
                }
                else if (Convert.ToInt32(dr["Content_id"].ToString()) == -2)
                {
                    SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
                    SqlCommand sqlComm = new SqlCommand("BASE_Wizard_AutoFill", sqlConn);
                    sqlComm.CommandType = CommandType.StoredProcedure;
                    sqlComm.Parameters.AddWithValue("@page", i_fr.ToString());
                    sqlComm.Parameters.AddWithValue("@zone", dr["Zone_id"].ToString());
                    sqlComm.Parameters.AddWithValue("@lang", 2);
                    sqlComm.Parameters.AddWithValue("@name", this.txt_Facility_fr.Text.Trim() + dr["Zone_id"].ToString());

                    sqlConn.Open();
                    sqlComm.ExecuteNonQuery();
                    sqlConn.Close();
                }
            }

            /////update in page_content_zone/////////    

            //for en                
            mUpdate_Pages_Content_Zone(i_en.ToString(), con_en.ToString(), "24");

            //for fr                
            mUpdate_Pages_Content_Zone(i_fr.ToString(), con_fr.ToString(), "24");
            string murl = this.txt_Facility_seo_en.Text.Trim();
            //Response.Redirect("/en/" + murl.Replace(" ", ""));
            Response.Redirect(CMSHelper.SeoPrefixEN + murl.Replace(" ", ""));
        }



    }

	protected void btn_Submit_Mini_Click(object sender, EventArgs e)
    {
		if (this.mininameen.Text.Trim() == "" || this.txtGroup.Text == "")
			return;

		if ((bool)isMultilingual)
			if (this.mininamefr.Text.Trim() == "")
				return;
			

		SqlConnection sqlConn2 = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);

        //////string command = "select id from pages where seo=@en ";
        //////if ((bool)isMultilingual)
        //////	command += "or seo=@fr";
        //////SqlDataAdapter dapt = new SqlDataAdapter(command,sqlConn2);
        //////  	dapt.SelectCommand.Parameters.AddWithValue("@en", this.miniseoen.Text.Trim().Replace(" ", ""));
        //////if ((bool)isMultilingual)
        //////	dapt.SelectCommand.Parameters.AddWithValue("@fr", this.miniseofr.Text.Trim().Replace(" ", ""));
        //////DataSet ds = new DataSet();
        //////  	dapt.Fill(ds);
        //////if(ds.Tables[0].Rows.Count > 0)
        //////{
        //////	this.Page.ClientScript.RegisterStartupScript(GetType(),"asd","alert('A site with this URL exists already');",true);
        //////	return;
        //////}

        ////string sqlcmd = @"select id from pages where title=@title 
        ////                      select id from pages where title=@titlefr 
        ////                      select id from pages where seo=@en  
        ////                      select id from pages where seo=@fr 
        ////                      select id from groups where name=@group ";

        string sqlcmd = @"select id from pages where title=@title 
                              select id from pages where title=@titlefr 
                              select id from pages where seo=@en  
                              select id from pages where seo=@fr 
                              select id from groups where name=@group ";

        SqlDataAdapter dapt1 = new SqlDataAdapter(sqlcmd, sqlConn2);
        dapt1.SelectCommand.Parameters.AddWithValue("@en", this.miniseoen.Text.Trim().Replace(" ", ""));
        dapt1.SelectCommand.Parameters.AddWithValue("@fr", (bool)isMultilingual ? this.miniseofr.Text.Trim().Replace(" ", "") : "");
        dapt1.SelectCommand.Parameters.AddWithValue("@title", txt_TitleMini_EN.Text);
        dapt1.SelectCommand.Parameters.AddWithValue("@titlefr", (bool)isMultilingual ? txt_TitleMini_FR.Text : "");
        dapt1.SelectCommand.Parameters.AddWithValue("@group", txtGroup.Text);

        DataSet ds1 = new DataSet();
        dapt1.Fill(ds1);

        if (ds1.Tables[0].Rows.Count > 0 || ds1.Tables[1].Rows.Count > 0 || ds1.Tables[2].Rows.Count > 0 )
        {
            string mssg = "";
            if (ds1.Tables[0].Rows.Count > 0)
                mssg += "A site with the field Title already exists. " + "\\r\\n";
            if (ds1.Tables[1].Rows.Count > 0)
                mssg += "A site with the URL already exists. " + "\\r\\n";
            if (ds1.Tables[2].Rows.Count > 0)
                mssg += "A site with the English URL already exists. " + "\\r\\n";
            if (ds1.Tables[4].Rows.Count > 0)
                mssg += "A group with this name already exists. " + "\\r\\n";
            if (ds1.Tables[1].Rows.Count > 0)
                mssg += "A site with the French Title already exists. " + "\\r\\n";
            if (ds1.Tables[3].Rows.Count > 0)
                mssg += "A site with the French URL already exists. ";

            this.Page.ClientScript.RegisterStartupScript(GetType(), "asd", "alert('" + mssg + "');", true);
            return;
        }


        /*
        if (this.DDL_JGH.SelectedValue.ToString() != "none")
        {
            DataTable dt9 = new DataTable();
			DataTable dtFr = new DataTable();

            dt9 = mGet_One_Page(Convert.ToInt32(this.DDL_JGH.SelectedValue.ToString()));
        	dtFr = mGet_One_Page_Linkid(Convert.ToInt32(this.DDL_JGH.SelectedValue.ToString()));

            mAdd_JGH_Program(this.txt_JGH_en.Text, this.txt_JGH_fr.Text, dt9.Rows[0]["seo"].ToString(), dtFr.Rows[0]["seo"].ToString(), this.RB_Active_JGH.SelectedValue.ToString());
            string murl = dt9.Rows[0]["seo"].ToString();
            Response.Redirect(murl.Replace(" ",""));
		
        }
        else
        {*/

        //add in department///

        // mAdd_JGH_Program(this.txt_JGH_en.Text, this.txt_JGH_fr.Text, this.txt_JGH_en.Text.Trim().Replace(" ",""), this.txt_JGH_fr.Text.Trim().Replace(" ",""), this.RB_Active_JGH.SelectedValue.ToString());


        ///// add in pages /////
        //hard coded layout_id = "3"
        //en
        int i_en = mAdd_Page(this.mininameen.Text, "2", "1", this.txt_TitleMini_EN.Text, this.mininameen.Text, this.mininameen.Text, this.miniseoen.Text.Trim().Replace(" ", ""), "");
            mUpdateLinkid(i_en.ToString());

            //fr
		int i_fr = 0;
		if ((bool)isMultilingual)
            i_fr = mAdd_Page(this.mininamefr.Text, "2", "2", this.txt_TitleMini_FR.Text, this.mininamefr.Text, this.mininamefr.Text, this.miniseofr.Text.Trim().Replace(" ", ""), i_en.ToString());


            ////// add group//////////////

        //int groupid = mAdd_Group_Mini(this.mininameen.Text, this.mininameen.Text, "");
        int groupid = mAdd_Group_Mini(this.txtGroup.Text, this.txtGroup.Text, "");

            /////// add page group ////////
            //for engish
            mAdd_Page_Groups(i_en.ToString(), groupid.ToString());

            //French
			if ((bool)isMultilingual)
            mAdd_Page_Groups(i_fr.ToString(), groupid.ToString());


            /////////////////Add in Menus////////////////////////

            //english menu
            int oo_en = mAdd_Menu(this.mininameen.Text, "Vertical", "1", "");
            mUpdate_Menu_Linkid(oo_en.ToString(), oo_en.ToString());

            //french menu
		int oo_fr = 0;
		if ((bool)isMultilingual)
            oo_fr = mAdd_Menu(this.mininamefr.Text, "Vertical", "2", oo_en.ToString());


            /////////////////////////Add in Menuitems/////////////////////////
            int jj_en = mAdd_MenuItems(oo_en.ToString(), "0", i_en.ToString(), "1",  this.mininameen.Text , "",  this.miniseoen.Text.Trim().Replace(" ",""), "_self", "0", "1", "","1");
            mUpdate_MenuItems_Linkid(jj_en.ToString(), jj_en.ToString());
		int jj_fr = 0;
		if ((bool)isMultilingual)
			jj_fr = mAdd_MenuItems(oo_fr.ToString(), "0", i_fr.ToString(), "1",  this.mininamefr.Text , "", this.miniseofr.Text.Trim().Replace(" ",""), "_self", "0", "1", jj_en.ToString(),"2");


            ////////////////////////////////insert in content////////////////////////
            //for en
            int con_en = mAdd_Contents(this.mininameen.Text, "menu", oo_en.ToString(), "1");
            //for fr
		int con_fr = 0;
		if ((bool)isMultilingual) 
			con_fr = mAdd_Contents(this.mininamefr.Text, "menu", oo_fr.ToString(), "2");


            ///////////// add in Menu group ////////////////
            mAdd_Menu_Groups(jj_en.ToString(), groupid.ToString());

            //French
			if ((bool)isMultilingual)
            mAdd_Menu_Groups(jj_fr.ToString(), groupid.ToString());



            ////////////////////update in page_content_zone//////////////////////
            //hard coded template id = "12" (for inside)

            DataTable dt_temp_en = new DataTable();
            DataTable dt_temp_fr = new DataTable();

            dt_temp_en = mGet_Template_Contents_ByTemplateID("16", "1");
            dt_temp_fr = mGet_Template_Contents_ByTemplateID("16", "2");


            //english
            foreach (DataRow dr in dt_temp_en.Rows)
            {
                string s = dr["Content_id"].ToString();
                if (Convert.ToInt32(dr["Content_id"].ToString()) > 0)
                {
                    if ((bool)dr["copy"])
                        mAdd_Pages_Content_Zone_Copy(i_en.ToString(), dr["Content_id"].ToString(), dr["Zone_id"].ToString(), "1");
                    else
                        mAdd_Pages_Content_Zone(i_en.ToString(), dr["Content_id"].ToString(), dr["Zone_id"].ToString(), "1");
                    //mAdd_Pages_Content_Zone(i_en.ToString(), dr["Content_id"].ToString(), dr["Zone_id"].ToString(), "1");
                }
                else if (Convert.ToInt32(dr["Content_id"].ToString()) == -2)
                {
                    SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
                    SqlCommand sqlComm = new SqlCommand("BASE_Wizard_AutoFill", sqlConn);
                    sqlComm.CommandType = CommandType.StoredProcedure;
                    sqlComm.Parameters.AddWithValue("@page", i_en.ToString());
                    sqlComm.Parameters.AddWithValue("@zone", dr["Zone_id"].ToString());
                    sqlComm.Parameters.AddWithValue("@lang", 1);
                    sqlComm.Parameters.AddWithValue("@name", this.miniseoen.Text.Trim() + dr["Zone_id"].ToString());

                    sqlConn.Open();
                    sqlComm.ExecuteNonQuery();
                    sqlConn.Close();
                }
            }


        //French
        if ((bool)isMultilingual)
            foreach (DataRow dr in dt_temp_fr.Rows)
            {
                string s = dr["Content_id"].ToString();
                if (Convert.ToInt32(dr["Content_id"].ToString()) > 0)
                {
                    if ((bool)dr["copy"])
                        mAdd_Pages_Content_Zone_Copy(i_fr.ToString(), dr["Content_id"].ToString(), dr["Zone_id"].ToString(), "1");
                    else
                        mAdd_Pages_Content_Zone(i_fr.ToString(), dr["Content_id"].ToString(), dr["Zone_id"].ToString(), "1");

                    //mAdd_Pages_Content_Zone(i_fr.ToString(), dr["Content_id"].ToString(), dr["Zone_id"].ToString(), "1");
                }
                else if (Convert.ToInt32(dr["Content_id"].ToString()) == -2)
                {
                    SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
                    SqlCommand sqlComm = new SqlCommand("BASE_Wizard_AutoFill", sqlConn);
                    sqlComm.CommandType = CommandType.StoredProcedure;
                    sqlComm.Parameters.AddWithValue("@page", i_fr.ToString());
                    sqlComm.Parameters.AddWithValue("@zone", dr["Zone_id"].ToString());
                    sqlComm.Parameters.AddWithValue("@lang", 2);
                    sqlComm.Parameters.AddWithValue("@name", this.miniseofr.Text.Trim() + dr["Zone_id"].ToString());

                    sqlConn.Open();
                    sqlComm.ExecuteNonQuery();
                    sqlConn.Close();
                }
            }

        /////update in page_content_zone/////////    

        //for en                
        mUpdate_Pages_Content_Zone(i_en.ToString(), con_en.ToString(), "24");   //Left Menu

        //for fr                
        if ((bool)isMultilingual)
            mUpdate_Pages_Content_Zone(i_fr.ToString(), con_fr.ToString(), "24");   //Left Menu

        string murl = this.miniseoen.Text.Trim();
            //Response.Redirect("/en/" + murl.Replace(" ", ""));
            Response.Redirect(CMSHelper.SeoPrefixEN + murl.Replace(" ", ""));
       // }

    }

    public void mAdd_Pages_Content_Zone_Copy(string Page_ID, string Content_ID, string Zone_ID, string Priority)
    {

        sb = sb.Remove(0, sb.Length);
        sb.Append(" insert into html(html,timestamp,editor)  select html,timestamp,'1' from html where id=(select param from content where id=@Content_ID) ");
        sb.Append(" insert into content(name,control,param,language) values( (select name from content where id=@Content_ID), 'content', @@IDENTITY, (select language from content where id=@Content_ID))");
        sb.Append(" insert into Pages_Content_Zone  (Page_ID,Content_ID,Zone_ID,Priority) values(@Page_ID,@@IDENTITY,@Zone_ID,@Priority)");
        //sb.Append(" SELECT id FROM Pages WHERE (id = SCOPE_IDENTITY()) ");
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@Page_ID", Page_ID);
            cmd.Parameters.AddWithValue("@Content_ID", Content_ID);
            cmd.Parameters.AddWithValue("@Zone_ID", Zone_ID);
            cmd.Parameters.AddWithValue("@Priority", Priority);




            connection.Open();
            cmd.ExecuteScalar();


        }

    }

    /*protected void DDL_Depart_change(object sender, EventArgs e)
    {
        if (DDL_Depart.SelectedValue != "none")
        {
            RequiredFieldValidator12.Enabled = false;
            RequiredFieldValidator15.Enabled = false;
            RequiredFieldValidator24.Enabled = false;
            
        }
        else
        {
            RequiredFieldValidator12.Enabled = true;
            RequiredFieldValidator15.Enabled = true;
            RequiredFieldValidator24.Enabled = true;
        }
    }*/

}
