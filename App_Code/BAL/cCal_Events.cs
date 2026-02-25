using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;

/// <summary>
/// Summary description for cCal_Events
/// </summary>
public class cCal_Events
{
    StringBuilder sb;
    public cCal_Events()
    {
        sb = new StringBuilder(60);

        this.pIs_Deleted = 0;
        this.pIs_Enabled = 1;
        this.pDate_Created = DateTime.Now;
        this.pCreated_By = -1;

    }

    #region Variables

    private int Cal_Event_id;
    private int Cal_id;
    private int Event_id;

    private int Is_Deleted;
    private int Is_Enabled;
    private DateTime Date_Created;
    private DateTime Date_Edited;
    private int Created_By;
    private int Edited_By;


    #endregion

    #region Properties

    public int pCal_Event_id
    {
        set
        {
            this.Cal_Event_id = value;
        }
        get
        {
            return this.Cal_Event_id;
        }
    }

    public int pCal_id
    {
        set
        {
            this.Cal_id = value;
        }
        get
        {
            return this.Cal_id;
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

    public DataTable mGet_All()
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"];
        //string commandString = "select  * from tbl_Cal_Events where Is_deleted = 0";
        string commandString = "select  * from tbl_Cal_Events";
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


    public DataTable mGet_one(string mCal_Event_Id)
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"];
        //string commandString = "select  * from tbl_Cal_Events where Is_deleted = 0";
        string commandString = "select  * from tbl_Cal_Events where cal_event_Id = @Cal_Event_Id";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@Cal_Event_Id", mCal_Event_Id);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds, "table1");
        }

        return ds.Tables[0];
    }


    public DataTable mGet_ByCalID(string mCalid)
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"];
        // string commandString = "select  * from tbl_Cal_Events where cal_id = @mCalid";
        string commandString = "select * from tbl_cal_events a, tbl_events e where a.event_id = e.event_id and cal_id = @mCalid";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@mCalid", mCalid);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds, "table1");
        }

        return ds.Tables[0];
    }

    public DataTable mGet_ByEventID(string mEventid) //Is_deleted = 0
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"];
        // string commandString = "select  * from tbl_Cal_Events where cal_id = @mCalid";
        string commandString = "select  b.* from tbl_Cal_Events a, tbl_calendar b where b.Is_Deleted = 0 and a.event_Id =@mEventid and a.cal_id = b.cal_id";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@mEventid", mEventid);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds, "table1");
        }

        return ds.Tables[0];
    }

    //public DataTable mGet_ByEventID(string mEventid)
    //{
    //    string strConnectionString = ConfigurationManager.AppSettings["CMServer"];
    //    // string commandString = "select  * from tbl_Cal_Events where cal_id = @mCalid";
    //    string commandString = "select  b.* from tbl_Cal_Events a, tbl_calendar b where a.event_Id =@mEventid and a.cal_id = b.cal_id";
    //    DataSet ds = new DataSet();

    //    using (SqlConnection connection = new SqlConnection(strConnectionString))
    //    {
    //        SqlCommand cmd = new SqlCommand(commandString, connection);
    //        cmd.Parameters.AddWithValue("@mEventid", mEventid);
    //        connection.Open();
    //        SqlDataAdapter da = new SqlDataAdapter();
    //        da.SelectCommand = cmd;

    //        da.Fill(ds, "table1");
    //    }

    //    return ds.Tables[0];
    //}

    public DataTable mGet_Calendar_Exclude_EventID(string mEventid)  //Is_deleted = 0
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"];
        // string commandString = "select  * from tbl_Cal_Events where cal_id = @mCalid";
        string commandString = "select * from tbl_calendar where Is_Deleted = 0 and cal_id not in (select b.cal_id from tbl_Cal_Events a, tbl_calendar b where a.event_Id =@mEventid and a.cal_id = b.cal_id)";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@mEventid", mEventid);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds, "table1");
        }

		return ds.Tables[0];
	}


	public DataTable mGet_ByCalID_PlusEvents(string mCalid)	 //Is_deleted = 0
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"];
		////string commandString = "select *,g.ilocation,l.name as lang from tbl_cal_events a, tbl_events e, tbl_Groups g,languages l where l.id=e.language and g.id=e.group_id and a.event_id = e.event_id and e.Is_Deleted = 0  and cal_id = @mCalid order by Approved, Event_Date_Start";
		string commandString = "select isnull(Approved, 1) Is_Approved, *,g.ilocation,l.name as lang from tbl_cal_events a, tbl_events e, tbl_Groups g,languages l where l.id=e.language and g.id=e.group_id and a.event_id = e.event_id and e.Is_Deleted = 0  and cal_id = @mCalid and e.language = 1 order by Approved, Event_Date_Start";
        
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@mCalid", mCalid);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds, "table1");
        }

        return ds.Tables[0];
    }

    public DataTable mGet_ByCalID_PlusEvents_multiple(string mCalid, string lang) //Is_deleted = 0
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"];
        string commandString = "select distinct e.*, g.ilocation from tbl_cal_events a, tbl_events e, tbl_Groups g where g.id=e.group_id and a.event_id = e.event_id and e.Is_Enabled = 1 and e.Is_Deleted = 0  and   cal_id in (" + mCalid + ") and e.language=@Lang";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@mCalid", mCalid);
            cmd.Parameters.AddWithValue("@Lang", lang);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds, "table1");
        }

        return ds.Tables[0];
    }


    public DataTable mGet_ByCalID_PlusEvents_multiple_ByGroupid(string mCalid, string mGroupid, string lang) //Is_deleted = 0
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"];
        string commandString = "select distinct e.*, g.ilocation from tbl_cal_events a, tbl_events e, tbl_Groups g where g.id=e.group_id and a.event_id = e.event_id and e.Is_Enabled = 1 and e.Is_Deleted = 0  and   cal_id in (" + mCalid + ") and e.language=@Lang  and Group_id = " + mGroupid;
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@mCalid", mCalid);
            cmd.Parameters.AddWithValue("@mGroupid", mGroupid);
            cmd.Parameters.AddWithValue("@Lang", lang);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds, "table1");
        }

        return ds.Tables[0];
    }


    public void mDelete(int Cal_Event_id)
    {
        sb = sb.Remove(0, sb.Length);
        sb.Append(" declare @submissionid int update tbl_Events set Is_Deleted = 1, @submissionid = submissionid where Event_id = (select Event_id from tbl_Cal_Events where Cal_Event_id = @Cal_Event_id) ");
        sb.Append(" delete from tbl_Cal_Events ");
        sb.Append(" where Cal_Event_id = @Cal_Event_id");
        sb.Append(" update tbl_EventsSuggested set deleted = 1 where SubmissionId=@submissionid");
       

        string strConnectionString = ConfigurationManager.AppSettings["CMServer"];
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@Cal_Event_id", Cal_Event_id);

            connection.Open();
            cmd.ExecuteNonQuery();
        }



    }

    public void mDelete_ByEventid(int mEvent_id)
    {
        sb = sb.Remove(0, sb.Length);
        sb.Append(" delete from tbl_Cal_Events ");
        sb.Append(" where Event_id = @mEvent_id");


        string strConnectionString = ConfigurationManager.AppSettings["CMServer"];
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@mEvent_id", mEvent_id);

            connection.Open();
            cmd.ExecuteNonQuery();
        }



    }

    public void mAdd()
    {

        sb = sb.Remove(0, sb.Length);
        //sb.Append(" insert into  tbl_Cal_Events (Cal_id, Event_id, Is_Deleted, Is_Enabled, Created_By, Date_Created) ");
        //sb.Append(" values   (@Cal_id, @Event_id, @Is_Deleted, @Is_Enabled, @Created_By, @Date_Created)");

        sb.Append(" insert into  tbl_Cal_Events (Cal_id, Event_id) ");
        sb.Append(" values   (@Cal_id, @Event_id)");
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"];
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@Cal_id", this.pCal_id);
            cmd.Parameters.AddWithValue("@Event_id", this.pEvent_id);

            //cmd.Parameters.AddWithValue("@Is_Deleted", this.pIs_Deleted);
            //cmd.Parameters.AddWithValue("@Is_Enabled", this.pIs_Enabled);
            //cmd.Parameters.AddWithValue("@Created_By", this.pCreated_By);
            //cmd.Parameters.AddWithValue("@Date_Created", DateTime.Now.ToShortDateString());

            connection.Open();
            cmd.ExecuteNonQuery();
        }

    }

    //public void mEnabled(String mValue)
    //{
    //    sb = sb.Remove(0, sb.Length);
    //    sb.Append(" update tbl_Events ");
    //    sb.Append(" Set Is_Enabled = @mValue ");
    //    sb.Append(" where Event_id = @Event_id ");

    //    string strConnectionString = ConfigurationManager.AppSettings["CMServer"];
    //    string commandString = sb.ToString();


    //    using (SqlConnection connection = new SqlConnection(strConnectionString))
    //    {
    //        SqlCommand cmd = new SqlCommand(commandString, connection);
    //        cmd.Parameters.AddWithValue("@Event_id", this.pEvent_id);
    //        cmd.Parameters.AddWithValue("@mValue", mValue);


    //        connection.Open();
    //        cmd.ExecuteNonQuery();
    //    }





    //}


    public DataTable mGet_Events_NotinCal(string mCalid)
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"];
        string commandString = "select * from tbl_events a where  a.Is_Deleted = 0 and a.event_id not in (select Event_id from tbl_cal_events a where a.cal_id = @mCalid)";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@mCalid", mCalid);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds, "table1");
        }

        return ds.Tables[0];
    }

    public string mCopy(int Cal_Event_id)
    {
        sb = sb.Remove(0, sb.Length);
        sb.Append(" Declare @Cal_id int Declare @Event_id int Declare @Event_id_New int ");
        sb.Append(" select @Cal_id = Cal_id, @Event_id = Event_id from tbl_Cal_Events where Cal_Event_id = @Cal_Event_id ");

        sb.Append(" insert into tbl_Events([Group_id],[Event_Name],[Event_Contact],[Event_Desc],[Event_Date_Start],[Event_Date_End],[Event_Start_Time],[Event_End_Time],[Event_Is_Multiple],[Event_Type],[Event_Recur_Type],[Event_Recur_Start_Date],[Event_Recur_End_Date],[Event_Venue],[Event_Location_Detail],[Event_Attach_File],[Event_Address],[Event_City],[Event_Province],[Event_Postel_Code],[Event_Keywords],[Event_Registration_Required],[Is_Featured],[Is_Deleted],[Is_Enabled],[Created_By],[Date_Created],[language],[Event_Comments],[BannerFile],[BackgroundBannerPosition],[BackgroundBannerPosition_Horizontal],[ButtonText],[ButtonUrl]) ");
        sb.Append(" select [Group_id],[Event_Name] + ' - Copy',[Event_Contact],[Event_Desc],[Event_Date_Start],[Event_Date_End],[Event_Start_Time],[Event_End_Time],[Event_Is_Multiple],[Event_Type],[Event_Recur_Type],[Event_Recur_Start_Date],[Event_Recur_End_Date],[Event_Venue],[Event_Location_Detail],[Event_Attach_File],[Event_Address],[Event_City],[Event_Province],[Event_Postel_Code],[Event_Keywords],[Event_Registration_Required],[Is_Featured],[Is_Deleted],[Is_Enabled],@Created_By,getdate(),[language],[Event_Comments],[BannerFile],[BackgroundBannerPosition],[BackgroundBannerPosition_Horizontal],[ButtonText],[ButtonUrl] from tbl_Events where Event_id = @Event_id ");
        sb.Append(" select @Event_id_New = SCOPE_IDENTITY() ");

        sb.Append(" insert into tbl_Events_Images([Event_id],[Photo],[MIMEType],[PhotoAltText]) ");
        sb.Append(" select @Event_id_New,[Photo],[MIMEType],[PhotoAltText] from tbl_Events_Images where Event_id = @Event_id ");

        sb.Append(" insert into tbl_Event_Dynamic([Event_id],[Event_Text_Label],[Event_Text_Value],[Is_Deleted],[Is_Enabled],[Created_By],[Date_Created]) ");
        sb.Append(" select @Event_id_New,[Event_Text_Label],[Event_Text_Value],[Is_Deleted],[Is_Enabled],@Created_By,getdate() from tbl_Event_Dynamic where Event_id = @Event_id ");

        sb.Append(" DECLARE @link TABLE (Event_Multiple_id int, Event_Multiple_id_New int) ");
        //sb.Append(" insert into tbl_Event_Multiple([Event_id],[Day_No],[Start_Time],[End_Time],[Event_Date],[Is_Deleted],[Is_Enabled],[Created_By],[Date_Created]) ");
        //sb.Append(" select @Event_id_New,[Day_No],[Start_Time],[End_Time],[Event_Date],[Is_Deleted],[Is_Enabled],[Created_By],getdate() from tbl_Event_Multiple where Event_id = @Event_id ");
        sb.Append(" MERGE INTO tbl_Event_Multiple e ");
        sb.Append(" USING ( ");
        sb.Append("     select Event_Multiple_id,[Day_No],[Start_Time],[End_Time],[Event_Date],[Is_Deleted],[Is_Enabled] from tbl_Event_Multiple where Event_id = @Event_id ");
        sb.Append(" ) i ON 1 = 0 ");
        sb.Append(" WHEN NOT MATCHED THEN ");
        sb.Append(" INSERT ([Event_id],[Day_No],[Start_Time],[End_Time],[Event_Date],[Is_Deleted],[Is_Enabled],[Created_By],[Date_Created]) ");
        sb.Append(" VALUES( @Event_id_New,i.[Day_No],i.[Start_Time],i.[End_Time],i.[Event_Date],i.[Is_Deleted],i.[Is_Enabled],@Created_By,getdate() )");
        sb.Append(" OUTPUT i.Event_Multiple_id, inserted.Event_Multiple_id INTO @link(Event_Multiple_id, Event_Multiple_id_New) ; ");

        sb.Append(" insert into tbl_Event_Multiple_Time_Recur([Event_id],[Event_Multiple_id],[Start_Time],[End_Time],[Event_Date],[Is_Deleted],[Is_Enabled],[Created_By],[Date_Created]) ");
        sb.Append(" select @Event_id_New,l.Event_Multiple_id_New,m.[Start_Time],m.[End_Time],m.[Event_Date],m.[Is_Deleted],m.[Is_Enabled],@Created_By,getdate() from tbl_Event_Multiple_Time_Recur m inner join @link l on m.Event_Multiple_id = l.Event_Multiple_id where m.Event_id = @Event_id ");

        sb.Append(" insert into tbl_Event_Time_Recur([Event_id],[Start_Time],[End_Time],[Event_Date],[Is_Deleted],[Is_Enabled],[Created_By],[Date_Created]) ");
        sb.Append(" select @Event_id_New,[Start_Time],[End_Time],[Event_Date],[Is_Deleted],[Is_Enabled],@Created_By,getdate() from tbl_Event_Time_Recur where Event_id = @Event_id ");

        sb.Append(" insert into tbl_Event_Registration([Event_id],[Registration_by],[Registration_Fees],[Registration_Spots_TotalCount],[Registration_Spots_Remaining],[Registration_Terms_Required],[Registration_Terms_Conditions],[Is_Deleted],[Is_Enabled],[Created_By],[Date_Created],[language]) ");
        //sb.Append(" select @Event_id_New,[Registration_by],[Registration_Fees],[Registration_Spots_TotalCount],[Registration_Spots_Remaining],[Registration_Terms_Required],[Registration_Terms_Conditions],[Is_Deleted],[Is_Enabled],@Created_By,getdate(),[language] from tbl_Event_Registration where Event_id = @Event_id ");
        sb.Append(" select @Event_id_New,[Registration_by],[Registration_Fees],[Registration_Spots_TotalCount],[Registration_Spots_TotalCount],[Registration_Terms_Required],[Registration_Terms_Conditions],[Is_Deleted],[Is_Enabled],@Created_By,getdate(),[language] from tbl_Event_Registration where Event_id = @Event_id ");

        sb.Append(" insert into tbl_Cal_Events(Cal_id, Event_id) ");
        sb.Append(" select @Cal_id, @Event_id_New ");

        sb.Append(" select @Event_id_New ");


        string strConnectionString = ConfigurationManager.AppSettings["CMServer"];
        string commandString = sb.ToString();

        string newid = "";
        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@Cal_Event_id", Cal_Event_id);
            cmd.Parameters.AddWithValue("@Created_By", this.Created_By);

            connection.Open();
            newid = cmd.ExecuteScalar().ToString();
            connection.Close();
        }

        return newid;
    }

    #endregion

}
