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
/// Summary description for cEvent_Multiple
/// </summary>
public class cEvent_Multiple
{
	StringBuilder sb;
    public cEvent_Multiple()
    {
        sb = new StringBuilder(60);

       
        this.pIs_Deleted = 0;
        this.pIs_Enabled = 1;
        this.pDate_Created = DateTime.Now;
        this.pCreated_By = -1;
    }


    #region Variables

    private int Event_Multiple_id;
    private int Event_id;
    private string Day_No;
	//private DateTime Start_Time;
	//private DateTime End_Time;
	private string Start_Time;
	private string End_Time;
    private DateTime Event_Date;
   
    private int Is_Deleted;
    private int Is_Enabled;
    private DateTime Date_Created;
    private DateTime Date_Edited;
    private int Created_By;
    private int Edited_By;
    #endregion

    #region Properties

    public int pEvent_Multiple_id
    {
        set
        {
            this.Event_Multiple_id = value;
        }
        get
        {
            return this.Event_Multiple_id;
        }
    }

    public int pEvent_id
    {
        set
        {
            this.Event_id = value;
        }
        get
        {
            return this.Event_id;
        }
    }

    public string pDay_No
    {
        set
        {
            this.Day_No = value;
        }
        get
        {
            return this.Day_No;
        }
    }

	//public DateTime pStart_Time
	//{
	//    set
	//    {
	//        this.Start_Time = value;
	//    }
	//    get
	//    {
	//        return this.Start_Time;
	//    }
	//}

	//public DateTime pEnd_Time
	//{
	//    set
	//    {
	//        this.End_Time = value;
	//    }
	//    get
	//    {
	//        return this.End_Time;
	//    }
	//}

	public string pStart_Time
	{
		set
		{
			this.Start_Time = value;
		}
		get
		{
			return this.Start_Time;
		}
	}

	public string pEnd_Time
	{
		set
		{
			this.End_Time = value;
		}
		get
		{
			return this.End_Time;
		}
	}


    public DateTime pEvent_Date
    {
        set
        {
            this.Event_Date = value;
        }
        get
        {
            return this.Event_Date;
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
        string commandString = "select  * from tbl_Event_Multiple where Is_deleted = 0";
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

    public DataTable mGet_All_Enabled()
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"];
        string commandString = "select  * from tbl_Event_Multiple where Is_deleted = 0 and Is_enabled = 1";
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
        string commandString = "select * from tbl_Event_Multiple where Event_Multiple_id = @mID and Is_Deleted = 0";
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

    public DataTable mGet_ByEventID(int mID)
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"];
        string commandString = "select * from tbl_Event_Multiple where Event_id = @mID and Is_Deleted = 0";
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


    public int mAdd()
    {

        sb = sb.Remove(0, sb.Length);
        sb.Append(" insert into  tbl_Event_Multiple (Event_id,Day_No,Start_Time,End_Time,Event_Date, Is_Deleted, Is_Enabled, Created_By, Date_Created) ");
        sb.Append(" values   (@Event_id, @Day_No, @Start_Time, @End_Time, @Event_Date, @Is_Deleted, @Is_Enabled, @Created_By, @Date_Created)");
		sb.Append(" SELECT Event_Multiple_id FROM tbl_Event_Multiple WHERE (Event_Multiple_id = SCOPE_IDENTITY()) ");
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"];
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@Event_id", this.pEvent_id);
            cmd.Parameters.AddWithValue("@Day_No", this.pDay_No);
            cmd.Parameters.AddWithValue("@Start_Time", this.pStart_Time);
            cmd.Parameters.AddWithValue("@End_Time", this.pEnd_Time);
            cmd.Parameters.AddWithValue("@Event_Date", this.pEvent_Date);
            
            cmd.Parameters.AddWithValue("@Is_Deleted", this.pIs_Deleted);
            cmd.Parameters.AddWithValue("@Is_Enabled", this.pIs_Enabled);
            cmd.Parameters.AddWithValue("@Created_By", this.pCreated_By);
            cmd.Parameters.AddWithValue("@Date_Created", DateTime.Now.ToShortDateString());


            connection.Open();
			int i = Convert.ToInt32(cmd.ExecuteScalar());

            return i;
        }

    }      

    public void mEdit()
    {
        sb = sb.Remove(0, sb.Length);
        sb.Append(" update tbl_Event_Multiple ");
        sb.Append(" Set Event_id = @Event_id, ");
        sb.Append(" Day_No = @Day_No, ");
        sb.Append(" Start_Time = @Start_Time, ");
        sb.Append(" End_Time = @End_Time, ");
        sb.Append(" Event_Date = @Event_Date ");       

        sb.Append(" Is_Enabled = @Is_Enabled, ");
        sb.Append(" Edited_By = @Edited_By, ");
        sb.Append(" Date_Edited = @Date_Edited ");
        sb.Append(" where Event_Multiple_id = @Event_Multiple_id ");
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"];
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);

            cmd.Parameters.AddWithValue("@Event_Multiple_id", this.pEvent_Multiple_id); 
            cmd.Parameters.AddWithValue("@Event_id", this.pEvent_id);
            cmd.Parameters.AddWithValue("@Day_No", this.pDay_No);
            cmd.Parameters.AddWithValue("@Start_Time", this.pStart_Time);
            cmd.Parameters.AddWithValue("@End_Time", this.pEnd_Time);
            cmd.Parameters.AddWithValue("@Event_Date", this.pEvent_Date);
                                                                                        
            cmd.Parameters.AddWithValue("@Is_Enabled", this.pIs_Enabled);      
            cmd.Parameters.AddWithValue("@Edited_By", this.pEdited_By);
            cmd.Parameters.AddWithValue("@Date_Edited", DateTime.Now.ToShortDateString());


            connection.Open();
            cmd.ExecuteNonQuery();
        }

    }

    public void mDelete()
    {
        sb = sb.Remove(0, sb.Length);
        sb.Append(" update tbl_Event_Multiple ");
        sb.Append(" Set Is_Deleted = 1 ");
        sb.Append(" where Event_Multiple_id = @Event_Multiple_id ");

        string strConnectionString = ConfigurationManager.AppSettings["CMServer"];
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@Event_Multiple_id", this.pEvent_Multiple_id);

            connection.Open();
            cmd.ExecuteNonQuery();
        }


    }      

    public void mDelete_Physically(int mID)
    {
        sb = sb.Remove(0, sb.Length);
        sb.Append(" Delete from tbl_Event_Multiple ");
        sb.Append(" where Event_Multiple_id = @Event_Multiple_id ");

        string strConnectionString = ConfigurationManager.AppSettings["CMServer"];
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@Event_Multiple_id", mID);

            connection.Open();
            cmd.ExecuteNonQuery();
        }


    }

    public void mDelete_Physically_ByEventID(int mID)
    {
        sb = sb.Remove(0, sb.Length);
        sb.Append(" Delete from tbl_Event_Multiple ");
        sb.Append(" where Event_id = @Event_id ");

        string strConnectionString = ConfigurationManager.AppSettings["CMServer"];
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@Event_id", mID);

            connection.Open();
            cmd.ExecuteNonQuery();
        }


    }

    public DataTable mGet_UpcomingDates(int eventID)
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"];

        //string commandString = "select * from tbl_Event_Multiple where Event_id=@eventID and Event_Date  >= CONVERT(DATE,GETDATE())  and CONVERT(TIME,Start_time) >= (SELECT CONVERT(TIME, GETDATE())) and Is_Deleted = 0";
        string commandString = @"select[Event_id],[Start_Time],[End_Time],[Event_Date] from tbl_Event_Multiple 
                                where Event_id = @eventID and Event_Date > CONVERT(DATE, GETDATE()) and Is_Deleted = 0
                                 union
                                select top(1)e1.[Event_id],e1.[Start_Time],e1.[End_Time],e1.[Event_Date] from tbl_Event_Multiple e1 inner join tbl_Event_Multiple e2 
                                on e1.[Event_id] = e2.[Event_id] and e1.[Event_id] =  @eventID and e1.Event_Date = CONVERT(DATE, GETDATE()) and e1.Is_Deleted = 0
                                and CONVERT(TIME, e1.Start_time) > (SELECT CONVERT(TIME, GETDATE()))";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@eventID", eventID);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds, "table1");
        }

        return ds.Tables[0];
    }

    #endregion



}
