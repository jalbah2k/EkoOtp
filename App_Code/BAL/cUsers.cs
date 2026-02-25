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
/// Summary description for cUsers
/// </summary>
public class cUsers 
{
    StringBuilder sb;
    public cUsers()
    {
        sb = new StringBuilder(60);

        
        this.puUserName = "";
        this.puPassword = "";
        this.puFirst_Name = "";
        this.puLast_Name = "";
        this.puEmail = "";
        this.puRoleID = -1;
        this.pIs_Deleted = 0;
        this.pIs_Enabled = 1;
        this.pDate_Created = DateTime.Now;
        this.pCreated_By = -1;
    }


    #region Variables

    private int uID;
    private string uUserName;
    private string uPassword;
    private string uFirst_Name;
    private string uLast_Name;
    private string uEmail;
    private int uRoleID;
    private int Is_Deleted;
    private int Is_Enabled;
    private DateTime Date_Created;
    private DateTime Date_Edited;
    private int Created_By;
    private int Edited_By;
    #endregion  
    

    #region Properties

    public int puID
    {
        set
        {
            this.uID = value;
        }
        get
        {
            return this.uID;
        }
    }

    public string puUserName
    {
        set
        {
            this.uUserName = value;
        }
        get
        {
            return this.uUserName;
        }
    }

    public string puPassword
    {
        set
        {
            this.uPassword = value;
        }
        get
        {
            return this.uPassword;
        }
    }
    public string puFirst_Name
    {
        set
        {
            this.uFirst_Name = value;
        }
        get
        {
            return this.uFirst_Name;
        }
    }
    public string puLast_Name
    {
        set
        {
            this.uLast_Name = value;
        }
        get
        {
            return this.uLast_Name;
        }
    }
    public string puEmail
    {
        set
        {
            this.uEmail = value;
        }
        get
        {
            return this.uEmail;
        }
    }
    public int puRoleID
    {
        set
        {
            this.uRoleID = value;
        }
        get
        {
            return this.uRoleID;
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
       string commandString = "select  * from tbl_User_Info where Is_deleted = 0";
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

    public DataTable mGet_One(int mUID)
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"];
        string commandString = "select * from tbl_User_Info where uID = @uid and Is_Deleted = 0";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@uid", mUID);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds, "table1");
        }

        return ds.Tables[0];  
       
    }

    public DataTable mCheck_Authentication(string mUser_Name, string mPassword)
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"];
        string commandString = "select  * from tbl_User_Info where uUserName = @User_Name and uPassword = @Password and Is_deleted = 0 and Is_Enabled = 1";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@User_Name", mUser_Name);
            cmd.Parameters.AddWithValue("@Password", mPassword);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds, "table1");
        }

        return ds.Tables[0];  

    }

    public Boolean mIsPassword_Exist(int mUID, string mPassword)
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"];
        string commandString = "select  * from tbl_User_Info where uID = @mUID and uPassword = @Password and Is_deleted = 0 ";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@mUID", mUID);
            cmd.Parameters.AddWithValue("@Password", mPassword);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds, "table1");
        }

        if (ds.Tables[0].Rows.Count == 1)
        {
            return false; //password correct
        }
        else
        {
            return true; // password incorrect
        }
        
        
    }

    public void mChange_Password(int mUID, string mPassword)
    {
        sb = sb.Remove(0, sb.Length);
        sb.Append(" update tbl_User_Info ");
        sb.Append(" Set uPassword = @Password ");
        sb.Append(" where uID = @mUID ");

        string strConnectionString = ConfigurationManager.AppSettings["CMServer"];
        string commandString = sb.ToString();
       

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@mUID", mUID);
            cmd.Parameters.AddWithValue("@Password", mPassword);    
            connection.Open();
            cmd.ExecuteNonQuery();    
        }
        
        
        
    }

   
    #endregion

}
