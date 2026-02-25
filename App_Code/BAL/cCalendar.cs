using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Data.SqlClient;

/// <summary>
/// Summary description for cCalendar
/// </summary>
public class cCalendar
{
    StringBuilder sb;

    public cCalendar()
    {
        sb = new StringBuilder(60);
        //this.pIs_Deleted = 0;
        //this.pIs_Enabled = 1;
        //this.pDate_Created = DateTime.Now;
        //this.pCreated_By = -1;
    }


    #region Variables

    private int cal_id;
    private string cal_Name;
    private string cal_Desc;


    private int Is_Deleted;
    private int Is_Enabled;
    private DateTime Date_Created;
    private DateTime Date_Edited;
    private int Created_By;
    private int Edited_By;
    #endregion

    #region Properties

    public int pcal_id
    {
        set
        {
            this.cal_id = value;
        }
        get
        {
            return this.cal_id;
        }
    }

    public string pcal_Name
    {
        set
        {
            this.cal_Name = value;
        }
        get
        {
            return this.cal_Name;
        }
    }

    public string pcal_Desc
    {
        set
        {
            this.cal_Desc = value;
        }
        get
        {
            return this.cal_Desc;
        }
    }




    public int pIs_Deleted
    {
        set
        {
            this.Is_Deleted = value;
        }
        get
        {
            return this.Is_Deleted;
        }
    }

    public int pIs_Enabled
    {
        set
        {
            this.Is_Enabled = value;
        }
        get
        {
            return this.Is_Enabled;
        }
    }

    public DateTime pDate_Created
    {
        set
        {
            this.Date_Created = value;
        }
        get
        {
            return this.Date_Created;
        }
    }

    public DateTime pDate_Edited
    {
        set
        {
            this.Date_Edited = value;
        }
        get
        {
            return this.Date_Edited;
        }
    }

    public int pCreated_By
    {
        set
        {
            this.Created_By = value;
        }
        get
        {
            return this.Created_By;
        }
    }

    public int pEdited_By
    {
        set
        {
            this.Edited_By = value;
        }
        get
        {
            return this.Edited_By;
        }
    }




    #endregion

    #region Methods
    //1 = Yes
    //0 = No
    public DataTable mGet_All()
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"];
        string commandString = "select  * from tbl_Calendar where Is_deleted = 0";
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

    public DataTable mGet_All_GV(string uid = "1")
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"];
        string commandString = "select t.*,(select COUNT(*) from tbl_cal_events a where a.cal_id = t.cal_id and Event_Id in (select Event_Id from tbl_Events where Is_Deleted = 0)) as Event_counts from tbl_calendar t where t.is_deleted = 0 and (t.[group] in (select group_id from users_groups_access where (user_id=" + uid + " and access_level>1)) or " + uid + "=1) ";
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

    public DataTable mGet_One(int mID)
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"];
        string commandString = "select * from tbl_Calendar where cal_id = @mID and Is_Deleted = 0";
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

    public DataTable mGet_All_Enabled(string uid)
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"];
        string commandString = "select  * from tbl_Calendar where Is_deleted = 0 and Is_enabled = 1";
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

    public decimal mAdd(string uid = "1", string group = "1")
    {
        decimal ret = 0m;
        sb = sb.Remove(0, sb.Length);
        sb.Append(" insert into  tbl_Calendar (cal_Name, cal_Desc, Is_Deleted, Is_Enabled, Created_By, Date_Created,[group]) ");
        sb.Append(" values   (@cal_Name, @cal_Desc, @Is_Deleted, @Is_Enabled, @Created_By, getdate(),@Grp)");
        sb.Append(" select scope_identity()");
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"];
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@cal_Name", this.pcal_Name);
            cmd.Parameters.AddWithValue("@cal_Desc", this.pcal_Desc);

            cmd.Parameters.AddWithValue("@Is_Deleted", this.pIs_Deleted);
            cmd.Parameters.AddWithValue("@Is_Enabled", this.pIs_Enabled);
            cmd.Parameters.AddWithValue("@Created_By", uid);
            cmd.Parameters.AddWithValue("@Grp", group);
            //cmd.Parameters.AddWithValue("@Date_Created", DateTime.Now.ToShortDateString());


            connection.Open();
            ret = (decimal)cmd.ExecuteScalar();
        }
        return ret;
    }

    public void mAdd_Content(string mControl_Name, string mParam, string mName, string mLanguage_id)
    {

        sb = sb.Remove(0, sb.Length);
        sb.Append(" insert into  tbl_Content (Control_Name, Param, Name, Language_id) ");
        sb.Append(" values   (@Control_Name, @Param, @Name, @Language_id)");
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"];
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@Control_Name", mControl_Name);
            cmd.Parameters.AddWithValue("@Param", mParam);

            cmd.Parameters.AddWithValue("@Name", mName);
            cmd.Parameters.AddWithValue("@Language_id", mLanguage_id);

            connection.Open();
            cmd.ExecuteNonQuery();
        }

    }




    public void mEdit(string grp = "1")
    {
        sb = sb.Remove(0, sb.Length);
        sb.Append(" update tbl_Calendar ");
        sb.Append(" Set cal_Name = @cal_Name, ");
        sb.Append(" cal_Desc = @cal_Desc, ");


        sb.Append(" Is_Enabled = @Is_Enabled, ");
        sb.Append(" Edited_By = @Edited_By, ");
        sb.Append(" Date_Edited = @Date_Edited, ");
        sb.Append(" [group] = @Grp ");
        sb.Append(" where cal_id = @cal_id ");
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"];
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@cal_id", this.pcal_id);
            cmd.Parameters.AddWithValue("@cal_Name", this.pcal_Name);
            cmd.Parameters.AddWithValue("@cal_Desc", this.pcal_Desc);


            cmd.Parameters.AddWithValue("@Is_Enabled", this.pIs_Enabled);

            cmd.Parameters.AddWithValue("@Edited_By", this.pEdited_By);
            cmd.Parameters.AddWithValue("@Date_Edited", DateTime.Now.ToShortDateString());

            cmd.Parameters.AddWithValue("@Grp", grp);
            connection.Open();
            cmd.ExecuteNonQuery();
        }

    }

    public void mDelete()
    {
        sb = sb.Remove(0, sb.Length);
        sb.Append(" update tbl_Calendar ");
        sb.Append(" Set Is_Deleted = 1 ");
        sb.Append(" where cal_id = @cal_id ");

        string strConnectionString = ConfigurationManager.AppSettings["CMServer"];
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@cal_id", this.pcal_id);

            connection.Open();
            cmd.ExecuteNonQuery();
        }


    }


    #endregion

}
