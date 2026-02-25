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
/// Summary description for cRegistration_Attendees
/// </summary>
public class cRegistration_Attendees
{
	StringBuilder sb;
	public cRegistration_Attendees()
	{

		sb = new StringBuilder(60);


		this.pIs_Deleted = 0;
		this.pIs_Enabled = 1;
		this.pDate_Created = DateTime.Now;
		this.pCreated_By = -1;
	}

	#region Variables

	private int Attendee_id;
	private int Registration_id;
    public DateTime Specificdate = DateTime.Now;
	private string Reg_First_Name;
	private string Reg_Last_Name;
	private string Reg_Address;
	private string Reg_City;
	private string Reg_Province;
	private string Reg_Postel_Code;
	private string Reg_Email;
	private string Reg_Home_Phone;
	private string Reg_Mobile_Phone;
	private string Reg_Terms_Agreed;
	//private string Reg_Department;
	
	private int Is_Deleted;
	private int Is_Enabled;
	private DateTime Date_Created;
	private DateTime Date_Edited;
	private int Created_By;
	private int Edited_By;
	#endregion

	#region Properties

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

	public string pReg_First_Name
	{
		set
		{
			this.Reg_First_Name = value;
		}
		get
		{
			return this.Reg_First_Name;
		}
	}

	public string pReg_Last_Name
	{
		set
		{
			this.Reg_Last_Name = value;
		}
		get
		{
			return this.Reg_Last_Name;
		}
	}

	public string pReg_Address
	{
		set
		{
			this.Reg_Address = value;
		}
		get
		{
			return this.Reg_Address;
		}
	}

	public string pReg_City
	{
		set
		{
			this.Reg_City = value;
		}
		get
		{
			return this.Reg_City;
		}
	}

	public string pReg_Province
	{
		set
		{
			this.Reg_Province = value;
		}
		get
		{
			return this.Reg_Province;
		}
	}

	public string pReg_Postel_Code
	{
		set
		{
			this.Reg_Postel_Code = value;
		}
		get
		{
			return this.Reg_Postel_Code;
		}
	}

	public string pReg_Email
	{
		set
		{
			this.Reg_Email = value;
		}
		get
		{
			return this.Reg_Email;
		}
	}

	/*public string pReg_Department
	{
		set
		{
			this.Reg_Department = value;
		}
		get
		{
			return this.Reg_Department;
		}
	}*/

	public string pReg_Home_Phone
	{
		set
		{
			this.Reg_Home_Phone = value;
		}
		get
		{
			return this.Reg_Home_Phone;
		}
	}


	public string pReg_Mobile_Phone
	{
		set
		{
			this.Reg_Mobile_Phone = value;
		}
		get
		{
			return this.Reg_Mobile_Phone;
		}
	}

	public string pReg_Terms_Agreed
	{
		set
		{
			this.Reg_Terms_Agreed = value;
		}
		get
		{
			return this.Reg_Terms_Agreed;
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
		string commandString = "select  * from tbl_Registration_Attendees where Is_deleted = 0";
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
		string commandString = "select  * from tbl_Registration_Attendees where Is_deleted = 0 and Is_enabled = 1";
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

	public int mGet_NextID()
	{
		string strConnectionString = ConfigurationManager.AppSettings["CMServer"];
		string commandString = "select IDENT_CURRENT('tbl_Registration_Attendees') + IDENT_INCR('tbl_Registration_Attendees')";
		DataSet ds = new DataSet();

		using (SqlConnection connection = new SqlConnection(strConnectionString))
		{
			SqlCommand cmd = new SqlCommand(commandString, connection);
			connection.Open();
			//SqlDataAdapter da = new SqlDataAdapter();
			//da.SelectCommand = cmd;

			//da.Fill(ds, "table1");

			int i = Convert.ToInt32(cmd.ExecuteScalar());

			return i;
		}

		


	}

	public DataTable mGet_One(int mID)
	{
		string strConnectionString = ConfigurationManager.AppSettings["CMServer"];
		string commandString = "select * from tbl_Registration_Attendees where Attendee_id = @mID and Is_Deleted = 0";
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
		sb.Append(" insert into  tbl_Registration_Attendees (Registration_id,Reg_First_Name,Reg_Last_Name,Reg_Address,Reg_City,Reg_Province,Reg_Postel_Code,Reg_Email,Reg_Home_Phone,Reg_Mobile_Phone,Reg_Terms_Agreed, Is_Deleted, Is_Enabled, Created_By, Date_Created,Specificdate) ");
		sb.Append(" values									(@Registration_id, @Reg_First_Name, @Reg_Last_Name, @Reg_Address, @Reg_City, @Reg_Province, @Reg_Postel_Code, @Reg_Email, @Reg_Home_Phone, @Reg_Mobile_Phone, @Reg_Terms_Agreed, @Is_Deleted, @Is_Enabled, @Created_By, @Date_Created, @Specificdate)");
		sb.Append(" SELECT Attendee_id FROM tbl_Registration_Attendees WHERE (Attendee_id = SCOPE_IDENTITY()) ");
		string strConnectionString = ConfigurationManager.AppSettings["CMServer"];
		string commandString = sb.ToString();


		using (SqlConnection connection = new SqlConnection(strConnectionString))
		{
			SqlCommand cmd = new SqlCommand(commandString, connection);
			cmd.Parameters.AddWithValue("@Registration_id", this.pRegistration_id);
			cmd.Parameters.AddWithValue("@Reg_First_Name", this.pReg_First_Name);
			cmd.Parameters.AddWithValue("@Reg_Last_Name", this.pReg_Last_Name);
			cmd.Parameters.AddWithValue("@Reg_Address", this.pReg_Address);
			cmd.Parameters.AddWithValue("@Reg_City", this.pReg_City);
			cmd.Parameters.AddWithValue("@Reg_Province", this.pReg_Province);
			cmd.Parameters.AddWithValue("@Reg_Postel_Code", this.pReg_Postel_Code);
			cmd.Parameters.AddWithValue("@Reg_Email", this.pReg_Email);
			cmd.Parameters.AddWithValue("@Reg_Home_Phone", this.pReg_Home_Phone);
			cmd.Parameters.AddWithValue("@Reg_Mobile_Phone", this.pReg_Mobile_Phone);
			cmd.Parameters.AddWithValue("@Reg_Terms_Agreed", this.pReg_Terms_Agreed);
			//cmd.Parameters.AddWithValue("@Reg_Department", this.Reg_Department);
			cmd.Parameters.AddWithValue("@Is_Deleted", this.pIs_Deleted);
			cmd.Parameters.AddWithValue("@Is_Enabled", this.pIs_Enabled);
			cmd.Parameters.AddWithValue("@Created_By", this.pCreated_By);
			cmd.Parameters.AddWithValue("@Date_Created", DateTime.Now.ToShortDateString());
            cmd.Parameters.AddWithValue("@Specificdate", Specificdate.ToShortDateString());


			connection.Open();
			int i = Convert.ToInt32(cmd.ExecuteScalar());

			return i;
		}

	}

	public void mEdit()
	{
		sb = sb.Remove(0, sb.Length);
		sb.Append(" update tbl_Registration_Attendees ");
		sb.Append(" Set Registration_id = @Registration_id, ");
		sb.Append(" Reg_First_Name = @Reg_First_Name, ");
		sb.Append(" Reg_Last_Name = @Reg_Last_Name, ");
		sb.Append(" Reg_Address = @Reg_Address, ");
		sb.Append(" Reg_City = @Reg_City, ");
		sb.Append(" Reg_Province = @Reg_Province, ");
		sb.Append(" Reg_Postel_Code = @Reg_Postel_Code, ");
		sb.Append(" Reg_Email = @Reg_Email, ");
		sb.Append(" Reg_Home_Phone = @Reg_Home_Phone, ");
		sb.Append(" Reg_Mobile_Phone = @Reg_Mobile_Phone, ");
		sb.Append(" Reg_Terms_Agreed = @Reg_Terms_Agreed, ");
		//sb.Append(" Reg_Department = @Reg_Department, ");
		sb.Append(" Is_Enabled = @Is_Enabled, ");
		sb.Append(" Edited_By = @Edited_By, ");
		sb.Append(" Date_Edited = @Date_Edited, Specificdate = @Specificdate ");
		sb.Append(" where Attendee_id = @Attendee_id ");
		string strConnectionString = ConfigurationManager.AppSettings["CMServer"];
		string commandString = sb.ToString();


		using (SqlConnection connection = new SqlConnection(strConnectionString))
		{
			SqlCommand cmd = new SqlCommand(commandString, connection);

			cmd.Parameters.AddWithValue("@Attendee_id", this.pAttendee_id);
			cmd.Parameters.AddWithValue("@Registration_id", this.pRegistration_id);
			cmd.Parameters.AddWithValue("@Reg_First_Name", this.pReg_First_Name);
			cmd.Parameters.AddWithValue("@Reg_Last_Name", this.pReg_Last_Name);
			cmd.Parameters.AddWithValue("@Reg_Address", this.pReg_Address);
			cmd.Parameters.AddWithValue("@Reg_City", this.pReg_City);
			cmd.Parameters.AddWithValue("@Reg_Province", this.pReg_Province);
			cmd.Parameters.AddWithValue("@Reg_Postel_Code", this.pReg_Postel_Code);
			cmd.Parameters.AddWithValue("@Reg_Email", this.pReg_Email);
			cmd.Parameters.AddWithValue("@Reg_Home_Phone", this.pReg_Home_Phone);
			cmd.Parameters.AddWithValue("@Reg_Mobile_Phone", this.pReg_Mobile_Phone);
			cmd.Parameters.AddWithValue("@Reg_Terms_Agreed", this.pReg_Terms_Agreed);
			//cmd.Parameters.AddWithValue("@Reg_Department", this.Reg_Department);
			cmd.Parameters.AddWithValue("@Is_Enabled", this.pIs_Enabled);
			cmd.Parameters.AddWithValue("@Edited_By", this.pEdited_By);
			cmd.Parameters.AddWithValue("@Date_Edited", DateTime.Now.ToShortDateString());
            cmd.Parameters.AddWithValue("@Specificdate", Specificdate.ToShortDateString());


			connection.Open();
			cmd.ExecuteNonQuery();
		}

	}

	public void mDelete()
	{
		sb = sb.Remove(0, sb.Length);
		sb.Append(" update tbl_Registration_Attendees ");
		sb.Append(" Set Is_Deleted = 1 ");
		sb.Append(" where Attendee_id = @Attendee_id ");

		string strConnectionString = ConfigurationManager.AppSettings["CMServer"];
		string commandString = sb.ToString();


		using (SqlConnection connection = new SqlConnection(strConnectionString))
		{
			SqlCommand cmd = new SqlCommand(commandString, connection);
			cmd.Parameters.AddWithValue("@Attendee_id", this.pAttendee_id);

			connection.Open();
			cmd.ExecuteNonQuery();
		}


	}

	public void mDelete_Physically(int mID)
	{
		sb = sb.Remove(0, sb.Length);
		sb.Append(" Delete from tbl_Registration_Attendees ");
		sb.Append(" where Attendee_id = @Attendee_id ");

		string strConnectionString = ConfigurationManager.AppSettings["CMServer"];
		string commandString = sb.ToString();


		using (SqlConnection connection = new SqlConnection(strConnectionString))
		{
			SqlCommand cmd = new SqlCommand(commandString, connection);
			cmd.Parameters.AddWithValue("@Attendee_id", mID);

			connection.Open();
			cmd.ExecuteNonQuery();
		}


	}

	#endregion



}
