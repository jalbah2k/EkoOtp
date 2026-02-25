using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;

/// <summary>
/// Summary description for FTSAux
/// </summary>
public class FTSAux
{
    public static string RemoveNoiseWords(string searchTerm, string LCID)
    {
        //Do nothing if there is any quotations " or '
        string strReturn = "";

        if (searchTerm.IndexOf("\"") == -1 && searchTerm.IndexOf("'") == -1)
        {

            SqlParameter[] param;
            param = new SqlParameter[] { 
                new SqlParameter("@keywords", searchTerm),
                new SqlParameter("@lang", LCID)
                };

            DataTable tbKeywords = dal.getTable("RemoveNoiseWords", param);
            foreach (DataRow dr in tbKeywords.Rows)
            {
                strReturn += dr["item"] + " ";
            }
        }
        else
        {
            strReturn = searchTerm;
        }
        return strReturn.Trim();
    }

    public static string RemoveAllNoiseWords(string searchTerm, string LCID)
    {
        //Do nothing if there is any quotations " or '
        string strReturn = "";

        SqlParameter[] param;
        param = new SqlParameter[] { 
            new SqlParameter("@keywords", searchTerm),
            new SqlParameter("@lang", LCID)
            };

        DataTable tbKeywords = dal.getTable("RemoveNoiseWords", param);
        foreach (DataRow dr in tbKeywords.Rows)
        {
            strReturn += dr["item"] + " ";
        }

        return strReturn.Trim();
    } 
}
public class dal
{
    public dal() { }

    private static string _connection = System.Configuration.ConfigurationManager.AppSettings.Get("dbConnection");
    public static string GetConnection() { return _connection; }
    public static string ConnectionString { get { return _connection; } }

    #region getTables

    public static DataSet getTables(string cmd)
    {
        return getTables(cmd, new SqlParameter[] { });
    }

    public static DataSet getTables(string cmd, SqlParameter prm)
    {
        return getTables(cmd, new SqlParameter[] { prm });
    }
    public static DataSet getTables(string cmd, List<SqlParameter> prms)
    {
        return getTables(cmd, prms.ToArray());
    }

    public static DataSet getTables(string cmd, SqlParameter[] prms)
    {
        DataSet dt = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter(cmd, _connection);
        da.SelectCommand.CommandType = CommandType.StoredProcedure;
        da.SelectCommand.Parameters.AddRange(prms);
        da.Fill(dt);
        return dt;
    }
    #endregion

    #region getSTable
    public static DataTable getSTable(string cmd, SqlParameter[] prms)
    {
        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter(cmd, _connection);
        da.SelectCommand.Parameters.AddRange(prms);
        da.Fill(dt);
        return dt;
    }

    public static DataTable getSTable(string cmd)
    {
        return getTable(cmd, new SqlParameter[] { });
    }

    public static DataTable getSTable(string cmd, SqlParameter prm)
    {
        return getTable(cmd, new SqlParameter[] { prm });
    }

    public static DataTable getSTable(string cmd, string param, object val)
    {
        return getTable(cmd, new SqlParameter[] { new SqlParameter(param, val) });
    }

    public static DataTable getSTable(string cmd, List<SqlParameter> prms)
    {
        return getTable(cmd, prms.ToArray());
    }
    #endregion

    #region getTable
    public static DataTable getTable(string cmd, SqlParameter[] prms)
    {
        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter(cmd, _connection);
        da.SelectCommand.CommandType = CommandType.StoredProcedure;
        da.SelectCommand.Parameters.AddRange(prms);
        da.Fill(dt);
        return dt;
    }

    public static DataTable getTable(string cmd)
    {
        return getTable(cmd, new SqlParameter[] { });
    }

    public static DataTable getTable(string cmd, SqlParameter prm)
    {
        return getTable(cmd, new SqlParameter[] { prm });
    }

    public static DataTable getTable(string cmd, string param, object val)
    {
        return getTable(cmd, new SqlParameter[] { new SqlParameter(param, val) });
    }

    public static DataTable getTable(string cmd, List<SqlParameter> prms)
    {
        return getTable(cmd, prms.ToArray());
    }

    public static DataTable getTable(string cmd, string param, object val, CommandType ct)
    {
        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter(cmd, _connection);
        SqlParameter[] prms = new SqlParameter[] { new SqlParameter(param, val) };
        da.SelectCommand.CommandType = ct;
        da.SelectCommand.Parameters.AddRange(prms);
        da.Fill(dt);
        return dt;
    }
    #endregion

    #region Run Query
    public static int ExecuteNonQuery(string sql, SqlParameter[] prms, CommandType type)
    {
        SqlCommand cmd = new SqlCommand(sql, new SqlConnection(_connection));
        cmd.CommandType = type;
        cmd.Parameters.AddRange(prms);
        cmd.Connection.Open();
        int ret = cmd.ExecuteNonQuery();
        cmd.Connection.Close();
        return ret;
    }

    public static int ExecuteNonQuery(string sql, List<SqlParameter> prms, CommandType type)
    {
        return ExecuteNonQuery(sql, prms.ToArray(), type);
    }

    public static int ExecuteNonQuery(string sql, SqlParameter[] prms)
    {
        SqlCommand cmd = new SqlCommand(sql, new SqlConnection(_connection));
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddRange(prms);
        cmd.Connection.Open();
        int ret = cmd.ExecuteNonQuery();
        cmd.Connection.Close();
        return ret;
    }

    public static int ExecuteNonQuery(string sql, List<SqlParameter> prms)
    {
        return ExecuteNonQuery(sql, prms.ToArray());
    }

    public static int ExecuteNonQuery(string sql, SqlParameter prm)
    {
        return ExecuteNonQuery(sql, new SqlParameter[] { prm });
    }

    public static int ExecuteNonQuery(string sql, string param, object val)
    {
        return ExecuteNonQuery(sql, new SqlParameter[] { new SqlParameter(param, val) });
    }

    public static object ExecuteQuery(string sql, SqlParameter[] prms)
    {
        SqlCommand cmd = new SqlCommand(sql, new SqlConnection(_connection));
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddRange(prms);
        cmd.Connection.Open();
        object ret = cmd.ExecuteScalar();
        cmd.Connection.Close();
        return ret;
    }

    public static object ExecuteQuery(string sql, List<SqlParameter> prms)
    {
        return ExecuteQuery(sql, prms.ToArray());
    }

    public static object ExecuteQuery(string sql, SqlParameter prm)
    {
        return ExecuteQuery(sql, new SqlParameter[] { prm });
    }

    public static object ExecuteQuery(string sql, string param, object val)
    {
        return ExecuteQuery(sql, new SqlParameter[] { new SqlParameter(param, val) });
    }

    #endregion

    #region present dates
    public static string ThisDates(object Start, object End)
    {
        string ret = "";
        DateTime _start;
        DateTime _end;

        if (DateTime.TryParse(Convert.ToString(Start), out _start))
        {
            _start.ToString("MMM d, yyyy");

            if (DateTime.TryParse(Convert.ToString(End), out _end))
            {
                if (_end > _start)
                {
                    int yS = _start.Year;
                    int yE = _end.Year;
                    int mS = _start.Month;
                    int mE = _end.Month;

                    if (yS == yE && mS == mE)
                        ret = string.Format("{0: MMM} {0: d} - {1: d}, {0: yyyy}", _start, _end);
                    else if (yS == yE)
                        ret = string.Format("{0: MMM d} - {1: MMM d}, {0: yyyy}", _start, _end);
                    else
                        ret = string.Format("{0: MMM d, yyyy} - {1: MMM d, yyyy}", _start, _end);
                }
            }
        }
        return ret;
    }
    #endregion

    public static bool IsExists(string fld, string val, DataTable dt)
    {
        List<string> _val = new List<string>();
        foreach (DataRow rw in dt.Rows)
        {
            _val.Add(rw[fld].ToString());
        }

        return _val.Contains(val);
    }
}