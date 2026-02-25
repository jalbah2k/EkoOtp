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
/// Summary description for cEvents
/// </summary>
public class cEvents
{
    StringBuilder sb;
    public cEvents()
    {
        sb = new StringBuilder(60);

        Event_Recur_Start_Date = DateTime.Now;
        Event_Recur_End_Date = DateTime.Now;

        this.pIs_Deleted = 0;
        this.pIs_Enabled = 1;
        this.pDate_Created = DateTime.Now;
        this.pCreated_By = -1;

        this.pEvent_Contact = "";
    }


    #region Variables

    private int Event_id;
    private int Group_id;
    private string Event_Name;
    private string Event_Desc;
    private DateTime Event_Date_Start;
    private DateTime Event_Date_End;
    private string Event_Start_Time;
    private string Event_End_Time;
    private string Event_Is_Multiple;
    private string Event_Type;
    private string Event_Recur_Type;
    private DateTime Event_Recur_Start_Date;
    private DateTime Event_Recur_End_Date;
    private string Event_Venue;
    private string Event_Location_Detail;
    //private string Event_Fee;
    //private string Event_Register_By;
    private string Event_Attach_File;
    private string Event_Address;
    private string Event_City;
    private string Event_Province;
    private string Event_Postel_Code;
    private string Event_Keywords;
    private string Event_Email;
    private string Event_Website;
    private string Event_Registration_Required;
    private string Event_Contact;
    private string Event_Comments;

    private int Is_Featured;

    private int Is_Deleted;
    private int Is_Enabled;
    private DateTime Date_Created;
    private DateTime Date_Edited;
    private int Created_By;
    private int Edited_By;

    private bool Event_Approved;
    #endregion

    #region Properties


    public bool pEvent_Approved
    {
        set
        {
            this.Event_Approved = value;
        }
        get
        {
            return this.Event_Approved;
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

    public int pGroup_id
    {
        set
        {
            this.Group_id = value;
        }
        get
        {
            return this.Group_id;
        }
    }

    public string pEvent_Name
    {
        set
        {
            this.Event_Name = value;
        }
        get
        {
            return this.Event_Name;
        }
    }

    public string pEvent_Contact
    {
        set
        {
            this.Event_Contact = value;
        }
        get
        {
            return this.Event_Contact;
        }
    }
    public string pEvent_Comments
    {
        set
        {
            this.Event_Comments = value;
        }
        get
        {
            return this.Event_Comments;
        }
    }

    public string pEvent_Desc
    {
        set
        {
            this.Event_Desc = value;
        }
        get
        {
            return this.Event_Desc;
        }
    }

    public DateTime pEvent_Date_Start
    {
        set
        {
            this.Event_Date_Start = value;
        }
        get
        {
            return this.Event_Date_Start;
        }
    }

    public DateTime pEvent_Date_End
    {
        set
        {
            this.Event_Date_End = value;
        }
        get
        {
            return this.Event_Date_End;
        }
    }

    public string pEvent_Start_Time
    {
        set
        {
            this.Event_Start_Time = value;
        }
        get
        {
            return this.Event_Start_Time;
        }
    }


    public string pEvent_End_Time
    {
        set
        {
            this.Event_End_Time = value;
        }
        get
        {
            return this.Event_End_Time;
        }
    }


    public string pEvent_Is_Multiple
    {
        set
        {
            this.Event_Is_Multiple = value;
        }
        get
        {
            return this.Event_Is_Multiple;
        }
    }

    public string pEvent_Type
    {
        set
        {
            this.Event_Type = value;
        }
        get
        {
            return this.Event_Type;
        }
    }


    public string pEvent_Recur_Type
    {
        set
        {
            this.Event_Recur_Type = value;
        }
        get
        {
            return this.Event_Recur_Type;
        }
    }


    public DateTime pEvent_Recur_Start_Date
    {
        set
        {
            this.Event_Recur_Start_Date = value;
        }
        get
        {
            return this.Event_Recur_Start_Date;
        }
    }


    public DateTime pEvent_Recur_End_Date
    {
        set
        {
            this.Event_Recur_End_Date = value;
        }
        get
        {
            return this.Event_Recur_End_Date;
        }
    }

    public string pEvent_Venue
    {
        set
        {
            this.Event_Venue = value;
        }
        get
        {
            return this.Event_Venue;
        }
    }


    public string pEvent_Location_Detail
    {
        set
        {
            this.Event_Location_Detail = value;
        }
        get
        {
            return this.Event_Location_Detail;
        }
    }


    //    public string pEvent_Fee
    //{
    //    set
    //    {
    //        this.Event_Fee = value;
    //    }
    //    get
    //    {
    //        return this.Event_Fee;
    //    }
    //}


    //    public string pEvent_Register_By
    //{
    //    set
    //    {
    //        this.Event_Register_By = value;
    //    }
    //    get
    //    {
    //        return this.Event_Register_By;
    //    }
    //}


    public string pEvent_Attach_File
    {
        set
        {
            this.Event_Attach_File = value;
        }
        get
        {
            return this.Event_Attach_File;
        }
    }


    public string pEvent_Address
    {
        set
        {
            this.Event_Address = value;
        }
        get
        {
            return this.Event_Address;
        }
    }


    public string pEvent_City
    {
        set
        {
            this.Event_City = value;
        }
        get
        {
            return this.Event_City;
        }
    }

    public string pEvent_Province
    {
        set
        {
            this.Event_Province = value;
        }
        get
        {
            return this.Event_Province;
        }
    }


    public string pEvent_Postel_Code
    {
        set
        {
            this.Event_Postel_Code = value;
        }
        get
        {
            return this.Event_Postel_Code;
        }
    }


    public string pEvent_Keywords
    {
        set
        {
            this.Event_Keywords = value;
        }
        get
        {
            return this.Event_Keywords;
        }
    }
    public string pEvent_Email
    {
        set
        {
            this.Event_Email = value;
        }
        get
        {
            return this.Event_Email;
        }
    }
    public string pEvent_Website
    {
        set
        {
            this.Event_Website = value;
        }
        get
        {
            return this.Event_Website;
        }
    }

    public string pEvent_Registration_Required
    {
        set
        {
            this.Event_Registration_Required = value;
        }
        get
        {
            return this.Event_Registration_Required;
        }
    }


    public int pIs_Featured
    {
        set
        {
            this.Is_Featured = value;
        }
        get
        {
            return this.Is_Featured;
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

    public string pBannerFile
    {
        set; get;
    }

    public string pBackgroundBannerPosition
    {
        set; get;
    }
    public string pBackgroundBannerPosition_Horizontal
    {
        set; get;
    }

    public string pButtonText
    {
        set; get;
    }

    public string pButtonUrl
    {
        set; get;
    }

    
    #endregion

    #region Methods
    //1 = Yes
    //0 = No
    public DataTable mGet_All()
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"];
        string commandString = "select  * from tbl_Events where Is_deleted = 0";
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
        string commandString = "select  * from tbl_Events where Is_deleted = 0 and Is_enabled = 1";
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
        string commandString = "select e.*,g.name as 'Event Type' from tbl_Events e inner join tbl_Groups g on e.Group_id= g.id where Event_id = @mID and Is_Deleted = 0";
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

    public DataTable mGet_One(int mID, bool active)
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"];
        string commandString = "select e.*,g.name as 'Event Type' from tbl_Events e inner join tbl_Groups g on e.Group_id= g.id where Event_id = @mID and Is_Deleted = 0";
        if (active)
            commandString += " and Approved = 1";

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

    public int mAdd(int lang)
    {

        sb = sb.Remove(0, sb.Length);
        sb.Append(" declare @id int ");
        //sb.Append(" insert into  tbl_Events (language,Group_id, Event_Name, Event_Contact, Event_Desc, Event_Date_Start, Event_Date_End, Event_Start_Time, Event_End_Time, Event_Is_Multiple, Event_Type, Event_Recur_Type,Event_Recur_Start_Date,Event_Recur_End_Date, Event_Venue, Event_Location_Detail, Event_Attach_File, Event_Address, Event_City, Event_Province, Event_Postel_Code, Event_Keywords, Event_Registration_Required, Is_Featured, Is_Deleted, Is_Enabled, Created_By, Date_Created) ");
        //sb.Append(" values   (" + lang + ",@Group_id, @Event_Name, @Event_Contact, @Event_Desc, @Event_Date_Start, @Event_Date_End, @Event_Start_Time, @Event_End_Time, @Event_Is_Multiple, @Event_Type, @Event_Recur_Type, @Event_Recur_Start_Date, @Event_Recur_End_Date, @Event_Venue, @Event_Location_Detail, @Event_Attach_File, @Event_Address, @Event_City, @Event_Province, @Event_Postel_Code, @Event_Keywords, @Event_Registration_Required, @Is_Featured, @Is_Deleted, @Is_Enabled, @Created_By, getdate())");


        sb.Append(" insert into  tbl_Events (language,Group_id, Event_Name, Event_Contact, Event_Desc, Event_Date_Start, Event_Date_End, Event_Start_Time, Event_End_Time, Event_Is_Multiple, Event_Type, Event_Recur_Type,Event_Recur_Start_Date,Event_Recur_End_Date, Event_Venue, Event_Location_Detail, Event_Attach_File, Event_Address, Event_City, Event_Province, Event_Postel_Code, Event_Keywords, Event_Registration_Required, Is_Featured, Is_Deleted, Is_Enabled, Created_By, Date_Created, Approved, Event_Comments, BannerFile, BackgroundBannerPosition, BackgroundBannerPosition_Horizontal, ButtonText, ButtonUrl) ");
        sb.Append(" values   (" + lang + ",@Group_id, @Event_Name, @Event_Contact, @Event_Desc, @Event_Date_Start, @Event_Date_End, @Event_Start_Time, @Event_End_Time, @Event_Is_Multiple, @Event_Type, @Event_Recur_Type, @Event_Recur_Start_Date, @Event_Recur_End_Date, @Event_Venue, @Event_Location_Detail, @Event_Attach_File, @Event_Address, @Event_City, @Event_Province, @Event_Postel_Code, @Event_Keywords, @Event_Registration_Required, @Is_Featured, @Is_Deleted, @Is_Enabled, @Created_By, getdate(), @approved, @Event_Comments, @BannerFile, @BackgroundBannerPosition, @BackgroundBannerPosition_Horizontal, @ButtonText, @ButtonUrl)");


        sb.Append(" select @id = SCOPE_IDENTITY() ");
        sb.Append(" update e set Event_Attach_File=case ISNULL(Event_Attach_File,'') when '' then '' else convert(varchar,@id)+'_'+ Event_Attach_File end FROM tbl_Events e WHERE Event_id = @id ");
        sb.Append(" SELECT @id ");
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"];
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@Event_Name", this.pEvent_Name);
            cmd.Parameters.AddWithValue("@Event_Contact", this.pEvent_Contact);
            cmd.Parameters.AddWithValue("@Group_id", this.pGroup_id);
            cmd.Parameters.AddWithValue("@Event_Desc", this.pEvent_Desc);
            cmd.Parameters.AddWithValue("@Event_Date_Start", this.pEvent_Date_Start);
            cmd.Parameters.AddWithValue("@Event_Date_End", this.pEvent_Date_End);

            cmd.Parameters.AddWithValue("@Event_Start_Time", this.pEvent_Start_Time);
            cmd.Parameters.AddWithValue("@Event_End_Time", this.pEvent_End_Time);

            cmd.Parameters.AddWithValue("@Event_Is_Multiple", this.pEvent_Is_Multiple);
            cmd.Parameters.AddWithValue("@Event_Type", this.pEvent_Type);
            cmd.Parameters.AddWithValue("@Event_Recur_Type", this.pEvent_Recur_Type);
            cmd.Parameters.AddWithValue("@Event_Recur_Start_Date", this.pEvent_Recur_Start_Date);
            cmd.Parameters.AddWithValue("@Event_Recur_End_Date", this.pEvent_Recur_End_Date);

            cmd.Parameters.AddWithValue("@Event_Venue", this.pEvent_Venue);
            cmd.Parameters.AddWithValue("@Event_Location_Detail", this.pEvent_Location_Detail);
            // cmd.Parameters.AddWithValue("@Event_Fee", this.pEvent_Fee);
            // cmd.Parameters.AddWithValue("@Event_Register_By", this.pEvent_Register_By);
            cmd.Parameters.AddWithValue("@Event_Attach_File", this.pEvent_Attach_File);
            cmd.Parameters.AddWithValue("@Event_Address", this.pEvent_Address);
            cmd.Parameters.AddWithValue("@Event_City", this.pEvent_City);
            cmd.Parameters.AddWithValue("@Event_Province", this.pEvent_Province);
            cmd.Parameters.AddWithValue("@Event_Postel_Code", this.pEvent_Postel_Code);
            cmd.Parameters.AddWithValue("@Event_Keywords", this.pEvent_Keywords);
            cmd.Parameters.AddWithValue("@Event_Registration_Required", this.pEvent_Registration_Required);

            cmd.Parameters.AddWithValue("@Is_Featured", this.pIs_Featured);
            cmd.Parameters.AddWithValue("@Is_Deleted", this.pIs_Deleted);
            cmd.Parameters.AddWithValue("@Is_Enabled", this.pIs_Enabled);
            cmd.Parameters.AddWithValue("@Created_By", this.pCreated_By);
            //cmd.Parameters.AddWithValue("@Date_Created", DateTime.Now.ToShortDateString());

            cmd.Parameters.AddWithValue("@approved", this.pEvent_Approved);
            cmd.Parameters.AddWithValue("@Event_Comments", this.pEvent_Comments);

            cmd.Parameters.AddWithValue("@BannerFile", this.pBannerFile);
            cmd.Parameters.AddWithValue("@BackgroundBannerPosition", this.pBackgroundBannerPosition);
            cmd.Parameters.AddWithValue("@BackgroundBannerPosition_Horizontal", this.pBackgroundBannerPosition_Horizontal);
            cmd.Parameters.AddWithValue("@ButtonText", this.pButtonText);
            cmd.Parameters.AddWithValue("@ButtonUrl", this.pButtonUrl);

            connection.Open();
            int i = Convert.ToInt32(cmd.ExecuteScalar());

            return i;
        }

    }


    public void mEdit(string elang)
    {
        sb = sb.Remove(0, sb.Length);
        sb.Append(" update tbl_Events ");
        sb.Append(" Set Event_Name = @Event_Name, ");
        sb.Append(" Event_Contact = @Event_Contact, ");
        sb.Append(" Group_id = @Group_id, ");
        sb.Append(" Event_Desc = @Event_Desc, ");
        sb.Append(" Event_Date_Start = @Event_Date_Start, ");
        sb.Append(" Event_Date_End = @Event_Date_End, ");
        sb.Append(" Event_Start_Time = @Event_Start_Time, ");
        sb.Append(" Event_End_Time = @Event_End_Time, ");

        sb.Append(" Event_Is_Multiple = @Event_Is_Multiple, ");
        sb.Append(" Event_Type = @Event_Type, ");
        sb.Append(" Event_Recur_Type = @Event_Recur_Type, ");
        sb.Append(" Event_Recur_Start_Date = @Event_Recur_Start_Date, ");
        sb.Append(" Event_Recur_End_Date = @Event_Recur_End_Date, ");

        sb.Append(" Event_Venue = @Event_Venue, ");
        sb.Append(" Event_Location_Detail = @Event_Location_Detail, ");
        //sb.Append(" Event_Fee = @Event_Fee, ");
        //sb.Append(" Event_Register_By = @Event_Register_By, ");
        sb.Append(" Event_Attach_File = @Event_Attach_File, ");
        sb.Append(" Event_Address = @Event_Address, ");
        sb.Append(" Event_City = @Event_City, ");
        sb.Append(" Event_Province = @Event_Province, ");
        sb.Append(" Event_Postel_Code = @Event_Postel_Code, ");
        sb.Append(" Event_Email = @Event_Email, ");
        sb.Append(" Event_Website = @Event_Website, ");
        sb.Append(" Event_Keywords = @Event_Keywords, ");
        sb.Append(" Event_Registration_Required = @Event_Registration_Required, ");
        sb.Append(" Is_Featured = @Is_Featured, ");

        sb.Append(" Approved = @approved, ");

        sb.Append(" Is_Enabled = @Is_Enabled, ");
        sb.Append(" Edited_By = @Edited_By, ");
        sb.Append(" Date_Edited = @Date_Edited, ");
		sb.Append(" Event_Comments = @Event_Comments, ");

        sb.Append(" BannerFile = isnull(@BannerFile, BannerFile), ");
        sb.Append(" BackgroundBannerPosition = @BackgroundBannerPosition, ");
        sb.Append(" BackgroundBannerPosition_Horizontal = @BackgroundBannerPosition_Horizontal, ");
        sb.Append(" ButtonText = @ButtonText, ");
        sb.Append(" ButtonUrl = @ButtonUrl, ");

        sb.Append(" language = @Lang ");

        sb.Append(" where Event_id = @Event_id ");
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"];
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@Event_id", this.pEvent_id);
            cmd.Parameters.AddWithValue("@Group_id", this.pGroup_id);
            cmd.Parameters.AddWithValue("@Event_Name", this.pEvent_Name);
            cmd.Parameters.AddWithValue("@Event_Contact", this.pEvent_Contact);
            cmd.Parameters.AddWithValue("@Event_Desc", this.pEvent_Desc);
            cmd.Parameters.AddWithValue("@Event_Date_Start", this.pEvent_Date_Start);
            cmd.Parameters.AddWithValue("@Event_Date_End", this.pEvent_Date_End);
            cmd.Parameters.AddWithValue("@Event_Start_Time", this.pEvent_Start_Time);
            cmd.Parameters.AddWithValue("@Event_End_Time", this.pEvent_End_Time);

            cmd.Parameters.AddWithValue("@Event_Is_Multiple", this.pEvent_Is_Multiple);
            cmd.Parameters.AddWithValue("@Event_Type", this.pEvent_Type);
            cmd.Parameters.AddWithValue("@Event_Recur_Type", this.pEvent_Recur_Type);
            cmd.Parameters.AddWithValue("@Event_Recur_Start_Date", this.pEvent_Recur_Start_Date);
            cmd.Parameters.AddWithValue("@Event_Recur_End_Date", this.pEvent_Recur_End_Date);

            cmd.Parameters.AddWithValue("@Event_Venue", this.pEvent_Venue);
            cmd.Parameters.AddWithValue("@Event_Location_Detail", this.pEvent_Location_Detail);
            //cmd.Parameters.AddWithValue("@Event_Fee", this.pEvent_Fee);
            //cmd.Parameters.AddWithValue("@Event_Register_By", this.pEvent_Register_By);
            cmd.Parameters.AddWithValue("@Event_Attach_File", this.pEvent_Attach_File);
            cmd.Parameters.AddWithValue("@Event_Address", this.pEvent_Address);
            cmd.Parameters.AddWithValue("@Event_City", this.pEvent_City);
            cmd.Parameters.AddWithValue("@Event_Province", this.pEvent_Province);
            cmd.Parameters.AddWithValue("@Event_Postel_Code", this.pEvent_Postel_Code);
            cmd.Parameters.AddWithValue("@Event_Email", this.pEvent_Email);
            cmd.Parameters.AddWithValue("@Event_Website", this.pEvent_Website);
            cmd.Parameters.AddWithValue("@Event_Keywords", this.pEvent_Keywords);
            cmd.Parameters.AddWithValue("@Event_Registration_Required", this.pEvent_Registration_Required);
            cmd.Parameters.AddWithValue("@Is_Featured", this.pIs_Featured);

            cmd.Parameters.AddWithValue("@Is_Enabled", this.pIs_Enabled);

            cmd.Parameters.AddWithValue("@Lang", elang);

            cmd.Parameters.AddWithValue("@Edited_By", this.pEdited_By);
            cmd.Parameters.AddWithValue("@Date_Edited", DateTime.Now.ToShortDateString());

            cmd.Parameters.AddWithValue("@approved", this.pEvent_Approved);
            cmd.Parameters.AddWithValue("@Event_Comments", this.pEvent_Comments);

            if(this.pBannerFile != "")
                cmd.Parameters.AddWithValue("@BannerFile", this.pBannerFile);
            else
                cmd.Parameters.AddWithValue("@BannerFile", DBNull.Value);

            cmd.Parameters.AddWithValue("@BackgroundBannerPosition", this.pBackgroundBannerPosition);
            cmd.Parameters.AddWithValue("@BackgroundBannerPosition_Horizontal", this.pBackgroundBannerPosition_Horizontal);
            cmd.Parameters.AddWithValue("@ButtonText", this.pButtonText);
            cmd.Parameters.AddWithValue("@ButtonUrl", this.pButtonUrl);


            connection.Open();
            cmd.ExecuteNonQuery();
        }

    }

    public void mDelete()
    {
        sb = sb.Remove(0, sb.Length);
        sb.Append(" update tbl_Events ");
        sb.Append(" Set Is_Deleted = 1 ");
        sb.Append(" where Event_id = @Event_id ");

        string strConnectionString = ConfigurationManager.AppSettings["CMServer"];
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@Event_id", this.pEvent_id);

            connection.Open();
            cmd.ExecuteNonQuery();
        }


    }

    #endregion
}
