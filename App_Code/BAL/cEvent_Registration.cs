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
/// Summary description for cEvent_Registration
/// </summary>
public class cEvent_Registration
{
	StringBuilder sb;
    public cEvent_Registration()
    {
        sb = new StringBuilder(60);

       
        this.pIs_Deleted = 0;
        this.pIs_Enabled = 1;
        this.pDate_Created = DateTime.Now;
        this.pCreated_By = -1;

        Registration_Spots_Remaining_Diff = 0;
    }

    #region Variables

    private int Registration_id;
	private int Event_id;
	private DateTime Registration_by;
	private string Registration_Fees;
	private int Registration_Spots_TotalCount;
	private int Registration_Spots_Remaining;
    private int Registration_Spots_Remaining_Diff;
    private string Registration_Terms_Required;
	private string Registration_Terms_Conditions;
	

	private int Is_Deleted;
	private int Is_Enabled;
	private DateTime Date_Created;
	private DateTime Date_Edited;
	private int Created_By;
	private int Edited_By;
	#endregion

	#region Properties

	public int pRegistration_id
	{
		set
		{
			this.Registration_id = value;
		}
		get
		{
			return this.Registration_id;
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

	public DateTime pRegistration_by
	{
		set
		{
			this.Registration_by = value;
		}
		get
		{
			return this.Registration_by;
		}
	}

	public string pRegistration_Fees
	{
		set
		{
			this.Registration_Fees = value;
		}
		get
		{
			return this.Registration_Fees;
		}
	}


	public int pRegistration_Spots_TotalCount
	{
		set
		{
			this.Registration_Spots_TotalCount = value;
		}
		get
		{
			return this.Registration_Spots_TotalCount;
		}
	}

	public int pRegistration_Spots_Remaining
	{
		set
		{
			this.Registration_Spots_Remaining = value;
		}
		get
		{
			return this.Registration_Spots_Remaining;
		}
	}

    public int pRegistration_Spots_Remaining_Diff
    {
        set
        {
            this.Registration_Spots_Remaining_Diff = value;
        }
        get
        {
            return this.Registration_Spots_Remaining_Diff;
        }
    }


    public string pRegistration_Terms_Required
	{
		set
		{
			this.Registration_Terms_Required = value;
		}
		get
		{
			return this.Registration_Terms_Required;
		}
	}

	public string pRegistration_Terms_Conditions
	{
		set
		{
			this.Registration_Terms_Conditions = value;
		}
		get
		{
			return this.Registration_Terms_Conditions;
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
		string commandString = "select  * from tbl_Event_Registration where Is_deleted = 0";
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
		string commandString = "select  * from tbl_Event_Registration where Is_deleted = 0 and Is_enabled = 1";
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
		string commandString = "select * from tbl_Event_Registration where Registration_id = @mID and Is_Deleted = 0";
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
		string commandString = "select * from tbl_Event_Registration where Event_id = @mID and Is_Deleted = 0";
        //string commandString = "select ev.*,ev.Registration_Spots_TotalCount-(select COUNT(*) from tbl_Registration_Attendees where Specificdate=@date and Registration_id = (select Registration_id from tbl_Event_Registration where Event_id = @mID and Is_Deleted = 0)) as 'Left' from tbl_Event_Registration ev where ev.Event_id = @mID and ev.Is_Deleted = 0";
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

    public DataTable mGet_ByEventID(int mID, DateTime date)
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"];
        //string commandString = "select * from tbl_Event_Registration where Event_id = @mID and Is_Deleted = 0";
        string commandString = "select ev.*,ev.Registration_Spots_TotalCount-(select COUNT(*) from tbl_Registration_Attendees where Specificdate=@date and Registration_id = (select Registration_id from tbl_Event_Registration where Event_id = @mID and Is_Deleted = 0)) as 'Left',evinfo.Event_Is_Multiple from tbl_Event_Registration ev, tbl_Events evinfo where ev.Event_id = @mID and evinfo.Event_id=@mid and ev.Is_Deleted = 0";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@mID", mID);
            cmd.Parameters.AddWithValue("@date", date);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds, "table1");
        }

        return ds.Tables[0];



    }


	public void mAdd(string elang)
	{

		sb = sb.Remove(0, sb.Length);
		sb.Append(" insert into  tbl_Event_Registration (Event_id,Registration_by,Registration_Fees,Registration_Spots_TotalCount,Registration_Spots_Remaining,Registration_Terms_Required,Registration_Terms_Conditions, Is_Deleted, Is_Enabled, Created_By, Date_Created, language) ");
		sb.Append(" values   (@Event_id, @Registration_by, @Registration_Fees, @Registration_Spots_TotalCount, @Registration_Spots_Remaining, @Registration_Terms_Required, @Registration_Terms_Conditions, @Is_Deleted, @Is_Enabled, @Created_By, @Date_Created, @Lang)");
		// sb.Append(" SELECT Event_id FROM tbl_Events WHERE (Event_id = SCOPE_IDENTITY()) ");
		string strConnectionString = ConfigurationManager.AppSettings["CMServer"];
		string commandString = sb.ToString();


		using (SqlConnection connection = new SqlConnection(strConnectionString))
		{
			SqlCommand cmd = new SqlCommand(commandString, connection);
			cmd.Parameters.AddWithValue("@Event_id", this.pEvent_id);
			cmd.Parameters.AddWithValue("@Registration_by", this.pRegistration_by);
			cmd.Parameters.AddWithValue("@Registration_Fees", this.pRegistration_Fees);
			cmd.Parameters.AddWithValue("@Registration_Spots_TotalCount", this.pRegistration_Spots_TotalCount);
			cmd.Parameters.AddWithValue("@Registration_Spots_Remaining", this.pRegistration_Spots_Remaining);
			cmd.Parameters.AddWithValue("@Registration_Terms_Required", this.pRegistration_Terms_Required);
			cmd.Parameters.AddWithValue("@Registration_Terms_Conditions", this.pRegistration_Terms_Conditions);
			
			cmd.Parameters.AddWithValue("@Is_Deleted", this.pIs_Deleted);
			cmd.Parameters.AddWithValue("@Is_Enabled", this.pIs_Enabled);
			cmd.Parameters.AddWithValue("@Created_By", this.pCreated_By);
			cmd.Parameters.AddWithValue("@Date_Created", DateTime.Now.ToShortDateString());

			cmd.Parameters.AddWithValue("@Lang", elang);


			connection.Open();
			cmd.ExecuteNonQuery();

			//return i;
		}

	}

	public void mEdit()
	{
		sb = sb.Remove(0, sb.Length);
		sb.Append(" update tbl_Event_Registration ");
		sb.Append(" Set Event_id = @Event_id, ");
		sb.Append(" Registration_by = @Registration_by, ");
		sb.Append(" Registration_Fees = @Registration_Fees, ");
		sb.Append(" Registration_Spots_TotalCount = @Registration_Spots_TotalCount, ");
        //sb.Append(" Registration_Spots_Remaining = @Registration_Spots_Remaining, ");
        if (this.pRegistration_Spots_Remaining_Diff != 0)
        {
            sb.Append(" Registration_Spots_Remaining = Registration_Spots_Remaining + @Registration_Spots_Remaining_Diff, ");
        }
        sb.Append(" Registration_Terms_Required = @Registration_Terms_Required, ");
		sb.Append(" Registration_Terms_Conditions = @Registration_Terms_Conditions, ");			

		sb.Append(" Is_Enabled = @Is_Enabled, ");
		sb.Append(" Edited_By = @Edited_By, ");
		sb.Append(" Date_Edited = @Date_Edited ");
		sb.Append(" where Registration_id = @Registration_id ");
		string strConnectionString = ConfigurationManager.AppSettings["CMServer"];
		string commandString = sb.ToString();


		using (SqlConnection connection = new SqlConnection(strConnectionString))
		{
			SqlCommand cmd = new SqlCommand(commandString, connection);

			cmd.Parameters.AddWithValue("@Registration_id", this.pRegistration_id);
			cmd.Parameters.AddWithValue("@Event_id", this.pEvent_id);
			cmd.Parameters.AddWithValue("@Registration_by", this.pRegistration_by);
			cmd.Parameters.AddWithValue("@Registration_Fees", this.pRegistration_Fees);
			cmd.Parameters.AddWithValue("@Registration_Spots_TotalCount", this.pRegistration_Spots_TotalCount);
            //cmd.Parameters.AddWithValue("@Registration_Spots_Remaining", this.pRegistration_Spots_Remaining);
            if (this.pRegistration_Spots_Remaining_Diff != 0)
            {
                cmd.Parameters.AddWithValue("@Registration_Spots_Remaining_Diff", this.pRegistration_Spots_Remaining_Diff);
            }
            cmd.Parameters.AddWithValue("@Registration_Terms_Required", this.pRegistration_Terms_Required);
			cmd.Parameters.AddWithValue("@Registration_Terms_Conditions", this.pRegistration_Terms_Conditions);
			


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
		sb.Append(" update tbl_Event_Registration ");
		sb.Append(" Set Is_Deleted = 1 ");
		sb.Append(" where Registration_id = @Registration_id ");

		string strConnectionString = ConfigurationManager.AppSettings["CMServer"];
		string commandString = sb.ToString();


		using (SqlConnection connection = new SqlConnection(strConnectionString))
		{
			SqlCommand cmd = new SqlCommand(commandString, connection);
			cmd.Parameters.AddWithValue("@Registration_id", this.pRegistration_id);

			connection.Open();
			cmd.ExecuteNonQuery();
		}


	}


	public void mUpdate_Spots_Remaining()
	{
		sb = sb.Remove(0, sb.Length);
		sb.Append(" update tbl_Event_Registration ");
		sb.Append(" Set Registration_Spots_Remaining = @Registration_Spots_Remaining ");
		sb.Append(" where Registration_id = @Registration_id ");

		string strConnectionString = ConfigurationManager.AppSettings["CMServer"];
		string commandString = sb.ToString();


		using (SqlConnection connection = new SqlConnection(strConnectionString))
		{
			SqlCommand cmd = new SqlCommand(commandString, connection);
			cmd.Parameters.AddWithValue("@Registration_id", this.pRegistration_id);
			cmd.Parameters.AddWithValue("@Registration_Spots_Remaining", this.pRegistration_Spots_Remaining);

			connection.Open();
			cmd.ExecuteNonQuery();
		}


	}

	public void mDelete_Physically(int mID)
	{
		sb = sb.Remove(0, sb.Length);
		sb.Append(" Delete from tbl_Event_Registration ");
		sb.Append(" where Registration_id = @Registration_id ");

		string strConnectionString = ConfigurationManager.AppSettings["CMServer"];
		string commandString = sb.ToString();


		using (SqlConnection connection = new SqlConnection(strConnectionString))
		{
			SqlCommand cmd = new SqlCommand(commandString, connection);
			cmd.Parameters.AddWithValue("@Registration_id", mID);

			connection.Open();
			cmd.ExecuteNonQuery();
		}


	}


	public void mDelete_Physically_ByEventID(int mID)
	{
		sb = sb.Remove(0, sb.Length);
		sb.Append(" Delete from tbl_Event_Registration ");
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

	#endregion






}
