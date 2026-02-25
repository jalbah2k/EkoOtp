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
/// Summary description for cRegistration_Attendees_CreditCardInfo
/// </summary>

public class cRegistration_Attendees_CreditCardInfo
{
	StringBuilder sb;
	public cRegistration_Attendees_CreditCardInfo()
	{
		sb = new StringBuilder(60);


		this.pIs_Deleted = 0;
		this.pIs_Enabled = 1;
		this.pDate_Created = DateTime.Now;
		this.pCreated_By = -1;
	}


	#region Variables

	private int Card_id;
	private int Attendee_id;
	private string Receipt_id;
	private string Card_Number;
	private string Card_Type;
	private string Card_Expiry_Date;
	private double Card_Amount;
	

	private int Is_Deleted;
	private int Is_Enabled;
	private DateTime Date_Created;
	private DateTime Date_Edited;
	private int Created_By;
	private int Edited_By;
	#endregion

	#region Properties

	public int pCard_id
	{
		set
		{
			this.Card_id = value;
		}
		get
		{
			return this.Card_id;
		}
	}


	public int pAttendee_id
	{
		set
		{
			this.Attendee_id = value;
		}
		get
		{
			return this.Attendee_id;
		}
	}

	public string pReceipt_id
	{
		set
		{
			this.Receipt_id = value;
		}
		get
		{
			return this.Receipt_id;
		}
	}

	public string pCard_Number
	{
		set
		{
			this.Card_Number = value;
		}
		get
		{
			return this.Card_Number;
		}
	}

	public string pCard_Type
	{
		set
		{
			this.Card_Type = value;
		}
		get
		{
			return this.Card_Type;
		}
	}

	public string pCard_Expiry_Date
	{
		set
		{
			this.Card_Expiry_Date = value;
		}
		get
		{
			return this.Card_Expiry_Date;
		}
	}

	public double pCard_Amount
	{
		set
		{
			this.Card_Amount = value;
		}
		get
		{
			return this.Card_Amount;
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
		string commandString = "select  * from tbl_Registration_Attendees_CreditCardInfo where Is_deleted = 0";
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
		string commandString = "select  * from tbl_Registration_Attendees_CreditCardInfo where Is_deleted = 0 and Is_enabled = 1";
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
		string commandString = "select * from tbl_Registration_Attendees_CreditCardInfo where Card_id = @mID and Is_Deleted = 0";
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


	public void mAdd()
	{

		sb = sb.Remove(0, sb.Length);
		sb.Append(" insert into  tbl_Registration_Attendees_CreditCardInfo (Attendee_id, Receipt_id, Card_Number, Card_Type, Card_Expiry_Date, Card_Amount, Is_Deleted, Is_Enabled, Created_By, Date_Created) ");
		sb.Append(" values   (@Attendee_id, @Receipt_id, @Card_Number, @Card_Type, @Card_Expiry_Date, @Card_Amount, @Is_Deleted, @Is_Enabled, @Created_By, @Date_Created)");
		// sb.Append(" SELECT Event_id FROM tbl_Events WHERE (Event_id = SCOPE_IDENTITY()) ");
		string strConnectionString = ConfigurationManager.AppSettings["CMServer"];
		string commandString = sb.ToString();


		using (SqlConnection connection = new SqlConnection(strConnectionString))
		{
			SqlCommand cmd = new SqlCommand(commandString, connection);
			cmd.Parameters.AddWithValue("@Attendee_id", this.pAttendee_id);
			cmd.Parameters.AddWithValue("@Receipt_id", this.pReceipt_id);
			cmd.Parameters.AddWithValue("@Card_Number", this.pCard_Number);
			cmd.Parameters.AddWithValue("@Card_Type", this.pCard_Type);
			cmd.Parameters.AddWithValue("@Card_Expiry_Date", this.pCard_Expiry_Date);
			cmd.Parameters.AddWithValue("@Card_Amount", this.pCard_Amount);
			
			cmd.Parameters.AddWithValue("@Is_Deleted", this.pIs_Deleted);
			cmd.Parameters.AddWithValue("@Is_Enabled", this.pIs_Enabled);
			cmd.Parameters.AddWithValue("@Created_By", this.pCreated_By);
			cmd.Parameters.AddWithValue("@Date_Created", DateTime.Now.ToShortDateString());


			connection.Open();
			cmd.ExecuteNonQuery();

			//return i;
		}

	}

	public void mEdit()
	{
		sb = sb.Remove(0, sb.Length);
		sb.Append(" update tbl_Registration_Attendees_CreditCardInfo ");
		sb.Append(" Set Attendee_id = @Attendee_id, ");
		sb.Append(" Receipt_id = @Receipt_id, ");
		sb.Append(" Card_Number = @Card_Number, ");
		sb.Append(" Card_Type = @Card_Type, ");
		sb.Append(" Card_Expiry_Date = @Card_Expiry_Date, ");
		sb.Append(" Card_Amount = @Card_Amount, ");		

		sb.Append(" Is_Enabled = @Is_Enabled, ");
		sb.Append(" Edited_By = @Edited_By, ");
		sb.Append(" Date_Edited = @Date_Edited ");
		sb.Append(" where Card_id = @Card_id ");
		string strConnectionString = ConfigurationManager.AppSettings["CMServer"];
		string commandString = sb.ToString();


		using (SqlConnection connection = new SqlConnection(strConnectionString))
		{
			SqlCommand cmd = new SqlCommand(commandString, connection);

			cmd.Parameters.AddWithValue("@Card_id", this.pCard_id);
			cmd.Parameters.AddWithValue("@Attendee_id", this.pAttendee_id);
			cmd.Parameters.AddWithValue("@Receipt_id", this.pReceipt_id);
			cmd.Parameters.AddWithValue("@Card_Number", this.pCard_Number);
			cmd.Parameters.AddWithValue("@Card_Type", this.pCard_Type);
			cmd.Parameters.AddWithValue("@Card_Expiry_Date", this.pCard_Expiry_Date);
			cmd.Parameters.AddWithValue("@Card_Amount", this.pCard_Amount);


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
		sb.Append(" update tbl_Registration_Attendees_CreditCardInfo ");
		sb.Append(" Set Is_Deleted = 1 ");
		sb.Append(" where Card_id = @Card_id ");

		string strConnectionString = ConfigurationManager.AppSettings["CMServer"];
		string commandString = sb.ToString();


		using (SqlConnection connection = new SqlConnection(strConnectionString))
		{
			SqlCommand cmd = new SqlCommand(commandString, connection);
			cmd.Parameters.AddWithValue("@Card_id", this.pCard_id);

			connection.Open();
			cmd.ExecuteNonQuery();
		}


	}

	public void mDelete_Physically(int mID)
	{
		sb = sb.Remove(0, sb.Length);
		sb.Append(" Delete from tbl_Registration_Attendees_CreditCardInfo ");
		sb.Append(" where Card_id = @Card_id ");

		string strConnectionString = ConfigurationManager.AppSettings["CMServer"];
		string commandString = sb.ToString();


		using (SqlConnection connection = new SqlConnection(strConnectionString))
		{
			SqlCommand cmd = new SqlCommand(commandString, connection);
			cmd.Parameters.AddWithValue("@Card_id", mID);

			connection.Open();
			cmd.ExecuteNonQuery();
		}


	}

	#endregion



}