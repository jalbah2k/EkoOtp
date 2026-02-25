using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class Admin_eForms_eFormDal : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public string _connection = ConfigurationManager.AppSettings.Get("CMServer");

    public DataTable getTable(string cmd)
    {
        //return getTable(cmd, null);
        return getTable(cmd, new List<SqlParameter>());
    }

    public string ProcessRecord(string sql)
    {
        return ProcessRecord(sql, null);
    }

    public DataTable getTable(string cmd, List<SqlParameter> prms)
    {
        return getTable(cmd, prms.ToArray());
    }
    public DataTable getTable(string cmd, SqlParameter[] prms)
    {
        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter(cmd, _connection);
        da.SelectCommand.CommandType = CommandType.StoredProcedure;
        if (prms != null && prms.Length > 0)
            da.SelectCommand.Parameters.AddRange(prms);
        da.Fill(dt);
        return dt;
    }

    public string ProcessRecord(string sql, SqlParameter[] prms)
    {
        SqlCommand cmd = new SqlCommand(sql, new SqlConnection(_connection));
        cmd.CommandType = CommandType.StoredProcedure;
        if (prms != null)
            cmd.Parameters.AddRange(prms);
        cmd.Connection.Open();
        string ret = Convert.ToString(cmd.ExecuteScalar());
        cmd.Connection.Close();
        return ret;
    }

    public void RemoveRecord(string sql, string rcrd)
    {
        SqlParameter[] prms = { new SqlParameter("@id", rcrd) };
        ProcessRecord(sql, prms);
    }


}
