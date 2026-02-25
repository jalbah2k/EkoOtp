using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Text;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for cEvent_Dynamic
/// </summary>
public class cEvent_Dynamic
{
	StringBuilder sb;
    public cEvent_Dynamic()
    {
        sb = new StringBuilder(60);

       
        this.pIs_Deleted = 0;
        this.pIs_Enabled = 1;
        this.pDate_Created = DateTime.Now;
        this.pCreated_By = -1;
    }

    #region Variables

    private int Event_Dynamic_id;
    private int Event_id;
    private string Event_Text_Label;
    private string Event_Text_Value;
    

    private int Is_Deleted;
    private int Is_Enabled;
    private DateTime Date_Created;
    private DateTime Date_Edited;
    private int Created_By;
    private int Edited_By;
    #endregion


    #region Properties

    public int pEvent_Dynamic_id
    {
        set
        {
            this.Event_Dynamic_id = value;
        }
        get
        {
            return this.Event_Dynamic_id;
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

    public string pEvent_Text_Label
    {
        set
        {
            this.Event_Text_Label = value;
        }
        get
        {
            return this.Event_Text_Label;
        }
    }

    public string pEvent_Text_Value
    {
        set
        {
            this.Event_Text_Value = value;
        }
        get
        {
            return this.Event_Text_Value;
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
        string commandString = "select  * from tbl_Event_Dynamic where Is_deleted = 0";
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
        string commandString = "select  * from tbl_Event_Dynamic where Is_deleted = 0 and Is_enabled = 1";
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
        string commandString = "select * from tbl_Event_Dynamic where Event_Dynamic_id = @mID and Is_Deleted = 0";
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

    public DataTable mGet_By_Eventid(int mID)
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"];
        string commandString = "select * from tbl_Event_Dynamic where Event_id = @mID and Is_Deleted = 0";
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
        sb.Append(" insert into  tbl_Event_Dynamic (Event_id, Event_Text_Label, Event_Text_Value, Is_Deleted, Is_Enabled, Created_By, Date_Created) ");
        sb.Append(" values   (@Event_id, @Event_Text_Label, @Event_Text_Value, @Is_Deleted, @Is_Enabled, @Created_By, @Date_Created)");
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"];
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@Event_id", this.pEvent_id);
            cmd.Parameters.AddWithValue("@Event_Text_Label", this.pEvent_Text_Label);
            cmd.Parameters.AddWithValue("@Event_Text_Value", this.pEvent_Text_Value);   

            cmd.Parameters.AddWithValue("@Is_Deleted", this.pIs_Deleted);
            cmd.Parameters.AddWithValue("@Is_Enabled", this.pIs_Enabled);
            cmd.Parameters.AddWithValue("@Created_By", this.pCreated_By);
            cmd.Parameters.AddWithValue("@Date_Created", DateTime.Now.ToShortDateString());


            connection.Open();
            cmd.ExecuteNonQuery();
        }

    }


    public void mEdit()
    {
        sb = sb.Remove(0, sb.Length);
        sb.Append(" update tbl_Event_Dynamic ");
        sb.Append(" Set Event_id = @Event_id, ");
        sb.Append("  Event_Text_Label = @Event_Text_Label, ");
        sb.Append("  Event_Text_Value = @Event_Text_Value, ");
        

        sb.Append(" Is_Enabled = @Is_Enabled, ");
        sb.Append(" Edited_By = @Edited_By, ");
        sb.Append(" Date_Edited = @Date_Edited, ");
        sb.Append(" where Event_Dynamic_id = @Event_Dynamic_id ");
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"];
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@Event_Dynamic_id", this.pEvent_Dynamic_id);
            cmd.Parameters.AddWithValue("@Event_id", this.pEvent_id);
            cmd.Parameters.AddWithValue("@Event_Text_Label", this.pEvent_Text_Label);
            cmd.Parameters.AddWithValue("@Event_Text_Value", this.pEvent_Text_Value);  

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
        sb.Append(" update tbl_Event_Dynamic ");
        sb.Append(" Set Is_Deleted = 1 ");
        sb.Append(" where Event_Dynamic_id = @Event_Dynamic_id ");

        string strConnectionString = ConfigurationManager.AppSettings["CMServer"];
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@Event_Dynamic_id", this.pEvent_Dynamic_id);

            connection.Open();
            cmd.ExecuteNonQuery();
        }


    }

    public void mDelete_ByEvent_id(int mID)
    {
        sb = sb.Remove(0, sb.Length);
        sb.Append(" Delete from tbl_Event_Dynamic ");
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
