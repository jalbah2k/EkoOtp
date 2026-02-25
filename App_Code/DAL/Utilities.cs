using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Utilities
{
    public class dal
    {
        public dal()
        {
        }
        private static string _connection = ConfigurationManager.AppSettings.Get("CMServer");

        public string GetConnectionString()
        {
            return _connection;
        }

        public static DataTable getTable(string cmd, SqlParameter[] prms, bool sp)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd, _connection);
            if(sp)
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Parameters.AddRange(prms);
            da.Fill(dt);
            return dt;
        }

        public static string ProcessRecord(string sql, SqlParameter[] prms)
        {
            SqlCommand cmd = new SqlCommand(sql, new SqlConnection(_connection));
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddRange(prms);
            cmd.Connection.Open();
            string ret = Convert.ToString(cmd.ExecuteScalar());
            cmd.Connection.Close();
            return ret;
        } 

        public static DataTable getTable(string cmd, SqlParameter[] prms)
        {
            return getTable(cmd, prms, true);
        }
       
        
        public static DataTable getTable(string cmd)
        {
            return getTable(cmd, new SqlParameter[] { });
        }

        public static DataTable getTable(string cmd, bool sp)
        {
            return getTable(cmd, new SqlParameter[] { }, sp);
        }


        public static DataTable getTable(string cmd, SqlParameter prm)
        {
            return getTable(cmd, new SqlParameter[] { prm });
        }

        public static DataTable getTable(string cmd, SqlParameter prm,bool sp)
        {
            return getTable(cmd, new SqlParameter[] { prm },sp);
        }

        public static DataTable getTable(string cmd, List<SqlParameter> prms)
        {
            return getTable(cmd, prms.ToArray());
        }

        public static DataTable getTable(string cmd, List<SqlParameter> prms,bool sp)
        {
            return getTable(cmd, prms.ToArray(), sp);
        }

        public static string ProcessRecord(string sql, List<SqlParameter> prms)
        {
            return ProcessRecord(sql, prms.ToArray());
        }

        public static string ProcessRecord(string sql, SqlParameter prm)
        {
            return ProcessRecord(sql, new SqlParameter[] { prm });
        }
    }
}
